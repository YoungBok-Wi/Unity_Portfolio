using System;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Library
{
    /// <summary>치명적 오류 발생 시 UI를 표시하고 자동 재접속을 처리하는 매니저</summary>
    public class ShutdownManager : GlobalManagerBase
    {
        public static ShutdownManager instance { get; private set; }
        #region Inspector
        [SerializeField, TabGroup("ShutdownManager", "설정")] private ShutdownUI m_ShutdownUI;
        [SerializeField, TabGroup("ShutdownManager", "설정")] private string m_ReconnectScene = "Scene_Lobby";
        #endregion
        #region Property
        /// <summary>이미 중단됐는지 여부. true 면 이후 Shutdown 호출은 무시된다</summary>
        public bool IsShutdown { get; private set; } = false;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void InitFirst()
        {
            m_ShutdownUI.gameObject.SetActive(false);
            base.InitFirst();
        }

        /// <summary>오류 UI 를 잠시 보여 준 뒤 Global 을 버리고 재접속 씬으로 되돌린다</summary>
        private IEnumerator ShutdownCor()
        {
            // timeScale 이 0이라 Realtime 으로 기다려야 한다
            yield return new WaitForSecondsRealtime(5);

            Destroy(Global.instance.gameObject);
            SceneManager.LoadScene(m_ReconnectScene);
        }
        #endregion
        #region Function
        /// <summary>게임을 중단하고 오류 UI 를 띄운 뒤 재접속 씬으로 되돌린다. _system 은 중단을 일으킨 곳의 이름, _e·_debugMsg 는 로그에 남는다. 이미 중단됐으면 무시한다</summary>
        public void Shutdown(string _system, Exception _e = null, string _debugMsg = "Unknown Error")
        {
            if (IsShutdown)
                return;
            IsShutdown = true;

            m_ShutdownUI.gameObject.SetActive(true);
            m_ShutdownUI.SetText((LanguageManager.instance != null) ? LanguageManager.instance.Get("Text_Shutdown_Text") : "!!ERROR!!");

            Debug.LogError((_e != null) ? $"[{_system}] {_debugMsg}\n\n{_e.Message}\n\n{_e.StackTrace}" : $"[{_system}] {_debugMsg}");

            Stop();

            StartCoroutine(ShutdownCor());
        }
        /// <summary>시간·소리·매니저를 모두 멈춘다. UI 는 건드리지 않아 오류 화면은 계속 보인다</summary>
        public void Stop()
        {
            if (TimeManager.instance && TimeManager.instance.IsInited)
                TimeManager.instance.SetTimeScale(this, this, 0);

            // TODO : SoundManager 추가 후 옮기기
            AudioListener.pause = true;

            Global.instance?.Shutdown();
            Local.instance?.Shutdown();
        }
#endregion
    }
}