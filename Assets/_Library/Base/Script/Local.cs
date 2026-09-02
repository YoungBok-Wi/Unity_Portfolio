using System;
using UnityEngine;

namespace Library
{
    /// <summary>씬 로컬 매니저 생명주기 관리 및 Global 연동 초기화 시스템. 오브젝트 자가 등록 전에 로컬 매니저 instance가 준비되도록 일반 오브젝트보다 먼저 실행한다</summary>
    [DefaultExecutionOrder(-100)]
    public class Local : Root
    {
        public static Local instance { get; private set; }

        #region Property
        /// <summary>Init 이 끝났는지 여부</summary>
        public bool IsInited { get; private set; }

        /// <summary>InitGame 이 끝났는지 여부. 백엔드 초기화 전에는 false 로 남는다</summary>
        public bool IsGameInited { get; private set; }
        #endregion
        #region Value
        private LocalManagerBase[] m_Managers;

        /// <summary>소속 매니저 없이 씬에 배치된 오브젝트들. 매니저 초기화가 끝난 뒤 같은 단계를 이어 받는다</summary>
        private readonly System.Collections.Generic.List<ObjectBase> m_Objects = new();
        #endregion

        #region Event
        private void Awake()
        {
            instance = this;
            m_Managers = GetComponentsInChildren<LocalManagerBase>();
            InitSingletons("Game", m_Managers);
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 다음 씬의 Local 이 서기 전에 파괴된 인스턴스를 잡는 것을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        // 씬 진입의 실제 순서를 잡는 곳이다. Global 이 아직 안 떴으면 여기서 함께 세운다 — 씬을 직접 실행해도 흐름이 성립해야 한다.
        // 백엔드가 필요한 빌드에서는 InitGame 을 미루고, 로그인이 끝난 뒤 Global.InitBackend 가 이어서 부른다
        private void Start()
        {
            if (ShutdownManager.instance && ShutdownManager.instance.IsShutdown)
            {
                Shutdown();
                return;
            }

            try
            {
                Global.instance.InitValue();

                if (!Global.instance.IsInited)
                    Global.instance.Init();
                Init();

                Global.instance.InitUIOnce();
                InitUIOnce();

                Global.instance.InitUI();
                InitUI();

                Global.instance.InitBackendDummy();

                if (Global.instance.IsBackendInited)
                {
                    Global.instance.InitGame();
                    InitGame();

                    Global.instance.InitUI();
                    InitUI();
                }
            }
            catch (Exception e)
            {
                ShutdownManager.instance.Shutdown("Local", e);
            }
        }

        /// <summary>_object 가 소속 매니저 없이 자가 등록할 때 호출된다. 늦게 등록돼도 이미 지난 초기화 단계를 그 자리에서 따라잡는다</summary>
        public void OnRegisterObject(ObjectBase _object)
        {
            if (_object == null || _object.IsRegistered) return;
            m_Objects.Add(_object);
            _object.IsRegistered = true;
            if (IsInited)
            {
                _object.Init();
                _object.InitUIOnce();
                _object.InitUI();
            }
            if (IsGameInited)
            {
                _object.InitGame();
                _object.InitUI();
            }
        }

        /// <summary>로컬 매니저들을 가동한다</summary>
        private void Init()
        {
            InitManagers("Game", m_Managers);
            foreach (var v in m_Objects)
                v.Init();
            IsInited = true;
        }

        /// <summary>구독 등록·구조 생성 등 1회만 할 UI 초기화를 돌린다</summary>
        public void InitUIOnce()
        {
            InitManagersBase("Local", "InitUIOnce", m_Managers, (v) => true, (v) => v.InitUIOnce());
            foreach (var v in m_Objects)
                v.InitUIOnce();
        }

        /// <summary>UI 를 현재 값으로 다시 그린다 (기본·게임 초기화가 끝날 때마다 호출된다)</summary>
        public void InitUI()
        {
            InitManagersBase("Local", "InitUI", m_Managers, (v) => true, (v) => v.InitUI());
            foreach (var v in m_Objects)
                v.InitUI();
        }

        /// <summary>인게임 매니저들을 가동한다 (이후 IsGameInited 가 true)</summary>
        public void InitGame()
        {
            InitGameManagers("Game", m_Managers);
            foreach (var v in m_Objects)
                v.InitGame();
            IsGameInited = true;
        }
        #endregion
        #region Manual Function
        /// <summary>로컬 매니저를 모두 멈춘다</summary>
        public void Shutdown()
        {
            foreach (var v in GetComponentsInChildren<LocalManagerBase>())
            {
                v.OnShutdown();
                v.enabled = false;
            }
            foreach (var v in m_Objects)
                v.OnShutdown();
        }
        #endregion
    }
}