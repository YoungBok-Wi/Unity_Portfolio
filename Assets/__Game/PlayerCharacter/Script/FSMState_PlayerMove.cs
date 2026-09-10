using Library;

namespace Game
{
    /// <summary>플레이어 좌우 이동 상태에서 입력과 방 경계를 적용한다.</summary>
    public class FSMState_PlayerMove : FSMState_PlayerBase
    {
        #region Event
        protected override void OnStart()
        {
            Player.PlayAnim(UnitConst.AnimMove, true);
        }
        protected override FSMState OnUpdate()
        {
            var dead = CheckDead();
            if (dead != null || !Player.CanControl)
                return dead ?? State(UnitConst.StateIdle);
            if (Player.JumpPressed && Player.IsGrounded)
                return State(UnitConst.StateJump);
            if (Player.AttackPressed && Player.IsAttackReady())
                return State(UnitConst.StateAttack);
            return Player.MoveInput == 0f ? State(UnitConst.StateIdle) : this;
        }
        protected override FSMState OnFixedUpdate()
        {
            if (Player.CanControl)
                Player.Move(Player.MoveInput);
            return this;
        }
        protected override void OnEnd()
        {
            Player.StopMove();
        }
        #endregion
    }
}
