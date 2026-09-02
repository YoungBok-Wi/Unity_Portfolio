using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>교환 딜의 아이콘, 수량, 액션 표시 버튼</summary>
    [RequireComponent(typeof(UIWrapper_Button))]
    public class Control_Button_DealFrame : ControlBase
    {
        #region Inspector
        [SerializeField, TabGroup("Control_Button_DealFrame", "UI")] private Image m_Icon;
        [SerializeField, TabGroup("Control_Button_DealFrame", "UI")] private TMP_Text m_CountText;
        [SerializeField, TabGroup("Control_Button_DealFrame", "설정")] private bool m_IsUnit;
        [SerializeField, TabGroup("Control_Button_DealFrame", "설정")] private SDeal m_Deal;
        #endregion
        #region Property
        /// <summary>현재 딜의 반응형 값</summary>
        public StructValue<SDeal> DealValue { get; private set; }
        #endregion
        #region Value
        private UIWrapper_Button m_Button;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            DealValue = new StructValue<SDeal>(null, "", m_Deal);

            m_Button = GetComponent<UIWrapper_Button>();
            m_Button.AddClickListener(OnClick);
        }

        private void OnClick(UIWrapper_Button _)
        {
        }
        #endregion
        #region Function
        /// <summary>딜 정보로 아이콘/수량/액션 설정</summary>
        public void Set(SDeal _deal)
        {
            m_Deal = _deal;
            m_Icon.sprite = IconManager.instance.Get(_deal.Key);
            m_CountText.text = $"x{_deal.CountLong.ToStringLong(m_IsUnit)}";
            DealValue.Set(_deal, true, false);
        }
        #endregion
    }
}
