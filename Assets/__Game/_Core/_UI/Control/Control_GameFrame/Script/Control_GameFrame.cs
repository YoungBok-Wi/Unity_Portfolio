using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>게임 화풍 팝업 프레임 — 제목 라벨·닫기 버튼·본문 슬롯(ControlRoot)을 노출한다</summary>
    public class Control_GameFrame : ControlBase
    {
        #region Inspector
        [SerializeField, Tooltip("제목 라벨")] private UIWrapper_Text m_Title;
        [SerializeField, Tooltip("닫기 버튼 (없으면 비활성 프레임)")] private UIWrapper_Button m_CloseButton;
        [SerializeField, Tooltip("본문 컨트롤을 두는 슬롯")] private RectTransform m_ControlRoot;
        #endregion
        #region Property
        /// <summary>본문 컨트롤 슬롯</summary>
        public RectTransform ControlRoot => m_ControlRoot;
        #endregion

        #region Event
        #endregion
        #region Function
        /// <summary>제목을 _title 로 바꾼다 (번역된 문구)</summary>
        public void SetTitle(string _title)
        {
            UIWrapper_Text.Set(m_Title, _title);
        }
        /// <summary>닫기 버튼 클릭 리스너 _onClose 를 등록한다. 닫기 버튼이 없으면 무시</summary>
        public void AddCloseListener(Action<UIWrapper_Button> _onClose)
        {
            if (m_CloseButton != null)
                m_CloseButton.AddClickListener(_onClose);
        }
        /// <summary>닫기 버튼 표시를 _isShow 로 바꾼다</summary>
        public void SetCloseVisible(bool _isShow)
        {
            if (m_CloseButton != null)
                m_CloseButton.gameObject.SetActive(_isShow);
        }
        #endregion
    }
}
