using Library;
using System;

namespace Game
{
    /// <summary>플레이어 상태가 공유하는 소유자와 전환 헬퍼를 제공한다.</summary>
    public abstract class FSMState_PlayerBase : FSMState
    {
        #region Property
        /// <summary>상태를 소유한 플레이어를 반환한다.</summary>
        protected Object_PlayerBase Player { get; private set; }
        #endregion

        #region Event
        protected override void OnInit()
        {
            Player = GetComponentInParent<Object_PlayerBase>();
            if (Player == null)
                throw new InvalidOperationException($"{name} : 상위에 Object_PlayerBase가 없다");
        }
        #endregion
        #region Function
        /// <summary>_id에 해당하는 상태를 반환하고 없으면 예외를 던진다.</summary>
        protected FSMState State(string _id)
        {
            var state = Parent.GetState(_id);
            if (state == null)
                throw new InvalidOperationException($"{name} : {_id} 상태가 등록되지 않았다");
            return state;
        }
        /// <summary>플레이어가 사망했으면 Die 상태를, 아니면 null을 반환한다.</summary>
        protected FSMState CheckDead()
        {
            return Player.IsDead.v ? State(UnitConst.StateDie) : null;
        }
        #endregion
    }
}
