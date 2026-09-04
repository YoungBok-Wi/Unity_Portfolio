namespace Game
{
    /// <summary>Battle 모듈 상수 — FSM 상태 ID·애니메이션 동작명·테이블 열거 문자열·재화 ID</summary>
    public static class BattleConst
    {
        #region Const
        public const string StateMove = "Move";
        public const string StateAttack = "Attack";
        public const string StateDie = "Die";
        public const string StateIdle = "Idle";
        public const string StateSkill1 = "Skill1";
        public const string StateSkill2 = "Skill2";
        public const string StateEnrage = "Enrage";
        public const string AnimIdle = "Idle";
        public const string AnimMove = "Move";
        public const string AnimAttack = "Attack";
        public const string AnimAttack1 = "Attack1";
        public const string AnimAttack2 = "Attack2";
        public const string AnimHit = "Hit";
        public const string AnimJump = "Jump";
        public const string AnimAttackKnife = "Attack_Knife";
        public const string AnimAttackKnife2 = "Attack2";
        public const string AnimAttackKnife3 = "Attack3";
        public const string AnimAttackGun = "Attack_Gun";
        public const string AnimDie = "Die";
        public const string GroupMelee = "Melee";
        public const string GroupTank = "Tank";
        public const string GroupRanged = "Ranged";
        public const string StackInstant = "Instant";
        public const string AbilityMultiHit = "MultiHit";
        public const string CrumbId = "Crumb";
        public const float MeleeWaitDistance = 3.0f;
        public const float HitBoxHeight = 1.5f;
        public const float BossProjectileRange = 15.0f;
        public const float PlayerKnockbackDist = 0.5f;
        public const float PlayerKnockbackTime = 0.15f;
        public const float RetreatBlockSec = 0.1f;
        public const float RetreatBlockSpeed = 0.05f;
        #endregion
    }
}
