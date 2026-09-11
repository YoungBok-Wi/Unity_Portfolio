# [게임개발_모듈_폴더_작성] "공격 이펙트 후 이동·점프 입력 전환" 업무 레포트

## 요약
- Knife는 공격 판정 프레임, Gun은 투사체 생성 직후에 공격 실행 완료를 기록하도록 구현했다.
- 공격 실행 완료 뒤 이동 입력은 `Move`, 점프 입력은 `Jump` 상태로 전환하며 입력이 없으면 공격 상태를 유지한다.
- `PlayerCharacter` 모듈 export는 성공했지만 플레이어 오브젝트 2종의 일괄 export 절차 결함으로 컴파일 전 작업을 중단했다.

## 완료업무

### 공격 후 입력 전환 구현
**산출물**
`Assets/__Game/PlayerCharacter/Script/Object_PlayerBase.cs`
`Assets/__Game/PlayerCharacter/Script/FSMState_PlayerAttack.cs`
`Assets/__Game/_Core/_Object/Object_Player_Knife/Script/Object_Player_Knife.cs`
`Assets/__Game/_Core/_Object/Object_Player_Gun/Script/Object_Player_Gun.cs`
**작업내용**
- `Object_PlayerBase`에 공격 실행 완료 상태와 `CommitAttack` 계약을 추가했다.
- Knife는 `HitBox` 실행 뒤, Gun은 `Fire` 실행 뒤 공격 완료를 기록한다.
- 완료 전 점프 입력은 소비하고, 완료 뒤에는 점프를 이동보다 우선해 상태를 전환한다.
- `module_manage export`의 `Game/PlayerCharacter` 응답은 `success:true`다.

## 비고
- 프리셋 export와 Unity 재컴파일은 수행되지 않았다.
- 게임 컨셉의 기존 공격 취소 불가 문구는 새 요구와 충돌하며 보완 대상이다.

## 예외상황
- 대상 — `게임개발_프리셋_파일_익스포트`, `Object_Player_Knife`·`Object_Player_Gun` 일괄 export
- 막힌 지점 — `preset_manage export`의 `prefabId` 목록 입력 처리
- 에러 원문 — `TypeError [ERR_INVALID_ARG_TYPE]: The "path" argument must be of type string. Received an instance of Array`
- 문서 불일치 — 스킬 입력은 복수 대상이면 목록으로 지정하라고 하지만 실제 도구는 배열을 거부한다.
- 확인 요청 — 두 프리셋을 `prefabId` 문자열로 각각 export하는 우회 허용 여부가 필요하다.
