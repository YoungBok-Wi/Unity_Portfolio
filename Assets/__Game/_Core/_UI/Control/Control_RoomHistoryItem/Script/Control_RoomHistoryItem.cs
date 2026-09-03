using Library;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>지나온 방 이력 항목 — 방 종류 아이콘 하나와 앞 항목과 잇는 점선을 표시한다</summary>
    public class Control_RoomHistoryItem : ControlBase
    {
        #region Inspector
        [SerializeField, Tooltip("방 종류 아이콘")] private Image m_Icon;
        [SerializeField, Tooltip("앞 항목과 잇는 점선 (첫 항목은 끈다)")] private GameObject m_Link;
        #endregion

        #region Event
        #endregion
        #region Function
        /// <summary>아이콘을 _icon 으로 바꾸고 점선을 _showLink 로 켜고 끈다</summary>
        public void Set(Sprite _icon, bool _showLink)
        {
            m_Icon.sprite = _icon;
            if (m_Link != null)
                m_Link.SetActive(_showLink);
        }
        #endregion
    }
}
