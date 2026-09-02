#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Library;

/// <summary>플레이 모드에서 PlayerPrefs 데이터 실시간 조회 에디터 도구</summary>
public class PlayerPrefsViewerTool : EditorWindow
{
    private Vector2 scrollPos;

    [MenuItem("Tools/Save/PlayerPrefs Viewer")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsViewerTool>("PlayerPrefs Viewer");
    }

    private void OnGUI()
    {
        GUILayout.Label("In-Game PlayerPrefs Viewer", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("이 툴은 플레이 모드(Play Mode)에서만 데이터 확인이 가능합니다.", MessageType.Info);
            return;
        }
        if (PlayerPrefsSaveManager.instance == null)
        {
            EditorGUILayout.HelpBox("인스턴스를 찾을 수 없습니다.\n게임이 초기화되었는지 확인하세요.", MessageType.Warning);
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("PlayerPrefsSaveManager", EditorStyles.boldLabel);
        foreach (var value in PlayerPrefsSaveManager.instance.GetValues())
            DrawValueItem(value.Key);
        EditorGUILayout.EndScrollView();

        Repaint();
    }

    private void DrawValueItem(string _id)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("ID (Key):", _id, EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Value:", GUILayout.Width(100));
        EditorGUILayout.TextField(PlayerPrefs.GetString(_id), GUILayout.MinHeight(10));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        GUILayout.Space(5);
    }
}
#endif