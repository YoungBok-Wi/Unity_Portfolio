using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>BGM/SE 볼륨을 관리하고 DealManager와 연동하는 사운드 매니저</summary>
    public class SoundManager : GlobalManagerBase
    {
        public static SoundManager instance { get; private set; }
        #region Property
        /// <summary>BGM 볼륨 0~1. PlayerPrefs 로 영속된다</summary>
        public IReadOnlyFloatValue BGMVolume => m_BGMVolume;
        /// <summary>효과음 볼륨 0~1. PlayerPrefs 로 영속된다</summary>
        public IReadOnlyFloatValue SEVolume => m_SEVolume;
        #endregion
        #region Value
        private FloatValue m_BGMVolume;
        private FloatValue m_SEVolume;
        // SE 단발 재생 전용 소스. 최초 재생 시 만든다
        private AudioSource m_SESource;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void Init()
        {
            m_BGMVolume = SaveUtil.Create(this, null, new FloatValue(this, "BGMVolume", 1.0f), SaveUtil.EType.PlayerPrefs);
            m_SEVolume = SaveUtil.Create(this, null, new FloatValue(this, "SEVolume", 1.0f), SaveUtil.EType.PlayerPrefs);

            m_BGMVolume.AddConstraintChanged(this, (_) =>
            {
                m_BGMVolume.Set(Mathf.Clamp01(m_BGMVolume.v), false, false);
            });
            m_SEVolume.AddConstraintChanged(this, (_) =>
            {
                m_SEVolume.Set(Mathf.Clamp01(m_SEVolume.v), false, false);
            });

            DealManager.instance.Create(this, "BGMVolume", (_deal, _seed) =>
            {   //Need
                return m_BGMVolume.v < _deal.CountFloat;
            }, (_deal) =>
            {   //NeedValue
                return new ValueBase[] { m_BGMVolume };
            }, (_deal, _seed) =>
            {   //Set
                SetBGMVolume(_deal.CountFloat);
                return new SDeal[] { _deal };
            }, (_deal, _seed) =>
            {   //Change
                SetBGMVolume(m_BGMVolume.v + _deal.CountFloat);
                return new SDeal[] { _deal };
            }, null);

            DealManager.instance.Create(this, "SEVolume", (_deal, _seed) =>
            {   //Need
                return m_SEVolume.v < _deal.CountFloat;
            }, (_deal) =>
            {   //NeedValue
                return new ValueBase[] { m_SEVolume };
            }, (_deal, _seed) =>
            {   //Set
                SetSEVolume(_deal.CountFloat);
                return new SDeal[] { _deal };
            }, (_deal, _seed) =>
            {   //Change
                SetSEVolume(m_SEVolume.v + _deal.CountFloat);
                return new SDeal[] { _deal };
            }, null);

            NumberManager.instance.Create(this, "BGMVolume", m_BGMVolume);
            NumberManager.instance.Create(this, "SEVolume", m_SEVolume);
            LanguageManager.instance.Create(this, "BGMVolume", m_BGMVolume, (_, _table) => _table.SetEng($"{Mathf.RoundToInt(m_BGMVolume.v * 100)}%"));
            LanguageManager.instance.Create(this, "SEVolume", m_SEVolume, (_, _table) => _table.SetEng($"{Mathf.RoundToInt(m_SEVolume.v * 100)}%"));

            base.Init();
        }
        #endregion
        #region Function
        /// <summary>BGM 볼륨을 _value 로 바꾼다. 0~1 범위로 보정되며 저장은 자동이다</summary>
        public void SetBGMVolume(float _value)
        {
            m_BGMVolume.v = _value;
        }
        /// <summary>효과음 볼륨을 _value 로 바꾼다. 0~1 범위로 보정되며 저장은 자동이다</summary>
        public void SetSEVolume(float _value)
        {
            m_SEVolume.v = _value;
        }
        /// <summary>_clip 효과음을 설정된 SE 볼륨으로 단발 재생한다. _clip 이 null 이면 무시한다</summary>
        public void PlaySE(AudioClip _clip)
        {
            if (_clip == null) return;
            if (m_SESource == null)
            {
                m_SESource = gameObject.AddComponent<AudioSource>();
                m_SESource.playOnAwake = false;
            }
            m_SESource.PlayOneShot(_clip, m_SEVolume.v);
        }
        #endregion
    }
}
