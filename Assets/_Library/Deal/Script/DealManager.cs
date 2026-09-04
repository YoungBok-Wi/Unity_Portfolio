using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>거래 시스템 매니저, Need/Set/Change/Pay 로직 위임 및 Key/Table 기반 매핑</summary>
    public class DealManager : GlobalManagerBase
    {
        public static DealManager instance { get; private set; }

        #region Preview
#if UNITY_EDITOR
        [Serializable] private struct SPreview
        {
            public string id;
            public string by;
            public bool need;
            public bool needValue;
            public bool set;
            public bool change;
            public bool pay;
            public SPreview(string _id, string _by, bool _need, bool _needValue, bool _set, bool _change, bool _pay)
            {
                id = _id;
                by = _by;
                need = _need;
                needValue = _needValue;
                set = _set;
                change = _change;
                pay = _pay;
            }
        }
        [SerializeField] private List<SPreview> m_Preview = new();
#endif
        #endregion
        #region Value
        private Dictionary<string, Func<SDeal, int, bool>> m_OnNeed = new();
        private Dictionary<string, Func<SDeal, ValueBase[]>> m_OnNeedValue = new();
        private Dictionary<string, Func<SDeal, int, SDeal[]>> m_OnSet = new();
        private Dictionary<string, Func<SDeal, int, SDeal[]>> m_OnChange = new();
        private Dictionary<string, Func<SDeal, int, SDeal[]>> m_OnPay = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        #endregion
        #region Function
        /// <summary>_id_table(Key 또는 Table ID)의 거래 처리 콜백들을 등록한다. _callBy 는 초기화 전이어야 하며(Init 안에서 호출), 같은 _id_table 을 두 번 등록해도 예외. 각 콜백은 null 로 둘 수 있고 그 동작은 호출 시 예외가 된다</summary>
        public void Create(GlobalManagerBase _callBy, string _id_table, Func<SDeal, int, bool> _onNeed, Func<SDeal, ValueBase[]> _onNeedValue, Func<SDeal, int, SDeal[]> _onSet, Func<SDeal, int, SDeal[]> _onChange, Func<SDeal, int, SDeal[]> _onPay)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy), $"거래 콜백 등록 호출자가 null : {_id_table}");
            if (_callBy.IsInited)
                throw new InvalidOperationException($"초기화가 끝난 매니저는 거래 콜백을 등록할 수 없다 : {_callBy.name} / {_id_table}");

            m_OnNeed.Add(_id_table, _onNeed);
            m_OnNeedValue.Add(_id_table, _onNeedValue);
            m_OnSet.Add(_id_table, _onSet);
            m_OnChange.Add(_id_table, _onChange);
            m_OnPay.Add(_id_table, _onPay);
#if UNITY_EDITOR
            m_Preview.Add(new SPreview(_id_table, _callBy.gameObject.name, _onNeed != null, _onNeedValue != null, _onSet != null, _onChange != null, _onPay != null));
