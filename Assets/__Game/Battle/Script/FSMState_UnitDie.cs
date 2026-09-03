using Library;
using UnityEngine;

namespace Game
{
    /// <summary>사망 상태 — 멈추고 Die 애니메이션 길이만큼 기다린 뒤 풀로 되돌린다 (적·보스 공용)</summary>
    public class FSMState_UnitDie : FSMState_UnitBase
    {
        #region Value
        private float m_Timer;
        #endregion

        #region Event
        protected override void OnStart()
        {
            if (Unit.Physics != null)
                Unit.Physics.SetVelocity(Vector2.zero);
            LocalBattleManager.instance.ReleaseMeleeSlot(Unit);
            m_Timer = AnimLength(BattleConst.AnimDie, 0.5f);
        }
        protected override FSMState OnUpdate()
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0)
                LocalBattleManager.instance.Despawn(Unit);
            return this;
        }
        #endregion
    }
}
