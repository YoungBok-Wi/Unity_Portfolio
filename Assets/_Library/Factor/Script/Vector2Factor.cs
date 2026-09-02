using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>Vector2 효과를 모아 하나로 집계하는 Factor. 집계 방식은 프리셋으로 고르거나 함수를 직접 준다</summary>
    public sealed class Vector2Factor<TKey> : FactorBase<TKey, Vector2>, IReadOnlyVector2Factor
    {
        #region Type
        /// <summary>집계 프리셋. Average 는 효과가 하나도 없으면 Total 조회 시 예외이고, Add 는 zero 를 돌려준다</summary>
        public enum ETotalType
        {
            Average,
            Add,
        }
        #endregion

        #region Event
        /// <summary>집계 방식을 _totalFunc 으로 직접 정하는 Factor 를 만든다. _totalFunc 은 Global·Local 딕셔너리를 받는다</summary>
        public Vector2Factor(IManageValue _callBy, Func<Dictionary<TKey, Vector2>, Dictionary<TKey, Vector2>, Vector2> _totalFunc) : base(_callBy, _totalFunc)
        {
        }
        /// <summary>_totalType 프리셋으로 집계하는 Factor 를 만든다</summary>
        public Vector2Factor(IManageValue _callBy, ETotalType _totalType) : base(_callBy, null)
        {
            if (_totalType == ETotalType.Average)
                m_OnTotalFunc = (_global, _local) =>
                {
                    if (_global.Count + _local.Count == 0)
                        throw new InvalidOperationException();

                    Vector2 all = Vector2.zero;
                    foreach (var v in _global)
                        all += v.Value;
                    foreach (var v in _local)
                        all += v.Value;

                    return all / (_global.Count + _local.Count);
                };
            else if (_totalType == ETotalType.Add)
                m_OnTotalFunc = (_global, _local) =>
                {
                    Vector2 all = Vector2.zero;
                    foreach (var v in _global)
                        all += v.Value;
                    foreach (var v in _local)
                        all += v.Value;

                    return all;
                };
            else
                throw new ArgumentOutOfRangeException(nameof(_totalType));
        }
        #endregion
    }
}