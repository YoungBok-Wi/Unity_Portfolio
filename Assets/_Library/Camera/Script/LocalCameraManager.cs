using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Library
{
    /// <summary>카메라 관리 매니저, 위치/회전/줌 애니메이션 및 Clamp 지원</summary>
    public class LocalCameraManager : LocalManagerBase
    {
        public static LocalCameraManager instance { get; private set; }

        #region Inspector
        [SerializeField, TabGroup("Component"), LabelText("카메라")] private Camera m_Camera;
        [SerializeField, TabGroup("Component"), LabelText("카메라 회전 기준")] private Transform m_CameraRotRoot;
        [SerializeField, TabGroup("Component"), LabelText("카메라 위치 기준")] private Transform m_CameraPosRoot;
        [SerializeField, TabGroup("Component"), LabelText("카메라 제한")] private CameraClampBase m_CameraClamp;
        [SerializeField, TabGroup("Animation"), LabelText("이동 커브")] private AnimationCurve m_PosCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, TabGroup("Animation"), LabelText("이동 속도")] private float m_PosSpeed = 3.0f;
        [SerializeField, TabGroup("Animation"), LabelText("회전 커브")] private AnimationCurve m_RotCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, TabGroup("Animation"), LabelText("회전 속도")] private float m_RotSpeed = 3.0f;
        [SerializeField, TabGroup("Animation"), LabelText("줌 커브")] private AnimationCurve m_ZoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, TabGroup("Animation"), LabelText("줌 속도")] private float m_ZoomSpeed = 3.0f;
        [SerializeField, TabGroup("Animation"), LabelText("매니저 이동 커브")] private AnimationCurve m_ManagerPosCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, TabGroup("Animation"), LabelText("매니저 이동 속도")] private float m_ManagerPosSpeed = 3.0f;
        #endregion
        #region Property
        /// <summary>현재 카메라</summary>
        public Camera CurCam => m_Camera;
        /// <summary>회전 기준 Transform</summary>
        public Transform RotRoot => m_CameraRotRoot;
        /// <summary>위치 기준 Transform</summary>
        public Transform PosRoot => m_CameraPosRoot;
        /// <summary>현재 카메라 제한 컴포넌트</summary>
        public CameraClampBase CurClamp => m_CameraClamp;
        /// <summary>Orthographic 카메라 여부</summary>
        public bool IsOrthographic => m_Camera.orthographic;

        /// <summary>목표 위치</summary>
        public Vector3 TargetPos => m_TargetPos ?? NowPos;
        /// <summary>목표 회전</summary>
        public Quaternion TargetRot => m_TargetRot ?? NowRot;
        /// <summary>목표 줌</summary>
        public float TargetZoom => m_TargetZoom ?? CurrentZoom;
        /// <summary>매니저 자체 목표 위치</summary>
        public Vector3 TargetManagerPos => m_TargetManagerPos ?? NowManagerPos;

        /// <summary>매니저 자체 현재 위치 (월드)</summary>
        public Vector3 NowManagerPos => transform.position;
        /// <summary>현재 위치</summary>
        public Vector3 NowPos => m_CameraPosRoot.localPosition;
        /// <summary>현재 회전</summary>
        public Quaternion NowRot => m_CameraRotRoot.localRotation;
        /// <summary>현재 줌</summary>
        public float CurrentZoom => m_Camera.orthographic ? m_Camera.orthographicSize : m_Camera.fieldOfView;
        /// <summary>흔들림이 남아 있는지</summary>
        public bool IsShaking => 0.0f < m_ShakeTimer;
        #endregion
        #region Value
        private Vector3? m_TargetPos;
        private Vector3 m_StartPos;
        private float m_PosTimer;
        private Quaternion? m_TargetRot;
        private Quaternion m_StartRot;
        private float m_RotTimer;
        private float? m_TargetZoom;
        private float m_StartZoom;
        private float m_ZoomTimer;
        private Vector3? m_TargetManagerPos;
        private Vector3 m_StartManagerPos;
        private float m_ManagerPosTimer;
        private Transform m_FollowTarget;
        private float m_FollowLerpSpeed;
        private float m_FollowClampX;
        private float m_FollowFixedY;
        // 탑다운 추종의 Y 좌우 한계. m_FollowY 가 true 일 때만 쓰인다
        private float m_FollowClampY;
        // true 면 Y 도 대상을 따라간다 (탑다운), false 면 m_FollowFixedY 로 고정한다 (사이드뷰)
        private bool m_FollowY;
        private float m_ShakeAmplitude;
        private float m_ShakeDuration;
        private float m_ShakeTimer;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        /// <summary>씬 전환 파괴 시 정적 참조를 해제한다 — 이 매니저가 없는 씬에서 파괴된 인스턴스 접근을 막는다</summary>
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        public override void Init()
        {
            m_CameraClamp?.Init(this);
            base.Init();
        }
        private void Update()
        {
            if (m_TargetPos.HasValue)
            {
                m_PosTimer += Time.deltaTime * m_PosSpeed;
                float t = m_PosCurve.Evaluate(Mathf.Clamp01(m_PosTimer));
                SetPosInternal(Vector3.LerpUnclamped(m_StartPos, m_TargetPos.Value, t));
                if (m_PosTimer >= 1.0f)
                {
                    SetPosInternal(m_TargetPos.Value);
                    m_TargetPos = null;
                }
            }

            if (m_TargetRot.HasValue)
            {
                m_RotTimer += Time.deltaTime * m_RotSpeed;
                float t = m_RotCurve.Evaluate(Mathf.Clamp01(m_RotTimer));
                SetRotInternal(Quaternion.LerpUnclamped(m_StartRot, m_TargetRot.Value, t));
                if (m_RotTimer >= 1.0f)
                {
                    SetRotInternal(m_TargetRot.Value);
                    m_TargetRot = null;
                }
            }

            if (m_TargetZoom.HasValue)
            {
                m_ZoomTimer += Time.deltaTime * m_ZoomSpeed;
                float t = m_ZoomCurve.Evaluate(Mathf.Clamp01(m_ZoomTimer));
                SetZoomInternal(Mathf.LerpUnclamped(m_StartZoom, m_TargetZoom.Value, t));
                if (m_ZoomTimer >= 1.0f)
                {
                    SetZoomInternal(m_TargetZoom.Value);
                    m_TargetZoom = null;
                }
            }

            if (m_TargetManagerPos.HasValue)
            {
                m_ManagerPosTimer += Time.deltaTime * m_ManagerPosSpeed;
                float t = m_ManagerPosCurve.Evaluate(Mathf.Clamp01(m_ManagerPosTimer));
                SetManagerPosInternal(Vector3.LerpUnclamped(m_StartManagerPos, m_TargetManagerPos.Value, t));
                if (m_ManagerPosTimer >= 1.0f)
                {
                    SetManagerPosInternal(m_TargetManagerPos.Value);
                    m_TargetManagerPos = null;
                }
            }
        }
        // 추종 대상이 붙어 있는 동안에만 매니저 위치를 잡는다 (미설정이면 기존 동작 그대로 아무것도 하지 않는다)
        private void LateUpdate()
        {
            if (m_FollowTarget == null) return;

            Vector3 now = NowManagerPos;
            float rate = 1.0f - Mathf.Exp(-m_FollowLerpSpeed * Time.unscaledDeltaTime);
            float targetX = Mathf.Clamp(m_FollowTarget.position.x, -m_FollowClampX, m_FollowClampX);
            float lerpedX = Mathf.Lerp(now.x, targetX, rate);
            float lerpedY = m_FollowFixedY;
            if (m_FollowY)
            {
                float targetY = Mathf.Clamp(m_FollowTarget.position.y, -m_FollowClampY, m_FollowClampY);
                lerpedY = Mathf.Lerp(now.y, targetY, rate);
            }

            float shakeX = 0.0f;
            float shakeY = 0.0f;
            if (0.0f < m_ShakeTimer)
            {
                m_ShakeTimer = Mathf.Max(0.0f, m_ShakeTimer - Time.unscaledDeltaTime);
                float power = m_ShakeAmplitude * (m_ShakeTimer / Mathf.Max(0.01f, m_ShakeDuration));
                shakeX = Random.Range(-power, power);
                shakeY = Random.Range(-power, power);
            }
            SetManagerPos(new Vector3(lerpedX + shakeX, lerpedY + shakeY, now.z));
        }
        #endregion
        #region Local Function
        /// <summary>클램프 없이 매니저 위치를 적용한다</summary>
        private void SetManagerPosInternal(Vector3 pos) => transform.position = pos;
        /// <summary>클램프 없이 카메라 위치를 적용한다</summary>
        private void SetPosInternal(Vector3 pos) => m_CameraPosRoot.localPosition = pos;
        /// <summary>클램프 없이 카메라 회전을 적용한다</summary>
        private void SetRotInternal(Quaternion rot) => m_CameraRotRoot.localRotation = rot;
        /// <summary>투영 방식에 맞는 줌 값을 적용한다</summary>
        private void SetZoomInternal(float zoom)
        {
            if (m_Camera.orthographic)
                m_Camera.orthographicSize = zoom;
            else
                m_Camera.fieldOfView = zoom;
        }
        #endregion
        #region Function
        /// <summary>_screenPos 를 지나는 카메라 레이를 반환한다</summary>
        public Ray GetScreenRay(Vector2 _screenPos)
        {
            return m_Camera.ScreenPointToRay(new Vector3(_screenPos.x, _screenPos.y, 0));
        }
        /// <summary>ray 와 plane 의 교차점을 월드 좌표로 반환한다. 만나지 않으면 null</summary>
        public Vector3? GetWorldScreenPos_Plane(Ray ray, Plane plane)
        {
            if (plane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);
            return null;
        }
        /// <summary>ray 와 plane 의 교차점을 카메라 로컬 좌표로 반환한다. 만나지 않으면 null</summary>
        public Vector3? GetLocalScreenPos_Plane(Ray ray, Plane plane)
        {
            Vector3? worldScreenPos = GetWorldScreenPos_Plane(ray, plane);
            if (worldScreenPos.HasValue)
                return m_Camera.transform.InverseTransformPoint(worldScreenPos.Value);
            return null;
        }
        /// <summary>카메라 위치를 pos 로 즉시 옮긴다. 진행 중인 이동 애니메이션은 취소되고 제한 범위로 클램프된다</summary>
        public void SetPos(Vector3 pos)
        {
            m_TargetPos = null;
            SetPosInternal(pos);
            ClampCurrent();
        }
        /// <summary>카메라 회전을 rot 으로 즉시 바꾼다. 진행 중인 회전 애니메이션은 취소되고 제한 범위로 클램프된다</summary>
        public void SetRot(Quaternion rot)
        {
            m_TargetRot = null;
            SetRotInternal(rot);
            ClampCurrent();
        }
        /// <summary>줌을 zoom 으로 즉시 바꾼다. zoom 은 Orthographic 이면 orthographicSize, 아니면 fieldOfView 로 해석된다</summary>
        public void SetZoom(float zoom)
        {
            m_TargetZoom = null;
            SetZoomInternal(zoom);
            ClampCurrent();
        }
        /// <summary>매니저 자체를 월드 좌표 pos 로 즉시 옮긴다 (클램프 대상 아님)</summary>
        public void SetManagerPos(Vector3 pos)
        {
            m_TargetManagerPos = null;
            SetManagerPosInternal(pos);
        }
        /// <summary>매니저 자체를 월드 좌표 pos 까지 커브 보간으로 옮긴다</summary>
        public void ChangeManagerPos(Vector3 pos)
        {
            m_TargetManagerPos = pos;
            m_StartManagerPos = NowManagerPos;
            m_ManagerPosTimer = 0.0f;
        }
        /// <summary>카메라를 pos 까지 커브 보간으로 옮긴다</summary>
        public void ChangePos(Vector3 pos)
        {
            m_TargetPos = pos;
            m_StartPos = NowPos;
            m_PosTimer = 0.0f;
            ClampTarget();
        }
        /// <summary>카메라를 rot 까지 커브 보간으로 회전시킨다</summary>
        public void ChangeRot(Quaternion rot)
        {
            m_TargetRot = rot;
            m_StartRot = NowRot;
            m_RotTimer = 0.0f;
            ClampTarget();
        }
        /// <summary>줌을 zoom 까지 커브 보간으로 바꾼다</summary>
        public void ChangeZoom(float zoom)
        {
            m_TargetZoom = zoom;
            m_StartZoom = CurrentZoom;
            m_ZoomTimer = 0.0f;
            ClampTarget();
        }
        /// <summary>사이드뷰용 추종 — 매니저가 매 프레임 _target 의 X 만 따라가고 Y 는 _fixedY 로 고정한다. _target 이 null 이면 추종을 멈춘다. _lerpSpeed 는 따라붙는 속도, _clampX 는 대상 X 의 좌우 한계다 (실시간 기준이라 timeScale 정지 중에도 움직인다)</summary>
        public void SetFollow(Transform _target, float _lerpSpeed, float _clampX, float _fixedY)
        {
            m_FollowTarget = _target;
            m_FollowLerpSpeed = _lerpSpeed;
            m_FollowClampX = _clampX;
            m_FollowFixedY = _fixedY;
            m_FollowY = false;
        }
        /// <summary>탑다운용 추종 — 매니저가 매 프레임 _target 의 X·Y 를 함께 따라간다. _target 이 null 이면 추종을 멈춘다. _lerpSpeed 는 따라붙는 속도, _clampX·_clampY 는 대상 X·Y 의 한계다 (실시간 기준이라 timeScale 정지 중에도 움직인다)</summary>
        public void SetFollowXY(Transform _target, float _lerpSpeed, float _clampX, float _clampY)
        {
            m_FollowTarget = _target;
            m_FollowLerpSpeed = _lerpSpeed;
            m_FollowClampX = _clampX;
            m_FollowClampY = _clampY;
            m_FollowY = true;
        }
        /// <summary>흔들림을 시작한다. _amplitude 는 시작 진폭이고 _duration 초에 걸쳐 0 으로 줄며, 앞선 흔들림을 대체한다. 추종 중일 때만 실제로 보인다</summary>
        public void Shake(float _amplitude, float _duration)
        {
            m_ShakeAmplitude = _amplitude;
            m_ShakeDuration = _duration;
            m_ShakeTimer = _duration;
        }
        /// <summary>남은 흔들림을 즉시 없앤다</summary>
        public void StopShake()
        {
            m_ShakeTimer = 0.0f;
        }
        /// <summary>애니메이션의 목표 위치·회전·줌을 제한 범위 안으로 당긴다 (진행 중이 아닌 축은 현재 값에 바로 적용)</summary>
        public void ClampTarget()
        {
            Vector3 pos = TargetPos;
            Quaternion rot = TargetRot;
            float zoom = TargetZoom;

            m_CameraClamp?.Clamp(ref pos, ref rot, ref zoom);

            if (m_TargetPos.HasValue)
                m_TargetPos = pos;
            else
                SetPosInternal(pos);

            if (m_TargetRot.HasValue)
                m_TargetRot = rot;
            else
                SetRotInternal(rot);

            if (m_TargetZoom.HasValue)
                m_TargetZoom = zoom;
            else
                SetZoomInternal(zoom);
        }
        /// <summary>현재 위치·회전·줌을 제한 범위 안으로 당긴다</summary>
        public void ClampCurrent()
        {
            Vector3 pos = NowPos;
            Quaternion rot = NowRot;
            float zoom = CurrentZoom;

            m_CameraClamp?.Clamp(ref pos, ref rot, ref zoom);

            SetPosInternal(pos);
            SetRotInternal(rot);
            SetZoomInternal(zoom);
        }
        #endregion
    }
}
