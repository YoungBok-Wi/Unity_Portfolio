using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Resources/SpriteAnim 의 "{접두}_{동작}_{NN}" 프레임을 순차 로드해 재생하는 스프라이트 애니메이터</summary>
    public class SpriteAnimPlayer : MonoBehaviour
    {
        #region Inspector
        [SerializeField, Tooltip("프레임을 그릴 렌더러")] private SpriteRenderer m_Renderer;
        [SerializeField, Tooltip("프레임명 접두 (예: AnimationSheet_Casual_Enemy_Apple)")] private string m_Prefix;
        [SerializeField, Tooltip("초당 프레임 수")] private float m_Fps = 10f;
        #endregion
        #region Property
        /// <summary>재생 중인 동작명. 아직 없으면 null</summary>
        public string CurAction { get; private set; }
        /// <summary>비루프 동작이 마지막 프레임까지 재생됐는지</summary>
        public bool IsFinished { get; private set; }
        #endregion
        #region Value
        private readonly Dictionary<string, Sprite[]> m_Frames = new();
        private Sprite[] m_Cur;
        private bool m_Loop;
        private float m_Timer;
        #endregion

        #region Event
        private void Update()
        {
            if (m_Cur == null || IsFinished)
                return;
            m_Timer += Time.deltaTime * m_Fps;
            int frame = (int)m_Timer;
            if (m_Cur.Length <= frame)
            {
                if (m_Loop)
                {
                    m_Timer -= m_Cur.Length;
                    frame = (int)m_Timer;
                }
                else
                {
                    IsFinished = true;
                    frame = m_Cur.Length - 1;
                }
            }
            m_Renderer.sprite = m_Cur[frame];
        }
        #endregion
        #region Local Function
        /// <summary>_action 프레임 배열을 첫 조회 때 Resources 에서 모아 캐시한다. 1장도 없으면 예외</summary>
        private Sprite[] GetFrames(string _action)
        {
            if (m_Frames.TryGetValue(_action, out var frames))
                return frames;
            var list = new List<Sprite>();
            for (int i = 1; ; i++)
            {
                var sprite = Resources.Load<Sprite>($"SpriteAnim/{m_Prefix}_{_action}_{i:00}");
                if (sprite == null)
                    break;
                list.Add(sprite);
            }
            if (list.Count == 0)
                throw new InvalidOperationException($"{name} : Resources/SpriteAnim/{m_Prefix}_{_action}_01 프레임이 없다");
            frames = list.ToArray();
            m_Frames.Add(_action, frames);
            return frames;
        }
        #endregion
        #region Function
        /// <summary>_action 동작을 처음부터 재생한다. _loop 면 반복하며, 같은 동작을 루프 중이면 그대로 둔다</summary>
        public void Play(string _action, bool _loop)
        {
            if (_loop && m_Loop && !IsFinished && CurAction == _action)
                return;
            m_Cur = GetFrames(_action);
            CurAction = _action;
            m_Loop = _loop;
            m_Timer = 0;
            IsFinished = false;
            m_Renderer.sprite = m_Cur[0];
        }
        /// <summary>_action 동작 한 바퀴의 재생 길이(초)를 반환한다</summary>
        public float GetLength(string _action)
        {
            return GetFrames(_action).Length / m_Fps;
        }
        /// <summary>좌우 반전을 _isLeft 로 바꾼다 (원본 프레임은 우향)</summary>
        public void SetFlip(bool _isLeft)
        {
            m_Renderer.flipX = _isLeft;
        }
        #endregion
    }
}
