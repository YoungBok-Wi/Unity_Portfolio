using Library;

namespace Game
{
    /// <summary>플레이어 대기 상태에서 입력에 따라 이동·점프·공격으로 전환한다.</summary>
    public class FSMState_PlayerIdle : FSMState_PlayerBase
    {
        #region Event
        protected override void OnStart()
        {
            Player.StopMove();
            Player.PlayAnim(UnitConst.AnimIdle, true);
        }
        protected override FSMState OnUpdate()
        {
            var dead = CheckDead();
            if (dead != null || !Player.CanControl)
                return dead ?? this;
            if (Player.JumpPressed && Player.IsGrounded)
                return State(UnitConst.StateJump);
            if (Player.AttackPressed && Player.IsAttackReady())
                return State(UnitConst.StateAttack);
            return Player.MoveInput != 0f ? State(UnitConst.StateMove) : this;
        }
        #endregion
    }
}
