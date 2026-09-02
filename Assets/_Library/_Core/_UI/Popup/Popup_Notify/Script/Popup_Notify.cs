using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>확인/취소 버튼을 제공하며 옵션 큐와 비동기 대기를 지원하는 알림 팝업</summary>
    public class Popup_Notify : PopupBase
    {
        public static Popup_Notify instance { get; private set; }
        #region Type
        public struct SOption
        {
            public string title;
            public string text;
            public string btn1Text;
            public Action btn1Event;
            public string btn2Text;
            public Action btn2Event;

            public SOption(string _title, string _text, string _btn1Text, Action _btn1Event, string _btn2Text = null, Action _btn2Event = null)
            {
                title = _title;
                text = _text;
                btn1Text = _btn1Text;
                btn1Event = _btn1Event;
                btn2Text = _btn2Text;
                btn2Event = _btn2Event;
            }
        }
        public struct SOptionAwait
        {
            public string title;
            public string text;
            public string btn1Text;
            public string btn2Text;
            public SOptionAwait(string _title, string _text, string _btn1Text, string _btn2Text = null)
            {
                title = _title;
                text = _text;
                btn1Text = _btn1Text;
                btn2Text = _btn2Text;
            }
        }
        #endregion
        #region Inspector
        [SerializeField] private TMP_Text m_Title;
        [SerializeField] private TMP_Text m_Text;
        [SerializeField] private UIWrapper_Button m_Btn1;
        [SerializeField] private TMP_Text m_Btn1Text;
        [SerializeField] private UIWrapper_Button m_Btn2;
        [SerializeField] private TMP_Text m_Btn2Text;
        #endregion
        #region Value
        private SOption m_Option;
        private Queue<SOption> m_OptionQueue = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 이 팝업이 없는 씬에서 파괴된 인스턴스 접근을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            m_Btn1.AddClickListener(OnClickBtn1);
            m_Btn2.AddClickListener(OnClickBtn2);
            base.Init();
        }

        public override void OnOpen(object _option = null)
        {
            bool isAlreadyOpen = IsOpened & !IsClosing;
            base.OnOpen(_option);
            if (_option != null)
                m_OptionQueue.Enqueue((SOption)_option);
            if (isAlreadyOpen)
                return;

            SOption option = m_OptionQueue.Dequeue();
            m_Option = option;
            // title 미지정(null) 시 기본 알림 제목 사용
            m_Title.text = option.title ?? LanguageManager.instance.Get("Text_Popup_NotifyTitle");
            m_Text.text = option.text;

            if (option.btn1Text != null)
            {
                m_Btn1.gameObject.SetActive(true);
                m_Btn1Text.text = option.btn1Text;
            }
            else
                m_Btn1.gameObject.SetActive(false);

            if (option.btn2Text != null)
            {
                m_Btn2.gameObject.SetActive(true);
                m_Btn2Text.text = option.btn2Text;
            }
            else
                m_Btn2.gameObject.SetActive(false);
        }
        public override void OnClose(object _option = null)
        {
            base.OnClose(_option);

            m_Option = default;
            if (0 < m_OptionQueue.Count)
                Open();
        }
        /// <summary>첫 번째 버튼 클릭 시 이벤트 호출 후 팝업 닫기</summary>
        private void OnClickBtn1(UIWrapper_Button _)
        {
            m_Option.btn1Event?.Invoke();
            Close();
        }
        /// <summary>두 번째 버튼 클릭 시 이벤트 호출 후 팝업 닫기</summary>
        private void OnClickBtn2(UIWrapper_Button _)
        {
            m_Option.btn2Event?.Invoke();
            Close();
        }
        #endregion
        #region Function
        /// <summary>팝업을 열고 버튼 클릭을 비동기로 대기하여 결과 반환</summary>
        public async Awaitable<int> OpenAsync(SOptionAwait _option)
        {
            int result = 0;
            Open(new SOption(_option.title, _option.text, _option.btn1Text, () =>
            {
                // 첫번째 버튼 클릭시
                result = 1;
            }, _option.btn2Text, () =>
            {
                // 두번째 버튼 클릭시
                result = 2;
            }));

            //버튼이 눌러질 때 까지 대기 후 리턴
            while (result == 0)
                await Awaitable.NextFrameAsync();
            return result;
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            _report.Add("title", m_Title != null ? m_Title.text : "");
            _report.Add("text", m_Text != null ? m_Text.text : "");
        }
        // 실제로 표시 중인 버튼만 조작으로 노출한다 (Btn1=Confirm, Btn2=Cancel)
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (m_Btn1 != null && m_Btn1.gameObject.activeSelf)
                _report.Add("Confirm", "확인 버튼");
            if (m_Btn2 != null && m_Btn2.gameObject.activeSelf)
                _report.Add("Cancel", "취소 버튼");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "Confirm": OnClickBtn1(null); return "{\"success\":true}";
                case "Cancel": OnClickBtn2(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}