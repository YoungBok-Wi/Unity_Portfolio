using Library;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>투사체 공통 오브젝트 — 소유 유닛에 맞는 스프라이트로 직선 비행하며 상대 진영에 명중·관통하고 다 쓰면 풀로 돌아간다</summary>
    public class Object_Projectile : ProjectileBase
    {
        #region Inspector
        [SerializeField, Tooltip("투사체 렌더러 (반전 루트)")] private SpriteRenderer m_Renderer;
        [SerializeField, Tooltip("Gun 탄 스프라이트")] private Sprite m_GunSprite;
        [SerializeField, Tooltip("Banana 껍질 스프라이트")] private Sprite m_BananaSprite;
        [SerializeField, Tooltip("Pineapple 가시 스프라이트")] private Sprite m_PineappleSprite;
        [SerializeField, Tooltip("명중 판정 반지름 (u)")] private float m_Radius = 0.25f;
        #endregion
        #region Value
        private SProjectile m_Data;
        private bool m_IsFlying;
        private float m_Traveled;
        private int m_HitCount;
        private readonly List<Object_UnitBase> m_HitList = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        private void Update()
        {
            if (!m_IsFlying)
                return;
            var step = m_Data.Velocity * Time.deltaTime;
            transform.position += (Vector3)step;
            m_Traveled += step.magnitude;
            var battle = LocalBattleManager.instance;
            if (m_Data.MaxDistance <= m_Traveled || battle == null)
            {
                Finish();
                return;
            }
            foreach (var col in Physics2D.OverlapCircleAll(transform.position, m_Radius))
            {
                var unit = col.GetComponentInParent<Object_UnitBase>();
                if (unit == null || unit == m_Data.Owner || m_HitList.Contains(unit))
                    continue;
                m_HitList.Add(unit);
                int dir = m_Data.Velocity.x < 0 ? -1 : 1;
                if (!battle.Hit(new SHit(m_Data.Owner, m_Data.Damage, m_Data.KnockbackDist, m_Data.KnockbackTime, false, dir, unit.HitPoint), unit))
                    continue;
                m_HitCount += 1;
                if (m_Data.Pierce < m_HitCount)
                {
                    Finish();
                    return;
                }
            }
        }
        #endregion
        #region Local Function
        /// <summary>소유 유닛 ID _ownerId 에 맞는 스프라이트를 반환한다. 대응이 없으면 예외</summary>
        private Sprite PickSprite(string _ownerId)
        {
            switch (_ownerId)
            {
                case "Gun": return m_GunSprite;
                case "Banana": return m_BananaSprite;
                case "Pineapple": return m_PineappleSprite;
                default: throw new System.ArgumentException($"투사체 스프라이트가 없는 소유 유닛 : {_ownerId}", nameof(_ownerId));
            }
        }
        /// <summary>비행을 끝내고 풀로 돌아간다 (매니저가 없으면 비활성만)</summary>
        private void Finish()
        {
            m_IsFlying = false;
            if (LocalBattleManager.instance != null)
                LocalBattleManager.instance.ReturnProjectile(gameObject);
            else
                gameObject.SetActive(false);
        }
        #endregion
        #region Function
        /// <summary>_data 로 비행을 시작한다 — 소유자별 스프라이트·진행 방향 반전·명중 기록 초기화</summary>
        public override void Launch(SProjectile _data)
        {
            m_Data = _data;
            m_Traveled = 0;
            m_HitCount = 0;
            m_HitList.Clear();
            m_Renderer.sprite = PickSprite(_data.Owner.Id);
            var scale = m_Renderer.transform.localScale;
            scale.x = _data.Velocity.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            m_Renderer.transform.localScale = scale;
            m_IsFlying = true;
        }
        #endregion
    }
}
