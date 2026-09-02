using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>숫자 값 관리 매니저, ID→ValueBase 매핑 및 타입별 Get/Set</summary>
    public class NumberManager : GlobalManagerBase
    {
        public static NumberManager instance { get; private set; }

        #region Preview
#if UNITY_EDITOR
        [Serializable] private struct SPreview
        {
            public string id;
            public string by;
            public double number;
            public SPreview(string _id, string _by, double _number)
            {
                id = _id;
                by = _by;
                number = _number;
            }
        }
        [SerializeField, TabGroup("NumberManager", "미리보기"), ReadOnly] private List<SPreview> m_Preview = new();
#endif
        #endregion
        #region Property
        /// <summary>등록된 ID → ValueBase 전체</summary>
        public IReadOnlyDictionary<string, ValueBase> Value => m_Value;
        #endregion
        #region Value
        private Dictionary<string, ValueBase> m_Value = new Dictionary<string, ValueBase>();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        #endregion
        #region Function
        /// <summary>_id 로 _value 를 등록한다. _callBy 는 초기화 전이어야 하며(초기화 후 등록은 예외), 같은 _id 를 두 번 등록해도 예외다</summary>
        public void Create(GlobalManagerBase _callBy, string _id, ValueBase _value)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy), $"표시 값 등록 호출자가 null : {_id}");
            if (_callBy.IsInited)
                throw new InvalidOperationException($"초기화가 끝난 매니저는 표시 값을 등록할 수 없다 : {_callBy.name} / {_id}");

#if UNITY_EDITOR
            int index = m_Preview.Count;
            m_Preview.Add(new(_id, _callBy.name, 0));
#endif

            m_Value.Add(_id, _value);
#if UNITY_EDITOR
            _value.AddResourceChanged(this, (_value) =>
            {
                m_Preview[index] = new(_id, _callBy.name, GetDouble(_id));
            }, true);
