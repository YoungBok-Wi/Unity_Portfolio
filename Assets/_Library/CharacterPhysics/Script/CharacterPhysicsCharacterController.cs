using UnityEngine;

namespace Library
{
    /// <summary>CharacterController 기반 캐릭터 물리. 자체 중력·이동 시뮬레이션으로 이동을 처리한다</summary>
    public class CharacterPhysicsCharacterController : CharacterPhysicsBase
    {
        #region Inspector
        [SerializeField] private CharacterController m_Controller;
        [SerializeField] private float m_SimScale = 1.0f;
        [SerializeField] private float m_MoveSpeed = 2f;
        [SerializeField] private float m_MoveMaxSec = 0.05f;
        [SerializeField] private float m_MoveMinSec = 0.05f;
        [SerializeField] private float m_Gravity = -9.81f;
        #endregion
        #region Property
        /// <summary>이동·중력 시뮬레이션 배율. 1보다 크면 전체가 빨라진다</summary>
        public float SimScale { get => m_SimScale; set => m_SimScale = value; }
        /// <summary>최대 이동속도</summary>
        public FloatValue MoveSpeed { get; set; }
        #endregion
        #region Value
        private bool m_IsMovedThisFrame;
        private Vector2 m_MoveVel;
        private float m_MoveEndTimer;
        private Vector2 m_Velocity;
        private float m_UpDown;
        #endregion

        #region Event
        public override void Init()
        {
            base.Init();
            MoveSpeed = new FloatValue(null, "", m_MoveSpeed);
        }
        protected void Update()
        {
            // 중력은 unscaledDeltaTime 을 쓴다 — timeScale 과 무관하게 떨어지고 SimScale 로만 조절된다
            m_UpDown += m_Gravity * Time.unscaledDeltaTime * m_SimScale;

            // m_MoveVel 은 Move 에서 이미 MoveSpeed 를 곱한 목표 속도다 — 여기서 또 곱하면 제곱이 된다
            if (m_IsMovedThisFrame)
                SetMoveVelocityLerp(m_MoveMaxSec, m_MoveVel);
            else
            {
                if (0 < m_MoveEndTimer)
                {
                    SetMoveVelocityLerp(m_MoveMinSec, Vector2.zero);
                    m_MoveEndTimer -= Time.deltaTime;
                }
                else
                    m_Velocity = Vector2.zero;
            }

            CollisionFlags flag = m_Controller.Move(new Vector3(m_Velocity.x, m_UpDown, m_Velocity.y) * Time.deltaTime * m_SimScale);
            // 바닥에 닿으면 낙하 속도를 끊는다 — 안 그러면 접지 상태로 계속 누적된다
            if ((flag & CollisionFlags.Below) != 0)
                m_UpDown = 0;

            m_IsMovedThisFrame = false;
        }
        #endregion
        #region Local Function
        /// <summary>수평속도를 목표값까지 보간한다</summary>
        private void SetMoveVelocityLerp(float _changeSec, Vector2 _move)
        {
            if (_changeSec != 0)
            {
                //축별 차이가 0이면 0 나누기로 속도가 NaN 이 된다
                float diffX = Mathf.Abs(m_Velocity.x - _move.x);
                float diffY = Mathf.Abs(m_Velocity.y - _move.y);
                float velX = (0 < diffX) ? Mathf.Lerp(m_Velocity.x, _move.x, (MoveSpeed.v / diffX) * Time.deltaTime / _changeSec) : _move.x;
                float velY = (0 < diffY) ? Mathf.Lerp(m_Velocity.y, _move.y, (MoveSpeed.v / diffY) * Time.deltaTime / _changeSec) : _move.y;
                m_Velocity = new Vector2(velX, velY);
            }
            else
                m_Velocity = _move;
        }
        #endregion
        #region Function
        /// <summary>수평 이동을 요청한다. _vec 는 최대속도에 곱할 -1~1 비율이며, _isNow 가 true 면 가속 보간 없이 즉시 적용한다. 매 프레임 호출해야 이동이 유지된다</summary>
        public void Move(Vector2 _vec, bool _isNow = false)
        {
            m_IsMovedThisFrame = true;
            m_MoveVel = MoveSpeed.v * _vec;
            m_MoveEndTimer = m_MoveMinSec;

            if (_isNow)
                m_Velocity = _vec;
        }
        #endregion
    }
}
