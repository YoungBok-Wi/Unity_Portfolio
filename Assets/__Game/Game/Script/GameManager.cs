using Library;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>BGM 재생과 로비 기본 곡을 관리한다.</summary>
    public class GameManager : GlobalManagerBase
    {
        public static GameManager instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("로비 씬 기본 BGM")] private AudioClip m_LobbyBgm;
        #endregion
        #region Property
        /// <summary>현재 BGM 재생 피치를 반환한다.</summary>
        public float BgmPitch => m_BgmSource != null ? m_BgmSource.pitch : 1f;
        #endregion
        #region Value
        private AudioSource m_BgmSource;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override bool RequireInit()
        {
            return InitUtil.IsInit(new ManagerBase[] { SoundManager.instance, SceneChangeManager.instance });
        }
        public override void Init()
        {
            SoundManager.instance.BGMVolume.AddChanged(this, OnBgmVolumeChanged);
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayLobbyBgmIfNeeded(SceneManager.GetActiveScene());
            base.Init();
        }
        public override void OnShutdown()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (SoundManager.instance != null)
                SoundManager.instance.BGMVolume.RemoveChanged(this, OnBgmVolumeChanged);
            base.OnShutdown();
        }
        private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
        {
            PlayLobbyBgmIfNeeded(_scene);
        }
        private void PlayLobbyBgmIfNeeded(Scene _scene)
        {
            if (SceneChangeManager.instance != null && _scene.name == SceneChangeManager.instance.LobbySceneID)
                PlayBGM(m_LobbyBgm);
        }
        private void OnBgmVolumeChanged(ValueBase _)
        {
            if (m_BgmSource != null)
                m_BgmSource.volume = SoundManager.instance.BGMVolume.v;
        }
        #endregion
        #region Function
        /// <summary>_clip을 _pitch로 반복 재생하고 null이면 BGM을 정지한다.</summary>
        public void PlayBGM(AudioClip _clip, float _pitch = 1f)
        {
            if (m_BgmSource == null)
            {
                m_BgmSource = gameObject.AddComponent<AudioSource>();
                m_BgmSource.playOnAwake = false;
                m_BgmSource.loop = true;
            }
            if (_clip == null)
            {
                m_BgmSource.Stop();
                m_BgmSource.clip = null;
                return;
            }
            m_BgmSource.pitch = _pitch;
            if (m_BgmSource.clip == _clip && m_BgmSource.isPlaying)
                return;
            m_BgmSource.clip = _clip;
            m_BgmSource.volume = SoundManager.instance.BGMVolume.v;
            m_BgmSource.Play();
        }
        #endregion
    }
}
