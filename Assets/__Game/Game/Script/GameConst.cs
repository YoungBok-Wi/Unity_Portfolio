namespace Game
{
    /// <summary>전투 매니저의 능력·풀·넉백·보상 연출 상수.</summary>
    public static class GameConst
    {
        #region Const
        public const string StackInstant = "Instant";
        public const string AbilityMultiHit = "MultiHit";
        public const float PlayerKnockbackDist = 0.5f;
        public const float PlayerKnockbackTime = 0.15f;
        public const float PlayerKnockbackDriftMax = 1.5f;
        public const int CrumbDropMax = 8;
        public const int CrumbDropPoolSize = 32;
        public const int EffectPoolSize = 16;
        public const float CrumbTossSpeedX = 2f;
        public const float CrumbTossSpeedY = 4f;
        public const float CrumbGravity = 12f;
        public const float CrumbCollectDistance = 0.6f;
        public const float CrumbMagnetDistance = 3f;
        public const float CrumbMagnetSpeed = 8f;
        #endregion
    }
}
