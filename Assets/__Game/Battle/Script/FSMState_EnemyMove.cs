using Library;
using UnityEngine;

namespace Game
{
    /// <summary>적 이동 상태 — 근접·탱커는 슬롯을 얻어 정지 거리까지 추격, 원거리는 정지 거리를 유지하다 사거리 안이면 공격으로 전환 (후퇴가 막히면 유지 거리 미달이어도 발사)</summary>
    public class FSMState_EnemyMove : FSMState_UnitBase
    {
        #region Value
        private float m_RetreatSec;
        #endregion

        #region Event
        protected override void OnStart()
        {
            PlayAnim(BattleConst.AnimMove, true);
            m_RetreatSec = 0;
        }
        protected override FSMState OnUpdate()
        {
            var die = CheckDie();
            if (die != null)
                return die;
            if (Unit.IsStunned || !HasPlayer)
                return this;

            var data = Unit.EnemyData;
            float dist = DistX;
            int dir = DirToPlayer;
            if (data.Group == BattleConst.GroupRanged)
            {
                if (dist < data.StopDistance * 0.7f)
                {
                    Move(-dir);
                    m_RetreatSec += Time.deltaTime;
                    // 후퇴가 벽에 막혀 서 있으면 유지 거리 미달이어도 사거리 안에서 발사한다 — 후퇴와 발사는 배타가 아니다
                    if (dist <= data.Range && IsRetreatBlocked())
                        return Parent.GetState(BattleConst.StateAttack);
                    return this;
                }
                m_RetreatSec = 0;
                if (data.StopDistance < dist)
                    Move(dir);
                else
                {
                    FacePlayer();
                    if (dist <= data.Range)
                        return Parent.GetState(BattleConst.StateAttack);
                }
                return this;
            }

            // 근접 슬롯이 없으면 대기 거리에서 기다린다 — 좌우 슬롯 수는 Battle_MeleeSlotPerSide
            if (!LocalBattleManager.instance.RequestMeleeSlot(Unit, -dir))
            {
                if (BattleConst.MeleeWaitDistance < dist)
                    Move(dir);
                else
                    FacePlayer();
                return this;
            }
            if (data.StopDistance < dist)
            {
                Move(dir);
                return this;
            }
            FacePlayer();
            return Parent.GetState(BattleConst.StateAttack);
        }
        #endregion
        #region Local Function
        /// <summary>후퇴 요청이 RetreatBlockSec 이상 이어졌는데 수평 속도가 RetreatBlockSpeed 미만이면 true 를 반환한다 (물리가 없으면 false)</summary>
        private bool IsRetreatBlocked()
        {
            return Unit.Physics != null && BattleConst.RetreatBlockSec <= m_RetreatSec && Mathf.Abs(Unit.Physics.Rig.linearVelocity.x) < BattleConst.RetreatBlockSpeed;
        }
        #endregion
    }
}
