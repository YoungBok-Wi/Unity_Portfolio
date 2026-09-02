using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Library
{
    /// <summary>Button 컴포넌트를 래핑하여 클릭 딜레이와 이벤트를 관리</summary>
    [RequireComponent(typeof(Button))]
    public class UIWrapper_Button : ControlBase
    {
        #region Inspector
        [SerializeField] private float m_Delay = 0.5f;
        #endregion
        #region Property
        /// <summary>감싸고 있는 Button</summary>
        // 비활성 GameObject 는 Awake 가 돌지 않으므로 여기서 지연 확보한다
        public Button v
        {
            get
            {
                if (m_Button == null)
                    m_Button = GetComponent<Button>();
                return m_Button;
            }
        }
        #endregion
        #region Value
        private Button m_Button;
        private Action<UIWrapper_Button> m_OnClick;
        private Action<UIWrapper_Button, bool> m_OnInteractableChanged;
        private Coroutine m_DelayCor;
        #endregion

        #region Event
        protected override void Awake()
        {
            base.Awake();
            v.onClick.AddListener(OnClick);
        }
        protected override void OnDisable()
        {
            m_DelayCor = null;
            base.OnDisable();
        }

        /// <summary>딜레이 중이 아니면 클릭 리스너를 호출한다</summary>
        private void OnClick()
        {
            if (m_DelayCor != null)
                return;

            m_DelayCor = StartCoroutine(DelayCoroutine());

            m_OnClick?.Invoke(this);
        }

        /// <summary>연속 클릭을 막는 딜레이를 흘려보낸다</summary>
        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(m_Delay);
            m_DelayCor = null;
        }
        #endregion
        #region Function
        /// <summary>클릭 리스너를 등록한다. _onClick 은 이 버튼을 받으며, 딜레이 중의 연속 클릭에서는 호출되지 않는다</summary>
        public void AddClickListener(Action<UIWrapper_Button> _onClick)
        {
            m_OnClick += _onClick;
        }
        /// <summary>활성 상태 변경 리스너를 등록한다. _onInteractableChanged 는 이 버튼과 바뀐 활성 여부를 받으며, 등록 즉시 현재 상태로 1회 호출된다</summary>
        public void AddInteractableChangedListener(Action<UIWrapper_Button, bool> _onInteractableChanged)
        {
            if (_onInteractableChanged == null)
                return;

            m_OnInteractableChanged += _onInteractableChanged;

            _onInteractableChanged.Invoke(this, v.interactable);
        }
        /// <summary>버튼 활성 여부를 _interactable 로 바꾼다. 값이 실제로 바뀔 때만 리스너가 호출된다</summary>
        public void SetInteractable(bool _interactable)
        {
            if (v.interactable == _interactable)
                return;

            v.interactable = _interactable;

            m_OnInteractableChanged?.Invoke(this, _interactable);
        }
        #endregion
    }
}