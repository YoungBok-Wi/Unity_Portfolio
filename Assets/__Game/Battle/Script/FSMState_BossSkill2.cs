using Library;
using UnityEngine;

namespace Game
{
    /// <summary>보스 보조 패턴 상태 — 전조 후 근접형(Charge)은 플레이어 방향으로 돌진해 접촉 판정, 원거리형(Rain)은 지정 구역에 낙하 판정을 내고 대기로 돌아간다</summary>
    public class FSMState_BossSkill2 : FSMState_UnitBase
    {
        #region Value
        private float m_Timer;
        private bool m_IsTelegraph;
        private bool m_Hit;
        private int m_Dir;
        private float m_StartX;
        private readonly System.Collections.Generic.List<Vector2> m_Areas = new();
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
            m_Hit = false;
            m_Dir = DirToPlayer;
            m_StartX = Unit.transform.position.x;
            m_Areas.Clear();
            var battle = LocalBattleManager.instance;
            if (data.AttackType == BattleConst.GroupMelee)
            {
                float length = data.Skill2TriggerDistance * 2f;
                battle.ShowTelegraph(Unit.HitPoint + Vector2.right * (m_Dir * length * 0.5f), new Vector2(length, BattleConst.HitBoxHeight), data.Skill2Telegraph);
                return;
            }
            int count = Unit.IsEnraged ? data.Skill2EnrageAreaCount : data.Skill2AreaCount;
            float playerX = HasPlayer ? Player.transform.position.x : Unit.transform.position.x;
            for (int i = 0; i < count; i++)
            {
                // 첫 구역은 플레이어 위치, 나머지는 좌우로 번갈아 한 구역 폭씩 벌린다
                float offset = (i + 1) / 2 * data.Skill2AreaWidth * 1.5f * ((i % 2 == 0) ? 1 : -1);
                var center = new Vector2(playerX + offset, Unit.HitPoint.y);
                m_Areas.Add(center);
                battle.ShowTelegraph(center, new Vector2(data.Skill2AreaWidth, BattleConst.HitBoxHeight * 2f), data.Skill2Telegraph);
            }
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
                if (m_Timer < data.Skill2Telegraph)
                    return this;
                m_IsTelegraph = false;
                m_Timer = 0;
                PlayAnim(BattleConst.AnimAttack2, false);
                if (data.AttackType == BattleConst.GroupMelee)
                    Unit.SetMoveSpeed(data.Skill2Speed);
            }

            int damage = Unit.ScaleAttack(data.Skill2Damage);
            if (data.AttackType == BattleConst.GroupMelee)
            {
                Move(m_Dir);
                if (!m_Hit)
                    m_Hit = 0 < HitBox(Unit.HitPoint, new Vector2(1.5f, BattleConst.HitBoxHeight), damage, 1);
                float moved = Mathf.Abs(Unit.transform.position.x - m_StartX);
                if (data.Skill2TriggerDistance * 2f <= moved || 1.5f <= m_Timer)
                {
                    Unit.SetMoveSpeed(Unit.IsEnraged ? data.EnrageMoveSpeed : data.MoveSpeed);
                    return Parent.GetState(BattleConst.StateIdle);
                }
                return this;
            }
            if (!m_Hit && AnimLength(BattleConst.AnimAttack2, 0.6f) * 0.5f <= m_Timer)
            {
                m_Hit = true;
                foreach (var center in m_Areas)
                    HitBox(center, new Vector2(data.Skill2AreaWidth, BattleConst.HitBoxHeight * 2f), damage, 1);
            }
            if (AnimLength(BattleConst.AnimAttack2, 0.6f) <= m_Timer)
                return Parent.GetState(BattleConst.StateIdle);
            return this;
        }
        #endregion
    }
}
