using UnityEngine;

namespace Library
{
    /// <summary>캐릭터 이동·점프·체공 물리의 공통 베이스. 비행 상태 enum과 Init 진입점을 제공한다</summary>
    public abstract class CharacterPhysicsBase : MonoBehaviour
    {
        #region Type
        public enum EFlyState
        {
            None,
            Float,
            Jump,
            Fly
        }
        #endregion

        #region Event
        public virtual void Init()
        {
        }
        #endregion
    }
}
