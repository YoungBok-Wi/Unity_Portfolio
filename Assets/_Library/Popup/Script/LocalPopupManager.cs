using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Library
{
    /// <summary>팝업 관리 매니저, Open/Close/정렬/Cancel 입력 처리</summary>
    public class LocalPopupManager : LocalManagerBase
    {
        public static LocalPopupManager instance { get; private set; }
        #region Inspector
        [SerializeField, TabGroup("LocalPopupManager", "설정")] private Camera m_UICamera;
        [SerializeField, TabGroup("LocalPopupManager", "설정")] private int m_BottomFixedOrder = 50;
        [SerializeField, TabGroup("LocalPopupManager", "설정")] private bool m_IsCloseByCancel = true;
        #endregion
        #region Property
        /// <summary>팝업 Canvas 들이 물릴 UI 카메라</summary>
        public Camera UICamera => m_UICamera;
        /// <summary>등록된 모든 팝업 ID (팝업 오브젝트의 이름)</summary>
        public string[] IDs => m_PopupDic.Keys.ToArray();
        #endregion
        #region Value
        private Dictionary<string, PopupBase> m_PopupDic = new Dictionary<string, PopupBase>();
        private List<PopupBase> m_OpenList = new List<PopupBase>();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 이 매니저가 없는 씬에서 파괴된 인스턴스 접근을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        /// <summary>자가 등록한 오브젝트 중 팝업만 이름으로 등록한다</summary>
        public override void OnRegisterObject(ObjectBase _object)
        {
            base.OnRegisterObject(_object);
            if (_object is PopupBase popup)
                m_PopupDic.Add(popup.name, popup);
        }
        public override void Init()
        {
            LocalInputManager.instance.Create(this, "Cancel", LocalInputManager.EPriority.UI, OnInputCancel);
            base.Init();
        }
        public override void InitUIOnce()
        {
            base.InitUIOnce();
            LanguageManager.instance.Language.AddChanged(this, RefreshAllPopupLanguage);
            RefreshAllPopupLanguage();
            foreach (var v in m_PopupDic.Values)
            {
                if (v.IsDefaultOpen)
                {
                    v.OnOpen();
                    m_OpenList.Add(v);
                }
            }
            sort();
        }
        /// <summary>관리 중인 모든 팝업에 언어 변경을 알린다</summary>
        private void RefreshAllPopupLanguage(ValueBase _ = null)
        {
            foreach (var v in m_PopupDic.Values)
                v.OnLanguageChanged();
        }
        public override void OnShutdown()
        {
            if (EventSystem.current != null)
                EventSystem.current.enabled = false;
            foreach (var v in m_PopupDic.Values)
                v.enabled = false;
            base.OnShutdown();
        }

        /// <summary>Cancel 입력을 위에 열린 팝업부터 넘겨 보고, 아무도 처리하지 않으면 닫힌 등록 팝업에 넘긴 뒤 종료 팝업을 연다</summary>
        private bool OnInputCancel(InputAction.CallbackContext _context)
        {
            for (int i = m_OpenList.Count - 1; i >= 0; i--)
                if (m_OpenList[i].OnInputCancel(_context))
                    return true;

            // 닫힌 등록 팝업에도 넘긴다 — 닫힌 상태로 취소를 받는 씬별 소비 주체(포기 확인 등)의 override가 처리한다
            // performed 페이즈에서만 넘긴다 — Started에서 열리면 같은 키 입력의 Performed가 열린 팝업 닫기로 소비해 즉시 도로 닫힌다
            if (_context.performed)
                foreach (var v in m_PopupDic.Values)
                    if (!v.IsOpened && v.OnInputCancel(_context))
                        return true;

            // 종료 팝업은 이 씬에 살아 있을 때만 연다 — 다른 씬에서 파괴된 인스턴스 접근(MissingReferenceException)으로 취소 체인이 끊기는 것을 막는다
            if (_context.performed && m_IsCloseByCancel && Popup_Quit.instance != null)
                Popup_Quit.instance.Open();

            return false;
        }
        #endregion
        #region Local Function
        /// <summary>등록된 팝업을 꺼낸다 (미등록이면 예외)</summary>
        private PopupBase GetPopup(string _id)
        {
            if (!m_PopupDic.TryGetValue(_id, out var popup))
                throw new System.ArgumentException($"등록되지 않은 팝업 ID : {_id}", nameof(_id));
            return popup;
        }
        /// <summary>열린 팝업들에 정렬 순서를 다시 매긴다</summary>
        // 하단 고정(m_BottomFixedOrder 미만) 팝업 위에서 쌓이도록, 고정 순서 중 최댓값을 시작점으로 잡는다
        private void sort()
        {
            int startOrder = -1;
            foreach (var v in m_OpenList)
                if (startOrder < v.FixedOreder && v.FixedOreder < m_BottomFixedOrder)
                    startOrder = v.FixedOreder;

            for (int i = 0; i < m_OpenList.Count; i++)
                m_OpenList[i].OnSort(i + startOrder + 1);
        }
        #endregion
        #region Function
        /// <summary>_id 팝업을 연다. _option 은 팝업의 열기 리스너로 전달된다. 이미 열려 있으면 리스너만 다시 호출하고 정렬 순서는 유지한다. 미등록 _id 면 예외</summary>
        public void Open(string _id, object _option = null)
        {
            var popup = GetPopup(_id);
            // 닫기 연출 중 재열림이면 중단된 닫힘을 먼저 완결한다 — IsClosing 잔존(이후 Close 전부 무시)과 닫기 리스너 누락을 막는다
            popup.OnCancelClose();
            var isOpened = popup.IsOpened;
            popup.OnOpen(_option);

            if (isOpened)
                return;

            m_OpenList.Add(popup);
            sort();
        }
        /// <summary>_id 팝업이 이 씬에 등록되어 있으면 열고 true 를, 미등록이면 아무 일도 하지 않고 false 를 반환한다. 씬마다 등록 팝업이 달라 열기가 선택적인 호출부용이다</summary>
        public bool TryOpen(string _id, object _option = null)
        {
            if (Find(_id) == null)
                return false;
            Open(_id, _option);
            return true;
        }
        /// <summary>_id 팝업을 닫는다. _option 은 팝업의 닫기 리스너로 전달된다. 닫기 애니메이션이 있으면 실제 종료는 그 뒤이며, 이미 닫혀 있으면 무시한다</summary>
        public void Close(string _id, object _option = null)
        {
            var popup = GetPopup(_id);
            if (!popup.IsOpened)
                return;

            m_OpenList.Remove(popup);
            sort();

            popup.OnStartClose(_option);
        }
        /// <summary>_id 팝업의 표시 여부를 _isShow 로 바꾼다. 열림 상태와는 별개이며, 열려 있을 때만 실제로 보인다</summary>
        public void SetShow(string _id, bool _isShow)
        {
            GetPopup(_id).OnSetShow(_isShow);
        }
        /// <summary>_id 팝업을 반환한다. 미등록 _id 면 예외</summary>
        public PopupBase Get(string _id)
        {
            return GetPopup(_id);
        }
        /// <summary>_id 팝업을 반환한다. Get 과 달리 미등록 _id 면 예외 없이 null</summary>
        public PopupBase Find(string _id)
        {
            return m_PopupDic.TryGetValue(_id, out var popup) ? popup : null;
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        /// <summary>자신 대신 관리 중인 팝업들을 각각 최상위 항목으로 _entries 에 담는다</summary>
        public override void MCPCollect(System.Collections.Generic.List<SMCPEntry> _entries, string _name)
        {
            foreach (var pair in m_PopupDic)
            {
                var popup = pair.Value;
                var detail = new MCPReport();
                popup.MCPDetail(detail);
                var interaction = new MCPReport();
                // 가려진 팝업은 조작 목록을 비워 둔다. 닫힌 팝업은 열기만 노출한다
                if (popup.MCPUsable)
                {
                    if (popup.IsOpened)
                        popup.MCPInteraction(interaction);
                    else
                        interaction.Add("Open", "팝업 열기");
                }
                _entries.Add(new SMCPEntry(pair.Key, detail.ToJson(), interaction.ToJson()));
            }
        }
        /// <summary>_name 팝업을 찾는다. Get 과 달리 미등록이면 예외 없이 null</summary>
        public PopupBase MCPFindPopup(string _name)
        {
            return m_PopupDic.TryGetValue(_name, out var popup) ? popup : null;
        }
        /// <summary>관리 중인 모든 팝업</summary>
        public IEnumerable<PopupBase> MCPAllPopups()
        {
            return m_PopupDic.Values;
        }
#endif
        #endregion
    }
}
