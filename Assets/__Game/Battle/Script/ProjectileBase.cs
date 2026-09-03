using Library;
using UnityEngine;

namespace Game
{
    /// <summary>투사체 프리팹 스크립트의 베이스 — IProjectile 계약을 모듈이 소유하고 비행 구현은 파생이 맡는다</summary>
    public abstract class ProjectileBase : ObjectBase, IProjectile
    {
        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        #endregion
        #region Function
        /// <summary>_data 로 비행을 시작한다 (IProjectile 계약, 파생이 구현)</summary>
        public abstract void Launch(SProjectile _data);
        #endregion
    }
}
