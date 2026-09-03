/* Object_Background 셋업 — 씬 루트 [Stage] 하위에 프리팹을 배치한다 */
using UnityEngine;

namespace Game
{
    /// <summary>씬 루트 [Stage] 하위에 Object_Background 을 배치한다 (이미 있으면 보존)</summary>
    public class Setup_Object_Object_Background : Library.ObjectSetupBase
    {
        #region Property
        public override string SetupName => "Object_Background";
        #endregion
        #region Function
        protected override void OnSetupLocal(GameObject _root)
        {
            var stage = FindOrCreateObjectGroup("Stage");
            InstantiatePrefabChild(stage, "Assets/__Game/_Core/_Object/Object_Background/Object_Background.prefab", "Object_Background");
        }
        #endregion
    }
}
