using Library;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>12x7 얼굴 타일을 대각선 순서로 팝 전환한다.</summary>
    public class SceneChangeAni_Face : SceneChangeAni
    {
        #region Inspector
        [SerializeField, Tooltip("행 우선 순서의 12x7 타일")] private RectTransform[] m_Tiles;
        [SerializeField, Tooltip("모든 타일에 표시할 플레이어 얼굴 Sprite")] private Sprite m_Face;
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
            if (m_Face == null)
                throw new InvalidOperationException($"{name} : 플레이어 얼굴 Sprite가 없다");
            foreach (var tile in m_Tiles)
            {
                if (tile == null)
                    throw new InvalidOperationException($"{name} : Face 타일 배열에 빈 슬롯이 있다");
                var image = tile.GetComponent<Image>();
                if (image == null)
                    throw new InvalidOperationException($"{tile.name} : Image가 없다");
                image.sprite = m_Face;
                image.preserveAspect = true;
            }
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
                    float scale = _from < _to
                        ? Mathf.LerpUnclamped(_from, _to, EaseOutBack(progress))
                        : Mathf.SmoothStep(_from, _to, progress);
                    SetScale(m_Tiles[i], scale);
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
        private static float EaseOutBack(float _progress)
        {
            float shifted = _progress - 1f;
            return 1f + 2.70158f * shifted * shifted * shifted + 1.70158f * shifted * shifted;
        }
        #endregion
    }
}
