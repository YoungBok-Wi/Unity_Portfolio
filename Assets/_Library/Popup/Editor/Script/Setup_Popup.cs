/* Popup 셋업 — [LocalPopupManager]+UICamera는 프리팹(localManagerPrefab)으로 인스턴스화되고, 프리팹 불가 작업(Main Camera Overlay 스택에 UICamera 등록 — 씬 참조)만 남긴다 */
using UnityEngine;

namespace Library
{
    /// <summary>UICamera 를 씬 카메라들의 URP Overlay 스택에 얹는다</summary>
    public class Setup_Popup : ModuleSetupBase
    {
        #region Property
        public override string SetupName => "LocalPopupManager";
        /// <summary>Main Camera 가 먼저 있어야 스택에 얹을 수 있어 Camera 셋업 뒤에 실행한다</summary>
        public override string[] RequireSetups => new[] { "LocalCameraManager" };
        #endregion
        #region Function
        protected override void OnSetupLocal(GameObject _root)
        {
            var mainCameraGo = GameObject.Find("Main Camera");
            if (mainCameraGo == null) return;

            var mainUrpData = GetOrAddComponentByName(mainCameraGo, "UniversalAdditionalCameraData");
            if (mainUrpData != null)
                AddObjectReference(mainUrpData, "m_Cameras", "UICamera (Camera)");

            // 인트로 카메라에도 얹는다 — 안 그러면 인트로 동안 UI 가 가려진다 (인트로가 없는 씬이면 건너뛴다)
            var introCameraGo = GameObject.Find("IntroCamera");
            if (introCameraGo != null)
            {
                var introUrpData = GetOrAddComponentByName(introCameraGo, "UniversalAdditionalCameraData");
                if (introUrpData != null)
                    AddObjectReference(introUrpData, "m_Cameras", "UICamera (Camera)");
            }
        }
        #endregion
    }
}
