using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>플레이어·적·보스 공용 유닛 베이스 — 테이블 스탯·HP·피격·넉백·경직·사망을 처리한다</summary>
    public abstract class Object_Unit : ObjectBase
    {
        #region Inspector
        [SerializeField, Tooltip("유닛 종류 (스탯 테이블 선택)")] private EUnitKind m_Kind;
        [SerializeField, Tooltip("테이블 행 ID (Knife·Apple·Pumpkin 등)")] private string m_Id;
        [SerializeField, Tooltip("이동·넉백 물리 (없으면 넉백 생략)")] private CharacterPhysics2DSide m_Physics;
        [SerializeField, Tooltip("프레임 애니메이터 (없으면 재생 생략)")] private SpriteAnimPlayer m_Anim;
        [SerializeField, Tooltip("상태 기계 (적·보스만)")] private FSM m_Fsm;
        [SerializeField, Tooltip("발 위치 기준 타격점 높이 (u)")] private float m_HitHeight = 0.5f;
        #endregion
        #region Property
        /// <summary>유닛 종류</summary>
        public EUnitKind Kind => m_Kind;
        /// <summary>테이블 행 ID</summary>
        public string Id => m_Id;
        /// <summary>진영 (플레이어 외 전부 Enemy)</summary>
        public EBattleTeam Team => m_Kind == EUnitKind.Player ? EBattleTeam.Player : EBattleTeam.Enemy;
        /// <summary>이동 물리. 없으면 null</summary>
        public CharacterPhysics2DSide Physics => m_Physics;
        /// <summary>프레임 애니메이터. 없으면 null</summary>
        public SpriteAnimPlayer Anim => m_Anim;
        /// <summary>상태 기계. 없으면 null</summary>
        public FSM Fsm => m_Fsm;
        /// <summary>현재 HP</summary>
        public IReadOnlyIntValue Hp => m_Hp;
        /// <summary>최대 HP (방 성장·능력 반영)</summary>
        public IReadOnlyIntValue MaxHp => m_MaxHp;
        /// <summary>사망 여부</summary>
        public IReadOnlyBoolValue IsDead => m_IsDead;
        /// <summary>넉백 경직 중인지</summary>
        public bool IsStunned => 0 < m_StunTimer;
        /// <summary>타격 판정·이펙트 기준점 (발 위치 + 높이)</summary>
        public Vector2 HitPoint => (Vector2)transform.position + Vector2.up * m_HitHeight;
        /// <summary>바라보는 방향 (+1 우, -1 좌)</summary>
        public int Facing => m_Facing;
        /// <summary>Enrage 전환 여부 (보스)</summary>
        public bool IsEnraged => m_IsEnraged;
        /// <summary>Enemy 테이블 행. 적이 아니면 null</summary>
        public EnemyTable EnemyData => m_EnemyData;
        /// <summary>Boss 테이블 행. 보스가 아니면 null</summary>
        public BossTable BossData => m_BossData;
        /// <summary>Character 테이블 행. 플레이어가 아니면 null</summary>
        public CharacterTable CharacterData => m_CharacterData;
        #endregion
        #region Value
        private IntValue m_Hp;
        private IntValue m_MaxHp;
        private BoolValue m_IsDead;
        private float m_StunTimer;
        private float m_KnockSpeed;
        private int m_KnockDir;
        private float m_MoveSpeedBase;
        private int m_Facing;
        private float m_AttackScale;
        private bool m_IsEnraged;
        private EnemyTable m_EnemyData;
        private BossTable m_BossData;
        private CharacterTable m_CharacterData;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            if (LocalBattleManager.instance != null)
                LocalBattleManager.instance.OnRegisterObject(this);
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
            if (m_StunTimer <= 0)
                return;
            m_StunTimer -= Time.deltaTime;
            if (m_StunTimer <= 0 && m_Physics != null)
                m_Physics.MoveSpeed.v = m_MoveSpeedBase;
        }
        // 넉백은 물리의 Move 통로로 밀어야 접지 감속 로직에 지워지지 않는다 — 경직 동안 이동속도를 넉백 속도로 바꿔 민다
        protected virtual void FixedUpdate()
        {
            if (m_StunTimer <= 0 || m_Physics == null)
                return;
            m_Physics.MoveSpeed.v = m_KnockSpeed;
            m_Physics.Move(m_KnockDir, true);
        }
        /// <summary>스폰 직후 호출된다. 파생이 등장 연출·입력 초기화를 얹는다</summary>
        protected virtual void OnSpawned()
        {
        }
        /// <summary>피해가 반영된 뒤(사망 제외) 호출된다. 기본은 Hit 애니메이션 재생(플레이어만)</summary>
        protected virtual void OnHit(SHit _hit)
        {
            if (m_Kind == EUnitKind.Player && m_Anim != null)
                m_Anim.Play(BattleConst.AnimHit, false);
        }
        /// <summary>HP 가 0 이 됐을 때 호출된다. 기본은 Die 애니메이션·FSM Die 전환·매니저 통지</summary>
        protected virtual void OnDie()
        {
            if (m_Anim != null)
                m_Anim.Play(BattleConst.AnimDie, false);
            if (m_Fsm != null && m_Fsm.GetState(BattleConst.StateDie) != null)
                m_Fsm.Set(BattleConst.StateDie);
            LocalBattleManager.instance.OnUnitDied(this);
        }
        #endregion
        #region Local Function
        /// <summary>종류에 맞는 테이블 행을 캐시하고 기본 HP·이동속도를 반환한다. 행이 없으면 예외</summary>
        private (int hp, float moveSpeed) LoadBase()
        {
            var table = TableManager.instance;
            switch (m_Kind)
            {
                case EUnitKind.Player:
                    if (!table.Character.Data.TryGetValue(m_Id, out var character))
                        throw new ArgumentException($"{name} : Character 테이블에 없는 ID {m_Id}");
                    m_CharacterData = character;
                    return (character.Hp, character.MoveSpeed);
                case EUnitKind.Enemy:
                    if (!table.Enemy.Data.TryGetValue(m_Id, out var enemy))
                        throw new ArgumentException($"{name} : Enemy 테이블에 없는 ID {m_Id}");
                    m_EnemyData = enemy;
                    return (enemy.Hp, enemy.MoveSpeed);
                default:
                    if (!table.Boss.Data.TryGetValue(m_Id, out var boss))
                        throw new ArgumentException($"{name} : Boss 테이블에 없는 ID {m_Id}");
                    m_BossData = boss;
                    return (boss.Hp, boss.MoveSpeed);
            }
        }
        #endregion
        #region Function
        /// <summary>_pos 에 배치하고 방 성장 배율 _hpScale·_atkScale 로 스탯을 초기화한다 (HP 가득, 경직·Enrage 해제)</summary>
        public void Spawn(Vector2 _pos, float _hpScale, float _atkScale)
        {
            transform.position = _pos;
            m_AttackScale = _atkScale;
            m_IsEnraged = false;
            m_StunTimer = 0;
            var (hp, moveSpeed) = LoadBase();
            m_MaxHp.Set(Mathf.RoundToInt(hp * _hpScale), false, false);
            m_IsDead.Set(false, false, false);
            m_Hp.Set(m_MaxHp.v, true, false);
            SetMoveSpeed(moveSpeed);
            if (m_Physics != null)
                m_Physics.SetVelocity(Vector2.zero);
            if (m_Fsm != null)
                m_Fsm.Set(m_Kind == EUnitKind.Boss ? BattleConst.StateIdle : BattleConst.StateMove);
            OnSpawned();
        }
        /// <summary>_hit 를 적용해 피해·넉백(보스 면역, Enemy 는 KnockbackRate 배)·경직을 주고 HP 0 이면 사망 처리한다. 이미 죽었으면 false</summary>
        public bool TakeHit(SHit _hit)
        {
            if (m_IsDead.v)
                return false;
            m_Hp.v = Mathf.Max(0, m_Hp.v - _hit.Damage);
            if (m_Kind != EUnitKind.Boss && m_Physics != null && 0 < _hit.KnockbackTime && 0 < _hit.KnockbackDist)
            {
                float rate = EnemyData != null ? EnemyData.KnockbackRate : 1f;
                m_KnockSpeed = _hit.KnockbackDist * rate / _hit.KnockbackTime;
                m_KnockDir = _hit.Direction;
                m_StunTimer = _hit.KnockbackTime;
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
        /// <summary>_amount 만큼 회복한다 (MaxHp 상한, 사망 중이면 무시)</summary>
        public void Heal(int _amount)
        {
            if (m_IsDead.v)
                return;
            m_Hp.v = Mathf.Min(m_MaxHp.v, m_Hp.v + _amount);
        }
        /// <summary>최대 HP 를 _maxHp 로 바꾼다. _isHealDiff 면 늘어난 만큼 즉시 회복한다</summary>
        public void SetMaxHp(int _maxHp, bool _isHealDiff)
        {
            int diff = _maxHp - m_MaxHp.v;
            m_MaxHp.v = _maxHp;
            if (_isHealDiff && 0 < diff)
                Heal(diff);
            else
                m_Hp.v = Mathf.Min(m_Hp.v, m_MaxHp.v);
        }
        /// <summary>기본 이동속도를 _speed 로 바꾼다 (경직이 풀릴 때도 이 값으로 돌아온다)</summary>
        public void SetMoveSpeed(float _speed)
        {
            m_MoveSpeedBase = _speed;
            if (m_Physics != null && m_StunTimer <= 0)
                m_Physics.MoveSpeed.v = _speed;
        }
        /// <summary>바라보는 방향을 _facing(+1 우·-1 좌)으로 바꾸고 스프라이트를 반전한다</summary>
        public void SetFacing(int _facing)
        {
            if (_facing == 0)
                return;
            m_Facing = _facing < 0 ? -1 : 1;
            if (m_Anim != null)
                m_Anim.SetFlip(Facing < 0);
        }
        /// <summary>Enrage 로 전환한다 (보스, 되돌리지 않는다)</summary>
        public void SetEnraged()
        {
            m_IsEnraged = true;
        }
        /// <summary>테이블 공격력 _base 에 성장 배율을 곱해 반올림한 값을 반환한다</summary>
        public int ScaleAttack(int _base)
        {
            return Mathf.RoundToInt(_base * m_AttackScale);
        }
        #endregion
    }
}
