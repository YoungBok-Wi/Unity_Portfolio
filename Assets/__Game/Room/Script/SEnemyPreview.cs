/* 방 선택지 미리보기 데이터 — 팝업이 아이콘·마릿수 표시에 쓴다 */

namespace Game
{
    /// <summary>다음 방 적 구성 미리보기 한 항목 — 유닛 ID(Enemy·Boss 테이블)와 마릿수</summary>
    public struct SEnemyPreview
    {
        #region Value
        public string Id;
        public int Count;
        #endregion

        #region Event
        public SEnemyPreview(string _id, int _count)
        {
            Id = _id;
            Count = _count;
        }
        #endregion
    }
}
