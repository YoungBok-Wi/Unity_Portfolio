using System;
using UnityEditor;
using UnityEngine;

public class AutoTextureSettingOnImport : AssetPostprocessor
{
    // 폴더명 매칭: 경로의 폴더 세그먼트가 Image(단수) 이거나, 경로에 /Images/ /Textures/ /Icon/ 이 포함되면 적용
    // 예: Assets/__Game/_Core/Image/foo.png, Assets/UI/Images/foo.png, Assets/Art/Textures/bar_Normal.tga 등

    void OnPreprocessTexture()
    {
        var importer = (TextureImporter)assetImporter;
        string path = assetPath.Replace('\\', '/'); // 윈도 경로 대비

        // 폴더명이 정확히 "Image"인 폴더의 텍스처 → Sprite/Single/FullRect (이미 Sprite면 미변경)
        if (IsInFolderNamed(path, "Image"))
        {
            ApplySpriteSingleFullRect(importer);
            return;
        }

        bool isInImagesFolder = path.IndexOf("/Images/", StringComparison.OrdinalIgnoreCase) >= 0;
        bool isInIconFolder = path.IndexOf("/Icon/", StringComparison.OrdinalIgnoreCase) >= 0;
        bool isInTexturesFolder = path.IndexOf("/Textures/", StringComparison.OrdinalIgnoreCase) >= 0;

        // 우선순위: Image/Icon이 Sprite 규칙, Textures가 Default 규칙
        // (폴더 구조상 겹칠 가능성이 적지만, 겹치면 Sprite를 우선 적용)
        if (isInImagesFolder || isInIconFolder)
        {
            ApplySpriteSingle(importer);
            return;
        }

        if (isInTexturesFolder)
        {
            ApplyDefaultOrNormalMap(importer, path);
            return;
        }
    }

    // 경로의 폴더 세그먼트(파일명 제외) 중 folderName 과 정확히 일치하는 것이 있으면 true.
    // 파일명은 검사하지 않으므로 "Image_foo.png" 같은 파일명은 "Image" 폴더로 오인하지 않는다.
    private static bool IsInFolderNamed(string path, string folderName)
    {
        int lastSlash = path.LastIndexOf('/');
        if (lastSlash < 0) return false;
        string dir = path.Substring(0, lastSlash);
        foreach (var seg in dir.Split('/'))
        {
            if (string.Equals(seg, folderName, StringComparison.Ordinal))
                return true;
        }
        return false;
    }

    // Image 폴더 규칙: Sprite / Single / FullRect. 이미 Sprite 로 설정된 텍스처는
    // 사용자가 지정한 모드·메시타입 등을 보존하기 위해 손대지 않는다.
    private static void ApplySpriteSingleFullRect(TextureImporter importer)
    {
        if (importer.textureType == TextureImporterType.Sprite)
            return;

        try
        {
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.textureType = TextureImporterType.Sprite;
            settings.spriteMode = (int)SpriteImportMode.Single;
            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
        }
        catch (Exception e)
        {
            // 훅 실패가 임포트 파이프라인 전체를 막지 않도록 로그만 남기고 통과시킨다.
            Debug.LogWarning($"[AutoTextureSettingOnImport] Image 폴더 Sprite 설정 실패: {importer.assetPath} — {e.Message}");
        }
    }

    private static void ApplySpriteSingle(TextureImporter importer)
    {
        bool changed = false;

        if (importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            changed = true;
        }

        if (importer.spriteImportMode != SpriteImportMode.Single)
        {
            importer.spriteImportMode = SpriteImportMode.Single;
            changed = true;
        }

        // 필요하면 여기서 공통 Sprite 옵션도 강제할 수 있음(예: mipmap off 등)
        // if (importer.mipmapEnabled) { importer.mipmapEnabled = false; changed = true; }

        if (changed)
        {
            // Debug.Log($"[AutoImport] Sprite Single 적용: {importer.assetPath}");
        }
    }

    private static void ApplyDefaultOrNormalMap(TextureImporter importer, string path)
    {
        bool changed = false;

        // 기본은 Default
        if (importer.textureType != TextureImporterType.Default)
        {
            importer.textureType = TextureImporterType.Default;
            changed = true;
        }

        // 파일명 기준: _Normal 포함이면 NormalMap으로
        string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
        bool isNormal = fileName.IndexOf("_Normal", StringComparison.OrdinalIgnoreCase) >= 0;

        if (isNormal)
        {
            // Unity 버전에 따라 NormalMap 처리 방식이 조금 다를 수 있지만
            // TextureImporterType.NormalMap이 가장 직관적임.
            if (importer.textureType != TextureImporterType.NormalMap)
            {
                importer.textureType = TextureImporterType.NormalMap;
                changed = true;
            }
        }

        if (changed)
        {
            // Debug.Log($"[AutoImport] Textures 규칙 적용: {importer.assetPath} (Normal={isNormal})");
        }
    }
}