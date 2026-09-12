# 업무지시서

## 1. Idle_Gun 누락 원인 조회

**대상 스킬**: 게임개발_프리셋_파일_질문

**"question"**: `Object_Player_Gun`의 `SpriteAnimPlayer`가 `Idle_Gun`을 찾지 못하는 원인을 프리셋·클립·참조 정의처에서 확인한다.

**업무**
- `Object_Player_Gun`과 정상 플레이어 프리셋의 애니메이션 목록·클립 이름·직렬화 참조를 대조한다.
- `Object_PlayerBase.OnSpawned`가 요청하는 액션명과 `SpriteAnimPlayer.GetClip`의 조회 규칙을 확인한다.
- 수정 대상이 프리팹 배선인지 코드 로직인지 실측값으로 확정한다.

## 2. Object_Player_Gun 애니메이션 배선 복구

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_구성

**"content"**: 원인 조회에서 확인한 `Object_Player_Gun`의 `Idle_Gun` 누락 배선을 기존 총기 애니메이션 리소스로 복구한다.

**업무**
- 현재 프리팹 조회값을 보존한 부분 수정으로 누락된 클립 참조를 복구한다.
- 기존 `Idle_Gun` 리소스와 액션명을 재사용하고 새 애니메이션을 임의 생성하지 않는다.
- 수정 뒤 프리팹 상세 조회에서 `Idle_Gun` 액션과 실재 클립 참조를 확인한다.

## 3. 변경 프리셋 개별 익스포트

**대상 스킬**: 게임개발_프리셋_파일_익스포트

**"presetId"**: `Object_Player_Gun`

**업무**
- `Object_Player_Gun`을 문자열 ID 단건으로 export하고 Unity 재임포트를 완료한다.
- `confirmed`·`reuse`는 변경하지 않는다.
- 코드 작성·컴파일 체인은 원인 조회에서 스크립트 변경이 필요 없다고 확인될 때 제외한다.
- `유니티엔진_씬_검증`은 사용자 지시에 따라 실행하지 않는다.
