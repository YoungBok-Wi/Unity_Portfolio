# [게임개발_프리셋_파일_오브젝트_구성] "넉백 FSM 프리셋 배선" 업무 레포트

## 요약
- 넉백 가능한 플레이어 2종과 일반 적 3종의 원본·엔진 프리팹에 `FSMState_UnitKnockback`을 추가하고 FSM 상태 참조를 연결했습니다.
- 보스 2종은 `IsKnockbackImmune=true` 계약에 따라 변경하지 않았고, `SceneChangeAni_Face`는 모듈 소유 프리팹의 기존 얼굴 Sprite 배선을 유지했습니다.
- 변경 프리셋 5종의 익스포트와 Unity 재임포트가 모두 성공했으며, 재조회한 FSM 배열에 상태 중복이 없고 Missing 스크립트·무효 GUID 패턴이 없습니다.

## 완료업무

### 플레이어 프리팹 넉백 상태 배선
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/Object_Player_Knife.prefab`
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
**작업내용**
- 두 FSM을 `Idle·Move·Jump·Attack·Hit·Knockback·Die` 순서로 구성하고 기본 상태 `Idle`을 유지했습니다.
- 각 `States` 오브젝트에 ID가 `Knockback`인 `FSMState_UnitKnockback`을 한 개씩 추가했습니다.

### 일반 적 프리팹 넉백 상태 배선
**산출물**
`Assets/__Game/_Core/_Object/Object_Enemy_Apple/Object_Enemy_Apple.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Watermelon/Object_Enemy_Watermelon.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Banana/Object_Enemy_Banana.prefab`
**작업내용**
- 세 FSM을 `Move·Attack·Knockback·Die` 순서로 구성하고 기본 상태 `Move`를 유지했습니다.
- 각 `States` 오브젝트에 ID가 `Knockback`인 `FSMState_UnitKnockback`을 한 개씩 추가했습니다.

### 프리셋 메타데이터와 엔진 사본 반영
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/object.json`
`Assets/__Game/_Core/_Object/Object_Player_Gun/object.json`
`Assets/__Game/_Core/_Object/Object_Enemy_Apple/object.json`
`Assets/__Game/_Core/_Object/Object_Enemy_Watermelon/object.json`
`Assets/__Game/_Core/_Object/Object_Enemy_Banana/object.json`
**작업내용**
- 5개 프리셋 설명에 넉백 상태 배선을 반영하고, 각 `preset_manage export` 결과가 `success=true`임을 확인했습니다.
- Unity 재임포트 후 5개 FSM을 재조회했고 Missing 스크립트와 무효 GUID 패턴이 `0건`임을 확인했습니다.

## 비고
- `confirmed`와 `reuse` 설정은 변경하지 않았습니다.
- `SceneChangeAni_Face`의 84개 타일 얼굴 Sprite 배선은 `Work_2` 산출물의 모듈 소유 범위이므로 변경 없이 검증했습니다.

## 예외상황
- `Balance` 컨셉은 스턴 설정 소스를 제거된 고정 상수 `Battle_HitStunSec=0.1`로 설명하지만 실제 계약은 `LocalGameManager.m_KnockbackStunSec=0.1`입니다. 보완 Work에서 정본을 수정해야 합니다.
- `Icon_Casual_Face` 리소스 타입 설명은 결과 크기·점유율을 `232px·0.90625`로 기록하지만 실제 리소스는 `252px·0.984375`입니다. 보완 Work에서 타입 정본을 수정해야 합니다.
