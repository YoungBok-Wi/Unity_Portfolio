using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Library
{
    /// <summary>재화 관리 매니저, ID별 잔액/최대값 및 거래 시스템 연동</summary>
    public class BankManager : GlobalManagerBase
    {
        public static BankManager instance { get; private set; }

        #region Preview
#if UNITY_EDITOR
        [Serializable] private struct SPreview
        {
            public string id;
            public string by;
            public long max;
            public long value;
            public SPreview(string _id, string _by, long _max, long _value)
            {
                id = _id;
                by = _by;
                max = _max;
                value = _value;
            }
        }
        [SerializeField, TabGroup("BankManager", "미리보기"), ReadOnly] private List<SPreview> m_Preview = new();
#endif
        #endregion
        #region Property
        /// <summary>등록된 재화 ID → 잔액</summary>
        public IReadOnlyDictionary<string, IReadOnlyLongValue> Value => m_ReadOnlyValue;
        #endregion
        #region Value
        private Dictionary<string, long> m_Max = new();
        private Dictionary<string, LongValue> m_Value = new();
        private Dictionary<string, IReadOnlyLongValue> m_ReadOnlyValue = new();
        private Dictionary<string, bool> m_IsUnit = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        #endregion
        #region Function
        /// <summary>_id 재화를 등록하고 그 잔액 값을 반환한다. 저장·숫자·표시·거래에 한꺼번에 연동되며, _max 가 0보다 크면 잔액이 그 값으로 제한되고 잔액은 항상 0 이상으로 보정된다. _saveTable 이 비면 저장하지 않고, _isUnit 은 표시에 단위(K·M)를 붙일지 정한다. _on* 콜백을 주면 기본 거래 동작을 갈아끼운다. _callBy 는 초기화 전이어야 한다</summary>
        public LongValue Create(GlobalManagerBase _callBy, string _id, string _saveTable, string _saveArray, int _saveIndex, bool _isUnit, long _max = -1, Func<SDeal, int, bool> _onNeed = null, Func<SDeal, ValueBase[]> _onNeedValue = null, Func<SDeal, int, SDeal[]> _onSet = null, Func<SDeal, int, SDeal[]> _onChange = null, Func<SDeal, int, SDeal[]> _onPay = null)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy), $"재화 등록 호출자가 null : {_id}");
            if (_callBy.IsInited)
                throw new InvalidOperationException($"초기화가 끝난 매니저는 재화를 등록할 수 없다 : {_callBy.name} / {_id}");

            var value = new LongValue(_callBy, _id, 0);
            if (!string.IsNullOrEmpty(_saveTable))
            {
                if (string.IsNullOrEmpty(_saveArray))
                    value = SaveUtil.Create(_callBy, _saveTable, value, SaveUtil.EType.DB);
                else
                    value = SaveUtil.CreateArray(_callBy, _saveTable, _saveArray, _saveIndex, value, (v) => value.Set(v, true, false), () => value.v, 0L, SaveUtil.EType.DB);
            }
            m_Value.Add(_id, value);
            m_ReadOnlyValue.Add(_id, value);
            m_Max.Add(_id, _max);
            m_IsUnit.Add(_id, _isUnit);

            value.AddConstraintChanged(_callBy, (_) =>
            {
                var count = math.max(0, value.v);
                if (m_Max.TryGetValue(_id, out var m) && 0 < m)
                    count = math.min(count, m);

                value.Set(count, false, false);
            });

            NumberManager.instance.Create(_callBy, _id, value);
            LanguageManager.instance.Create(_callBy, $"{_id}_Max", null, (_, _table) => _table.SetEng(m_Max[_id].ToStringLong(_isUnit)));
            LanguageManager.instance.Create(_callBy, _id, value, (_, _table) => _table.SetEng(value.v.ToStringLong(_isUnit)));
            DealManager.instance.Create(_callBy, _id, _onNeed ?? ((_deal, _seed) =>
            {   //Need
                return _deal.CountLong <= value.v;
            }), _onNeedValue ?? ((_deal) =>
            {   //NeedValue
                return new ValueBase[] { value };
            }), _onSet ?? ((_deal, _seed) =>
            {   //Set
                Set(_id, _deal.CountLong);
                return new SDeal[] { _deal };
            }), _onChange ?? ((_deal, _seed) =>
            {   //Change
                Change(_id, _deal.CountLong);
                return new SDeal[] { _deal };
            }), _onPay ?? ((_deal, _seed) =>
            {   //Pay — Need 가 양수 Count 를 전제하므로 지불은 그만큼 차감한다
                Change(_id, -_deal.CountLong);
                return new SDeal[] { _deal };
            }));

#if UNITY_EDITOR
            int index = m_Preview.Count;
            m_Preview.Add(new SPreview(_id, _callBy.name, _max, value.v));
            value.AddChanged(_callBy, (_) =>
            {
                m_Preview[index] = new SPreview(_id, _callBy.name, _max, value.v);
            });
#endif

            return value;
        }
        /// <summary>_id 재화의 잔액을 반환한다. 미등록이면 null</summary>
        public IReadOnlyLongValue Get(string _id)
        {
            if (m_Value.TryGetValue(_id, out var v))
                return v;

            return null;
        }
        /// <summary>_id 재화의 잔액을 _count 로 설정한다. 0~최대 범위로 보정되며, 미등록 _id 면 예외</summary>
        public void Set(string _id, long _count)
        {
            m_Value[_id].v = _count;
        }
        /// <summary>_id 재화의 잔액에 _count 를 더한다. 음수를 넘기면 차감이며, 결과는 0~최대 범위로 보정된다</summary>
        public void Change(string _id, long _count)
        {
            m_Value[_id].v += _count;
        }
        #endregion
    }
}
