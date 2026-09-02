using System;
using UnityEngine;

namespace Library
{
    /// <summary>DoubleFactor 를 쓰기 없이 노출하는 인터페이스. 효과를 등록할 권한은 소유 매니저만 갖게 한다</summary>
    public interface IReadOnlyDoubleFactor
    {
        /// <summary>등록된 효과를 모두 집계한 값</summary>
        double Total { get; }

        /// <summary>값 변경 리스너를 등록한다. _callBy 의 타입이 등록 계열(글로벌·로컬·컨트롤)을 정하며, _isCallNow 가 true 면 등록 즉시 한 번 호출한다</summary>
        void AddChanged(object _callBy, Action<ValueBase> _action, bool _isCallNow = false);

        /// <summary>등록한 리스너를 제거한다. _callBy 는 등록 때와 같은 것을 넘겨야 한다</summary>
        void RemoveChanged(object _callBy, Action<ValueBase> _action);
    }
}