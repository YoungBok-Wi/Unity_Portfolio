/* Game 셋업 — SceneChangeManager 아래에 Face 전환 연출을 등록한다 */
using Library;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>Face 씬 전환 프리팹을 전역 SceneChangeManager에 배선한다.</summary>
    public class Setup_Game : ModuleSetupBase
    {
        #region Property
        public override string SetupName => "Game";
        #endregion
        #region Function
        protected override void OnSetupGlobal(GameObject _root)
        {
            var sceneChangeObject = FindOrCreateManager<SceneChangeManager>(_root, "[SceneChangeManager]");
            var manager = sceneChangeObject.GetComponent<SceneChangeManager>();
            var faceObject = InstantiatePrefabChild(sceneChangeObject, "Assets/__Game/Game/Prefab/SceneChangeAni_Face.prefab", "Face");
            if (faceObject == null)
                throw new InvalidOperationException("Face 씬 전환 프리팹을 만들 수 없다");
            SetAssetReference(faceObject.GetComponent<SceneChangeAni_Face>(), "m_Face", "Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png");
            AddObjectReference(manager, "m_SceneChangeAni", "Face (SceneChangeAni_Face)");
        }
        #endregion
    }
}
