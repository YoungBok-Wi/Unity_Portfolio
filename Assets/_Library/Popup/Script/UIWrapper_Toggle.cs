using System;
using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>Toggle 컴포넌트를 래핑하여 값 변경 이벤트를 관리</summary>
    [RequireComponent(typeof(Toggle))]
    public class UIWrapper_Toggle : ControlBase
    {
        #region Property
        /// <summary>감싸고 있는 Toggle</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public Toggle v
        {
            get
            {
                if (m_Toggle == null)
                    m_Toggle = GetComponent<Toggle>();
                return m_Toggle;
            }
        }
        #endregion
        #region Value
        private Toggle m_Toggle;
        private Action<UIWrapper_Toggle, bool> m_OnValueChanged;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            v.onValueChanged.AddListener(OnValueChanged);
        }
        /// <summary>Toggle 의 값 변경을 리스너에 넘긴다</summary>
        private void OnValueChanged(bool _dummy)
        {
            m_OnValueChanged?.Invoke(this, v.isOn);
        }
        #endregion
        #region Function
        /// <summary>토글 상태를 _isOn 으로 바꾼다. 값이 실제로 바뀌면 리스너도 호출된다</summary>
        public void SetIsOn(bool _isOn)
        {
            v.isOn = _isOn;
        }
        /// <summary>토글 활성 여부를 _isInteractable 로 바꾼다</summary>
        public void SetIsInteractable(bool _isInteractable)
        {
            v.interactable = _isInteractable;
        }
        /// <summary>값 변경 리스너를 등록한다. _onValueChanged 는 이 토글과 바뀐 상태를 받는다</summary>
        public void AddValueChangedListener(Action<UIWrapper_Toggle, bool> _onValueChanged)
        {
            m_OnValueChanged += _onValueChanged;
        }
        #endregion
    }
}