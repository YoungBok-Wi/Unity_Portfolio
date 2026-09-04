# [게임개발_프리셋_파일_오브젝트_구성] "프리셋 정렬 순서·로비 취소 입력" 업무 레포트

## 요약
- 업무 4건 전부 완료 — 로비 취소 입력 코드 수정(`Popup_Lobby.OnInputCancel` override), 오브젝트 정렬 순서 7종 규격 반영(Player 2·Enemy/Boss 1·Projectile 0 유지), export·verify 8건 success, 재임포트 `.meta` 15건 전건 존재
- 업무 2 플레이 실측: Setting→Apply Notify 열림 상태 `escape` 1회 → `Popup_Notify opened=false`·`Popup_Setting opened=true`·`Popup_Quit opened=false`; 2회 → Setting 닫힘·Quit false; 팝업 없음 상태 `escape` → `Popup_Quit opened=true` (`qa_play get`)
- 업무 3 플레이 실측: Apple 3마리 접촉(dx 0.56·0.72·0.82, `Attack_01~03`) 중 플레이어(`Hit_01`) 전신 노출 — `_Temp/Screenshot/Job004_Work5_apple_contact.png`, 씬 `sortingOrder` Player 2·Apple 1 (`eval`)
- 컴파일 `recompile_status` `status:completed`·`failed:false`, 콘솔 에러 0건. 종료 `editor_status playMode:stopped`, `Scene_Lobby isDirty:false`
- `Popup_Setting` 팝업 구성(취소 플래그)은 미수행 — 사유 `## 비고`
- `confirmed`·`reuse` 무변경 (`preset_manage get` 8종 `reuse:add`·`Popup_Lobby add`·`Popup_Setting default`, `confirmed` 값 그대로)

## 완료업무

### 1. 프리셋 현황 조회
**산출물**
`Assets/__Game/_Core/_Object/`
`Assets/__Game/_Core/_UI/Popup/Popup_Setting/Popup_Setting.prefab`
`Assets/__Game/_Core/_UI/Popup/Popup_Lobby/Popup_Lobby.prefab`
`Assets/_Library/_Core/_UI/Popup/Popup_Notify/Popup_Notify.prefab`
**작업내용**
- 하위 스킬 `게임개발_프리셋_파일_오브젝트_질문`·`게임개발_프리셋_파일_팝업_질문` 수행. 대상이 특정돼 트리 탐색 건너뜀
- 정렬 값 YAML 실측(`View` `SpriteRenderer`): 8종 전부 `m_SortingOrder: 0`, `Object_Background` -10·`Object_Floor` -5. 전조·히트 이펙트용 별도 프리팹 없음(오브젝트 프리팹 10개)
- 취소 플래그(`prefab_popup get`·YAML 일치): `Popup_Setting` `m_IsCloseByCancel:false`·`m_FixedOrder:-1`, `Popup_Lobby` false·`m_IsDefaultOpen:true`·`m_FixedOrder:0`, `Popup_Notify` false·`m_FixedOrder:83`. 세 스크립트 모두 `OnInputCancel` override 없음
- 취소 체인: `Assets/_Library/Popup/Script/PopupBase.cs` 203~213행 `performed && m_IsCloseByCancel && IsOpened`일 때만 자기 닫기·소비. `LocalPopupManager.OnInputCancel` 84~102행 열린 팝업 역순 → 닫힌 팝업 → `Popup_Quit.Open()`

### 2. 로비 취소 입력 코드 수정
**산출물**
`Assets/__Game/_Core/_UI/Popup/Popup_Lobby/Script/Popup_Lobby.cs`
**작업내용**
- (나) 채택. (가) 불가 사유: `Popup_Setting` 플래그 1이면 Notify 열림 중 Notify(플래그 0)가 넘기고 Setting이 자기를 닫아 "최상단 1개 닫힘" 위반
- `OnInputCancel` override 추가: `performed`에 `FindTopOpenedPopup()`(자기 제외 `IsOpened && !IsClosing` 중 `PopupCanvas.sortingOrder` 최대)을 `Close()`하고 true, 없으면 `base`(플래그 0 → false → `Popup_Quit` 열림). `using UnityEngine.InputSystem` 추가
- 신규 `[SerializeField]` 없음. `OnOpen`·`OnClose` 외부 상태 다루지 않음 — 중복 진입 가드 대상 아님. MCP override 변경 없음(상태 필드·버튼 핸들러 무변경)
- `template_manage test module-Popup` errors 없음. `유니티엔진_컴파일_실행`: `clear_console` → `recompile` → `recompile_status` `{"status":"completed","failed":false}` → `get_console_logs total:0`. 되돌릴 산출물 없음(콘솔 비움만)
- 라이브러리 무수정

