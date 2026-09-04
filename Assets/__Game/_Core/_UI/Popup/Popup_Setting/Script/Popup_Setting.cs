using Game;
using UnityEngine;

namespace Library
{
    /// <summary>설정 팝업. BGM·효과음 볼륨과 전체 화면 여부를 조정하고 적용·기본값 복원을 처리한다</summary>
    public class Popup_Setting : PopupBase
    {
        public static Popup_Setting instance { get; private set; }

        #region Inspector
        [SerializeField, Tooltip("제목·닫기를 가진 팝업 프레임")] private Control_GameFrame m_Frame;
        [SerializeField, Tooltip("BGM 볼륨 슬라이더")] private UIWrapper_Slider m_BgmSlider;
        [SerializeField, Tooltip("효과음 볼륨 슬라이더")] private UIWrapper_Slider m_SfxSlider;
        [SerializeField, Tooltip("전체 화면 토글")] private UIWrapper_Toggle m_FullscreenToggle;
        [SerializeField, Tooltip("적용 버튼")] private UIWrapper_Button m_ApplyButton;
        [SerializeField, Tooltip("기본값 복원 버튼")] private UIWrapper_Button m_DefaultButton;
        [SerializeField, Tooltip("BGM 항목 라벨")] private UIWrapper_Text m_BgmLabel;
        [SerializeField, Tooltip("효과음 항목 라벨")] private UIWrapper_Text m_SfxLabel;
        [SerializeField, Tooltip("전체 화면 항목 라벨")] private UIWrapper_Text m_FullscreenLabel;
        [SerializeField, Tooltip("적용 버튼 라벨")] private UIWrapper_Text m_ApplyLabel;
        [SerializeField, Tooltip("기본값 버튼 라벨")] private UIWrapper_Text m_DefaultLabel;
        #endregion
        #region Value
        // 기본값 복원이 되돌릴 볼륨. SoundManager 가 만드는 초기값과 같은 값이다
        private const float DefaultVolume = 1.0f;
        private const string BgmVolumeId = "BGMVolume";
        private const string SfxVolumeId = "SEVolume";
        private const string NotifyPopupId = "Popup_Notify";
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 이 팝업이 없는 씬에서 파괴된 인스턴스 접근을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            if (m_ApplyButton != null) m_ApplyButton.AddClickListener(OnClickApply);
            if (m_DefaultButton != null) m_DefaultButton.AddClickListener(OnClickDefault);
            if (m_Frame != null) m_Frame.AddCloseListener(_ => Close());
            base.Init();
        }
        public override void InitUIOnce()
        {
            BindVolume(m_BgmSlider, BgmVolumeId);
            BindVolume(m_SfxSlider, SfxVolumeId);
            if (m_FullscreenToggle != null)
                m_FullscreenToggle.AddValueChangedListener(OnFullscreenChanged);
            base.InitUIOnce();
        }
        public override void InitUI()
        {
            RefreshVolume(m_BgmSlider, BgmVolumeId);
            RefreshVolume(m_SfxSlider, SfxVolumeId);
            if (m_FullscreenToggle != null)
                m_FullscreenToggle.SetIsOn(Screen.fullScreen);
            base.InitUI();
        }
        public override void OnLanguageChanged()
        {
            base.OnLanguageChanged();
            if (m_Frame != null)
                m_Frame.SetTitle(LanguageManager.instance.Get("Text_Popup_Setting"));
            UIWrapper_Text.SetTextId(m_BgmLabel, "Text_Popup_Setting_BGM");
            UIWrapper_Text.SetTextId(m_SfxLabel, "Text_Popup_Setting_SE");
            UIWrapper_Text.SetTextId(m_FullscreenLabel, "Text_Popup_Setting_Fullscreen");
            UIWrapper_Text.SetTextId(m_ApplyLabel, "Text_Popup_Setting_Apply");
            UIWrapper_Text.SetTextId(m_DefaultLabel, "Text_Popup_Setting_Default");
        }