#endif
        }
        /// <summary>_key(Key 또는 Table ID)의 조건 판정 콜백이 등록되어 있는지 반환한다 (해당 모듈이 임포트되지 않은 빌드에서 호출측이 미리 거를 때 쓴다)</summary>
        public bool HasNeed(string _key)
        {
            if (TableManager.instance.TryGet<object>(_key, out var table) && table is ITableType iTable && m_OnNeed.ContainsKey(iTable.Table))
                return true;
            return m_OnNeed.ContainsKey(_key);
        }
        // Key 를 Table 로 먼저 해석해 테이블 단위 콜백을 찾고, 없으면 Key 단위 콜백으로 폴백한다
        /// <summary>_deal 의 조건이 충족되는지 반환한다. _seed 는 확률형 조건에 쓰이며, 같은 값이면 결과가 재현된다. _deal.Action 에 "Not" 이 들어 있으면 결과가 반전된다. 등록되지 않은 Key 면 예외</summary>
        public bool Need(SDeal _deal, int _seed)
        {
            Func<SDeal, int, bool> func = null;
            if (TableManager.instance.TryGet<object>(_deal.Key, out var table) && table is ITableType iTable)
                m_OnNeed.TryGetValue(iTable.Table, out func);
            if (func == null)
                m_OnNeed.TryGetValue(_deal.Key, out func);
            if (func == null)
                throw new ArgumentException($"Need 콜백이 등록되지 않은 거래 Key : {_deal.Key}", nameof(_deal));

            var result = func.Invoke(_deal, _seed);

            if (_deal.Action.Contains("Not"))
                result = !result;

            return result;
        }
        /// <summary>_deal 의 조건이 충족되는지 반환한다 (시드를 매번 무작위로 뽑으므로 확률형 조건은 재현되지 않는다)</summary>
        public bool Need(SDeal _deal)
        {
            return Need(_deal, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>_deals 가 모두 충족되는지 반환한다. 하나라도 실패하면 나머지는 확인하지 않는다. 각 항목에는 _seed 에 인덱스를 더한 시드가 쓰인다</summary>
        public bool NeedAll(SDeal[] _deals, int _seed)
        {
            bool result = true;
            for (int i = 0; i < _deals.Length; i++)
            {
                result = result && Need(_deals[i], _seed + i);
                if (!result)
                    break;
            }

            return result;
        }
        /// <summary>_deals 가 모두 충족되는지 반환한다 (시드를 매번 무작위로 뽑는다)</summary>
        public bool NeedAll(SDeal[] _deals)
        {
            return NeedAll(_deals, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>_deal 의 충족 여부가 달라질 때 알 수 있도록, 구독할 ValueBase 들을 반환한다. 등록되지 않은 Key 면 예외</summary>
        public ValueBase[] NeedValue(SDeal _deal)
        {
            if (!m_OnNeedValue.TryGetValue(_deal.Key, out var func) || func == null)
                throw new ArgumentException($"NeedValue 콜백이 등록되지 않은 거래 Key : {_deal.Key}", nameof(_deal));

            var result = func.Invoke(_deal);

            return result;
        }
        /// <summary>_deals 전체의 충족 여부를 구독할 ValueBase 들을 모아 반환한다. NeedValue 와 달리 미등록 Key 는 예외 없이 건너뛴다</summary>
        public List<ValueBase> NeedAllValue(SDeal[] _deals)
        {
            var result = new List<ValueBase>();
            foreach (var v in _deals)
                if (m_OnNeedValue.TryGetValue(v.Key, out var func) && func != null)
                    result.AddRange(func.Invoke(v));

            return result;
        }
        /// <summary>_deal 의 값을 그대로 설정하고, 실제로 설정된 거래를 반환한다. _seed 는 확률형 처리에 쓰인다. 조건 검사 없이 적용된다. 등록되지 않은 Key 면 예외</summary>
        public SDeal[] Set(SDeal _deal, int _seed)
        {
            Func<SDeal, int, SDeal[]> func = null;
            if (TableManager.instance.TryGet<object>(_deal.Key, out var table) && table is ITableType iTable)
                m_OnSet.TryGetValue(iTable.Table, out func);
            if (func == null)
                m_OnSet.TryGetValue(_deal.Key, out func);
            if (func == null)
                throw new ArgumentException($"Set 콜백이 등록되지 않은 거래 Key : {_deal.Key}", nameof(_deal));
            var result = func.Invoke(_deal, _seed);
            return result;
        }
        /// <summary>_deal 의 값을 설정하고 실제로 설정된 거래를 반환한다 (시드를 매번 무작위로 뽑는다)</summary>
        public SDeal[] Set(SDeal _deal)
        {
            return Set(_deal, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>_deals 를 모두 설정하고 실제로 설정된 거래를 한 배열로 모아 반환한다. 각 항목에는 _seed 에 인덱스를 더한 시드가 쓰인다</summary>
        public SDeal[] SetAll(SDeal[] _deals, int _seed)
        {
            var result = new List<SDeal>();
            for (int i = 0; i < _deals.Length; i++)
                result.AddRange(Set(_deals[i], _seed + i));

            return result.ToArray();
        }
        /// <summary>_deals 를 모두 설정하고 실제로 설정된 거래를 반환한다 (시드를 매번 무작위로 뽑는다)</summary>
        public SDeal[] SetAll(SDeal[] _deals)
        {
            return SetAll(_deals, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>_deal 의 양만큼 증감하고, 실제로 변동된 거래를 반환한다. _seed 는 확률형 처리에 쓰인다. 조건 검사 없이 적용된다. 등록되지 않은 Key 면 예외</summary>
        public SDeal[] Change(SDeal _deal, int _seed)
        {
            Func<SDeal, int, SDeal[]> func = null;
            if (TableManager.instance.TryGet<object>(_deal.Key, out var table) && table is ITableType iTable)
                m_OnChange.TryGetValue(iTable.Table, out func);
            if (func == null)
                m_OnChange.TryGetValue(_deal.Key, out func);
            if (func == null)
                throw new ArgumentException($"Change 콜백이 등록되지 않은 거래 Key : {_deal.Key}", nameof(_deal));

            var result = func.Invoke(_deal, _seed);

            return result;
        }
        /// <summary>_deal 의 양만큼 증감하고 실제로 변동된 거래를 반환한다 (시드를 매번 무작위로 뽑는다)</summary>
        public SDeal[] Change(SDeal _deal)
        {
            return Change(_deal, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>_deals 를 모두 증감하고 실제로 변동된 거래를 한 배열로 모아 반환한다. 각 항목에는 _seed 에 인덱스를 더한 시드가 쓰인다</summary>
        public SDeal[] ChangeAll(SDeal[] _deals, int _seed)
        {
            var result = new List<SDeal>();
            for (int i = 0; i < _deals.Length; i++)
                result.AddRange(Change(_deals[i], _seed + i));

            return result.ToArray();
        }
        /// <summary>_deals 를 모두 증감하고 실제로 변동된 거래를 반환한다 (시드를 매번 무작위로 뽑는다)</summary>
        public SDeal[] ChangeAll(SDeal[] _deals)
        {
            return ChangeAll(_deals, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>조건을 먼저 확인하고 통과할 때만 _deal 의 양만큼 지불한다. 실제로 지불된 거래를 반환하며, 조건 미충족이면 아무것도 하지 않고 null. 등록되지 않은 Key 면 예외</summary>
        public SDeal[] Pay(SDeal _deal, int _seed)
        {
            if (!Need(_deal, _seed))
                return null;

            Func<SDeal, int, SDeal[]> func = null;
            if (TableManager.instance.TryGet<object>(_deal.Key, out var table) && table is ITableType iTable)
                m_OnPay.TryGetValue(iTable.Table, out func);
            if (func == null)
                m_OnPay.TryGetValue(_deal.Key, out func);
            if (func == null)
                throw new ArgumentException($"Pay 콜백이 등록되지 않은 거래 Key : {_deal.Key}", nameof(_deal));

            var result = func.Invoke(_deal, _seed);

            return result;
        }
        /// <summary>조건을 확인하고 통과할 때만 지불한다. 조건 미충족이면 null (시드를 매번 무작위로 뽑는다)</summary>
        public SDeal[] Pay(SDeal _deal)
        {
            return Pay(_deal, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        /// <summary>_deals 전체 조건을 먼저 확인하고, 모두 통과할 때만 전부 지불한다. 하나라도 미충족이면 아무것도 지불하지 않고 null. 반환 배열은 _deals 와 같은 순서다</summary>
        public SDeal[][] PayAll(SDeal[] _deals, int _seed)
        {
            if (!NeedAll(_deals, _seed))
                return null;

            var result = new SDeal[_deals.Length][];
            for (int i = 0; i < _deals.Length; i++)
                result[i] = Pay(_deals[i], _seed + i);

            return result;
        }
        /// <summary>_deals 전체 조건이 모두 통과할 때만 전부 지불한다. 하나라도 미충족이면 null (시드를 매번 무작위로 뽑는다)</summary>
        public SDeal[][] PayAll(SDeal[] _deals)
        {
            return PayAll(_deals, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        }
        #endregion
    }
}