using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>넉백 곡선 이동과 종료 후 스턴을 소유하고 정상 상태로 복귀한다.</summary>
    public class FSMState_UnitKnockback : FSMState
    {
        #region Property
        private Object_UnitBase Unit { get; set; }
        #endregion
        #region Value
        private float m_Elapsed;
        private float m_Time;
        private float m_Distance;
        private float m_Applied;
        private float m_StunTimer;
        private int m_Direction;
        #endregion

        #region Event
        protected override void OnInit()
        {
            Unit = GetComponentInParent<Object_UnitBase>();
            if (Unit == null)
                throw new InvalidOperationException($"{name} : 상위에 Object_UnitBase가 없다");
        }
        protected override void OnStart()
        {
            Begin();
            if (Unit.Anim != null)
                Unit.Anim.Play(UnitConst.AnimHit, false);
        }
        protected override FSMState OnUpdate()
        {
            if (Unit.IsDead.v)
                return Parent.GetState(UnitConst.StateDie);
            if (m_Elapsed < m_Time)
                return this;
            m_StunTimer -= Time.deltaTime;
            return 0f < m_StunTimer ? this : Parent.GetState(Unit.KnockbackReturnState);
        }
        protected override FSMState OnFixedUpdate()
        {
            if (Unit.Physics == null || m_Time <= m_Elapsed)
                return this;
            float nextElapsed = Mathf.Min(m_Elapsed + Time.fixedDeltaTime, m_Time);
            float progress = nextElapsed / m_Time;
            var curve = LocalGameManager.instance != null ? LocalGameManager.instance.KnockbackCurve : null;
            float normalized = curve != null ? curve.Evaluate(progress) : progress;
            float targetDistance = m_Distance * normalized;
            float delta = Mathf.Max(0f, targetDistance - m_Applied);
            Unit.Physics.MoveSpeed.v = delta / Time.fixedDeltaTime;
            Unit.Physics.Move(m_Direction, true);
            m_Applied = targetDistance;
            m_Elapsed = nextElapsed;
            if (m_Time <= m_Elapsed)
                m_StunTimer = LocalGameManager.instance != null ? LocalGameManager.instance.KnockbackStunSec : 0f;
            return this;
        }
        protected override void OnEnd()
        {
            Unit.RestoreMoveSpeed();
            Unit.StopHorizontal();
        }
        #endregion
        #region Local Function
        private void Begin()
        {
            if (!Unit.TryConsumeKnockback(out m_Time, out m_Distance, out m_Direction))
                throw new InvalidOperationException($"{name} : 소비할 넉백이 없다");
            m_Elapsed = 0f;
            m_Applied = 0f;
            m_StunTimer = 0f;
            Unit.StopHorizontal();
        }
        #endregion
        #region Function
        /// <summary>대기 중인 넉백을 다시 소비해 현재 상태를 처음부터 시작한다.</summary>
        public void Restart()
        {
            Begin();
        }
        #endregion
    }
}
