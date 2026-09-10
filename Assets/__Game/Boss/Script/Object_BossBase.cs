using Library;
using System;

namespace Game
{
    /// <summary>Boss 테이블 스탯과 넉백 면역·강화 상태를 제공한다.</summary>
    public abstract class Object_BossBase : Object_EnemyBase
    {
        #region Property
        /// <summary>로드한 Boss 테이블 행을 반환한다.</summary>
        public BossTable BossData => m_BossData;
        /// <summary>강화 상태 여부를 반환한다.</summary>
        public bool IsEnraged => m_IsEnraged;
        /// <summary>보스가 넉백 면역임을 반환한다.</summary>
        public override bool IsKnockbackImmune => true;
        /// <summary>보스의 넉백 거리 배율 1을 반환한다.</summary>
        public override float KnockbackRate => 1f;
        /// <summary>Boss 테이블의 Crumb 드롭량을 반환한다.</summary>
        public override int CrumbDrop => BossData != null ? BossData.CrumbDrop : 0;
        /// <summary>스폰 직후 Idle 상태로 진입한다.</summary>
        protected override string SpawnState => UnitConst.StateIdle;
        #endregion
        #region Value
        private BossTable m_BossData;
        private bool m_IsEnraged;
        #endregion

        #region Event
        protected override (int hp, float moveSpeed) LoadBase()
        {
            if (!TableManager.instance.Boss.Data.TryGetValue(Id, out var data))
                throw new ArgumentException($"{name} : Boss 테이블에 없는 ID {Id}");
            m_BossData = data;
            m_IsEnraged = false;
            return (data.Hp, data.MoveSpeed);
        }
        #endregion
        #region Function
        /// <summary>보스를 강화 상태로 바꾼다.</summary>
        public void SetEnraged()
        {
            m_IsEnraged = true;
        }
        #endregion
    }
}
