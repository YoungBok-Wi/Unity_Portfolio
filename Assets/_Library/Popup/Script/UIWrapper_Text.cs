using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>TMP_Text 를 팝업 컨트롤로 감싼 래퍼</summary>
    [RequireComponent(typeof(TMP_Text))]
    public class UIWrapper_Text : ControlBase
    {
        #region Property
        /// <summary>감싸고 있는 TMP_Text</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public TMP_Text v
        {
            get
            {
                if (m_Text == null)
                    m_Text = GetComponent<TMP_Text>();
                return m_Text;
            }
        }
        #endregion
        #region Value
        private TMP_Text m_Text;
        #endregion
        #region Function
        /// <summary>표시 문구를 _text 로 바꾼다. 번역은 하지 않으므로 이미 번역된 문구를 넘겨야 한다. 비활성 상태에서도 동작한다</summary>
        public void Set(string _text)
        {
            v.text = _text;
        }
        /// <summary>_textId 를 현재 언어로 번역해 표시한다. LanguageManager 가 아직 없으면 아무 일도 하지 않으며, 등록되지 않은 ID 는 그대로 표시된다</summary>
        public void SetTextId(string _textId)
        {
            if (LanguageManager.instance == null)
                return;
            v.text = LanguageManager.instance.Get(_textId);
        }
        /// <summary>_label 이 배선되어 있으면 _text 를 넣고, null 이면 아무 일도 하지 않는다 (소비처가 미배선 라벨을 각자 막지 않게 하는 통로)</summary>
        public static void Set(UIWrapper_Text _label, string _text)
        {
            if (_label == null)
                return;
            _label.Set(_text);
        }
        /// <summary>_label 이 배선되어 있으면 _textId 의 번역 문구를 넣고, null 이면 아무 일도 하지 않는다</summary>
        public static void SetTextId(UIWrapper_Text _label, string _textId)
        {
            if (_label == null)
                return;
            _label.SetTextId(_textId);
        }
        #endregion
    }
}
