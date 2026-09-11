# [오케스트레이터_오케스트레이션_실행] "Job_009 넉백 FSM·Face 씬전환 최종 레포트" 업무 레포트

## 요약
- Job 판정: 완료 — 체크리스트 `c01~c06`과 Work 6건이 전부 `Done`입니다.
- 넉백은 플레이어 2종·일반 적 3종의 `FSMState_UnitKnockback`으로 처리하며, 곡선 이동과 종료 후 스턴 `0.1초`를 `LocalGameManager` 인스펙터에서 설정합니다.
- Face 전환은 `84개` 타일 전부가 얼굴 스프라이트를 종횡비 유지로 표시하고 대각선 순서로 팝·소멸합니다.
- 모듈·프리셋 익스포트와 컴파일은 성공했으며, 씬 런타임 검증은 사용자 지시에 따라 제외했습니다.

## 완료업무

### 넉백 FSM과 밸런스 설정
**산출물**
`Assets/__Game/Unit/Script/FSMState_UnitKnockback.cs`
`Assets/__Game/Game/Script/LocalGameManager.cs`
`_Data/Concept/Balance/concept.md`
**작업내용**
- 넉백 곡선 이동·연속 피격 재시작·종료 후 스턴·정상 상태 복귀 책임을 FSM 상태로 이전했습니다.
- 제거된 `Battle_HitStunSec` 참조를 `LocalGameManager.KnockbackStunSec` 계약으로 정정했습니다.

### 유닛 프리팹 상태 배선
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/Object_Player_Knife.prefab`
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Apple/Object_Enemy_Apple.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Watermelon/Object_Enemy_Watermelon.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Banana/Object_Enemy_Banana.prefab`
**작업내용**
- 넉백 가능한 플레이어와 일반 적의 FSM 배열에 `Knockback` 상태를 한 개씩 연결했습니다.
- 보스 2종은 `IsKnockbackImmune=true` 계약을 유지했습니다.

### Face 타일 연출과 리소스
**산출물**
`Assets/__Game/Game/Script/SceneChangeAni_Face.cs`
`Assets/__Game/_Core/Prefab/[Global].prefab`
`Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png`
`_Data/Resource/File/Icon_Casual_Face/type.json`
**작업내용**
- 타일 `84개`에 얼굴 Sprite·`preserveAspect=true`와 대각선 오버슈트 팝 순서를 적용했습니다.
- 얼굴 리소스 타입을 실측 높이 `252px`, `fillRatio=0.984375`, GUID `9b7955a211788da4a90792cebbf2e24b`에 맞췄습니다.

### 씬 반영과 저장
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Game.unity`
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
**작업내용**
- `Scene_Game`의 플레이어 2종을 `[Stage]` 아래 위치 `(0, -2.4, 0)`·비활성으로 배치하고 `LocalPlayerCharacterManager.m_Players`를 연결했습니다.
- `Scene_Game` 카메라 `4`, `Scene_Lobby` 카메라 `6.5`, Face 원본 참조와 `[Global]` 의미 있는 오버라이드 `0건`을 보존해 저장했습니다.

## 비고
- 건너뜀 — 대상: `Work_4` 업무 4 `유니티엔진_씬_검증`; 조건: 사용자 지시 "검증이 의미없어 다른 곳에서 삭제했으므로 해당 스킬 실행 없이 진행"; 실측 근거: DataMCP 응답 `존재하지 않는 스킬: 유니티엔진_씬_검증`과 원본 스킬 폴더 없음.
- `confirmed`와 `reuse`는 변경하지 않았고, 기존 사용자 변경 `DefaultFont_Bold.asset`과 `ProjectSettings` 3개 파일은 보존했습니다.
- 작업패턴 학습은 자율 진행 지시에 따라 전부 넘어갔습니다.
- Claude 요금제가 작업 중 종료되어 남은 작업은 GPT-5.6 Sol로 진행했습니다.
