using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Library
{
    /// <summary>씬 전환을 관리하고 애니메이션과 연동하여 SceneManager.LoadScene을 래핑</summary>
    public class SceneChangeManager : GlobalManagerBase
    {
        public static SceneChangeManager instance { get; private set; }
        #region Inspector
        [SerializeField] private SceneChangeAni[] m_SceneChangeAni;
        [SerializeField] private string m_LobbySceneID = "Scene_Lobby";
        [SerializeField] private string m_GameSceneID = "GameScene";
        #endregion
        #region Property
        /// <summary>로비 씬 이름</summary>
        public string LobbySceneID => m_LobbySceneID;
        /// <summary>게임 씬 이름</summary>
        public string GameSceneID => m_GameSceneID;
        #endregion
        #region Value
        private Dictionary<string, SceneChangeAni> m_Ani = new();
        // 진행 중인 연출. null 이 아니면 전환 중이라 새 요청을 받지 않는다
        private SceneChangeAni m_CurAni;
        private string m_TargetSceneID;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void Init()
        {
            foreach(var v in m_SceneChangeAni)
            {
                if (v == null)
                    throw new InvalidOperationException($"{name}의 씬변경 애니메이션 배열에 빈 슬롯이 있다");
                m_Ani.Add(v.name, v);
                v.Init();
                // Overlay 로 강제한다 — 씬 전환 중에는 카메라가 사라져도 연출이 계속 보여야 한다
                foreach (var canvas in v.GetComponentsInChildren<Canvas>(true))
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            SceneManager.sceneLoaded += OnSceneLoaded;

            base.Init();
        }
        public override void OnShutdown()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            base.OnShutdown();
        }

        /// <summary>새 씬이 올라오면 마무리 연출을 재생한다</summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (m_CurAni != null)
                StartCoroutine(PlayEndAniAfterLoadSpike(m_CurAni));
        }
        // 동기 씬 로드는 로드에 걸린 시간이 로드 직후 프레임의 deltaTime 에 한꺼번에 실린다.
        // 그 프레임에 바로 종료 연출을 시작하면 deltaTime 급증으로 클립이 한 프레임에 끝까지 진행돼
        // 마지막 프레임 이벤트(PostEnd)가 즉시 불려 연출이 보이지 않고 사라진다.
        // 급증 프레임을 흘려보낸 뒤 종료 연출을 시작한다.
        private IEnumerator PlayEndAniAfterLoadSpike(SceneChangeAni _ani)
        {
            yield return null;
            yield return null;
            if (_ani != null)
                _ani.EndAni();
        }

        /// <summary>연출이 화면을 가린 시점에 실제 씬을 로드한다. 연출이 호출한다</summary>
        internal void OnChange()
        {
            SceneManager.LoadScene(m_TargetSceneID, LoadSceneMode.Single);
        }
        /// <summary>연출이 끝났음을 알린다. 이 시점부터 다음 전환을 받을 수 있다</summary>
        internal void OnEnd()
        {
            m_CurAni = null;
        }
        #endregion
        #region Function
        /// <summary>_aniName 연출과 함께 _nextScene 으로 전환한다. 이미 전환 중이면 무시하며, 빈 씬 이름이나 등록되지 않은 연출 이름은 예외</summary>
        public void SceneChange(string _nextScene, string _aniName = "Default")
        {
            if (string.IsNullOrEmpty(_nextScene))
                throw new ArgumentException("전환할 씬 이름이 비어 있다", nameof(_nextScene));
            if (!m_Ani.TryGetValue(_aniName, out var ani))
                throw new ArgumentException($"등록되지 않은 씬변경 애니메이션: {_aniName}", nameof(_aniName));

            if (m_CurAni)
                return;

            m_TargetSceneID = _nextScene;

            m_CurAni = ani;
            m_CurAni.StartAni();
        }
        #endregion
    }
}