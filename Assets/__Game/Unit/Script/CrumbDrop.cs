using UnityEngine;

namespace Game
{
    /// <summary>처치 보상을 플레이어에게 흡인해 수거하는 낙하물.</summary>
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
            var game = LocalGameManager.instance;
            if (game == null)
                return;
            var pos = (Vector2)transform.position;
            var player = game.Player;
            bool hasPlayer = player != null && !player.IsDead.v;
            float dist = hasPlayer ? Vector2.Distance(player.HitPoint, pos) : float.MaxValue;
            if (hasPlayer && dist <= GameConst.CrumbCollectDistance)
            {
                game.CollectCrumb(this);
                return;
            }
            if (hasPlayer && dist <= GameConst.CrumbMagnetDistance)
                pos = Vector2.MoveTowards(pos, player.HitPoint, GameConst.CrumbMagnetSpeed * Time.deltaTime);
            else if (!m_IsLanded)
            {
                m_Velocity += Vector2.down * (GameConst.CrumbGravity * Time.deltaTime);
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
