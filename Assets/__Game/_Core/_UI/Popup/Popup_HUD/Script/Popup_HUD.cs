using Library;
using UnityEngine;

namespace Game
{
    /// <summary>전투 HUD 베이스형 팝업 — 플레이어 HP 게이지·방 순번·웨이브·지나온 방 종류 아이콘 열·Crumb 잔액·일시정지 버튼</summary>
    public class Popup_HUD : PopupBase
    {
        public static Popup_HUD instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("HP 게이지 (Filled 이미지)")] private UIWrapper_Guage m_HpGauge;
        [SerializeField, Tooltip("HP 숫자 라벨 (현재/최대)")] private UIWrapper_Text m_HpText;
        [SerializeField, Tooltip("현재 방 순번 라벨")] private UIWrapper_Text m_RoomText;
        [SerializeField, Tooltip("웨이브 진행 라벨 (현재/전체, Battle 방에서만 표시)")] private UIWrapper_Text m_WaveText;
        [SerializeField, Tooltip("지나온 방 이력 항목 (입장 순, 넘치면 최근 것만)")] private Control_RoomHistoryItem[] m_HistoryItems;
        [SerializeField, Tooltip("Crumb 잔액 라벨")] private UIWrapper_Text m_CrumbText;
        [SerializeField, Tooltip("일시정지 버튼")] private UIWrapper_Button m_PauseButton;
        #endregion
        #region Value
        private const string PausePopupId = "Popup_Pause";
        private Object_UnitBase m_Player;
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
            m_PauseButton.AddClickListener(OnClickPause);
            base.Init();
        }
        public override void InitUIOnce()
        {
            var room = LocalRoomManager.instance;
            room.RoomIndex.AddChanged(this, OnRoomChanged);
            room.RoomKind.AddChanged(this, OnWaveChanged);
            room.HistoryCount.AddChanged(this, OnHistoryChanged);
            room.WaveIndex.AddChanged(this, OnWaveChanged);
            room.WaveCount.AddChanged(this, OnWaveChanged);
            BattleManager.instance.Crumb.AddChanged(this, OnCrumbChanged);
            base.InitUIOnce();
        }
        public override void InitUI()
        {
            BindPlayer();
            OnRoomChanged(null);
            OnWaveChanged(null);
            OnHistoryChanged(null);
            OnCrumbChanged(null);
            base.InitUI();
        }
        public override void OnShutdown()
        {
            var room = LocalRoomManager.instance;
            if (room != null)
            {
                room.RoomIndex.RemoveChanged(this, OnRoomChanged);
                room.RoomKind.RemoveChanged(this, OnWaveChanged);
                room.HistoryCount.RemoveChanged(this, OnHistoryChanged);
                room.WaveIndex.RemoveChanged(this, OnWaveChanged);
                room.WaveCount.RemoveChanged(this, OnWaveChanged);
            }
            if (BattleManager.instance != null)
                BattleManager.instance.Crumb.RemoveChanged(this, OnCrumbChanged);
            UnbindPlayer();
            base.OnShutdown();
        }
        /// <summary>일시정지 팝업을 연다</summary>
        private void OnClickPause(UIWrapper_Button _)
        {
            LocalPopupManager.instance.Open(PausePopupId);
        }
        /// <summary>방 순번 갱신 — 런 시작·방 입장마다 플레이어 HP 구독을 다시 잇는다 (플레이어는 런 시작 시 스폰된다)</summary>
        private void OnRoomChanged(ValueBase _)
        {
            var room = LocalRoomManager.instance;
            if (room == null)
                return;
            UIWrapper_Text.Set(m_RoomText, room.RoomIndex.v.ToString());
            BindPlayer();
        }
        /// <summary>웨이브 라벨 갱신 (Battle 방이 아니면 비운다)</summary>
        private void OnWaveChanged(ValueBase _)
        {
            var room = LocalRoomManager.instance;
            if (room == null)
                return;
            bool isBattle = room.RoomKind.v == RoomConst.KindBattle && 0 < room.WaveCount.v;
            UIWrapper_Text.Set(m_WaveText, isBattle ? $"{room.WaveIndex.v}/{room.WaveCount.v}" : "");
        }
        /// <summary>이력 아이콘 열 갱신 — 항목 수를 넘치면 최근 방만 보인다</summary>
        private void OnHistoryChanged(ValueBase _)
        {
            var room = LocalRoomManager.instance;
            if (room == null)
                return;
            var history = room.History;
            int start = Mathf.Max(0, history.Count - m_HistoryItems.Length);
            for (int i = 0; i < m_HistoryItems.Length; i++)
            {
                int index = start + i;
                bool show = index < history.Count;
                m_HistoryItems[i].gameObject.SetActive(show);
                if (show)
                    m_HistoryItems[i].Set(IconManager.GetIcon(history[index]));
            }
        }
        /// <summary>Crumb 잔액 라벨 갱신</summary>
        private void OnCrumbChanged(ValueBase _)
        {
            if (BattleManager.instance != null)
                UIWrapper_Text.Set(m_CrumbText, BattleManager.instance.Crumb.v.ToString());
        }
        /// <summary>HP 게이지·숫자 갱신</summary>
        private void OnHpChanged(ValueBase _)
        {
            if (m_Player == null)
                return;
            int max = Mathf.Max(1, m_Player.MaxHp.v);
            m_HpGauge.Set((float)m_Player.Hp.v / max);
            UIWrapper_Text.Set(m_HpText, $"{m_Player.Hp.v}/{m_Player.MaxHp.v}");
        }
        #endregion
        #region Local Function
        /// <summary>현재 플레이어 유닛의 HP·MaxHp 를 구독한다 (같은 유닛이면 유지)</summary>
        private void BindPlayer()
        {
            var battle = LocalBattleManager.instance;
            var player = battle != null ? battle.Player : null;
            if (player == m_Player)
            {
                OnHpChanged(null);
                return;
            }
            UnbindPlayer();
            m_Player = player;
            if (m_Player == null)
                return;
            m_Player.Hp.AddChanged(this, OnHpChanged);
            m_Player.MaxHp.AddChanged(this, OnHpChanged);
            OnHpChanged(null);
        }
        /// <summary>플레이어 HP 구독을 푼다</summary>
        private void UnbindPlayer()
        {
            if (m_Player == null)
                return;
            m_Player.Hp.RemoveChanged(this, OnHpChanged);
            m_Player.MaxHp.RemoveChanged(this, OnHpChanged);
            m_Player = null;
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            var room = LocalRoomManager.instance;
            if (room == null) return;
            _report.AddNumber("hp", m_Player != null ? m_Player.Hp.v : 0);
            _report.AddNumber("maxHp", m_Player != null ? m_Player.MaxHp.v : 0);
            _report.AddNumber("roomIndex", room.RoomIndex.v);
            _report.Add("wave", m_WaveText != null ? m_WaveText.v.text : "");
            _report.Add("history", string.Join(",", room.History));
            _report.AddNumber("crumb", BattleManager.instance != null ? BattleManager.instance.Crumb.v : 0);
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (IsOpened)
                _report.Add("Pause", "일시정지 팝업 열기");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "Pause": OnClickPause(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
