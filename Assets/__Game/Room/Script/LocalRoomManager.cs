using Library;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>런 시작·방 입장·웨이브·클리어·사망 종료를 관리한다.</summary>
    public class LocalRoomManager : LocalManagerBase
    {
        public static LocalRoomManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("플레이어 스폰 위치")] private Transform m_PlayerSpawn;
        [SerializeField, Tooltip("방 왼쪽 끝 적 스폰 위치")] private Transform m_SpawnLeft;
        [SerializeField, Tooltip("방 오른쪽 끝 적 스폰 위치")] private Transform m_SpawnRight;
        [SerializeField, Tooltip("같은 쪽 적 사이 간격 (u)")] private float m_EnemySpacing = 1f;
        [SerializeField, Tooltip("카메라 추종 속도")] private float m_CameraLerp = 5f;
        [SerializeField, Tooltip("방 반폭 (u)")] private float m_RoomHalfWidth = 12f;
        [SerializeField, Tooltip("카메라 고정 Y")] private float m_CameraFixedY;
        #endregion
        #region Property
        /// <summary>현재 방 순번 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue RoomIndex => m_RoomIndex;
        /// <summary>현재 방 종류 읽기 값을 반환한다.</summary>
        public IReadOnlyStringValue RoomKind => m_RoomKind;
        /// <summary>현재 방 진행 상태 읽기 값을 반환한다.</summary>
        public IReadOnlyEnumValue<ERoomState> State => m_State;
        /// <summary>현재 웨이브 순번 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue WaveIndex => m_WaveIndex;
        /// <summary>현재 방의 전체 웨이브 수 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue WaveCount => m_WaveCount;
        /// <summary>누적 방 기록 수 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue HistoryCount => m_HistoryCount;
        /// <summary>현재 능력 리롤 수 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue RerollCount => m_RerollCount;
        /// <summary>최근 방 종류 기록을 반환한다.</summary>
        public IReadOnlyList<string> History => m_History;
        /// <summary>현재 제시된 능력 ID 목록을 반환한다.</summary>
        public IReadOnlyList<string> AbilityChoices => m_AbilityChoices;
        /// <summary>현재 능력 리롤 비용을 반환한다.</summary>
        public int RerollCost => TableManager.instance.Const.Ability_RerollBaseCost + m_RerollCount.v * TableManager.instance.Const.Ability_RerollCostStep;
        /// <summary>방 중심에서 벽까지의 수평 거리를 반환한다.</summary>
        public float RoomHalfWidth => m_RoomHalfWidth;
        #endregion
        #region Value
        private IntValue m_RoomIndex;
        private StringValue m_RoomKind;
        private EnumValue<ERoomState> m_State;
        private IntValue m_WaveIndex;
        private IntValue m_WaveCount;
        private IntValue m_HistoryCount;
        private IntValue m_RerollCount;
        private readonly List<string> m_History = new();
        private readonly List<string> m_AbilityChoices = new();
        private List<WaveTable> m_Waves;
        private string m_BossId;
        private int m_Variant;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            m_RoomIndex = new IntValue(this, "RoomIndex", 0);
            m_RoomKind = new StringValue(this, "RoomKind", "");
            m_State = new EnumValue<ERoomState>(this, "RoomState", ERoomState.None);
            m_WaveIndex = new IntValue(this, "WaveIndex", 0);
            m_WaveCount = new IntValue(this, "WaveCount", 0);
            m_HistoryCount = new IntValue(this, "HistoryCount", 0);
            m_RerollCount = new IntValue(this, "RerollCount", 0);
            base.Init();
        }
        public override bool RequireInitGame()
        {
            return LocalGameManager.instance != null && LocalGameManager.instance.IsGameInited
                && LocalPlayerCharacterManager.instance != null && LocalPlayerCharacterManager.instance.IsInited
                && LocalRoomSelectManager.instance != null && LocalRoomSelectManager.instance.IsInited;
        }
        public override void InitGame()
        {
            var game = LocalGameManager.instance;
            game.AliveEnemyCount.AddChanged(this, OnAliveChanged);
            game.IsPlayerDead.AddChanged(this, OnPlayerDead);
            game.IsBossDead.AddChanged(this, OnBossDead);
            CreateWalls();
            base.InitGame();
            StartRun();
        }
        public override void OnShutdown()
        {
            var game = LocalGameManager.instance;
            if (game != null)
            {
                game.AliveEnemyCount.RemoveChanged(this, OnAliveChanged);
                game.IsPlayerDead.RemoveChanged(this, OnPlayerDead);
                game.IsBossDead.RemoveChanged(this, OnBossDead);
            }
            base.OnShutdown();
        }
        private void OnAliveChanged(ValueBase _)
        {
            if (m_State.v != ERoomState.Playing || m_RoomKind.v != RoomConst.KindBattle || 0 < LocalGameManager.instance.AliveEnemyCount.v)
                return;
            if (m_WaveIndex.v < m_WaveCount.v)
                NextWave();
            else
                ClearRoom();
        }
        private void OnPlayerDead(ValueBase _)
        {
            if (LocalGameManager.instance.IsPlayerDead.v && m_State.v != ERoomState.Ended)
                EndRun();
        }
        private void OnBossDead(ValueBase _)
        {
            if (LocalGameManager.instance.IsBossDead.v && m_State.v == ERoomState.Playing && m_RoomKind.v == RoomConst.KindBoss)
                ClearRoom();
        }
        #endregion
        #region Local Function
        private void CreateWalls()
        {
            foreach (int side in new[] { -1, 1 })
            {
                var wall = new GameObject(side < 0 ? "WallLeft" : "WallRight");
                wall.transform.SetParent(transform, false);
                wall.transform.position = new Vector3(side * (m_RoomHalfWidth + RoomConst.WallThickness * 0.5f), m_CameraFixedY, 0f);
                wall.AddComponent<BoxCollider2D>().size = new Vector2(RoomConst.WallThickness, 40f);
            }
        }
        private float GetCameraClampX()
        {
            var cam = LocalCameraManager.instance.CurCam;
            return Mathf.Max(0f, m_RoomHalfWidth - cam.orthographicSize * cam.aspect);
        }
        private void EnterRoom(SRoomChoice _choice)
        {
            var game = LocalGameManager.instance;
            game.ClearUnits();
            m_History.Add(_choice.Kind);
            if (RoomConst.HistoryMax < m_History.Count)
                m_History.RemoveAt(0);
            m_HistoryCount.v += 1;
            m_RoomKind.v = _choice.Kind;
            m_BossId = _choice.BossId;
            m_Variant = _choice.Variant;
            m_State.v = ERoomState.Playing;
            switch (_choice.Kind)
            {
                case RoomConst.KindBattle:
                    m_Waves = RoomUtil.GetWaves(m_RoomIndex.v, m_Variant);
                    m_WaveCount.v = m_Waves.Count;
                    m_WaveIndex.Set(0, false, false);
                    NextWave();
                    break;
                case RoomConst.KindHeal:
                    game.HealPlayer(TableManager.instance.Const.Room_HealRatio);
                    ClearRoom();
                    break;
                case RoomConst.KindAbility:
                    m_RerollCount.Set(0, true, false);
                    RollAbilities();
                    if (m_AbilityChoices.Count == 0)
                        ClearRoom();
                    else
                        LocalPopupManager.instance.Open(RoomConst.PopupAbility);
                    break;
                case RoomConst.KindBoss:
                    if (string.IsNullOrEmpty(m_BossId))
                        m_BossId = RoomUtil.RollBoss();
                    game.SpawnUnit(m_BossId, m_SpawnRight.position, RoomUtil.GetHpScale(m_RoomIndex.v), RoomUtil.GetAtkScale(m_RoomIndex.v));
                    break;
                default:
                    throw new ArgumentException($"Room 테이블에 없는 방 종류 : {_choice.Kind}", nameof(_choice));
            }
        }
        private void NextWave()
        {
            m_WaveIndex.v += 1;
            LocalGameManager.instance.SpawnWave(m_Waves[m_WaveIndex.v - 1], RoomUtil.GetHpScale(m_RoomIndex.v), RoomUtil.GetAtkScale(m_RoomIndex.v), m_SpawnLeft.position, m_SpawnRight.position, m_EnemySpacing);
        }
        private void ClearRoom()
        {
            LocalGameManager.instance.CollectAllCrumbs();
            if (DataManager.instance.OnRoomCleared(m_RoomIndex.v) && Popup_Notify.instance != null)
            {
                LocalGameManager.instance.PlayUnlockSfx();
                var language = LanguageManager.instance;
                Popup_Notify.instance.Open(new Popup_Notify.SOption(null, string.Format(language.Get(RoomConst.TextGunUnlocked), TableManager.instance.Const.Room_GunUnlock), language.Get(RoomConst.TextConfirm), null));
            }
            m_State.v = ERoomState.Choosing;
            LocalRoomSelectManager.instance.RollChoices(m_RoomIndex.v + 1, m_RoomKind.v);
            LocalPopupManager.instance.Open(RoomConst.PopupRoomSelect);
        }
        private void RollAbilities()
        {
            var game = LocalGameManager.instance;
            var pool = new List<string>();
            foreach (var id in TableManager.instance.Ability.ID)
                if (game.CanAddAbility(id))
                    pool.Add(id);
            m_AbilityChoices.Clear();
            int count = TableManager.instance.Const.Ability_ChoiceCount;
            while (0 < pool.Count && m_AbilityChoices.Count < count)
            {
                int index = UnityEngine.Random.Range(0, pool.Count);
                m_AbilityChoices.Add(pool[index]);
                pool.RemoveAt(index);
            }
        }
        private void EndRun()
        {
            m_State.v = ERoomState.Ended;
            LocalPopupManager.instance.Open(RoomConst.PopupResult);
        }
        private void StartRun()
        {
            DataManager.instance.ResetRun();
            LocalGameManager.instance.ResetRun();
            m_History.Clear();
            m_HistoryCount.Set(0, false, false);
            m_BossId = null;
            var player = LocalPlayerCharacterManager.instance.Player;
            if (player == null)
                throw new InvalidOperationException("선택된 씬 상주 플레이어가 없다");
            LocalGameManager.instance.SetPlayer(player);
            player.Spawn(m_PlayerSpawn.position, 1f, 1f);
            if (LocalCameraManager.instance != null)
                LocalCameraManager.instance.SetFollow(player.transform, m_CameraLerp, GetCameraClampX(), m_CameraFixedY);
            m_RoomIndex.Set(1, true, false);
            EnterRoom(new SRoomChoice(RoomConst.KindBattle, null, 1, Array.Empty<SEnemyPreview>()));
        }
        #endregion
        #region Function
        /// <summary>_choice를 다음 방으로 선택하고 입장한다.</summary>
        public void EnterNext(SRoomChoice _choice)
        {
            if (m_State.v != ERoomState.Choosing)
                throw new InvalidOperationException($"방 선택 중이 아니다 : {m_State.v}");
            m_RoomIndex.v += 1;
            EnterRoom(_choice);
        }
        /// <summary>제시된 _abilityId 능력을 선택하고 방을 끝낸다.</summary>
        public void SelectAbility(string _abilityId)
        {
            if (m_State.v != ERoomState.Playing || m_RoomKind.v != RoomConst.KindAbility || !m_AbilityChoices.Contains(_abilityId))
                throw new InvalidOperationException($"제시 중이 아닌 능력 : {_abilityId}");
            LocalGameManager.instance.AddAbility(_abilityId);
            m_AbilityChoices.Clear();
            LocalPopupManager.instance.Close(RoomConst.PopupAbility);
            ClearRoom();
        }
        /// <summary>Crumb를 지불해 능력 선택지를 다시 뽑고 성공 여부를 반환한다.</summary>
        public bool RerollAbility()
        {
            if (m_State.v != ERoomState.Playing || m_RoomKind.v != RoomConst.KindAbility)
                throw new InvalidOperationException("Ability 방이 아니다");
            if (DealManager.instance.Pay(new SDeal(DataConst.CrumbId, "", RerollCost)) == null)
                return false;
            m_RerollCount.v += 1;
            RollAbilities();
            return true;
        }
        /// <summary>Face 전환 연출로 로비 씬에 돌아간다.</summary>
        public void ReturnLobby()
        {
            SceneChangeManager.instance.SceneChange(SceneChangeManager.instance.LobbySceneID, "Face");
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            _report.AddNumber("roomIndex", m_RoomIndex.v);
            _report.Add("roomKind", m_RoomKind.v);
            _report.Add("state", m_State.v.ToString());
            _report.AddNumber("variant", m_Variant);
            _report.AddNumber("waveIndex", m_WaveIndex.v);
            _report.AddNumber("waveCount", m_WaveCount.v);
            _report.Add("history", string.Join(",", m_History));
            _report.AddNumber("historyCount", m_HistoryCount.v);
            _report.Add("abilityChoices", string.Join(",", m_AbilityChoices));
            _report.AddNumber("rerollCost", RerollCost);
        }
        public override void MCPInteraction(MCPReport _report)
        {
            if (m_State.v == ERoomState.Playing && m_RoomKind.v == RoomConst.KindAbility)
            {
                foreach (var id in m_AbilityChoices)
                    _report.Add($"SelectAbility_{id}", $"{id} 능력 선택");
                _report.Add("RerollAbility", $"능력 리롤 (Crumb {RerollCost})");
            }
            if (m_State.v == ERoomState.Ended)
                _report.Add("ReturnLobby", "로비로 돌아가기");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            if (_interactionId.StartsWith("SelectAbility_"))
            {
                SelectAbility(_interactionId.Substring("SelectAbility_".Length));
                return "{\"success\":true}";
            }
            if (_interactionId == "RerollAbility")
                return RerollAbility() ? "{\"success\":true}" : "{\"error\":\"Crumb 부족\"}";
            if (_interactionId == "ReturnLobby")
            {
                ReturnLobby();
                return "{\"success\":true}";
            }
            return base.MCPInteract(_interactionId, _value);
        }
        public override void MCPCheats(MCPReport _report)
        {
            if (m_State.v == ERoomState.Playing)
                _report.Add("ClearRoom", "현재 방 즉시 클리어");
            if (m_State.v != ERoomState.Ended)
                _report.Add("LoseRun", "플레이어 사망 처리");
        }
        public override string MCPCheatApply(string _cheatId)
        {
            if (_cheatId == "ClearRoom" && m_State.v == ERoomState.Playing)
            {
                LocalGameManager.instance.ClearUnits();
                m_AbilityChoices.Clear();
                LocalPopupManager.instance.Close(RoomConst.PopupAbility);
                ClearRoom();
                return "{\"success\":true}";
            }
            if (_cheatId == "LoseRun")
            {
                EndRun();
                return "{\"success\":true}";
            }
            return base.MCPCheatApply(_cheatId);
        }
#endif
        #endregion
    }
}
