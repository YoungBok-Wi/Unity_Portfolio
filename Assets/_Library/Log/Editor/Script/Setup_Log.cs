/* Log 셋업 — [Global] 하위에 [LogManager] 생성, 디버그 콘솔을 비활성으로 배치 */
using UnityEngine;

namespace Library
{
    /// <summary>[LogManager] 를 [Global] 하위에 만들고 디버그 콘솔을 비활성으로 배치해 물린다. 콘솔을 켜는 건 런타임(LogManager.InitFirst) 몫이다</summary>
    public class Setup_Log : ModuleSetupBase
    {
        #region Property
        public override string SetupName => "LogManager";
        #endregion
        #region Function
        protected override void OnSetupGlobal(GameObject _root)
        {
            var go = FindOrCreateManager<LogManager>(_root, "[LogManager]");
            if (go == null) return;

            var console = InstantiatePrefabChild(go, "Assets/Plugins/IngameDebugConsole/IngameDebugConsole.prefab", "DebugConsole");
            if (console == null) return;
            console.SetActive(false);

            var mgr = go.GetComponent<LogManager>();
            SetObjectReference(mgr, "m_DebugConsole", "DebugConsole (GameObject)");
        }
        #endregion
    }
}
