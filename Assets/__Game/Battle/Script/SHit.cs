using UnityEngine;

namespace Game
{
    /// <summary>한 번의 피격 정보 — 공격자·피해·넉백·마무리 여부·방향·타격 위치</summary>
    public struct SHit
    {
        #region Value
        public Object_UnitBase Attacker;
        public int Damage;
        public float KnockbackDist;
        public float KnockbackTime;
        public bool IsFinish;
        public int Direction;
        public Vector2 Point;
        #endregion

        #region Event
        public SHit(Object_UnitBase _attacker, int _damage, float _knockbackDist, float _knockbackTime, bool _isFinish, int _direction, Vector2 _point)
        {
            Attacker = _attacker;
            Damage = _damage;
            KnockbackDist = _knockbackDist;
            KnockbackTime = _knockbackTime;
            IsFinish = _isFinish;
            Direction = _direction;
            Point = _point;
        }
        #endregion
    }
}
