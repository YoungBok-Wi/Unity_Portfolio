using Library;
using System;

namespace Game
{
    /// <summary>Enemy 테이블 스탯과 일반 적 공통 계약을 제공한다.</summary>
    public abstract class Object_EnemyBase : Object_UnitBase
    {
        #region Property
        /// <summary>플레이어 진영이 아님을 반환한다.</summary>
        public override bool IsPlayerSide => false;
        /// <summary>Enemy 테이블의 넉백 배율을 반환한다.</summary>
        public override float KnockbackRate => EnemyData != null ? EnemyData.KnockbackRate : 1f;
        /// <summary>Enemy 테이블의 Crumb 드롭량을 반환한다.</summary>
        public override int CrumbDrop => EnemyData != null ? EnemyData.CrumbDrop : 0;
        /// <summary>로드한 Enemy 테이블 행을 반환한다.</summary>
        public EnemyTable EnemyData => m_EnemyData;
        /// <summary>스폰 직후 Move 상태로 진입한다.</summary>
        protected override string SpawnState => UnitConst.StateMove;
        #endregion
        #region Value
        private EnemyTable m_EnemyData;
        #endregion

        #region Event
        protected override (int hp, float moveSpeed) LoadBase()
        {
            if (!TableManager.instance.Enemy.Data.TryGetValue(Id, out var data))
                throw new ArgumentException($"{name} : Enemy 테이블에 없는 ID {Id}");
            m_EnemyData = data;
            return (data.Hp, data.MoveSpeed);
        }
        #endregion
    }
}
