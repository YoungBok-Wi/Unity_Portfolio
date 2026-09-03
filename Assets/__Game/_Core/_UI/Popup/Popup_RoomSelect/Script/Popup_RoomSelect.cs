using Library;
using UnityEngine;

namespace Game
{
    /// <summary>방 선택 프레임형 팝업 — LocalRoomManager 의 2택 선택지를 카드(방 종류 아이콘·이름·설명·적 미리보기)로 보이고 선택을 넘긴다. 취소 불가</summary>
    public class Popup_RoomSelect : PopupBase
    {
        public static Popup_RoomSelect instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("프레임 (제목)")] private Control_GameFrame m_Frame;
        [SerializeField, Tooltip("선택지 카드 2개 (좌·우)")] private Control_RoomChoice[] m_Choices;
        [SerializeField, Tooltip("적 미리보기 항목 — 카드마다 PreviewPerChoice 개씩 순서대로")] private Control_EnemyPreview[] m_Previews;
        [SerializeField, Tooltip("카드 하나가 갖는 미리보기 항목 수")] private int m_PreviewPerChoice = 3;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            for (int i = 0; i < m_Choices.Length; i++)
            {
                int index = i;
                m_Choices[i].AddClickListener(_ => OnClickChoice(index));
            }
            if (m_Frame != null)
                m_Frame.SetCloseVisible(false);
            base.Init();
        }
        public override void OnOpen(object _option = null)
        {
            base.OnOpen(_option);
            Refresh();
        }
        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            if (m_Frame != null)
                m_Frame.SetTitle(LanguageManager.instance.Get("Text_Core_RoomSelectTitle"));
            if (IsOpened)
                Refresh();
        }
        /// <summary>_index 선택지의 방으로 들어간다</summary>
        private void OnClickChoice(int _index)
        {
            var room = LocalRoomManager.instance;
            if (room == null || room.State.v != ERoomState.Choosing)
                return;
            room.SelectRoom(_index);
        }
        #endregion
        #region Local Function
        /// <summary>선택지·미리보기 표시를 현재 Choices 로 채운다</summary>
        private void Refresh()
        {
            var room = LocalRoomManager.instance;
            if (room == null)
                return;
            var choices = room.Choices;
            var roomTable = TableManager.instance.Room.Data;
            var language = LanguageManager.instance;
            for (int c = 0; c < m_Choices.Length; c++)
            {
                bool has = c < choices.Count;
                m_Choices[c].gameObject.SetActive(has);
                if (!has)
                    continue;
                var choice = choices[c];
                var row = roomTable[choice.Kind];
                m_Choices[c].Set(IconManager.GetIcon(choice.Kind), language.Get(row.Name), language.Get(row.Desc));
                for (int p = 0; p < m_PreviewPerChoice; p++)
                {
                    int slot = c * m_PreviewPerChoice + p;
                    if (m_Previews.Length <= slot)
                        break;
                    bool show = p < choice.Enemies.Length;
                    m_Previews[slot].gameObject.SetActive(show);
                    if (show)
                        m_Previews[slot].Set(RoomUtil.LoadUnitIcon(choice.Enemies[p].Id), choice.Enemies[p].Count);
                }
            }
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            var room = LocalRoomManager.instance;
            if (room == null) return;
            for (int i = 0; i < room.Choices.Count; i++)
            {
                var choice = room.Choices[i];
                var enemies = new System.Text.StringBuilder();
                foreach (var e in choice.Enemies)
                    enemies.Append(e.Id).Append('x').Append(e.Count).Append(' ');
                _report.Add($"choice{i}", $"{choice.Kind} {enemies.ToString().Trim()}");
            }
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (!IsOpened || LocalRoomManager.instance == null) return;
            for (int i = 0; i < LocalRoomManager.instance.Choices.Count; i++)
                _report.Add($"Select{i}", $"{LocalRoomManager.instance.Choices[i].Kind} 방 선택");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            if (_interactionId.StartsWith("Select") && int.TryParse(_interactionId.Substring("Select".Length), out var index))
            {
                OnClickChoice(index);
                return "{\"success\":true}";
            }
            return base.MCPInteract(_interactionId, _value);
        }
#endif
        #endregion
    }
}
