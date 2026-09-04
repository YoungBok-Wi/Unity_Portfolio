using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>Control_Button_Icon 컨트롤 전용 UI 스크립트. 비활성 시 아이콘을 어둡게 틴트하고 활성 복귀 시 원래 컬러로 되돌린다</summary>
    [RequireComponent(typeof(UIWrapper_Button))]
    public class Control_Button_Icon : ControlBase
    {
        #region Inspector
        [SerializeField] private Image m_Image;
        [SerializeField] private Color m_DisableColor = new Color(0f, 0f, 0f, 0.25f);
        #endregion
        #region Value
        private UIWrapper_Button m_Button;
        private Color m_NormalColor;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            m_Button = GetComponent<UIWrapper_Button>();
            m_NormalColor = m_Image.color;
            m_Button.AddInteractableChangedListener(OnInteractableChanged);
        }
        /// <summary>상호작용 가능 여부에 따라 사용처별 원본 컬러와 비활성 틴트 컬러를 전환</summary>
        private void OnInteractableChanged(UIWrapper_Button _, bool _interactable)
        {
            m_Image.color = _interactable ? m_NormalColor : m_DisableColor;
        }
        #endregion
    }
}
