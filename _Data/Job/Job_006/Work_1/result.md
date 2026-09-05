# [게임개발_구성_데이터_테이블_텍스트_작성] "Text 일본어 33행 입력·잔재 112행 제거·익스포트" 업무 레포트

## 요약
- `Text` 테이블 `Core` 시트 게임 사용 33행 `Jap` 입력(`table_excel patch` `success`), 잔재 101행 제거(`Core` 93 + `Popup_Setting` 8 — 지시서 112건 중 `Popup`·`Shutdown`·`Quit` 시트 9행은 라이브러리 소속이라 유지, `Core` 2행은 `_Data/Concept`·`_Data/Module/Game` 참조가 있어 유지)
- 데이터 전 종류 익스포트 11회 `success`, 사본 `TableText.json` 177 → 76행, 게임 사용 행 `NameJap` 빈 값 0(남은 빈 값은 라이브러리 `Text_Popup_*`·`Text_Shutdown_Text` 10행), 컴파일 에러 0

## 완료업무

### 문구 정본 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Game\concept.md`
**작업내용**
- `게임컨셉`에 지원 언어 규정 없음(grep 언어·English·Korean·Japanese 0건), `씬설정` Scene_Game·Scene_Lobby "Language: 팝업·HUD 문구 조회 (문구 정본은 Text 테이블)" — 지원 언어 실측 근거는 라이브러리 `LanguageConst.LanguageList` = English·Korean·Japanese
- 잔재 112행: `게임컨셉` 정본 ID(캐릭터 2·적 3·보스 2·방종류 4·능력 6·재화 1)·`씬설정` 어디에도 소속되지 않음(이전 템플릿 Card·Weapon·Shop·Mission·Attend·Difficulty·Sugar 계열)

### 일본어 입력·잔재 제거
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Table\Text\Core.xlsx`
**작업내용**
- 일본어 33행: 기존 행 어체(체언 종결·`（最大5重複）` 표기)와 고유명사(`クリームガン`·`ナイフ`)에 맞춤, `{0}` 자리표시자 유지(`Text_Core_GunUnlock` "{0}番目の部屋をクリアするとクリームガン解放")
- 제거 전 참조 재검색(`Assets` `.cs`·`.prefab`·`.unity`·`.asset`·테이블 JSON + `_Data/Concept` + `_Data/Module/Game`): 미참조 `Core` 93·`Popup_Setting` 8 확정 → `table_excel remove` 2회 `success`. 라이브러리 `.cs` 참조 `Text_Core_Yes`·`No`는 대상 밖(유지)
- 수동검증: 참조 실재(남은 행 전부 코드·프리팹·테이블 참조 있음), 밸런스 정합 대상 아님, 소비·명명(새 행 없음·행 ID 규약 유지), 런타임 포함 `table_excel list` `Core`·`Popup_Setting` `포함`

### 데이터 익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableText.json`
**작업내용**
- export: `type_manage`·`table_data`·`table_excel` × 7·`const_data`·`const_excel` 전부 `{"success":true}`
- 사본 실측: 76행, `Text_Core_ShopTitle`·`Text_Popup_Setting_Resolution` 부재, 33행 `NameJap` 채워짐
- 컴파일(`유니티엔진_컴파일_실행`): `clear_console` → `recompile` → `recompile_status` `failed:false` → 콘솔 에러 0 (스크립트 변경 없음 — 구조 코드 재생성 동일)

## 비고
- 일본어 폰트 글리프 표시 여부는 Work_5 QA(일본어 전환 `qa_ui`)에서 판정
