using UnityEngine;

namespace Library
{
    /// <summary>상위 Control_Button_DealFrame의 딜 Key로 이름 자동 표시</summary>
    [RequireComponent(typeof(UIWrapper_Text))]
    public class Addon_Button_DealFrame__BotNameText : ControlBase
    {
        #region Value
        private UIWrapper_Text m_Text;
        private Control_Button_DealFrame m_DealFrame;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            m_Text = GetComponent<UIWrapper_Text>();
            m_DealFrame = GetComponentInParent<Control_Button_DealFrame>();
        }
        public override void InitUIOnce()
        {
            base.InitUIOnce();
            m_DealFrame.DealValue.AddChanged(this, OnSetDeal);
        }

        private void OnSetDeal(ValueBase _)
        {
            m_Text.Set(LanguageManager.instance.Get(m_DealFrame.DealValue.v.Key));
        }
        #endregion
    }
}
