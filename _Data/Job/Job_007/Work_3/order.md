# 업무지시서

## 1. 폰트 에셋 현황 조회

**대상 스킬**: 유니티엔진_에셋_질문

**"question"**: `Assets/TextMesh Pro/Resources/Fonts & Materials/DefaultFont.asset`·`DefaultFont_Bold.asset`의 `m_SourceFontFile`·`m_AtlasPopulationMode`·`m_FallbackFontAssetTable`·아틀라스 크기, 게임 프리팹이 참조하는 폰트 에셋 GUID(`8f586378…` 33건)

**업무**

- 근거: `job.md` ⑳ (DefaultFont 원본 = `Font_Casual_GyeonggiTitle_Light_Default.ttf`, Dynamic, 폴백 `[]`)

## 2. Noto Sans JP 폰트 에셋 생성·폴백 연결

**대상 스킬**: 유니티엔진_에셋_폰트_구성

**"assetPath"**: `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular SDF.asset`

**업무**

- "content": 원본 `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular.otf`(Work_2), `CreateFontAsset(font, 36, 9, SDFAA, 1024, 1024, Dynamic, true)`로 생성, `m_ClearDynamicDataOnBuild` 기본값 유지(빌드 시 동적 데이터 정리 — 런타임에 다시 굽는다). 생성 후 `DefaultFont.asset`·`DefaultFont_Bold.asset` `m_FallbackFontAssetTable`에 새 에셋 1건 추가(YAML 직접 편집 가능 — `get_serialized_fields`로 반영 확인)
- 완료 기준: 두 DefaultFont의 폴백 테이블에 새 에셋 GUID, 새 에셋 `m_SourceFontFile`이 `.otf` 실재, 콘솔 에러 0, 재임포트(`유니티엔진_재임포트_실행`)

## 3. 에셋 검증

**대상 스킬**: 유니티엔진_에셋_검증

**"scope"**: 폰트 에셋 3건 필드·미싱·고아meta

**업무**

- 완료 기준: 폴백 필드 일치, 미싱 0, 고아 meta 0, 컴파일 에러 0. 실제 글리프 표시는 Work_5 QA(일본어 캡처)로 판정
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
