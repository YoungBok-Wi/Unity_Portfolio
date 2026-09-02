using System;
using UnityEngine;

namespace Library
{
    /// <summary>전역 매니저 생명주기 관리 및 DontDestroyOnLoad 기반 초기화 시스템. 오브젝트 자가 등록 전에 매니저 instance가 준비되도록 가장 먼저 실행한다</summary>
    [DefaultExecutionOrder(-200)]
    public class Global : Root
    {
        public static Global instance { get; private set; }

        #region Property
        /// <summary>Init 이 끝났는지 여부</summary>
        public bool IsInited { get; private set; }

        /// <summary>백엔드 초기화가 끝났는지 여부. 오프라인 빌드에서도 더미 초기화 후 true 가 된다</summary>
        public bool IsBackendInited { get; private set; }

        /// <summary>셧다운됐는지 여부. true 면 다음 Global 이 이 인스턴스를 밀어내고 자리를 잡는다</summary>
        public bool IsShutdown { get; private set; }
        #endregion
        #region Value
        private GlobalManagerBase[] m_Managers;
        #endregion

        #region Event
        private void Awake()
        {
            if (instance)
            {
                if (instance.IsShutdown)
                    Destroy(instance.gameObject);
                else
                {
                    Destroy(gameObject);
                    return;
                }
            }
            instance = this;
            m_Managers = GetComponentsInChildren<GlobalManagerBase>();
            InitSingletons("Global", m_Managers);
            DontDestroyOnLoad(this);

            try
            {
                InitManagersBase("Global", "InitFirst", m_Managers, (v) => true, (v) => v.InitFirst());
            }
            catch (Exception e)
            {
                ShutdownManager.instance.Shutdown("Global", e);
            }
        }
        #endregion
        #region Manual Function
        /// <summary>매니저를 가동하기 전에 변수 초기값을 잡는다</summary>
        public void InitValue()
        {
            InitManagersBase("Global", "InitValue", m_Managers, (v) => true, (v) => v.InitValue());
        }

        /// <summary>전역 매니저들을 가동한다 (이후 IsInited 가 true)</summary>
        public void Init()
        {
            InitManagers("Global", m_Managers);
            IsInited = true;
        }

        /// <summary>구독 등록·구조 생성 등 1회만 할 UI 초기화를 돌린다</summary>
        public void InitUIOnce()
        {
            InitManagersBase("Global", "InitUIOnce", m_Managers, (v) => true, (v) => v.InitUIOnce());
        }

        /// <summary>UI 를 현재 값으로 다시 그린다 (기본·게임 초기화가 끝날 때마다 호출된다)</summary>
        public void InitUI()
        {
            InitManagersBase("Global", "InitUI", m_Managers, (v) => true, (v) => v.InitUI());
        }

        /// <summary>백엔드 없이 초기화를 마친 것으로 처리한다 (오프라인 빌드용)</summary>
        public void InitBackendDummy()
        {
            foreach(var v in m_Managers)
                v.InitBackendDummy();
            IsBackendInited = true;
        }

        /// <summary>인게임 매니저들을 가동한다</summary>
        public void InitGame()
        {
            InitGameManagers("Global", m_Managers);
        }

        /// <summary>전역 매니저를 모두 멈춘다. 오브젝트는 남으므로 다음 Global 이 이 인스턴스를 밀어내고 자리를 잡는다</summary>
        public void Shutdown()
        {
            IsShutdown = true;

            foreach (var v in GetComponentsInChildren<GlobalManagerBase>())
            {
                v.OnShutdown();
                v.enabled = false;
            }
        }
        #endregion
    }
}