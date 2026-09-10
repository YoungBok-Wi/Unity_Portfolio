using Library;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>다음 방 선택지와 Battle Variant를 생성하고 선택을 전달한다.</summary>
    public class LocalRoomSelectManager : LocalManagerBase
    {
        public static LocalRoomSelectManager instance { get; private set; }

        #region Property
        /// <summary>현재 방 선택지 목록을 반환한다.</summary>
        public IReadOnlyList<SRoomChoice> Choices => m_Choices;
        #endregion
        #region Value
        private readonly List<SRoomChoice> m_Choices = new();
        #endregion

        #region Event
        public event Action<IReadOnlyList<SRoomChoice>> ChoicesChanged;
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
        public override void OnShutdown()
        {
            ChoicesChanged = null;
            base.OnShutdown();
        }
        #endregion
        #region Local Function
        private SRoomChoice MakeChoice(string _kind, int _roomIndex, int _variant)
        {
            switch (_kind)
            {
                case RoomConst.KindBattle:
                    return new SRoomChoice(_kind, null, _variant, RoomUtil.GetPreview(_roomIndex, _variant));
                case RoomConst.KindBoss:
                    string bossId = RoomUtil.RollBoss();
                    return new SRoomChoice(_kind, bossId, 0, new[] { new SEnemyPreview(bossId, 1) });
                default:
                    return new SRoomChoice(_kind, null, 0, Array.Empty<SEnemyPreview>());
            }
        }
        #endregion
        #region Function
        /// <summary>_nextIndex와 _prevKind에 맞는 다음 방 선택지를 생성한다.</summary>
        public void RollChoices(int _nextIndex, string _prevKind)
        {
            if (_nextIndex <= 0)
                throw new ArgumentOutOfRangeException(nameof(_nextIndex));
            m_Choices.Clear();
            int cycle = TableManager.instance.Const.Room_BossCycle;
            if (cycle <= 0)
                throw new InvalidOperationException($"Room_BossCycle이 0 이하다 : {cycle}");
            if (_nextIndex % cycle == 0)
            {
                m_Choices.Add(MakeChoice(RoomConst.KindBoss, _nextIndex, 0));
                ChoicesChanged?.Invoke(m_Choices);
                return;
            }

            string left;
            string right;
            if (_prevKind == RoomConst.KindHeal || _prevKind == RoomConst.KindAbility)
            {
                left = RoomConst.KindBattle;
                right = RoomConst.KindBattle;
            }
            else
            {
                var c = TableManager.instance.Const;
                string[] sets = { c.Room_ChoiceSet1, c.Room_ChoiceSet2, c.Room_ChoiceSet4 };
                (left, right) = RoomUtil.ParseChoiceSet(sets[UnityEngine.Random.Range(0, sets.Length)]);
            }

            int battleCount = (left == RoomConst.KindBattle ? 1 : 0) + (right == RoomConst.KindBattle ? 1 : 0);
            int leftVariant = 0;
            int rightVariant = 0;
            if (battleCount == 2)
            {
                bool normalFirst = UnityEngine.Random.Range(0, 2) == 0;
                leftVariant = normalFirst ? 1 : 2;
                rightVariant = normalFirst ? 2 : 1;
            }
            else if (battleCount == 1)
            {
                int variant = UnityEngine.Random.Range(1, 3);
                if (left == RoomConst.KindBattle) leftVariant = variant;
                else rightVariant = variant;
            }
            m_Choices.Add(MakeChoice(left, _nextIndex, leftVariant));
            m_Choices.Add(MakeChoice(right, _nextIndex, rightVariant));
            ChoicesChanged?.Invoke(m_Choices);
        }
        /// <summary>_index 선택지를 다음 방으로 전달한다.</summary>
        public void SelectRoom(int _index)
        {
            if (_index < 0 || m_Choices.Count <= _index)
                throw new ArgumentOutOfRangeException(nameof(_index), $"선택지 범위 밖 : {_index}");
            if (LocalRoomManager.instance == null || LocalRoomManager.instance.State.v != ERoomState.Choosing)
                throw new InvalidOperationException("방 선택 중이 아니다");
            var choice = m_Choices[_index];
            LocalPopupManager.instance.Close(RoomConst.PopupRoomSelect);
            LocalRoomManager.instance.EnterNext(choice);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            for (int i = 0; i < m_Choices.Count; i++)
                _report.Add($"choice{i}", $"{m_Choices[i].Kind}:{m_Choices[i].Variant}{(m_Choices[i].BossId != null ? ":" + m_Choices[i].BossId : "")}");
        }
        public override void MCPInteraction(MCPReport _report)
        {
            if (LocalRoomManager.instance != null && LocalRoomManager.instance.State.v == ERoomState.Choosing)
                for (int i = 0; i < m_Choices.Count; i++)
                    _report.Add($"SelectRoom{i}", $"{m_Choices[i].Kind} 방 선택");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            if (_interactionId.StartsWith("SelectRoom") && int.TryParse(_interactionId.Substring("SelectRoom".Length), out var index))
            {
                SelectRoom(index);
                return "{\"success\":true}";
            }
            return base.MCPInteract(_interactionId, _value);
        }
#endif
        #endregion
    }
}
