using Library;
using UnityEngine;

namespace Game
{
    /// <summary>보스 Enrage 전환 상태 — 이동속도를 Enrage 값으로 올리고 잠시 멈춘 뒤 이동으로 복귀한다 (1회만 진입)</summary>
    public class FSMState_BossEnrage : FSMState_UnitBase
    {
        #region Property
        public override bool IsEnable => !Unit.IsEnraged;
        #endregion
        #region Value
        private float m_Timer;
        #endregion

        #region Event
        protected override void OnStart()
        {
            Unit.SetEnraged();
            Unit.SetMoveSpeed(Unit.BossData.EnrageMoveSpeed);
            if (Unit.Physics != null)
                Unit.Physics.SetVelocity(Vector2.zero);
            PlayAnim(BattleConst.AnimIdle, true);
            m_Timer = 0.5f;
        }
        protected override FSMState OnUpdate()
        {
            var die = CheckDie();
            if (die != null)
                return die;
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0)
                return Parent.GetState(BattleConst.StateMove);
            return this;
        }
        #endregion
    }
}
