using Library;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>씬 방 진행 매니저 — 런 시작·방 입장·웨이브 진행·클리어 판정·2택 선택지·능력 선택·런 종료를 관리한다</summary>
    public class LocalRoomManager : LocalManagerBase
    {
        public static LocalRoomManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("플레이어 스폰 위치")] private Transform m_PlayerSpawn;
        [SerializeField, Tooltip("방 왼쪽 끝 적 스폰 위치")] private Transform m_SpawnLeft;
        [SerializeField, Tooltip("방 오른쪽 끝 적 스폰 위치")] private Transform m_SpawnRight;
        [SerializeField, Tooltip("같은 쪽 적 사이 간격 (u)")] private float m_EnemySpacing = 1.0f;
        [SerializeField, Tooltip("카메라 추종 속도")] private float m_CameraLerp = 5f;
        [SerializeField, Tooltip("카메라 추종 X 한계 (방 반폭, u)")] private float m_CameraClampX = 8f;
        [SerializeField, Tooltip("카메라 고정 Y")] private float m_CameraFixedY = 0f;
        #endregion
        #region Property
        /// <summary>현재 방 순번 (1부터)</summary>
        public IReadOnlyIntValue RoomIndex => m_RoomIndex;
        /// <summary>현재 방 종류 (Room 테이블 ID)</summary>
        public IReadOnlyStringValue RoomKind => m_RoomKind;
        /// <summary>진행 상태</summary>
        public IReadOnlyEnumValue<ERoomState> State => m_State;
        /// <summary>Battle 방 현재 웨이브 (1부터)</summary>
        public IReadOnlyIntValue WaveIndex => m_WaveIndex;
        /// <summary>Battle 방 웨이브 수</summary>
        public IReadOnlyIntValue WaveCount => m_WaveCount;
        /// <summary>런 결과 (Ended 에서 확정)</summary>
        public IReadOnlyEnumValue<ERunResult> Result => m_Result;
        /// <summary>지나온 방 수 (이력 갱신 통지)</summary>
        public IReadOnlyIntValue HistoryCount => m_HistoryCount;
        /// <summary>이번 Ability 방 리롤 횟수</summary>
        public IReadOnlyIntValue RerollCount => m_RerollCount;
        /// <summary>지나온 방 종류 (입장 순)</summary>
        public IReadOnlyList<string> History => m_History;
        /// <summary>현재 2택 선택지 (Choosing 에서 확정)</summary>
        public IReadOnlyList<SRoomChoice> Choices => m_Choices;
        /// <summary>현재 제시 중인 능력 ID (Ability 방)</summary>
        public IReadOnlyList<string> AbilityChoices => m_AbilityChoices;
        /// <summary>다음 리롤 Crumb 비용</summary>
        public int RerollCost => TableManager.instance.Const.Ability_RerollBaseCost + m_RerollCount.v * TableManager.instance.Const.Ability_RerollCostStep;
        #endregion
        #region Value
        private IntValue m_RoomIndex;
        private StringValue m_RoomKind;
        private EnumValue<ERoomState> m_State;
        private IntValue m_WaveIndex;
        private IntValue m_WaveCount;
        private EnumValue<ERunResult> m_Result;
        private IntValue m_HistoryCount;
        private IntValue m_RerollCount;
        private readonly List<string> m_History = new();
        private readonly List<SRoomChoice> m_Choices = new();
        private readonly List<string> m_AbilityChoices = new();
        private List<WaveTable> m_Waves;
        private string m_BossId;
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
            m_RoomIndex = new IntValue(this, "RoomIndex", 0);
            m_RoomKind = new StringValue(this, "RoomKind", "");
            m_State = new EnumValue<ERoomState>(this, "RoomState", ERoomState.None);
            m_WaveIndex = new IntValue(this, "WaveIndex", 0);
            m_WaveCount = new IntValue(this, "WaveCount", 0);
            m_Result = new EnumValue<ERunResult>(this, "RunResult", ERunResult.None);
            m_HistoryCount = new IntValue(this, "HistoryCount", 0);
            m_RerollCount = new IntValue(this, "RerollCount", 0);
            base.Init();
        }
        public override void InitGame()
        {
            var battle = LocalBattleManager.instance;
            battle.AliveEnemyCount.AddChanged(this, OnAliveChanged);
            battle.IsPlayerDead.AddChanged(this, OnPlayerDead);
            battle.IsBossDead.AddChanged(this, OnBossDead);
            base.InitGame();
            StartRun();
        }
        public override void OnShutdown()
        {
            var battle = LocalBattleManager.instance;
            if (battle != null)
            {
                battle.AliveEnemyCount.RemoveChanged(this, OnAliveChanged);
                battle.IsPlayerDead.RemoveChanged(this, OnPlayerDead);
                battle.IsBossDead.RemoveChanged(this, OnBossDead);
            }
            base.OnShutdown();
        }
        #endregion
        #region Local Function
        /// <summary>살아 있는 적이 0 이면 다음 웨이브 또는 클리어로 넘긴다</summary>
        private void OnAliveChanged(ValueBase _)
        {
            if (m_State.v != ERoomState.Playing || m_RoomKind.v != RoomConst.KindBattle || 0 < LocalBattleManager.instance.AliveEnemyCount.v)
                return;
            if (m_WaveIndex.v < m_WaveCount.v)
                NextWave();
            else
                ClearRoom();
        }
        /// <summary>플레이어 사망이면 패배로 끝낸다</summary>
        private void OnPlayerDead(ValueBase _)
        {
            if (LocalBattleManager.instance.IsPlayerDead.v && m_State.v != ERoomState.Ended)
                EndRun(ERunResult.Lose);
        }
        /// <summary>보스 사망이면 보스방 클리어(승리)다</summary>
        private void OnBossDead(ValueBase _)
        {
            if (LocalBattleManager.instance.IsBossDead.v && m_State.v == ERoomState.Playing && m_RoomKind.v == RoomConst.KindBoss)
                ClearRoom();
        }
        /// <summary>_kind 방에 입장한다 — 이력 기록 후 종류별 진행을 시작한다</summary>
        private void EnterRoom(string _kind)
        {
            var battle = LocalBattleManager.instance;
            battle.ClearUnits();
            m_History.Add(_kind);
            m_HistoryCount.v = m_History.Count;
            m_RoomKind.v = _kind;
            m_State.v = ERoomState.Playing;
            switch (_kind)
            {
                case RoomConst.KindBattle:
                    m_Waves = RoomUtil.GetWaves(m_RoomIndex.v);
                    m_WaveCount.v = m_Waves.Count;
                    m_WaveIndex.Set(0, false, false);
                    NextWave();
                    break;
                case RoomConst.KindHeal:
                    battle.HealPlayer(TableManager.instance.Const.Room_HealRatio);
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
                    battle.SpawnUnit(m_BossId, m_SpawnRight.position, RoomUtil.GetHpScale(m_RoomIndex.v), RoomUtil.GetAtkScale(m_RoomIndex.v));
                    break;
                default:
                    throw new ArgumentException($"Room 테이블에 없는 방 종류 : {_kind}", nameof(_kind));
            }
        }
        /// <summary>다음 웨이브를 스폰한다</summary>
        private void NextWave()
        {
            m_WaveIndex.v += 1;
            LocalBattleManager.instance.SpawnWave(m_Waves[m_WaveIndex.v - 1], RoomUtil.GetHpScale(m_RoomIndex.v), RoomUtil.GetAtkScale(m_RoomIndex.v), m_SpawnLeft.position, m_SpawnRight.position, m_EnemySpacing);
        }
        /// <summary>방 클리어 — 해금·최고 순번 갱신 후 보스방이면 승리, 아니면 선택지를 뽑아 선택 팝업을 연다</summary>
        private void ClearRoom()
        {
            if (CharacterManager.instance.OnRoomCleared(m_RoomIndex.v) && Popup_Notify.instance != null)
            {
                var language = LanguageManager.instance;
                Popup_Notify.instance.Open(new Popup_Notify.SOption(null, language.Get(RoomConst.TextGunUnlock), language.Get(RoomConst.TextConfirm), null));
            }
            if (m_RoomKind.v == RoomConst.KindBoss)
            {
                EndRun(ERunResult.Win);
                return;
            }
            RollChoices();
            m_State.v = ERoomState.Choosing;
            LocalPopupManager.instance.Open(RoomConst.PopupRoomSelect);
        }
        /// <summary>선택지 세트를 골라 두 선택지를 만든다 — Room_BossForce 클리어 후 [Battle/Boss] 고정, Room_BossMin 이후 보스 세트 포함</summary>
        private void RollChoices()
        {
            var c = TableManager.instance.Const;
            int next = m_RoomIndex.v + 1;
            string set;
            if (c.Room_BossForce <= m_RoomIndex.v)
                set = c.Room_ChoiceSet3;
            else
            {
                var sets = new List<string> { c.Room_ChoiceSet1, c.Room_ChoiceSet2, c.Room_ChoiceSet4 };
                if (c.Room_BossMin <= next)
                    sets.Add(c.Room_ChoiceSet3);
                set = sets[UnityEngine.Random.Range(0, sets.Count)];
            }
            var (left, right) = RoomUtil.ParseChoiceSet(set);
            m_Choices.Clear();
            m_Choices.Add(MakeChoice(left, next));
            m_Choices.Add(MakeChoice(right, next));
        }
        /// <summary>_kind 방 선택지를 만든다 (Battle 은 웨이브 합계, Boss 는 추첨 보스 1)</summary>
        private SRoomChoice MakeChoice(string _kind, int _roomIndex)
        {
            switch (_kind)
            {
                case RoomConst.KindBattle:
                    return new SRoomChoice(_kind, null, RoomUtil.GetPreview(_roomIndex));
                case RoomConst.KindBoss:
                    string bossId = RoomUtil.RollBoss();
                    return new SRoomChoice(_kind, bossId, new[] { new SEnemyPreview(bossId, 1) });
                default:
                    return new SRoomChoice(_kind, null, Array.Empty<SEnemyPreview>());
            }
        }
        /// <summary>상한에 닿지 않은 능력 중 Ability_ChoiceCount 개를 무작위로 제시한다</summary>
        private void RollAbilities()
        {
            var battle = LocalBattleManager.instance;
            var pool = new List<string>();
            foreach (var id in TableManager.instance.Ability.ID)
                if (battle.CanAddAbility(id))
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
        /// <summary>런을 _result 로 끝내고 결과 팝업을 연다</summary>
        private void EndRun(ERunResult _result)
        {
            m_Result.v = _result;
            m_State.v = ERoomState.Ended;
            LocalPopupManager.instance.Open(RoomConst.PopupResult);
        }
        /// <summary>런을 시작한다 — 재화·전투·이력을 초기화하고 플레이어를 스폰한 뒤 1번째 방(Battle)에 입장한다</summary>
        private void StartRun()
        {
            BattleManager.instance.ResetRun();
            LocalBattleManager.instance.ResetRun();
            m_History.Clear();
            m_HistoryCount.Set(0, false, false);
            m_Result.Set(ERunResult.None, false, false);
            m_BossId = null;

            var playerGo = LocalCharacterManager.instance.SpawnPlayer(m_PlayerSpawn.position);
            var player = playerGo.GetComponent<Object_UnitBase>();
            if (player == null)
                throw new InvalidOperationException($"{playerGo.name} 에 Object_UnitBase 이 없다");
            player.Spawn(m_PlayerSpawn.position, 1f, 1f);
            if (LocalCameraManager.instance != null)
                LocalCameraManager.instance.SetFollow(player.transform, m_CameraLerp, m_CameraClampX, m_CameraFixedY);

            m_RoomIndex.Set(1, true, false);
            EnterRoom(RoomConst.KindBattle);
        }
        #endregion
        #region Function
        /// <summary>_index(0·1) 선택지의 방으로 들어간다. Choosing 이 아니거나 범위 밖이면 예외</summary>
        public void SelectRoom(int _index)
        {
            if (m_State.v != ERoomState.Choosing)
                throw new InvalidOperationException($"방 선택 중이 아니다 : {m_State.v}");
            if (_index < 0 || m_Choices.Count <= _index)
                throw new ArgumentOutOfRangeException(nameof(_index), $"선택지 범위 밖 : {_index}");
            var choice = m_Choices[_index];
            LocalPopupManager.instance.Close(RoomConst.PopupRoomSelect);
            m_BossId = choice.BossId;
            m_RoomIndex.v += 1;
            EnterRoom(choice.Kind);
        }
        /// <summary>제시 중인 _abilityId 를 얻고 Ability 방을 클리어한다. 제시 목록에 없으면 예외</summary>
        public void SelectAbility(string _abilityId)
        {
            if (m_State.v != ERoomState.Playing || m_RoomKind.v != RoomConst.KindAbility || !m_AbilityChoices.Contains(_abilityId))
                throw new InvalidOperationException($"제시 중이 아닌 능력 : {_abilityId}");
            LocalBattleManager.instance.AddAbility(_abilityId);
            m_AbilityChoices.Clear();
            LocalPopupManager.instance.Close(RoomConst.PopupAbility);
            ClearRoom();
        }
        /// <summary>Crumb 를 지불하고 능력 제시를 다시 뽑는다. 잔액이 모자라면 false</summary>
        public bool RerollAbility()
        {
            if (m_State.v != ERoomState.Playing || m_RoomKind.v != RoomConst.KindAbility)
                throw new InvalidOperationException("Ability 방이 아니다");
            if (DealManager.instance.Pay(new SDeal(BattleConst.CrumbId, "", RerollCost)) == null)
                return false;
            m_RerollCount.v += 1;
            RollAbilities();
            return true;
        }
        /// <summary>런을 버리고 로비 씬으로 전환한다</summary>
        public void ReturnLobby()
        {
            SceneChangeManager.instance.SceneChange(SceneChangeManager.instance.LobbySceneID);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            _report.AddNumber("roomIndex", m_RoomIndex.v);
            _report.Add("roomKind", m_RoomKind.v);
            _report.Add("state", m_State.v.ToString());
            _report.AddNumber("waveIndex", m_WaveIndex.v);
            _report.AddNumber("waveCount", m_WaveCount.v);
            _report.Add("result", m_Result.v.ToString());
            _report.Add("history", string.Join(",", m_History));
            for (int i = 0; i < m_Choices.Count; i++)
                _report.Add($"choice{i}", $"{m_Choices[i].Kind}{(m_Choices[i].BossId != null ? ":" + m_Choices[i].BossId : "")}");
            _report.Add("abilityChoices", string.Join(",", m_AbilityChoices));
            _report.AddNumber("rerollCost", RerollCost);
        }
        public override void MCPInteraction(MCPReport _report)
        {
            if (m_State.v == ERoomState.Choosing)
                for (int i = 0; i < m_Choices.Count; i++)
                    _report.Add($"SelectRoom{i}", $"{m_Choices[i].Kind} 방 선택");
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
            if (_interactionId.StartsWith("SelectRoom") && int.TryParse(_interactionId.Substring("SelectRoom".Length), out var index))
            {
                SelectRoom(index);
                return "{\"success\":true}";
            }
            if (_interactionId.StartsWith("SelectAbility_"))
            {
                SelectAbility(_interactionId.Substring("SelectAbility_".Length));
                return "{\"success\":true}";
            }
            switch (_interactionId)
            {
                case "RerollAbility":
                    return RerollAbility() ? "{\"success\":true}" : "{\"error\":\"Crumb 부족\"}";
                case "ReturnLobby":
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
            {
                _report.Add("WinRun", "런 승리 처리");
                _report.Add("LoseRun", "런 패배 처리");
            }
        }
        public override string MCPCheatApply(string _cheatId)
        {
            switch (_cheatId)
            {
                case "ClearRoom":
                    LocalBattleManager.instance.ClearUnits();
                    m_AbilityChoices.Clear();
                    LocalPopupManager.instance.Close(RoomConst.PopupAbility);
                    ClearRoom();
                    return "{\"success\":true}";
                case "WinRun":
                    EndRun(ERunResult.Win);
                    return "{\"success\":true}";
                case "LoseRun":
                    EndRun(ERunResult.Lose);
                    return "{\"success\":true}";
            }
            return base.MCPCheatApply(_cheatId);
        }
#endif
        #endregion
    }
}
