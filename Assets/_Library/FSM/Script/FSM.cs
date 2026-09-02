using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>유한 상태 기계. 상태 목록을 등록하고 Update/FixedUpdate로 현재 상태를 전환한다. 오브젝트에 붙여 수동으로 Init한다</summary>
    public class FSM : MonoBehaviour, IManageValue
    {
        #region Inspector
        [SerializeField, TabGroup("Component"), LabelText("상태 목록")] private FSMState[] m_States;
        [SerializeField, TabGroup("Component"), LabelText("기본 상태")] private FSMState m_DefaultState;
        #endregion
        #region Property
        /// <summary>현재 상태 ID (구독용)</summary>
        public IReadOnlyStringValue CurState => m_State;
        /// <summary>현재 상태 객체. 초기화 전이면 null</summary>
        public FSMState CurStateObject => m_StateDic != null && m_StateDic.TryGetValue(m_State.v ?? "", out var s) ? s : null;
        #endregion
        #region Value
        private Dictionary<string, FSMState> m_StateDic;
        private StringValue m_State;
        private List<ValueBase> m_ManageValue = new();
        #endregion

        #region Event
        /// <summary>등록된 상태들을 초기화하고 기본 상태로 진입한다. 소유 오브젝트가 직접 호출해야 하며, 상태 목록이 없거나 ID 가 비었거나 겹치면 예외</summary>
        public void Init()
        {
            if (m_States == null)
                throw new InvalidOperationException(name);

            m_StateDic = new Dictionary<string, FSMState>();
            foreach (var v in m_States)
            {
                if (v == null || string.IsNullOrEmpty(v.ID))
                    throw new InvalidOperationException(name);
                if (m_StateDic.ContainsKey(v.ID))
                    throw new InvalidOperationException(v.ID);
                m_StateDic[v.ID] = v;
            }
            m_State = new StringValue(this, "state", "");
            foreach (var v in m_States)
                v.Init(this);
            if (m_DefaultState)
                Set(m_DefaultState);
        }
        // 상태가 자기 자신이 아닌 것을 돌려주면 그게 곧 전환 요청이다
        protected virtual void Update()
        {
            var cur = CurStateObject;
            if (cur)
            {
                var next = cur.OnUpdate();
                if (next != cur)
                    Set(next);
            }
        }
        protected virtual void FixedUpdate()
        {
            var cur = CurStateObject;
            if (cur)
            {
                var next = cur.OnFixedUpdate();
                if (next != cur)
                    Set(next);
            }
        }
        #endregion
        #region Manual Function
        void IManageValue.ManageValue(ValueBase _value)
        {
            m_ManageValue.Add(_value);
        }
        #endregion
        #region Function
        /// <summary>_id 상태를 반환한다. 미등록이면 null 이며, 초기화 전 호출은 예외</summary>
        public FSMState GetState(string _id)
        {
            if (m_StateDic == null)
                throw new InvalidOperationException(name);
            return m_StateDic.TryGetValue(_id, out var s) ? s : null;
        }
        /// <summary>_id 상태를 T 로 반환한다. 미등록이거나 타입이 다르면 null</summary>
        public T GetState<T>(string _id) where T : FSMState => GetState(_id) as T;
        /// <summary>_state 로 전환하고 이전 상태의 OnEnd·새 상태의 OnStart 를 부른다. 비활성 상태이거나 이미 그 상태면 무시하며, 등록되지 않은 상태는 예외</summary>
        public void Set(FSMState _state)
        {
            if (m_StateDic == null)
                throw new InvalidOperationException(name);
            if (_state == null)
                throw new ArgumentNullException(nameof(_state));
            if (!m_StateDic.ContainsValue(_state))
                throw new ArgumentException(_state.ID);
            if (!_state.IsEnable) return;
            if (CurStateObject == _state) return;
            FSMState old = CurStateObject;
            m_State.Set(_state.ID, true, false);
            old?.OnEnd();
            _state.OnStart();
        }
        /// <summary>_id 상태로 전환한다. 미등록 _id 면 예외</summary>
        public void Set(string _id)
        {
            var state = GetState(_id);
            if (state == null)
                throw new ArgumentException(_id);
            Set(state);
        }
        #endregion
    }
}
