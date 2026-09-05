# 업무지시서

## 1. 문구 정본 조회

**대상 스킬**: 게임개발_구성_컨셉_질문

**"question"**: `게임컨셉` 지원 언어·문구 정본 규정과 `씬설정` Language 항목, 잔재 문구 112행이 어느 정본에도 소속되지 않는지

**업무**

- 근거: `_Data/Job/Job_006/job.md` 점검 결과 ⑯·⑰. 지원 언어 정본이 없으면 라이브러리 `LanguageConst.LanguageList`(English·Korean·Japanese)를 실측 근거로 쓴다

## 2. 일본어 입력·잔재 제거

**대상 스킬**: 게임개발_구성_데이터_테이블_텍스트_작성

**"content"**: `Text` 테이블 `Core` 시트 게임 사용 33행(`Text_Core_WeaponKnifeDesc`~`Text_Core_GunUnlocked`, `job.md` ⑯ 목록)의 `Jap` 값을 Kor·Eng와 같은 뜻·같은 `{0}` 자리표시자로 입력하고, 미참조 잔재 행을 제거

**업무**

- 일본어 문구 규칙: 게임 고유명사(Kitchen Riot·Crumb 등)는 기존 행의 일본어 표기(`Text_Core_CurrencyCrumb` "くず" 등)와 맞추고, 문장은 다른 행의 어체(です·ます 없는 짧은 체언 종결)에 맞춘다. `{0}`은 그대로 둔다
- 제거 대상: 잔재 112행 중 `Core` 시트 95행과 `Popup_Setting` 시트 8행(`job.md` ⑰ — `Popup`·`Shutdown`·`Quit` 시트 9행은 라이브러리 모듈 소속이라 유지). 제거 전 `Grep`으로 `Assets`·`_Data/Concept`·`_Data/Module/Game` 참조 0건을 행마다 재확인하고, 1건이라도 참조되면 그 행은 남기고 보고한다
- 완료 기준: `table_excel get Text/Core` 재조회에서 33행 Jap 비어 있지 않음·제거 행 부재, 제거 행 ID의 `Assets` 참조 0건

## 3. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"scope"**: `데이터` 전 종류

**업무**

- 완료 기준: export 전 종류 `success`, `TableText.json` 행 수 = 177 − 제거 수, 컴파일 `completed`/`up_to_date`·콘솔 에러 0
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
