# 넉백 Hit 클립 예외와 타격 이펙트 중단 수정

## 목적

`FSMState_UnitKnockback.OnStart`가 `Hit` 애니메이션 클립이 없는 유닛에서 예외를 발생시키는 원인을 수정하고, 예외로 중단된 타격 이펙트 흐름을 복구한다.

## 사용자 보고

- `InvalidOperationException: View : Hit 애니메이션 클립이 없다`
- 호출 경로: `FSMState_UnitKnockback.OnStart` → `Object_UnitBase.TakeHit` → `LocalGameManager.Hit`
- 증상: 넉백 피격 시 타격 이펙트가 표시되지 않는다.

## 범위

- `Unit` 모듈의 넉백 상태와 `SpriteAnimPlayer` 클립 조회 계약을 대조한다.
- 플레이어·일반 적 프리셋의 애니메이션 구성과 타격 이펙트 호출 순서를 확인한다.
- 공용 원인 한 곳을 최소 수정하고 모듈 익스포트·Unity 컴파일을 확인한다.

## 제약

- 실행 모드는 `직접`이며 서브에이전트를 만들지 않는다.
- 기존 사용자 변경 `README.md`, `DefaultFont_Bold.asset`, `ProjectSettings` 3개 파일을 보존한다.
- `confirmed`·`reuse`는 변경하지 않는다.
