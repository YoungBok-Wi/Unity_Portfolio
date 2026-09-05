# [게임개발_구성_데이터_테이블_텍스트_작성] "게임 제목 텍스트 Kitchen Riot 수정·익스포트" 업무 레포트

## 요약
- `Text` 테이블 `Core` 시트 `Text_Core_GameTitle` Kor·Eng·Jap = "Kitchen Riot" — `table_excel patch` `success:true`, 재조회 일치
- 데이터 전 종류 익스포트 11회(타입 1·테이블 구조 1·테이블 값 7·고정값 2) 전부 `success:true`, 사본 `TableText.json` 82~86행 3값 "Kitchen Riot" 실측
- 컴파일 `recompile_status` `status:up_to_date`·`failed:false`, 콘솔 에러 0 (C# 변경 파일 0건이라 `up_to_date` 합격 — `git status` 변경은 `TableText.json` 1건)

## 완료업무

### 제목 정본 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
**작업내용**
- `concept_manage get Game`: `reuse:add`·`confirmed:null` (무변경). 게임 제목 = 문서 H1 "Kitchen Riot - 게임컨셉"(scopes `titleVar: title`), `게임컨셉`에 productName 별도 항목 없음
- 로비 제목 라벨 참조 ID = `Text_Core_GameTitle` (`Assets/__Game/_Core/_UI/Popup/Popup_Lobby/Script/Popup_Lobby.cs` 97행 `UIWrapper_Text.SetTextId(m_Title, "Text_Core_GameTitle")`)

### 게임 제목 텍스트 수정
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Table\Text\Core.xlsx`
**작업내용**
- `table_excel list Text`: `Core` 시트 `포함` (그 외 Popup·Shutdown·Quit·Popup_Setting·Popup_Quit 포함, Thebackend 미포함 — 대상 아님)
- `table_excel get Text/Core` 전 행 대조: 제목 용도 행은 `Text_Core_GameTitle` 1건뿐 ("Game"·"게임" 값 다른 행 없음 — `Text_Core_TabHome` "주방/Kitchen"은 탭 라벨이라 대상 아님)
- patch 후 재조회 `Name.Kor/Eng/Jap` = "Kitchen Riot" (고유명사 — 번역·음차 없음, `Rich` 빈 값 유지)
- 수동검증: 참조 실재(행 ID `Text_Core_GameTitle` 소비 코드 `Popup_Lobby.cs` 97행 실재), 밸런스 정합 대상 아님, 소비·명명(새 필드 없음, 행 ID 규약 `Text_Core_{용도}` 유지)

### 데이터 익스포트·컴파일
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableText.json`
**작업내용**
- export 호출: `type_manage`, `table_data`, `table_excel` × Text·Ability·Boss·Character·Enemy·Room·Wave, `const_data`, `const_excel` — 전부 `{"success":true}`, errors 없음
- 사본 실측: `TableText.json` `Text_Core_GameTitle` NameKor/NameEng/NameJap = "Kitchen Riot"
- 컴파일(`유니티엔진_컴파일_실행`): `list_open_scenes` `Scene_Lobby isDirty:false` → `clear_console` → `recompile` "No scripts needed recompilation" → `recompile_status` `{"status":"up_to_date","failed":false,"errors":[]}` → `get_console_logs` `total:0`. `up_to_date` 합격 근거: 스크립트 변경 0건(`git status` 변경 파일 `TableText.json`만). 콘솔 버퍼 비움 외 되돌릴 대상 없음

## 비고
- 로비 라벨 표시 폭·잘림 판정은 Work_6 QA 몫 (지시서대로 플레이 미수행)
