using System;

namespace Game
{
    /// <summary>투사체 프리팹 스크립트 계약 — LocalBattleManager.Fire 가 풀에서 꺼낸 뒤 호출한다</summary>
    public interface IProjectile
    {
        /// <summary>_data 로 비행을 시작한다. 명중·소멸 시 LocalBattleManager.ReturnProjectile 로 되돌린다</summary>
        void Launch(SProjectile _data);
    }
}
