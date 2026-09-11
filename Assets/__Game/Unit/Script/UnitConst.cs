namespace Game
{
    /// <summary>유닛 공통 상태·애니메이션·행동 상수.</summary>
    public static class UnitConst
    {
        #region Const
        public const string StateIdle = "Idle";
        public const string StateMove = "Move";
        public const string StateJump = "Jump";
        public const string StateAttack = "Attack";
        public const string StateHit = "Hit";
        public const string StateKnockback = "Knockback";
        public const string StateDie = "Die";
        public const string StateSkill1 = "Skill1";
        public const string StateSkill2 = "Skill2";
        public const string StateEnrage = "Enrage";
        public const string AnimIdle = "Idle";
        public const string AnimMove = "Move";
        public const string AnimJump = "Jump";
        public const string AnimAttack = "Attack";
        public const string AnimAttack1 = "Attack1";
        public const string AnimAttack2 = "Attack2";
        public const string AnimHit = "Hit";
        public const string AnimDie = "Die";
        public const string AnimAttackKnife = "Attack_Knife";
        public const string AnimAttackKnife2 = "Attack2";
        public const string AnimAttackKnife3 = "Attack3";
        public const string AnimAttackGun = "Attack_Gun";
        public const string GroupMelee = "Melee";
        public const string GroupTank = "Tank";
        public const string GroupRanged = "Ranged";
        public const float MeleeWaitDistance = 3f;
        public const float HitBoxHeight = 1.5f;
        public const float BossProjectileRange = 15f;
        public const float RetreatBlockSec = 0.1f;
        public const float RetreatBlockSpeed = 0.05f;
        #endregion
    }
}
