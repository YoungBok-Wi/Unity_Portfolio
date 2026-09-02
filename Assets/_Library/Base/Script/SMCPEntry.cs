/* MCP 노출 목록의 한 항목 (에디터 전용) */
#if UNITY_EDITOR

namespace Library
{
    /// <summary>노출 목록의 한 항목. Name 은 노출 이름([Manager]·Popup_XXX), Detail·Interaction 은 각각 맵 JSON 이다</summary>
    public struct SMCPEntry
    {
        #region Value
        public string Name;
        public string Detail;
        public string Interaction;
        #endregion

        #region Event
        public SMCPEntry(string _name, string _detail, string _interaction)
        {
            Name = _name;
            Detail = _detail;
            Interaction = _interaction;
        }
        #endregion
    }
}
#endif
