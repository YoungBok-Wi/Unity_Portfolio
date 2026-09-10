# 업무지시서

## 1. 프리셋 현황 조회

**대상 스킬**: 게임개발_프리셋_파일_질문

**"question"**: 팝업 7종(`Popup_HUD`·`RoomSelect`·`Ability`·`Pause`·`Result`·`Setting`·`Lobby`) 노드 구조·Canvas Scaler·연출 애드온 유무, 컨트롤 `Control_RoomChoice` 구조, 오브젝트 10종(플레이어 2·적 3·보스 2·투사체·배경·바닥) 노드·컴포넌트·FSM 자식·`SpriteAnimPlayer` 배선

**업무**

- 목표: 수정 대상 노드·필드 목록 확정 (`Popup_Notify` 프리팹의 애드온 배치·타깃 값을 기준 예시로 기록)

## 2. 컨셉 근거 조회

**대상 스킬**: 게임개발_구성_컨셉_질문

**"question"**: `리소스컨셉` "UI" 팝업 연출·Canvas Scaler 규격, `밸런스컨셉` 저체력 깜빡임 주기·알파, `게임컨셉` 방 선택(단일 선택지)·결과 팝업 항목

**업무**

- 목표: 팝업 배선 값의 정본 확인

## 3. 모듈 API 조회

**대상 스킬**: 게임개발_모듈_질문

**"question"**: Work_4 결과 모듈(Game·Data·Room·RoomSelect·PlayerCharacter·Unit) 공개 API — `LocalGameManager.Player`·`HitApplied`·`GetUnitIcon`, `LocalRoomSelectManager.Choices`·`SelectRoom`, `LocalRoomManager.State`, `DataManager` 값, `SpriteAnimPlayer` 클립 구조·`Object_PlayerBase` 공격 훅·`Object_UnitBase.Icon`

**업무**

- 목표: 팝업·오브젝트 스크립트가 참조할 API 확정

## 4. 리소스 조회

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `Assets/__Game/_Core/SpriteAnim/` 프레임 목록(동작별 파일·순서), `UI_Common_Shape_Vignette`·`Icon_Casual_Face_Chef` 에셋 경로

**업무**

- 목표: `SpriteAnimPlayer` 클립 배선에 쓸 프레임 경로 표 (플레이어 `Attack_Knife`·`Attack2`·`Attack3`은 각 프레임을 열어 "칼을 완전히 내린 자세" 프레임을 표에 표시)

## 5. 팝업 코드 작성

**대상 스킬**: 게임개발_프리셋_파일_팝업_코드_작성

**"prefabId"**: Popup_HUD, Popup_RoomSelect, Popup_Result, Popup_Lobby, Popup_Pause, Popup_Ability (대상만 달리해 — 변경이 있는 것만)

**"content"**: HUD 저체력 비네트 깜빡임, RoomSelect 단일 선택지 표시, Result 승패 삭제, 매니저 참조명 교체

**업무**

- `Popup_HUD`: `[SerializeField] CanvasGroup m_LowHpVignette` 추가, 플레이어 `Hp`·`MaxHp` 구독으로 비율 < `TableManager.instance.Const.Battle_LowHpRatio`이면 활성 후 알파를 `Mathf.PingPong` 주기 1.0s로 0.15~0.45 왕복(unscaled), 아니면 비활성. 플레이어 참조는 `LocalGameManager.Player` 변경 통지(활성 전환 시 재구독)
- `Popup_RoomSelect`: `Choices.Count`가 1이면 두 번째 카드 비활성·첫 카드 중앙 정렬(`[SerializeField] RectTransform m_ChoiceRoot` 정렬 또는 레이아웃 그룹), `LocalRoomSelectManager` 참조로 교체
- `Popup_Result`: 승패 문구·분기 삭제, 도달 순번·Crumb 총량·Gun 해금 알림만, 확인 시 `SceneChange(Lobby, "Face")`는 Room/Game이 담당하면 호출만
- `Popup_Lobby`·`Popup_Pause`·`Popup_Ability`·`Popup_Setting`: `CharacterManager`→`DataManager`, `LocalBattleManager`→`LocalGameManager` 등 참조명 교체 외 로직 변경 없음 (Work_4에서 이미 바꿨으면 건너뛰고 사유)
- 완료 기준: 각 스크립트 템플릿 verify `success:true`

## 6. 컨트롤 코드 작성

**대상 스킬**: 게임개발_프리셋_파일_컨트롤_코드_작성

**"prefabId"**: Control_RoomChoice, Control_EnemyPreview (대상만 달리해 — 변경이 있는 것만)

**"content"**: 미리보기 아이콘을 `LocalGameManager.GetUnitIcon(id)`로 조회 (`RoomUtil.LoadUnitIcon` 삭제 대응), `SRoomChoice.Variant` 무관

**업무**

- 완료 기준: 템플릿 verify `success:true` (변경 없으면 건너뛰고 사유)

## 7. 오브젝트 코드 작성 (2D 사이드뷰 캐릭터)

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_코드_2D_사이드뷰_캐릭터_작성

**"prefabId"**: Object_Player_Knife, Object_Player_Gun, Object_Enemy_Apple, Object_Enemy_Watermelon, Object_Enemy_Banana, Object_Boss_Pumpkin, Object_Boss_Pineapple (대상만 달리해 7회)

**"content"**: 플레이어 2종은 Work_4 `Object_PlayerBase` 공격 훅 구현(Knife 2번째 프레임 판정·단계별 궤적 `PlaySlashEffect(step)`·3단 마무리 넉백, Gun 유지 연사), 적·보스 5종은 `Object_EnemyBase`·`Object_BossBase` 파생으로 교체 (좌우 반전은 `SetFacing` → `SpriteAnimPlayer.SetFlip` 경로 유지)

