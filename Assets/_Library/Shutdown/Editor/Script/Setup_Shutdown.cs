/* Shutdown 셋업 — [Global] 하위에 [ShutdownManager] 생성, ShutdownUI 프리팹 배치·참조 */
using UnityEngine;
using UnityEditor;

namespace Library
{
    /// <summary>[ShutdownManager]를 [Global] 하위에 생성하고 ShutdownUI 프리팹을 배치하여 참조한다</summary>
    public class Setup_Shutdown : ModuleSetupBase
    {
        #region Property
        public override string SetupName => "ShutdownManager";
        #endregion
        #region Function
        protected override void OnSetupGlobal(GameObject _root)
        {
            var go = FindOrCreateManager<ShutdownManager>(_root, "[ShutdownManager]");
            if (go == null) return;
            var shutdownUI = FindOrCreateChild(go, "ShutdownUI");
            if (shutdownUI == null) return;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Library/_Core/Prefab/ShutdownUI.prefab");
            if (prefab != null && shutdownUI.GetComponent<ShutdownUI>() == null)
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, go.transform);
                instance.name = "ShutdownUI";
                RegisterCreatedObjectUndo(instance, "Create ShutdownUI");
                if (shutdownUI != instance)
                {
                    DestroyImmediate(shutdownUI);
                    shutdownUI = instance;
                }
            }
            var mgr = go.GetComponent<ShutdownManager>();
            var uiComp = shutdownUI.GetComponent<ShutdownUI>();
            if (mgr != null && uiComp != null)
                SetObjectReference(mgr, "m_ShutdownUI", "ShutdownUI (ShutdownUI)");
        }
        #endregion
    }
}
