using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>씬 상주 플레이어 중 저장된 캐릭터 하나만 활성화한다.</summary>
    public class LocalPlayerCharacterManager : LocalManagerBase
    {
        public static LocalPlayerCharacterManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("씬에 함께 배치된 플레이어 목록")] private Object_PlayerBase[] m_Players;
        #endregion
        #region Property
        /// <summary>현재 활성 플레이어를 반환한다.</summary>
        public Object_PlayerBase Player => m_Player;
        #endregion
        #region Value
        private Object_PlayerBase m_Player;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override bool RequireInit()
        {
            return DataManager.instance != null && DataManager.instance.IsInited;
        }
        public override void Init()
        {
            DataManager.instance.SelectedId.AddChanged(this, OnSelectedChanged);
            SelectPlayer();
            base.Init();
        }
        public override void OnShutdown()
        {
            if (DataManager.instance != null)
                DataManager.instance.SelectedId.RemoveChanged(this, OnSelectedChanged);
            base.OnShutdown();
        }
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        private void OnSelectedChanged(ValueBase _)
        {
            SelectPlayer();
        }
        #endregion
        #region Local Function
        private void SelectPlayer()
        {
            if (m_Players == null || m_Players.Length == 0)
                throw new InvalidOperationException($"{name} : 씬 상주 플레이어가 배선되지 않았다");
            string selectedId = DataManager.instance.SelectedId.v;
            Object_PlayerBase selected = null;
            foreach (var candidate in m_Players)
            {
                if (candidate == null)
                    throw new InvalidOperationException($"{name} : 플레이어 배열에 빈 슬롯이 있다");
                bool isSelected = candidate.Id == selectedId;
                candidate.gameObject.SetActive(isSelected);
                if (isSelected)
                {
                    if (selected != null)
                        throw new InvalidOperationException($"{name} : 캐릭터 ID가 겹친다 ({selectedId})");
                    selected = candidate;
                }
            }
            if (selected == null)
                throw new InvalidOperationException($"{name} : 선택 캐릭터 {selectedId}가 없다");
            m_Player = selected;
            if (LocalGameManager.instance != null)
                LocalGameManager.instance.SetPlayer(Player);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            var data = DataManager.instance;
            _report.Add("selected", data.SelectedId.v);
            _report.AddRaw("gunUnlocked", data.GunUnlocked.v ? "true" : "false");
            _report.AddNumber("bestRoom", data.BestRoom.v);
            _report.Add("player", Player != null ? Player.Id : "");
        }
        public override void MCPInteraction(MCPReport _report)
        {
            var data = DataManager.instance;
            foreach (var id in TableManager.instance.Character.ID)
                if (data.IsUnlocked(id) && data.SelectedId.v != id)
                    _report.Add($"Select_{id}", $"{id} 선택");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            if (_interactionId.StartsWith("Select_"))
            {
                DataManager.instance.Select(_interactionId.Substring("Select_".Length));
                return "{\"success\":true}";
            }
            return base.MCPInteract(_interactionId, _value);
        }
        public override void MCPCheats(MCPReport _report)
        {
            if (!DataManager.instance.GunUnlocked.v)
                _report.Add("UnlockGun", "Gun 즉시 해금");
        }
        public override string MCPCheatApply(string _cheatId)
        {
            if (_cheatId == "UnlockGun")
            {
                DataManager.instance.OnRoomCleared(TableManager.instance.Const.Room_GunUnlock);
                return "{\"success\":true}";
            }
            return base.MCPCheatApply(_cheatId);
        }
#endif
        #endregion
    }
}
