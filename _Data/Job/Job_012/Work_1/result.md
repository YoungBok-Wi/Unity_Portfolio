# [게임개발_프리셋_파일_질문] "전체 버튼 표준 Control_Button 구조 통일" 업무 레포트

## 요약
- Work 판정: 합격 — 게임 `컨트롤` 3종의 버튼과 `팝업` 6종의 버튼 12개를 `Image`·`AddonSlot` 표준 계층으로 통일했다.
- 표준 프리셋 재사용: 박스 버튼 8개는 `Control_Button_Box` 색상 변형과 `Addon_Button_Box__Text`, 아이콘 버튼 2개는 `Control_Button_Icon`, 무기 카드 2개는 표준화한 `Control_AbilityCard`를 사용한다.
- 배선 확인: `prefab_popup get depth:0` 재조회에서 모든 버튼·라벨·카드 이미지·선택·잠금 참조가 새 계층 경로로 확인됐다.
- 반영 확인: 변경 프리셋 9종의 단건 `preset_manage export`가 모두 `success:true`, `AssetDatabase.Refresh()`가 `result:true`였다.
- 설정 보존: `preset_manage get` 재조회에서 `confirmed`·`reuse`·`inAsset`은 기존 값 그대로이며 코드 파일은 변경하지 않았다.

## 완료업무

### 1. 표준 버튼 구조와 적용 대상 조회
**산출물**
`Assets/_Library/_Core/_UI/Control/Control_Button_Box/`
`Assets/_Library/_Core/_UI/Control/Control_Button_Icon/`
`Assets/_Library/_Core/_UI/Addon/Control_Button_Box/Addon_Button_Box__Text/`
**작업내용**
- `prefab_control get`에서 표준 버튼의 루트 `Button`·`Animator`·`UIWrapper_Button`, 자식 `Image`·`AddonSlot` 구조를 확인했다.
- `Animator_Button.controller`의 곡선 대상 경로가 `Image`·`AddonSlot`임을 원본에서 확인했다.
- 게임 프리팹의 `Animator_Button` 직접 참조를 검색해 비표준 대상이 `Control_AbilityCard`·`Control_GameFrame`·`Control_RoomChoice`와 팝업 raw 버튼 12개임을 확정했다.

### 2. 게임 컨트롤 버튼 구조 통일
**산출물**
`Assets/__Game/_Core/_UI/Control/Control_AbilityCard/`
`Assets/__Game/_Core/_UI/Control/Control_GameFrame/`
`Assets/__Game/_Core/_UI/Control/Control_RoomChoice/`
**작업내용**
- `Control_AbilityCard`와 `Control_RoomChoice`는 루트 버튼 아래 `Image`·`AddonSlot`을 두고 기존 아이콘·이름·설명·선택 영역을 슬롯 아래로 옮겼다.
- `Control_GameFrame.CloseButton`은 표시 이미지를 `CloseButton/Image`로 분리하고 빈 `AddonSlot`을 추가했다.
- `prefab_control get` 재조회에서 세 대상의 `Button.m_TargetGraphic`, 전용 컴포넌트 참조, `Animator_Button` 연결과 기존 레이아웃 값이 새 경로로 확인됐다.

### 3. 게임 팝업 버튼 구조 통일
**산출물**
`Assets/__Game/_Core/_UI/Popup/Popup_Ability/`
`Assets/__Game/_Core/_UI/Popup/Popup_HUD/`
`Assets/__Game/_Core/_UI/Popup/Popup_Lobby/`
`Assets/__Game/_Core/_UI/Popup/Popup_Pause/`
`Assets/__Game/_Core/_UI/Popup/Popup_Result/`
`Assets/__Game/_Core/_UI/Popup/Popup_Setting/`
**작업내용**
- raw 버튼을 제거한 뒤 표준 컨트롤 인스턴스를 배치하고, 기존 앵커·크기·스프라이트·라벨 값을 인스턴스 오버라이드로 복원했다.
- `Popup_Lobby` 무기 카드는 `Control_AbilityCard` 인스턴스로 교체하고 `Select`·`Lock`을 `AddonSlot` 아래에 배치했다.
- `prefab_popup info` 재조회에서 12개 대상이 표준 컨트롤 프리팹 인스턴스로 확인됐고, `get depth:0`에서 직렬화 참조가 모두 새 경로를 가리켰다.

### 4. 변경 프리셋 개별 익스포트
**산출물**
`Assets/__Game/_Core/_UI/Control/`
`Assets/__Game/_Core/_UI/Popup/`
**작업내용**
- `Control_AbilityCard`·`Control_GameFrame`·`Control_RoomChoice`와 팝업 6종을 배열 없이 문자열 ID로 각각 export했고 9건 모두 `success:true`였다.
- 열린 `Scene_Game`이 `isDirty:false`임을 확인한 뒤 `AssetDatabase.Refresh()`를 실행했고 응답은 `success:true`·`result:true`였다.
- 건너뛰기 — 대상: 코드 작성·컴파일 체인 / 조건: `order.md`의 “스크립트 계약을 바꾸지 않는 구조 교체이므로 제외” / 실측 근거: 변경 파일 목록에 `.cs`가 없다.
- 건너뛰기 — 대상: `유니티엔진_씬_검증` / 조건: `job.md`의 사용자 제외 지시 / 실측 근거: 프리팹 `info`·`get`과 export·Refresh만 수행했고 씬 검증 호출은 없다.
