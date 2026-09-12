# [프리셋] "전체 UI 버튼 Animator_Button 전환 통일" 업무 레포트

## 요약
- 버튼 보유 프리셋을 전수 조회해 누락 15곳을 `Button.m_Transition: Animation`과 `Animator_Button`으로 통일했다.
- 변경한 컨트롤 3종과 팝업 6종의 개별 익스포트·재임포트가 모두 성공했다.

## 완료업무

### 버튼 프리셋 전수 조회
**산출물**
`Assets/_Library/Popup/Animator/Animator_Button.controller`
**작업내용**
- 라이브러리 `Control_Button_Box`, `Control_Button_DealFrame`, `Control_Button_Icon`, `Control_Button_Text`와 `Addon_Frame__Button_Close`는 이미 `Animation`·`Animator_Button` 상태였다.
- 게임 컨트롤 3곳과 게임 팝업 raw 버튼 12곳에서 `Animator` 누락을 확인했다.
- 오브젝트 프리셋 10종과 나머지 애드온에는 `Button` 대상이 없었다.

### 게임 컨트롤 버튼 통일
**산출물**
`Assets/__Game/_Core/_UI/Control/Control_AbilityCard/Control_AbilityCard.prefab`
`Assets/__Game/_Core/_UI/Control/Control_GameFrame/Control_GameFrame.prefab`
`Assets/__Game/_Core/_UI/Control/Control_RoomChoice/Control_RoomChoice.prefab`
**작업내용**
- `Control_AbilityCard` 루트, `Control_GameFrame.CloseButton`, `Control_RoomChoice` 루트에 `Animator_Button`을 연결했다.
- 세 버튼 모두 재조회에서 `m_Transition: Animation`, `m_UpdateMode: Unscaled Time`을 반환했다.

### 게임 팝업 버튼 통일
**산출물**
`Assets/__Game/_Core/_UI/Popup/Popup_Ability/Popup_Ability.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_HUD/Popup_HUD.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Lobby/Popup_Lobby.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Pause/Popup_Pause.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Result/Popup_Result.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Setting/Popup_Setting.prefab`
**작업내용**
- `Ability.Reroll`, `HUD.Pause`, `Lobby` 4곳, `Pause` 3곳, `Result.Confirm`, `Setting` 2곳을 변경했다.
- 12곳 모두 재조회에서 `m_Transition: Animation`과 `Animator_Button.controller`를 반환했다.
- 기존 `m_TargetGraphic`, 이미지, 레이아웃, `UIWrapper_Button` 배선을 보존했다.

### 익스포트와 재임포트
**산출물**
`Assets/__Game/_Core/_UI/Control`
`Assets/__Game/_Core/_UI/Popup`
**작업내용**
- 변경 컨트롤 3종과 팝업 6종을 문자열 ID로 각각 익스포트했고 전 응답은 `success: true`였다.
- `AssetDatabase.Refresh()`는 `success: true`, `result: true`, 진단 0건이었다.
- 열린 `Scene_Lobby`는 재임포트 전에 `isDirty: false`였다.

## 비고
- `confirmed`·`reuse`는 변경하지 않았다.
- `DefaultFont.asset`과 `Scene_Lobby.unity`의 기존 작업 트리 변경은 본 Work 범위에서 수정·커밋하지 않는다.
