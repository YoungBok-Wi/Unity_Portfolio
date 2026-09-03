using Library;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>지나온 방 이력 항목 — 방 종류 아이콘 하나를 표시한다</summary>
    public class Control_RoomHistoryItem : ControlBase
    {
        #region Inspector
        [SerializeField, Tooltip("방 종류 아이콘")] private Image m_Icon;
        #endregion

        #region Event
        #endregion
        #region Function
        /// <summary>아이콘을 _icon 으로 바꾼다</summary>
        public void Set(Sprite _icon)
        {
            m_Icon.sprite = _icon;
        }
        #endregion
    }
}
