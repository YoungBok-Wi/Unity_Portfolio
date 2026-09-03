using Library;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>일시정지 프레임형 팝업 — 열리면 게임 시간을 멈추고 재개·설정·포기(로비로)를 제공한다. 취소 입력 소비 주체</summary>
    public class Popup_Pause : PopupBase
    {
        public static Popup_Pause instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("프레임 (제목)")] private Control_GameFrame m_Frame;
        [SerializeField, Tooltip("재개 버튼")] private UIWrapper_Button m_ResumeButton;
        [SerializeField, Tooltip("설정 버튼")] private UIWrapper_Button m_SettingButton;
        [SerializeField, Tooltip("포기 버튼 (로비로)")] private UIWrapper_Button m_GiveUpButton;
        [SerializeField, Tooltip("재개 버튼 라벨")] private UIWrapper_Text m_ResumeLabel;
        [SerializeField, Tooltip("설정 버튼 라벨")] private UIWrapper_Text m_SettingLabel;
        [SerializeField, Tooltip("포기 버튼 라벨")] private UIWrapper_Text m_GiveUpLabel;
        [SerializeField, Tooltip("취소 입력을 무시할 팝업 ID (열려 있으면 일시정지를 열지 않는다)")] private string[] m_BlockingPopups = { "Popup_RoomSelect", "Popup_Ability", "Popup_Result" };
        #endregion
        #region Value
        private const string SettingPopupId = "Popup_Setting";
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            m_ResumeButton.AddClickListener(_ => Close());
            m_SettingButton.AddClickListener(_ => LocalPopupManager.instance.TryOpen(SettingPopupId));
            m_GiveUpButton.AddClickListener(OnClickGiveUp);
            if (m_Frame != null)
                m_Frame.AddCloseListener(_ => Close());
            base.Init();
        }
        // 중복 진입 가드 — 이미 열려 있으면 시간 정지를 다시 걸지 않는다 (OnClose 와 짝)
        public override void OnOpen(object _option = null)
        {
            bool wasOpened = IsOpened;
            base.OnOpen(_option);
            if (wasOpened)
                return;
            SetPaused(true);
        }
        // 중복 진입 가드 — 열려 있지 않았으면 시간 복원을 하지 않는다 (OnOpen 과 짝)
        public override void OnClose(object _option = null)
        {
            bool wasOpened = IsOpened;
            base.OnClose(_option);
            if (!wasOpened)
                return;
            SetPaused(false);
        }
        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            if (m_Frame != null)
                m_Frame.SetTitle(LanguageManager.instance.Get("Text_Core_PauseTitle"));
            UIWrapper_Text.SetTextId(m_ResumeLabel, "Text_Core_Resume");
            UIWrapper_Text.SetTextId(m_SettingLabel, "Text_Popup_Setting");
            UIWrapper_Text.SetTextId(m_GiveUpLabel, "Text_Core_GiveUp");
        }
        // 닫힌 상태의 취소 입력으로 열린다 — 선택·결과 팝업이 떠 있으면 무시한다 (씬설정 "취소 입력")
        public override bool OnInputCancel(InputAction.CallbackContext _context)
        {
            if (IsOpened)
                return base.OnInputCancel(_context);
            if (!_context.performed || IsBlocked())
                return false;
            Open();
            return true;
        }
        /// <summary>포기 — 시간을 되돌리고 로비로 돌아간다</summary>
        private void OnClickGiveUp(UIWrapper_Button _)
        {
            SetPaused(false);
            if (LocalRoomManager.instance != null)
                LocalRoomManager.instance.ReturnLobby();
        }
        #endregion
        #region Local Function
        /// <summary>시간 정지를 전투 매니저(소유자)에 맡긴다. 매니저가 없으면 timeScale 을 직접 다룬다</summary>
        private void SetPaused(bool _isPaused)
        {
            if (LocalBattleManager.instance != null)
                LocalBattleManager.instance.SetPaused(_isPaused);
            else
                Time.timeScale = _isPaused ? 0 : 1;
        }
        /// <summary>취소 입력을 무시할 팝업이 열려 있는지</summary>
        private bool IsBlocked()
        {
            var manager = LocalPopupManager.instance;
            foreach (var id in m_BlockingPopups)
            {
                var popup = manager.Find(id);
                if (popup != null && popup.IsOpened)
                    return true;
            }
            return false;
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            _report.AddRaw("paused", LocalBattleManager.instance != null && LocalBattleManager.instance.IsPaused ? "true" : "false");
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (!IsOpened) return;
            _report.Add("Resume", "재개");
            _report.Add("Setting", "설정 열기");
            _report.Add("GiveUp", "포기 (로비로)");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "Resume": Close(); return "{\"success\":true}";
                case "Setting": LocalPopupManager.instance.TryOpen(SettingPopupId); return "{\"success\":true}";
                case "GiveUp": OnClickGiveUp(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
