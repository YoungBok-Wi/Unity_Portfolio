# [게임개발_프리셋_파일_오브젝트_구성] "Idle_Gun 애니메이션 배선 복구" 업무 레포트

## 요약
`Object_Player_Gun`의 총기 전용 대기·이동 요청명과 프리팹 클립명을 `Idle_Gun`·`Move_Gun`으로 일치시켰다. 프리팹 상세 조회에서 두 액션의 실재 프레임과 `Fps: 10`을 확인했고, 단건 export와 Unity 재임포트가 성공했다.

## 완료업무

### Idle_Gun 누락 원인 조회
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Gun/Script/Object_Player_Gun.cs`
`Assets/__Game/PlayerCharacter/Script/Object_PlayerBase.cs`
`Assets/__Game/Unit/Script/SpriteAnimPlayer.cs`
**작업내용**
- `Object_PlayerBase.OnSpawned`는 `Idle`을 요청하고, `Object_Player_Gun.ResolveAnim`은 이를 `Idle_Gun`으로 변환한다.
- `SpriteAnimPlayer.GetClip`은 액션명을 정확히 일치시켜 조회한다.
- 수정 전 `prefab_object get`에서 총기 프레임을 가진 클립명이 `Idle`·`Move`로 확인되어 프리팹 배선 불일치를 원인으로 확정했다.

### Object_Player_Gun 애니메이션 배선 복구
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
`Assets/__Game/_Core/_Object/Object_Player_Gun/object.json`
**작업내용**
- `SpriteAnimPlayer.m_Clips`의 기존 전체 조회값을 보존해 `Idle`을 `Idle_Gun`, `Move`를 `Move_Gun`으로 변경했다.
- 변경 후 `prefab_object get`에서 `Idle_Gun` 4프레임, `Move_Gun` 6프레임, 각 `Fps: 10`과 기존 `Jump`·`Attack_Gun`·`Hit`·`Die` 클립을 확인했다.
- `preset_manage get`에서 `reuse: add`, `confirmed: {}`, `inAsset: true`가 유지된 것을 확인했다.

### 변경 프리셋 개별 익스포트
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
**작업내용**
- `preset_manage export`에 문자열 ID `Object_Player_Gun`을 전달해 `success: true`를 확인했다.
- `AssetDatabase.Refresh` 결과 `success: true`, `result: true`를 확인했다.
- 재임포트 뒤 프리팹 상세 조회에서 `Idle_Gun`·`Move_Gun` 배선을 다시 확인했다.

## 비고
- 대상 — 업무 3의 코드 작성·컴파일 체인. 조건 — 스크립트 변경이 필요 없다고 확인될 때 제외. 실측 근거 — 변경 파일은 프리팹과 메타뿐이며 원인은 기존 `ResolveAnim`과 클립명의 불일치다.
- 대상 — 업무 3의 `유니티엔진_씬_검증`. 조건 — 사용자 지시에 따라 실행하지 않는다. 실측 근거 — 프리팹 조회·export·재임포트만 수행했으며 씬 검증 결과는 없다.
