using Library;

namespace Game
{
    /// <summary>씬 상주 플레이어를 제거하지 않고 사망 모션 상태로 유지한다.</summary>
    public class FSMState_PlayerDie : FSMState_PlayerBase
    {
        #region Event
        protected override void OnStart()
        {
            Player.StopMove();
            Player.PlayAnim(UnitConst.AnimDie, false);
        }
        #endregion
    }
}
