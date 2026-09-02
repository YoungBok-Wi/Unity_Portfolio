using System;
using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>Slider 컴포넌트를 래핑하여 값 변경 이벤트를 관리</summary>
    [RequireComponent(typeof(Slider))]
    public class UIWrapper_Slider : ControlBase
    {
        #region Property
        /// <summary>감싸고 있는 Slider</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public Slider v
        {
            get
            {
                if (m_Slider == null)
                    m_Slider = GetComponent<Slider>();
                return m_Slider;
            }
        }
        #endregion
        #region Value
        private Slider m_Slider;
        private Action<UIWrapper_Slider> m_OnChanged;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            v.onValueChanged.AddListener(OnValueChanged);
        }
        /// <summary>Slider 의 값 변경을 리스너에 넘긴다</summary>
        private void OnValueChanged(float value)
        {
            m_OnChanged?.Invoke(this);
        }
        #endregion
        #region Function
        /// <summary>슬라이더 값을 _value 로 바꾼다. 범위 밖 값은 Slider 가 잘라내며, 값이 실제로 바뀌면 리스너도 호출된다</summary>
        public void Set(float _value)
        {
            v.value = _value;
        }
        /// <summary>슬라이더 범위를 _min~_max 로 바꾼다</summary>
        public void SetMinMax(float _min, float _max)
        {
            v.minValue = _min;
            v.maxValue = _max;
        }
        /// <summary>값 변경 리스너를 등록한다. _onClick 은 이 슬라이더를 받으며, 값은 v.value 에서 읽는다</summary>
        public void AddChangeListener(Action<UIWrapper_Slider> _onClick)
        {
            m_OnChanged += _onClick;
        }
        #endregion
    }
}