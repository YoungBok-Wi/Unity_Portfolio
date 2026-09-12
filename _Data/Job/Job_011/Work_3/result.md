# [프리셋] "전체 프레임형 팝업 4종 애니메이션 통일" 업무 레포트

## 요약
- 게임 프레임형 팝업 5종에 Position·Rotation·Scale·Alpha 애니메이션을 모두 구성했다.
- `Popup_Notify`의 시간·변화량을 기준으로 누락된 Position·Scale을 추가했고, 기존 Rotation·Alpha는 기준값을 유지했다.

## 완료업무

### 기준 팝업과 대상 전수 조회
**산출물**
`Assets/_Library/_Core/_UI/Popup/Popup_Notify/Popup_Notify.prefab`
**작업내용**
- `Popup_Notify`는 Position·Rotation·Scale·Alpha 4종을 모두 보유한다.
- Position은 열기 0.3초·닫기 0.12초·x 860→0, Rotation은 z -14→0, Scale은 0.6→1.0이다.
- Alpha는 열기 0.25초·닫기 0.15초·0→1이며 라이브러리 `Popup_Quit`·`Popup_ChangeResult`도 이미 4종을 보유한다.
- 게임 프레임형 `Ability`, `Pause`, `Result`, `RoomSelect`, `Setting`은 Alpha·Rotation만 있어 Position·Scale이 누락된 상태였다.

### 게임 프레임형 팝업 애니메이션 통일
**산출물**
`Assets/__Game/_Core/_UI/Popup/Popup_Ability/Popup_Ability.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Pause/Popup_Pause.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Result/Popup_Result.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_RoomSelect/Popup_RoomSelect.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Setting/Popup_Setting.prefab`
**작업내용**
- 5종 모두 `PopupAni_Position_Dynamic`과 `PopupAni_Scale_Dynamic` 인스턴스를 추가했다.
- Position·Scale 대상은 각 팝업의 `Frame (RectTransform)`이며 값은 `Popup_Notify`와 동일하다.
- 기존 Rotation은 `Frame`, Alpha는 `Blocker` 대상의 기존 배선을 보존했다.
- 재조회된 다섯 팝업 모두 4종 애니메이션 인스턴스를 반환했다.

### 익스포트와 재임포트
**산출물**
`Assets/__Game/_Core/_UI/Popup`
**작업내용**
- 변경 팝업 5종을 문자열 ID로 각각 익스포트했고 전 응답은 `success: true`였다.
- `AssetDatabase.Refresh()`는 `success: true`, `result: true`, 진단 0건이었다.
- 열린 `Scene_Lobby`는 재임포트 전에 `isDirty: false`였다.

## 비고
- 베이스형 `Popup_HUD`·`Popup_Lobby`와 시스템 표시형 `Popup_BlockingUI`·`Popup_NotifyUI`는 프레임형 대상에서 제외했다.
- `confirmed`·`reuse`와 라이브러리 팝업 원본은 변경하지 않았다.
- `DefaultFont.asset`과 `Scene_Lobby.unity`의 기존 작업 트리 변경은 본 Work 범위에서 수정·커밋하지 않는다.
