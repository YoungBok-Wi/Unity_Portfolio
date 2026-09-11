using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq; // ToArray() 사용을 위해 추가
using UnityEngine;

namespace Library
{
    /// <summary>같은 프레임에 몰린 콜백을 하나로 합치고, 매 프레임 정해진 시간만큼만 처리해 부하를 나눠 지는 매니저</summary>
    public class DelegateManager : GlobalManagerBase
    {
        public static DelegateManager instance { get; private set; }

        #region Inspector
        [SerializeField, TabGroup("DelegateManager", "설정"), Range(0.1f, 10.0f), SuffixLabel("ms")]  private double m_MaxMSPerFrame = 2.0f;
        #endregion
        #region Value
        private Dictionary<(object owner, ValueBase value), Action<ValueBase>> m_EventMap = new();
        private Stopwatch m_Stopwatch = new Stopwatch();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }

        private void Update()
        {
            if (m_EventMap.Count == 0)
                return;

            LogManager.instance.Log("Delegate",$"이벤트 호출 시작, 이벤트수: {m_EventMap.Count}");
            m_Stopwatch.Restart();
            foreach(var v in m_EventMap.Keys.ToArray())
            {
                //호출 전에 빼 둔다 — 예외가 나도 다음 프레임에 무한 재시도되지 않는다
                var action = m_EventMap[v];
                m_EventMap.Remove(v);
                action.Invoke(v.value);

                if (m_MaxMSPerFrame <= m_Stopwatch.Elapsed.TotalMilliseconds)
                    break;
            }
            m_Stopwatch.Stop();
            LogManager.instance.Log("Delegate",$"이벤트 호출 완료, 남은이벤트수: {m_EventMap.Count} / 걸린시간: {m_Stopwatch.Elapsed.TotalMilliseconds:F4}ms");
        }
        #endregion
        #region Function
        /// <summary>_action 을 다음 프레임 처리 대기열에 넣는다. 같은 _callBy·_value 조합이 이미 대기 중이면 합쳐지므로, 한 프레임에 여러 번 불러도 한 번만 실행된다</summary>
        public void AddUpdate(object _callBy, ValueBase _value, Action<ValueBase> _action)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy));
            if (_value == null)
                throw new ArgumentNullException(nameof(_value));
            if (_action == null)
                throw new ArgumentNullException(nameof(_action));

            m_EventMap.AddEx((_callBy, _value), _action);
        }
        #endregion
    }
}