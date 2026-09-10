using System;

namespace Game
{
    /// <summary>방 종류·보스·웨이브 변형·적 미리보기를 전달한다.</summary>
    [Serializable]
    public struct SRoomChoice
    {
        #region Value
        public string Kind;
        public string BossId;
        public int Variant;
        public SEnemyPreview[] Enemies;
        #endregion

        #region Event
        public SRoomChoice(string _kind, string _bossId, int _variant, SEnemyPreview[] _enemies)
        {
            Kind = _kind;
            BossId = _bossId;
            Variant = _variant;
            Enemies = _enemies;
        }
        #endregion
    }
}
