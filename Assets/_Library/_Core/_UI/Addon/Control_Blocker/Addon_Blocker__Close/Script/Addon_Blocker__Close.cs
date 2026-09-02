using UnityEngine;

namespace Library
{
    /// <summary>클릭 시 부모 팝업을 닫는 애드온</summary>
    [RequireComponent(typeof(UIWrapper_Button))]
    public class Addon_Blocker__Close : ControlBase
    {
        #region Value
        private UIWrapper_Button m_Button;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            m_Button = GetComponent<UIWrapper_Button>();
            m_Button.AddClickListener(OnClick);
        }
        /// <summary>클릭 시 부모 팝업을 PopupManager를 통해 닫기</summary>
        private void OnClick(UIWrapper_Button _)
        {
            var popup = GetComponentInParent<PopupBase>();
            LocalPopupManager.instance.Close(popup.name);
        }
        #endregion
    }
}
