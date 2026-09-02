using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Library
{
    /// <summary>
    /// 씬 루트 레벨 오브젝트 구성 셋업의 공용 베이스. [Global]/[Local]과 형제인 [Popup]·[{오브젝트 그룹}] 루트를 다룬다.
    /// 모듈 매니저 셋업은 ModuleSetupBase를 직접 상속하고, 씬 컨셉 기반 팝업·오브젝트 배치 셋업은 본 클래스를 상속한다.
    /// </summary>
    public abstract class ObjectSetupBase : ModuleSetupBase
    {
        #region Value
        /// <summary>이번 실행에서 확보한 씬 루트 이름들. 런너가 셋업 산출 루트를 식별하는 데 쓰며, 실행 시작 시 비운다</summary>
        public static readonly HashSet<string> TouchedSceneRoots = new HashSet<string>();
        #endregion

        #region Function
        /// <summary>Settings 의 "localObject" 에 적힌 오브젝트 이름들을 반환한다. 이 이름으로 오브젝트의 조건부 생성을 판정한다. Settings 나 키가 없으면 빈 목록</summary>
        protected IReadOnlyList<string> GetObjectNames()
        {
            if (Settings == null || !Settings.TryGetValue("localObject", out var raw))
                return new List<string>();
            if (raw is IReadOnlyDictionary<string, object> localObjects)
                return new List<string>(localObjects.Keys);
            return new List<string>();
        }

        /// <summary>씬 루트에서 _name 오브젝트를 찾거나 만든다 ([Global]·[Local] 과 형제). 다룬 이름은 TouchedSceneRoots 에 기록된다</summary>
        protected GameObject FindOrCreateSceneRoot(string _name)
        {
            TouchedSceneRoots.Add(_name);
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == _name)
                    return go;
            }
            var created = new GameObject(_name);
            RegisterCreatedObjectUndo(created, $"Create {_name}");
            EditorUtility.SetDirty(created);
            return created;
        }

        /// <summary>_groupName 의 그룹 루트를 "[{_groupName}]" 이름으로 확보한다 (예: "ObjGroup1" → [ObjGroup1])</summary>
        protected GameObject FindOrCreateObjectGroup(string _groupName)
            => FindOrCreateSceneRoot($"[{_groupName}]");

        /// <summary>_from 의 모든 자식을 _to 로 옮긴다. 구 구조 마이그레이션용이며 한쪽이라도 null 이면 무시한다</summary>
        protected void MoveChildren(Transform _from, Transform _to)
        {
            if (_from == null || _to == null) return;
            for (int i = _from.childCount - 1; i >= 0; i--)
                Undo.SetTransformParent(_from.GetChild(i), _to, $"Move to {_to.name}");
        }
        #endregion
    }
}
