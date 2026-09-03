using Library;
using UnityEngine;

namespace Game
{
    /// <summary>적 공격 상태 — 그룹별로 근접 박스·탱커 돌진 박치기·원거리 투사체를 애니메이션 중간에 발동하고 공격 주기가 지나면 이동으로 돌아간다</summary>
    public class FSMState_EnemyAttack : FSMState_UnitBase
    {
        #region Value
        private float m_Timer;
        private float m_AnimLen;
        private bool m_Fired;
        #endregion

        #region Event
        protected override void OnStart()
        {
            FacePlayer();
            PlayAnim(BattleConst.AnimAttack, false);
            m_AnimLen = AnimLength(BattleConst.AnimAttack, 0.5f);
            m_Timer = 0;
            m_Fired = false;
        }
        protected override FSMState OnUpdate()
        {
            var die = CheckDie();
            if (die != null)
                return die;
            if (Unit.IsStunned)
                return Parent.GetState(BattleConst.StateMove);

            var data = Unit.EnemyData;
            m_Timer += Time.deltaTime;
            if (!m_Fired && data.Group == BattleConst.GroupTank)
                Move(Unit.Facing);
            if (!m_Fired && m_AnimLen * 0.5f <= m_Timer)
            {
                m_Fired = true;
                Fire(data);
            }
            if (Mathf.Max(m_AnimLen, data.AttackInterval) <= m_Timer)
                return Parent.GetState(BattleConst.StateMove);
            return this;
        }
        #endregion
        #region Local Function
        /// <summary>그룹에 맞는 판정을 한 번 낸다 — 원거리는 투사체, 그 외는 전방 박스</summary>
        private void Fire(EnemyTable _data)
        {
            int damage = Unit.ScaleAttack(_data.Attack);
            if (_data.Group == BattleConst.GroupRanged)
            {
                var velocity = new Vector2(Unit.Facing * _data.ProjectileSpeed, 0);
                LocalBattleManager.instance.Fire(new SProjectile(Unit, Unit.HitPoint, velocity, damage, 0, _data.Range, 0, 0));
                return;
            }
            float reach = _data.StopDistance + _data.HitboxWidth;
            var center = Unit.HitPoint + Vector2.right * (Unit.Facing * reach * 0.5f);
            HitBox(center, new Vector2(reach, BattleConst.HitBoxHeight), damage, 1);
        }
        #endregion
    }
}