#endif
        }
        /// <summary>_value 를 long 으로 읽는다. 숫자형(Int·Long·Float·Double)이 아니면 예외이며, 실수형은 버림 변환된다</summary>
        public long GetLong(ValueBase _value)
        {
            if (_value == null)
                throw new ArgumentNullException(nameof(_value));

            switch (_value)
            {
                case IntValue v: return v.v;
                case LongValue v: return v.v;
                case FloatValue v: return (long)v.v;
                case DoubleValue v: return (long)v.v;
            }
            throw new ArgumentException(nameof(_value));
        }
        /// <summary>_value 를 int 로 읽는다 (long 범위를 넘으면 잘린다)</summary>
        public int GetInt(ValueBase _value)
        {
            return (int)GetLong(_value);
        }
        /// <summary>_value 를 double 로 읽는다. 숫자형(Int·Long·Float·Double)이 아니면 예외</summary>
        public double GetDouble(ValueBase _value)
        {
            if (_value == null)
                throw new ArgumentNullException(nameof(_value));

            switch (_value)
            {
                case IntValue v: return v.v;
                case LongValue v: return v.v;
                case FloatValue v: return (double)v.v;
                case DoubleValue v: return v.v;
            }
            throw new ArgumentException(nameof(_value));
        }
        /// <summary>_value 를 float 로 읽는다 (double 값은 정밀도가 줄 수 있다)</summary>
        public float GetFloat(ValueBase _value)
        {
            return (float)GetDouble(_value);
        }
        /// <summary>_id 의 값을 long 으로 읽는다. 미등록 _id 면 예외</summary>
        public long GetLong(string _id)
        {
            if (m_Value.TryGetValue(_id, out var v))
                return GetLong(v);
            throw new KeyNotFoundException(_id);
        }
        /// <summary>_id 의 값을 int 로 읽는다. 미등록 _id 면 예외</summary>
        public int GetInt(string _id)
        {
            return (int)GetLong(_id);
        }
        /// <summary>_id 의 값을 double 로 읽는다. 미등록 _id 면 예외</summary>
        public double GetDouble(string _id)
        {
            if (m_Value.TryGetValue(_id, out var v))
                return GetDouble(v);
            throw new KeyNotFoundException(_id);
        }
        /// <summary>_id 의 값을 float 로 읽는다. 미등록 _id 면 예외</summary>
        public float GetFloat(string _id)
        {
            return (float)GetDouble(_id);
        }
        /// <summary>_id 에 등록된 ValueBase 를 반환한다. 미등록이면 null (조회 계열과 달리 예외가 아니다)</summary>
        public ValueBase GetValue(string _id)
        {
            if (m_Value.TryGetValue(_id, out var v))
                return v;
            return null;
        }
        /// <summary>_ids 각각의 ValueBase 를 같은 순서로 반환한다. 미등록 항목은 배열에 null 로 남는다</summary>
        public ValueBase[] GetValues(string[] _ids)
        {
            ValueBase[] values = new ValueBase[_ids.Length];
            for (int i = 0; i < _ids.Length; i++)
                values[i] = GetValue(_ids[i]);

            return values;
        }
        /// <summary>_id 의 값을 _value 로 설정한다. _value 는 등록된 값의 타입에 맞춰 변환되므로 IntValue 에 넣으면 잘린다. 미등록 _id 면 예외</summary>
        public void Set(string _id, long _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v = (int)_value;
                    break;
                case LongValue:
                    (v as LongValue).v = _value;
                    break;
                case FloatValue:
                    (v as FloatValue).v = _value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v = _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값을 _value 로 설정한다. _value 는 등록된 값의 타입에 맞춰 변환된다. 미등록 _id 면 예외</summary>
        public void Set(string _id, int _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v = _value;
                    break;
                case LongValue:
                    (v as LongValue).v = _value;
                    break;
                case FloatValue:
                    (v as FloatValue).v = _value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v = _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값을 _value 로 설정한다. _value 는 등록된 값의 타입에 맞춰 변환되므로 정수형에 넣으면 소수점이 버려진다. 미등록 _id 면 예외</summary>
        public void Set(string _id, float _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v = (int)_value;
                    break;
                case LongValue:
                    (v as LongValue).v = (long)_value;
                    break;
                case FloatValue:
                    (v as FloatValue).v = _value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v = _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값을 _value 로 설정한다. _value 는 등록된 값의 타입에 맞춰 변환되므로 정수형에 넣으면 소수점이 버려진다. 미등록 _id 면 예외</summary>
        public void Set(string _id, double _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v = (int)_value;
                    break;
                case LongValue:
                    (v as LongValue).v = (long)_value;
                    break;
                case FloatValue:
                    (v as FloatValue).v = (float)_value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v = _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값에 _value 를 더한다. _value 는 등록된 값의 타입에 맞춰 변환된다. 미등록 _id 면 예외</summary>
        public void Add(string _id, long _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v += (int)_value;
                    break;
                case LongValue:
                    (v as LongValue).v += _value;
                    break;
                case FloatValue:
                    (v as FloatValue).v += _value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v += _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값에 _value 를 더한다. _value 는 등록된 값의 타입에 맞춰 변환된다. 미등록 _id 면 예외</summary>
        public void Add(string _id, int _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v += _value;
                    break;
                case LongValue:
                    (v as LongValue).v += _value;
                    break;
                case FloatValue:
                    (v as FloatValue).v += _value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v += _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값에 _value 를 더한다. 정수형 값에 더하면 _value 의 소수점이 버려진 뒤 더해진다. 미등록 _id 면 예외</summary>
        public void Add(string _id, float _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v += (int)_value;
                    break;
                case LongValue:
                    (v as LongValue).v += (long)_value;
                    break;
                case FloatValue:
                    (v as FloatValue).v += _value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v += _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        /// <summary>_id 의 값에 _value 를 더한다. 정수형 값에 더하면 _value 의 소수점이 버려진 뒤 더해진다. 미등록 _id 면 예외</summary>
        public void Add(string _id, double _value)
        {
            if (!m_Value.TryGetValue(_id, out var v))
                throw new KeyNotFoundException(_id);

            switch (v)
            {
                case IntValue:
                    (v as IntValue).v += (int)_value;
                    break;
                case LongValue:
                    (v as LongValue).v += (long)_value;
                    break;
                case FloatValue:
                    (v as FloatValue).v += (float)_value;
                    break;
                case DoubleValue:
                    (v as DoubleValue).v += _value;
                    break;
                default:
                    throw new InvalidOperationException(_id);
            }
        }
        #endregion
    }
}
