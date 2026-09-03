using UnityEngine;

namespace Game
{
    /// <summary>투사체 발사 정보 — 소유 유닛·시작점·속도·피해·관통·최대 비행 거리·넉백</summary>
    public struct SProjectile
    {
        #region Value
        public Object_Unit Owner;
        public Vector2 Origin;
        public Vector2 Velocity;
        public int Damage;
        public int Pierce;
        public float MaxDistance;
        public float KnockbackDist;
        public float KnockbackTime;
        #endregion

        #region Event
        public SProjectile(Object_Unit _owner, Vector2 _origin, Vector2 _velocity, int _damage, int _pierce, float _maxDistance, float _knockbackDist, float _knockbackTime)
        {
            Owner = _owner;
            Origin = _origin;
            Velocity = _velocity;
            Damage = _damage;
            Pierce = _pierce;
            MaxDistance = _maxDistance;
            KnockbackDist = _knockbackDist;
            KnockbackTime = _knockbackTime;
        }
        #endregion
    }
}
