# 업무지시서

## 1. 회차 커밋

**대상 스킬**: 깃_커밋_실행

**"message"**: [Job_008 / Work_7] 폴리싱 7회차 — 사용자 상세 오더 16건 반영·README 갱신 (요약은 실제 결과로)

**"paths"**: (전체 변경분 — `Assets/`·`ProjectSettings/`·`_Data/Job/Job_008/`·`_Data/Concept/`·`README.md` 등)

**업무**

- 커밋 전 README 3건(폴리싱 절·개요 Fable 5.1·GPT6 Astra 절)이 갱신돼 있어야 한다 — 오케스트레이터가 이 Work 진입 직전에 직접 수행하며, 없으면 예외로 보고한다
- `.gitignore` 대상(`_Data` 제외 항목)은 스킬 절차대로 스테이징에서 빠진다
- 완료 기준: 커밋 해시 확보, 작업 트리 clean (미추적 잔여는 사유 기록)

## 2. 푸시

**대상 스킬**: 깃_푸시_실행

**"remote"**: origin

**"branch"**: main

**업무**

- 완료 기준: 원격 `main`이 커밋 해시와 일치. 인증 실패 등으로 푸시가 막히면 예외로 보고하고 커밋은 유지한다
- DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
