using Sirenix.OdinInspector;
using UnityEngine;

namespace Library
{
    /// <summary>쿼터뷰 2D 캐릭터 물리. XY 평면 자유 이동을 가감속 보간으로 처리한다</summary>
    public class CharacterPhysics2DQuater : CharacterPhysics2D
    {
        #region Inspector
        [SerializeField, TabGroup("Option"), LabelText("기본 이동속도")] private float m_MoveSpeed = 2f;
        [SerializeField, TabGroup("Option"), LabelText("0->이속 시간")] private float m_MoveMaxSec = 0.05f;
        [SerializeField, TabGroup("Option"), LabelText("이속->0 시간")] private float m_MoveMinSec = 0.05f;
        #endregion
        #region Property
        /// <summary>최대 이동속도</summary>
        public FloatValue MoveSpeed { get; private set; }
        /// <summary>정지에서 최대속도까지 가속에 걸리는 시간(초)</summary>
        public FloatValue MoveMaxSec { get; private set; }
        /// <summary>최대속도에서 정지까지 감속에 걸리는 시간(초)</summary>
        public FloatValue MoveMinSec { get; private set; }
        #endregion
        #region Value
        private bool m_IsMovedThisFrame;
        private Vector2 m_MoveVel;
        private float m_MoveEndTimer;
        #endregion

        #region Event
        public override void Init()
        {
            base.Init();
            MoveSpeed = new FloatValue(null, "", m_MoveSpeed);
            MoveMaxSec = new FloatValue(null, "", m_MoveMaxSec);
            MoveMinSec = new FloatValue(null, "", m_MoveMinSec);
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (m_IsMovedThisFrame)
                SetMoveVelocityLerp(MoveMaxSec.v, m_MoveVel);
            else
            {
                if (0 < m_MoveEndTimer)
                {
                    SetMoveVelocityLerp(MoveMinSec.v, Vector2.zero);
                    m_MoveEndTimer -= Time.deltaTime;
                }
                else
                    Rig.linearVelocity = Vector2.zero;
            }

            m_IsMovedThisFrame = false;
        }
        #endregion
        #region Local Function
        /// <summary>속도를 목표값까지 보간한다</summary>
        private void SetMoveVelocityLerp(float _changeSec, Vector2 _move)
        {
            if (_changeSec != 0)
            {
                //축별 차이가 0이면 0 나누기로 속도가 NaN 이 된다
                float diffX = Mathf.Abs(Rig.linearVelocity.x - _move.x);
                float diffY = Mathf.Abs(Rig.linearVelocity.y - _move.y);
                float velX = (0 < diffX) ? Mathf.Lerp(Rig.linearVelocity.x, _move.x, (MoveSpeed.v / diffX) * Time.deltaTime / _changeSec) : _move.x;
                float velY = (0 < diffY) ? Mathf.Lerp(Rig.linearVelocity.y, _move.y, (MoveSpeed.v / diffY) * Time.deltaTime / _changeSec) : _move.y;
                Rig.linearVelocity = new Vector2(velX, velY);
            }
            else
                Rig.linearVelocity = _move;
        }
        #endregion
        #region Function
        /// <summary>속도를 _vec 로 직접 설정한다. 이번 프레임의 Move 요청보다 우선한다</summary>
        public override void SetVelocity(Vector2 _vec)
        {
            base.SetVelocity(_vec);
            m_IsMovedThisFrame = false;
        }
        /// <summary>XY 이동을 요청한다. _vec 는 최대속도에 곱할 -1~1 비율이며, _isNow 가 true 면 가속 보간 없이 즉시 적용한다. 매 물리 프레임 호출해야 이동이 유지된다</summary>
        public void Move(Vector2 _vec, bool _isNow = false)
        {
            m_IsMovedThisFrame = true;
            m_MoveVel = MoveSpeed.v * _vec;
            m_MoveEndTimer = MoveMinSec.v;

            if (_isNow)
                Rig.linearVelocity = _vec;
        }
        #endregion
    }
}
