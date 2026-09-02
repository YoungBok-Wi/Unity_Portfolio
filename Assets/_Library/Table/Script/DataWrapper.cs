using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Library
{
    /// <summary>Vector·Color·DateTime 처럼 생성자를 손댈 수 없는 외부 타입을, 테이블 생성기가 부르는 4인자 시그니처로 맞춰 주는 래퍼</summary>
    public static class DataWrapper
    {
        #region Local Function
        /// <summary>컬럼을 float 로 읽는다. 없거나 파싱에 실패하면 0</summary>
        // 지역 설정과 무관하게 읽도록 InvariantCulture 로 고정한다
        static float F(Dictionary<string, string> _dic, string _key)
        {
            if (_dic != null && _dic.TryGetValue(_key, out var s) && !string.IsNullOrEmpty(s)
                && float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
                return v;
            return 0f;
        }
        /// <summary>컬럼을 long 으로 읽는다. 없거나 파싱에 실패하면 0</summary>
        static long L(Dictionary<string, string> _dic, string _key)
        {
            if (_dic != null && _dic.TryGetValue(_key, out var s) && !string.IsNullOrEmpty(s)
                && long.TryParse(s, out var v))
                return v;
            return 0L;
        }
        #endregion
        #region Function
        /// <summary>"{_addID}x"·"{_addID}y" 컬럼으로 Vector2 를 만든다. 빠진 축은 0</summary>
        public static Vector2 Vector2New(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
            => new Vector2(F(_dic, _addID + "x"), F(_dic, _addID + "y"));
        /// <summary>"{_addID}x"·"y"·"z" 컬럼으로 Vector3 를 만든다. 빠진 축은 0</summary>
        public static Vector3 Vector3New(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
            => new Vector3(F(_dic, _addID + "x"), F(_dic, _addID + "y"), F(_dic, _addID + "z"));
        /// <summary>"{_addID}r"·"g"·"b"·"a" 컬럼으로 Color 를 만든다. 빠진 값은 0 이라 a 를 적지 않으면 완전 투명이 된다</summary>
        public static Color ColorNew(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
            => new Color(F(_dic, _addID + "r"), F(_dic, _addID + "g"), F(_dic, _addID + "b"), F(_dic, _addID + "a"));
        /// <summary>"{_addID}" 컬럼의 long 을 DateTime.FromBinary 로 되돌린다. 값이 없으면 0을 되돌린 시각이 된다</summary>
        public static DateTime DateTimeNew(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
            => DateTime.FromBinary(L(_dic, _addID));
        #endregion
    }
}
