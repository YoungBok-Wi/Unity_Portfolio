# [게임개발_구성_리소스_타입_생성] "Noto Sans JP 서체 타입·파일 등록·반입·익스포트" 업무 레포트

## 요약
- 타입 `Font/Font_Casual_NotoSansJP` 생성(`reuse:add`·`location:project`, 소속 `Font_Casual`, `addressableId Core`)·구성(출력 슬롯 `font` `.otf`·`leaf Font`·`resources false`·`idPrefix Font_Casual_NotoSansJP_`), 파일 entry `Regular` 등록 + 원본 적재(pool `1.otf`·`select 1.otf`)·`inAsset true` → `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular.otf`(4,533,028B) + `.meta` 실재, 라이선스 `Font_Casual_NotoSansJP_OFL.txt` 반입
- 글리프 실측(fontTools cmap 16,732자): 한자 "戦闘開始" 포함, 가나 포함, 한글 미포함(주 서체 몫), 라틴 포함 — 폴백 용도 적합. 라이선스 SIL OFL 1.1(동봉)
- 전체 `resource_file export`는 서버 장시간 블로킹 이력대로 백그라운드 curl로 투입(03:40 시작) — 완료 여부는 `_Temp/Work_1_J7/export_full.json`, 사본은 `inAsset` patch가 이미 복사해 실측으로 대조

## 완료업무

### 서체 타입 현황 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Font_Casual_GyeonggiTitle_Light\type.json`
**작업내용**
- `resource_node node Font_Casual`: 타입 2건(GyeonggiTitle Bold·Light, `reuse:default`·shared), `resource_type get Light`: 슬롯 `font` `.ttf`·`leaf Font`·`resources false`·`idPrefix Font_Casual_GyeonggiTitle_Light_`·suffix `""` → 새 타입은 같은 규약에 `.otf`만 다르게

### 타입 생성·구성
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Font_Casual_NotoSansJP\type.json`
**작업내용**
- `resource_type create Font/Font_Casual_NotoSansJP reuse:add` → patch(설명·`parentNodeId Font_Casual`·`addressableId Core`·outputs.font) → get 재조회 전건 반영. 변종 묶음 아님(공유 규격 대상 아님), 자동화 없음

### 파일 등록·반입·익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Font\Font_Casual_NotoSansJP_Regular.otf`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Font\Font_Casual_NotoSansJP_OFL.txt`
**작업내용**
- `resource_file create Regular`(`filePath` `_Temp/Work_1_J7/NotoSansJP-Regular.otf`, slot `font`) → get: pool `1.otf`·select `1.otf`(서버 부여 파일명 확인 — "pool 적재 확인")
- 업로드 판정(`게임개발_구성_리소스_파일_업로드` 폰트 항목 규칙): 라이선스 OFL 1.1(`LICENSE_OFL.txt` 원문 첫 줄 "SIL Open Font License, Version 1.1"), 표시 언어 글리프 — 일본어 한자·가나 포함 실측, 최소 표시 크기 가독 — 규격 36pt 샘플링·SDFAA(Work_3에서 폰트 에셋 생성 시 적용)
- `patch {"inAsset": true}` → 사본 `Font_Casual_NotoSansJP_Regular.otf` 즉시 복사, `AssetDatabase.Refresh` 후 `.meta` 생성·`get_import_settings` `TrueTypeFontImporter` 응답, 임포트 에러 0
- 라이선스: `unity cmd import_asset --source=…/LICENSE_OFL.txt --path=Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_OFL.txt` `success`
- 익스포트(`게임개발_구성_리소스_파일_익스포트`): export 액션은 `projectID`만 받아 전 리소스를 돌린다 — Job_004 `Work_3_1` 실측(엔트리별 타입 재로드 ≈16분 블로킹)대로 백그라운드 curl 투입, export 완료(03:56 `success:true`), `resource_file path` → `font` = `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular.otf` 일치. 사본 실측: 파일 크기 4,533,028B = 원본, `.meta` 실재

## 비고
- 헤더 규격 실측은 이미지 항목 전용이라 서체는 건너뜀(`od` 규칙 대상 아님) — 대신 fontTools로 cmap 실측
- 전체 export 완료 시각·응답은 `_Temp/Work_1_J7/export_full.json`(완료 후 Work_5 재점검에서 재확인)
