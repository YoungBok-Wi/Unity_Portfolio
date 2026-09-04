using Library;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    /// <summary>로비 화면 베이스형 팝업 — Knife·Gun 선택 카드(선택 파랑·잠금 회색·해금 조건)·중앙 요리사 일러스트·시작·설정 버튼·최고 도달 방 순번을 표시한다</summary>
    public class Popup_Lobby : PopupBase
    {
        public static Popup_Lobby instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("Knife 선택 카드 버튼")] private UIWrapper_Button m_KnifeButton;
        [SerializeField, Tooltip("Gun 선택 카드 버튼")] private UIWrapper_Button m_GunButton;
        [SerializeField, Tooltip("Knife 선택 강조 프레임")] private GameObject m_KnifeSelect;
        [SerializeField, Tooltip("Gun 선택 강조 프레임")] private GameObject m_GunSelect;
        [SerializeField, Tooltip("Gun 잠금 마크 (미해금 시 표시)")] private GameObject m_GunLock;
        [SerializeField, Tooltip("Knife 카드 배경 이미지 (선택 시 파란 스프라이트로 교체)")] private Image m_KnifeCardImage;
        [SerializeField, Tooltip("Gun 카드 배경 이미지 (선택 시 파란 스프라이트로 교체)")] private Image m_GunCardImage;
        [SerializeField, Tooltip("카드 기본 스프라이트")] private Sprite m_CardNormal;
        [SerializeField, Tooltip("카드 선택 스프라이트 (파랑)")] private Sprite m_CardSelected;
        [SerializeField, Tooltip("Gun 미해금 시 회색 처리할 그래픽 (카드 배경·아이콘)")] private Graphic[] m_GunGrayTargets;
        [SerializeField, Tooltip("중앙 요리사 일러스트 Knife (Knife 선택 시 표시)")] private GameObject m_ChefKnife;
        [SerializeField, Tooltip("중앙 요리사 일러스트 Gun (Gun 선택 시 표시)")] private GameObject m_ChefGun;
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
        private static readonly Color LockedTint = new(0.55f, 0.55f, 0.55f);
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
        /// <summary>취소 입력 — 이 팝업 위에 열린 팝업이 있으면 최상단 하나만 닫고 소비한다. 없으면 기본 처리로 넘겨 종료 팝업이 열리게 둔다</summary>
        public override bool OnInputCancel(InputAction.CallbackContext _context)
        {
            if (!_context.performed)
                return base.OnInputCancel(_context);
            var top = FindTopOpenedPopup();
            if (top == null)
                return base.OnInputCancel(_context);
            top.Close();
            return true;
        }
        #endregion
        #region Local Function
        /// <summary>선택 강조(파란 카드·프레임·요리사 일러스트)·잠금 마크·회색 처리·Gun 설명(해금 전엔 해금 조건)을 갱신한다</summary>
        private void RefreshCards(ValueBase _)
        {
            var mgr = CharacterManager.instance;
            if (mgr == null)
                return;
            bool gunUnlocked = mgr.IsUnlocked(GunId);
            bool knifeSelected = mgr.SelectedId.v == KnifeId;
            bool gunSelected = mgr.SelectedId.v == GunId;
            m_KnifeSelect.SetActive(knifeSelected);
            m_GunSelect.SetActive(gunSelected);
            SetCardSprite(m_KnifeCardImage, knifeSelected);
            SetCardSprite(m_GunCardImage, gunSelected);
            if (m_ChefKnife != null)
                m_ChefKnife.SetActive(knifeSelected);
            if (m_ChefGun != null)
                m_ChefGun.SetActive(gunSelected);
            m_GunLock.SetActive(!gunUnlocked);
            if (m_GunGrayTargets != null)
                foreach (var graphic in m_GunGrayTargets)
                    if (graphic != null)
                        graphic.color = gunUnlocked ? Color.white : LockedTint;
            if (LanguageManager.instance != null)
                UIWrapper_Text.Set(m_GunDesc, gunUnlocked ? LanguageManager.instance.Get(TableManager.instance.Character.Data[GunId].Desc) : UnlockText());
        }
        /// <summary>_card 의 스프라이트를 _selected 에 따라 선택·기본 스프라이트로 바꾼다 (미배선이면 무시)</summary>
        private void SetCardSprite(Image _card, bool _selected)
        {
            if (_card == null)
                return;
            var sprite = _selected ? m_CardSelected : m_CardNormal;
            if (sprite != null)
                _card.sprite = sprite;
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
        /// <summary>이 팝업을 제외하고 열려 있는(닫는 중 아님) 팝업 중 Canvas 정렬 순서가 가장 높은 것을 반환한다. 없으면 null</summary>
        private PopupBase FindTopOpenedPopup()
        {
            var manager = LocalPopupManager.instance;
            if (manager == null)
                return null;
            PopupBase top = null;
            foreach (var id in manager.IDs)
            {
                var popup = manager.Get(id);
                if (popup == this || !popup.IsOpened || popup.IsClosing)
                    continue;
                if (top == null || top.PopupCanvas.sortingOrder < popup.PopupCanvas.sortingOrder)
                    top = popup;
            }
            return top;
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
