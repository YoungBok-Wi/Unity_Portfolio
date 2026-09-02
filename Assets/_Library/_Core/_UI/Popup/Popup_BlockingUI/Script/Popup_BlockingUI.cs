using UnityEngine;

namespace Library
{
    /// <summary>BoolFactor 기반으로 다중 요청을 관리하는 로딩/블로킹 UI 팝업</summary>
    public class Popup_BlockingUI : PopupBase
    {
        public static Popup_BlockingUI instance { get; private set; }

        #region Type
        public struct SOption
        {
            public MonoBehaviour factor;

            public SOption(MonoBehaviour _factor)
            {
                factor = _factor;
            }
        }
        #endregion
        #region Value
        private BoolFactor<MonoBehaviour> m_CloseFactor;
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
            m_CloseFactor = new(this, BoolFactor<MonoBehaviour>.ETotalType.Or);

            base.Init();
        }
        public override void OnOpen(object _option = null)
        {
            if (_option != null)
            {
                var option = (SOption)_option;
                m_CloseFactor.Set(this, option.factor, true);
            }

            base.OnOpen(_option);
        }
        public override void OnClose(object _option = null)
        {
            if (_option != null)
            {
                var option = (SOption)_option;
                m_CloseFactor.Remove(option.factor);
            }

            base.OnClose(_option);

            if (m_CloseFactor.Total)
                Open();
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        // 순수 입력차단 팝업. 현재 블로킹 유지 여부만 노출하고 조작은 기본(열림 여부)만 둔다
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            bool blocking = m_CloseFactor != null && m_CloseFactor.Total;
            _report.AddRaw("blocking", blocking ? "true" : "false");
        }
#endif
        #endregion
    }
}
