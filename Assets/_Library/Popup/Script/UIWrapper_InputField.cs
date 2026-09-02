using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>TMP_InputField 를 팝업 컨트롤로 감싼 래퍼. 팝업을 열 때 내용을 비우는 옵션과 비밀번호 토글을 제공한다</summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class UIWrapper_InputField : ControlBase
    {
        #region Inspector
        [SerializeField] private bool m_ResetOnOpen = false;
        #endregion
        #region Property
        /// <summary>감싸고 있는 TMP_InputField</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public TMP_InputField v
        {
            get
            {
                if (m_InputField == null)
                    m_InputField = GetComponent<TMP_InputField>();
                return m_InputField;
            }
        }
        #endregion
        #region Value
        private TMP_InputField m_InputField;
        #endregion

        #region Event
        public override void OnOpen()
        {
            base.OnOpen();
            if (m_ResetOnOpen)
                v.text = string.Empty;
        }
        #endregion
        #region Function
        /// <summary>입력 내용을 가릴지 여부를 뒤집는다</summary>
        public void TogglePassword()
        {
            v.contentType = (v.contentType == TMP_InputField.ContentType.Standard) ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard;
            v.ForceLabelUpdate();
        }
        #endregion
    }
}
