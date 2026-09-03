using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>씬 전투 매니저 — 유닛 등록·풀 스폰·근접 박스·투사체·히트 연출·근접 슬롯·능력 Factor 를 처리한다</summary>
    public class LocalBattleManager : LocalManagerBase
    {
        public static LocalBattleManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("스폰 유닛·투사체를 둘 루트 (없으면 매니저 자신)")] private Transform m_UnitRoot;
        [SerializeField, Tooltip("적 프리팹 목록 (Object_UnitBase.Id 로 매칭)")] private GameObject[] m_EnemyPrefabs;
        [SerializeField, Tooltip("보스 프리팹 목록 (Object_UnitBase.Id 로 매칭)")] private GameObject[] m_BossPrefabs;
        [SerializeField, Tooltip("투사체 프리팹 (IProjectile 구현)")] private GameObject m_ProjectilePrefab;
        [SerializeField, Tooltip("적 종류별 풀 크기")] private int m_EnemyPoolSize = 8;
        [SerializeField, Tooltip("투사체 풀 크기")] private int m_ProjectilePoolSize = 24;
        [SerializeField, Tooltip("히트스톱 시간 (초, 실시간)")] private float m_HitStopSec = 0.06f;
        [SerializeField, Tooltip("히트 이펙트 프리팹 (없으면 생략)")] private GameObject m_HitEffectPrefab;
        [SerializeField, Tooltip("히트 이펙트 수명 (초)")] private float m_HitEffectSec = 0.3f;
        [SerializeField, Tooltip("전조 표시 프리팹 (1u 스프라이트, 없으면 생략)")] private GameObject m_TelegraphPrefab;
        [SerializeField, Tooltip("타격음")] private AudioClip m_SfxHit;
        [SerializeField, Tooltip("처치음")] private AudioClip m_SfxDie;
        #endregion
        #region Property
        /// <summary>등록된 플레이어 유닛. 없으면 null</summary>
        public Object_UnitBase Player { get; private set; }
        /// <summary>스폰된 보스 유닛. 없으면 null</summary>
        public Object_UnitBase Boss { get; private set; }
        /// <summary>살아 있는 일반 적 수 (스폰·사망마다 갱신)</summary>
        public IReadOnlyIntValue AliveEnemyCount => m_AliveEnemyCount;
        /// <summary>플레이어 사망 통지</summary>
        public IReadOnlyBoolValue IsPlayerDead => m_IsPlayerDead;
        /// <summary>보스 사망 통지</summary>
        public IReadOnlyBoolValue IsBossDead => m_IsBossDead;
        /// <summary>공격력 가산 합 (배율 = 1 + Total)</summary>
        public IReadOnlyFloatFactor AttackFactor => m_AttackFactor;
        /// <summary>공격 주기 곱 배율</summary>
        public IReadOnlyFloatFactor AttackSpeedFactor => m_AttackSpeedFactor;
        /// <summary>이동속도 가산 합 (배율 = 1 + Total)</summary>
        public IReadOnlyFloatFactor MoveSpeedFactor => m_MoveSpeedFactor;
        /// <summary>능력 ID → 스택</summary>
        public IReadOnlyDictionary<string, int> AbilityStacks => m_AbilityStacks;
        /// <summary>일시정지 중인지 (timeScale 소유자는 이 매니저 하나다)</summary>
        public bool IsPaused => m_IsPaused;
        #endregion
        #region Value
        private IntValue m_AliveEnemyCount;
        private BoolValue m_IsPlayerDead;
        private BoolValue m_IsBossDead;
        private FloatFactor<string> m_AttackFactor;
        private FloatFactor<string> m_AttackSpeedFactor;
        private FloatFactor<string> m_MoveSpeedFactor;
        private readonly Dictionary<string, int> m_AbilityStacks = new();
        private readonly Dictionary<string, ObjectPool> m_UnitPools = new();
        private readonly Dictionary<GameObject, ObjectPool> m_PoolByObject = new();
        private readonly List<Object_UnitBase> m_Enemies = new();
        private readonly Dictionary<Object_UnitBase, int> m_MeleeSlots = new();
        private readonly List<Object_UnitBase> m_HitBuffer = new();
        private ObjectPool m_ProjectilePool;
        private bool m_IsHitStopping;
        private bool m_IsPaused;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
            Time.timeScale = 1;
        }
        public override void OnRegisterObject(ObjectBase _object)
        {
            base.OnRegisterObject(_object);
            if (_object is Object_UnitBase unit && unit.Kind == EUnitKind.Player)
                Player = unit;
        }
        public override void Init()
        {
            m_AliveEnemyCount = new IntValue(this, "AliveEnemyCount", 0);
            m_IsPlayerDead = new BoolValue(this, "IsPlayerDead", false);
            m_IsBossDead = new BoolValue(this, "IsBossDead", false);
            m_AttackFactor = new FloatFactor<string>(this, FloatFactor<string>.ETotalType.Add);
            m_AttackSpeedFactor = new FloatFactor<string>(this, FloatFactor<string>.ETotalType.Multifly);
            m_MoveSpeedFactor = new FloatFactor<string>(this, FloatFactor<string>.ETotalType.Add);
            base.Init();
        }
        public override void InitGame()
        {
            var root = m_UnitRoot != null ? m_UnitRoot : transform;
            foreach (var prefab in m_EnemyPrefabs)
                CreateUnitPool(prefab, m_EnemyPoolSize, root);
            foreach (var prefab in m_BossPrefabs)
                CreateUnitPool(prefab, 1, root);
            if (m_ProjectilePrefab != null)
                m_ProjectilePool = new ObjectPool(m_ProjectilePrefab, root, m_ProjectilePoolSize);
            base.InitGame();
        }
        #endregion
        #region Local Function
        /// <summary>_prefab 의 Object_UnitBase.Id 로 풀을 만든다. 컴포넌트가 없거나 ID 가 겹치면 예외</summary>
        private void CreateUnitPool(GameObject _prefab, int _size, Transform _root)
        {
            if (_prefab == null)
                throw new InvalidOperationException($"{name} : 유닛 프리팹 슬롯이 비어 있다");
            var unit = _prefab.GetComponent<Object_UnitBase>();
            if (unit == null)
                throw new InvalidOperationException($"{_prefab.name} 에 Object_UnitBase 이 없다");
            if (m_UnitPools.ContainsKey(unit.Id))
                throw new InvalidOperationException($"유닛 ID 가 겹친다 : {unit.Id}");
            m_UnitPools.Add(unit.Id, new ObjectPool(_prefab, _root, _size));
        }
        /// <summary>_point 에 히트 이펙트를 띄우고 _clip 을 재생한다 (프리팹·클립이 없으면 각각 생략)</summary>
        private void PlayHitEffect(Vector2 _point, AudioClip _clip)
        {
            if (m_HitEffectPrefab != null)
                Destroy(Instantiate(m_HitEffectPrefab, _point, Quaternion.identity), m_HitEffectSec);
            SoundManager.instance.PlaySE(_clip);
        }
        /// <summary>실시간 _sec 동안 timeScale 을 0 으로 멈춘다 — 복원값은 일시정지 여부로 정한다 (timeScale 소유자 단일화)</summary>
        private IEnumerator HitStopRoutine(float _sec)
        {
            m_IsHitStopping = true;
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(_sec);
            Time.timeScale = m_IsPaused ? 0 : 1;
            m_IsHitStopping = false;
        }
        /// <summary>_id 능력 스택을 Factor·플레이어 스탯에 반영한다</summary>
        private void ApplyAbility(string _id, AbilityTable _table, int _stack)
        {
            switch (_id)
            {
                case "Attack":
                    m_AttackFactor.Set(this, _id, _table.Value * _stack);
                    break;
                case "AttackSpeed":
                    m_AttackSpeedFactor.Set(this, _id, Mathf.Pow(1f - _table.Value, _stack));
                    break;
                case "MoveSpeed":
                    m_MoveSpeedFactor.Set(this, _id, _table.Value * _stack);
                    break;
                case "MaxHp":
                    if (Player != null)
                    {
                        Player.SetMaxHp(Player.MaxHp.v + (int)_table.Value, false);
                        Player.Heal((int)_table.ValueSub);
                    }
                    break;
                case "HealMacaron":
                    if (Player != null)
                        Player.Heal(Mathf.RoundToInt(Player.MaxHp.v * _table.Value));
                    break;
                case BattleConst.AbilityMultiHit:
                    break;
                default:
                    throw new ArgumentException($"처리가 정의되지 않은 능력 ID : {_id}", nameof(_id));
            }
        }
        /// <summary>히트스톱을 건다 (이미 정지·일시정지 중이면 무시)</summary>
        private void HitStop()
        {
            if (m_IsHitStopping || m_IsPaused)
                return;
            StartCoroutine(HitStopRoutine(m_HitStopSec));
        }
        /// <summary>MultiHit 스택에 따른 가산치를 반환한다 — _isMelee 면 명중 상한(Value), 아니면 관통 수(ValueSub)</summary>
        private int GetMultiHit(bool _isMelee)
        {
            if (!m_AbilityStacks.TryGetValue(BattleConst.AbilityMultiHit, out var stack))
                return 0;
            var table = TableManager.instance.Ability.Data[BattleConst.AbilityMultiHit];
            return Mathf.RoundToInt((_isMelee ? table.Value : table.ValueSub) * stack);
        }
        #endregion
        #region Function
        /// <summary>_id 유닛을 풀에서 꺼내 _pos 에 성장 배율 _hpScale·_atkScale 로 스폰하고 반환한다. 풀이 없거나 비면 예외</summary>
        public Object_UnitBase SpawnUnit(string _id, Vector2 _pos, float _hpScale, float _atkScale)
        {
            if (!m_UnitPools.TryGetValue(_id, out var pool))
                throw new ArgumentException($"풀에 등록되지 않은 유닛 ID : {_id}", nameof(_id));
            var go = pool.Get();
            if (go == null)
                throw new InvalidOperationException($"{_id} 풀이 비었다 (크기 {m_EnemyPoolSize})");
            var unit = go.GetComponent<Object_UnitBase>();
            m_PoolByObject.Set(go, pool);
            unit.Spawn(_pos, _hpScale, _atkScale);
            if (unit.Kind == EUnitKind.Boss)
            {
                Boss = unit;
                m_IsBossDead.Set(false, false, false);
            }
            else
            {
                m_Enemies.Add(unit);
                m_AliveEnemyCount.v = m_Enemies.Count;
            }
            return unit;
        }
        /// <summary>_wave 구성을 좌우 _left·_right 에서 번갈아 _spacing 간격으로 스폰하고 스폰 수를 반환한다</summary>
        public int SpawnWave(WaveTable _wave, float _hpScale, float _atkScale, Vector2 _left, Vector2 _right, float _spacing)
        {
            var slots = new (string id, int count)[] { (_wave.Enemy1Id, _wave.Enemy1Count), (_wave.Enemy2Id, _wave.Enemy2Count), (_wave.Enemy3Id, _wave.Enemy3Count) };
            int index = 0;
            foreach (var (id, count) in slots)
            {
                for (int i = 0; i < count; i++, index++)
                {
                    int side = index % 2;
                    float offset = index / 2 * _spacing;
                    var pos = side == 0 ? _left + Vector2.left * offset : _right + Vector2.right * offset;
                    SpawnUnit(id, pos, _hpScale, _atkScale);
                }
            }
            return index;
        }
        /// <summary>_unit 을 풀로 되돌린다 (살아 있으면 통지 없이 목록에서 뺀다). 풀 소속이 아니면 예외</summary>
        public void Despawn(Object_UnitBase _unit)
        {
            if (!m_PoolByObject.TryGetValue(_unit.gameObject, out var pool))
                throw new ArgumentException($"풀 소속이 아닌 유닛 : {_unit.name}", nameof(_unit));
            m_PoolByObject.Remove(_unit.gameObject);
            ReleaseMeleeSlot(_unit);
            if (m_Enemies.Remove(_unit))
                m_AliveEnemyCount.Set(m_Enemies.Count, false, false);
            if (Boss == _unit)
                Boss = null;
            pool.Return(_unit.gameObject);
        }
        /// <summary>_unit 이 죽었을 때 유닛이 호출한다 — 처치음·Crumb 적립·통지 갱신</summary>
        public void OnUnitDied(Object_UnitBase _unit)
        {
            if (_unit.Kind == EUnitKind.Player)
            {
                m_IsPlayerDead.v = true;
                return;
            }
            PlayHitEffect(_unit.HitPoint, m_SfxDie);
            // 단순화: 낙하·수거 연출 없이 처치 즉시 적립한다 — 수거 오브젝트가 생기면 여기서 낙하만 만들고 적립은 수거 쪽으로 옮긴다
            int drop = _unit.Kind == EUnitKind.Boss ? _unit.BossData.CrumbDrop : _unit.EnemyData.CrumbDrop;
            if (0 < drop)
                BattleManager.instance.AddCrumb(drop);
            ReleaseMeleeSlot(_unit);
            if (_unit.Kind == EUnitKind.Boss)
                m_IsBossDead.v = true;
            else
            {
                m_Enemies.Remove(_unit);
                m_AliveEnemyCount.v = m_Enemies.Count;
            }
        }
        /// <summary>_hit 를 _target 에 적용하고 연출을 재생한다. 같은 진영·사망·null 이면 false</summary>
        public bool Hit(SHit _hit, Object_UnitBase _target)
        {
            if (_target == null || _hit.Attacker == null || _target.Team == _hit.Attacker.Team)
                return false;
            if (!_target.TakeHit(_hit))
                return false;
            PlayHitEffect(_hit.Point, m_SfxHit);
            if (_hit.IsFinish || _target.Kind == EUnitKind.Boss)
                HitStop();
            return true;
        }
        /// <summary>_center·_size 사각 범위의 상대 진영 유닛을 가까운 순으로 최대 _maxHits(플레이어는 MultiHit 가산) 명중시키고 명중 수를 반환한다</summary>
        public int HitBox(Object_UnitBase _attacker, Vector2 _center, Vector2 _size, int _damage, int _maxHits, float _knockbackDist, float _knockbackTime, bool _isFinish)
        {
            if (_attacker.Kind == EUnitKind.Player)
                _maxHits += GetMultiHit(true);
            m_HitBuffer.Clear();
            foreach (var col in Physics2D.OverlapBoxAll(_center, _size, 0))
            {
                var unit = col.GetComponentInParent<Object_UnitBase>();
                if (unit == null || unit.IsDead.v || unit.Team == _attacker.Team || m_HitBuffer.Contains(unit))
                    continue;
                m_HitBuffer.Add(unit);
            }
            float ax = _attacker.transform.position.x;
            m_HitBuffer.Sort((a, b) => Mathf.Abs(a.transform.position.x - ax).CompareTo(Mathf.Abs(b.transform.position.x - ax)));
            int hits = 0;
            foreach (var unit in m_HitBuffer)
            {
                if (_maxHits <= hits)
                    break;
                int dir = unit.transform.position.x < ax ? -1 : 1;
                if (Hit(new SHit(_attacker, _damage, _knockbackDist, _knockbackTime, _isFinish, dir, unit.HitPoint), unit))
                    hits += 1;
            }
            return hits;
        }
        /// <summary>_data 로 투사체를 풀에서 꺼내 발사한다 (플레이어는 관통에 MultiHit 가산). 풀·구현이 없으면 예외</summary>
        public void Fire(SProjectile _data)
        {
            if (m_ProjectilePool == null)
                throw new InvalidOperationException($"{name} : 투사체 프리팹이 등록되지 않았다");
            var go = m_ProjectilePool.Get();
            if (go == null)
                throw new InvalidOperationException($"투사체 풀이 비었다 (크기 {m_ProjectilePoolSize})");
            var projectile = go.GetComponent<IProjectile>();
            if (projectile == null)
                throw new InvalidOperationException($"{go.name} 에 IProjectile 구현이 없다");
            if (_data.Owner.Kind == EUnitKind.Player)
                _data.Pierce += GetMultiHit(false);
            go.transform.position = _data.Origin;
            projectile.Launch(_data);
        }
        /// <summary>다 쓴 투사체 _object 를 풀에 되돌린다</summary>
        public void ReturnProjectile(GameObject _object)
        {
            m_ProjectilePool.Return(_object);
        }
        /// <summary>_unit 에 플레이어 _side(-1 좌·+1 우) 근접 슬롯을 준다. 이미 가졌거나 자리가 있으면 true</summary>
        public bool RequestMeleeSlot(Object_UnitBase _unit, int _side)
        {
            if (m_MeleeSlots.ContainsKey(_unit))
                return true;
            int count = 0;
            foreach (var side in m_MeleeSlots.Values)
                if (side == _side)
                    count += 1;
            if (TableManager.instance.Const.Battle_MeleeSlotPerSide <= count)
                return false;
            m_MeleeSlots.Add(_unit, _side);
            return true;
        }
        /// <summary>_unit 의 근접 슬롯을 반납한다 (없으면 무시)</summary>
        public void ReleaseMeleeSlot(Object_UnitBase _unit)
        {
            m_MeleeSlots.Remove(_unit);
        }
        /// <summary>_center·_size 범위에 전조 표시를 _sec 동안 띄운다 (프리팹이 없으면 생략)</summary>
        public void ShowTelegraph(Vector2 _center, Vector2 _size, float _sec)
        {
            if (m_TelegraphPrefab == null)
                return;
            var go = Instantiate(m_TelegraphPrefab, _center, Quaternion.identity);
            go.transform.localScale = new Vector3(_size.x, _size.y, 1);
            Destroy(go, _sec);
        }
        /// <summary>_id 능력을 한 스택 더 얻을 수 있는지 반환한다 (MaxStack 0 은 무제한)</summary>
        public bool CanAddAbility(string _id)
        {
            if (!TableManager.instance.Ability.Data.TryGetValue(_id, out var table))
                throw new ArgumentException($"Ability 테이블에 없는 ID : {_id}", nameof(_id));
            if (table.MaxStack <= 0)
                return true;
            return !m_AbilityStacks.TryGetValue(_id, out var stack) || stack < table.MaxStack;
        }
        /// <summary>_id 능력을 한 스택 얻어 적용하고 현재 스택을 반환한다 (Instant 는 스택 없이 즉시). 상한이면 예외</summary>
        public int AddAbility(string _id)
        {
            if (!CanAddAbility(_id))
                throw new InvalidOperationException($"{_id} 능력이 상한에 닿았다");
            var table = TableManager.instance.Ability.Data[_id];
            int stack = m_AbilityStacks.TryGetValue(_id, out var cur) ? cur : 0;
            if (table.StackMode != BattleConst.StackInstant)
            {
                stack += 1;
                m_AbilityStacks.Set(_id, stack);
            }
            ApplyAbility(_id, table, stack);
            return stack;
        }
        /// <summary>런 시작 시 유닛·투사체·능력·통지를 초기 상태로 되돌린다 (통지 없음)</summary>
        public void ResetRun()
        {
            ClearUnits();
            foreach (var id in new List<string>(m_AbilityStacks.Keys))
            {
                m_AttackFactor.Remove(id);
                m_AttackSpeedFactor.Remove(id);
                m_MoveSpeedFactor.Remove(id);
            }
            m_AbilityStacks.Clear();
            m_IsPlayerDead.Set(false, false, false);
            m_IsBossDead.Set(false, false, false);
        }
        /// <summary>스폰된 적·보스·투사체를 전부 풀로 되돌린다 (통지 없음)</summary>
        public void ClearUnits()
        {
            foreach (var unit in m_Enemies.ToArray())
                Despawn(unit);
            if (Boss != null)
                Despawn(Boss);
            m_ProjectilePool?.Clear();
            m_MeleeSlots.Clear();
            m_AliveEnemyCount.Set(0, false, false);
        }
        /// <summary>일시정지를 _isPaused 로 바꾸고 timeScale 을 반영한다 (히트스톱 중이면 히트스톱 종료 시 반영)</summary>
        public void SetPaused(bool _isPaused)
        {
            m_IsPaused = _isPaused;
            if (!m_IsHitStopping)
                Time.timeScale = _isPaused ? 0 : 1;
        }
        /// <summary>플레이어를 최대 HP 의 _ratio 만큼 회복한다 (플레이어가 없으면 무시)</summary>
        public void HealPlayer(float _ratio)
        {
            if (Player != null)
                Player.Heal(Mathf.RoundToInt(Player.MaxHp.v * _ratio));
        }
        /// <summary>플레이어 기본 공격력 _base 에 Attack 능력을 반영한 값을 반환한다</summary>
        public int GetPlayerDamage(int _base)
        {
            return Mathf.RoundToInt(_base * (1f + m_AttackFactor.Total));
        }
        /// <summary>플레이어 기본 공격 주기 _base 에 AttackSpeed 능력을 반영한 값을 반환한다</summary>
        public float GetPlayerAttackInterval(float _base)
        {
            return _base * m_AttackSpeedFactor.Total;
        }
        /// <summary>플레이어 기본 이동속도 _base 에 MoveSpeed 능력을 반영한 값을 반환한다</summary>
        public float GetPlayerMoveSpeed(float _base)
        {
            return _base * (1f + m_MoveSpeedFactor.Total);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            _report.AddNumber("playerHp", Player != null ? Player.Hp.v : 0);
            _report.AddNumber("playerMaxHp", Player != null ? Player.MaxHp.v : 0);
            _report.AddRaw("playerDead", m_IsPlayerDead.v ? "true" : "false");
            _report.AddNumber("aliveEnemy", m_AliveEnemyCount.v);
            _report.Add("boss", Boss != null ? Boss.Id : "");
            _report.AddNumber("bossHp", Boss != null ? Boss.Hp.v : 0);
            _report.AddRaw("bossDead", m_IsBossDead.v ? "true" : "false");
            foreach (var pair in m_AbilityStacks)
                _report.AddNumber($"ability_{pair.Key}", pair.Value);
        }
        public override void MCPCheats(MCPReport _report)
        {
            _report.Add("KillEnemies", "적 전멸");
            if (Boss != null)
                _report.Add("KillBoss", "보스 처치");
            _report.Add("HealPlayer", "플레이어 HP 가득");
            _report.Add("KillPlayer", "플레이어 사망");
            foreach (var id in TableManager.instance.Ability.ID)
                if (CanAddAbility(id))
                    _report.Add($"Ability_{id}", $"{id} 능력 1스택");
        }
        public override string MCPCheatApply(string _cheatId)
        {
            switch (_cheatId)
            {
                case "KillEnemies":
                    foreach (var unit in m_Enemies.ToArray())
                        unit.TakeHit(new SHit(Player, unit.Hp.v, 0, 0, false, 1, unit.HitPoint));
                    return "{\"success\":true}";
                case "KillBoss":
                    if (Boss != null)
                        Boss.TakeHit(new SHit(Player, Boss.Hp.v, 0, 0, false, 1, Boss.HitPoint));
                    return "{\"success\":true}";
                case "HealPlayer":
                    HealPlayer(1f);
                    return "{\"success\":true}";
                case "KillPlayer":
                    if (Player != null)
                        Player.TakeHit(new SHit(Boss ?? Player, Player.Hp.v, 0, 0, false, 1, Player.HitPoint));
                    return "{\"success\":true}";
            }
            if (_cheatId.StartsWith("Ability_"))
            {
                int stack = AddAbility(_cheatId.Substring("Ability_".Length));
                return $"{{\"success\":true,\"stack\":{stack}}}";
            }
            return base.MCPCheatApply(_cheatId);
        }
#endif
        #endregion
    }
}
