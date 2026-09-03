# [오케스트레이터_워커_실행] "Job_003 Work_4_2 Enemy 테이블 값 반영·데이터 익스포트" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 2건 완료. `Enemy` `Core` 시트 `Apple.Attack` 8→6 patch(`table_excel get` "6"), 전 종류 익스포트 11회 호출 전부 `{"success":true}`, 사본 `Assets/_Library/_Core/Resources/Table/TableEnemy.json`(16:15:07) `Apple.Attack` "6", 컴파일 `completed`·`failed:false`·`errors:[]`·콘솔 에러 `total:0`
- 무입력 생존 플레이 실측(순번 1, Apple 3마리, 스폰→사망): Knife 8.62s(목표 8~15s 안, 17타×6), Gun 7.50s(목표 6~12s 안, 14타×6) — 컨셉 산식값 8.19s·7.07s와 +0.43s 차, 첫 피격 3.19s(산식 접근 2.63s)
- 다음 행동: `밸런스컨셉` 149행 "개정 후 실측은 `Enemy` 테이블 반영 뒤 갱신" 자리에 실측 Knife 8.62s·Gun 7.50s 기재(컨셉 밸런스 작성 Work)

## 완료업무

### Enemy 테이블 값 반영
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Table\Enemy\Core.xlsx`
**작업내용**
- 수행 스킬: `게임개발_구성_데이터_테이블_작성` (child `게임개발_구성_데이터_테이블_텍스트_작성` 제외 — search.md 처리가능이 `Text` 테이블 전용)
- 선행 `게임개발_구성_컨셉_질문` → `게임개발_구성_컨셉_밸런스_질문`: `concept_manage path Balance` `_Data/Concept/Balance/concept.md`, `get Balance` `reuse:add`·`confirmed.concept.md:null`(무변경), `path Game` `_Data/Concept/Game/concept.md` 장르 "2D 사이드뷰 캐주얼 로그라이트 액션". 개정 값은 83행 Apple 공격력 6 하나 — Work_4_1 레포트 "변경 상수는 Apple 공격력 8→6뿐" 일치. 그 외 Apple(HP 30·주기 1.0·정지 0.8·히트박스 0.8·Crumb 2)·Watermelon(90/12/1.5/1.0/1.2/0.5배/4)·Banana(25/6/2.0/5.0/8.0/7.0/3) 명시값은 `table_excel get Enemy Core` 현재 행과 전부 일치 → 다른 행 반영 대상 없음
- 조회: `table_data get Enemy` 15필드, `table_excel list Enemy` `Core:포함`, 시트 행 3(Apple·Watermelon·Banana) — `*_Dummy_*` 행 0건
- 건너뛴 절차: 미작성 대상 전수 조회 — 조건 "content의 대상이 개별 행 ID로 확정되어 있으면 건너뛴다", 실측 근거 order.md 대상 `Apple` 행 지정 / 리소스 참조값 확인·리소스 로드 실측 — 조건 "리소스 참조 필드를 채웠을 때만", 실측 근거 변경 필드 `Attack`(int) / 행 제거·참조처 정리 — 조건 "더미 행·삭제 행이 있을 때만", 실측 근거 더미 0건·order.md 삭제 지시 없음
- 입력: `table_excel patch Enemy Core {"Apple":{"Attack":6}}` → `{"success":true}`
- MCP검증: `table_excel get Enemy Core jsonPath=Apple` `Attack:"6"`, 나머지 14필드 변경 전과 동일, 더미 행 없음
- 수동검증 항목: 참조 실재 — 대상 아님(리소스·텍스트 필드 무변경) / 밸런스 정합 — `Balance` 83행 공격력 6 = 행 값 6, 진행 테이블 아님(변화 구간 대상 아님) / 소비·명명 — 새 필드 없음, 대상 아님

### 데이터 익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableEnemy.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableAbility.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableBoss.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableCharacter.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableRoom.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableText.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableWave.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableConst.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\GenerateScript`
`C:\_Projects\Unity_Portfolio\_Temp\Work_4_2\measure.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_4_2\hook2.cs`
**작업내용**
- 수행 스킬: `게임개발_구성_데이터_익스포트`(child `{}`) → `유니티엔진_재임포트_실행` → `유니티엔진_컴파일_실행`
- export 11회 전부 `{"success":true}`·errors 없음: `type_manage export`, `table_data export`(1회), `table_excel export` × 7(`Ability`·`Boss`·`Character`·`Enemy`·`Room`·`Text`·`Wave` — `table_data list` 전수), `const_data export`, `const_excel export`
- 사본 실측: `TableEnemy.json` 16:15:07 갱신, `Apple` `{"Attack":"6","Hp":"30","AttackInterval":"1",...}`. 갱신 파일 24건(`GenerateScript/*.cs` 16건 16:15:03~16:15:10, `Table*.json` 8건 16:15:04~16:15:10)
- 재임포트: `list_open_scenes` `Scene_Lobby isDirty:false` → `eval AssetDatabase.Refresh()` `success:true` → 24건 `.meta` 누락 0건
- 컴파일: `clear_console` → `recompile`(16:16:20) `up_to_date`·`ScriptAssemblies/Library.dll` 15:33:38 < 트리거 → 규칙상 미성립 → `Refresh(ForceUpdate)`·`clear_console`·`recompile`(16:16:46) 재차 `up_to_date`(생성 스크립트 내용 동일) → `eval CompilationPipeline.RequestScriptCompilation()`(16:16:54, "eval 대체" 규칙·Work_4 선례) → `recompile_status` `{"status":"completed","failed":false,"errors":[]}`(16:17:09) → `get_console_logs` `total:0`. 실컴파일 근거: `Library/Bee/artifacts/1900b0aEDbg.dag/Library.dll` 16:16:59 재생성(크기 223744 동일이라 `ScriptAssemblies/Library.dll` 사본 미갱신)
- 컴파일 절차 산출물: 콘솔 버퍼 비움 외 없음(되돌릴 대상 없음). 씬 dirty 변화 없음
- 플레이 실측(`_Temp/Work_4_2/measure.sh` — `editor_play` → 로비 `Select_{캐릭터}` → `BattleManager` 코루틴 hook 선장착(`hook2.cs`, 플레이어 스폰 프레임에 t0 고정·`HitApplied`·`IsDead` 기록, 힐 없음) → `Popup_Lobby` `Start` → 사망 후 `editor_stop`): Knife `spawn hp 100/100`·적 3마리 스폰 +0.00s(x ±10·−11)·첫 피격 +3.19s·17타(피해 6/타 — `hp 94→88→82`)·사망 +8.62s / Gun `hp 80/80`·첫 피격 +3.18s·14타·사망 +7.50s. 두 런 모두 콘솔 에러 `total:0`, 종료 후 `Scene_Lobby isDirty:false`
- 목표 판정: Knife 8.62s ∈ 8~15s, Gun 7.50s ∈ 6~12s — 둘 다 범위 안. 산식값(8.19s·7.07s) 대비 +0.43s는 첫 피격 3.19s − 접근 산식 2.63s = 0.56s(공격 선딜)에서 발생, HP 소진 구간은 산식과 일치(Knife 17타×6 = 102 ≥ 100)

