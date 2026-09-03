using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>캐릭터 전역 매니저 — 선택 캐릭터·Gun 해금·최고 도달 방 순번을 저장하고 해금 판정을 맡는다</summary>
    public class CharacterManager : GlobalManagerBase
    {
        public static CharacterManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("저장 테이블명")] private string m_SaveTable = "profile";
        [SerializeField, Tooltip("기본 선택 캐릭터 ID")] private string m_DefaultId = "Knife";
        #endregion
        #region Property
        /// <summary>선택 캐릭터 ID (Character 테이블)</summary>
        public IReadOnlyStringValue SelectedId => m_SelectedId;
        /// <summary>Gun 영구 해금 여부</summary>
        public IReadOnlyBoolValue GunUnlocked => m_GunUnlocked;
        /// <summary>최고 도달 방 순번</summary>
        public IReadOnlyIntValue BestRoom => m_BestRoom;
        #endregion
        #region Value
        private StringValue m_SelectedId;
        private BoolValue m_GunUnlocked;
        private IntValue m_BestRoom;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override bool RequireInit()
        {
            return InitUtil.IsInit(new ManagerBase[] { NumberManager.instance });
        }
        public override void Init()
        {
            m_SelectedId = SaveUtil.Create(this, m_SaveTable, new StringValue(this, "SelectedCharacter", m_DefaultId), SaveUtil.EType.DB);
            m_GunUnlocked = SaveUtil.Create(this, m_SaveTable, new BoolValue(this, "GunUnlocked", false), SaveUtil.EType.DB);
            m_BestRoom = SaveUtil.Create(this, m_SaveTable, new IntValue(this, "BestRoom", 0), SaveUtil.EType.DB);
            NumberManager.instance.Create(this, "BestRoom", m_BestRoom);
            base.Init();
        }
        #endregion
        #region Local Function
        /// <summary>_id 의 Character 테이블 행을 반환한다. 없으면 예외</summary>
        private CharacterTable GetTable(string _id)
        {
            if (!TableManager.instance.Character.Data.TryGetValue(_id, out var table))
                throw new ArgumentException($"Character 테이블에 없는 ID : {_id}", nameof(_id));
            return table;
        }
        #endregion
        #region Function
        /// <summary>_id 캐릭터가 해금됐는지 반환한다 (UnlockRoom 0 이면 항상, 아니면 GunUnlocked)</summary>
        public bool IsUnlocked(string _id)
        {
            return GetTable(_id).UnlockRoom <= 0 || m_GunUnlocked.v;
        }
        /// <summary>_id 를 선택 캐릭터로 저장한다. 미해금이면 예외</summary>
        public void Select(string _id)
        {
            if (!IsUnlocked(_id))
                throw new InvalidOperationException($"해금되지 않은 캐릭터 : {_id}");
            m_SelectedId.v = _id;
        }
        /// <summary>_roomIndex 방 클리어를 기록한다 — 최고 순번 갱신, Room_GunUnlock 이상이면 Gun 해금. 이번에 새로 해금됐으면 true</summary>
        public bool OnRoomCleared(int _roomIndex)
        {
            if (m_BestRoom.v < _roomIndex)
                m_BestRoom.v = _roomIndex;
            if (m_GunUnlocked.v || _roomIndex < TableManager.instance.Const.Room_GunUnlock)
                return false;
            m_GunUnlocked.v = true;
            return true;
        }
        #endregion
    }
}
