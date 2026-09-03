using Library;

namespace Game
{
    /// <summary>일반 적 Banana — 스탯은 Enemy 테이블, 행동은 자식 FSM 상태(Move·Attack·Die)가 맡는다</summary>
    public class Object_Enemy_Banana : Object_UnitBase
    {
        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        #endregion
    }
}
