using UnityEngine;

namespace Library
{
    /// <summary>테이블 행이 갖춰야 할 최소 형태. 행 타입을 몰라도 소속·ID·컬럼을 꺼낼 수 있게 한다</summary>
    public interface ITableType
    {
        /// <summary>소속 테이블 이름</summary>
        string Table { get; }

        /// <summary>행의 고유 ID</summary>
        string ID { get; }

        /// <summary>_id 컬럼 값을 반환한다. 없으면 null</summary>
        object this[string _id] { get; }
    }
}