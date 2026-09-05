# [유니티엔진_에셋_폰트] "Noto Sans JP TMP 폰트 에셋 생성·DefaultFont 폴백 연결" 업무 레포트

## 요약
- `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular SDF.asset`(guid `38ff9e584ec7f5f4e8e8ca665498de09`) 생성 — 원본 `Font_Casual_NotoSansJP_Regular.otf`(guid `09e72d2dded715b4db78737eb584e072`), 샘플링 36pt·패딩 9·SDFAA·1024×1024·Dynamic·멀티 아틀라스 — `DefaultFont.asset`·`DefaultFont_Bold.asset` `m_FallbackFontAssetTable` 1건 연결
- 플레이 실측: 일본어 전환 후 로비 "戦闘開始"·"近接3段コンボ"·"遠距離連射", 설정 "設定·効果音·適用·既定値" 전부 정상 렌더(□ 0) — ⑳ 해소. 콘솔 에러 0

## 완료업무

### 폰트 에셋 생성
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Font\Font_Casual_NotoSansJP_Regular SDF.asset`
**작업내용**
- `TMP_FontAsset.CreateFontAsset(font, 36, 9, SDFAA, 1024, 1024, Dynamic, true)` 로 생성 후 `AssetDatabase.CreateAsset`·`SaveAssets`
- `get_serialized_fields` 대조: `m_AtlasPopulationMode 1`(Dynamic), `m_AtlasWidth/Height 1024`, `m_AtlasPadding 9`, `m_AtlasRenderMode 4165`(SDFAA), `m_IsMultiAtlasTexturesEnabled 1`, `m_SourceFontFile_EditorRef` guid `09e72d2d…`

### 폴백 연결
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\TextMesh Pro\Resources\Fonts & Materials\DefaultFont.asset`
`C:\_Projects\Unity_Portfolio\Assets\TextMesh Pro\Resources\Fonts & Materials\DefaultFont_Bold.asset`
**작업내용**
- 두 에셋 `fallbackFontAssetTable` 에 SDF 에셋 추가(각 1건, 중복 없음) → `EditorUtility.SetDirty`·`SaveAssets`, YAML `m_FallbackFontAssetTable: - {fileID: 11400000, guid: 38ff9e58…}` 확인
- 검증(`유니티엔진_에셋_검증`): `AssetDatabase.Refresh` 후 미싱 참조 0·고아 meta 0, 콘솔 에러 0, 캡처 `_Temp/Work_5_J6/cap/lobby_ja_fix.png`·`setting_ja_fix.png`·`quit_ja_fix.png`

## 비고
- Dynamic 모드라 한자 글리프는 실행 중 아틀라스에 추가된다 — 에디터 플레이에서 추가된 글리프는 에셋에 남지 않는다(빌드 실행 시 첫 표시 프레임에 생성)
