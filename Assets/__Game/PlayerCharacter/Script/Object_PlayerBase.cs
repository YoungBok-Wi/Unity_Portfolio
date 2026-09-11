using Library;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>플레이어 입력·모션·공격 훅과 Character 테이블 스탯을 제공한다.</summary>
    [DefaultExecutionOrder(-100)]
    public abstract class Object_PlayerBase : Object_UnitBase
    {
        #region Inspector
        [SerializeField, Tooltip("공격 중 활성화할 전방 판정 범위")] private BoxCollider2D m_AttackRange;
        #endregion
        #region Property
        /// <summary>로드한 Character 테이블 행을 반환한다.</summary>
        public CharacterTable CharacterData => m_CharacterData;
        /// <summary>플레이어 진영임을 반환한다.</summary>
        public override bool IsPlayerSide => true;
        /// <summary>공격 상태 여부를 반환한다.</summary>
        public bool IsAttacking => m_IsAttacking;
        internal float MoveInput => m_MoveInput;
        internal bool JumpPressed => m_JumpPressed;
        internal bool AttackPressed => m_AttackPressed;
        internal bool AttackHeld => m_AttackHeld;
        internal bool IsGrounded => Physics != null && Physics.FlyState == CharacterPhysicsBase.EFlyState.None;
        internal bool CanControl => !IsDead.v && !IsStunned && 0f < Time.timeScale && LocalRoomManager.instance != null && LocalRoomManager.instance.State.v == ERoomState.Playing;
        protected LocalGameManager Game => LocalGameManager.instance;
        internal override string KnockbackReturnState => IsGrounded && Mathf.Abs(m_MoveInput) > 0f ? UnitConst.StateMove : UnitConst.StateIdle;
        #endregion
        #region Value
        private CharacterTable m_CharacterData;
        private bool m_IsAttacking;
        private float m_MoveInput;
        private bool m_JumpPressed;
        private bool m_AttackPressed;
        private bool m_AttackHeld;
        private string m_CurAnim;
        private bool m_CurLoop;
        #endregion

        #region Event
        private void Update()
        {
            ReadInput();
        }
        protected override (int hp, float moveSpeed) LoadBase()
        {
            if (!TableManager.instance.Character.Data.TryGetValue(Id, out var data))
                throw new ArgumentException($"{name} : Character 테이블에 없는 ID {Id}");
            m_CharacterData = data;
            return (data.Hp, data.MoveSpeed);
        }
        protected override void OnSpawned()
        {
            m_IsAttacking = false;
            m_CurAnim = null;
            m_MoveInput = 0f;
            m_JumpPressed = false;
            m_AttackPressed = false;
            m_AttackHeld = false;
            SetAttackRange(false);
            PlayAnim(UnitConst.AnimIdle, true);
            base.OnSpawned();
        }
        protected override void OnHit(SHit _hit)
        {
            m_IsAttacking = false;
            SetAttackRange(false);
            if (Fsm != null && Fsm.GetState(UnitConst.StateHit) != null)
                Fsm.Set(UnitConst.StateHit);
            else
                PlayAnim(UnitConst.AnimHit, false);
            base.OnHit(_hit);
        }
        protected override void OnDie()
        {
            m_IsAttacking = false;
            SetAttackRange(false);
            base.OnDie();
        }
        protected abstract void OnAttackStart(int _step);
        protected abstract void OnAttackFrame(int _step, int _frame);
        protected abstract void OnAttackEnd();
        protected virtual string ResolveAnim(string _action) => _action;
        #endregion
        #region Local Function
        private void ReadInput()
        {
            float move = 0f;
            bool jump = false;
            bool attackHeld = false;
            bool attackPressed = false;
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move += 1f;
                jump = keyboard.spaceKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame;
                attackHeld = keyboard.jKey.isPressed;
                attackPressed = keyboard.jKey.wasPressedThisFrame;
            }
            var mouse = Mouse.current;
            if (mouse != null)
            {
                attackHeld |= mouse.leftButton.isPressed;
                attackPressed |= mouse.leftButton.wasPressedThisFrame;
            }
            var pad = Gamepad.current;
            if (pad != null)
            {
                float x = pad.leftStick.ReadValue().x;
                if (0.3f < Mathf.Abs(x)) move = Mathf.Sign(x);
                jump |= pad.buttonSouth.wasPressedThisFrame;
                attackHeld |= pad.buttonWest.isPressed;
                attackPressed |= pad.buttonWest.wasPressedThisFrame;
            }
            m_MoveInput = move;
            m_JumpPressed |= jump;
            m_AttackHeld = attackHeld;
            m_AttackPressed = attackPressed;
        }
        #endregion
        #region Function
        /// <summary>대기 중인 점프 입력을 소비한다.</summary>
        public void ConsumeJump() => m_JumpPressed = false;
        /// <summary>현재 공격을 시작할 수 있는지 반환한다.</summary>
        public bool IsAttackReady() => CanControl && !IsAttacking;
        /// <summary>능력 배율을 반영한 공격 간격을 반환한다.</summary>
        public float AttackInterval()
        {
            return Game != null ? Game.GetPlayerAttackInterval(CharacterData.AttackInterval) : CharacterData.AttackInterval;
        }
        /// <summary>_step 공격을 시작하고 판정 범위를 활성화한다.</summary>
        public void BeginAttack(int _step)
        {
            m_IsAttacking = true;
            StopMove();
            SetAttackRange(true);
            OnAttackStart(_step);
        }
        /// <summary>_step 공격의 현재 프레임 훅을 실행한다.</summary>
        public void TickAttack(int _step)
        {
            OnAttackFrame(_step, Anim != null ? Anim.CurFrame : 0);
        }
        /// <summary>현재 공격을 종료하고 판정 범위를 비활성화한다.</summary>
        public void FinishAttack()
        {
            m_IsAttacking = false;
            SetAttackRange(false);
            OnAttackEnd();
        }
        /// <summary>_input 방향으로 이동하고 방 경계를 적용한다.</summary>
        public void Move(float _input)
        {
            if (Physics == null || _input == 0f)
                return;
            SetMoveSpeed(Game != null ? Game.GetPlayerMoveSpeed(CharacterData.MoveSpeed) : CharacterData.MoveSpeed);
            SetFacing(_input < 0f ? -1 : 1);
            Physics.Move(_input);
            if (Game != null)
                Game.ResetPlayerKnockbackDrift();
            if (LocalRoomManager.instance != null)
            {
                var position = transform.position;
                position.x = Mathf.Clamp(position.x, -LocalRoomManager.instance.RoomHalfWidth, LocalRoomManager.instance.RoomHalfWidth);
                transform.position = position;
            }
        }
        /// <summary>접지 중이면 점프하고 입력을 소비한다.</summary>
        public void Jump()
        {
            if (Physics != null && IsGrounded)
                Physics.Jump();
            ConsumeJump();
        }
        /// <summary>_action 모션을 _loop 설정으로 재생한다.</summary>
        public void PlayAnim(string _action, bool _loop)
        {
            if (Anim == null)
                return;
            string action = ResolveAnim(_action);
            if (m_CurAnim == action && m_CurLoop == _loop && !Anim.IsFinished)
                return;
            m_CurAnim = action;
            m_CurLoop = _loop;
            Anim.Play(action, _loop);
        }
        /// <summary>공격 판정 범위를 _isActive 상태로 바꾼다.</summary>
        public void SetAttackRange(bool _isActive)
        {
            if (m_AttackRange != null)
                m_AttackRange.gameObject.SetActive(_isActive);
        }
        /// <summary>현재 방향의 공격 판정 중심과 크기를 반환한다.</summary>
        public (Vector2 center, Vector2 size) GetAttackBox()
        {
            if (m_AttackRange != null)
            {
                var t = m_AttackRange.transform;
                var lossy = t.lossyScale;
                return ((Vector2)t.TransformPoint(m_AttackRange.offset), new Vector2(m_AttackRange.size.x * Mathf.Abs(lossy.x), m_AttackRange.size.y * Mathf.Abs(lossy.y)));
            }
            return (HitPoint + Vector2.right * (Facing * CharacterData.RangeWidth * 0.5f), new Vector2(CharacterData.RangeWidth, CharacterData.RangeHeight));
        }
        /// <summary>플레이어의 수평 이동을 멈춘다.</summary>
        public void StopMove() => StopHorizontal();
        #endregion
    }
}
