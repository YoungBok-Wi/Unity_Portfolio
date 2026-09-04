using UnityEngine;

namespace Library
{
    /// <summary>Rigidbody2D 기반 캐릭터 물리 베이스. 속도 설정과 축별 속도 제한을 처리한다</summary>
    public abstract class CharacterPhysics2D : CharacterPhysicsBase
    {
        #region Inspector
        [SerializeField] private Rigidbody2D m_Rig;
        [SerializeField] private Vector2 m_LimitVel = new Vector2(float.MaxValue, float.MaxValue);
        #endregion
        #region Property
        /// <summary>제어 대상 Rigidbody2D</summary>
        public Rigidbody2D Rig => m_Rig;
        /// <summary>축별 속도 상한. 매 물리 프레임 ±이 값으로 잘린다</summary>
        public Vector2 LimitVelocity { get => m_LimitVel; set => m_LimitVel = value; }
        #endregion

        #region Event
        protected virtual void FixedUpdate()
        {
            m_Rig.linearVelocity = new Vector2(
                Mathf.Clamp(m_Rig.linearVelocity.x, -m_LimitVel.x, m_LimitVel.x),
                Mathf.Clamp(m_Rig.linearVelocity.y, -m_LimitVel.y, m_LimitVel.y));
        }
        #endregion
        #region Function
        /// <summary>속도를 _vec 로 직접 설정한다. 제한은 다음 물리 프레임에 걸리므로 여기서는 그대로 들어간다</summary>
        public virtual void SetVelocity(Vector2 _vec)
        {
            m_Rig.linearVelocity = _vec;
        }
        #endregion
    }
}
