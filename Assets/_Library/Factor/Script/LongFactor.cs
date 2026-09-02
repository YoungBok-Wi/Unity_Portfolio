using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>long 효과를 모아 하나로 집계하는 Factor. 집계 방식은 프리셋으로 고르거나 함수를 직접 준다</summary>
    public sealed class LongFactor<TKey> : FactorBase<TKey, long>, IReadOnlyLongFactor
    {
        #region Type
        /// <summary>집계 프리셋. Min·Max 는 효과가 하나도 없으면 Total 조회 시 예외이고, Add 는 0 을 돌려준다</summary>
        public enum ETotalType
        {
            Add,
            Min,
            Max,
        }
        #endregion

        #region Event
        /// <summary>집계 방식을 _totalFunc 으로 직접 정하는 Factor 를 만든다. _totalFunc 은 Global·Local 딕셔너리를 받는다</summary>
        public LongFactor(IManageValue _callBy, Func<Dictionary<TKey, long>, Dictionary<TKey, long>, long> _totalFunc) : base(_callBy, _totalFunc)
        {
        }
        /// <summary>_totalType 프리셋으로 집계하는 Factor 를 만든다</summary>
        public LongFactor(IManageValue _callBy, ETotalType _totalType) : base(_callBy, null)
        {
            if (_totalType == ETotalType.Add)
                m_OnTotalFunc = (_global, _local) =>
                {
                    long all = 0;
                    foreach (var v in _global)
                        all += v.Value;
                    foreach (var v in _local)
                        all += v.Value;

                    return all;
                };
            else if (_totalType == ETotalType.Min)
                m_OnTotalFunc = (_global, _local) =>
                {
                    if (_global.Count + _local.Count == 0)
                        throw new InvalidOperationException();

                    long min = long.MaxValue;
                    foreach (var v in _global)
                        if (v.Value < min)
                            min = v.Value;
                    foreach (var v in _local)
                        if (v.Value < min)
                            min = v.Value;

                    return min;
                };
            else if (_totalType == ETotalType.Max)
                m_OnTotalFunc = (_global, _local) =>
                {
                    if (_global.Count + _local.Count == 0)
                        throw new InvalidOperationException();

                    long max = long.MinValue;
                    foreach (var v in _global)
                        if (max < v.Value)
                            max = v.Value;
                    foreach (var v in _local)
                        if (max < v.Value)
                            max = v.Value;

                    return max;
                };
            else
                throw new ArgumentOutOfRangeException(nameof(_totalType));
        }
        #endregion
    }
}