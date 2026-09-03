using Library;
using System;
using TMPro;
using UnityEngine;

namespace Game
{
    /// <summary>데미지 숫자 팝 — 지정 위치에서 위로 떠오르며 사라진다 (Popup_HUD 전용, 풀 복제 대상)</summary>
    public class Control_DamagePop : ControlBase
    {
        #region Inspector
        [SerializeField, Tooltip("데미지 숫자 라벨")] private TMP_Text m_Label;
        [SerializeField, Tooltip("표시 수명 (초)")] private float m_LifeSec = 0.6f;
        [SerializeField, Tooltip("수명 동안 떠오르는 거리 (캔버스 px)")] private float m_RiseDist = 60f;
        #endregion
        #region Value
        private RectTransform m_Rect;
        private Vector2 m_Origin;
        private float m_Timer;
        private Action<Control_DamagePop> m_OnDone;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            m_Rect = (RectTransform)transform;
        }
        /// <summary>수명 진행 — 위로 이동·투명해지고 수명이 끝나면 완료 콜백을 부른다</summary>
        private void Update()
        {
            m_Timer += Time.deltaTime;
            float t = Mathf.Clamp01(m_Timer / m_LifeSec);
            m_Rect.anchoredPosition = m_Origin + Vector2.up * (m_RiseDist * t);
            m_Label.alpha = 1f - t;
            if (t < 1f)
                return;
            var onDone = m_OnDone;
            m_OnDone = null;
            onDone?.Invoke(this);
        }
        #endregion
        #region Function
        /// <summary>_localPos(부모 로컬)에서 _damage 를 _color 로 띄우기 시작하고, 수명이 끝나면 _onDone 을 부른다</summary>
        public void Show(Vector2 _localPos, int _damage, Color _color, Action<Control_DamagePop> _onDone)
        {
            m_Origin = _localPos;
            m_Timer = 0f;
            m_OnDone = _onDone;
            m_Rect.anchoredPosition = _localPos;
            m_Label.text = _damage.ToString();
            m_Label.color = _color;
            m_Label.alpha = 1f;
        }
        #endregion
    }
}
