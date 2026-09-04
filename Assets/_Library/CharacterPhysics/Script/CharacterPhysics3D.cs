using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>Rigidbody 기반 3D 캐릭터 물리. XZ 이동·점프·중력과 바닥 충돌 기반 비행 상태를 관리한다</summary>
    public class CharacterPhysics3D : CharacterPhysicsBase
    {
        #region Inspector
        [SerializeField] private Rigidbody m_Rig;
        [SerializeField] private CapsuleCollider m_Capsule;
        [SerializeField] private PhysicsMaterial m_DefaultMat;
        [SerializeField] private PhysicsMaterial m_MoveMat;
        [SerializeField] private Vector3 m_LimitVel = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        [SerializeField] private float m_MoveSpeed = 2f;
        [SerializeField] private float m_MoveMaxSec = 0.05f;
        [SerializeField] private float m_MoveMinSec = 0.05f;
        [SerializeField] private float m_JumpPower = 6f;
        [SerializeField] private float m_JumpMoveChangeFac = 5f;
        [SerializeField, Tooltip("낙하·점프 시 총 중력 배율(1=기본). Rigidbody 기본 중력에 (배율-1)배를 매 물리프레임 수동 추가해 배율×기본으로 만든다. 값이 클수록 무겁게 떨어지고 점프가 짧아진다")] private float m_GravityScale = 2f;
        #endregion
        #region Property
        /// <summary>현재 비행 상태. 바닥에 닿아 있으면 항상 None</summary>
        public EFlyState FlyState => 0 < m_GroundCol.Count ? EFlyState.None : m_FlyState;
        /// <summary>제어 대상 Rigidbody</summary>
        public Rigidbody Rig => m_Rig;
        /// <summary>최대 이동속도</summary>
        public FloatValue MoveSpeed { get; set; }
        /// <summary>점프 상승속도</summary>
        public FloatValue JumpPower { get; private set; }
        /// <summary>비행 상태가 바뀔 때 바뀐 상태로 호출된다</summary>
        public Action<EFlyState> OnFlyStateChanged;
        #endregion
        #region Value
        private List<Collider> m_GroundCol = new List<Collider>();
        private EFlyState m_FlyState;
        private bool m_IsMovedThisFrame;
        private Vector2 m_MoveVel;
        private float m_MoveEndTimer;
        #endregion

        #region Event
        public override void Init()
        {
            base.Init();
            m_FlyState = EFlyState.Float;
            MoveSpeed = new FloatValue(null, "", m_MoveSpeed);
            JumpPower = new FloatValue(null, "", m_JumpPower);
        }
        protected virtual void FixedUpdate()
        {
            // Rigidbody 가 이미 1배 중력을 적용하므로 (배율-1)배만 더해 총 배율을 맞춘다. Acceleration 이라 질량과 무관하고, 접지 중엔 바닥이 받쳐 주므로 적용하지 않는다
            if (m_GravityScale != 1f && FlyState != EFlyState.None)
                m_Rig.AddForce((m_GravityScale - 1f) * Physics.gravity, ForceMode.Acceleration);

            if (m_IsMovedThisFrame)
            {
                float changeSec = m_MoveMaxSec * ((FlyState == EFlyState.Jump) ? m_JumpMoveChangeFac : 1);
                // m_MoveVel 은 Move 에서 이미 MoveSpeed 를 곱한 목표 속도다 — 여기서 또 곱하면 제곱이 된다
                SetMoveVelocityLerp(changeSec, m_MoveVel);
                m_Capsule.sharedMaterial = m_MoveMat;
            }
            else
            {
                if (FlyState == EFlyState.None)
                {
                    if (0 < m_MoveEndTimer)
                    {
                        SetMoveVelocityLerp(m_MoveMinSec, Vector2.zero);
                        m_MoveEndTimer -= Time.deltaTime;
                    }
                    else
                    {
                        m_Rig.linearVelocity = new Vector3(0, m_Rig.linearVelocity.y, 0);
                        m_Capsule.sharedMaterial = m_DefaultMat;
                    }
                }
                else
                    m_Capsule.sharedMaterial = m_MoveMat;
            }

            m_Rig.linearVelocity = new Vector3(
                Mathf.Clamp(m_Rig.linearVelocity.x, -m_LimitVel.x, m_LimitVel.x),
                Mathf.Clamp(m_Rig.linearVelocity.y, -m_LimitVel.y, m_LimitVel.y),
                Mathf.Clamp(m_Rig.linearVelocity.z, -m_LimitVel.z, m_LimitVel.z));

            m_IsMovedThisFrame = false;
        }
        private void OnCollisionEnter(Collision _col)
        {
            Vector3 avgPos = Vector3.zero;
            for (int i = 0; i < _col.contactCount; ++i)
                avgPos += _col.contacts[i].point;
            avgPos /= _col.contactCount;

            if (avgPos.y < transform.position.y)
                AddGroundCol(_col.collider);
        }
        private void OnCollisionStay(Collision _col)
        {
            if (FlyState != EFlyState.Jump)
            {
                Vector3 avgPos = Vector3.zero;
                for (int i = 0; i < _col.contactCount; ++i)
                    avgPos += _col.contacts[i].point;
                avgPos /= _col.contactCount;

                if (avgPos.y < transform.position.y)
                    AddGroundCol(_col.collider);
                else
                    RemoveGroundCol(_col.collider);
            }
        }
        private void OnCollisionExit(Collision _col)
        {
            RemoveGroundCol(_col.collider);
        }
        #endregion
        #region Local Function
        /// <summary>XZ 속도를 목표값까지 보간한다</summary>
        private void SetMoveVelocityLerp(float _changeSec, Vector2 _move)
        {
            if (_changeSec != 0)
            {
                //축별 차이가 0이면 0 나누기로 속도가 NaN 이 된다
                float diffX = Mathf.Abs(m_Rig.linearVelocity.x - _move.x);
                float diffZ = Mathf.Abs(m_Rig.linearVelocity.z - _move.y);
                float velX = (0 < diffX) ? Mathf.Lerp(m_Rig.linearVelocity.x, _move.x, (MoveSpeed.v / diffX) * Time.deltaTime / _changeSec) : _move.x;
                float velZ = (0 < diffZ) ? Mathf.Lerp(m_Rig.linearVelocity.z, _move.y, (MoveSpeed.v / diffZ) * Time.deltaTime / _changeSec) : _move.y;
                m_Rig.linearVelocity = new Vector3(velX, m_Rig.linearVelocity.y, velZ);
            }
            else
                m_Rig.linearVelocity = new Vector3(_move.x, m_Rig.linearVelocity.y, _move.y);
        }
        /// <summary>바닥 충돌을 등록한다</summary>
        private void AddGroundCol(Collider _col)
        {
            if (!m_GroundCol.Contains(_col))
                m_GroundCol.Add(_col);
            m_FlyState = EFlyState.Float;
        }
        /// <summary>바닥 충돌을 해제하고 상태가 바뀌면 알린다</summary>
        private void RemoveGroundCol(Collider _col)
        {
            EFlyState prev = FlyState;
            m_GroundCol.Remove(_col);
            if (prev != FlyState)
                OnFlyStateChanged?.Invoke(FlyState);
        }
        /// <summary>바닥 충돌을 모두 비워 공중 상태로 만든다</summary>
        private void ClearGroundCol()
        {
            EFlyState prev = FlyState;
            m_GroundCol.Clear();
            if (prev != FlyState)
                OnFlyStateChanged?.Invoke(FlyState);
        }
        #endregion
        #region Function
        /// <summary>XZ 이동을 요청한다. _vec 는 최대속도에 곱할 -1~1 비율(y 가 월드 Z)이며, _isNow 가 true 면 보간 없이 즉시 적용한다. 매 물리 프레임 호출해야 이동이 유지된다</summary>
        public void Move(Vector2 _vec, bool _isNow = false)
        {
            m_IsMovedThisFrame = true;
            m_MoveVel = MoveSpeed.v * _vec;
            m_MoveEndTimer = m_MoveMinSec;

            if (_isNow)
                m_Rig.linearVelocity = new Vector3(_vec.x, m_Rig.linearVelocity.y, _vec.y);
        }
        /// <summary>점프시킨다. _power 는 점프 세기에 곱할 배율이다. 접지 여부를 확인하지 않으므로 공중에서도 그대로 점프한다</summary>
        public void Jump(float _power = 1.0f)
        {
            m_Rig.linearVelocity = new Vector3(m_Rig.linearVelocity.x, JumpPower.v * _power, m_Rig.linearVelocity.z);
            m_FlyState = EFlyState.Jump;
            ClearGroundCol();
        }
        #endregion
    }
}
