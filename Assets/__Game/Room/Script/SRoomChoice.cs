/* 방 선택지 데이터 — LocalRoomManager.Choices 로 팝업에 전달된다 */

namespace Game
{
    /// <summary>방 선택지 한 칸 — 방 종류(Room 테이블 ID), 보스방이면 보스 ID, 적 구성 미리보기</summary>
    public struct SRoomChoice
    {
        #region Value
        public string Kind;
        public string BossId;
        public SEnemyPreview[] Enemies;
        #endregion

        #region Event
        public SRoomChoice(string _kind, string _bossId, SEnemyPreview[] _enemies)
        {
            Kind = _kind;
            BossId = _bossId;
            Enemies = _enemies;
        }
        #endregion
    }
}
