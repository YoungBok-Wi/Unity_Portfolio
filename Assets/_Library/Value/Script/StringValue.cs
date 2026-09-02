using System;
using UnityEngine;

namespace Library
{
    /// <summary>string 반응형 값. 변경 감지·저장·로드를 지원한다</summary>
    public class StringValue : ValueBase, IReadOnlyStringValue
    {
        #region Property
        /// <summary>현재 값. 대입하면 값이 실제로 달라졌을 때만 변경·저장이 통지된다</summary>
        public string v
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
        private string m_Value;
        #endregion

        #region Event
        public StringValue(IManageValue _callBy, string _id, string _default = default) : base(_callBy, _id)
        {
            m_Value = _default;
        }
        public override object OnSave()
        {
            return m_Value;
        }
        public override void OnLoad(object _data)
        {
            m_Value = (string)_data;
            OnChanged(EChangeType.Loaded);
        }
        public override void OnLoadString(string _data)
        {
            if (string.IsNullOrEmpty(_data))
                return;

            m_Value = _data;
            OnChanged(EChangeType.Loaded);
        }
        #endregion
        #region Function
        /// <summary>값을 _v 로 설정한다. _isCallChanged 가 false 면 아무에게도 알리지 않고 조용히 바꾸며, _isCallSave 는 저장까지 태울지 정한다. v 대입과 달리 같은 값이어도 통지된다</summary>
        public void Set(string _v, bool _isCallChanged, bool _isCallSave)
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