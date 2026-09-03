using Library;
using UnityEngine;

namespace Game
{
    /// <summary>보스 대기 상태 — 패턴 간격을 기다린 뒤 거리·보조 패턴 주기로 Skill1·Skill2·Move 를 고르고, HP 비율 도달 시 Enrage 로 넘어간다</summary>
    public class FSMState_BossIdle : FSMState_UnitBase
    {
        #region Value
        private float m_Timer;
        private float m_Skill2Timer;
        #endregion

        #region Event
        protected override void OnStart()
        {
            PlayAnim(BattleConst.AnimIdle, true);
            if (Unit.Physics != null)
                Unit.Physics.SetVelocity(Vector2.zero);
            m_Timer = 0;
        }
        protected override FSMState OnUpdate()
        {
            var die = CheckDie();
            if (die != null)
                return die;
            var data = Unit.BossData;
            if (!Unit.IsEnraged && Unit.Hp.v <= Unit.MaxHp.v * data.EnrageHpRatio)
                return Parent.GetState(BattleConst.StateEnrage);
            if (!HasPlayer)
                return this;

            FacePlayer();
            m_Timer += Time.deltaTime;
            m_Skill2Timer += Time.deltaTime;
            float interval = Unit.IsEnraged ? data.Skill1EnrageInterval : data.Skill1Interval;
            if (m_Timer < interval)
                return this;

            float dist = DistX;
            bool isMelee = data.AttackType == BattleConst.GroupMelee;
            bool skill2Ready = data.Skill2Interval <= m_Skill2Timer;
            if (skill2Ready && (!isMelee || data.Skill2TriggerDistance <= dist))
            {
                m_Skill2Timer = 0;
                return Parent.GetState(BattleConst.StateSkill2);
            }
            if (!isMelee || dist <= data.Skill1Range)
                return Parent.GetState(BattleConst.StateSkill1);
            return Parent.GetState(BattleConst.StateMove);
        }
        #endregion
    }
}
