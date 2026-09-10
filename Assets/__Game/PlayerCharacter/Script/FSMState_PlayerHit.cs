using Library;

namespace Game
{
    /// <summary>피격 모션과 경직이 끝날 때까지 입력을 막는다.</summary>
    public class FSMState_PlayerHit : FSMState_PlayerBase
    {
        #region Event
        protected override void OnStart()
        {
            Player.StopMove();
            Player.PlayAnim(UnitConst.AnimHit, false);
        }
        protected override FSMState OnUpdate()
        {
            var dead = CheckDead();
            if (dead != null)
                return dead;
            bool animDone = Player.Anim == null || Player.Anim.IsFinished;
            return !Player.IsStunned && animDone ? State(UnitConst.StateIdle) : this;
        }
        #endregion
    }
}
