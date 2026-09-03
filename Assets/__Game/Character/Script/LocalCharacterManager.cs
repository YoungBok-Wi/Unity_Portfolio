using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>씬 캐릭터 매니저 — 선택 캐릭터의 플레이어 프리팹을 스폰·회수하고 선택·해금 상태를 MCP 로 노출한다</summary>
    public class LocalCharacterManager : LocalManagerBase
    {
        public static LocalCharacterManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("플레이어 프리팹 목록 (이름 Object_Player_{캐릭터 ID})")] private GameObject[] m_PlayerPrefabs;
        [SerializeField, Tooltip("스폰한 플레이어를 둘 루트 (없으면 씬 루트)")] private Transform m_PlayerRoot;
        #endregion
        #region Property
        /// <summary>스폰된 플레이어 오브젝트. 없으면 null</summary>
        public GameObject Player { get; private set; }
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
        #endregion
        #region Local Function
        /// <summary>스폰된 플레이어를 없앤다 (없으면 무시)</summary>
        private void DespawnPlayer()
        {
            if (Player == null)
                return;
            Destroy(Player);
            Player = null;
        }
        #endregion
        #region Function
        /// <summary>선택 캐릭터의 프리팹을 _pos 에 스폰해 반환한다 (기존 플레이어는 회수). 프리팹이 없으면 예외</summary>
        public GameObject SpawnPlayer(Vector3 _pos)
        {
            string id = CharacterManager.instance.SelectedId.v;
            string prefabName = $"Object_Player_{id}";
            GameObject prefab = null;
            foreach (var candidate in m_PlayerPrefabs)
                if (candidate != null && candidate.name == prefabName)
                    prefab = candidate;
            if (prefab == null)
                throw new InvalidOperationException($"{name} : 플레이어 프리팹 {prefabName} 이 등록되지 않았다");
            DespawnPlayer();
            Player = Instantiate(prefab, _pos, Quaternion.identity, m_PlayerRoot);
            Player.name = prefabName;
            return Player;
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            var mgr = CharacterManager.instance;
            _report.Add("selected", mgr.SelectedId.v);
            _report.AddRaw("gunUnlocked", mgr.GunUnlocked.v ? "true" : "false");
            _report.AddNumber("bestRoom", mgr.BestRoom.v);
            _report.AddRaw("playerSpawned", Player != null ? "true" : "false");
        }
        public override void MCPInteraction(MCPReport _report)
        {
            var mgr = CharacterManager.instance;
            foreach (var id in TableManager.instance.Character.ID)
                if (mgr.IsUnlocked(id) && mgr.SelectedId.v != id)
                    _report.Add($"Select_{id}", $"{id} 선택");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            if (_interactionId.StartsWith("Select_"))
            {
                CharacterManager.instance.Select(_interactionId.Substring("Select_".Length));
                return "{\"success\":true}";
            }
            return base.MCPInteract(_interactionId, _value);
        }
        public override void MCPCheats(MCPReport _report)
        {
            if (!CharacterManager.instance.GunUnlocked.v)
                _report.Add("UnlockGun", "Gun 즉시 해금");
        }
        public override string MCPCheatApply(string _cheatId)
        {
            if (_cheatId == "UnlockGun")
            {
                CharacterManager.instance.OnRoomCleared(TableManager.instance.Const.Room_GunUnlock);
                return "{\"success\":true}";
            }
            return base.MCPCheatApply(_cheatId);
        }
#endif
        #endregion
    }
}
