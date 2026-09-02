using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>팝업 내 UI 컨트롤의 베이스 클래스로 라이프사이클 콜백을 제공</summary>
    public class ControlBase : MonoBehaviour
    {
        #region Property
        /// <summary>활성 상태인지 여부. 비활성인 컨트롤에는 값 변경이 통지되지 않는다</summary>
        public bool IsActive { get; protected set; }
        #endregion
        #region Value
        private PopupBase m_ParentPopup;
        private List<ValueBase> m_ValueBase = new();
        #endregion

        #region Event
        protected virtual void Awake()
        {
            m_ParentPopup = GetComponentInParent<PopupBase>();
        }
        protected virtual void Start()
        {
            m_ParentPopup.OnRegisterControl(this);
        }
        protected virtual void OnEnable()
        {
            // 비활성 동안 놓친 값 변경을 여기서 한 번에 따라잡는다
            IsActive = true;
            foreach(var v in m_ValueBase)
                v.CallControlEvent(this);
        }
        protected virtual void OnDisable()
        {
            IsActive = false;
        }

        /// <summary>최초 1회만 호출된다. 구독 등록·구조 생성처럼 되풀이하면 안 되는 작업을 여기서 한다</summary>
        public virtual void InitUIOnce()
        {
        }
        /// <summary>초기화가 끝날 때마다 호출된다. 표시 갱신처럼 여러 번 해도 되는 작업을 여기서 한다</summary>
        public virtual void InitUI()
        {
        }
        /// <summary>소속 팝업이 열릴 때 호출된다</summary>
        public virtual void OnOpen()
        {
        }
        /// <summary>소속 팝업이 닫힐 때 호출된다</summary>
        public virtual void OnClose()
        {
        }
        /// <summary>시스템이 내려갈 때 호출된다</summary>
        public virtual void OnShutdown()
        {
        }
        #endregion
        #region Manual Function
        /// <summary>_v 를 이 컨트롤이 보는 값으로 등록한다. 활성화 시점에 밀린 변경을 따라잡는 대상이 되며, 중복 등록해도 한 번만 남는다</summary>
        public void AddValueBase(ValueBase _v)
        {
            m_ValueBase.Remove(_v);
            m_ValueBase.Add(_v);
        }
        /// <summary>_v 를 이 컨트롤이 보는 값에서 뺀다</summary>
        public void RemoveValueBase(ValueBase _v)
        {
            m_ValueBase.Remove(_v);
        }
        #endregion
    }
}