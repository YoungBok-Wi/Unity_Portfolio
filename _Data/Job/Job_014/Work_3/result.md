# [게임개발_모듈_폴더_작성] "공격 FSM 최소 재공격 지연 적용" 업무 레포트
## 요약
플레이어별 기존 `AttackInterval`을 공격 시작 시 절대 시각 쿨다운으로 기록했다. 공격 직후 이동·점프로 상태를 벗어나 다시 공격해도 남은 간격이 끝나기 전에는 공격 FSM에 재진입할 수 없다.

## 완료업무

### 공격 간격 정본과 소비 범위 조회
**산출물**
`Assets/__Game/PlayerCharacter/module.md`
`_Data/Concept/Balance/concept.md`
**작업내용**
- 정본 공격 주기는 Knife 0.5s, Gun 0.25s이며 `Character.AttackInterval`의 단위는 초다.
- `Object_PlayerBase.AttackInterval()`은 `LocalGameManager.GetPlayerAttackInterval`을 통해 `AttackSpeed` 배율을 반영한다.
- `Object_Player_Knife`와 `Object_Player_Gun`이 같은 `Object_PlayerBase`와 `FSMState_PlayerAttack`을 사용한다.

### 생명주기 공격 쿨다운 작성
**산출물**
`Assets/__Game/PlayerCharacter/Script/Object_PlayerBase.cs`
**작업내용**
- 공격 시작 시 `Time.time + max(0.01s, AttackInterval())`을 다음 공격 가능 시각으로 저장한다.
- `IsAttackReady()`가 현재 시각과 다음 공격 가능 시각을 함께 검사한다.
- 스폰 시 값을 0으로 초기화해 첫 공격은 즉시 가능하며 피격·사망·상태 전환은 값을 초기화하지 않는다.

### 모듈 익스포트와 컴파일
**산출물**
`PlayerCharacter`
**작업내용**
- `module_manage export` 결과 `success: true`를 확인했다.
- `AssetDatabase.Refresh()` 결과 `success: true`, `result: true`를 확인했다.
- 최종 재컴파일 결과는 `status: completed`, `failed: false`, 에러 로그 0건이다.

## 비고
- `유니티엔진_씬_검증`은 사용자 지시에 따라 실행하지 않았다.
