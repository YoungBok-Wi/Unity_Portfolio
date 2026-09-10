using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>캐릭터 선택·해금·최고 방과 런 Crumb 상태를 관리한다.</summary>
    public class DataManager : GlobalManagerBase
    {
        public static DataManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("저장 테이블명")] private string m_SaveTable = "profile";
        [SerializeField, Tooltip("기본 선택 캐릭터 ID")] private string m_DefaultId = "Knife";
        #endregion
        #region Property
        /// <summary>선택 캐릭터 ID 읽기 값을 반환한다.</summary>
        public IReadOnlyStringValue SelectedId => m_SelectedId;
        /// <summary>Gun 해금 여부 읽기 값을 반환한다.</summary>
        public IReadOnlyBoolValue GunUnlocked => m_GunUnlocked;
        /// <summary>최고 방 순번 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue BestRoom => m_BestRoom;
        /// <summary>현재 Crumb 잔액 읽기 값을 반환한다.</summary>
        public IReadOnlyLongValue Crumb => m_Crumb;
        /// <summary>현재 런의 누적 Crumb 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue CrumbTotal => m_CrumbTotal;
        #endregion
        #region Value
        private StringValue m_SelectedId;
        private BoolValue m_GunUnlocked;
        private IntValue m_BestRoom;
        private LongValue m_Crumb;
        private IntValue m_CrumbTotal;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override bool RequireInit()
        {
            return InitUtil.IsInit(new ManagerBase[] { NumberManager.instance, BankManager.instance });
        }
        public override void Init()
        {
            m_SelectedId = SaveUtil.Create(this, m_SaveTable, new StringValue(this, "SelectedCharacter", m_DefaultId), SaveUtil.EType.DB);
            m_GunUnlocked = SaveUtil.Create(this, m_SaveTable, new BoolValue(this, "GunUnlocked", false), SaveUtil.EType.DB);
            m_BestRoom = SaveUtil.Create(this, m_SaveTable, new IntValue(this, "BestRoom", 0), SaveUtil.EType.DB);
            NumberManager.instance.Create(this, "BestRoom", m_BestRoom);
            m_Crumb = BankManager.instance.Create(this, DataConst.CrumbId, "", "", 0, false);
            m_CrumbTotal = new IntValue(this, "CrumbTotal", 0);
            NumberManager.instance.Create(this, "CrumbTotal", m_CrumbTotal);
            base.Init();
        }
        #endregion
        #region Local Function
        private CharacterTable GetTable(string _id)
        {
            if (!TableManager.instance.Character.Data.TryGetValue(_id, out var table))
                throw new ArgumentException($"Character 테이블에 없는 ID : {_id}", nameof(_id));
            return table;
        }
        #endregion
        #region Function
        /// <summary>_id 캐릭터가 해금됐는지 반환한다.</summary>
        public bool IsUnlocked(string _id) => GetTable(_id).UnlockRoom <= 0 || m_GunUnlocked.v;
        /// <summary>해금된 _id 캐릭터를 선택한다.</summary>
        public void Select(string _id)
        {
            if (!IsUnlocked(_id))
                throw new InvalidOperationException($"해금되지 않은 캐릭터 : {_id}");
            m_SelectedId.v = _id;
        }
        /// <summary>_roomIndex 클리어를 저장하고 Gun을 새로 해금했는지 반환한다.</summary>
        public bool OnRoomCleared(int _roomIndex)
        {
            if (m_BestRoom.v < _roomIndex)
                m_BestRoom.v = _roomIndex;
            if (m_GunUnlocked.v || _roomIndex < TableManager.instance.Const.Room_GunUnlock)
                return false;
            m_GunUnlocked.v = true;
            return true;
        }
        /// <summary>Crumb 잔액과 런 누적량에 _amount를 더한다.</summary>
        public void AddCrumb(int _amount)
        {
            if (_amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(_amount), $"Crumb 적립량이 0 이하다 : {_amount}");
            BankManager.instance.Change(DataConst.CrumbId, _amount);
            m_CrumbTotal.v += _amount;
        }
        /// <summary>현재 런의 Crumb 잔액과 누적량을 0으로 초기화한다.</summary>
        public void ResetRun()
        {
            BankManager.instance.Set(DataConst.CrumbId, 0);
            m_CrumbTotal.v = 0;
        }
        #endregion
    }
}
