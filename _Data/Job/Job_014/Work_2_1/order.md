# 업무지시서

## 1. 총기 점프 외형 분기 작성

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_코드_작성

**"objectId"**: `Object_Player_Gun`

**업무**
- `Jump_Gun` 리소스가 없는 동안 `Jump` 요청을 기존 총기 전용 `Idle_Gun` 클립으로 변환한다.
- 점프 물리는 기존 `FSMState_PlayerJump`를 보존하고 외형만 칼 캐릭터로 바뀌지 않게 한다.
- 기존 `Idle_Gun`·`Move_Gun`·`Attack_Gun` 동작을 보존한다.

## 2. 총기 플레이어 코드 컴파일

**대상 스킬**: 유니티엔진_컴파일_실행

**"changedPaths"**: `Assets/__Game/_Core/_Object/Object_Player_Gun/Script/Object_Player_Gun.cs`

**업무**
- Unity 재컴파일 완료와 컴파일 에러 부재를 확인한다.

## 3. Object_Player_Gun 개별 익스포트

**대상 스킬**: 게임개발_프리셋_파일_익스포트

**"presetId"**: `Object_Player_Gun`

**업무**
- 문자열 ID 단건으로 export하고 Unity 재임포트를 완료한다.
- `confirmed`·`reuse`는 변경하지 않는다.
- `유니티엔진_씬_검증`은 사용자 지시에 따라 실행하지 않는다.
