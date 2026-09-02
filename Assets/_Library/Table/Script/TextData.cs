using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>다국어(한/영/일) 텍스트 데이터를 저장하는 테이블 타입</summary>
    public partial class TextData : TableType
    {
        #region Property
        /// <summary>한국어 문구. 미번역이면 null</summary>
        public string Kor { get; private set; }
        /// <summary>영어 문구. 다른 언어가 없을 때의 폴백이라 사실상 필수다</summary>
        public string Eng { get; private set; }
        /// <summary>일본어 문구. 미번역이면 null</summary>
        public string Jap { get; private set; }
        /// <summary>문구를 감쌀 RichText 포맷("{0}" 자리에 문구가 들어간다). 없으면 null</summary>
        public string Rich { get; private set; }
        #endregion

        #region Event
        /// <summary>테이블 행 _dic 에서 "{_addID}Kor"·"{_addID}Eng" 같은 컬럼을 찾아 만든다. 없는 언어는 null 로 남는다</summary>
        public TextData(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
        {
            Table = _table;
            ID = $"{_baseID}.{_addID}";
            string o = null;
            if (_dic.TryGetValue(_addID + "Kor", out o))
                Kor = o;
            m_Data.Add("Kor", Kor);
            if (_dic.TryGetValue(_addID + "Eng", out o))
                Eng = o;
            m_Data.Add("Eng", Eng);
            if (_dic.TryGetValue(_addID + "Jap", out o))
                Jap = o;
            m_Data.Add("Jap", Jap);
            if (_dic.TryGetValue(_addID + "Rich", out o))
                Rich = o;
            m_Data.Add("Rich", Rich);
        }
        /// <summary>문구를 직접 지정해 만든다. _eng 는 폴백으로 쓰이므로 생략할 수 없다</summary>
        public TextData(string _table, string _baseID, string _addID, string _eng, string _kor = null, string _jap = null, string _rich = null)
        {
            Table = _table;
            ID = $"{_baseID}.{_addID}";

            Kor = _kor;
            m_Data.Add("Kor", Kor);
            Eng = _eng;
            m_Data.Add("Eng", Eng);
            Jap = _jap;
            m_Data.Add("Jap", Jap);
            Rich = _rich;
            m_Data.Add("Rich", Rich);
        }
        #endregion
        #region Function
        /// <summary>_lang 문구를 반환한다. 그 언어가 없으면 영어로, 영어도 없으면 ID 로 폴백한다. _isRich 는 현재 쓰이지 않으며 리치 적용은 UseRich 를 직접 호출해야 한다</summary>
        public string Translate(SystemLanguage _lang, bool _isRich)
        {
            switch (_lang)
            {
                case SystemLanguage.Korean:
                    if (Kor != null)
                        return Kor;
                    break;
                case SystemLanguage.English:
                    if (Eng != null)
                        return Eng;
                    break;
                case SystemLanguage.Japanese:
                    if (Jap != null)
                        return Jap;
                    break;
            }
            return Eng != null ? Eng : ID;
        }
        /// <summary>_text 를 Rich 포맷으로 감싸 반환한다. 포맷이 없으면 _text 를 그대로 돌려준다</summary>
        public string UseRich(string _text)
        {
            if (Rich != null)
                return string.Format(Rich, _text);
            else
                return _text;
        }
        /// <summary>_table 의 문구들을 자신에게 덮어쓴다 (Table·ID 는 그대로 둔다)</summary>
        public void Copy(TextData _table)
        {
            Kor = _table.Kor;
            m_Data["Kor"] = Kor;

            Eng = _table.Eng;
            m_Data["Eng"] = Eng;

            Jap = _table.Jap;
            m_Data["Jap"] = Jap;

            Rich = _table.Rich;
            m_Data["Rich"] = Rich;
        }

        /// <summary>모든 문구를 비우고 자신을 반환한다 (이후 Translate 는 ID 를 돌려준다)</summary>
        public TextData Clear()
        {
            Kor = null;
            Eng = null;
            Jap = null;
            Rich = null;
            return this;
        }
        /// <summary>영어 문구를 _eng 로 바꾼 자신을 반환한다 (Clear().SetEng(...) 형태로 이어 쓴다)</summary>
        public TextData SetEng(string _eng)
        {
            Eng = _eng;
            return this;
        }
        #endregion
    }
}
