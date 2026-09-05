# [유니티엔진_에셋_폰트_구성] "㉒ 보완 — 폴백 SDF 샘플링·패딩 정합 재생성" 업무 레포트

## 요약
- 폴백 에셋 2건으로 분리: `Font_Casual_NotoSansJP_Regular SDF.asset`(96pt·패딩 4, guid `38ff9e58…` 유지 경로 재생성) → `DefaultFont` 폴백, `Font_Casual_NotoSansJP_Regular_Bold SDF.asset`(64pt·패딩 2, guid `8eb84798…`) → `DefaultFont_Bold` 폴백 — 이전 36pt·패딩 9 에셋 삭제
- 캡처 실측: Notify `お知らせ`·`閉じる`, Pause `一時停止·再開·設定·営業終了`, Result `営業結果·調理失敗`, Quit `ゲーム終了`·로비 `戦闘開始` 전부 주 서체와 같은 외곽선·색(그림자 사각·흰 밑판 0). 일본어 문구 전 문자 197자 `HasCharacters` Regular·Bold 모두 true, 콘솔 에러 0 — ㉒ 해소
- `리소스컨셉` 서체 항목에 "폴백 에셋은 연결 대상 서체와 같은 샘플링·패딩" 규칙 추가, `concept_manage verify Resource` `success:true`

## 완료업무

### 폴백 SDF 재생성·재연결
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Font\Font_Casual_NotoSansJP_Regular SDF.asset`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Font\Font_Casual_NotoSansJP_Regular_Bold SDF.asset`
`C:\_Projects\Unity_Portfolio\Assets\TextMesh Pro\Resources\Fonts & Materials\DefaultFont.asset`
`C:\_Projects\Unity_Portfolio\Assets\TextMesh Pro\Resources\Fonts & Materials\DefaultFont_Bold.asset`
**작업내용**
- 원인: TMP 는 폴백 글리프에 원 머티리얼 속성(외곽선·밑판)을 복사해 쓰는데 SDF 확산폭은 샘플링 대비 패딩 비율이라 36/9(25%)와 64/2(3%)가 달라 같은 값이 폴백에서 8배로 번진다
- `_Temp/Work_1_J7/mkfont.cs`(`eval_file`): 기존 에셋 삭제 → `CreateFontAsset(otf, 96, 4, …)`·`(otf, 64, 2, …)` 생성 → 두 DefaultFont `fallbackFontAssetTable` 재연결 → `SaveAssets`. YAML: `m_PointSize 96/64`, `m_AtlasPadding 4/2`, `m_AtlasPopulationMode 1`, `m_IsMultiAtlasTexturesEnabled 1`, 폴백 guid 각 1건
- 검증: 캡처 `_Temp/Work_5_J6/cap/notify_ja_fix2.png`·`pause_ja_fix2.png`·`result_ja_fix2.png`·`quit_ja_fix2.png`(확대 `_Temp/Work_1_J7/crop2_*.png`), `_Temp/Work_1_J7/hascheck.cs` 197자 결손 0

## 비고
- `DefaultFont.asset`·`DefaultFont_Bold.asset` 은 git `skip-worktree` 플래그가 걸려 있다(Dynamic 글리프 갱신이 매 플레이마다 diff 수천 줄) — 커밋은 HEAD 사본에 폴백 줄만 넣어 올린 뒤 플래그를 되돌리는 방식(Work_3 과 동일)