**업무**

- `Object_Player_Knife`: `OnAttackFrame(step, frame)`에서 `frame == 1`일 때 1회 `HitBox`(단별 피해·넉백, `SHit.Step`), `OnAttackStart(step)`에서 `PlayAttackSfx`·`PlaySlashEffect(center, Facing, step)`·판정 범위 활성, `OnAttackEnd`에서 비활성
- `Object_Player_Gun`: 유지 입력 동안 `AttackInterval` 주기 발사(기존), 공격 상태 종료는 입력 해제
- Work_4에서 이미 반영된 부분은 건너뛰고 사유를 남긴다
- 완료 기준: 7건 템플릿 verify `success:true`

## 8. 컴파일

**대상 스킬**: 유니티엔진_컴파일_실행

**"changedPaths"**: Assets/__Game/_Core/**

**업무**

- 완료 기준: 에러 0, 실제 컴파일 수행

## 9. 팝업 구성

**대상 스킬**: 게임개발_프리셋_파일_팝업_구성

**"prefabId"**: Popup_HUD, Popup_RoomSelect, Popup_Ability, Popup_Pause, Popup_Result, Popup_Setting, Popup_Lobby (대상만 달리해 7회)

**"content"**: Canvas Scaler 7종 공통(`uiScaleMode` Scale With Screen Size, 1920x1080, `screenMatchMode` Expand), 프레임형 5종 연출 애드온, HUD 비네트 노드, RoomSelect 단일 선택지 배선

**업무**

- 프레임형 5종(`Ability`·`Pause`·`Result`·`RoomSelect`·`Setting`): `Popup_Notify`와 같은 구성 — `PopupAni_Alpha_Smooth` 인스턴스(타깃 Blocker `CanvasGroup`, 없으면 Blocker에 `CanvasGroup` 추가; 열기 0→1·닫기 1→0), `PopupAni_Rotation_Dynamic` 인스턴스(타깃 프레임 루트 `Control_Frame`(또는 프레임 역할 노드) Transform, openStart z -14 → openEnd 0, closeStart 0 → closeEnd z -14). `Popup_Setting`의 기존 `PopupAni_Alpha_Smooth`는 타깃을 Blocker로 맞춘다. 프레임 회전 중심은 프레임 피벗 (0.5,0.5)
- `Popup_HUD`: 루트 아래 `LowHpVignette`(Image `UI_Common_Shape_Vignette`, 스트레치 전체, 색 (1, 0.25, 0.25), raycast off, `CanvasGroup` alpha 0, 비활성)를 HP 게이지보다 뒤(그리기 순서 앞)에 두고 `m_LowHpVignette` 배선
- `Popup_RoomSelect`: 단일 선택지 정렬 필드 배선
- 완료 기준: 7건 verify `success:true`, 프리팹 YAML에서 `m_UiScaleMode: 1`·`m_ScreenMatchMode: 1`·`m_ReferenceResolution: {x: 1920, y: 1080}` 확인

## 10. 컨트롤 구성

**대상 스킬**: 게임개발_프리셋_파일_컨트롤_구성

**"prefabId"**: Control_RoomChoice (변경이 있는 것만)

**"content"**: 6의 신규 필드 배선 (없으면 건너뛰고 사유)

**업무**

- 완료 기준: verify `success:true`

## 11. 오브젝트 구성

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_구성

**"prefabId"**: Object_Player_Knife, Object_Player_Gun, Object_Enemy_Apple, Object_Enemy_Watermelon, Object_Enemy_Banana, Object_Boss_Pumpkin, Object_Boss_Pineapple (대상만 달리해 7회)

**"content"**: `SpriteAnimPlayer` 클립 배열(동작별 프레임 스프라이트, `Assets/__Game/_Core/SpriteAnim/`), `Icon` 배선(적 Move_01·보스 Idle_01), 플레이어 FSM 자식 상태 노드 6종·`FSM` 컴포넌트 배선, Knife `AttackRange` 노드(반전 루트 자식 `BoxCollider2D` isTrigger, 폭 2.0u × 높이 1.5u, 중심 전방 1.0u·`HitHeight`), 적·보스 스크립트 교체 후 FSM·물리 참조 재배선

**업무**

- 플레이어 `Attack_Knife`·`Attack2`·`Attack3` 클립: 4의 표에서 "칼을 완전히 내린 자세"가 2번째 프레임이 되도록 앞 준비 프레임을 뺀 순서로 배선(프레임 파일은 삭제하지 않는다), 나머지 동작은 전체 프레임 순서대로
- 프리팹 스크립트 타입 교체(`Object_Enemy_* : Object_EnemyBase` 등)로 직렬화 필드가 유지되는지(`m_Id`·`m_Physics`·`m_Anim`·`m_Fsm`·`m_HitHeight`) 확인하고 빠진 배선을 복구한다
- 완료 기준: 7건 verify `success:true`, 프리팹 YAML에 `m_Clips` 프레임 GUID 배선·Missing 스크립트 0

## 12. 프리셋 익스포트

**대상 스킬**: 게임개발_프리셋_파일_익스포트

**"prefabType"**: Popup, Control, Object (대상만 달리해 3회)

**업무**

- 완료 기준: export 응답 `success:true`, 재임포트 완료, Missing 참조 0
- `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`) 수정 금지 — `Popup_Notify`·`Popup_Quit` 등 라이브러리 팝업의 Canvas Scaler는 손대지 않고 현재값을 레포트에 기록만 한다. DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
