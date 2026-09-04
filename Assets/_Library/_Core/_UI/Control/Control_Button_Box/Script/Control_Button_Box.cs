using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>Control_Button_Box 컨트롤 전용 UI 스크립트. 색상은 변형 프리팹으로 구분하고 비활성 시 공용 Disable 이미지로 전환</summary>
    [RequireComponent(typeof(UIWrapper_Button))]
    public class Control_Button_Box : ControlBase
    {
        #region Inspector
        [SerializeField] private Image m_Image;
        [SerializeField] private Sprite m_DisableSprite;
        #endregion
        #region Value
        private UIWrapper_Button m_Button;
        private Sprite m_NormalSprite;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            m_Button = GetComponent<UIWrapper_Button>();
            m_NormalSprite = m_Image.sprite;
            m_Button.AddInteractableChangedListener(OnInteractableChanged);
        }
        /// <summary>상호작용 가능 여부에 따라 변형 고유의 색상 이미지와 Disable 이미지를 전환</summary>
        private void OnInteractableChanged(UIWrapper_Button _, bool _interactable)
        {
            m_Image.sprite = _interactable ? m_NormalSprite : m_DisableSprite;
        }
        #endregion
    }
}
