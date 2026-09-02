using System;
using UnityEngine;

namespace Library
{
    /// <summary>DateTime 반응형 값. 저장할 때는 ToBinary 로 직렬화되므로 시각의 종류(UTC·Local)까지 함께 보존된다</summary>
    public class DateTimeValue : ValueBase, IReadOnlyDateTimeValue
    {
        #region Property
        /// <summary>현재 값. 대입하면 값이 실제로 달라졌을 때만 변경·저장이 통지된다</summary>
        public DateTime v
        {
            get => m_Value;
            set
            {
                m_Value = value;
                OnChanged(EChangeType.NeedSave);
            }
        }
        #endregion
        #region Value
        private DateTime m_Value;
        #endregion

        #region Event
        public DateTimeValue(IManageValue _callBy, string _id, DateTime _default = default) : base(_callBy, _id)
        {
            m_Value = _default;
        }
        public override object OnSave()
        {
            return m_Value.ToBinary();
        }
        public override void OnLoad(object _data)
        {
            m_Value = DateTime.FromBinary((long)_data);
            OnChanged(EChangeType.Loaded);
        }
        public override void OnLoadString(string _data)
        {
            if (string.IsNullOrEmpty(_data))
                return;

            m_Value = DateTime.FromBinary(long.Parse(_data));
            OnChanged(EChangeType.Loaded);
        }
        #endregion
        #region Function
        /// <summary>값을 _v 로 설정한다. _isCallChanged 가 false 면 아무에게도 알리지 않고 조용히 바꾸며, _isCallSave 는 저장까지 태울지 정한다. v 대입과 달리 같은 값이어도 통지된다</summary>
        public void Set(DateTime _v, bool _isCallChanged, bool _isCallSave)
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
