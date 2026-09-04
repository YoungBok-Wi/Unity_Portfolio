using System;
using System.Collections;
using UnityEngine;

namespace Library
{
    /// <summary>CanvasGroup 알파를 코드로 보간해 씬 전환 연출을 재생하는 기본 구현. 보간 시간은 timeScale 과 무관하게 흐른다</summary>
    public class SceneChangeAni_Fade : SceneChangeAni
    {
        #region Inspector
        [SerializeField] private CanvasGroup m_CanvasGroup;
        #endregion
        #region Value
        // 화면을 가리거나 걷는 데 걸리는 시간 (초)
        private const float FadeSecond = 0.5f;
        private Coroutine m_FadeCor;
        #endregion

        #region Event
        public override void Init()
        {
            if (m_CanvasGroup == null)
                throw new InvalidOperationException($"{name}의 CanvasGroup 이 배선되지 않았다");
            m_CanvasGroup.alpha = 0f;
            base.Init();
        }
        #endregion
        #region Local Function
        private void StartFade(float _from, float _to, Action _onEnd)
        {
            if (m_FadeCor != null)
                StopCoroutine(m_FadeCor);
            m_FadeCor = StartCoroutine(FadeCor(_from, _to, _onEnd));
        }
        private IEnumerator FadeCor(float _from, float _to, Action _onEnd)
        {
            m_CanvasGroup.alpha = _from;
            float elapsed = 0f;
            while (elapsed < FadeSecond)
            {
                elapsed += Time.unscaledDeltaTime;
                m_CanvasGroup.alpha = Mathf.Lerp(_from, _to, Mathf.Clamp01(elapsed / FadeSecond));
                yield return null;
            }
            m_CanvasGroup.alpha = _to;
            m_FadeCor = null;
            _onEnd();
        }
        #endregion
        #region Function
        /// <summary>화면을 덮는 페이드를 시작하고 끝나면 PostChange 로 씬 로드를 요청한다</summary>
        public override void StartAni()
        {
            base.StartAni();
            StartFade(0f, 1f, PostChange);
        }
        /// <summary>화면을 걷는 페이드를 시작하고 끝나면 PostEnd 로 전환 종료를 알린다</summary>
        public override void EndAni()
        {
            base.EndAni();
            StartFade(1f, 0f, PostEnd);
        }
        #endregion
    }
}