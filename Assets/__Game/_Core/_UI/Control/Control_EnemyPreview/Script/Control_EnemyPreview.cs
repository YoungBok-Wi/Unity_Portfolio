using Library;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>적 구성 미리보기 항목 — 적 아이콘과 마릿수를 표시한다</summary>
    public class Control_EnemyPreview : ControlBase
    {
        #region Inspector
        [SerializeField, Tooltip("적 아이콘")] private Image m_Icon;
        [SerializeField, Tooltip("마릿수 라벨")] private UIWrapper_Text m_Count;
        #endregion

        #region Event
        #endregion
        #region Function
        /// <summary>아이콘 _icon 과 마릿수 _count 를 표시한다</summary>
        public void Set(Sprite _icon, int _count)
        {
            m_Icon.sprite = _icon;
            UIWrapper_Text.Set(m_Count, $"x{_count}");
        }
        #endregion
    }
}
