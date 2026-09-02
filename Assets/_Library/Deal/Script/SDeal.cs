using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Library
{
    /// <summary>거래 데이터 구조체, Key/Action/Count 포함</summary>
    [Serializable] public struct SDeal : ITableType
    {
        #region Property
        /// <summary>소속 테이블. 간편 생성자로 만들면 null</summary>
        public string Table { get; private set; }
        /// <summary>"{baseID}.{addID}" 형식의 고유 ID. 간편 생성자로 만들면 null</summary>
        public string ID { get; private set; }
        /// <summary>테이블 원본 컬럼을 _id 로 읽는다. 없거나 간편 생성자로 만들었으면 null</summary>
        public object this[string _id]
        {
            get
            {
                if (m_Data.TryGetValue(_id, out var data))
                    return data;

                return null;
            }
        }
        /// <summary>수량을 int 로 (소수점 버림)</summary>
        public int CountInt => (int)Count;
        /// <summary>수량을 long 으로 (소수점 버림)</summary>
        public long CountLong => (long)Count;
        /// <summary>수량을 float 로</summary>
        public float CountFloat => (float)Count;
        #endregion
        #region Value
        /// <summary>거래 대상 키 (재화·아이템 ID 등)</summary>
        public string Key;
        /// <summary>거래 행동 타입. 선택이며, "Not" 이 들어 있으면 조건 판정이 반전된다</summary>
        public string Action;
        /// <summary>거래 수량</summary>
        public double Count;
        private Dictionary<string, object> m_Data;
        #endregion

        #region Event
        /// <summary>접두어 없는 "Key"·"Action"·"Count" 를 담은 _dic 로 만든다 (테이블 생성기 경로는 Dictionary&lt;string, string&gt; 오버로드다). 없으면 각각 빈 값·0 으로 채운다</summary>
        public SDeal(string _table, string _baseID, string _addID, Dictionary<string, object> _dic)
        {
            Table = _table;
            ID = $"{_baseID}.{_addID}";

            m_Data = new Dictionary<string, object>(_dic);
            object o = null;

            if (_dic.TryGetValue("Key", out o))
                Key = o as string;
            else
                Key = string.Empty;
            m_Data["Key"] = Key;

            if (_dic.TryGetValue("Action", out o))
                Action = o as string;
            else
                Action = string.Empty;
            m_Data["Action"] = Action;

            if (_dic.TryGetValue("Count", out o))
                Count = System.Convert.ToDouble(o);
            else
                Count = 0;
            m_Data["Count"] = Count;
        }
        /// <summary>테이블 행 _dic 에서 "{_addID}Key"·"{_addID}Action"·"{_addID}Count" 컬럼을 찾아 만든다. 없는 컬럼은 각각 빈 값·0 으로 채운다</summary>
        public SDeal(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
        {
            Table = _table;
            ID = $"{_baseID}.{_addID}";

            m_Data = new Dictionary<string, object>();
            string o = null;

            if (_dic.TryGetValue(_addID + "Key", out o))
                Key = o;
            else
                Key = string.Empty;
            m_Data["Key"] = Key;

            if (_dic.TryGetValue(_addID + "Action", out o))
                Action = o;
            else
                Action = string.Empty;
            m_Data["Action"] = Action;

            if (_dic.TryGetValue(_addID + "Count", out o)
                && double.TryParse(o, NumberStyles.Float, CultureInfo.InvariantCulture, out var count))
                Count = count;
            else
                Count = 0;
            m_Data["Count"] = Count;
        }
        /// <summary>테이블 소속을 갖되 값은 직접 지정해 만든다</summary>
        public SDeal(string _table, string _baseID, string _addID, string _key, string _action, double _count)
        {
            Table = _table;
            ID = $"{_baseID}.{_addID}";

            m_Data = new Dictionary<string, object>();
            Key = _key;
            m_Data.Add("Key", Key);
            Action = _action;
            m_Data.Add("Action", Action);
            Count = _count;
            m_Data.Add("Count", Count);
        }
        /// <summary>거래 실행에만 쓸 임시 거래를 만든다. 테이블 소속이 없어 Table·ID 는 null 이고 인덱서 조회도 되지 않는다</summary>
        public SDeal(string _key, string _action, double _count)
        {
            Table = null;
            ID = null;
            m_Data = null;

            Key = _key;
            Action = _action;
            Count = _count;
        }
        #endregion
        #region Function
        /// <summary>수량을 _count 로 바꾼 자신을 반환한다 (구조체 복사본이므로 반환값을 써야 한다)</summary>
        public SDeal SetCount(double _count)
        {
            Count = _count;
            return this;
        }
        /// <summary>수량을 _count 로 바꾼 자신을 반환한다 (구조체 복사본이므로 반환값을 써야 한다)</summary>
        public SDeal SetCount(int _count)
        {
            Count = _count;
            return this;
        }
        /// <summary>수량을 _count 로 바꾼 자신을 반환한다 (구조체 복사본이므로 반환값을 써야 한다)</summary>
        public SDeal SetCount(long _count)
        {
            Count = _count;
            return this;
        }
        /// <summary>수량을 _count 로 바꾼 자신을 반환한다 (구조체 복사본이므로 반환값을 써야 한다)</summary>
        public SDeal SetCount(float _count)
        {
            Count = _count;
            return this;
        }
        #endregion
    }
}
