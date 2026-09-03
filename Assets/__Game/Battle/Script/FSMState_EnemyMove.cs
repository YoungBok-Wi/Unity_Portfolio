using Library;
using UnityEngine;

namespace Game
{
    /// <summary>적 이동 상태 — 근접·탱커는 슬롯을 얻어 정지 거리까지 추격, 원거리는 정지 거리를 유지하다 사거리 안이면 공격으로 전환</summary>
    public class FSMState_EnemyMove : FSMState_UnitBase
    {
        #region Event
        protected override void OnStart()
        {
            PlayAnim(BattleConst.AnimMove, true);
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
                    Move(-dir);
                else if (data.StopDistance < dist)
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
    }
}
