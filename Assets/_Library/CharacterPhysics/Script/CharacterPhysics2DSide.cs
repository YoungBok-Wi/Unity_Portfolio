using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>횡스크롤 2D 캐릭터 물리. 좌우 이동·점프·중력과 바닥 충돌 기반 비행 상태를 관리한다</summary>
    public class CharacterPhysics2DSide : CharacterPhysics2D
    {
        #region Inspector
        [SerializeField] private PhysicsMaterial2D m_DefaultMat;
        [SerializeField] private PhysicsMaterial2D m_MoveMat;
        [SerializeField] private float m_MoveSpeed = 2f;
        [SerializeField] private float m_MoveMaxSec = 0.05f;
        [SerializeField] private float m_MoveMinSec = 0.05f;
        [SerializeField] private float m_JumpPower = 6f;
        [SerializeField] private float m_JumpMoveChangeFac = 1.0f;
        [SerializeField] private string[] m_IgnoreLayer;
        #endregion
        #region Property
        /// <summary>현재 비행 상태. 바닥에 닿아 있으면 항상 None</summary>
        public EFlyState FlyState => 0 < m_GroundCol.Count ? EFlyState.None : m_FlyState;
        /// <summary>최대 이동속도</summary>
        public FloatValue MoveSpeed { get; set; }
        /// <summary>정지에서 최대속도까지 가속에 걸리는 시간(초)</summary>
        public FloatValue MoveMaxSec { get; private set; }
        /// <summary>최대속도에서 정지까지 감속에 걸리는 시간(초)</summary>
        public FloatValue MoveMinSec { get; private set; }
        /// <summary>점프 상승속도</summary>
        public FloatValue JumpPower { get; private set; }
        /// <summary>Rigidbody2D 중력 배율. 매 물리 프레임 Rigidbody 에 반영된다</summary>
        public FloatValue GravityScale { get; private set; }
        /// <summary>점프 중 가속 시간에 곱하는 배율. 1보다 크면 공중 조작이 둔해진다</summary>
        public FloatValue JumpMoveChangeFac { get; private set; }
        #endregion
        #region Value
        private List<Collider2D> m_GroundCol = new List<Collider2D>();
        private EFlyState m_FlyState;
        private int m_JumpFlyUpdate;
        private bool m_IsMovedThisFrame;
        private float m_MoveVel;
        private float m_MoveEndTimer;
        private Action<EFlyState> m_OnFlyStateChanged;
        #endregion

        #region Event
        public override void Init()
        {
            base.Init();
            if (m_IgnoreLayer != null)
                foreach (var v in m_IgnoreLayer)
                    if (string.IsNullOrEmpty(v) || LayerMask.NameToLayer(v) == -1)
                        throw new InvalidOperationException($"{name}의 바닥 무시 레이어에 존재하지 않는 레이어가 있다: {v}");
            m_FlyState = EFlyState.Float;
            MoveSpeed = new FloatValue(null, "", m_MoveSpeed);
            MoveMaxSec = new FloatValue(null, "", m_MoveMaxSec);
            MoveMinSec = new FloatValue(null, "", m_MoveMinSec);
            JumpPower = new FloatValue(null, "", m_JumpPower);
            GravityScale = new FloatValue(null, "", Rig.gravityScale);
            JumpMoveChangeFac = new FloatValue(null, "", m_JumpMoveChangeFac);
        }
        protected override void FixedUpdate()
        {
            Rig.gravityScale = GravityScale.v;

            if (m_IsMovedThisFrame)
            {
                if (m_MoveVel < 0 && Rig.linearVelocity.x < m_MoveVel) { }
                else if (0 < m_MoveVel && m_MoveVel < Rig.linearVelocity.x) { }
                else
                {
                    float changeSec = MoveMaxSec.v * ((FlyState == EFlyState.Jump) ? JumpMoveChangeFac.v : 1);
                    SetMoveVelocityLerp(changeSec, m_MoveVel);
                }
                Rig.sharedMaterial = m_MoveMat;
            }
            else
            {
                if (FlyState == EFlyState.None)
                {
                    if (0 < m_MoveEndTimer)
                    {
                        SetMoveVelocityLerp(MoveMinSec.v, 0);
                        m_MoveEndTimer -= Time.deltaTime;
                    }
                    else
                    {
                        Rig.linearVelocity = new Vector2(0, Rig.linearVelocity.y);
                        Rig.sharedMaterial = m_DefaultMat;
                    }
                }
                else
                    Rig.sharedMaterial = m_MoveMat;
            }

            EFlyState flyState = FlyState;
            if (flyState == EFlyState.Jump || flyState == EFlyState.Fly)
                ++m_JumpFlyUpdate;

            m_IsMovedThisFrame = false;
            base.FixedUpdate();
        }
        private void OnCollisionEnter2D(Collision2D _col)
        {
            Vector2 avgNor = Vector2.zero;
            Vector2 avgPos = Vector2.zero;
            foreach (var v in _col.contacts)
            {
                avgNor += v.normal;
                avgPos += v.point;
            }
            avgNor /= _col.contactCount;
            avgPos /= _col.contactCount;

            if (0 < avgNor.y && avgPos.y < transform.position.y)
                AddGroundCol(_col.collider);
        }
        private void OnCollisionStay2D(Collision2D _col)
        {
            if (5 < m_JumpFlyUpdate || FlyState != EFlyState.Jump && FlyState != EFlyState.Fly)
            {
                Vector2 avgNor = Vector2.zero;
                Vector2 avgPos = Vector2.zero;
                foreach (var v in _col.contacts)
                {
                    avgNor += v.normal;
                    avgPos += v.point;
                }
                avgNor /= _col.contactCount;
                avgPos /= _col.contactCount;

                if (0 < avgNor.y && avgPos.y < transform.position.y)
                    AddGroundCol(_col.collider);
                else
                    RemoveGroundCol(_col.collider);
            }
        }
        private void OnCollisionExit2D(Collision2D _col)
        {
            RemoveGroundCol(_col.collider);
        }
        #endregion
        #region Local Function
        /// <summary>수평속도를 목표값까지 보간한다</summary>
        private void SetMoveVelocityLerp(float _changeSec, float _move)
        {
            float diff = Mathf.Abs(Rig.linearVelocity.x - _move);
            if (0 < _changeSec && 0 < diff)  //diff 가 0이면 0 나누기로 속도가 NaN 이 된다
            {
                float velX = Mathf.Lerp(Rig.linearVelocity.x, _move, (MoveSpeed.v / diff) * Time.deltaTime / _changeSec);
                Rig.linearVelocity = new Vector2(velX, Rig.linearVelocity.y);
            }
            else
                Rig.linearVelocity = new Vector2(_move, Rig.linearVelocity.y);
        }
        /// <summary>바닥 충돌을 등록하고 상태가 바뀌면 알린다 (무시 레이어는 건너뛴다)</summary>
        private void AddGroundCol(Collider2D _col)
        {
            foreach (var v in m_IgnoreLayer)
                if (_col.gameObject.layer == LayerMask.NameToLayer(v))
                    return;

            EFlyState prev = FlyState;
            if (!m_GroundCol.Contains(_col))
                m_GroundCol.Add(_col);

            if (prev != FlyState)
            {
                m_FlyState = EFlyState.Float;
                m_OnFlyStateChanged?.Invoke(FlyState);
            }
        }
        /// <summary>바닥 충돌을 해제하고 상태가 바뀌면 알린다</summary>
        private void RemoveGroundCol(Collider2D _col)
        {
            EFlyState prev = FlyState;
            m_GroundCol.Remove(_col);
            if (prev != FlyState)
                m_OnFlyStateChanged?.Invoke(FlyState);
        }
        #endregion
        #region Function
        /// <summary>속도를 _vec 로 직접 설정한다. _vec.y 가 양수면 Fly 상태로 전환하고 바닥 충돌을 비운다</summary>
        public override void SetVelocity(Vector2 _vec)
        {
            base.SetVelocity(_vec);
            m_IsMovedThisFrame = false;
            if (0 < _vec.y)
            {
                m_FlyState = EFlyState.Fly;
                m_JumpFlyUpdate = 0;
                ClearGroundCol();
            }
        }
        /// <summary>좌우 이동을 요청한다. _vec 는 최대속도에 곱할 -1~1 비율이며, _isNow 가 true 면 가속 보간 없이 즉시 적용한다. 매 물리 프레임 호출해야 이동이 유지된다</summary>
        public void Move(float _vec, bool _isNow = false)
        {
            m_IsMovedThisFrame = true;
            m_MoveVel = MoveSpeed.v * _vec;
            m_MoveEndTimer = MoveMinSec.v;

            if (_isNow)
                Rig.linearVelocity = new Vector2(MoveSpeed.v * _vec, Rig.linearVelocity.y);
        }
        /// <summary>점프시킨다. _power 는 점프 세기에 곱할 배율이다. 접지 여부를 확인하지 않으므로 공중에서도 그대로 점프한다</summary>
        public void Jump(float _power = 1.0f)
        {
            Rig.linearVelocity = new Vector2(Rig.linearVelocity.x, JumpPower.v * _power);
            m_FlyState = EFlyState.Jump;
            m_JumpFlyUpdate = 0;
            ClearGroundCol();
        }
        /// <summary>수평속도를 -_speed ~ _speed 로 제한한다 (수직속도는 건드리지 않는다)</summary>
        public void Clamp(float _speed)
        {
            Rig.linearVelocity = new Vector2(Mathf.Clamp(Rig.linearVelocity.x, -_speed, _speed), Rig.linearVelocity.y);
        }
        /// <summary>바닥 충돌 목록을 비워 강제로 공중 상태로 만든다. 상태가 바뀌면 변경 이벤트가 발생한다</summary>
        public void ClearGroundCol()
        {
            EFlyState prev = FlyState;
            m_GroundCol.Clear();
            if (prev != FlyState)
                m_OnFlyStateChanged?.Invoke(FlyState);
        }
        /// <summary>비행 상태 변경 리스너를 등록한다. _func 는 바뀐 EFlyState 를 받는다. _isCallNow 가 true 면 등록 즉시 현재 상태로 한 번 호출한다</summary>
        public void AddFlyStateChangeEvent(Action<EFlyState> _func, bool _isCallNow = true)
        {
            m_OnFlyStateChanged += _func;
            if (_isCallNow)
                _func(FlyState);
        }
        /// <summary>AddFlyStateChangeEvent 로 등록한 비행 상태 변경 콜백 _func 를 해제한다</summary>
        public void RemoveFlyStateChangeEvent(Action<EFlyState> _func)
        {
            m_OnFlyStateChanged -= _func;
        }
        #endregion
    }
}
