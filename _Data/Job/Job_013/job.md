# 총기 플레이어 Idle_Gun 애니메이션 배선 복구

## 목표
- `Object_Player_Gun` 스폰 시 `SpriteAnimPlayer.GetClip("Idle_Gun")`이 성공하도록 애니메이션 클립 배선을 복구한다.
- `LocalRoomManager.InitGame()`이 해당 누락 예외 없이 완료되도록 원인 위치에서 수정한다.

## 범위
- `Object_Player_Gun`과 비교 가능한 플레이어 프리셋의 `SpriteAnimPlayer` 클립 목록·클립 이름·참조 경로를 조회한다.
- 누락 또는 잘못된 `Idle_Gun` 프리팹 직렬화 참조를 수정한다.
- 변경 프리셋을 개별 export하고 Unity 재임포트 후 관련 배선을 재조회한다.

## 제약
- 직접 모드로 수행하고 서브에이전트를 만들지 않는다.
- `유니티엔진_씬_검증`은 실행하지 않는다.
- `confirmed`·`reuse`는 변경하지 않는다.
- 기존 사용자 작업 트리 변경을 수정하거나 커밋하지 않는다.
