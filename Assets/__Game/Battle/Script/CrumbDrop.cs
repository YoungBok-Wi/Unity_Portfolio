using UnityEngine;

namespace Game
{
    /// <summary>처치 시 떨어지는 Crumb 낙하물 — 위로 튀어 바닥에 떨어진 뒤 가까운 플레이어에게 흡인되어 수거된다 (수거·잔여 적립은 LocalBattleManager 가 한다)</summary>
    public class CrumbDrop : MonoBehaviour
    {
        #region Property
        /// <summary>수거 시 적립할 Crumb 양</summary>
        public int Value { get; private set; }
        #endregion
        #region Value
        private Vector2 m_Velocity;
        private float m_FloorY;
        private bool m_IsLanded;
        #endregion

        #region Event
        private void Update()
        {
            var battle = LocalBattleManager.instance;
            if (battle == null)
                return;
            var pos = (Vector2)transform.position;
            var player = battle.Player;
            bool hasPlayer = player != null && !player.IsDead.v;
            float dist = hasPlayer ? Vector2.Distance(player.HitPoint, pos) : float.MaxValue;
            if (hasPlayer && dist <= BattleConst.CrumbCollectDistance)
            {
                battle.CollectCrumb(this);
                return;
            }
            if (hasPlayer && dist <= BattleConst.CrumbMagnetDistance)
                pos = Vector2.MoveTowards(pos, player.HitPoint, BattleConst.CrumbMagnetSpeed * Time.deltaTime);
            else if (!m_IsLanded)
            {
                m_Velocity += Vector2.down * (BattleConst.CrumbGravity * Time.deltaTime);
                pos += m_Velocity * Time.deltaTime;
                if (pos.y <= m_FloorY)
                {
                    pos.y = m_FloorY;
                    m_IsLanded = true;
                }
            }
            transform.position = pos;
        }
        #endregion
        #region Function
        /// <summary>_pos 에서 _velocity 로 튀어 _floorY 에 떨어지도록 시작하고 수거 값을 _value 로 둔다</summary>
        public void Launch(Vector2 _pos, Vector2 _velocity, float _floorY, int _value)
        {
            transform.position = _pos;
            m_Velocity = _velocity;
            m_FloorY = _floorY;
            m_IsLanded = false;
            Value = _value;
        }
        #endregion
    }
}