## 비고
- `PlayerPrefs` 상태: 실측 전 `SelectedCharacter=Knife`·`GunUnlocked=True`·`BestRoom=8`. Gun 실측을 위해 로비에서 `Select_Gun`(저장값 `Gun`으로 변경) → 종료 후 `PlayerPrefs.SetString("SelectedCharacter","Knife")`로 복원, 최종 재조회 `SelectedCharacter=Knife GunUnlocked=True BestRoom=8`(실측 전과 동일). `GunUnlocked`·`BestRoom`은 변경 없음(순번 1에서 사망, 방 클리어 없음)
- 실측 시도 3회 중 1회차는 스크립트 오류(`Library.Popup_Lobby` 네임스페이스 오기, 실제 `Game`)로 게임 미진입, 2회차는 hook 후장착으로 스폰 기준 이탈(surv 5.95s, 무효) → 3회차(hook 선장착)만 채택. 1·2회차 모두 `PlayerPrefs` 무변경(Knife 선택 상태)
- `밸런스컨셉` 149행 "개정 후 실측은 `Enemy` 테이블 반영 뒤 갱신"은 컨셉 문서 몫이라 이번 Work에서 미수정(order.md 범위 밖)
- 사용자 질문 없음, `confirmed`·`reuse` 무변경, DataMCP 호출 전부 MCP 도구 1회 성공(`Fallback` 미사용), 라이브러리 무수정(`_Temp/라이브러리_수정요청.md` 미작성)
