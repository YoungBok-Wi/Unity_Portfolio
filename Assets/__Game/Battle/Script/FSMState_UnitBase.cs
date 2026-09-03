using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>유닛 FSM 상태 공용 베이스 — 소유 유닛·플레이어 거리·이동·애니메이션 헬퍼</summary>
    public abstract class FSMState_UnitBase : FSMState
    {
        #region Property
        /// <summary>이 상태를 가진 유닛</summary>
        protected Object_UnitBase Unit { get; private set; }
        /// <summary>현재 플레이어 유닛. 없으면 null</summary>
        protected Object_UnitBase Player => LocalBattleManager.instance.Player;
        /// <summary>플레이어가 살아 있는지</summary>
        protected bool HasPlayer => Player != null && !Player.IsDead.v;
        /// <summary>플레이어와의 X 거리. 플레이어가 없으면 최대값</summary>
        protected float DistX => HasPlayer ? Mathf.Abs(Player.transform.position.x - Unit.transform.position.x) : float.MaxValue;
        /// <summary>플레이어 쪽 방향 (+1 우·-1 좌). 플레이어가 없으면 현재 방향</summary>
        protected int DirToPlayer => HasPlayer ? (Player.transform.position.x < Unit.transform.position.x ? -1 : 1) : Unit.Facing;
        #endregion

        #region Event
        protected override void OnInit()
        {
            Unit = GetComponentInParent<Object_UnitBase>();
            if (Unit == null)
                throw new InvalidOperationException($"{name} : 상위에 Object_UnitBase 이 없다");
        }
        #endregion
        #region Function
        /// <summary>유닛이 죽었으면 Die 상태를, 아니면 null 을 반환한다</summary>
        protected FSMState CheckDie()
        {
            return Unit.IsDead.v ? Parent.GetState(BattleConst.StateDie) : null;
        }
        /// <summary>_dir 방향으로 이동 요청하고 그쪽을 본다 (물리가 없으면 방향만)</summary>
        protected void Move(int _dir)
        {
            Unit.SetFacing(_dir);
            if (Unit.Physics != null)
                Unit.Physics.Move(_dir);
        }
        /// <summary>플레이어 쪽을 본다</summary>
        protected void FacePlayer()
        {
            Unit.SetFacing(DirToPlayer);
        }
        /// <summary>_action 애니메이션을 재생한다 (애니메이터가 없으면 생략)</summary>
        protected void PlayAnim(string _action, bool _loop)
        {
            if (Unit.Anim != null)
                Unit.Anim.Play(_action, _loop);
        }
        /// <summary>_action 애니메이션 길이(초)를 반환한다. 애니메이터가 없으면 _fallback</summary>
        protected float AnimLength(string _action, float _fallback)
        {
            return Unit.Anim != null ? Unit.Anim.GetLength(_action) : _fallback;
        }
        /// <summary>_center·_size 사각 범위로 근접 판정을 낸다 (플레이어 피격 넉백은 매니저 Hit 이 공통값으로 준다)</summary>
        protected int HitBox(Vector2 _center, Vector2 _size, int _damage, int _maxHits)
        {
            return LocalBattleManager.instance.HitBox(Unit, _center, _size, _damage, _maxHits, 0, 0, false);
        }
        #endregion
    }
}
