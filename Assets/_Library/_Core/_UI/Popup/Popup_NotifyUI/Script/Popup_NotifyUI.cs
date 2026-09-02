using System.Collections;
using TMPro;
using UnityEngine;

namespace Library
{
    /// <summary>ObjectPool 기반으로 일정 시간 후 자동으로 사라지는 토스트 알림 팝업</summary>
    public class Popup_NotifyUI : PopupBase
    {
        public static Popup_NotifyUI instance { get; private set; }
        #region Type
        public struct SOption
        {
            public string text;
            public float time;

            public SOption(string _text, float _time = 6)
            {
                text = _text;
                time = _time;
            }
        }
        #endregion
        #region Inspector
        [SerializeField] private GameObject m_Template;
        // 템플릿의 문구 노드. 복제본에서 같은 이름의 자식을 찾는 키로도 쓴다
        [SerializeField] private TMP_Text m_TemplateText;
        #endregion
        #region Value
        private ObjectPool m_NotifyPool;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 이 팝업이 없는 씬에서 파괴된 인스턴스 접근을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            m_NotifyPool = new ObjectPool(m_Template, m_Template.transform.parent, 30);
            base.Init();
        }
        public override void OnOpen(object _option = null)
        {
            base.OnOpen(_option);
            if (_option == null)
                return;

            var obj = m_NotifyPool.Get();

            SOption o = (SOption)_option;
            var tmpText = obj.transform.Find(m_TemplateText.name).GetComponent<TMP_Text>();
            tmpText.text = o.text;

            StartCoroutine(NotifyCoroutine(obj, o.time));
        }

        /// <summary>지정된 시간 후 알림을 닫기 애니메이션 재생 후 풀에 반환</summary>
        private IEnumerator NotifyCoroutine(GameObject _object, float _time)
        {
            yield return new WaitForSecondsRealtime(_time);
            _object.GetComponent<Animator>().Play("Close");

            yield return new WaitForSecondsRealtime(1.0f);
            m_NotifyPool.Return(_object);
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        // 자동 소멸 토스트. 추가 상태·조작이 없어 base(opened·Open/Close)만 노출한다
#endif
        #endregion
    }
}