        /// <summary>전체 화면 여부를 화면 설정에 반영한다</summary>
        private void OnFullscreenChanged(UIWrapper_Toggle _, bool _isOn)
        {
            Screen.fullScreen = _isOn;
        }
        /// <summary>현재 값이 반영됐음을 알림으로 알린다</summary>
        private void OnClickApply(UIWrapper_Button _)
        {
            if (m_FullscreenToggle != null)
                Screen.fullScreen = m_FullscreenToggle.v.isOn;
            OpenAppliedNotify();
        }
        /// <summary>볼륨·전체 화면을 기본값으로 되돌린다</summary>
        private void OnClickDefault(UIWrapper_Button _)
        {
            if (NumberManager.instance != null)
            {
                NumberManager.instance.Set(BgmVolumeId, DefaultVolume);
                NumberManager.instance.Set(SfxVolumeId, DefaultVolume);
            }
            if (m_FullscreenToggle != null)
                m_FullscreenToggle.SetIsOn(true);
        }
        #endregion
        #region Local Function
        /// <summary>_slider 를 _id 볼륨 값과 양방향으로 잇는다</summary>
        private void BindVolume(UIWrapper_Slider _slider, string _id)
        {
            if (_slider == null || NumberManager.instance == null) return;
            _slider.SetMinMax(0.0f, 1.0f);
            var value = NumberManager.instance.GetValue(_id);
            if (value == null)
                throw new System.InvalidOperationException($"{name}: 볼륨 값이 없다 ({_id})");
            value.AddChanged(this, _ => RefreshVolume(_slider, _id));
            _slider.AddChangeListener(_ => NumberManager.instance.Set(_id, _slider.v.value));
        }
        /// <summary>_slider 표시를 _id 볼륨 값으로 맞춘다</summary>
        private void RefreshVolume(UIWrapper_Slider _slider, string _id)
        {
            if (_slider == null || NumberManager.instance == null) return;
            _slider.Set(NumberManager.instance.GetFloat(_id));
        }
        /// <summary>설정 적용 알림을 띄운다. 알림 팝업·번역 매니저가 없으면 건너뛴다</summary>
        private void OpenAppliedNotify()
        {
            var manager = LocalPopupManager.instance;
            if (manager == null || LanguageManager.instance == null) return;
            var language = LanguageManager.instance;
            manager.TryOpen(NotifyPopupId, new Popup_Notify.SOption(
                language.Get("Text_Popup_Setting"),
                language.Get("Text_Popup_Setting_Applied"),
                language.Get("Text_Core_Close"), null));
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        public override void MCPDetail(MCPReport _report)
        {
            base.MCPDetail(_report);
            var ci = System.Globalization.CultureInfo.InvariantCulture;
            float bgm = NumberManager.instance != null ? NumberManager.instance.GetFloat(BgmVolumeId) : 0.0f;
            float sfx = NumberManager.instance != null ? NumberManager.instance.GetFloat(SfxVolumeId) : 0.0f;
            _report.AddRaw("bgmVolume", bgm.ToString(ci));
            _report.AddRaw("sfxVolume", sfx.ToString(ci));
            _report.AddRaw("fullscreen", Screen.fullScreen ? "true" : "false");
        }
        public override void MCPInteraction(MCPReport _report)
        {
            base.MCPInteraction(_report);
            if (!IsOpened) return;
            _report.Add("SetBgm", "BGM 볼륨 설정 (value 0~1)");
            _report.Add("SetSfx", "효과음 볼륨 설정 (value 0~1)");
            _report.Add("SetFullscreen", "전체 화면 설정 (value 0 끄기 · 1 켜기)");
            _report.Add("Apply", "설정 적용");
            _report.Add("Default", "기본값 복원");
        }
        public override string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case "SetBgm":
                    if (NumberManager.instance == null) return "{\"error\":\"NumberManager 없음\"}";
                    NumberManager.instance.Set(BgmVolumeId, _value);
                    return "{\"success\":true}";
                case "SetSfx":
                    if (NumberManager.instance == null) return "{\"error\":\"NumberManager 없음\"}";
                    NumberManager.instance.Set(SfxVolumeId, _value);
                    return "{\"success\":true}";
                case "SetFullscreen":
                    if (m_FullscreenToggle == null) return "{\"error\":\"전체 화면 토글 없음\"}";
                    m_FullscreenToggle.SetIsOn(0.5f <= _value);
                    return "{\"success\":true}";
                case "Apply": OnClickApply(null); return "{\"success\":true}";
                case "Default": OnClickDefault(null); return "{\"success\":true}";
                default: return base.MCPInteract(_interactionId, _value);
            }
        }
#endif
        #endregion
    }
}
