using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>Image 의 채움 정도로 게이지를 표시하는 컨트롤. Image 의 Type 이 Filled 여야 보인다</summary>
    [RequireComponent(typeof(Image))]
    public class UIWrapper_Guage : ControlBase
    {
        #region Property
        /// <summary>감싸고 있는 Image</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public Image v
        {
            get
            {
                if (m_Image == null)
                    m_Image = GetComponent<Image>();
                return m_Image;
            }
        }
        #endregion
        #region Value
        private Image m_Image;
        #endregion
        #region Function
        /// <summary>게이지를 _value 만큼 채운다. 0~1 범위로 잘리므로 범위 밖 값을 넘겨도 안전하다</summary>
        public void Set(float _value)
        {
            v.fillAmount = Mathf.Clamp01(_value);
        }
        #endregion
    }
}
