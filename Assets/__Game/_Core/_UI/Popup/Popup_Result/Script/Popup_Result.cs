using Library;
using UnityEngine;

namespace Game
{
    /// <summary>런 결과 프레임형 팝업 — 승패·도달 방 순번·Crumb 총량·Gun 해금 알림을 보이고 확인 시 로비로 돌아간다. 취소 불가</summary>
    public class Popup_Result : PopupBase
    {
        public static Popup_Result instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("프레임 (제목)")] private Control_GameFrame m_Frame;
        [SerializeField, Tooltip("승패 라벨")] private UIWrapper_Text m_ResultLabel;
        [SerializeField, Tooltip("도달 방 순번 라벨")] private UIWrapper_Text m_RoomLabel;
        [SerializeField, Tooltip("Crumb 총량 라벨")] private UIWrapper_Text m_CrumbLabel;
        [SerializeField, Tooltip("Gun 해금 알림 라벨 (이번 런에 해금됐을 때만 표시)")] private UIWrapper_Text m_UnlockLabel;
        [SerializeField, Tooltip("확인 버튼 (로비로)")] private UIWrapper_Button m_ConfirmButton;
        [SerializeField, Tooltip("확인 버튼 라벨")] private UIWrapper_Text m_ConfirmLabel;
        #endregion
        #region Value
        private bool m_GunUnlockedAtStart;
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
            m_ConfirmButton.AddClickListener(OnClickConfirm);
            if (m_Frame != null)
                m_Frame.SetCloseVisible(false);
            base.Init();
        }
        public override void InitGame()
        {
            m_GunUnlockedAtStart = CharacterManager.instance.GunUnlocked.v;
            base.InitGame();
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
                m_Frame.SetTitle(LanguageManager.instance.Get("Text_Core_StageResultTitle"));
            UIWrapper_Text.SetTextId(m_ConfirmLabel, "Text_Core_Lobby");
            if (IsOpened)
                Refresh();
        }
        /// <summary>확인 — 로비로 돌아간다</summary>
        private void OnClickConfirm(UIWrapper_Button _)
        {
            if (LocalRoomManager.instance != null)
                LocalRoomManager.instance.ReturnLobby();
        }
        #endregion
        #region Local Function
        /// <summary>승패·순번·Crumb·해금 표시를 채운다</summary>
        private void Refresh()
        {
            var room = LocalRoomManager.instance;
            if (room == null)
                return;
            var language = LanguageManager.instance;
            UIWrapper_Text.SetTextId(m_ResultLabel, room.Result.v == ERunResult.Win ? "Text_Core_Clear" : "Text_Core_Fail");
            UIWrapper_Text.Set(m_RoomLabel, room.RoomIndex.v.ToString());
            UIWrapper_Text.Set(m_CrumbLabel, BattleManager.instance != null ? BattleManager.instance.CrumbTotal.v.ToString() : "0");
            bool newlyUnlocked = !m_GunUnlockedAtStart && CharacterManager.instance.GunUnlocked.v;
            if (m_UnlockLabel != null)
            {
                m_UnlockLabel.gameObject.SetActive(newlyUnlocked);
                if (newlyUnlocked)
                    m_UnlockLabel.Set(language.Get(RoomConst.TextGunUnlocked));
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
            _report.Add("result", room.Result.v.ToString());
            _report.AddNumber("roomIndex", room.RoomIndex.v);
            _report.AddNumber("crumbTotal", BattleManager.instance != null ? BattleManager.instance.CrumbTotal.v : 0);
            _report.AddRaw("gunNewlyUnlocked", !m_GunUnlockedAtStart && CharacterManager.instance.GunUnlocked.v ? "true" : "false");
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (IsOpened)
                _report.Add("Confirm", "확인 (로비로)");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "Confirm": OnClickConfirm(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