### 3. 오브젝트 구성
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/Object_Player_Knife.prefab`
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Apple/Object_Enemy_Apple.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Watermelon/Object_Enemy_Watermelon.prefab`
`Assets/__Game/_Core/_Object/Object_Enemy_Banana/Object_Enemy_Banana.prefab`
`Assets/__Game/_Core/_Object/Object_Boss_Pumpkin/Object_Boss_Pumpkin.prefab`
`Assets/__Game/_Core/_Object/Object_Boss_Pineapple/Object_Boss_Pineapple.prefab`
**작업내용**
- 규격 `_Data/Concept/Resource/concept.md` 46~48행. `View` `SpriteRenderer` `m_SortingOrder`를 YAML 직접 기입(노드 규칙: `patch`가 이 키를 버림) — Player 2건 2, Enemy 3건·Boss 2건 1, `Object_Projectile` 0 현행 유지(무변경)
- 반영 실측: `AssetDatabase.Refresh` 후 YAML 재독 일치, 에디터 로드 프리팹 `GetComponentInChildren<SpriteRenderer>().sortingOrder` Knife 2·Gun 2·Apple 1·Watermelon 1·Banana 1·Pumpkin 1·Pineapple 1·Projectile 0 (레이어 Default)
- 스프라이트 실측(불투명 바운딩 높이, ppu 128, `isReadable` 차단 없음): Player Idle 128px/Move 129px, Idle_Gun 127/Move_Gun 133, Apple Move 113/Attack 114, Watermelon Move 138/Attack 110, Banana Move 109/Attack 114, Pumpkin Idle 225/Move 151, Pineapple Idle 224/Move 183 — 모션 간 차이 0 없음(캔버스 아님)
- 메타 `preset_manage set` 7건 description에 정렬 순서 추가(`reuse`·`confirmed` 무변경 — `get` 재조회 `reuse:add`·`confirmed:{}`)
- 컨셉아트 대조·연출 요구 3종·인스턴스화 경로·계열명: 이번 변경은 정렬 값뿐이라 대상 아님(구조·리소스·배선 무변경)

### 4. 익스포트
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/object.json`
`Assets/__Game/_Core/_Object/Object_Player_Gun/object.json`
`Assets/__Game/_Core/_Object/Object_Enemy_Apple/object.json`
`Assets/__Game/_Core/_Object/Object_Enemy_Watermelon/object.json`
`Assets/__Game/_Core/_Object/Object_Enemy_Banana/object.json`
`Assets/__Game/_Core/_Object/Object_Boss_Pumpkin/object.json`
`Assets/__Game/_Core/_Object/Object_Boss_Pineapple/object.json`
`_Temp/Screenshot/Job004_Work5_apple_contact.png`
**작업내용**
- 대상 판정: `Popup_Lobby`(스크립트)·오브젝트 7종(프리팹). `preset_manage export` 8건 `success:true`, `verify` 8건 `success:true`. export 후 YAML 정렬값 보존 확인
- `유니티엔진_재임포트_실행`: `AssetDatabase.Refresh()` → 변경 파일 15건(`git status`) `.meta` 전건 존재. 이동·개명 없음
- 플레이 실측(`editor_play` → `qa_play`): 업무 2·3 완료 기준 `## 요약` 참조. Apple 접촉 캡처는 timeScale 0.2(`eval`)·`LocalBattleManager.MCPCheatApply("HealPlayer")` 폴링으로 생존 유지 후 채취
- 종료: timeScale 1 복구 → `editor_stop` → `playMode:stopped`, `Scene_Lobby isDirty:false`. `editor_util setup` 미실행

## 비고
- 건너뛴 업무 — 대상: 업무 3 `게임개발_프리셋_파일_팝업_구성`(`Popup_Setting` `m_IsCloseByCancel`) / 조건: order.md "업무 2 결정에 따름" / 실측 근거: (나) 채택 시 플래그 0 유지가 필요 — 플래그 1이면 `PopupBase.OnInputCancel` 206행 조건으로 Notify 열림 중 Setting이 먼저 닫힘(플레이 실측 escape 1회에 Notify만 닫힘으로 현행 플래그 0이 기준 충족)
- 플레이 진입 직후 무입력 사망(Gun HP 80, 첫 폴링 게임시간 12~17초에 `Die_06`) — `eval` 최초 호출 지연 때문이며 결함 아님. 사망 후 `HealPlayer`는 부활시키지 않음
- `unity cmd set_timescale --value=0.2`는 인자를 에코하면서 "paused(0)"로 반영되는 무성실패 — `eval`로 `Time.timeScale` 직접 설정으로 대체
- 실측 관찰(이번 Work 범위 밖, 리소스 규격): Banana Move 109px가 Apple 113px보다 작아 `리소스컨셉` 크기 위계(Apple 113 < Banana 123 < Watermelon 138)와 불일치, 보스 Idle(225·224) 대 Move(151·183) 모션 간 높이 편차 큼
- 하네스 병렬 지시에 따라 독립 도구 호출은 묶어 요청함(프로젝트 규칙 "병렬을 요청하면 그 지시를 우선")
