using Sirenix.OdinInspector;
using UnityEngine;

namespace Library
{
    /// <summary>FSM의 개별 상태 베이스. 진입·갱신·종료 콜백과 다음 상태 반환을 정의한다</summary>
    public abstract class FSMState : MonoBehaviour
    {
        #region Inspector
        [SerializeField, TabGroup("Base"), LabelText("ID")] private string m_ID;
        #endregion
        #region Property
        /// <summary>상태 ID. 같은 FSM 안에서 겹치면 안 된다</summary>
        public string ID => m_ID;
        /// <summary>이 상태를 가진 FSM. Init 전에는 null</summary>
        public FSM Parent { get; private set; }
        /// <summary>지금 이 상태로 넘어와도 되는지 여부. false 면 전환 요청이 무시된다</summary>
        public virtual bool IsEnable => true;
        #endregion

        #region Event
        /// <summary>상태를 _parent 에 연결한다. FSM 이 자기 Init 에서 부른다</summary>
        public void Init(FSM _parent)
        {
            Parent = _parent;
            OnInit();
        }
        /// <summary>참조 캐싱 등 1회 준비를 한다</summary>
        protected virtual void OnInit() { }
        /// <summary>이 상태에 들어올 때 호출된다</summary>
        protected internal virtual void OnStart() { }
        /// <summary>매 프레임 호출된다. 다른 상태를 돌려주면 그리로 전환하고, 자신을 돌려주면 머문다</summary>
        protected internal virtual FSMState OnUpdate() => this;
        /// <summary>매 물리 프레임 호출된다. 다른 상태를 돌려주면 그리로 전환하고, 자신을 돌려주면 머문다</summary>
        protected internal virtual FSMState OnFixedUpdate() => this;
        /// <summary>이 상태에서 나갈 때 호출된다</summary>
        protected internal virtual void OnEnd() { }
        #endregion
    }
}
