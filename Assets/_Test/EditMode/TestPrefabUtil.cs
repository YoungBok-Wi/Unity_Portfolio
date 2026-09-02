using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>프리팹 에셋을 열어 전용 스크립트의 직렬화 참조 배선을 검사하는 테스트 헬퍼</summary>
public static class TestPrefabUtil
{
    #region Const
    /// <summary>오브젝트 프리팹 폴더 (폴더-퍼-프리팹)</summary>
    public const string ObjectRoot = "Assets/__Game/_Core/_Object";
    /// <summary>팝업 프리팹 폴더</summary>
    public const string PopupRoot = "Assets/__Game/_Core/_UI/Popup";
    /// <summary>컨트롤 프리팹 폴더</summary>
    public const string ControlRoot = "Assets/__Game/_Core/_UI/Control";
    /// <summary>애드온 프리팹 폴더 (하위가 종속 컨트롤로 한 겹 더 나뉜다)</summary>
    public const string AddonRoot = "Assets/__Game/_Core/_UI/Addon";
    #endregion

    #region Function
    /// <summary>_root/_id/_id.prefab 프리팹 에셋을 반환한다. 없으면 null</summary>
    public static GameObject Load(string _root, string _id)
    {
        return AssetDatabase.LoadAssetAtPath<GameObject>($"{_root}/{_id}/{_id}.prefab");
    }
    /// <summary>_parentId 종속 애드온 프리팹 에셋을 반환한다. 없으면 null</summary>
    public static GameObject LoadAddon(string _parentId, string _id)
    {
        return AssetDatabase.LoadAssetAtPath<GameObject>($"{AddonRoot}/{_parentId}/{_id}/{_id}.prefab");
    }
    /// <summary>_prefab 루트의 T 컴포넌트에서 값이 비어 있는 UnityEngine.Object 직렬화 필드명을 모아 반환한다</summary>
    public static List<string> FindUnassigned<T>(GameObject _prefab) where T : Component
    {
        var result = new List<string>();
        var component = _prefab.GetComponent<T>();
        if (component == null)
            return result;

        var serialized = new SerializedObject(component);
        var property = serialized.GetIterator();
        bool enterChildren = true;
        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                    result.Add(property.name);
                continue;
            }
            if (!property.isArray || !property.arrayElementType.StartsWith("PPtr<"))
                continue;

            for (int i = 0; i < property.arraySize; ++i)
                if (property.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    result.Add($"{property.name}[{i}]");
        }
        return result;
    }
    /// <summary>_prefab 루트 T 컴포넌트의 _fieldName 직렬화 필드 값을 반환한다. 필드가 없으면 null</summary>
    public static object GetField<T>(GameObject _prefab, string _fieldName) where T : Component
    {
        var component = _prefab.GetComponent<T>();
        if (component == null)
            return null;

        var field = typeof(T).GetField(_fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        return (field == null) ? null : field.GetValue(component);
    }
    #endregion
}
