using System;
using UnityEngine;

namespace Library
{
    /// <summary>여러 곳이 요청한 타임스케일 배율을 모두 곱해 Time.timeScale 에 반영하는 매니저. 한 곳이 0을 걸면 전체가 멈춘다</summary>
    public class TimeManager : GlobalManagerBase
    {
        public static TimeManager instance { get; private set; }
        #region Property
        /// <summary>등록된 배율들과 그 곱. 구독하면 timeScale 변화를 따라갈 수 있다</summary>
        public IReadOnlyFloatFactor TimeScaleFactor => m_TimeScaleFactor;
        #endregion
        #region Value
        private FloatFactor<MonoBehaviour> m_TimeScaleFactor;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void Init()
        {
            m_TimeScaleFactor = new(this, FloatFactor<MonoBehaviour>.ETotalType.Multifly);
            TimeScaleFactor.AddChanged(this, OnUpdate, true);
            base.Init();
        }
        public override void InitGame()
        {
            OnUpdate(null);
            base.InitGame();
        }
        public override void OnShutdown()
        {
            Time.timeScale = 0;
            base.OnShutdown();
        }

        /// <summary>배율이 바뀔 때마다 Time.timeScale 에 반영한다</summary>
        private void OnUpdate(ValueBase _v)
        {
            Time.timeScale = TimeScaleFactor.Total;
        }
        #endregion
        #region Function
        /// <summary>_owner 몫의 배율을 _value 로 건다. 다른 소유자의 배율과 곱해져 최종 timeScale 이 되므로, 혼자 1을 걸어도 남이 0이면 멈춘 채로 있다. _value 는 0 이상이어야 하며 음수·NaN 이면 예외</summary>
        public void SetTimeScale(MonoBehaviour _callBy, MonoBehaviour _owner, float _value)
        {
            if (float.IsNaN(_value) || _value < 0f)
                throw new ArgumentOutOfRangeException(nameof(_value), _value, "타임스케일 배율은 0 이상이어야 한다");
            m_TimeScaleFactor.Set(_callBy, _owner, _value);
        }
        #endregion
    }
}