using UnityEngine;

namespace Game
{
    /// <summary>공격 단계와 피해·넉백·타격 위치를 전달한다.</summary>
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
        public int Step;
        #endregion

        #region Event
        /// <summary>공격자와 피해 정보를 구성하며 `_step`은 단계별 연출에 사용한다.</summary>
        public SHit(Object_UnitBase _attacker, int _damage, float _knockbackDist, float _knockbackTime, bool _isFinish, int _direction, Vector2 _point, int _step = 1)
        {
            Attacker = _attacker;
            Damage = _damage;
            KnockbackDist = _knockbackDist;
            KnockbackTime = _knockbackTime;
            IsFinish = _isFinish;
            Direction = _direction;
            Point = _point;
            Step = Mathf.Max(1, _step);
        }
        #endregion
    }
}
