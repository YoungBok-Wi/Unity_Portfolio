using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Library
{
    /// <summary>다국어 관리 매니저, ID→번역 텍스트 및 플레이스홀더 치환</summary>
    public class LanguageManager : GlobalManagerBase
    {
        public static LanguageManager instance { get; private set; }

        #region Preview
#if UNITY_EDITOR
        [Serializable] private struct SPreview
        {
            public string id;
            public string by;
            public string eng;
            public string kor;
            public SPreview(string _id, string _by, string _eng, string _kor)
            {
                id = _id;
                by = _by;
                eng = _eng;
                kor = _kor;
            }
        }
        [SerializeField, TabGroup("LanguageManager", "미리보기"), ReadOnly] private List<SPreview> m_Preview = new();
#endif
        #endregion
        #region Property
        /// <summary>현재 언어. PlayerPrefs 로 영속되며 기본값은 기기 언어다</summary>
        public IReadOnlyEnumValue<SystemLanguage> Language => m_Language;

        /// <summary>지원 언어 목록</summary>
        public IReadOnlyList<SystemLanguage> LanguageList => m_LanguageList;

        /// <summary>언어 → 번역 컬럼 인덱스</summary>
        public IReadOnlyDictionary<SystemLanguage, int> LanguageIndex => m_LanguageIndex;
        #endregion
        #region Value
        private EnumValue<SystemLanguage> m_Language;
        private List<SystemLanguage> m_LanguageList = LanguageConst.LanguageList;
        private Dictionary<SystemLanguage, int> m_LanguageIndex = LanguageConst.LanguageIndex;
        private Dictionary<string, TextData> m_Text = new Dictionary<string, TextData>();
        private Dictionary<string, ValueBase> m_Value = new Dictionary<string, ValueBase>();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void InitFirst()
        {
            m_Language = SaveUtil.Create(this, null, new EnumValue<SystemLanguage>(this, "lang_lng", Application.systemLanguage), SaveUtil.EType.PlayerPrefs);
            base.InitFirst();
        }
        #endregion
        #region Local Function
        /// <summary>테이블 값에서 TextData 를 꺼낸다 (값 자신이거나 Name 컬럼)</summary>
        private bool TryAsText(object _table, out TextData _text)
        {
            if (_table is TextData textData)
                _text = textData;
            else if (_table is ITableType iTable && iTable["Name"] is TextData name)
                _text = name;
            else
            {
                _text = null;
                return false;
            }
            return true;
        }
        #endregion
        #region Function
        /// <summary>현재 언어를 _language 로 바꾼다. 저장은 자동이며, 구독자들이 갱신 통지를 받는다</summary>
        public void SetLanguage(SystemLanguage _language)
        {
            m_Language.v = _language;
        }
        /// <summary>_id 로 번역 텍스트를 등록한다. _value 를 주면 그 값이 바뀔 때마다 _onChanged 로 문구를 다시 채우고, null 이면 _onChanged 를 등록 시 1회만 호출한다. _callBy 는 초기화 전이어야 한다</summary>
        public void Create(GlobalManagerBase _callBy, string _id, ValueBase _value, Action<ValueBase, TextData> _onChanged)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy), $"번역 텍스트 등록 호출자가 null : {_id}");
            if (_callBy.IsInited)
                throw new InvalidOperationException($"초기화가 끝난 매니저는 번역 텍스트를 등록할 수 없다 : {_callBy.name} / {_id}");

#if UNITY_EDITOR
            int index = m_Preview.Count;
            m_Preview.Add(new(_id, _callBy.name, "", ""));
#endif

            TextData t = new TextData("", _id, "", "");
            m_Text.Add(_id, t);
            if (_value != null)
            {
                m_Value.Add(_id, _value);
                _value.AddResourceChanged(this, (_value) =>
                {
                    _onChanged(_value, t);
#if UNITY_EDITOR
                    m_Preview[index] = new(_id, _callBy.name, t.Eng, t.Kor);
#endif
                }, true);
            }
            else
            {
                _onChanged(_value, t);
#if UNITY_EDITOR
                m_Preview[index] = new(_id, _callBy.name, t.Eng, t.Kor);
#endif
            }
        }
        /// <summary>_id 의 문구를 현재 언어로 반환한다. 등록·테이블 어디에도 없으면 _id 를 그대로 돌려준다</summary>
        public string Get(string _id, bool _isFormat = true, bool _isRich = false)
        {
            return Get(_id, _isFormat, _isRich, Language.v);
        }
        /// <summary>_id 의 문구를 _lang 으로 반환한다. _isFormat 이 true 면 {ID} 플레이스홀더까지 치환하고, _isRich 는 리치텍스트 표기를 켠다. 없으면 _id 를 그대로 돌려준다</summary>
        public string Get(string _id, bool _isFormat, bool _isRich, SystemLanguage _lang)
        {
            string msg = _id;

            if (m_Text.TryGetValue(_id, out var v) && !_id.Contains("."))
                msg = v.Translate(_lang, _isRich);
            else if (TableManager.instance.TryGet<object>(_id, out var table))
            {
                if (TryAsText(table, out var text))
                    msg = text.Translate(_lang, _isRich);
                else
                    msg = table.ToString();
            }

            if (_isFormat)
                return UseFormat(msg, _isRich, _lang);
            else
                return msg;
        }
        /// <summary>_id 에 연동된 TextData 를 반환한다. 테이블 행이면 Name 컬럼을 돌려주며, 없으면 null</summary>
        public TextData GetText(string _id)
        {
            if (m_Text.TryGetValue(_id, out var v) && !_id.Contains("."))
                return v;
            if (TableManager.instance.TryGet<object>(_id, out var table) && TryAsText(table, out var text))
                return text;
            return null;
        }
        /// <summary>_id 에 연동된 ValueBase 를 반환한다 (문구가 값에 따라 바뀔 때 구독용). 없으면 null</summary>
        public ValueBase GetValue(string _id)
        {
            if (m_Value.TryGetValue(_id, out var v))
                return v;
            return null;
        }
        /// <summary>_ids 각각의 ValueBase 를 같은 순서로 반환한다. 연동이 없는 항목은 null 로 남는다</summary>
        public ValueBase[] GetValues(string[] _ids)
        {
            ValueBase[] values = new ValueBase[_ids.Length];
            for (int i = 0; i < _ids.Length; i++)
                values[i] = GetValue(_ids[i]);

            return values;
        }
        /// <summary>_source 의 {ID} 플레이스홀더를 현재 언어 문구로 바꾼다</summary>
        public string UseFormat(string _source, bool _isRich = false)
        {
            return UseFormat(_source, _isRich, Language.v);
        }
        /// <summary>_source 의 {ID} 플레이스홀더를 _lang 문구로 바꾼다. 찾지 못한 자리는 {ID} 그대로 남는다</summary>
        public string UseFormat(string _source, bool _isRich, SystemLanguage _lang)
        {
            string result = Regex.Replace(_source, @"\{([^\}]+)\}", (_match) =>
            {
                string key = _match.Groups[1].Value;
                if (m_Text.TryGetValue(key, out var v))
                    return v.Translate(_lang, _isRich);
                else if (TableManager.instance.TryGet<object>(key, out var table) && TryAsText(table, out var text))
                    return text.Translate(_lang, _isRich);

                return _match.Value;
            });
            return result;
        }
        #endregion
    }
}