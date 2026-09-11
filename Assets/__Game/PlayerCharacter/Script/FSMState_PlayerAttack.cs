using Library;
using UnityEngine;

namespace Game
{
    /// <summary>공격 간격 뒤 콤보 입력창을 열고 Knife 콤보 또는 Gun 연사를 진행한다.</summary>
    public class FSMState_PlayerAttack : FSMState_PlayerBase
    {
        #region Value
        private int m_Step;
        private float m_Timer;
        private float m_Interval;
        #endregion

        #region Event
        protected override void OnStart()
        {
            m_Step = 1;
            StartStep();
        }
        protected override FSMState OnUpdate()
        {
            var dead = CheckDead();
            if (dead != null)
                return dead;
            if (Player.IsStunned)
            {
                Player.FinishAttack();
                return State(UnitConst.StateHit);
            }
            m_Timer += Time.deltaTime;
            Player.TickAttack(m_Step);
            if (!Player.IsAttackCommitted)
                Player.ConsumeJump();
            else if (Player.CanControl)
            {
                if (Player.JumpPressed && Player.IsGrounded)
                    return State(UnitConst.StateJump);
                if (Player.MoveInput != 0f)
                    return State(UnitConst.StateMove);
            }
            if (m_Timer < m_Interval)
                return this;
            float comboWindow = Mathf.Max(0f, Player.CharacterData.ComboWindow);
            if (comboWindow <= 0f)
            {
                if (Player.AttackHeld)
                {
                    StartStep();
                    return this;
                }
                return Finish();
            }
            if (Player.AttackPressed)
            {
                m_Step = m_Step % 3 + 1;
                StartStep();
                return this;
            }
            return m_Interval + comboWindow <= m_Timer ? Finish() : this;
        }
        protected override void OnEnd()
        {
            if (Player.IsAttacking)
                Player.FinishAttack();
        }
        #endregion
        #region Local Function
        private void StartStep()
        {
            m_Timer = 0f;
            m_Interval = Mathf.Max(0.01f, Player.AttackInterval());
            Player.BeginAttack(m_Step);
        }
        private FSMState Finish()
        {
            Player.FinishAttack();
            return State(UnitConst.StateIdle);
        }
        #endregion
    }
}
