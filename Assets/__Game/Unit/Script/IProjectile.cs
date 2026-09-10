using System;

namespace Game
{
    /// <summary>투사체 프리팹이 구현하는 발사 계약.</summary>
    public interface IProjectile
    {
        /// <summary>`_data`로 비행을 시작하고 종료 시 `LocalGameManager` 풀로 돌아간다.</summary>
        void Launch(SProjectile _data);
    }
}
