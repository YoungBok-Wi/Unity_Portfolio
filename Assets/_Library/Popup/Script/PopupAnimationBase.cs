using System;
using UnityEngine;

namespace Library
{
    /// <summary>팝업 열기·닫기 연출의 베이스. 기본 구현은 연출 없이 즉시 끝나며, 파생이 OnStartOpen·OnStartClose 를 override 해 실제 연출을 넣는다</summary>
    public class PopupAnimationBase : MonoBehaviour
    {
        #region Type
        private enum EState
        {
            None = 0,
            Open,
            Close
        }
        #endregion
        #region Value
        private EState m_State = EState.None;
        private Action m_OnEndOpen;
        private Action m_OnEndClose;
        #endregion

        #region Event
        /// <summary>소속 팝업에 자기를 열기·닫기 연출로 등록한다</summary>
        protected virtual void Awake()
        {
            // Start 가 아니라 Awake 에서 등록한다 — 팝업은 초기화 중 컨트롤 Start 전에 비활성화되어, Start 등록이면 첫 오픈에 연출이 빠진다
            GetComponentInParent<PopupBase>().OnRegisterPopupAnimation(this);
        }
        /// <summary>열기 연출을 시작한다. 파생은 이걸 override 하고 연출이 끝날 때 OnEndOpen 을 부른다</summary>
        protected virtual void OnStartOpen(Action _onEnd)
        {
            OnEndOpen();
        }
        /// <summary>열기 연출이 끝났음을 알린다</summary>
        protected virtual void OnEndOpen()
        {
            // 콜백을 먼저 떼고 부른다 — 콜백이 곧바로 다음 연출을 시작해도 그 상태를 덮지 않게
            var onEnd = m_OnEndOpen;
            m_State = EState.None;
            m_OnEndOpen = null;
            onEnd?.Invoke();
        }
        /// <summary>닫기 연출을 시작한다. 파생은 이걸 override 하고 연출이 끝날 때 OnEndClose 를 부른다</summary>
        protected virtual void OnStartClose(Action _onEnd)
        {
            OnEndClose();
        }
        /// <summary>닫기 연출이 끝났음을 알린다</summary>
        protected virtual void OnEndClose()
        {
            var onEnd = m_OnEndClose;
            m_State = EState.None;
            m_OnEndClose = null;
            onEnd?.Invoke();
        }
        /// <summary>진행 중인 연출을 접는다</summary>
        // 끊긴 연출의 완료 콜백을 대신 불러 준다 — 안 그러면 팝업이 여는·닫는 중 상태에 갇힌다
        protected virtual void OnCancelState()
        {
            if (m_State == EState.Open)
                m_OnEndOpen?.Invoke();
            else if (m_State == EState.Close)
                m_OnEndClose?.Invoke();

            m_State = EState.None;
            m_OnEndOpen = null;
            m_OnEndClose = null;
        }
        #endregion
        #region Manual Function
        /// <summary>열기 연출을 시작한다. _onEnd 는 연출이 끝나면 호출되며, 진행 중이던 연출은 접힌다</summary>
        public void StartOpen(Action _onEnd)
        {
            OnCancelState();
            // 연출 시작 전에 기록한다 — 이 값이 있어야 연출이 끊길 때 OnCancelState 가 완료 콜백을 대신 부를 수 있다
            m_State = EState.Open;
            m_OnEndOpen = _onEnd;
            OnStartOpen(_onEnd);
        }
        /// <summary>닫기 연출을 시작한다. _onEnd 는 연출이 끝나면 호출되며, 진행 중이던 연출은 접힌다</summary>
        public void StartClose(Action _onEnd)
        {
            OnCancelState();
            m_State = EState.Close;
            m_OnEndClose = _onEnd;
            OnStartClose(_onEnd);
        }
        #endregion
    }
}
