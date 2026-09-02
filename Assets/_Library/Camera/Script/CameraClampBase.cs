using System;
using UnityEngine;

namespace Library
{
    /// <summary>카메라의 이동·회전·줌 한계를 정하는 베이스. 뷰 방식(사이드·쿼터·3인칭)마다 파생이 있다</summary>
    public abstract class CameraClampBase : MonoBehaviour
    {
        #region Property
        /// <summary>이 클램프를 쓰는 카메라 매니저. Init 전에는 null</summary>
        public LocalCameraManager ParCamMgr { get; private set; }
        /// <summary>줌을 카메라 거리로 할지 FOV·사이즈로 할지. 이 카메라의 줌 방식을 정하는 단일 소스이며 멀티터치 입력도 이 값을 따른다</summary>
        public abstract EZoomType ZoomClampType { get; }
        #endregion

        #region Event
        /// <summary>클램프를 cameraManager 에 연결한다. 카메라 매니저가 자기 Init 에서 부르며, null 이면 예외</summary>
        public void Init(LocalCameraManager cameraManager)
        {
            if (cameraManager == null)
                throw new ArgumentNullException(nameof(cameraManager));
            ParCamMgr = cameraManager;
        }
        #endregion
        #region Function
        /// <summary>pos·rot·zoom 을 제한 범위 안으로 당긴다. 카메라 매니저가 값을 적용하기 직전에 호출하며, 파생은 자기가 다루지 않는 값은 그대로 둔다</summary>
        public abstract void Clamp(ref Vector3 pos, ref Quaternion rot, ref float zoom);
        #endregion
    }
}
