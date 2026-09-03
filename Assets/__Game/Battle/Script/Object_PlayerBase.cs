using Library;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>플레이어 공용 베이스 — 입력 폴링·좌우 이동·점프·모션 전환·공격 판정 범위를 처리하고 공격 방식은 파생이 정한다</summary>
    public abstract class Object_PlayerBase : Object_UnitBase
    {
        #region Inspector
        [SerializeField, Tooltip("공격 판정 범위 (반전 루트 자식, 공격 중에만 활성)")] private BoxCollider2D m_AttackRange;
        #endregion
        #region Property
        /// <summary>공격 진행 중인지 (이동 입력을 무시한다)</summary>
        public bool IsAttacking { get; protected set; }
        /// <summary>이번 프레임 공격 입력이 눌렸는지</summary>
        protected bool AttackPressed => m_AttackPressed;
        /// <summary>공격 입력이 유지 중인지</summary>
        protected bool AttackHeld => m_AttackHeld;
        /// <summary>조작 가능 상태인지 (생존·방 진행 중·시간 정지 아님)</summary>
        protected bool CanControl => !IsDead.v && 0 < Time.timeScale && LocalRoomManager.instance != null && LocalRoomManager.instance.State.v == ERoomState.Playing;
        /// <summary>전투 매니저 (없으면 null)</summary>
        protected LocalBattleManager Battle => LocalBattleManager.instance;
        #endregion
        #region Value
        private float m_MoveInput;
        private bool m_JumpPressed;
        private bool m_AttackPressed;
        private bool m_AttackHeld;
        private string m_CurAnim;
        private bool m_CurLoop;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        protected override void Update()
        {
            base.Update();
            ReadInput();
            if (!CanControl)
            {
                if (!IsDead.v)
                    UpdateMotion();
                return;
            }
            UpdateAttack();
            UpdateMotion();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsStunned || Physics == null || !CanControl)
                return;
            SetMoveSpeed(Battle != null ? Battle.GetPlayerMoveSpeed(CharacterData.MoveSpeed) : CharacterData.MoveSpeed);
            if (IsAttacking)
                return;
            if (m_MoveInput != 0)
            {
                SetFacing(m_MoveInput < 0 ? -1 : 1);
                Physics.Move(m_MoveInput);
            }
            if (m_JumpPressed && Physics.FlyState == CharacterPhysicsBase.EFlyState.None)
                Physics.Jump();
            m_JumpPressed = false;
        }
        protected override void OnSpawned()
        {
            base.OnSpawned();
            IsAttacking = false;
            m_CurAnim = null;
            SetAttackRange(false);
            PlayAnim(BattleConst.AnimIdle, true);
        }
        protected override void OnDie()
        {
            IsAttacking = false;
            SetAttackRange(false);
            base.OnDie();
        }
        /// <summary>매 프레임 공격 입력·진행을 갱신한다 (파생이 구현)</summary>
        protected abstract void UpdateAttack();
        #endregion
        #region Local Function
        /// <summary>키보드·마우스·게임패드에서 이동·점프·공격 입력을 읽는다</summary>
        private void ReadInput()
        {
            float move = 0;
            bool jump = false, attackHeld = false, attackPressed = false;
            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) move -= 1;
                if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move += 1;
                jump = kb.spaceKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame;
                attackHeld = kb.jKey.isPressed;
                attackPressed = kb.jKey.wasPressedThisFrame;
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
        /// <summary>공격 중이 아닐 때 이동·점프·대기 모션을 고른다 (피격 모션이 재생 중이면 유지)</summary>
        private void UpdateMotion()
        {
            if (Anim == null || IsAttacking)
                return;
            if (Anim.CurAction == BattleConst.AnimHit && !Anim.IsFinished)
                return;
            if (Physics != null && Physics.FlyState != CharacterPhysicsBase.EFlyState.None)
                PlayAnim(BattleConst.AnimJump, false);
            else if (m_MoveInput != 0 && CanControl)
                PlayAnim(BattleConst.AnimMove, true);
            else
                PlayAnim(BattleConst.AnimIdle, true);
        }
        #endregion
        #region Function
        /// <summary>_action 모션을 _loop 로 재생한다 — 같은 모션이 재생 중이면 다시 시작하지 않는다 (모션 전환 단일 통로)</summary>
        public void PlayAnim(string _action, bool _loop)
        {
            if (Anim == null)
                return;
            if (m_CurAnim == _action && m_CurLoop == _loop && !Anim.IsFinished)
                return;
            m_CurAnim = _action;
            m_CurLoop = _loop;
            Anim.Play(_action, _loop);
        }
        /// <summary>공격 판정 범위 오브젝트를 _isActive 로 켜고 끈다</summary>
        public void SetAttackRange(bool _isActive)
        {
            if (m_AttackRange != null)
                m_AttackRange.gameObject.SetActive(_isActive);
        }
        /// <summary>공격 판정 범위의 월드 중심·크기를 반환한다 (범위 오브젝트가 없으면 테이블 값으로 전방 계산)</summary>
        public (Vector2 center, Vector2 size) GetAttackBox()
        {
            if (m_AttackRange != null)
            {
                var t = m_AttackRange.transform;
                var lossy = t.lossyScale;
                var center = (Vector2)t.TransformPoint(m_AttackRange.offset);
                var size = new Vector2(m_AttackRange.size.x * Mathf.Abs(lossy.x), m_AttackRange.size.y * Mathf.Abs(lossy.y));
                return (center, size);
            }
            var data = CharacterData;
            return (HitPoint + Vector2.right * (Facing * data.RangeWidth * 0.5f), new Vector2(data.RangeWidth, data.RangeHeight));
        }
        /// <summary>제자리에 선다 — 공격 시작 시 수평 속도를 지운다 (공중이면 유지)</summary>
        public void StopMove()
        {
            if (Physics != null && Physics.FlyState == CharacterPhysicsBase.EFlyState.None)
                Physics.SetVelocity(new Vector2(0, Physics.Rig.linearVelocity.y));
        }
        #endregion
    }
}
