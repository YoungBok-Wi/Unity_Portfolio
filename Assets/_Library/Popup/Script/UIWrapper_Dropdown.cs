using System;
using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>TMP_Dropdown 컴포넌트를 래핑하여 값 변경 이벤트를 관리</summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class UIWrapper_Dropdown : ControlBase
    {
        #region Property
        /// <summary>감싸고 있는 TMP_Dropdown</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public TMP_Dropdown v
        {
            get
            {
                if (m_Dropdown == null)
                    m_Dropdown = GetComponent<TMP_Dropdown>();
                return m_Dropdown;
            }
        }
        #endregion
        #region Value
        private TMP_Dropdown m_Dropdown;
        private Action<UIWrapper_Dropdown> m_OnChanged;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            v.onValueChanged.AddListener(OnValueChanged);
        }
        /// <summary>Dropdown 의 값 변경을 리스너에 넘긴다</summary>
        private void OnValueChanged(int value)
        {
            m_OnChanged?.Invoke(this);
        }
        #endregion
        #region Function
        /// <summary>값 변경 리스너를 등록한다. _onClick 은 이 드롭다운을 받으며, 선택 값은 v.value 에서 읽는다</summary>
        public void AddChangeListener(Action<UIWrapper_Dropdown> _onClick)
        {
            m_OnChanged += _onClick;
        }
        #endregion
    }
}