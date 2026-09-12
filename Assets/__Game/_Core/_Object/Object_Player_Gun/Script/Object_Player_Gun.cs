using UnityEngine;

namespace Game
{
    /// <summary>Gun 요리사 플레이어 — 공격 입력 유지 중 제자리에서 전방으로 투사체를 연사하며 대기·이동·점프 중 Gun 외형을 유지한다.</summary>
    public class Object_Player_Gun : Object_PlayerBase
    {
        #region Value
        private const string AnimIdleGun = "Idle_Gun";
        private const string AnimMoveGun = "Move_Gun";
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        protected override string ResolveAnim(string _action)
        {
            if (_action == UnitConst.AnimIdle)
                return AnimIdleGun;
            if (_action == UnitConst.AnimMove)
                return AnimMoveGun;
            // 단순화: Jump_Gun 리소스가 추가되면 전용 액션으로 교체한다.
            if (_action == UnitConst.AnimJump)
                return AnimIdleGun;
            return _action;
        }
        protected override void OnAttackStart(int _step)
        {
            PlayAnim(UnitConst.AnimAttackGun, true);
            Fire();
        }
        protected override void OnAttackFrame(int _step, int _frame)
        {
        }
        protected override void OnAttackEnd()
        {
        }
        #endregion
        #region Local Function
        /// <summary>전방으로 투사체 1발을 발사한다</summary>
        private void Fire()
        {
            if (Game == null)
                return;
            var data = CharacterData;
            var velocity = new Vector2(Facing * data.ProjectileSpeed, 0);
            Game.PlayAttackSfx();
            Game.Fire(new SProjectile(this, HitPoint, velocity, Game.GetPlayerDamage(data.Attack1), data.Pierce, data.RangeWidth, data.KnockbackDist, data.KnockbackTime));
            CommitAttack();
        }
        #endregion
    }
}
