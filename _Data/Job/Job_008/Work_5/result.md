# [프리셋] "팝업·캐릭터 프리팹 배선과 익스포트" 업무 레포트

## 요약
- 판정: 실패 — `Object_Enemy_Banana` 기준 이동 프레임이 정본 크기·서열을 위반해 `오브젝트 구성` 3단계에서 중단했다.
- 완료 범위: 팝업 코드·구성, 컨트롤/오브젝트 코드 대조, 컴파일, 캐릭터 7종 프레임·아이콘·FSM·공격 범위 배선.
- 미완료 범위: 결손 리소스 교체, 오브젝트 구성 최종 검증, `Popup`·`Control`·`Object` 익스포트.

## 완료업무

### 팝업 코드와 구성
**산출물**
`Assets/__Game/_Core/_UI/Popup/Popup_HUD/Script/Popup_HUD.cs`
`Assets/__Game/_Core/_UI/Popup/Popup_RoomSelect/Script/Popup_RoomSelect.cs`
`Assets/__Game/_Core/_UI/Popup/Popup_Result/Script/Popup_Result.cs`
`Assets/__Game/_Core/_UI/Popup/Popup_HUD/Popup_HUD.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_RoomSelect/Popup_RoomSelect.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Result/Popup_Result.prefab`
**작업내용**
- `Popup_HUD`는 `LocalGameManager.PlayerChanged` 재배선과 저체력 비네트 알파 0.15~0.45·1초 주기를 반영했다. 비네트는 루트 첫 자식, `Active:false`, 전체 스트레치, 색 `(1,0.25,0.25,1)`이며 `m_LowHpVignette` 재조회가 `LowHpVignette (CanvasGroup)`을 반환했다.
- `Popup_RoomSelect`는 `Frame/ControlRoot`의 `HorizontalLayoutGroup`과 `m_ChoiceLayout`을 배선했다. 재조회 값은 `MiddleCenter`, 간격 80, 선택지 2개다.
- `Popup_Result`는 승패 표시 코드와 `ResultLabel` 노드를 제거했다. 재조회에는 방 순번·Crumb 총량·해금·확인 항목만 남았다.
- 7개 게임 팝업의 `CanvasScaler` CLI 재조회가 모두 `ScaleWithScreenSize`, `1920×1080`, `Expand`, match 0을 반환했다. 프레임형 5종은 `Blocker (CanvasGroup)` 알파 0→1/1→0과 `Frame (RectTransform)` z -14→0/0→-14 애드온을 반환했다.
- 시각 대조 기준은 `_Data/Concept/Resource/concept.md`의 1920×1080 규격으로 환산 배율 1이다. 구도는 HUD 배경 비네트·선택지 중앙 정렬, 크기 비율은 전체 스트레치·프레임 피벗 `(0.5,0.5)`, 색은 붉은 틴트, 스타일은 기존 `Game` 프레임 유지, 텍스처 반복은 대상 없음으로 확인했다. 신규 라벨은 없고 `ResultLabel`만 제거했다.

### 컨트롤·오브젝트 코드와 컴파일
**산출물**
`Assets/__Game/_Core/_UI/Control/Control_RoomChoice/Script/Control_RoomChoice.cs`
`Assets/__Game/_Core/_UI/Control/Control_EnemyPreview/Script/Control_EnemyPreview.cs`
`Assets/__Game/_Core/_Object/Object_Player_Knife/Script/Object_Player_Knife.cs`
**작업내용**
- 컨트롤은 제공 아이콘만 표시하고 `Popup_RoomSelect`가 `LocalGameManager.GetUnitIcon`을 소비하는 구조라 변경하지 않았다. 신규 직렬화 필드가 없어 컨트롤 구성도 건너뛰었다.
- 오브젝트 스크립트 7종은 새 베이스·공격 훅·반전 규칙을 이미 충족해 변경하지 않았다. `module-Object` 템플릿 테스트는 7건 모두 `errors:[]`이었다.
- 컴파일은 실제 수행 결과 `completed`, `failed:false`, 오류 로그 0건이었다.

### 캐릭터 프리팹 배선
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/Object_Player_Knife.prefab`
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Apple/Object_Enemy_Apple.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Watermelon/Object_Enemy_Watermelon.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Banana/Object_Enemy_Banana.prefab`
`Assets/__Game/_Core/_Object/Object_Boss_Pumpkin/Object_Boss_Pumpkin.prefab`
`Assets/__Game/_Core/_Object/Object_Boss_Pineapple/Object_Boss_Pineapple.prefab`
**작업내용**
- 7종 `SpriteAnimPlayer.m_Clips`를 실제 `Assets/__Game/_Core/SpriteAnim/` 스프라이트로 배선했다. Knife 공격 클립은 `Attack_Knife 04~06`, `Attack2 01~06`, `Attack3 02~06` 순서라 완전히 내린 자세가 각 2번째 프레임이다.
- 플레이어 2종은 `States`의 Idle·Move·Jump·Attack·Hit·Die 6컴포넌트, 루트 `FSM`, `m_Fsm` 참조를 재조회했다. Knife `AttackRange`는 `Active:false`, trigger, offset `(1,0.5)`, size `(2,1.5)`다.
- 적·보스 5종은 기존 `m_Id`·`m_Physics`·`m_Anim`·`m_Fsm`·`m_HitHeight`가 유지됐고, 적 Move 01·보스 Idle 01 아이콘 참조를 재조회했다. 풀 대상 적·보스 루트는 `Active:false`, 플레이어 루트는 활성 상태다.
- 불투명 높이 실측은 플레이어·보스가 정본 허용 범위이며 PPU 128이다. 기준 프레임은 Knife Idle 128px, Gun Idle 127px, Apple Move 113px, Watermelon Move 138px, Pumpkin Idle 225px, Pineapple Idle 224px다.

## 비고
- `Popup_HUD`·`Popup_Lobby`는 하단 고정 0, 동적 프레임형은 -1이다. 취소 소비 주체는 `Scene_Game`의 `Popup_Pause`(`m_IsCloseByCancel:true`)와 `Scene_Lobby`의 `Popup_Quit`(`true`)로 정본과 일치한다.
- `Popup_Notify`·`Popup_Quit` 등 `Assets/_Library/**`는 조회만 했고 수정하지 않았다. `confirmed`·`reuse`도 변경하지 않았다.

## 예외상황
- 대상: `Assets/__Game/_Core/SpriteAnim/AnimationSheet_Casual_Enemy_Banana_Move_01.png`.
- 실측: `opaque=109px`, `ppu=128`, `h=0.8515625u`.
- 정본: `_Data/Concept/Resource/concept.md`는 Banana 기준 123px과 `Apple =113px < Banana=123px < Watermelon=138px` 서열을 요구한다.
- 막힌 지점: `게임개발_프리셋_파일_오브젝트_구성` 3단계는 원본 규격 불일치를 PPU·프리팹 값으로 덮지 말고 리소스 결손으로 보고하도록 규정한다. 보완 Work에서 Banana Move 01을 교체한 뒤 오브젝트 검증과 3종 프리셋 익스포트를 수행해야 한다.
