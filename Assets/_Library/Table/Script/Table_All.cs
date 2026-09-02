using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>로드된 테이블 데이터를 ID 로 모아 두는 컨테이너</summary>
    [Serializable] public class Table_All
    {
        #region Property
        /// <summary>ID → 데이터</summary>
        public IReadOnlyDictionary<string, object> Data => m_Data;
        /// <summary>등록된 ID 목록 (등록 순서대로)</summary>
        public IReadOnlyList<string> ID => m_ID;
        /// <summary>등록된 데이터 수</summary>
        public int Count => m_ID.Count;
        #endregion
        #region Value
        private Dictionary<string, object> m_Data = new Dictionary<string, object>();
        private List<string> m_ID = new List<string>();
        #endregion
        #region Function
        /// <summary>_id 에 _data 를 넣는다. 이미 있으면 데이터만 갈아끼우고 ID 순서는 그대로 둔다</summary>
        public void Apply(string _id, object _data)
        {
            m_Data.Set(_id, _data);

            if (!m_ID.Contains(_id))
                m_ID.Add(_id);
        }
        #endregion
    }
}