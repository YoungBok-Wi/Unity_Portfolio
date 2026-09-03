using Library;
using UnityEngine;

namespace Game
{
    /// <summary>보스 주 패턴 상태 — 전조 후 근접형(Slam)은 전방 광역 박스, 원거리형(Spike)은 투사체 연발을 발동하고 대기로 돌아간다</summary>
    public class FSMState_BossSkill1 : FSMState_UnitBase
    {
        #region Value
        private float m_Timer;
        private float m_AnimLen;
        private bool m_IsTelegraph;
        private int m_Count;
        private float m_CountTimer;
        #endregion

        #region Event
        protected override void OnStart()
        {
            FacePlayer();
            if (Unit.Physics != null)
                Unit.Physics.SetVelocity(Vector2.zero);
            PlayAnim(BattleConst.AnimIdle, true);
            var data = Unit.BossData;
            m_Timer = 0;
            m_IsTelegraph = true;
            m_Count = 0;
            m_CountTimer = 0;
            if (data.AttackType == BattleConst.GroupMelee)
                LocalBattleManager.instance.ShowTelegraph(new Vector2(AreaCenter(data).x, Unit.transform.position.y), data.Skill1Range, data.Skill1Telegraph);
        }
        protected override FSMState OnUpdate()
        {
            var die = CheckDie();
            if (die != null)
                return die;
            var data = Unit.BossData;
            m_Timer += Time.deltaTime;
            if (m_IsTelegraph)
            {
                if (m_Timer < data.Skill1Telegraph)
                    return this;
                m_IsTelegraph = false;
                m_Timer = 0;
                PlayAnim(BattleConst.AnimAttack1, false);
                m_AnimLen = AnimLength(BattleConst.AnimAttack1, 0.6f);
            }

            int damage = Unit.ScaleAttack(data.Skill1Damage);
            if (data.AttackType == BattleConst.GroupMelee)
            {
                if (m_Count == 0 && m_AnimLen * 0.5f <= m_Timer)
                {
                    m_Count = 1;
                    HitBox(AreaCenter(data), AreaSize(data), damage, 1);
                }
            }
            else
            {
                m_CountTimer -= Time.deltaTime;
                if (m_Count < data.Skill1Count && m_CountTimer <= 0)
                {
                    m_Count += 1;
                    m_CountTimer = data.Skill1CountInterval;
                    var velocity = new Vector2(Unit.Facing * data.Skill1ProjectileSpeed, 0);
                    LocalBattleManager.instance.Fire(new SProjectile(Unit, Unit.HitPoint, velocity, damage, 0, BattleConst.BossProjectileRange, 0, 0));
                }
            }
            if (m_AnimLen <= m_Timer && (data.AttackType == BattleConst.GroupMelee || data.Skill1Count <= m_Count))
                return Parent.GetState(BattleConst.StateIdle);
            return this;
        }
        #endregion
        #region Local Function
        /// <summary>전방 광역 범위의 중심을 반환한다</summary>
        private Vector2 AreaCenter(BossTable _data)
        {
            return Unit.HitPoint + Vector2.right * (Unit.Facing * _data.Skill1Range * 0.5f);
        }
        /// <summary>전방 광역 범위의 크기를 반환한다</summary>
        private Vector2 AreaSize(BossTable _data)
        {
            return new Vector2(_data.Skill1Range, BattleConst.HitBoxHeight);
        }
        #endregion
    }
}
