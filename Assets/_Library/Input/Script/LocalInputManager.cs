using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Library
{
    /// <summary>입력 관리 매니저, PlayerInput 콜백 우선순위(UI→Game) 분배</summary>
    public class LocalInputManager : LocalManagerBase
    {
        public static LocalInputManager instance { get; private set; }

        #region Type
        /// <summary>입력을 넘겨 받는 순서. 앞선 우선순위가 입력을 삼키면 뒤로 넘어가지 않는다</summary>
        public enum EPriority
        {
            UI,
            Game,
            /// <summary>개수 표시용 센티넬. 실제 우선순위가 아니다</summary>
            End
        }
        #endregion
        #region Value
        private PlayerInput m_PlayerInput;
        private Dictionary<string, List<Func<InputAction.CallbackContext, bool>>[]> m_OnInputMap = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 이 매니저가 없는 씬에서 파괴된 인스턴스 접근을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        public override void Init()
        {
            m_PlayerInput = transform.GetComponent<PlayerInput>();
            if (m_PlayerInput == null)
                throw new InvalidOperationException($"{name}에 PlayerInput 컴포넌트가 없다");
            m_PlayerInput.onActionTriggered += OnInput;

            base.Init();
        }

        /// <summary>입력을 우선순위 순으로 넘긴다. 콜백이 true 를 돌려주면 거기서 멈춘다</summary>
        // 같은 우선순위 안에서는 나중에 등록한 쪽이 먼저 받는다 — 나중에 열린 팝업이 입력을 먼저 가져간다
        public void OnInput(InputAction.CallbackContext _context)
        {
            if (m_OnInputMap.TryGetValue(_context.action.name, out var priorityArray))
                for (int i = 0; i < (int)EPriority.End; i++)
                {
                    var list = priorityArray[i];
                    if (list == null) continue;
                    for (int j = list.Count - 1; j >= 0; j--)
                        if (list[j].Invoke(_context))
                            return;
                }
        }
        #endregion
        #region Function
        /// <summary>_name 입력 액션에 _callback 을 _priority 로 등록한다. _callback 이 true 를 돌려주면 그 입력은 뒤로 넘어가지 않는다. 중복 등록은 무시되며, _callBy 는 초기화 전이어야 한다</summary>
        public void Create(LocalManagerBase _callBy, string _name, EPriority _priority, Func<InputAction.CallbackContext, bool> _callback)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy));
            if (_callBy.IsInited)
                throw new InvalidOperationException($"{_callBy.name} 초기화 후에는 등록할 수 없다 — LocalInputManager.Create({_name})");
            if (_callback == null)
                throw new ArgumentNullException(nameof(_callback));
            if (_priority < EPriority.UI || EPriority.End <= _priority)
                throw new ArgumentException($"유효하지 않은 우선순위: {_priority}", nameof(_priority));

            if (!m_OnInputMap.TryGetValue(_name, out var array))
            {
                array = new List<Func<InputAction.CallbackContext, bool>>[(int)EPriority.End];
                m_OnInputMap.Add(_name, array);
            }

            int index = (int)_priority;
            if (array[index] == null)
                array[index] = new List<Func<InputAction.CallbackContext, bool>>();

            if (!array[index].Contains(_callback))
                array[index].Add(_callback);
        }
        /// <summary>_screenPos 의 터치·클릭을 월드 조작에 써도 되는지 반환한다. 열린 팝업의 UI 위면 false 이며, 팝업 매니저가 없으면 항상 true</summary>
        public bool CanProcessTouch(Vector2 _screenPos)
        {
            if (LocalPopupManager.instance == null)
                return true;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = _screenPos;

            foreach (var id in LocalPopupManager.instance.IDs)
            {
                var popup = LocalPopupManager.instance.Get(id);
                if (!popup.IsOpened) continue;

                List<RaycastResult> result = new List<RaycastResult>();
                var raycaster = popup.PopupGraphicRaycaster;
                if (raycaster)
                {
                    raycaster.Raycast(eventData, result);
                    if (0 < result.Count)
                        return false;
                }
            }

            return true;
        }
        #endregion
    }
}