using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>위치 이동으로 팝업을 여닫는 연출. m_Targets 의 대상들을 같은 커브·시간으로 동시에 움직이며, 대상마다 타이밍을 다르게 하려면 이 컴포넌트를 여러 개 둔다</summary>
    public class PopupAnimation_Position : PopupAnimationBase
    {
        #region Type
        /// <summary>이동할 대상 하나의 시작·끝 위치</summary>
        [Serializable]
        public class Entry
        {
            [Tooltip("이동 대상 RectTransform")] public RectTransform target;
            [Tooltip("열기 시작 위치(anchoredPosition)")] public Vector2 openStart = new Vector2(0f, -1080f);
            [Tooltip("열기 끝 위치")] public Vector2 openEnd = Vector2.zero;
            [Tooltip("닫기 시작 위치")] public Vector2 closeStart = Vector2.zero;
            [Tooltip("닫기 끝 위치")] public Vector2 closeEnd = new Vector2(0f, -1080f);
        }
        #endregion
        #region Inspector
        [Title("Open")]
        [SerializeField] private float m_OpenStartDelay = 0f;
        [SerializeField] private AnimationCurve m_OpenCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float m_OpenTime = 0.5f;

        [Title("Close")]
        [SerializeField] private float m_CloseStartDelay = 0f;
        [SerializeField] private AnimationCurve m_CloseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float m_CloseTime = 0.5f;

        [Title("Targets (동시 이동)")]
        [Tooltip("같은 타이밍(위 Open/Close 커브·시간)으로 동시에 이동할 대상들. 대상마다 시작·끝 위치를 각자 가진다. 다른 타이밍이 필요하면 컴포넌트를 여러 개 둔다")]
        [SerializeField] private Entry[] m_Targets;
        #endregion
        #region Value
        private Coroutine m_OpenCoroutine = null;
        private Coroutine m_CloseCoroutine = null;
        #endregion

        #region Event
        protected override void OnStartOpen(Action _onEnd)
        {
            m_OpenCoroutine = StartCoroutine(OpenCoroutine());
        }
        protected override void OnStartClose(Action _onEnd)
        {
            m_CloseCoroutine = StartCoroutine(CloseCoroutine());
        }
        protected override void OnCancelState()
        {
            base.OnCancelState();

            if (m_OpenCoroutine != null)
            {
                StopCoroutine(m_OpenCoroutine);
                m_OpenCoroutine = null;
            }
            if (m_CloseCoroutine != null)
            {
                StopCoroutine(m_CloseCoroutine);
                m_CloseCoroutine = null;
            }
        }

        /// <summary>열기 연출을 재생한다</summary>
        // 팝업은 timeScale=0 에서도 움직여야 해서 unscaled 시간을 쓴다
        private IEnumerator OpenCoroutine()
        {
            Collect(true, out var rects, out var starts, out var ends);
            for (int i = 0; i < rects.Count; i++)
                rects[i].anchoredPosition = starts[i];
            if (m_OpenStartDelay > 0f)
                yield return new WaitForSecondsRealtime(m_OpenStartDelay);

            float timer = 0f;
            while (timer < m_OpenTime)
            {
                timer += Time.unscaledDeltaTime;
                float p = m_OpenCurve.Evaluate(timer / m_OpenTime);
                for (int i = 0; i < rects.Count; i++)
                    rects[i].anchoredPosition = Vector2.LerpUnclamped(starts[i], ends[i], p);
                yield return null;
            }

            for (int i = 0; i < rects.Count; i++)
                rects[i].anchoredPosition = ends[i];
            m_OpenCoroutine = null;
            OnEndOpen();
        }
        /// <summary>닫기 연출을 재생한다</summary>
        private IEnumerator CloseCoroutine()
        {
            Collect(false, out var rects, out var starts, out var ends);
            for (int i = 0; i < rects.Count; i++)
                rects[i].anchoredPosition = starts[i];
            if (m_CloseStartDelay > 0f)
                yield return new WaitForSecondsRealtime(m_CloseStartDelay);

            float timer = 0f;
            while (timer < m_CloseTime)
            {
                timer += Time.unscaledDeltaTime;
                float p = m_CloseCurve.Evaluate(timer / m_CloseTime);
                for (int i = 0; i < rects.Count; i++)
                    rects[i].anchoredPosition = Vector2.LerpUnclamped(starts[i], ends[i], p);
                yield return null;
            }

            for (int i = 0; i < rects.Count; i++)
                rects[i].anchoredPosition = ends[i];
            m_CloseCoroutine = null;
            OnEndClose();
        }
        #endregion
        #region Local Function
        /// <summary>이동할 대상과 그 시작·끝 위치를 모은다 (배선되지 않은 대상은 건너뛴다)</summary>
        private void Collect(bool _open, out List<RectTransform> _rects, out List<Vector2> _starts, out List<Vector2> _ends)
        {
            _rects = new List<RectTransform>();
            _starts = new List<Vector2>();
            _ends = new List<Vector2>();
            if (m_Targets == null) return;

            foreach (Entry e in m_Targets)
            {
                if (e == null || e.target == null) continue;
                _rects.Add(e.target);
                _starts.Add(_open ? e.openStart : e.closeStart);
                _ends.Add(_open ? e.openEnd : e.closeEnd);
            }
        }
        #endregion
    }
}
