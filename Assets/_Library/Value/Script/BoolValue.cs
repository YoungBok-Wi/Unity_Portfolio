using System;
using UnityEngine;

namespace Library
{
    /// <summary>bool 반응형 값. 변경 감지·저장·로드를 지원한다</summary>
    public class BoolValue : ValueBase, IReadOnlyBoolValue
    {
        #region Property
        /// <summary>현재 값. 대입하면 값이 실제로 달라졌을 때만 변경·저장이 통지된다</summary>
        public bool v
        {
            get => m_Value;
            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    OnChanged(EChangeType.NeedSave);
                }
            }
        }
        #endregion
        #region Value
        private bool m_Value;
        #endregion

        #region Event
        public BoolValue(IManageValue _callBy, string _id, bool _default = default) : base(_callBy, _id)
        {
            m_Value = _default;
        }
        public override object OnSave()
        {
            return m_Value;
        }
        public override void OnLoad(object _data)
        {
            m_Value = (bool)_data;
            OnChanged(EChangeType.Loaded);
        }
        public override void OnLoadString(string _data)
        {
            if (string.IsNullOrEmpty(_data))
                return;

            m_Value = bool.Parse(_data);
            OnChanged(EChangeType.Loaded);
        }
        #endregion
        #region Function
        /// <summary>값을 _v 로 설정한다. _isCallChanged 가 false 면 아무에게도 알리지 않고 조용히 바꾸며, _isCallSave 는 저장까지 태울지 정한다. v 대입과 달리 같은 값이어도 통지된다</summary>
        public void Set(bool _v, bool _isCallChanged, bool _isCallSave)
        {
            if (_isCallChanged)
            {
                m_Value = _v;
                OnChanged(_isCallSave ? EChangeType.NeedSave : EChangeType.None);
            }
            else
                m_Value = _v;
        }
        #endregion
    }
}