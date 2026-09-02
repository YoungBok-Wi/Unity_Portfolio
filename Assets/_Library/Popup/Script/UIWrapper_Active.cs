using UnityEngine;

namespace Library
{
    /// <summary>배선한 오브젝트 하나를 켜고 끄는 컨트롤. 조건별 표시 전환의 베이스로 쓴다</summary>
    public class UIWrapper_Active : ControlBase
    {
        #region Inspector
        [SerializeField] private GameObject m_Target;
        #endregion
        #region Function
        /// <summary>배선된 대상을 _active 로 켜고 끈다. 대상이 배선돼 있지 않으면 예외</summary>
        public void SetTarget(bool _active)
        {
            m_Target.SetActive(_active);
        }
        #endregion
    }
}
