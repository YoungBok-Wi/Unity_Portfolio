using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>Dictionary 확장 메서드 모음</summary>
    public static class DictionaryUtil
    {
        #region Function
        /// <summary>keys 가 전부 있는지 반환한다. 하나라도 없으면 false 이며, keys 가 비면 true</summary>
        public static bool ContainsKeys<TKey, TValue>(this Dictionary<TKey, TValue> _dic, params TKey[] keys)
        {
            if (_dic == null)
                throw new ArgumentNullException(nameof(_dic));
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            foreach (var key in keys)
                if (!_dic.ContainsKey(key))
                    return false;

            return true;
        }
        /// <summary>_key 가 없을 때만 넣는다. 이미 있으면 기존 값을 그대로 둔다 (Add 와 달리 예외가 아니다)</summary>
        public static void AddEx<TKey, TValue>(this Dictionary<TKey, TValue> _dic, TKey _key, TValue _value)
        {
            if (!_dic.ContainsKey(_key))
                _dic.Add(_key, _value);
        }
        /// <summary>_key 에 _value 를 넣는다. 이미 있으면 덮어쓴다</summary>
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> _dic, TKey _key, TValue _value)
        {
            if (_dic.ContainsKey(_key))
                _dic[_key] = _value;
            else
                _dic.Add(_key, _value);
        }
        /// <summary>_remove 의 키를 모두 지운다. 없는 키는 그냥 넘어간다</summary>
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> _list, TKey[] _remove)
        {
            foreach (var v in _remove)
                _list.Remove(v);
        }
        /// <summary>_remove 의 키를 모두 지운다. 없는 키는 그냥 넘어간다</summary>
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> _list, List<TKey> _remove)
        {
            foreach (var v in _remove)
                _list.Remove(v);
        }
        #endregion
    }
}
