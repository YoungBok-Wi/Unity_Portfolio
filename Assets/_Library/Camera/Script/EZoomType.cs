namespace Library
{
    /// <summary>카메라 줌 방식 구분</summary>
    public enum EZoomType
    {
        /// <summary>줌 없음</summary>
        None,
        /// <summary>카메라 자체 줌(직교 크기·시야각)으로 확대/축소</summary>
        CameraZoom,
        /// <summary>카메라 위치 이동으로 확대/축소</summary>
        PositionZoom,
    }
}
