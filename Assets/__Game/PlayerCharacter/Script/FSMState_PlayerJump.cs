using Library;

namespace Game
{
    /// <summary>플레이어 점프를 시작하고 착지하면 대기로 전환한다.</summary>
    public class FSMState_PlayerJump : FSMState_PlayerBase
    {
        #region Event
        protected override void OnStart()
        {
            Player.Jump();
            Player.PlayAnim(UnitConst.AnimJump, false);
        }
        protected override FSMState OnUpdate()
        {
            var dead = CheckDead();
            if (dead != null)
                return dead;
            return Player.IsGrounded ? State(UnitConst.StateIdle) : this;
        }
        protected override FSMState OnFixedUpdate()
        {
            if (Player.CanControl)
                Player.Move(Player.MoveInput);
            return this;
        }
        #endregion
    }
}
