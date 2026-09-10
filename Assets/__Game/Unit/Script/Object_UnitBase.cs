using Library;
using UnityEngine;

namespace Game
{
    /// <summary>유닛 공통 HP·피격·넉백·경직·사망 처리를 제공한다.</summary>
    public abstract class Object_UnitBase : ObjectBase
    {
        #region Inspector
        [SerializeField, Tooltip("테이블 행 ID")] private string m_Id;
        [SerializeField, Tooltip("이동·넉백 물리")] private CharacterPhysics2DSide m_Physics;
        [SerializeField, Tooltip("프레임 애니메이터")] private SpriteAnimPlayer m_Anim;
        [SerializeField, Tooltip("상태 기계")] private FSM m_Fsm;
        [SerializeField, Tooltip("발 위치 기준 타격점 높이 (u)")] private float m_HitHeight = 0.5f;
        [SerializeField, Tooltip("선택지에 표시할 유닛 아이콘")] private Sprite m_Icon;
        #endregion
        #region Property
        /// <summary>테이블 행 ID를 반환한다.</summary>
        public string Id => m_Id;
        /// <summary>이동·넉백 물리를 반환한다.</summary>
        public CharacterPhysics2DSide Physics => m_Physics;
        /// <summary>프레임 애니메이터를 반환한다.</summary>
        public SpriteAnimPlayer Anim => m_Anim;
        /// <summary>상태 기계를 반환한다.</summary>
        public FSM Fsm => m_Fsm;
        /// <summary>선택지용 아이콘을 반환한다.</summary>
        public Sprite Icon => m_Icon;
        /// <summary>현재 HP 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue Hp => m_Hp;
        /// <summary>최대 HP 읽기 값을 반환한다.</summary>
        public IReadOnlyIntValue MaxHp => m_MaxHp;
        /// <summary>사망 여부 읽기 값을 반환한다.</summary>
        public IReadOnlyBoolValue IsDead => m_IsDead;
        /// <summary>넉백 또는 피격 경직 중인지 반환한다.</summary>
        public bool IsStunned => m_KnockElapsed < m_KnockTime || 0f < m_StunTimer;
        /// <summary>월드 타격점을 반환한다.</summary>
        public Vector2 HitPoint => (Vector2)transform.position + Vector2.up * m_HitHeight;
        /// <summary>바라보는 수평 방향을 반환한다.</summary>
        public int Facing => m_Facing;
        /// <summary>넉백 면역 여부를 반환한다.</summary>
        public virtual bool IsKnockbackImmune => false;
        /// <summary>넉백 거리 배율을 반환한다.</summary>
        public virtual float KnockbackRate => 1f;
        /// <summary>사망 시 Crumb 드롭량을 반환한다.</summary>
        public virtual int CrumbDrop => 0;
        /// <summary>플레이어 진영 여부를 반환한다.</summary>
        public abstract bool IsPlayerSide
        {
            get;
        }
        /// <summary>스폰 직후 진입할 상태 ID를 반환한다.</summary>
        protected virtual string SpawnState => UnitConst.StateIdle;
        #endregion
        #region Value
        private IntValue m_Hp;
        private IntValue m_MaxHp;
        private BoolValue m_IsDead;
        private float m_StunTimer;
        private float m_KnockElapsed;
        private float m_KnockTime;
        private float m_KnockDistance;
        private float m_KnockApplied;
        private int m_KnockDirection;
        private float m_MoveSpeedBase;
        private int m_Facing;
        private float m_AttackScale;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            if (LocalGameManager.instance != null)
                LocalGameManager.instance.OnRegisterObject(this);
            base.InitSingleton();
        }
        public override void Init()
        {
            m_Hp = new IntValue(this, "Hp", 1);
            m_MaxHp = new IntValue(this, "MaxHp", 1);
            m_IsDead = new BoolValue(this, "IsDead", false);
            m_Facing = 1;
            m_AttackScale = 1f;
            if (m_Physics != null)
                m_Physics.Init();
            if (m_Fsm != null)
                m_Fsm.Init();
            base.Init();
        }
        protected virtual void Update()
        {
            if (m_KnockElapsed < m_KnockTime || m_StunTimer <= 0f)
                return;
            m_StunTimer -= Time.deltaTime;
            if (m_StunTimer <= 0f)
            {
                SetMoveSpeed(m_MoveSpeedBase);
                StopHorizontal();
            }
        }
        protected virtual void FixedUpdate()
        {
            if (m_Physics == null || m_KnockTime <= m_KnockElapsed)
                return;
            float nextElapsed = Mathf.Min(m_KnockElapsed + Time.fixedDeltaTime, m_KnockTime);
            float progress = nextElapsed / m_KnockTime;
            var curve = LocalGameManager.instance != null ? LocalGameManager.instance.KnockbackCurve : null;
            float normalized = curve != null ? curve.Evaluate(progress) : progress;
            float targetDistance = m_KnockDistance * normalized;
            float delta = Mathf.Max(0f, targetDistance - m_KnockApplied);
            m_Physics.MoveSpeed.v = delta / Time.fixedDeltaTime;
            m_Physics.Move(m_KnockDirection, true);
            m_KnockApplied = targetDistance;
            m_KnockElapsed = nextElapsed;
            if (m_KnockTime <= m_KnockElapsed)
                m_StunTimer = Mathf.Max(0f, TableManager.instance.Const.Battle_HitStunSec);
        }
        protected abstract (int hp, float moveSpeed) LoadBase();
        protected virtual void OnSpawned()
        {
        }
        protected virtual void OnHit(SHit _hit)
        {
        }
        protected virtual void OnDie()
        {
            if (m_Anim != null)
                m_Anim.Play(UnitConst.AnimDie, false);
            if (m_Fsm != null && m_Fsm.GetState(UnitConst.StateDie) != null)
                m_Fsm.Set(UnitConst.StateDie);
            if (LocalGameManager.instance != null)
                LocalGameManager.instance.OnUnitDied(this);
        }
        #endregion
        #region Function
        /// <summary>_pos에 _hpScale·_atkScale을 반영해 유닛을 스폰한다.</summary>
        public void Spawn(Vector2 _pos, float _hpScale, float _atkScale)
        {
            transform.position = _pos;
            m_AttackScale = _atkScale;
            m_StunTimer = 0f;
            m_KnockElapsed = 0f;
            m_KnockTime = 0f;
            var (hp, moveSpeed) = LoadBase();
            m_MaxHp.Set(Mathf.Max(1, Mathf.RoundToInt(hp * _hpScale)), false, false);
            m_IsDead.Set(false, false, false);
            m_Hp.Set(m_MaxHp.v, true, false);
            SetMoveSpeed(moveSpeed);
            if (m_Physics != null)
                m_Physics.SetVelocity(Vector2.zero);
            if (m_Fsm != null && !string.IsNullOrEmpty(SpawnState))
                m_Fsm.Set(SpawnState);
            OnSpawned();
        }
        /// <summary>_hit을 적용하고 성공 여부를 반환한다.</summary>
        public bool TakeHit(SHit _hit)
        {
            if (m_IsDead.v)
                return false;
            m_Hp.v = Mathf.Max(0, m_Hp.v - _hit.Damage);
            if (!IsKnockbackImmune && m_Physics != null && 0f < _hit.KnockbackTime && 0f <= _hit.KnockbackDist)
            {
                m_KnockElapsed = 0f;
                m_KnockTime = _hit.KnockbackTime;
                m_KnockDistance = _hit.KnockbackDist * Mathf.Max(0f, KnockbackRate);
                m_KnockApplied = 0f;
                m_KnockDirection = _hit.Direction < 0 ? -1 : 1;
                m_StunTimer = 0f;
            }
            if (m_Hp.v == 0)
            {
                m_IsDead.v = true;
                OnDie();
            }
            else
                OnHit(_hit);
            return true;
        }
        /// <summary>현재 HP를 _amount만큼 회복한다.</summary>
        public void Heal(int _amount)
        {
            if (!m_IsDead.v)
                m_Hp.v = Mathf.Min(m_MaxHp.v, m_Hp.v + _amount);
        }
        /// <summary>최대 HP를 _maxHp로 바꾸고 _isHealDiff이면 증가분을 회복한다.</summary>
        public void SetMaxHp(int _maxHp, bool _isHealDiff)
        {
            int next = Mathf.Max(1, _maxHp);
            int diff = next - m_MaxHp.v;
            m_MaxHp.v = next;
            if (_isHealDiff && 0 < diff)
                Heal(diff);
            else
                m_Hp.v = Mathf.Min(m_Hp.v, m_MaxHp.v);
        }
        /// <summary>기본 이동 속도를 _speed로 바꾼다.</summary>
        public void SetMoveSpeed(float _speed)
        {
            m_MoveSpeedBase = _speed;
            if (m_Physics != null && !IsStunned)
                m_Physics.MoveSpeed.v = _speed;
        }
        /// <summary>바라보는 방향을 _facing의 부호로 바꾼다.</summary>
        public void SetFacing(int _facing)
        {
            if (_facing == 0)
                return;
            m_Facing = _facing < 0 ? -1 : 1;
            if (m_Anim != null)
                m_Anim.SetFlip(m_Facing < 0);
        }
        /// <summary>_base에 공격 배율을 적용한 정수 피해량을 반환한다.</summary>
        public int ScaleAttack(int _base) => Mathf.RoundToInt(_base * m_AttackScale);
        /// <summary>수평 이동 속도를 0으로 만든다.</summary>
        public void StopHorizontal()
        {
            if (m_Physics != null)
                m_Physics.Rig.linearVelocity = new Vector2(0f, m_Physics.Rig.linearVelocity.y);
        }
        #endregion
    }
}
