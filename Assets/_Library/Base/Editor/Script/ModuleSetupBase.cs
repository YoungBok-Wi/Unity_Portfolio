using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Library
{
    #region Type
    /// <summary>셋업 실행 컨텍스트. Global=[Global] 프리팹, Local=[Local] 씬 오브젝트, Etc=기타 프리팹 스테이지</summary>
    public enum ESetupContext { Global, Local, Etc }
    #endregion

    /// <summary>
    /// 모듈 셋업의 단일 베이스 추상 클래스. 구현체는 필요한 컨텍스트의 OnSetupXxx 함수만 오버라이드한다.
    /// 각 구현체는 주입된 Settings(파싱된 기획서 JSON)를 읽어 세팅한다. 한 구현체가 여러 컨텍스트(Global+Local 등)를 동시에 오버라이드할 수 있다.
    /// </summary>
    public abstract class ModuleSetupBase
    {
        #region Property
        public abstract string SetupName { get; }
        /// <summary>선행 실행이 필요한 SetupName 목록. 런너가 이 순서를 보장한다. 기본 없음(동시 실행 안전). 극히 일부 의존 관계에서만 사용한다</summary>
        public virtual string[] RequireSetups => null;
        /// <summary>true이면 프리팹 편집 모드. Undo를 쓰지 않고 오브젝트 검색을 루트 계층으로 제한한다. Run 호출 시 컨텍스트에 따라 자동 설정된다 (Global/Etc만 true)</summary>
        protected bool PrefabMode { get; private set; } = true;
        /// <summary>현재 셋업 실행에 주입된 concept parse 결과 JSON. 없을 수 있다(하드코딩 기본값 사용)</summary>
        protected IReadOnlyDictionary<string, object> Settings { get; private set; }
        #endregion

        #region Event
        /// <summary>[Global] 프리팹 루트 대상 셋업. 필요한 구현체만 오버라이드한다</summary>
        protected virtual void OnSetupGlobal(GameObject _root) { }
        /// <summary>[Local] 씬 오브젝트 대상 셋업. 필요한 구현체만 오버라이드한다</summary>
        protected virtual void OnSetupLocal(GameObject _root) { }
        /// <summary>기타 프리팹 스테이지 루트 대상 셋업. 필요한 구현체만 오버라이드한다</summary>
        protected virtual void OnSetupEtc(GameObject _root) { }
        #endregion

        #region Function
        /// <summary>_context 의 셋업 함수가 구현체에서 오버라이드돼 있는지 반환한다</summary>
        public bool Has(ESetupContext _context)
        {
            var method = GetType().GetMethod(MethodName(_context), BindingFlags.Instance | BindingFlags.NonPublic);
            return method != null && method.DeclaringType != typeof(ModuleSetupBase);
        }

        /// <summary>_context 의 셋업 함수를 _root 에 실행한다. _settings 는 구현체가 읽을 기획서 JSON 이며 생략 가능하다. Global·Etc 는 프리팹 모드, Local 은 씬 모드로 동작한다</summary>
        public void Run(ESetupContext _context, GameObject _root, IReadOnlyDictionary<string, object> _settings = null)
        {
            Settings = _settings;
            PrefabMode = _context == ESetupContext.Global || _context == ESetupContext.Etc;
            switch (_context)
            {
                case ESetupContext.Global: OnSetupGlobal(_root); break;
                case ESetupContext.Local: OnSetupLocal(_root); break;
                case ESetupContext.Etc: OnSetupEtc(_root); break;
            }
        }

        /// <summary>Settings 에서 문자열 값을 조회한다. _path 는 점 경로("overview.camera")이며, 값이 없으면 _fallback 을 반환한다</summary>
        protected string GetSetting(string _path, string _fallback = null)
        {
            if (Settings == null || string.IsNullOrEmpty(_path)) return _fallback;
            object cur = Settings;
            foreach (var seg in _path.Split('.'))
            {
                if (cur is IReadOnlyDictionary<string, object> dict && dict.TryGetValue(seg, out var next))
                    cur = next;
                else
                    return _fallback;
            }
            return cur?.ToString() ?? _fallback;
        }

        /// <summary>_root 하위에서 _managerName 오브젝트를 찾거나 만들고 컴포넌트 T 를 붙인다. 이미 있으면 그대로 쓴다(재셋업 안전)</summary>
        protected GameObject FindOrCreateManager<T>(GameObject _root, string _managerName) where T : Component
        {
            if (_root == null)
            {
                Debug.LogError("[ModuleSetup] 루트 오브젝트가 null이다");
                return null;
            }
            var go = FindChildByName(_root.transform, _managerName);
            if (go == null)
            {
                go = new GameObject(_managerName);
                go.transform.SetParent(_root.transform, false);
                if (!PrefabMode)
                    Undo.RegisterCreatedObjectUndo(go, $"Create {_managerName}");
            }
            if (go.GetComponent<T>() == null)
                AddComponent<T>(go);
            EditorUtility.SetDirty(go);
            return go;
        }
        /// <summary>_prefabPath 의 프리팹을 _root 하위에 _name 으로 배치한다. 같은 이름의 자식이 이미 있으면 보존해 그대로 반환한다</summary>
        protected GameObject InstantiatePrefabChild(GameObject _root, string _prefabPath, string _name)
        {
            if (_root == null)
            {
                Debug.LogError("[ModuleSetup] 루트 오브젝트가 null이다");
                return null;
            }
            var existing = FindChildByName(_root.transform, _name);
            if (existing != null) return existing;

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"[ModuleSetup] 프리팹을 찾을 수 없다: {_prefabPath}");
                return null;
            }
            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, _root.transform);
            go.name = _name;
            if (!PrefabMode)
                Undo.RegisterCreatedObjectUndo(go, $"Instantiate {_name}");
            EditorUtility.SetDirty(go);
            return go;
        }

        /// <summary>_parent 하위에서 _childName 자식을 찾거나 만든다</summary>
        protected GameObject FindOrCreateChild(GameObject _parent, string _childName)
        {
            if (_parent == null)
            {
                Debug.LogError("[ModuleSetup] 부모 오브젝트가 null이다");
                return null;
            }
            var child = FindChildByName(_parent.transform, _childName);
            if (child == null)
            {
                child = new GameObject(_childName);
                child.transform.SetParent(_parent.transform, false);
                if (!PrefabMode)
                    Undo.RegisterCreatedObjectUndo(child, $"Create {_childName}");
            }
            EditorUtility.SetDirty(child);
            return child;
        }
        /// <summary>_typeName 컴포넌트를 _go 에서 찾거나 붙인다. URP 처럼 직접 참조할 수 없는 타입용</summary>
        protected Component GetOrAddComponentByName(GameObject _go, string _typeName)
        {
            if (_go == null) return null;
            foreach (var c in _go.GetComponents<Component>())
            {
                if (c != null && c.GetType().Name == _typeName)
                    return c;
            }
            var type = FindType(_typeName);
            if (type == null)
            {
                Debug.LogError($"[ModuleSetup] 타입을 찾을 수 없다: {_typeName}");
                return null;
            }
            if (PrefabMode)
                return _go.AddComponent(type);
            return Undo.AddComponent(_go, type);
        }

        /// <summary>PrefabMode를 고려하여 컴포넌트를 추가한다</summary>
        protected T AddComponent<T>(GameObject _go) where T : Component
        {
            if (PrefabMode)
                return _go.AddComponent<T>();
            return Undo.AddComponent<T>(_go);
        }
        /// <summary>PrefabMode를 고려하여 오브젝트를 즉시 삭제한다</summary>
        protected void DestroyImmediate(UnityEngine.Object _obj)
        {
            if (PrefabMode)
                UnityEngine.Object.DestroyImmediate(_obj);
            else
                Undo.DestroyObjectImmediate(_obj);
        }
        /// <summary>PrefabMode를 고려하여 Undo에 오브젝트 생성을 등록한다</summary>
        protected void RegisterCreatedObjectUndo(UnityEngine.Object _obj, string _name)
        {
            if (!PrefabMode)
                Undo.RegisterCreatedObjectUndo(_obj, _name);
        }

        /// <summary>_component 의 _propertyName 에 _value 를 설정한다. _value 는 프로퍼티 타입에 맞게 자동 변환된다</summary>
        protected static void SetProperty(Component _component, string _propertyName, string _value)
        {
            if (_component == null)
            {
                Debug.LogError("[ModuleSetup] 컴포넌트가 null이다");
                return;
            }
            var so = new SerializedObject(_component);
            var prop = so.FindProperty(_propertyName);
            if (prop == null)
            {
                Debug.LogError($"[ModuleSetup] 프로퍼티를 찾을 수 없다: {_component.GetType().Name}.{_propertyName}");
                return;
            }
            ApplyStringValue(prop, _value);
            so.ApplyModifiedProperties();
        }

        /// <summary>_component 의 _propertyName 에 _assetPath 의 에셋을 물린다. _assetPath 가 비면 null 로 설정한다</summary>
        protected static void SetAssetReference(Component _component, string _propertyName, string _assetPath)
        {
            if (_component == null)
            {
                Debug.LogError("[ModuleSetup] 컴포넌트가 null이다");
                return;
            }
            var so = new SerializedObject(_component);
            var prop = so.FindProperty(_propertyName);
            if (prop == null)
            {
                Debug.LogError($"[ModuleSetup] 프로퍼티를 찾을 수 없다: {_component.GetType().Name}.{_propertyName}");
                return;
            }
            if (prop.propertyType != SerializedPropertyType.ObjectReference)
            {
                Debug.LogError($"[ModuleSetup] ObjectReference 타입이 아니다: {_component.GetType().Name}.{_propertyName} ({prop.propertyType})");
                return;
            }
            if (string.IsNullOrEmpty(_assetPath))
            {
                prop.objectReferenceValue = null;
                so.ApplyModifiedProperties();
                return;
            }
            // 프로퍼티 기대 타입으로 로드 시도 (Sprite 등 서브에셋 대응)
            UnityEngine.Object asset = null;
            var expectedType = GetExpectedObjectType(prop);
            if (expectedType != null)
                asset = AssetDatabase.LoadAssetAtPath(_assetPath, expectedType);
            if (asset == null)
                asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(_assetPath);
            if (asset == null)
            {
                Debug.LogError($"[ModuleSetup] 에셋을 찾을 수 없다: {_assetPath}");
                return;
            }
            prop.objectReferenceValue = asset;
            so.ApplyModifiedProperties();
        }
        /// <summary>_component 의 _propertyName 에 씬·프리팹 오브젝트를 물린다. _targetRef 는 "ObjectName (ComponentType)" 형식이며, "null"·빈 값이면 참조를 해제한다</summary>
        protected void SetObjectReference(Component _component, string _propertyName, string _targetRef)
        {
            if (_component == null)
            {
                Debug.LogError("[ModuleSetup] 컴포넌트가 null이다");
                return;
            }
            var so = new SerializedObject(_component);
            var prop = so.FindProperty(_propertyName);
            if (prop == null)
            {
                Debug.LogError($"[ModuleSetup] 프로퍼티를 찾을 수 없다: {_component.GetType().Name}.{_propertyName}");
                return;
            }
            if (prop.propertyType != SerializedPropertyType.ObjectReference)
            {
                Debug.LogError($"[ModuleSetup] ObjectReference 타입이 아니다: {_component.GetType().Name}.{_propertyName} ({prop.propertyType})");
                return;
            }
            if (string.IsNullOrEmpty(_targetRef) || _targetRef == "null")
            {
                prop.objectReferenceValue = null;
                so.ApplyModifiedProperties();
                return;
            }
            var target = ResolveObjectRef(_component, _targetRef);
            if (target == null)
            {
                Debug.LogError($"[ModuleSetup] 대상을 찾을 수 없다: {_targetRef}");
                return;
            }
            prop.objectReferenceValue = target;
            so.ApplyModifiedProperties();
        }

        /// <summary>_component 의 배열 프로퍼티 _propertyName 에 _targetRef 를 중복 없이 추가한다. URP 카메라 스택(m_Cameras) 같은 List 참조용</summary>
        protected void AddObjectReference(Component _component, string _propertyName, string _targetRef)
        {
            if (_component == null)
            {
                Debug.LogError("[ModuleSetup] 컴포넌트가 null이다");
                return;
            }
            var so = new SerializedObject(_component);
            var prop = so.FindProperty(_propertyName);
            if (prop == null)
            {
                Debug.LogError($"[ModuleSetup] 프로퍼티를 찾을 수 없다: {_component.GetType().Name}.{_propertyName}");
                return;
            }
            if (!prop.isArray)
            {
                Debug.LogError($"[ModuleSetup] 배열 프로퍼티가 아니다: {_component.GetType().Name}.{_propertyName}");
                return;
            }
            var target = ResolveObjectRef(_component, _targetRef);
            if (target == null)
            {
                Debug.LogError($"[ModuleSetup] 대상을 찾을 수 없다: {_targetRef}");
                return;
            }
            // 기존 null(dangling)·중복을 정리하고 target을 추가하여 배열을 재구성한다 (재셋업 idempotent)
            var elements = new List<UnityEngine.Object>();
            for (int i = 0; i < prop.arraySize; i++)
            {
                var e = prop.GetArrayElementAtIndex(i).objectReferenceValue;
                if (e != null && !elements.Contains(e)) elements.Add(e);
            }
            if (!elements.Contains(target)) elements.Add(target);
            prop.arraySize = elements.Count;
            for (int i = 0; i < elements.Count; i++)
                prop.GetArrayElementAtIndex(i).objectReferenceValue = elements[i];
            so.ApplyModifiedProperties();
        }

        /// <summary>"ObjectName (ComponentType)" 형식 참조를 실제 오브젝트·컴포넌트로 해석한다</summary>
        private UnityEngine.Object ResolveObjectRef(Component _component, string _targetRef)
        {
            var parenIdx = _targetRef.LastIndexOf(" (");
            if (parenIdx < 0)
            {
                Debug.LogError($"[ModuleSetup] 참조 형식이 올바르지 않다: {_targetRef} (\"ObjectName (ComponentType)\" 형식 필요)");
                return null;
            }
            var objName = _targetRef.Substring(0, parenIdx);
            var typeName = _targetRef.Substring(parenIdx + 2).TrimEnd(')');
            IEnumerable<GameObject> objects;
            if (PrefabMode)
            {
                var list = new List<GameObject>();
                CollectAllChildren(_component.transform.root, list);
                objects = list;
            }
            else
            {
                objects = UnityEngine.Object.FindObjectsByType<GameObject>(
                    FindObjectsInactive.Include, FindObjectsSortMode.None);
            }
            foreach (var go in objects)
            {
                if (go.name != objName) continue;
                if (typeName == "GameObject") return go;
                var comp = go.GetComponent(typeName);
                if (comp != null) return comp;
            }
            return null;
        }
        #endregion

        #region Local Function
        private static string MethodName(ESetupContext _context)
        {
            switch (_context)
            {
                case ESetupContext.Global: return nameof(OnSetupGlobal);
                case ESetupContext.Local: return nameof(OnSetupLocal);
                default: return nameof(OnSetupEtc);
            }
        }
        private static Type FindType(string _typeName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in asm.GetTypes())
                {
                    if (t.Name == _typeName && typeof(Component).IsAssignableFrom(t))
                        return t;
                }
            }
            return null;
        }
        /// <summary>문자열 값을 프로퍼티 타입에 맞게 변환해 적용한다</summary>
        private static void ApplyStringValue(SerializedProperty _prop, string _value)
        {
            try
            {
                switch (_prop.propertyType)
                {
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.LayerMask:
                        _prop.intValue = int.Parse(_value);
                        break;
                    case SerializedPropertyType.Boolean:
                        _prop.boolValue = bool.Parse(_value);
                        break;
                    case SerializedPropertyType.Float:
                        _prop.floatValue = float.Parse(_value);
                        break;
                    case SerializedPropertyType.String:
                        _prop.stringValue = _value;
                        break;
                    case SerializedPropertyType.Enum:
                        var idx = Array.IndexOf(_prop.enumNames, _value);
                        if (idx >= 0)
                            _prop.enumValueIndex = idx;
                        else if (int.TryParse(_value, out var enumIdx))
                            _prop.enumValueIndex = enumIdx;
                        break;
                    case SerializedPropertyType.Color:
                        if (ColorUtility.TryParseHtmlString(_value, out var color))
                            _prop.colorValue = color;
                        break;
                    case SerializedPropertyType.Vector2:
                        _prop.vector2Value = ParseVector2(_value);
                        break;
                    case SerializedPropertyType.Vector3:
                        _prop.vector3Value = ParseVector3(_value);
                        break;
                    case SerializedPropertyType.Vector4:
                        _prop.vector4Value = ParseVector4(_value);
                        break;
                    default:
                        Debug.LogWarning($"[ModuleSetup] 지원하지 않는 프로퍼티 타입: {_prop.propertyType} ({_prop.propertyPath})");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[ModuleSetup] 프로퍼티 설정 실패: {_prop.propertyPath} = {_value} ({e.Message})");
            }
        }
        /// <summary>Transform 계층의 모든 자식을 재귀적으로 수집한다</summary>
        private static void CollectAllChildren(Transform _parent, List<GameObject> _list)
        {
            _list.Add(_parent.gameObject);
            for (int i = 0; i < _parent.childCount; i++)
                CollectAllChildren(_parent.GetChild(i), _list);
        }
        /// <summary>"x,y" 형식 문자열을 Vector2로 파싱한다</summary>
        private static Vector2 ParseVector2(string _value)
        {
            var parts = _value.Split(',');
            return new Vector2(
                float.Parse(parts[0].Trim()),
                float.Parse(parts[1].Trim()));
        }
        /// <summary>"x,y,z" 형식 문자열을 Vector3로 파싱한다</summary>
        private static Vector3 ParseVector3(string _value)
        {
            var parts = _value.Split(',');
            return new Vector3(
                float.Parse(parts[0].Trim()),
                float.Parse(parts[1].Trim()),
                float.Parse(parts[2].Trim()));
        }
        /// <summary>"x,y,z,w" 형식 문자열을 Vector4로 파싱한다</summary>
        private static Vector4 ParseVector4(string _value)
        {
            var parts = _value.Split(',');
            return new Vector4(
                float.Parse(parts[0].Trim()),
                float.Parse(parts[1].Trim()),
                float.Parse(parts[2].Trim()),
                float.Parse(parts[3].Trim()));
        }
        /// <summary>프로퍼티가 기대하는 오브젝트 타입을 PPtr 형식에서 추출한다</summary>
        private static Type GetExpectedObjectType(SerializedProperty _prop)
        {
            var type = _prop.type;
            if (!type.StartsWith("PPtr<$")) return null;
            var typeName = type.Substring(6, type.Length - 7);
            var result = Type.GetType($"UnityEngine.{typeName}, UnityEngine.CoreModule");
            if (result != null) return result;
            result = Type.GetType($"UnityEngine.{typeName}, UnityEngine");
            if (result != null) return result;
            result = Type.GetType($"UnityEngine.UI.{typeName}, UnityEngine.UI");
            if (result != null) return result;
            result = Type.GetType($"TMPro.{typeName}, Unity.TextMeshPro");
            if (result != null) return result;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                result = assembly.GetType(typeName);
                if (result != null) return result;
                foreach (var t in assembly.GetTypes())
                {
                    if (t.Name == typeName && typeof(UnityEngine.Object).IsAssignableFrom(t))
                        return t;
                }
            }
            return null;
        }
        /// <summary>직속 자식 중 이름이 일치하는 오브젝트를 찾는다 (재귀 아님)</summary>
        private static GameObject FindChildByName(Transform _parent, string _name)
        {
            for (int i = 0; i < _parent.childCount; i++)
            {
                var child = _parent.GetChild(i);
                if (child.name == _name)
                    return child.gameObject;
            }
            return null;
        }
        #endregion
    }
}
