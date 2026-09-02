/* Camera2D 셋업 — 2D 카메라 프리팹을 [Local] 하위에 [LocalCameraManager] 로 배치 */
using UnityEngine;

namespace Library
{
    /// <summary>2D 카메라 프리팹을 [Local] 하위에 [LocalCameraManager] 로 배치한다 (이미 있으면 보존)</summary>
    public class Setup_Camera2D : ModuleSetupBase
    {
        #region Property
        public override string SetupName => "LocalCameraManager";
        #endregion
        #region Function
        protected override void OnSetupLocal(GameObject _root)
        {
            const string PrefabPath = "Assets/_Library/Camera2D/Prefabs/[LocalCameraManager].prefab";
            InstantiatePrefabChild(_root, PrefabPath, "[LocalCameraManager]");
        }
        #endregion
    }
}
