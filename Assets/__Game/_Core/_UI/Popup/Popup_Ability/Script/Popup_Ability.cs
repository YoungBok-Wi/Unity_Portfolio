using Library;
using UnityEngine;

namespace Game
{
    /// <summary>능력 선택 프레임형 팝업 — 제시된 능력 3택1 카드와 Crumb 소모 리롤 버튼. 취소 불가</summary>
    public class Popup_Ability : PopupBase
    {
        public static Popup_Ability instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("프레임 (제목)")] private Control_GameFrame m_Frame;
        [SerializeField, Tooltip("능력 카드 (제시 수만큼)")] private Control_AbilityCard[] m_Cards;
        [SerializeField, Tooltip("리롤 버튼")] private UIWrapper_Button m_RerollButton;
        [SerializeField, Tooltip("리롤 버튼 라벨 (문구 + 비용)")] private UIWrapper_Text m_RerollLabel;
        [SerializeField, Tooltip("Crumb 잔액 라벨")] private UIWrapper_Text m_CrumbLabel;
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
            for (int i = 0; i < m_Cards.Length; i++)
            {
                int index = i;
                m_Cards[i].AddClickListener(_ => OnClickCard(index));
            }
            m_RerollButton.AddClickListener(OnClickReroll);
            if (m_Frame != null)
                m_Frame.SetCloseVisible(false);
            base.Init();
        }
        public override void InitUIOnce()
        {
            LocalRoomManager.instance.RerollCount.AddChanged(this, OnRerollChanged);
            BattleManager.instance.Crumb.AddChanged(this, OnCrumbChanged);
            base.InitUIOnce();
        }
        public override void OnShutdown()
        {
            if (LocalRoomManager.instance != null)
                LocalRoomManager.instance.RerollCount.RemoveChanged(this, OnRerollChanged);
            if (BattleManager.instance != null)
                BattleManager.instance.Crumb.RemoveChanged(this, OnCrumbChanged);
            base.OnShutdown();
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
                m_Frame.SetTitle(LanguageManager.instance.Get("Text_Core_UpgradeSelectTitle"));
            if (IsOpened)
                Refresh();
        }
        /// <summary>_index 카드의 능력을 얻는다</summary>
        private void OnClickCard(int _index)
        {
            var room = LocalRoomManager.instance;
            if (room == null || room.AbilityChoices.Count <= _index)
                return;
            room.SelectAbility(room.AbilityChoices[_index]);
        }
        /// <summary>리롤 — 잔액이 모자라면 알림</summary>
        private void OnClickReroll(UIWrapper_Button _)
        {
            var room = LocalRoomManager.instance;
            if (room == null || room.RerollAbility())
                return;
            if (Popup_Notify.instance != null)
                Popup_Notify.instance.Open(new Popup_Notify.SOption(null, LanguageManager.instance.Get("Text_Core_NotEnoughCrumb"), LanguageManager.instance.Get("Text_Core_Close"), null));
        }
        /// <summary>리롤 후 카드·비용을 다시 채운다</summary>
        private void OnRerollChanged(ValueBase _)
        {
            if (IsOpened)
                Refresh();
        }
        /// <summary>Crumb 잔액 라벨 갱신</summary>
        private void OnCrumbChanged(ValueBase _)
        {
            if (BattleManager.instance != null)
                UIWrapper_Text.Set(m_CrumbLabel, BattleManager.instance.Crumb.v.ToString());
        }
        #endregion
        #region Local Function
        /// <summary>카드·리롤 비용·잔액 표시를 현재 제시 목록으로 채운다</summary>
        private void Refresh()
        {
            var room = LocalRoomManager.instance;
            if (room == null)
                return;
            var choices = room.AbilityChoices;
            var table = TableManager.instance.Ability.Data;
            var language = LanguageManager.instance;
            for (int i = 0; i < m_Cards.Length; i++)
            {
                bool has = i < choices.Count;
                m_Cards[i].gameObject.SetActive(has);
                if (has)
                    m_Cards[i].Set(IconManager.GetIcon(choices[i]), language.Get(table[choices[i]].Name), language.Get(table[choices[i]].Desc));
            }
            UIWrapper_Text.Set(m_RerollLabel, $"{language.Get("Text_Core_Retry")} {room.RerollCost}");
            OnCrumbChanged(null);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            var room = LocalRoomManager.instance;
            if (room == null) return;
            _report.Add("choices", string.Join(",", room.AbilityChoices));
            _report.AddNumber("rerollCost", room.RerollCost);
            _report.AddNumber("crumb", BattleManager.instance != null ? BattleManager.instance.Crumb.v : 0);
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (!IsOpened || LocalRoomManager.instance == null) return;
            var choices = LocalRoomManager.instance.AbilityChoices;
            for (int i = 0; i < choices.Count; i++)
                _report.Add($"Select{i}", $"{choices[i]} 능력 선택");
            _report.Add("Reroll", $"리롤 (Crumb {LocalRoomManager.instance.RerollCost})");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            if (_interactionId.StartsWith("Select") && int.TryParse(_interactionId.Substring("Select".Length), out var index))
            {
                OnClickCard(index);
                return "{\"success\":true}";
            }
            switch (_interactionId)
            {
                case "Reroll": OnClickReroll(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
