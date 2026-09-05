# 업무지시서

## 1. 성능테스트

**대상 스킬**: QA_유니티게임개발_성능테스트_테스트

**"scenario"**: 방 10 Battle(적 8마리·투사체) 안정 구간과 보스방(Pumpkin Skill 전조) 각 120프레임 이상

**업무**

- 선행 `QA_유니티게임개발_성능테스트_질문`으로 성능 예산 기준 확인. 진행은 엔진 내 코루틴 드라이버(`_Temp/Work_6_J5/driver.cs` `Advance` 재사용)로 방 10·보스방 도달 후 측정
- 완료 기준: frameMsAvg ≤ 16.6·스파이크·gcAllocPerFrameKB·메모리 항목별 판정, 초과 시 원인 후보 지목

## 2. 플레이테스트 계획 갱신

**대상 스킬**: QA_유니티게임개발_플레이테스트_계획

**"content"**: `_Temp/QA` 시나리오에 Job_006 수정분 61~68 추가 — 처치 스플래터·Knife 궤적·능력 LevelUp SFX·해금 Unlock SFX·Crumb 낙하/수거/잔여 0·일본어 문구 표시(`LanguageManager` 일본어 전환 후 팝업 9종 `qa_ui` 잘림·글리프 결손 0)·잔재 문구 제거 후 `Text_` 원문 노출 0·companyName 변경 후 첫 실행 상태

**업무**

- 기존 60단계는 회귀 확인으로 유지(Job_005 합격 항목), 치트 명세는 `QA_유니티게임개발_플레이테스트_치트_작성`으로 갱신(KillEnemies 낙하분 적립 확인)

## 3. 전체 재평가·안정화 판정

**대상 스킬**: QA_유니티게임개발_플레이테스트_테스트

**"scope"**: 갱신 시나리오 전 단계, 안정화 판정, 재점검(`project_manage unused` 재조회·콘솔 전 심각도 0·씬 Missing 0)

**업무**

- 1920x1080, 캡처 `_Temp/Work_5_J6/cap/`. `## 비고`에 "Job_006 점검 항목 대조" 표(⑭~⑲ 해소·잔존)와 "안정화 상태 판정"·"신규 발견 목록(⑳부터)"을 넣는다 — 신규 발견 0건이면 반복 종료 근거가 된다
- 일본어 표시는 폰트 글리프 결손(□ 표시)을 `TMP_Text` `havePropertiesChanged`가 아니라 `qa_ui` `renderW`(0이면 결손)와 캡처로 판정한다
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 산출물 무수정. 사용자에게 질문하지 않는다
