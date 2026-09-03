using Library;
using UnityEngine;

namespace Game
{
    /// <summary>Gun 요리사 플레이어 — 공격 입력 유지 중 제자리에서 전방으로 투사체를 연사한다 (첫 충돌 단일 명중, MultiHit 로 관통). 대기·이동은 Gun 전용 프레임(Idle_Gun·Move_Gun)을 쓴다</summary>
    public class Object_Player_Gun : Object_PlayerBase
    {
        #region Value
        private const string AnimIdleGun = "Idle_Gun";
        private const string AnimMoveGun = "Move_Gun";
        private float m_Timer;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        protected override string ResolveAnim(string _action)
        {
            if (_action == BattleConst.AnimIdle)
                return AnimIdleGun;
            if (_action == BattleConst.AnimMove)
                return AnimMoveGun;
            return _action;
        }
        protected override void UpdateAttack()
        {
            if (!AttackHeld)
            {
                if (IsAttacking)
                {
                    IsAttacking = false;
                    SetAttackRange(false);
                }
                return;
            }
            if (!IsAttacking)
            {
                IsAttacking = true;
                m_Timer = 0;
                StopMove();
                SetAttackRange(true);
                PlayAnim(BattleConst.AnimAttackGun, true);
            }
            m_Timer -= Time.deltaTime;
            if (0 < m_Timer)
                return;
            m_Timer = Battle != null ? Battle.GetPlayerAttackInterval(CharacterData.AttackInterval) : CharacterData.AttackInterval;
            Fire();
        }
        #endregion
        #region Local Function
        /// <summary>전방으로 투사체 1발을 발사한다</summary>
        private void Fire()
        {
            if (Battle == null)
                return;
            var data = CharacterData;
            var velocity = new Vector2(Facing * data.ProjectileSpeed, 0);
            Battle.Fire(new SProjectile(this, HitPoint, velocity, Battle.GetPlayerDamage(data.Attack1), data.Pierce, data.RangeWidth, data.KnockbackDist, data.KnockbackTime));
        }
        #endregion
    }
}
