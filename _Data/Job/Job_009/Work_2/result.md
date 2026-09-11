# [게임개발_모듈_폴더_작성] "넉백 FSM·설정 노출·Face 타일 배선 구현" 업무 레포트

## 요약
- `Unit`에 `Knockback` FSM 상태를 추가하고 기존 `Object_UnitBase`의 프레임 타이머 넉백 경로를 제거했습니다. 플레이어는 입력에 따라 `Idle·Move`, 일반 적은 스폰 상태인 `Move`로 복귀합니다.
- `LocalGameManager` 인스펙터에 넉백 진행 곡선과 완료 후 스턴 시간 `0.1초`를 노출했습니다.
- `SceneChangeAni_Face`가 84개 타일 모두에 플레이어 얼굴 Sprite와 종횡비 보존을 적용하고, 대각선 순서의 오버슈트 팝 스케일로 표시하도록 구현했습니다.
- 네 모듈의 `module_manage export`가 모두 `success=true`, Unity 재컴파일은 `status=completed`, `failed=false`, 콘솔 로그 `0건`입니다.

## 완료업무

### 넉백 FSM 상태 구현
**산출물**
`Assets/__Game/Unit/Script/FSMState_UnitKnockback.cs`
`Assets/__Game/Unit/Script/Object_UnitBase.cs`
`Assets/__Game/Unit/Script/UnitConst.cs`
`Assets/__Game/PlayerCharacter/Script/Object_PlayerBase.cs`
**작업내용**
- 넉백 곡선 이동·완료 후 스턴·연속 피격 재시작·정상 상태 복귀를 `FSMState_UnitKnockback`이 소유하도록 구성했습니다.
- 기존 `Object_UnitBase.Update·FixedUpdate`의 넉백 이동과 `Battle_HitStunSec` 소비를 제거했습니다.
- 등록된 `Knockback` 상태가 없는 넉백 가능 유닛은 대상 이름이 포함된 예외를 발생시키도록 했습니다.

### 전투 설정 노출
**산출물**
`Assets/__Game/Game/Script/LocalGameManager.cs`
**작업내용**
- 기존 `m_KnockbackCurve`를 상태가 사용하고, `m_KnockbackStunSec` 기본값 `0.1초`를 추가했습니다.

### Face 타일 얼굴 그래픽 배선
**산출물**
`Assets/__Game/Game/Script/SceneChangeAni_Face.cs`
`Assets/__Game/Game/Editor/Script/Setup_Game.cs`
`Assets/__Game/_Core/Prefab/[Global].prefab`
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
**작업내용**
- 각 타일의 `Image.sprite`를 `Icon_Casual_Face_Chef.png`로 설정하고 `preserveAspect=true`를 강제했습니다.
- `[Global].prefab`의 Face 인스턴스 `m_Face`가 Sprite GUID `9b7955a211788da4a90792cebbf2e24b`를 참조하는 것을 확인했습니다.
- `editor_util setup`과 `save_scene`이 성공했습니다.

## 비고
- 보스의 `IsKnockbackImmune=true` 계약은 유지했으며 보스 FSM에는 새 상태를 추가하지 않습니다.
- `confirmed`와 `reuse` 설정은 변경하지 않았습니다.
