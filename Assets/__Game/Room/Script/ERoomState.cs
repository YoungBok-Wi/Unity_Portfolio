namespace Game
{
    /// <summary>방 진행 상태 — 방 안 진행 중, 다음 방 선택 중, 런 종료</summary>
    public enum ERoomState
    {
        None,
        Playing,
        Choosing,
        Ended,
    }
}
