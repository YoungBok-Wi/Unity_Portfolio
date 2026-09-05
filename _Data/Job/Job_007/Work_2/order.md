# 업무지시서

## 1. 서체 타입 현황 조회

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `Font` 계열 노드·타입 2건(`Font_Casual_GyeonggiTitle_Bold/Light`)의 출력 슬롯 규약(`font` 슬롯 `.ttf`·`leaf Font`·`resources false`·idPrefix)과 `Assets/__Game/_Core/Font` 익스포트 결과

**업무**

- 근거: `job.md` ⑳. 새 타입은 기존 타입과 같은 슬롯 규약을 따르되 확장자는 `.otf`(원본 `_Temp/Work_1_J7/NotoSansJP-Regular.otf`, OpenType CFF)

## 2. 타입 생성·구성

**대상 스킬**: 게임개발_구성_리소스_타입_생성

**"content"**: `Font/Font_Casual_NotoSansJP` 생성 후 `게임개발_구성_리소스_타입_구성`으로 설명("Noto Sans JP Regular — 일본어 한자 폴백 서체, SIL OFL 1.1, TMP 폴백 전용")·`addressableId Core`·출력 슬롯 `font`(`suffix ""`·`ext ".otf"`·`leaf "Font"`·`resources false`·`idPrefix "Font_Casual_NotoSansJP_"`·`isPreview true`) 구성

**업무**

- 변종 묶음 아님(공유 규격 3항목 대상 아님 — 서체). 자동화 없음(업로드 전용)
- 완료 기준: `resource_type get` 반영, `reuse`는 생성 기본값(`add`·프로젝트 저장)

## 3. 파일 등록·반입·익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_생성

**"content"**: entry `Regular`(설명 "Noto Sans JP Regular OTF") 등록(`filePath` 원본 적재·슬롯 `font`) → `게임개발_구성_리소스_파일_업로드` 규칙대로 라이선스(OFL 1.1 — `_Temp/Work_1_J7/LICENSE_OFL.txt`)·글리프(일본어 한자 포함)·가독 판정 후 반입 → `게임개발_구성_리소스_파일_익스포트`로 `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular.otf` 반영·재임포트

**업무**

- 라이선스 사본은 `unity cmd import_asset --source={LICENSE_OFL.txt 절대경로} --path=Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_OFL.txt`로 서체 옆에 둔다(OFL 조건: 라이선스 동봉)
- 완료 기준: `resource_file get` pool·select 반영, 익스포트 파일·`.meta` 실재, 임포트 에러 0
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
