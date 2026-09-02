using System;
using UnityEngine;

namespace Library
{
    /// <summary>struct 반응형 값. 저장할 때는 JSON 으로 직렬화되므로 대상 구조체가 [Serializable] 이어야 한다</summary>
    public class StructValue<T> : ValueBase, IReadOnlyStructValue<T> where T : struct
    {
        #region Property
        /// <summary>현재 값. 구조체 복사본이라 필드만 바꿔서는 반영되지 않으니, 꺼내 고친 뒤 다시 대입해야 한다</summary>
        public T v
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
        private T m_Value;
        #endregion

        #region Event
        public StructValue(IManageValue _callBy, string _id, T _default = default) : base(_callBy, _id)
        {
            m_Value = _default;
        }
        public override object OnSave()
        {
            return JsonUtility.ToJson(m_Value);
        }
        public override void OnLoad(object _data)
        {
            m_Value = JsonUtility.FromJson<T>((string)_data);
            OnChanged(EChangeType.Loaded);
        }
        public override void OnLoadString(string _data)
        {
            if (string.IsNullOrEmpty(_data))
                return;

            m_Value = JsonUtility.FromJson<T>(_data);
            OnChanged(EChangeType.Loaded);
        }
        #endregion
        #region Function
        /// <summary>값을 _v 로 설정한다. _isCallChanged 가 false 면 아무에게도 알리지 않고 조용히 바꾸며, _isCallSave 는 저장까지 태울지 정한다</summary>
        public void Set(T _v, bool _isCallChanged, bool _isCallSave)
        {
            m_Value = _v;
            if (_isCallChanged)
                OnChanged(_isCallSave ? EChangeType.NeedSave : EChangeType.None);
        }
        #endregion
    }
}