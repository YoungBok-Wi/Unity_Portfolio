using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>테이블 조회·상수 브릿지를 담당하는 매니저. 테이블 프로퍼티와 초기화는 생성 partial(TableManager_Generate)에 있고, 확장은 이 파일에 손으로 적는다</summary>
    public partial class TableManager : GlobalManagerBase
    {
        public static TableManager instance { get; private set; }

        #region Property
        /// <summary>로드된 테이블 전체</summary>
        public Table_All All { get; } = new();
        #endregion
        #region Value
        // Library 가 게임 상수를 직접 참조할 수 없어, 하위(Game)가 등록하고 Library 는 GetConst 로 꺼내 쓴다
        protected Dictionary<string, object> m_Const = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        #endregion
        #region Function
        /// <summary>_key 로 등록된 게임 상수를 T 로 반환한다. 미등록이거나 타입이 다르면 예외</summary>
        public T GetConst<T>(string _key) => m_Const.TryGetValue(_key, out var v) ? (T)v : throw new ArgumentException();
        /// <summary>_path 를 따라 테이블 데이터를 반환한다. 경로가 없으면 예외</summary>
        public T Get<T>(string[] _path)
        {
            if (!TryGet<T>(_path, out var result))
                throw new ArgumentException();
            return result;
        }
        /// <summary>_path 를 따라 테이블 데이터를 반환한다. _path 는 "Table.Row.Column" 처럼 점으로 구분하며, 경로가 없으면 예외</summary>
        public T Get<T>(string _path)
        {
            var paths = _path.Split('.');
            return Get<T>(paths);
        }
        /// <summary>_path 를 따라 테이블 데이터를 찾아 _out 에 담고 성공 여부를 반환한다. 경로가 없으면 false 이며, _path 자체가 비면 예외</summary>
        public bool TryGet<T>(string[] _path, out T _out)
        {
            if (_path == null || _path.Length == 0)
                throw new ArgumentException();

            _out = default;
            if (!All.Data.TryGetValue(_path[0], out var table))
                return false;
            for (int i = 1; i < _path.Length; ++i)
            {
                if (table is ITableType iTable)
                    table = iTable[_path[i]];
                else
                    return false;
            }
            if (table == null)
                return false;

            _out = (T)table;
            return true;
        }
        /// <summary>_path 를 따라 테이블 데이터를 찾아 _out 에 담고 성공 여부를 반환한다. _path 는 점으로 구분한다</summary>
        public bool TryGet<T>(string _path, out T _out)
        {
            var paths = _path.Split('.');
            return TryGet(paths, out _out);
        }
        #endregion
    }
}
