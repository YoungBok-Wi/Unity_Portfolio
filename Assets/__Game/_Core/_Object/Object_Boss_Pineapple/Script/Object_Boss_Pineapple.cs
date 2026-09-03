using Library;

namespace Game
{
    /// <summary>보스 Pineapple — 스탯·패턴 값은 Boss 테이블, 행동은 자식 FSM 상태(Idle·Move·Skill1·Skill2·Enrage·Die)가 맡는다</summary>
    public class Object_Boss_Pineapple : Object_UnitBase
    {
        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        #endregion
    }
}
