using Library;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>능력 선택 카드 — 능력 아이콘·이름·설명을 표시하는 버튼</summary>
    public class Control_AbilityCard : ControlBase
    {
        #region Inspector
        [SerializeField, Tooltip("선택 버튼")] private UIWrapper_Button m_Button;
        [SerializeField, Tooltip("능력 아이콘")] private Image m_Icon;
        [SerializeField, Tooltip("능력 이름 라벨")] private UIWrapper_Text m_NameLabel;
        [SerializeField, Tooltip("능력 설명 라벨")] private UIWrapper_Text m_Desc;
        #endregion

        #region Event
        #endregion
        #region Function
        /// <summary>아이콘 _icon·이름 _name·설명 _desc 를 표시한다 (문구는 번역본)</summary>
        public void Set(Sprite _icon, string _name, string _desc)
        {
            m_Icon.sprite = _icon;
            UIWrapper_Text.Set(m_NameLabel, _name);
            UIWrapper_Text.Set(m_Desc, _desc);
        }
        /// <summary>선택 클릭 리스너 _onClick 을 등록한다</summary>
        public void AddClickListener(Action<UIWrapper_Button> _onClick)
        {
            m_Button.AddClickListener(_onClick);
        }
        #endregion
    }
}
