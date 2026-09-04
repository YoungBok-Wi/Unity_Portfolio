using UnityEngine;

namespace Library
{
    /// <summary>2D 카메라 위치 및 줌 제한 관리</summary>
    public class CameraClamp_2D : CameraClampBase
    {
        #region Inspector
        [SerializeField] private float m_TestDepth = 0;
        [SerializeField] private Vector2 m_RectLeftBot = new Vector2(-10, -10);
        [SerializeField] private Vector2 m_RectRightTop = new Vector2(10, 10);

        [Space(15)]
        [SerializeField] private EZoomType m_ZoomClampType = EZoomType.CameraZoom;
        [SerializeField] private Vector2 m_FOVRange = new Vector2(30.0f, 90.0f);
        [SerializeField] private Vector2 m_OrthographicSizeRange = new Vector2(3.0f, 10.0f);
        [SerializeField] private Vector2 m_ZoomDistanceRange = new Vector2(10.0f, 100.0f);
        #endregion
        #region Property
        /// <summary>화면 크기를 재는 기준 평면의 깊이</summary>
        public float TestDepth => m_TestDepth;
        /// <summary>카메라가 벗어날 수 없는 사각형의 좌하단</summary>
        public Vector2 RectLeftBot => m_RectLeftBot;
        /// <summary>카메라가 벗어날 수 없는 사각형의 우상단</summary>
        public Vector2 RectRightTop => m_RectRightTop;
        /// <summary>줌을 카메라 거리로 할지 FOV 로 할지. 카메라 조작측이 이 값을 단일 소스로 따른다</summary>
        public override EZoomType ZoomClampType => m_ZoomClampType;
        #endregion
        #region Local Function
        /// <summary>카메라 위치를 제한 사각형 안으로 당긴다</summary>
        // 화면 절반 크기만큼 안쪽으로 좁혀 클램프한다 — 그래야 화면 가장자리가 사각형 밖을 비추지 않는다
        private void ClampPos(ref Vector3 pos)
        {
            Ray screenRay = ParCamMgr.GetScreenRay(new Vector2(Screen.width, Screen.height));
            Plane testPlane = GetTestPlane(m_TestDepth);
            Vector2? halfSize = ParCamMgr.GetLocalScreenPos_Plane(screenRay, testPlane);
            if (halfSize.HasValue)
            {
                pos.x = Mathf.Clamp(pos.x, m_RectLeftBot.x + halfSize.Value.x, m_RectRightTop.x - halfSize.Value.x);
                pos.y = Mathf.Clamp(pos.y, m_RectLeftBot.y + halfSize.Value.y, m_RectRightTop.y - halfSize.Value.y);
            }
        }
        /// <summary>줌 타입에 맞춰 거리 또는 FOV·사이즈를 범위 안으로 당긴다</summary>
        // 카메라 로컬 z 는 음수(뒤)라 거리 범위의 부호를 뒤집어 건다
        private void ClampZoom(ref Vector3 pos, ref float zoom)
        {
            if (m_ZoomClampType == EZoomType.CameraZoom)
            {
                if (ParCamMgr.IsOrthographic)
                    zoom = Mathf.Clamp(zoom, m_OrthographicSizeRange.x, m_OrthographicSizeRange.y);
                else
                    zoom = Mathf.Clamp(zoom, m_FOVRange.x, m_FOVRange.y);
            }
            else if (m_ZoomClampType == EZoomType.PositionZoom)
            {
                pos.z = Mathf.Clamp(pos.z, -m_ZoomDistanceRange.y, -m_ZoomDistanceRange.x);
            }
        }
        #endregion
        #region Function
        /// <summary>pos·zoom 을 제한 범위 안으로 당긴다. 카메라 매니저가 값을 적용하기 직전에 호출한다 (rot 는 건드리지 않는다)</summary>
        public override void Clamp(ref Vector3 pos, ref Quaternion rot, ref float zoom)
        {
            ClampPos(ref pos);
            ClampZoom(ref pos, ref zoom);
        }

        /// <summary>깊이 testDepth 에 놓인, 카메라를 마주 보는 평면을 만든다</summary>
        public static Plane GetTestPlane(float testDepth)
        {
            return new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, testDepth));
        }

        /// <summary>화면 크기를 재는 기준 깊이를 value 로 바꾼다</summary>
        public void SetTestDepth(float value) => m_TestDepth = value;

        /// <summary>카메라가 벗어날 수 없는 사각형을 leftBot~rightTop 으로 바꾼다</summary>
        public void SetClampRect(Vector2 leftBot, Vector2 rightTop)
        {
            m_RectLeftBot = leftBot;
            m_RectRightTop = rightTop;
        }

        /// <summary>줌 방식을 value 로 바꾼다</summary>
        public void SetZoomClampType(EZoomType value) => m_ZoomClampType = value;

        /// <summary>줌 범위를 min~max 로 바꾼다. 현재 카메라 투영 방식에 해당하는 범위만 바뀐다</summary>
        public void SetZoomRange(float min, float max)
        {
            if (ParCamMgr.IsOrthographic)
                m_OrthographicSizeRange = new Vector2(min, max);
            else
                m_FOVRange = new Vector2(min, max);
        }

        /// <summary>줌 거리 범위를 min~max 로 바꾼다 (PositionZoom 일 때 쓰인다)</summary>
        public void SetZoomDistRange(float min, float max) => m_ZoomDistanceRange = new Vector2(min, max);
        #endregion
    }
}
