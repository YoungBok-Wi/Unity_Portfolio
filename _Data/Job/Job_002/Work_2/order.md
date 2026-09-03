# 업무지시서

## 1. Text 행 추가·Icon 값 수정

**대상 스킬**: 게임개발_구성_데이터_테이블_작성

**"taskContent"**: 플레이 화면에 ID로 노출된 `Text` 11건 행 추가와 테이블 `Icon` 값 보정

**업무**

- 근거: `_Data/Job/Job_001/Work_6/result.md` `## 비고` "데이터" 항목
- `Text` 행 추가(한국어·영어): `Text_Core_GunUnlock`(해금 조건·알림·결과 라벨 공용 — `{0}` 자리표시자 사용 여부는 `Popup_Lobby.cs`·`Popup_Result.cs` 호출부 실측), `Text_Core_Confirm`, `Text_Core_RoomSelectTitle`, `Text_Popup_Setting_{BGM,SE,Fullscreen,Apply,Default,Applied}`, `Text_Quit_Title`, `Text_Quit_Text`. `Popup_Setting` 시트에 이미 있는 행은 중복 추가하지 말고 익스포트 미반영 원인을 확인한다
- `Boss` 테이블 `Pineapple` 행 `Icon`을 `AnimationSheet_Casual_Boss_Pineapple_Idle_01`로 교체 (Work_3_2에서 Idle 제작 완료)
- 로비 최고 순번 배지 아이콘 값을 Work_1이 정본으로 정한 ID로 입력한다 (해당 필드가 고정값이면 `게임개발_구성_데이터_고정값_작성`으로 처리)
- 리소스 참조값은 런타임 로드 실측으로 검증한다

## 2. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"taskContent"**: 전 종류 익스포트로 `TableText.json` 등 런타임 값 반영

**업무**

- 익스포트 후 `Assets/_Library/_Core/Resources/Table/TableText.json`에 추가 행이 실재하는지 실측하고 컴파일 에러 0건을 확인한다
- `confirmed`·`reuse` 값은 변경하지 않으며, DataMCP 무응답 시 `Fallback` 순서를 따른다
