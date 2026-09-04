# 업무지시서

## 1. 커밋

**대상 스킬**: 깃_커밋_실행

**"taskContent"**: Job_004 개선 3회차 산출물 전체 커밋

**업무**

- 대상: `_Data/`(컨셉·고정값·리소스·Job 문서), `Assets/__Game/`(모듈·프리셋·SpriteAnim), `Assets/_Library/_Core/Resources/Table`·`GenerateScript`(고정값 익스포트), `_Temp/QA` 시나리오
- 메시지: `[Job_004 / Work_7] 개선 3회차 — {Work_6 안정화 판정 요약(해소 건수·잔존 건수)}`
- 커밋 전 `git status`로 의도치 않은 파일(캡처 `_Temp/Work_*_J4/cap` 등)을 제외한다. `.gitignore` 대상은 건드리지 않는다

## 2. 푸시

**대상 스킬**: 깃_푸시_실행

**"taskContent"**: `origin/main` 푸시

**업무**

- 완료 기준: `git log origin/main -1`이 커밋 해시와 일치. 사용자에게 질문하지 않는다
