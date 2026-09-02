using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>여러 출처의 효과를 Key 별로 모아 하나로 집계하는 값의 베이스. 출처는 Global·Local 로 나뉘며 씬을 벗어나면 Local 만 비워진다</summary>
    public class FactorBase<TKey, TValue> : ValueBase
    {
        #region Property
        /// <summary>Global·Local 을 합친 집계값. 집계 함수가 없으면 예외</summary>
        public TValue Total
        {
            get
            {
                if (m_OnTotalFunc == null)
                    throw new InvalidOperationException();
                return m_OnTotalFunc(m_Global, m_Local);
            }
        }
        #endregion
        #region Value
        protected Dictionary<TKey, TValue> m_Local = new();
        protected Dictionary<TKey, TValue> m_Global = new();
        protected Func<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, TValue> m_OnTotalFunc;
        #endregion

        #region Event
        public FactorBase(IManageValue _callBy, Func<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, TValue> _totalFunc) : base(_callBy, null)
        {
            m_OnTotalFunc = _totalFunc;
        }

        public override void OnResetLocalChanged()
        {
            base.OnResetLocalChanged();
            m_Local.Clear();
        }
        #endregion
        #region Function
        /// <summary>id 효과를 factor 로 등록·갱신한다. _callBy 가 글로벌 매니저면 Global, 그 외에는 Local 로 분류되어 씬 전환 시 정리 여부가 갈린다</summary>
        public void Set(object _callBy, TKey id, TValue factor = default)
        {
            if (_callBy as GlobalManagerBase)
                m_Global.Set(id, factor);
            else
                m_Local.Set(id, factor);

            OnChanged(EChangeType.None);
        }
        /// <summary>id 효과가 등록돼 있는지 반환한다 (Global·Local 모두 본다)</summary>
        public bool GetContains(TKey id)
        {
            return m_Global.ContainsKey(id) || m_Local.ContainsKey(id);
        }
        /// <summary>id 효과 값을 반환한다. Global 을 먼저 보므로 같은 id 가 양쪽에 있으면 Global 이 이긴다. 없으면 예외</summary>
        public TValue Get(TKey id)
        {
            if (m_Global.TryGetValue(id, out var v))
                return v;
            if (m_Local.TryGetValue(id, out v))
                return v;
            throw new KeyNotFoundException();
        }
        /// <summary>_id 효과를 지운다. Global 에서 지워지면 Local 은 건드리지 않으며, 실제로 지워졌을 때만 변경이 통지된다</summary>
        public void Remove(TKey _id)
        {
            if (m_Global.Remove(_id))
                OnChanged(EChangeType.None);
            else if (m_Local.Remove(_id))
                OnChanged(EChangeType.None);
        }
        #endregion
    }
}