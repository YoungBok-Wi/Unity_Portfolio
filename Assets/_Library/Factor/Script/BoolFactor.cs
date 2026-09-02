using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>bool 효과를 모아 하나로 집계하는 Factor. 버프 차단 플래그처럼 여러 곳이 동시에 한 조건을 쥐는 경우에 쓴다</summary>
    public class BoolFactor<TKey> : FactorBase<TKey, bool>, IReadOnlyBoolFactor
    {
        #region Type
        /// <summary>집계 프리셋. And 는 하나라도 false 면 false, Or 는 하나라도 true 면 true 다</summary>
        public enum ETotalType
        {
            And,
            Or,
        }
        #endregion

        #region Event
        /// <summary>집계 방식을 _totalFunc 으로 직접 정하는 Factor 를 만든다. _totalFunc 은 Global·Local 딕셔너리를 받는다</summary>
        public BoolFactor(IManageValue _callBy, Func<Dictionary<TKey, bool>, Dictionary<TKey, bool>, bool> _totalFunc) : base(_callBy, _totalFunc)
        {
        }
        /// <summary>_totalType 프리셋으로 집계하는 Factor 를 만든다</summary>
        public BoolFactor(IManageValue _callBy, ETotalType _totalType) : base(_callBy, null)
        {
            if (_totalType == ETotalType.And)
                m_OnTotalFunc = (_global, _local) =>
                {
                    bool and = true;
                    foreach (var v in _global)
                        and &= v.Value;
                    foreach (var v in _local)
                        and &= v.Value;

                    return and;
                };
            else if (_totalType == ETotalType.Or)
                m_OnTotalFunc = (_global, _local) =>
                {
                    bool or = false;
                    foreach (var v in _global)
                        or |= v.Value;
                    foreach (var v in _local)
                        or |= v.Value;

                    return or;
                };
            else
                throw new ArgumentOutOfRangeException(nameof(_totalType));
        }
        #endregion
    }
}