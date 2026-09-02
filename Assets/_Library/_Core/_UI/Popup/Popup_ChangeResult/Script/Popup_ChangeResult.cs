using UnityEngine;

namespace Library
{
    /// <summary>교환 결과 아이템 목록 표시 팝업</summary>
    public class Popup_ChangeResult : PopupBase
    {
        public static Popup_ChangeResult instance { get; private set; }

        #region Type
        /// <summary>팝업 옵션 데이터</summary>
        public struct SOption
        {
            public SDeal[] deals;
            public int startIndex;

            public SOption(SDeal[] _deals)
            {
                deals = _deals;
                startIndex = 0;
            }
        }
        #endregion
        #region Inspector
        [SerializeField] private GameObject m_ChangeIcon;
        [SerializeField] private int m_IconCount;
        [SerializeField] private UIWrapper_Button m_ConfirmButton;
        [SerializeField] private UIWrapper_Text m_ConfirmButtonLabel;
        #endregion
        #region Value
        private SOption m_Option;
        private ObjectPool m_IconPool;
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
            m_IconPool = new ObjectPool(m_ChangeIcon, m_ChangeIcon.transform.parent, m_IconCount);
            base.Init();
        }
        public override void InitUIOnce()
        {
            m_ConfirmButton.AddClickListener(OnClickConfirm);
            base.InitUIOnce();
        }
        // 번역 애드온 유실 대비: 확인 버튼 라벨을 팝업이 직접 현재 언어로 갱신한다
        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            m_ConfirmButtonLabel.SetTextId("Text_Popup_Confirm");
        }

        /// <summary>확인 버튼 클릭 시 팝업을 닫기</summary>
        private void OnClickConfirm(UIWrapper_Button _)
        {
            LocalPopupManager.instance.Close(name);
        }

        public override void OnOpen(object _option = null)
        {
            m_IconPool.Clear();

            if (_option != null)
            {
                m_Option = (SOption)_option;

                int lange = Mathf.Min(m_Option.startIndex + m_IconCount, m_Option.deals.Length);
                for (int i = m_Option.startIndex; i < lange; i++)
                {
                    var control = m_IconPool.Get().GetComponent<Control_Button_DealFrame>();
                    control.Set(m_Option.deals[i]);
                }
            }

            base.OnOpen(_option);
        }

        public override void OnClose(object _option = null)
        {
            int nextStartIndex = m_Option.startIndex + m_IconCount;
            if (nextStartIndex < m_Option.deals.Length)
            {
                m_Option.startIndex = nextStartIndex;
                Open(m_Option);
            }

            base.OnClose(_option);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            int dealCount = m_Option.deals != null ? m_Option.deals.Length : 0;
            _report.AddNumber("startIndex", m_Option.startIndex);
            _report.AddNumber("dealCount", dealCount);
            _report.AddNumber("pageSize", m_IconCount);
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            _report.Add("Confirm", "결과 확인");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "Confirm": OnClickConfirm(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}