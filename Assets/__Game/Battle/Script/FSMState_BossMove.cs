using Library;
using UnityEngine;

namespace Game
{
    /// <summary>보스 이동 상태 — 근접형은 Skill1 사거리까지 추격, 원거리형은 유지 거리를 지키며 후퇴·접근하고 자리를 잡으면 대기로 돌아간다</summary>
    public class FSMState_BossMove : FSMState_UnitBase
    {
        #region Value
        private float m_Timer;
        #endregion

        #region Event
        protected override void OnStart()
        {
            PlayAnim(BattleConst.AnimMove, true);
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
                return Parent.GetState(BattleConst.StateIdle);

            // 한 번의 이동은 최대 3초 — 자리를 못 잡아도 대기로 돌아가 패턴을 다시 고른다
            m_Timer += Time.deltaTime;
            if (3f < m_Timer)
                return Parent.GetState(BattleConst.StateIdle);

            float dist = DistX;
            int dir = DirToPlayer;
            if (data.AttackType == BattleConst.GroupMelee)
            {
                if (data.Skill1Range * 0.8f < dist)
                {
                    Move(dir);
                    return this;
                }
                return Parent.GetState(BattleConst.StateIdle);
            }
            if (dist < data.HoldDistance * 0.7f)
                Move(-dir);
            else if (data.HoldDistance * 1.3f < dist)
                Move(dir);
            else
                return Parent.GetState(BattleConst.StateIdle);
            return this;
        }
        #endregion
    }
}
