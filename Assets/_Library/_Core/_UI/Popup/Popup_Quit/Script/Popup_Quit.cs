using UnityEngine;

namespace Library
{
    /// <summary>앱 종료 확인 팝업. 확인 시 종료, 취소 시 팝업 닫기</summary>
    public class Popup_Quit : PopupBase
    {
        public static Popup_Quit instance { get; private set; }

        #region Inspector
        [SerializeField] private UIWrapper_Button m_ButtonYes;
        [SerializeField] private UIWrapper_Button m_ButtonNo;
        [SerializeField] private UIWrapper_Text m_ButtonYesLabel;
        [SerializeField] private UIWrapper_Text m_ButtonNoLabel;
        [SerializeField] private UIWrapper_Text m_Title;
        [SerializeField] private UIWrapper_Text m_Text;
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
        public override void InitUIOnce()
        {
            m_ButtonYes.AddClickListener(OnClickQuit);
            m_ButtonNo.AddClickListener(OnClickClose);

            base.InitUIOnce();
        }
        // 번역 애드온 유실 대비: 제목·본문·확인(예)·취소(아니오) 버튼 라벨을 팝업이 직접 현재 언어로 갱신한다
        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            m_ButtonYesLabel.SetTextId("Text_Core_Yes");
            m_ButtonNoLabel.SetTextId("Text_Core_No");
            m_Title.SetTextId("Text_Quit_Title");
            m_Text.SetTextId("Text_Quit_Text");
        }

        /// <summary>클릭 시 에디터 또는 빌드 환경에 맞게 종료 처리</summary>
        private void OnClickQuit(UIWrapper_Button _)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        /// <summary>클릭 시 팝업을 닫기</summary>
        private void OnClickClose(UIWrapper_Button _)
        {
            LocalPopupManager.instance.Close(name);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        // 유지 상태 없는 확인 팝업. Confirm=종료, Cancel=닫기
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            _report.Add("Confirm", "종료 확정");
            _report.Add("Cancel", "종료 취소(닫기)");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "Confirm": OnClickQuit(null); return "{\"success\":true}";
                case "Cancel": OnClickClose(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
