using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>텍스트 라벨을 가진 범용 버튼 컨트롤. 클릭 동작은 이 컨트롤을 사용하는 팝업이 UIWrapper_Button에 직접 결선한다. 비활성 시 라벨을 어둡게 틴트하고 활성 복귀 시 원래 컬러로 되돌린다</summary>
    [RequireComponent(typeof(UIWrapper_Button))]
    public class Control_Button_Text : ControlBase
    {
        #region Inspector
        [SerializeField] private TextMeshProUGUI m_Text;
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
            m_NormalColor = m_Text.color;
            m_Button.AddInteractableChangedListener(OnInteractableChanged);
        }
        /// <summary>상호작용 가능 여부에 따라 사용처별 원본 컬러와 비활성 틴트 컬러를 전환</summary>
        private void OnInteractableChanged(UIWrapper_Button _, bool _interactable)
        {
            m_Text.color = _interactable ? m_NormalColor : m_DisableColor;
        }
        #endregion
    }
}
