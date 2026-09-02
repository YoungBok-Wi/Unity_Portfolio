using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Library
{
    /// <summary>팝업 베이스 클래스, Open/Close/애니메이션/컨트롤 관리</summary>
    public abstract class PopupBase : ObjectBase
    {
        #region Inspector
        [SerializeField, TabGroup("PopupBase", "기본")] private bool m_IsCloseByCancel = false;
        [SerializeField, TabGroup("PopupBase", "기본")] private bool m_IsDefaultOpen = false;
        [SerializeField, TabGroup("PopupBase", "기본")] private int m_FixedOrder = -1;
        #endregion
        #region Property
        /// <summary>Cancel 키로 닫기 가능 여부</summary>
        public bool IsCloseByCancel => m_IsCloseByCancel;
        /// <summary>시작 시 자동 열림 여부</summary>
        public bool IsDefaultOpen => m_IsDefaultOpen;
        /// <summary>고정 정렬 순서, -1이면 동적</summary>
        public int FixedOreder => m_FixedOrder;
        /// <summary>열기 애니메이션 진행 중 여부</summary>
        public bool IsOpening { get; private set; } = false;
        /// <summary>현재 열려있는 상태 여부</summary>
        public bool IsOpened { get; private set; } = false;
        /// <summary>닫기 애니메이션 진행 중 여부</summary>
        public bool IsClosing { get; private set; } = false;
        /// <summary>표시 상태 여부</summary>
        public bool IsShow { get; private set; } = true;
        /// <summary>팝업 Canvas 컴포넌트</summary>
        public Canvas PopupCanvas { get; private set; }
        /// <summary>팝업 GraphicRaycaster 컴포넌트</summary>
        public GraphicRaycaster PopupGraphicRaycaster { get; private set; }
        #endregion
        #region Value
        private List<ControlBase> m_Controls = new();
        private List<PopupAnimationBase> m_Animations = new();
        private Action<object> m_OnOpen;
        private Action<object> m_OnClose;
        /// <summary>진행 중인 닫기 연출에 넘어온 옵션 — 연출이 접혀 닫힘을 대신 완결할 때 그대로 전달한다</summary>
        private object m_CloseOption;
        /// <summary>열기 연출 세대 — 접힌 연출의 완료 콜백이 새 연출의 카운터를 건드리지 못하게 거른다</summary>
        private int m_OpenSeq = 0;
        /// <summary>닫기 연출 세대 — 접힌 연출의 완료 콜백이 닫힘을 두 번 완결하지 못하게 거른다</summary>
        private int m_CloseSeq = 0;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            LocalPopupManager.instance.OnRegisterObject(this);
            base.InitSingleton();
        }
        public override void Init()
        {
            PopupCanvas = GetComponent<Canvas>();
            PopupGraphicRaycaster = GetComponent<GraphicRaycaster>();
            PopupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            PopupCanvas.worldCamera = LocalPopupManager.instance.UICamera;
            base.Init();
        }
        public override void InitUIOnce()
        {
            foreach (var v in m_Controls)
                v.InitUIOnce();
            gameObject.SetActive(false);
            base.InitUIOnce();
        }
        public override void InitUI()
        {
            foreach (var v in m_Controls)
                v.InitUI();
            base.InitUI();
        }
        public override void OnShutdown()
        {
            m_OnOpen = null;
            m_OnClose = null;
            foreach (var v in m_Controls)
            {
                v.OnShutdown();
                v.enabled = false;
            }
            base.OnShutdown();
        }

        /// <summary>Canvas 정렬 순서를 _order 로 맞춘다. 고정 순서(m_FixedOrder)가 있으면 _order 를 무시하고 그 값을 쓴다</summary>
        public void OnSort(int _order)
        {
            if (0 <= m_FixedOrder)
            {
                PopupCanvas.planeDistance = 100 - m_FixedOrder;
                PopupCanvas.sortingOrder = m_FixedOrder;
            }
            else
            {
                PopupCanvas.planeDistance = 100 - _order;
                PopupCanvas.sortingOrder = _order;
            }
        }
        public virtual void OnOpen(object _option = null)
        {
            IsOpened = true;
            gameObject.SetActive(IsOpened & IsShow);
            m_OnOpen?.Invoke(_option);
            foreach (var v in m_Controls)
                v.OnOpen();

            if (0 < m_Animations.Count)
            {
                IsOpening = true;
                int seq = ++m_OpenSeq;
                int counter = m_Animations.Count;
                Action onEnd = () =>
                {
                    if (seq != m_OpenSeq)
                        return;
                    counter -= 1;
                    if (counter == 0)
                        IsOpening = false;
                };
                foreach (var v in m_Animations)
                    v.StartOpen(onEnd);
            }
        }
        public virtual void OnStartClose(object _option = null)
        {
            if (IsClosing)
                return;

            if (0 < m_Animations.Count)
            {
                IsClosing = true;
                m_CloseOption = _option;
                int seq = ++m_CloseSeq;
                int counter = m_Animations.Count;
                Action onEnd = () =>
                {
                    if (seq != m_CloseSeq)
                        return;
                    counter -= 1;
                    if (counter == 0)
                    {
                        // OnClose 보다 먼저 해제한다 — 닫기 리스너가 곧바로 재열림을 요청해도 닫는 중으로 보이지 않게
                        IsClosing = false;
                        OnClose(_option);
                    }
                };
                foreach (var v in m_Animations)
                    v.StartClose(onEnd);
            }
            else
                OnClose(_option);
        }
        /// <summary>진행 중인 닫기 연출을 접고 닫힘을 즉시 완결한다(IsClosing 해제 후 OnClose 호출). 닫기 리스너는 OnStartClose 에 넘어온 옵션을 그대로 받는다. 닫는 중이 아니면 아무 일도 하지 않는다</summary>
        public void OnCancelClose()
        {
            if (!IsClosing)
                return;

            // 세대를 올려 진행 중 연출의 완료 콜백을 무효화한다 — 연출이 접히며 콜백이 대신 불려도 닫힘이 두 번 완결되지 않게
            m_CloseSeq += 1;
            IsClosing = false;
            var option = m_CloseOption;
            m_CloseOption = null;
            OnClose(option);
        }
        public virtual void OnClose(object _option = null)
        {
            IsOpened = false;
            m_OnClose?.Invoke(_option);
            gameObject.SetActive(IsOpened & IsShow);
            foreach (var v in m_Controls)
                v.OnClose();
        }
        /// <summary>팝업 표시 여부를 _isShow 로 설정한다. 열린 상태에서만 실제로 보인다</summary>
        public void OnSetShow(bool _isShow)
        {
            IsShow = _isShow;
            gameObject.SetActive(IsOpened & IsShow);
        }
        /// <summary>_control 을 이 팝업에 등록한다. 이미 초기화가 끝난 뒤 등록되면 초기화를 그 자리에서 따라잡는다</summary>
        public void OnRegisterControl(ControlBase _control)
        {
            m_Controls.Add(_control);

            if (Local.instance.IsInited)
            {
                _control.InitUIOnce();
                _control.InitUI();
            }
            if (Local.instance.IsGameInited)
                _control.InitUI();
        }
        /// <summary>_animation 을 이 팝업의 열기·닫기 연출로 등록한다</summary>
        public void OnRegisterPopupAnimation(PopupAnimationBase _animation)
        {
            m_Animations.Add(_animation);
        }

        public virtual bool OnInputCancel(InputAction.CallbackContext _context)
        {
            // 열린 상태만 기본 소비한다 — 닫힌 등록 팝업 순회에서 닫기 소비로 입력을 삼키지 않게 (닫힌 상태 소비는 파생 override 몫)
            if (_context.performed && m_IsCloseByCancel && IsOpened)
            {
                Close();
                return true;
            }

            return false;
        }
        /// <summary>언어 변경 훅. 번역이 필요한 팝업은 override 해서 라벨을 현재 언어로 갱신한다. LocalPopupManager 가 초기화 시 1회, 이후 언어가 바뀔 때마다 호출한다</summary>
        public virtual void OnLanguageChanged()
        {
        }
        #endregion
        #region Function
        /// <summary>이 팝업을 연다. _option 은 열기 리스너로 그대로 전달된다</summary>
        public void Open(object _option = null)
        {
            LocalPopupManager.instance.Open(gameObject.name, _option);
        }
        /// <summary>이 팝업을 닫는다. _option 은 닫기 리스너로 그대로 전달된다</summary>
        public void Close(object _option = null)
        {
            LocalPopupManager.instance.Close(gameObject.name, _option);
        }
        /// <summary>열기 리스너를 등록한다. _onOpen 은 Open 에 넘긴 _option 을 받는다</summary>
        public void AddOpenListener(Action<object> _onOpen)
        {
            m_OnOpen += _onOpen;
        }
        /// <summary>닫기 리스너를 등록한다. _onClose 는 Close 에 넘긴 _option 을 받는다</summary>
        public void AddCloseListener(Action<object> _onClose)
        {
            m_OnClose += _onClose;
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        /// <summary>MCP 열기/닫기 조작 ID</summary>
        protected const string MCPOpenId = "Open";
        protected const string MCPCloseId = "Close";
        /// <summary>가림 판정 기준 RectTransform. 기본은 팝업 루트이며, 팝업별로 override하여 본문 패널 등으로 좁힐 수 있다</summary>
        protected virtual RectTransform MCPVisibleRect => transform as RectTransform;
        /// <summary>이 팝업을 지금 조작할 수 있는지. 닫힌 팝업은 열 수 있으므로 true, 열린 팝업은 가려지지 않았을 때만 true</summary>
        public bool MCPUsable => !IsOpened || !MCPIsCovered();
        /// <summary>기준 Rect 가 위에 그려지는 단일 불투명 그래픽에 완전히 덮였는지 판정한다</summary>
        // 기준 Rect 하위는 이 팝업의 콘텐츠라 제외한다. 여러 그래픽이 조합으로 가리는 경우는 판정하지 않는다
        protected bool MCPIsCovered()
        {
            var rect = MCPVisibleRect;
            if (rect == null) return false;
            var selfCanvas = GetComponentInParent<Canvas>();
            if (selfCanvas == null) return false;
            var selfRoot = selfCanvas.rootCanvas;

            var refCorners = ScreenCorners(rect, CanvasCamera(selfRoot));
            var refPath = SiblingPath(rect.transform, selfRoot.transform);

            foreach (var graphic in FindObjectsByType<UnityEngine.UI.Graphic>(FindObjectsSortMode.None))
            {
                if (!(graphic is UnityEngine.UI.Image) && !(graphic is UnityEngine.UI.RawImage)) continue;
                if (!graphic.raycastTarget || !graphic.isActiveAndEnabled) continue;
                if (!IsOpaque(graphic)) continue;
                if (graphic.transform == rect.transform || graphic.transform.IsChildOf(rect.transform)) continue;
                var graphicCanvas = graphic.canvas;
                if (graphicCanvas == null) continue;

                // 기준 Rect보다 위에 그려지는 그래픽만 가림 후보다 — 같은 Canvas는 하이어라키 순서, 다른 Canvas(팝업별 Canvas)는 sortingOrder로 비교한다
                var graphicRoot = graphicCanvas.rootCanvas;
                if (graphicRoot == selfRoot)
                {
                    if (ComparePath(SiblingPath(graphic.transform, selfRoot.transform), refPath) <= 0) continue;
                }
                else
                {
                    if (graphicRoot.sortingOrder <= selfRoot.sortingOrder) continue;
                }

                var coverCorners = ScreenCorners(graphic.rectTransform, CanvasCamera(graphicRoot));
                if (Contains(coverCorners, refCorners)) return true;
            }
            return false;
        }
        /// <summary>_graphic 이 아래를 완전히 가리는 불투명 상태인지 판정한다 (그래픽 색 알파 × 상위 CanvasGroup 누적 알파)</summary>
        // 임계값을 두지 않고 알파 1(완전 불투명)만 가림으로 본다 — 1 미만이면 아래 팝업이 그만큼 비쳐 실제로 보이기 때문이다. 알파는 Color32 의 255/255 라 1 비교가 정확하다
        // 단순화: 스프라이트 텍스처 자체의 투명 픽셀은 보지 않는다 (알파 1 색의 투명 이미지는 여전히 가림으로 센다 — 필요해지면 스프라이트 알파 샘플링으로 넓힌다)
        private static bool IsOpaque(UnityEngine.UI.Graphic _graphic)
        {
            float alpha = _graphic.color.a;
            for (var cur = _graphic.transform; cur != null; cur = cur.parent)
            {
                var group = cur.GetComponent<CanvasGroup>();
                if (group == null) continue;
                alpha *= group.alpha;
                if (group.ignoreParentGroups) break;
            }
            return alpha >= 1f;
        }
        /// <summary>Canvas 좌표 변환에 쓸 카메라를 고른다 (Overlay 는 카메라 없음)</summary>
        private static Camera CanvasCamera(Canvas _root)
        {
            return _root.renderMode == RenderMode.ScreenSpaceOverlay ? null : _root.worldCamera;
        }
        /// <summary>RectTransform 을 감싸는 화면 좌표 사각형을 구한다</summary>
        private static Rect ScreenCorners(RectTransform _rect, Camera _cam)
        {
            var corners = new Vector3[4];
            _rect.GetWorldCorners(corners);
            var min = new Vector2(float.MaxValue, float.MaxValue);
            var max = new Vector2(float.MinValue, float.MinValue);
            for (int i = 0; i < 4; i++)
            {
                Vector2 p = _cam == null ? (Vector2)corners[i] : RectTransformUtility.WorldToScreenPoint(_cam, corners[i]);
                min = Vector2.Min(min, p);
                max = Vector2.Max(max, p);
            }
            return new Rect(min, max - min);
        }
        /// <summary>_cover 가 _target 을 완전히 감싸는지 판정한다</summary>
        private static bool Contains(Rect _cover, Rect _target)
        {
            const float epsilon = 0.5f;
            return _cover.xMin <= _target.xMin + epsilon && _cover.yMin <= _target.yMin + epsilon
                && _cover.xMax >= _target.xMax - epsilon && _cover.yMax >= _target.yMax - epsilon;
        }
        /// <summary>루트 기준 sibling index 경로를 만든다 (하이어라키 DFS 순서 = UI 그리기 순서)</summary>
        private static System.Collections.Generic.List<int> SiblingPath(Transform _target, Transform _root)
        {
            var path = new System.Collections.Generic.List<int>();
            for (var cur = _target; cur != null && cur != _root; cur = cur.parent)
                path.Insert(0, cur.GetSiblingIndex());
            return path;
        }
        /// <summary>두 sibling 경로의 그리기 순서를 비교한다</summary>
        private static int ComparePath(System.Collections.Generic.List<int> _a, System.Collections.Generic.List<int> _b)
        {
            int n = Mathf.Min(_a.Count, _b.Count);
            for (int i = 0; i < n; i++)
                if (_a[i] != _b[i]) return _a[i].CompareTo(_b[i]);
            return _a.Count.CompareTo(_b.Count);
        }
        /// <summary>현재 상태를 _report 에 담는다. 기본은 열림·가림 여부이며, 팝업별로 override 해서(base 호출 후) 상태를 추가한다</summary>
        public virtual void MCPDetail(MCPReport _report)
        {
            _report.AddRaw("opened", IsOpened ? "true" : "false");
            if (IsOpened)
                _report.AddRaw("visible", MCPIsCovered() ? "false" : "true");
        }
        /// <summary>지금 가능한 조작을 _report 에 "interactionId → 설명"으로 담는다. 기본은 Open·Close 이며, 팝업별로 override 해서(base 호출 후) 조작을 추가한다</summary>
        public virtual void MCPInteraction(MCPReport _report)
        {
            if (IsOpened)
                _report.Add(MCPCloseId, "팝업 닫기");
            else
                _report.Add(MCPOpenId, "팝업 열기");
        }
        /// <summary>_interactionId 조작을 실행하고 결과 JSON 을 반환한다. _value 는 슬라이더 등 값 입력용이다. 기본은 Open·Close 만 처리하며, 팝업별로 override 해서 나머지를 처리하고 미처리분은 base 로 넘긴다</summary>
        public virtual string MCPInteract(string _interactionId, float _value)
        {
            switch (_interactionId)
            {
                case MCPOpenId: Open(); return "{\"success\":true}";
                case MCPCloseId: Close(); return "{\"success\":true}";
                default: return $"{{\"error\":\"지원하지 않는 조작: {MCPReport.Escape(_interactionId)}\"}}";
            }
        }
        /// <summary>이 팝업이 제공하는 치트를 _report 에 나열한다. 기본은 없으며, 팝업별로 override 해서 추가한다</summary>
        public virtual void MCPCheats(MCPReport _report)
        {
        }
        /// <summary>_cheatId 치트를 실행하고 결과 JSON 을 반환한다. MCPCheats 에 나열한 치트를 팝업별로 override 해서 처리한다</summary>
        public virtual string MCPCheatApply(string _cheatId)
        {
            return $"{{\"error\":\"지원하지 않는 치트: {MCPReport.Escape(_cheatId)}\"}}";
        }
#endif
        #endregion
    }
}