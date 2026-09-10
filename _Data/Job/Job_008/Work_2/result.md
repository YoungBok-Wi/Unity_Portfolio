# [게임개발_구성_데이터_익스포트] "데이터 정의·값·익스포트 반영" 업무 레포트

## 요약
- 고정값 3건(`Room_BossCycle` 5·`Battle_HitStunSec` 0.1·`Battle_LowHpRatio` 0.3), `Character.ComboWindow`, `Wave.Variant` 정의와 값을 원본에 반영했다.
- `Wave`는 잘못 생성된 `Core` 행을 제거하고 변형 1·2 각 27행, 총 54행으로 정리했다. 두 변형의 마지막 구간 `RoomMax`는 `999`다.
- 타입·전체 테이블 7종·텍스트·고정값 export가 모두 `success:true`이며 생성 코드와 런타임 JSON에서 신규 필드를 확인했다.

## 완료업무

### 고정값 정의와 값 반영
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Const\consts.json`
`C:\_Projects\Unity_Portfolio\_Data\Const\consts.xlsx`
**작업내용**
- `const_data get`에서 3건의 타입·설명을 확인했고 `const_data patch` 3회가 모두 `success:true`였다.
- `const_excel get _Core`에서 5·0.1·0.3을 확인했고 동일 값 patch 3회가 모두 `success:true`였다.
- 수치 근거는 `_Data/Concept/Balance/concept.md`의 보스 주기·피격 경직·저체력 경고 항목과 일치한다.

### 테이블 구조와 행 반영
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Table\Wave\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Wave\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Character\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Character\Core.xlsx`
**작업내용**
- `table_data get`에서 `Wave.Variant int`와 `Character.ComboWindow float` 정의를 확인했고 구조 patch가 모두 `success:true`였다.
- `Wave` 검증값은 `Variant=1` 27행·`Variant=2` 27행·`*_Dummy_*` 0행·`Core` 0행이다.
- `Wave_R10_W1`과 `Wave_R10_W1_V2`의 `RoomMax`는 모두 `999`이며 변형별 총 마릿수 차는 웨이브당 ±1 이내다.
- `Character` 검증값은 Knife `ComboWindow=0.4`, Gun `ComboWindow=0`이다.

### 전체 데이터 익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\GenerateScript`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table`
**작업내용**
- `type_manage export`, `table_data export`, `table_excel export` 7종, `const_data export`, `const_excel export` 응답이 전부 `success:true`였다.
- `Table_Const.cs`에서 고정값 3건, `Type_WaveTable.cs`에서 `Variant`, `Type_CharacterTable.cs`에서 `ComboWindow`를 확인했다.
- `TableConst.json`·`TableWave.json`·`TableCharacter.json`에서 동일 값과 필드를 확인했다.

## 비고
- 대상 — 업무 2 `게임개발_구성_데이터_고정값_생성`의 고정값 3건 등록. 조건 — 스킬의 "이미 존재하면 신규 등록 대신 수정 대상임을 보고하고 후속 구성으로 잇는다". 실측 근거 — `const_data get`이 `Room_BossCycle int`·`Battle_HitStunSec float`·`Battle_LowHpRatio float` 정의를 반환했다.
- 코드 컴파일은 이 Work에서 미확인이다. 신규 필드 소비 코드는 후속 모듈 Work 대상이다.
