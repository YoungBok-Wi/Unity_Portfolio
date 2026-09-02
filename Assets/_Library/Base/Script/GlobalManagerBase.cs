using UnityEngine;

namespace Library
{
    /// <summary>씬을 넘어 살아남는 전역 매니저의 베이스. 씬별 매니저(LocalManagerBase)와 달리 씬 전환에 파괴되지 않는다</summary>
    public abstract class GlobalManagerBase : ManagerBase
    {
        #region Property
        /// <summary>InitFirst 가 끝났는지 여부</summary>
        public bool IsFirstInited { get; private set; } = false;

        /// <summary>백엔드 초기화가 끝났는지 여부. 다른 매니저가 RequireInitBackend 로 선행을 확인할 때 쓴다</summary>
        public bool IsBackendInited { get; private set; } = false;
        #endregion

        #region Event
        public virtual void InitFirst()
        {
            IsFirstInited = true;
        }

        public virtual bool RequireInitBackend()
        {
            return true;
        }

        public void InitBackendDummy()
        {
            IsBackendInited = true;
        }

        public virtual void InitValue()
        {
            foreach (var v in m_ManageValue)
                v.OnResetLocalChanged();
        }
        #endregion
    }
}