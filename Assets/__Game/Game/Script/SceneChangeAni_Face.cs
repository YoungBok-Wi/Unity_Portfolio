using Library;
using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    /// <summary>12x7 얼굴 타일을 대각선 순서로 팝 전환한다.</summary>
    public class SceneChangeAni_Face : SceneChangeAni
    {
        #region Inspector
        [SerializeField, Tooltip("행 우선 순서의 12x7 타일")] private RectTransform[] m_Tiles;
        #endregion
        #region Value

        private const int ColumnCount = 12;
        private const int RowCount = 7;
        private const float TotalSecond = 0.6f;
        private Coroutine m_Coroutine;
        #endregion

        #region Event
        public override void Init()
        {
            if (m_Tiles == null || m_Tiles.Length != ColumnCount * RowCount)
                throw new InvalidOperationException($"{name} : Face 타일은 {ColumnCount * RowCount}개여야 한다");
            SetAllScale(0f);
            base.Init();
        }
        public override void StartAni()
        {
            base.StartAni();
            Play(0f, 1f, PostChange);
        }
        public override void EndAni()
        {
            base.EndAni();
            Play(1f, 0f, PostEnd);
        }
        #endregion
        #region Local Function
        private void Play(float _from, float _to, Action _onEnd)
        {
            if (m_Coroutine != null)
                StopCoroutine(m_Coroutine);
            m_Coroutine = StartCoroutine(PlayRoutine(_from, _to, _onEnd));
        }
        private IEnumerator PlayRoutine(float _from, float _to, Action _onEnd)
        {
            float scaleSecond = TotalSecond * 0.5f;
            float delayStep = scaleSecond / (ColumnCount + RowCount - 2);
            float elapsed = 0f;
            while (elapsed < TotalSecond)
            {
                elapsed += Time.unscaledDeltaTime;
                for (int i = 0; i < m_Tiles.Length; i++)
                {
                    int row = i / ColumnCount;
                    int column = i % ColumnCount;
                    float progress = Mathf.Clamp01((elapsed - (row + column) * delayStep) / scaleSecond);
                    SetScale(m_Tiles[i], Mathf.Lerp(_from, _to, progress));
                }
                yield return null;
            }
            SetAllScale(_to);
            m_Coroutine = null;
            _onEnd();
        }
        private void SetAllScale(float _scale)
        {
            foreach (var tile in m_Tiles)
            {
                if (tile == null)
                    throw new InvalidOperationException($"{name} : Face 타일 배열에 빈 슬롯이 있다");
                SetScale(tile, _scale);
            }
        }
        private static void SetScale(RectTransform _tile, float _scale)
        {
            _tile.localScale = new Vector3(_scale, _scale, 1f);
        }
        #endregion
    }
}
