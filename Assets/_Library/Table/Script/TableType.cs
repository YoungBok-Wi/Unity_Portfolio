using System.Collections.Generic;

namespace Library
{
    /// <summary>테이블 행의 기본 구현. 생성기가 만드는 행 타입들이 이걸 상속한다</summary>
    public class TableType : ITableType
    {
        #region Property
        /// <summary>소속 테이블 이름</summary>
        public string Table { get; protected set; }
        /// <summary>행의 고유 ID</summary>
        public string ID { get; protected set; }
        /// <summary>_id 컬럼 값을 반환한다. 없으면 null</summary>
        public object this[string _id]
        {
            get
            {
                if (m_Data.TryGetValue(_id, out var data))
                    return data;

                return null;
            }
        }
        #endregion
        #region Value
        protected Dictionary<string, object> m_Data = new Dictionary<string, object>();
        #endregion
    }
}