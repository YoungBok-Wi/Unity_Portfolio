using Library;
using UnityEngine;

namespace Game
{
    /// <summary>Knife 요리사 플레이어 — 정지 후 전방 사각 박스 3단 콤보 (모션 후반 선입력으로 연결, 3단은 마무리 넉백)</summary>
    public class Object_Player_Knife : Object_PlayerBase
    {
        #region Value
        private int m_Step;
        private float m_Timer;
        private float m_Interval;
        private bool m_HitDone;
        private bool m_Buffered;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        protected override void UpdateAttack()
        {
            if (!IsAttacking)
            {
                if (AttackPressed)
                    StartStep(1);
                return;
            }
            m_Timer += Time.deltaTime;
            if (AttackPressed && m_Step < 3 && m_Interval - CharacterData.InputBuffer <= m_Timer)
                m_Buffered = true;
            if (!m_HitDone && m_Interval * 0.5f <= m_Timer)
            {
                m_HitDone = true;
                Strike();
            }
            if (m_Timer < m_Interval)
                return;
            if (m_Buffered)
                StartStep(m_Step + 1);
            else
                EndAttack();
        }
        #endregion
        #region Local Function
        /// <summary>_step 단 공격을 시작한다 — 제자리 정지·모션 재생·판정 범위 활성</summary>
        private void StartStep(int _step)
        {
            IsAttacking = true;
            m_Step = _step;
            m_Timer = 0;
            m_HitDone = false;
            m_Buffered = false;
            m_Interval = Battle != null ? Battle.GetPlayerAttackInterval(CharacterData.AttackInterval) : CharacterData.AttackInterval;
            StopMove();
            SetAttackRange(true);
            PlayAnim(_step == 1 ? BattleConst.AnimAttackKnife : (_step == 2 ? BattleConst.AnimAttackKnife2 : BattleConst.AnimAttackKnife3), false);
            if (Battle != null)
            {
                Battle.PlayAttackSfx();
                Battle.PlaySlashEffect(GetAttackBox().center, Facing);
            }
        }
        /// <summary>현재 단의 판정을 낸다 (3단은 마무리 넉백)</summary>
        private void Strike()
        {
            if (Battle == null)
                return;
            var data = CharacterData;
            bool isFinish = m_Step == 3;
            int baseDamage = m_Step == 1 ? data.Attack1 : (m_Step == 2 ? data.Attack2 : data.Attack3);
            var (center, size) = GetAttackBox();
            Battle.HitBox(this, center, size, Battle.GetPlayerDamage(baseDamage), data.HitMax,
                isFinish ? data.KnockbackDistFinish : data.KnockbackDist, isFinish ? data.KnockbackTimeFinish : data.KnockbackTime, isFinish);
        }
        /// <summary>콤보를 끝내고 이동 가능 상태로 돌아간다</summary>
        private void EndAttack()
        {
            IsAttacking = false;
            m_Step = 0;
            SetAttackRange(false);
        }
        #endregion
    }
}
