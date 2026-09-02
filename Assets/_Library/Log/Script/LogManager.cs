using Sirenix.OdinInspector;
using UnityEngine;

namespace Library
{
    /// <summary>로그 관리 매니저, Debug.Log 래퍼</summary>
    public class LogManager : GlobalManagerBase
    {
        public static LogManager instance { get; private set; }
        #region Inspector
        [SerializeField, TabGroup("LogManager", "설정")] private GameObject m_DebugConsole;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void InitFirst()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            m_DebugConsole.SetActive(true);
#endif
            base.InitFirst();
        }
        #endregion
        #region Function
        /// <summary>"[_system] _msg" 형식으로 로그를 남긴다. 에디터·개발 빌드에서만 출력된다</summary>
        public void Log(string _system, string _msg)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[{_system}] {_msg}");
#endif
        }
        /// <summary>"[_system] _msg" 형식으로 에러를 남긴다. 에디터·개발 빌드에서만 출력된다</summary>
        public void Error(string _system, string _msg)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError($"[{_system}] {_msg}");
#endif
        }
        #endregion
    }
}