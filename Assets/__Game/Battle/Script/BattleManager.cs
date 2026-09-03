using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>전투 전역 매니저 — 런 재화 Crumb 를 Bank 에 등록하고 런 누적 획득량을 Number 로 노출한다</summary>
    public class BattleManager : GlobalManagerBase
    {
        public static BattleManager instance { get; private set; }

        #region Property
        /// <summary>Crumb 잔액 (런 내 재화, 저장 없음)</summary>
        public IReadOnlyLongValue Crumb => m_Crumb;
        /// <summary>이번 런 누적 획득 Crumb (결과 점수)</summary>
        public IReadOnlyIntValue CrumbTotal => m_CrumbTotal;
        #endregion
        #region Value
        private LongValue m_Crumb;
        private IntValue m_CrumbTotal;
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
            return InitUtil.IsInit(new ManagerBase[] { BankManager.instance, NumberManager.instance, SoundManager.instance });
        }
        public override void Init()
        {
            m_Crumb = BankManager.instance.Create(this, BattleConst.CrumbId, "", "", 0, false);
            m_CrumbTotal = new IntValue(this, "CrumbTotal", 0);
            NumberManager.instance.Create(this, "CrumbTotal", m_CrumbTotal);
            SoundManager.instance.BGMVolume.AddChanged(this, OnBgmVolumeChanged);
            base.Init();
        }
        /// <summary>BGM 볼륨 변경을 재생 중인 소스에 반영한다</summary>
        private void OnBgmVolumeChanged(ValueBase _)
        {
            if (m_BgmSource != null)
                m_BgmSource.volume = SoundManager.instance.BGMVolume.v;
        }
        #endregion
        #region Function
        /// <summary>_amount 만큼 Crumb 를 적립하고 런 누적에 더한다. 0 이하면 예외</summary>
        public void AddCrumb(int _amount)
        {
            if (_amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(_amount), $"Crumb 적립량이 0 이하다 : {_amount}");
            BankManager.instance.Change(BattleConst.CrumbId, _amount);
            m_CrumbTotal.v += _amount;
        }
        /// <summary>런 시작 시 Crumb 잔액·누적을 0 으로 되돌린다</summary>
        public void ResetRun()
        {
            BankManager.instance.Set(BattleConst.CrumbId, 0);
            m_CrumbTotal.v = 0;
        }
        /// <summary>_clip 을 BGM 으로 루프 재생한다 (SoundManager BGM 볼륨 적용, 같은 클립이 재생 중이면 유지, null 이면 정지)</summary>
        public void PlayBGM(AudioClip _clip)
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
            if (m_BgmSource.clip == _clip && m_BgmSource.isPlaying)
                return;
            m_BgmSource.clip = _clip;
            m_BgmSource.volume = SoundManager.instance.BGMVolume.v;
            m_BgmSource.Play();
        }
        #endregion
    }
}
