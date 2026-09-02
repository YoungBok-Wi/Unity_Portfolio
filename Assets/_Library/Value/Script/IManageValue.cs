using UnityEngine;

namespace Library
{
    /// <summary>ValueBase 의 소유자가 되는 쪽이 구현한다. 값이 생성될 때 자기 소유로 등록받는다</summary>
    public interface IManageValue
    {
        /// <summary>_value 를 자기 소유로 등록한다. ValueBase 생성자가 직접 호출한다</summary>
        public void ManageValue(ValueBase _value);
    }
}
