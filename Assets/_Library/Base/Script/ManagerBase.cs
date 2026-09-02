using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>모든 매니저의 베이스. 초기화 라이프사이클과 관리 ValueBase 수집을 맡는다</summary>
    public abstract class ManagerBase : MonoBehaviour, IManageValue
    {
        #region Property
        /// <summary>Init 이 끝났는지 여부. 다른 매니저가 RequireInit 으로 선행을 확인할 때 쓴다</summary>
        public bool IsInited { get; private set; } = false;
        /// <summary>InitGame 이 끝났는지 여부</summary>
        public bool IsGameInited { get; private set; } = false;
        #endregion
        #region Value
        protected List<ValueBase> m_ManageValue = new();
        #endregion

        #region Event
        /// <summary>instance 를 세운다. 가장 먼저 호출되며 다른 매니저를 참조해선 안 된다</summary>
        public virtual void InitSingleton()
        {
        }
        /// <summary>지금 Init 을 해도 되는지 반환한다. 선행 매니저가 있으면 override 해서 그쪽의 IsInited 를 확인한다</summary>
        public virtual bool RequireInit()
        {
            return true;
        }
        /// <summary>매니저를 가동한다. 파생은 마지막에 base 를 호출해야 IsInited 가 선다</summary>
        public virtual void Init()
        {
            IsInited = true;
        }
        /// <summary>최초 1회만 호출된다. 구독 등록·구조 생성처럼 되풀이하면 안 되는 작업을 여기서 한다</summary>
        public virtual void InitUIOnce()
        {
        }
        /// <summary>초기화가 끝날 때마다 호출된다. 표시 갱신처럼 여러 번 해도 되는 작업을 여기서 한다</summary>
        public virtual void InitUI()
        {
        }
        /// <summary>지금 InitGame 을 해도 되는지 반환한다. 선행 매니저가 있으면 override 해서 확인한다</summary>
        public virtual bool RequireInitGame()
        {
            return true;
        }
        /// <summary>인게임 시스템을 가동한다. 파생은 마지막에 base 를 호출해야 IsGameInited 가 선다</summary>
        public virtual void InitGame()
        {
            IsGameInited = true;
        }

        /// <summary>시스템이 내려갈 때 호출된다. 구독 해제 등 뒷정리를 여기서 한다</summary>
        public virtual void OnShutdown()
        {
        }
        #endregion
        #region Manual Function
        void IManageValue.ManageValue(ValueBase _value)
        {
            m_ManageValue.Add(_value);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        /// <summary>이 매니저가 제공하는 치트를 _report 에 나열한다. 기본은 없으며, 테스트에 필요한 치트를 그 맥락의 매니저에 붙인다</summary>
        public virtual void MCPCheats(MCPReport _report)
        {
        }
        /// <summary>_cheatId 치트를 실행하고 결과 JSON 을 반환한다. MCPCheats 에 나열한 치트를 매니저별로 override 해서 처리한다</summary>
        public virtual string MCPCheatApply(string _cheatId)
        {
            return $"{{\"error\":\"지원하지 않는 치트: {MCPReport.Escape(_cheatId)}\"}}";
        }
#endif
        #endregion
    }
}