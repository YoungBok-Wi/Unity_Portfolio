using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>중단 시 띄우는 오류 화면. 매니저가 모두 멈춘 뒤에도 보여야 해서 매니저 체계 밖에 둔다</summary>
    public class ShutdownUI : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private TMP_Text m_Text;
        #endregion
        #region Function
        /// <summary>표시 문구를 _text 로 덮어쓴다</summary>
        public void SetText(string _text)
        {
            m_Text.text = _text;
        }
        /// <summary>기존 문구 뒤에 _text 를 덧붙인다</summary>
        public void AppendText(string _text)
        {
            m_Text.text += _text;
        }
        #endregion
    }
}
