using UnityEngine;

namespace Game
{
    /// <summary>Knife 공격 단계별 모션·두 번째 프레임 판정·마무리 넉백을 구현한다.</summary>
    public class Object_Player_Knife : Object_PlayerBase
    {
        #region Value
        private bool m_HitDone;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        protected override void OnAttackStart(int _step)
        {
            m_HitDone = false;
            PlayAnim(_step == 1 ? UnitConst.AnimAttackKnife : (_step == 2 ? UnitConst.AnimAttackKnife2 : UnitConst.AnimAttackKnife3), false);
            if (Game != null)
            {
                Game.PlayAttackSfx();
                Game.PlaySlashEffect(GetAttackBox().center, Facing, _step);
            }
        }
        protected override void OnAttackFrame(int _step, int _frame)
        {
            if (m_HitDone || _frame != 1 || Game == null)
                return;
            m_HitDone = true;
            var data = CharacterData;
            bool isFinish = _step == 3;
            int baseDamage = _step == 1 ? data.Attack1 : (_step == 2 ? data.Attack2 : data.Attack3);
            var (center, size) = GetAttackBox();
            Game.HitBox(this, center, size, Game.GetPlayerDamage(baseDamage), data.HitMax,
                isFinish ? data.KnockbackDistFinish : data.KnockbackDist, isFinish ? data.KnockbackTimeFinish : data.KnockbackTime, isFinish, _step);
        }
        protected override void OnAttackEnd()
        {
        }
        #endregion
    }
}
