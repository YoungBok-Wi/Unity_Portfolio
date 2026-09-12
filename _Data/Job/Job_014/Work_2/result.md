# [게임개발_프리셋_파일_질문] "총기 플레이어 점프 외형 복구" 업무 레포트
## 요약
`Object_Player_Gun`의 `Jump` 클립이 칼 캐릭터용 공용 `AnimationSheet_Casual_Player_Jump_01`~`06`을 참조하는 원인을 확정했다. 등록된 `Jump_Gun` 리소스가 없어 지시된 전용 배선은 수행하지 않았다.

## 완료업무

### 총기 점프 클립 원인 조회
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Gun`
**작업내용**
- `Object_Player_Gun`과 `Object_Player_Knife`의 `SpriteAnimPlayer`를 상세 조회했다.
- 두 프리팹의 `Jump` 클립이 같은 공용 `Jump` 6프레임을 참조함을 확인했다.
- `Object_Player_Gun.ResolveAnim`은 `Idle`·`Move`만 Gun 전용 이름으로 바꾸고 `Jump`는 그대로 반환한다.

### 총기 전용 점프 리소스 조회
**산출물**
`AnimationSheet/AnimationSheet_Casual_Player`
**작업내용**
- 등록 목록에 `Idle_Gun`·`Move_Gun`·`Attack_Gun`은 있으나 `Jump_Gun`은 없음을 확인했다.
- `Object_Player_Gun`은 `reuse: add`, `confirmed: {}`, `inAsset: true` 상태를 유지했다.

## 예외상황
- 대상: `AnimationSheet_Casual_Player_Jump_Gun`.
- 결손: 총기 전용 점프 프레임이 등록되어 있지 않다.
- 영향: 지시서가 임의 생성과 공용 프레임 재사용을 금지하므로 `Jump_Gun` 분기·클립 배선·익스포트를 수행할 수 없다.
- 다음 처리: 기존 총기 전용 외형을 점프 동안 유지하는 보완 Work를 생성한다.
