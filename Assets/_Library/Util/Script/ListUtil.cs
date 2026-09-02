using System.Collections.Generic;

namespace Library
{
    /// <summary>List 확장 메서드 모음</summary>
    public static class ListUtil
    {
        #region Function
        /// <summary>_remove 의 요소를 모두 지운다. 없는 요소는 그냥 넘어가며, 같은 값이 여러 개면 하나씩만 지워진다</summary>
        public static void RemoveAll<T>(this List<T> _list, T[] _remove)
        {
            foreach (var v in _remove)
                _list.Remove(v);
        }
        /// <summary>_remove 의 요소를 모두 지운다. 없는 요소는 그냥 넘어가며, 같은 값이 여러 개면 하나씩만 지워진다</summary>
        public static void RemoveAll<T>(this List<T> _list, List<T> _remove)
        {
            foreach (var v in _remove)
                _list.Remove(v);
        }
        /// <summary>_targetIndex 로 접근할 수 있을 때까지 _default 를 채워 늘린다. 이미 충분히 길면 그대로 둔다</summary>
        public static void EnsureListIndex<T>(this List<T> _list, int _targetIndex, T _default)
        {
            while (_list.Count <= _targetIndex)
                _list.Add(_default);
        }
        #endregion
    }
}
