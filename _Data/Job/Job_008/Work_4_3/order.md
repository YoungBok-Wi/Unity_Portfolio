# 업무지시서

## 1. LocalGameManager 플레이어 참조 변경 알림 API 작성

**대상 스킬**: 게임개발_모듈_폴더_작성

**"moduleId"**: Game

**"moduleNamespace"**: Game

**"content"**: `LocalGameManager`의 활성 플레이어 참조 변경을 구독할 수 있는 공개 알림 API 추가

**업무**

- `Player` 값이 실제로 달라질 때만 알림을 발행하고, `SetPlayer`를 플레이어 참조 갱신의 단일 통로로 유지한다
- 구독자가 새 `Player`를 즉시 다시 바인딩할 수 있도록 변경 후 알림을 발행한다
- 기존 `Player`·`SetPlayer` 호출 계약과 프리팹 메타는 유지한다
- 완료 기준: `Game` 모듈 코드 verify `success:true`

## 2. 컴파일

**대상 스킬**: 유니티엔진_컴파일_실행

**"changedPaths"**: Assets/__Game/Game/**

**업무**

- 완료 기준: 에러 0, `up_to_date`가 아니라 실제 컴파일 수행 확인
- 컴파일 에러가 있으면 1로 돌아가 수정 후 재실행한다

## 3. Game 모듈 익스포트

**대상 스킬**: 게임개발_모듈_폴더_익스포트

**"moduleId"**: Game

**"moduleNamespace"**: Game

**업무**

- 완료 기준: export 응답 `success:true`, 재임포트 완료
- `confirmed`·`reuse`는 변경하지 않는다
