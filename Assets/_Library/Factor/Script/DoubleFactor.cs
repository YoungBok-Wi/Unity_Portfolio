using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>double 효과를 모아 하나로 집계하는 Factor. 집계 방식은 프리셋으로 고르거나 함수를 직접 준다</summary>
    public sealed class DoubleFactor<TKey> : FactorBase<TKey, double>, IReadOnlyDoubleFactor
    {
        #region Type
        /// <summary>집계 프리셋. Average·Min·Max 는 효과가 하나도 없으면 Total 조회 시 예외이고, Add 는 0·Multifly 는 1 을 돌려준다</summary>
        public enum ETotalType
        {
            Average,
            Add,
            Min,
            Max,
            Multifly
        }
        #endregion

        #region Event
        /// <summary>집계 방식을 _totalFunc 으로 직접 정하는 Factor 를 만든다. _totalFunc 은 Global·Local 딕셔너리를 받는다</summary>
        public DoubleFactor(IManageValue _callBy, Func<Dictionary<TKey, double>, Dictionary<TKey, double>, double> _totalFunc) : base(_callBy, _totalFunc)
        {
        }
        /// <summary>_totalType 프리셋으로 집계하는 Factor 를 만든다</summary>
        public DoubleFactor(IManageValue _callBy, ETotalType _totalType) : base(_callBy, null)
        {
            if (_totalType == ETotalType.Average)
                m_OnTotalFunc = (_global, _local) =>
                {
                    if (_global.Count + _local.Count == 0)
                        throw new InvalidOperationException();

                    double all = 0;
                    foreach (var v in _global)
                        all += v.Value;
                    foreach (var v in _local)
                        all += v.Value;

                    return all / (_global.Count + _local.Count);
                };
            else if (_totalType == ETotalType.Add)
                m_OnTotalFunc = (_global, _local) =>
                {
                    double all = 0;
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

                    double min = double.MaxValue;
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

                    double max = double.MinValue;
                    foreach (var v in _global)
                        if (max < v.Value)
                            max = v.Value;
                    foreach (var v in _local)
                        if (max < v.Value)
                            max = v.Value;

                    return max;
                };
            else if (_totalType == ETotalType.Multifly)
                m_OnTotalFunc = (_global, _local) =>
                {
                    double multifly = 1.0f;
                    foreach (var v in _global)
                        multifly *= v.Value;
                    foreach (var v in _local)
                        multifly *= v.Value;

                    return multifly;
                };
            else
                throw new ArgumentOutOfRangeException(nameof(_totalType));
        }
        #endregion
        #region Function
        /// <summary>값이 음수인 효과를 모두 지운다. 실제로 지워졌을 때만 변경이 통지된다</summary>
        public void RemoveZeroLess()
        {
            var removeGlobal = new List<TKey>();
            foreach (var v in m_Global)
                if (v.Value < 0)
                    removeGlobal.Add(v.Key);
            var removeLocal = new List<TKey>();
            foreach (var v in m_Local)
                if (v.Value < 0)
                    removeLocal.Add(v.Key);

            m_Global.RemoveAll(removeGlobal);
            m_Local.RemoveAll(removeLocal);

            if (removeGlobal.Count + removeLocal.Count > 0)
                OnChanged(EChangeType.None);
        }
        /// <summary>등록된 모든 효과 값에 add 를 더한다 (Global·Local 전부)</summary>
        public void AddAll(double add)
        {
            foreach (var key in new List<TKey>(m_Global.Keys))
                m_Global[key] += add;
            foreach (var key in new List<TKey>(m_Local.Keys))
                m_Local[key] += add;

            OnChanged(EChangeType.None);
        }
        #endregion
    }
}