using Library;
using UnityEngine;

namespace Game
{
    /// <summary>로비 화면 베이스형 팝업 — Knife·Gun 선택 카드(잠금 마크·해금 조건)·시작·설정 버튼·최고 도달 방 순번을 표시한다</summary>
    public class Popup_Lobby : PopupBase
    {
        public static Popup_Lobby instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("Knife 선택 카드 버튼")] private UIWrapper_Button m_KnifeButton;
        [SerializeField, Tooltip("Gun 선택 카드 버튼")] private UIWrapper_Button m_GunButton;
        [SerializeField, Tooltip("Knife 선택 강조 프레임")] private GameObject m_KnifeSelect;
        [SerializeField, Tooltip("Gun 선택 강조 프레임")] private GameObject m_GunSelect;
        [SerializeField, Tooltip("Gun 잠금 마크 (미해금 시 표시)")] private GameObject m_GunLock;
        [SerializeField, Tooltip("Knife 이름 라벨")] private UIWrapper_Text m_KnifeName;
        [SerializeField, Tooltip("Gun 이름 라벨")] private UIWrapper_Text m_GunName;
        [SerializeField, Tooltip("Knife 설명 라벨")] private UIWrapper_Text m_KnifeDesc;
        [SerializeField, Tooltip("Gun 설명 라벨 (미해금 시 해금 조건)")] private UIWrapper_Text m_GunDesc;
        [SerializeField, Tooltip("시작 버튼")] private UIWrapper_Button m_StartButton;
        [SerializeField, Tooltip("시작 버튼 라벨")] private UIWrapper_Text m_StartLabel;
        [SerializeField, Tooltip("설정 버튼")] private UIWrapper_Button m_SettingButton;
        [SerializeField, Tooltip("게임 제목 라벨")] private UIWrapper_Text m_Title;
        [SerializeField, Tooltip("최고 도달 방 순번 라벨")] private UIWrapper_Text m_BestRoom;
        #endregion
        #region Value
        private const string KnifeId = "Knife";
        private const string GunId = "Gun";
        private const string SettingPopupId = "Popup_Setting";
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
            m_KnifeButton.AddClickListener(_ => OnClickCharacter(KnifeId));
            m_GunButton.AddClickListener(_ => OnClickCharacter(GunId));
            m_StartButton.AddClickListener(OnClickStart);
            m_SettingButton.AddClickListener(OnClickSetting);
            base.Init();
        }
        public override void InitUIOnce()
        {
            var mgr = CharacterManager.instance;
            mgr.SelectedId.AddChanged(this, RefreshCards);
            mgr.GunUnlocked.AddChanged(this, RefreshCards);
            mgr.BestRoom.AddChanged(this, RefreshBest);
            base.InitUIOnce();
        }
        public override void InitUI()
        {
            RefreshCards(null);
            RefreshBest(null);
            base.InitUI();
        }
        public override void OnShutdown()
        {
            var mgr = CharacterManager.instance;
            if (mgr != null)
            {
                mgr.SelectedId.RemoveChanged(this, RefreshCards);
                mgr.GunUnlocked.RemoveChanged(this, RefreshCards);
                mgr.BestRoom.RemoveChanged(this, RefreshBest);
            }
            base.OnShutdown();
        }
        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            var table = TableManager.instance.Character.Data;
            var language = LanguageManager.instance;
            UIWrapper_Text.Set(m_KnifeName, language.Get(table[KnifeId].Name));
            UIWrapper_Text.Set(m_GunName, language.Get(table[GunId].Name));
            UIWrapper_Text.Set(m_KnifeDesc, language.Get(table[KnifeId].Desc));
            UIWrapper_Text.SetTextId(m_StartLabel, "Text_Core_GameStart");
            UIWrapper_Text.SetTextId(m_Title, "Text_Core_GameTitle");
            RefreshCards(null);
            RefreshBest(null);
        }
        /// <summary>_id 카드 클릭 — 해금됐으면 선택, 아니면 해금 조건 알림</summary>
        private void OnClickCharacter(string _id)
        {
            var mgr = CharacterManager.instance;
            if (mgr.IsUnlocked(_id))
            {
                mgr.Select(_id);
                return;
            }
            if (Popup_Notify.instance != null)
                Popup_Notify.instance.Open(new Popup_Notify.SOption(null, UnlockText(), LanguageManager.instance.Get("Text_Core_Close"), null));
        }
        /// <summary>시작 — 게임 씬으로 전환한다</summary>
        private void OnClickStart(UIWrapper_Button _)
        {
            SceneChangeManager.instance.SceneChange(SceneChangeManager.instance.GameSceneID);
        }
        /// <summary>설정 팝업을 연다 (이 씬에 등재돼 있을 때만)</summary>
        private void OnClickSetting(UIWrapper_Button _)
        {
            LocalPopupManager.instance.TryOpen(SettingPopupId);
        }
        #endregion
        #region Local Function
        /// <summary>선택 강조·잠금 마크·Gun 설명(해금 전엔 해금 조건)을 갱신한다</summary>
        private void RefreshCards(ValueBase _)
        {
            var mgr = CharacterManager.instance;
            if (mgr == null)
                return;
            bool gunUnlocked = mgr.IsUnlocked(GunId);
            m_KnifeSelect.SetActive(mgr.SelectedId.v == KnifeId);
            m_GunSelect.SetActive(mgr.SelectedId.v == GunId);
            m_GunLock.SetActive(!gunUnlocked);
            if (LanguageManager.instance != null)
                UIWrapper_Text.Set(m_GunDesc, gunUnlocked ? LanguageManager.instance.Get(TableManager.instance.Character.Data[GunId].Desc) : UnlockText());
        }
        /// <summary>최고 도달 방 순번 표시를 갱신한다</summary>
        private void RefreshBest(ValueBase _)
        {
            if (CharacterManager.instance != null)
                UIWrapper_Text.Set(m_BestRoom, CharacterManager.instance.BestRoom.v.ToString());
        }
        /// <summary>Gun 해금 조건 문구를 반환한다 (Text_Core_GunUnlock 의 {0} 에 해금 방 순번)</summary>
        private string UnlockText()
        {
            return string.Format(LanguageManager.instance.Get(RoomConst.TextGunUnlock), TableManager.instance.Const.Room_GunUnlock);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            var mgr = CharacterManager.instance;
            if (mgr == null) return;
            _report.Add("selected", mgr.SelectedId.v);
            _report.AddRaw("gunUnlocked", mgr.GunUnlocked.v ? "true" : "false");
            _report.AddNumber("bestRoom", mgr.BestRoom.v);
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (!IsOpened) return;
            _report.Add("SelectKnife", "Knife 카드 선택");
            _report.Add("SelectGun", "Gun 카드 선택 (미해금이면 해금 조건 알림)");
            _report.Add("Start", "게임 시작");
            _report.Add("Setting", "설정 열기");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "SelectKnife": OnClickCharacter(KnifeId); return "{\"success\":true}";
                case "SelectGun": OnClickCharacter(GunId); return "{\"success\":true}";
                case "Start": OnClickStart(null); return "{\"success\":true}";
                case "Setting": OnClickSetting(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
