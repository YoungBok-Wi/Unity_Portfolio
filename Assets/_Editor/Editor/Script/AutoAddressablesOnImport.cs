#if NBING_ADDRESSABLE
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AutoAddressablesOnImport : AssetPostprocessor
{
    // Assets/__Game/_{addressableId}/... 형식의 에셋을 addressableId 그룹에 자동 등록한다.
    private const string GAME_ROOT = "Assets/__Game/_";
    // addressable 이 없는(무시할) 폴더 ID
    private const string IGNORE_ID = "Core";

    // 에셋이 import / delete / move 될 때마다 호출됨
    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        // Addressables 세팅 없으면 아무 것도 안 함
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null) return;

        bool changed = false;

        // import + move 된 대상들만 처리
        var targets = importedAssets.Concat(movedAssets)
            .Where(p => !string.IsNullOrEmpty(p))
            .Select(p => p.Replace("\\", "/"))
            .Distinct();

        foreach (var path in targets)
        {
            // 폴더/스크립트/메타 등 제외
            if (AssetDatabase.IsValidFolder(path)) continue;
            if (path.EndsWith(".meta", StringComparison.OrdinalIgnoreCase)) continue;
            if (path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) continue;
            if (path.EndsWith(".asmdef", StringComparison.OrdinalIgnoreCase)) continue;

            // Assets/__Game/_{addressableId}/... 형식만 처리 (Core·형식 불일치 제외)
            if (!TryGetAddressable(path, out var addressableId, out var address)) continue;

            // addressableId 이름의 그룹에 등록 (없으면 생성)
            var group = settings.FindGroup(addressableId);
            if (group == null)
                group = settings.CreateGroup(addressableId, false, false, false, null,
                    typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));

            var guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid)) continue;

            // 엔트리 생성/이동
            var entry = settings.CreateOrMoveEntry(guid, group, readOnly: false, postEvent: false);
            if (entry == null) continue;

            // 주소: _{addressableId}/ 이후 상대경로, 확장자 제거
            if (entry.address != address)
            {
                entry.address = address;
                changed = true;
            }
        }

        // _{addressableId} 폴더 밖(또는 Core)으로 이동한 에셋은 Addressables 엔트리 제거
        changed |= RemoveEntriesMovedOut(settings, movedFromAssetPaths, movedAssets);

        if (changed)
        {
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>Assets/__Game/_{addressableId}/{상대경로} 형식이면 addressableId 와 주소(확장자 제거 상대경로)를 반환. Core·형식 불일치는 false</summary>
    private static bool TryGetAddressable(string path, out string addressableId, out string address)
    {
        addressableId = null;
        address = null;

        if (!path.StartsWith(GAME_ROOT, StringComparison.OrdinalIgnoreCase)) return false;

        // "{addressableId}/{상대경로}"
        var rest = path.Substring(GAME_ROOT.Length);
        int slash = rest.IndexOf('/');
        if (slash <= 0) return false; // _{addressableId} 직속에 파일/하위가 있어야 함

        addressableId = rest.Substring(0, slash);
        if (addressableId.Equals(IGNORE_ID, StringComparison.OrdinalIgnoreCase)) return false;

        var sub = rest.Substring(slash + 1);
        var dot = sub.LastIndexOf('.');
        address = dot >= 0 ? sub.Substring(0, dot) : sub;
        return true;
    }

    // 폴더 밖(또는 Core)으로 이동한 에셋은 Addressables 엔트리 제거
    private static bool RemoveEntriesMovedOut(AddressableAssetSettings settings, string[] movedFrom, string[] movedTo)
    {
        bool changed = false;
        for (int i = 0; i < Math.Min(movedFrom.Length, movedTo.Length); i++)
        {
            var from = movedFrom[i]?.Replace("\\", "/");
            var to = movedTo[i]?.Replace("\\", "/");
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) continue;

            bool wasAddressable = TryGetAddressable(from, out _, out _);
            bool nowAddressable = TryGetAddressable(to, out _, out _);
            if (wasAddressable && !nowAddressable)
            {
                var guid = AssetDatabase.AssetPathToGUID(to); // 이동 후 경로 guid
                var entry = settings.FindAssetEntry(guid);
                if (entry != null)
                {
                    settings.RemoveAssetEntry(guid);
                    changed = true;
                }
            }
        }
        return changed;
    }
}
#endif
