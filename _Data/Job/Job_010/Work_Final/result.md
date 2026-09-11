# [오케스트레이터_오케스트레이션_실행] "넉백 Hit 클립 예외와 타격 이펙트 중단 수정" 업무 레포트

## 요약
- 체크리스트 `c01`을 완료했다.
- 일반 적에게 없는 `Hit` 클립을 넉백 상태가 요구하던 예외를 제거했다.
- 명중 처리의 예외 중단이 해소되어 `PlayHitEffect`와 타격 SFX 호출이 이어진다.
- Unity 컴파일은 `status:completed`·`failed:false`, 콘솔 에러는 0건이다.

## 완료업무

### 넉백과 명중 연출 복구
**산출물**
`Assets/__Game/Unit/Script/FSMState_UnitKnockback.cs`
`Assets/__Game/Unit/module.md`
`_Data/Job/Job_010/Work_1/result.md`
**작업내용**
- 넉백 상태의 공통 `Hit` 애니메이션 재생을 제거하고 이동·경직 책임만 유지했다.
- 플레이어는 기존 `Object_PlayerBase.OnHit` 경로로 `Hit` 애니메이션을 재생한다.
- 일반 적 프리팹 3종의 실제 클립 구성과 일치하며 `LocalGameManager.PlayHitEffect` 호출을 막던 예외가 사라졌다.
- `module_manage export`와 Unity 재컴파일 검증을 통과했다.
