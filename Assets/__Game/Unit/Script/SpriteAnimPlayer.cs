using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>인스펙터 클립 배열을 이름으로 찾아 재생하는 스프라이트 애니메이터.</summary>
    public class SpriteAnimPlayer : MonoBehaviour
    {
        #region Type
        [Serializable]
        public struct SAnimClip
        {
            public string Name;
            public Sprite[] Frames;
            public float Fps;
        }
        #endregion
        #region Inspector
        [SerializeField, Tooltip("프레임을 그릴 렌더러")] private SpriteRenderer m_Renderer;
        [SerializeField, Tooltip("동작명·프레임·초당 프레임 수 목록")] private SAnimClip[] m_Clips;
        #endregion
        #region Property
        /// <summary>현재 재생 동작명을 반환한다.</summary>
        public string CurAction { get; private set; }
        /// <summary>현재 프레임 인덱스를 반환한다.</summary>
        public int CurFrame { get; private set; }
        /// <summary>비반복 재생 완료 여부를 반환한다.</summary>
        public bool IsFinished { get; private set; }
        #endregion
        #region Value
        private Dictionary<string, SAnimClip> m_ClipByName;
        private SAnimClip m_CurClip;
        private bool m_Loop;
        private float m_Timer;
        #endregion

        #region Event
        private void Update()
        {
            if (m_CurClip.Frames == null || IsFinished)
                return;
            m_Timer += Time.deltaTime * m_CurClip.Fps;
            int frame = (int)m_Timer;
            if (m_CurClip.Frames.Length <= frame)
            {
                if (m_Loop)
                {
                    m_Timer %= m_CurClip.Frames.Length;
                    frame = (int)m_Timer;
                }
                else
                {
                    IsFinished = true;
                    frame = m_CurClip.Frames.Length - 1;
                }
            }
            CurFrame = frame;
            m_Renderer.sprite = m_CurClip.Frames[frame];
        }
        #endregion
        #region Local Function
        private void EnsureClips()
        {
            if (m_ClipByName != null)
                return;
            if (m_Renderer == null)
                throw new InvalidOperationException($"{name} : SpriteRenderer가 배선되지 않았다");
            m_ClipByName = new Dictionary<string, SAnimClip>();
            if (m_Clips == null)
                return;
            foreach (var clip in m_Clips)
            {
                if (string.IsNullOrEmpty(clip.Name))
                    throw new InvalidOperationException($"{name} : 이름이 빈 애니메이션 클립이 있다");
                if (clip.Frames == null || clip.Frames.Length == 0)
                    throw new InvalidOperationException($"{name} : {clip.Name} 클립 프레임이 비었다");
                if (clip.Fps <= 0f)
                    throw new InvalidOperationException($"{name} : {clip.Name} 클립 FPS가 0 이하다");
                if (!m_ClipByName.TryAdd(clip.Name, clip))
                    throw new InvalidOperationException($"{name} : 애니메이션 클립 이름이 겹친다 ({clip.Name})");
            }
        }
        private SAnimClip GetClip(string _action)
        {
            EnsureClips();
            if (!m_ClipByName.TryGetValue(_action, out var clip))
                throw new InvalidOperationException($"{name} : {_action} 애니메이션 클립이 없다");
            return clip;
        }
        #endregion
        #region Function
        /// <summary>_action 클립을 _loop 설정으로 재생한다.</summary>
        public void Play(string _action, bool _loop)
        {
            if (_loop && m_Loop && !IsFinished && CurAction == _action)
                return;
            m_CurClip = GetClip(_action);
            CurAction = _action;
            CurFrame = 0;
            m_Loop = _loop;
            m_Timer = 0f;
            IsFinished = false;
            m_Renderer.sprite = m_CurClip.Frames[0];
        }
        /// <summary>_action 클립 길이를 초 단위로 반환한다.</summary>
        public float GetLength(string _action)
        {
            var clip = GetClip(_action);
            return clip.Frames.Length / clip.Fps;
        }
        /// <summary>_isLeft에 맞춰 렌더러의 수평 방향을 바꾼다.</summary>
        public void SetFlip(bool _isLeft)
        {
            if (m_Renderer == null)
                return;
            var scale = m_Renderer.transform.localScale;
            scale.x = _isLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            m_Renderer.transform.localScale = scale;
        }
        #endregion
    }
}
