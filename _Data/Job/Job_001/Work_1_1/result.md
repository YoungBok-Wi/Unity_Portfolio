# [오케스트레이터_워커_실행] "Job_001 Work_1_1 Concept 타입 프롬프트 구성·컨셉아트 제작과 씬설정·씬 골격 셋업" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 1·2 완료 기준 충족 (`Concept_Resource/Overview` pool 반입·select 확정, 두 씬 파일 빌드 등록, `씬설정` 2건 verify success, 셋업 success, 컴파일 에러 0건)
- `Concept` 타입 3종(`Concept_Resource`·`Concept_Scene_Game`·`Concept_Scene_Lobby`) `basePromptText`·`automationType=Generate`·`automationId=codex_image`·`workerValues`·`workerLiteralValues` 구성 완료 (`resource_type` get 재조회 실측), 컨셉아트 3장 제작·반입 (`Concept/*/Overview` pool `1.png`·`select=1.png`)
- `씬설정` `Scene_Lobby` 최초 작성·`Scene_Game` 전면 수정, 경로 `Assets/__Game/_Core/__Scene/`로 통일, 빌드 순서 `Scene_Lobby`=0·`Scene_Game`=1 (`get_build_settings`·`EditorBuildSettings.asset` 실측)
- 신규 게임 모듈 `Room`·`Battle`·`Character` 등록, 재사용 모듈 `FSM`·`CharacterPhysics`·`Bank`·`Delegate` `inAsset=true` 전환·export·verify success, 프리셋 골격 오브젝트 10건·팝업 6건 생성·export success
- 발견 결함: `Popup_Setting`(Game, `reuse=default`)이 이전 게임 잔재 — `Control_GameFrame` 미존재 참조·중첩 프리팹 guid `f5fd32b466e5d4845b1409b135bb3463` 실체 없음으로 컴파일 에러 유발 → `inAsset=false`로 되돌리고 `씬설정` "포함(등재 대기)" 처리 (`## 비고`)

## 완료업무

### Concept 타입 프롬프트 구성과 컨셉아트 제작
**산출물**
`C:\_Projects\_WebForGameData\_Data\_Resource\File\Concept_Resource\type.json`
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Concept_Scene_Game\type.json`
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Concept_Scene_Lobby\type.json`
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Concept_Resource\Overview\art\1.png`
**작업내용**
- 수행 스킬: `게임개발_구성_리소스_타입_구성`(하위 없음, child `{}`) → `게임개발_구성_리소스_파일_구성` → `게임개발_구성_리소스_파일_이미지_제작`(`GPT_즉시_제작`) → `게임개발_구성_리소스_파일_업로드`. `게임개발_구성_리소스_파일_생성`은 `resource_file` list에 `Concept_Resource/Overview` 실재로 건너뜀 (조건: order.md "entry가 이미 있으므로 건너뛰고 사유 보고")
- 선행 실측: `resource_type` get — `Concept_Resource` `basePromptText=""`·`automationType=null`·`workerValues={}`, `Concept_Scene_Game`·`Lobby` `basePromptText`가 SugarSlash·쿼터뷰·1080x1920 문구. `automation_manage` list(Generate) → `codex_image` 실재, `input.md` size 허용값 `1024x1024/1536x1024/1024x1536/2048x2048/auto`
- 자율 확정: 자동화 `codex_image`, 워커 값 `options.prompt→prompt`, 리터럴 `size=1536x1024`(1920x1080 미지원이라 최근접 가로)·`background=opaque`·`quality=high`; 씬 타입 2종에 `ref_concept`(autoLink `Concept/Concept_Resource`)·`workerFiles image_1.png→ref_concept` 추가; 프롬프트는 `리소스컨셉` Casual 화풍·2D 사이드뷰·1920x1080·바닥선 하단 20%·배경 저채도 공통 지시로 영문 작성 (기존 Casual 타입 `Illust_Casual_Tile` 관례)
- 공통 규격 확인: 세 타입 description "캔버스 규격 미고정" → 규격 대조는 결손 보고로 대체, `리소스컨셉` 1920x1080을 문구 근거로 사용
- 검증: patch 후 get 3건 변경 반영 확인, `resource_file` list `Concept_Resource` 1건(`Overview`)·`Concept_Scene_*` 0건→생성 후 각 1건, `leaf` 빈 값이라 assetPath 대상 0건
- 파일 구성: `Overview` prompt value 기존 반영 확인(변경 없음), `project_manage unused` 조회(cleanup 미지정, 조회만)
- 즉시 제작: `automationWorker` `codex_image` `Work_0001` create→start→`Completed`; `Output/art.png` 읽기 검수 합격(요리사 Knife 우향·Apple·Watermelon·Banana 껍질 투척·Pumpkin 1.75배·HUD·바닥선), IHDR 실측 1672x941 (워커가 size 무시, 16:9 자동 크기) — 타입 규격 미고정이라 리사이즈 없이 원본 반입
- 업로드: upload `worker=manual` → get pool `1.png`·`select=1.png` (서버 자동 선택), `leaf` 빈 값이라 export·재임포트 건너뜀

### 씬설정 작성과 씬 파일·골격 셋업
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Lobby\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Game\concept.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Lobby.unity`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Game.unity`
`C:\_Projects\Unity_Portfolio\ProjectSettings\EditorBuildSettings.asset`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup`
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Concept_Scene_Game\Overview\art\1.png`
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Concept_Scene_Lobby\Overview\art\1.png`
**작업내용**
- 선행 조회 실측: `concept_manage` list Scene `Scene_Game`·`Scene_Lobby` 실재(`Scene_Lobby` 본문 빈 파일); `module_manage` list Library `notInAsset` `FSM`·`CharacterPhysics`·`Bank`·`Delegate` 등, Game `Lv`만; `preset_manage` list Popup Library 5건·Game `Popup_Setting` 1건, Object 0건, Control Library 7건; `unity_module global` 매니저 프리팹 등록값; `Setup_*` 스크립트 7종(`Camera2D`·`Input`·`Popup` 등); `RunLocal` 코드 실측 — 로컬 매니저·`Setup_*` 없는 모듈명은 조용히 건너뜀
- `게임개발_구성_컨셉_씬_생성` 건너뜀 (조건: order.md "두 씬 모두 이미 존재하므로 생성 건너뜀", 실측: `concept_manage` list Scene 2건). 씬 이름은 `SceneChangeManager.cs:16`·`ShutdownManager.cs:15` 기본값 `Scene_Lobby`와 일치
- `게임개발_구성_컨셉_씬_작성`: `Scene_Lobby` Write(최초), `Scene_Game` Edit(전면). 자율 확정 — 빌드 인덱스 Lobby 0·Game 1; 사용 모듈 Lobby 14종(`Camera2D`·`Input`·`Popup`·`Save`·`Sound`·`Table`·`Value`·`Number`·`Language`·`Icon`·`Deal`·`Bank`·`Delegate`·`Character`), Game 19종(위 + `FSM`·`CharacterPhysics`·`ObjectPool`·`Room`·`Battle`); 신규 게임 모듈 3종 `Room`(방 진행)·`Battle`(전투)·`Character`(선택·해금)으로 최소화; UI Lobby `Popup_Lobby`·`Popup_Quit`, Game `Popup_HUD`·`Popup_RoomSelect`·`Popup_Ability`·`Popup_Pause`·`Popup_Result`·`Popup_Notify`; Object Lobby `Object_Background`, Game 10건(플레이어 2·적 3·보스 2·`Object_Projectile`·`Object_Background`·`Object_Floor`)
- 필수 판정: 필수 팝업 3종 — Lobby: 종료 포함·설정 포함(등재 대기)·일시정지 제외(진행 상태 없는 씬); Game: 일시정지 포함·설정 포함(등재 대기)·종료 제외(`게임컨셉` 이탈 경로가 `Popup_Pause` 포기). 취소 입력 주체 — Lobby `Popup_Quit`, Game `Popup_Pause`(RoomSelect·Ability·Result 열림 시 무시). 전 항목 "확정"
- `게임컨셉` 정본 대조: 씬 2·캐릭터 2·적 3·보스 2·방종류 4·능력 6·재화 1 ID 쌍별 일치, 정본 밖 ID 0건. verify `Scene_Lobby`·`Scene_Game` `success=true`(최종 편집 후 재검증 포함). `unity_concept scene` — buildIndex 0/1·localModule·localPopup·localObject가 문서와 일치
- 경로 정리: `Scene_Game` 문서 경로를 실제 `Assets/__Game/_Core/__Scene/Scene_Game.unity`로 통일 (`EditorBuildSettings.asset`은 이미 실제 경로였음 — Work_1 레포트의 불일치 대상은 문서뿐). `Scene_Lobby.unity`를 같은 폴더에 `create_scene`으로 생성, `remove/add_scene_to_build`로 순서 Lobby 0·Game 1 확정
- `게임개발_프리셋_파일_생성`: 오브젝트 10건 `prefab_object create` + 메타 set, 팝업 6건 `prefab_popup create` + 메타 set (첫 배치 5건 도메인 리로드 연결 실패 → 재시도 성공). 스텁은 `template_manage test`(`module-Popup`·`module-Object`) errors 0으로 통과해 보완 없음. `preset_manage export` 16건 success, Refresh 후 `.meta` 전건 존재
- `게임개발_프리셋_파일_오브젝트_삭제` 건너뜀 (조건: 씬 컨셉 편성 범위 "등재에서 빠진 대상" 없음, 실측: 이전 `unity_concept scene` localObject `{}`·`preset_manage` list Object `{}`)
- `유니티엔진_씬_생성`: `Scene_Lobby.unity` 생성·빌드 등록·저장(`isDirty=false`), `find_assets` 1건. 루트 `Main Camera` 생성은 건너뜀 (조건: `Camera2D` 모듈 `[LocalCameraManager]` 프리팹이 `Main Camera` 보유 — 실측 `Scene_Game` 계층 `/[Local]/[LocalCameraManager]/PosRoot/RotRoot/Main Camera`, 루트에 또 만들면 카메라·AudioListener 중복)
- `게임개발_모듈_폴더_생성`: `Room`·`Battle`·`Character` create → `reuse=add` → `Assets/__Game/{모듈}/module.json`·`module.md` 작성·`_Data` 스텁 삭제 → `parentNodeId` patch(`게임기능_진행`·`게임기능_전투`·`게임기능_캐릭터`) → get 재조회 일치, path `infoPath`가 `Assets` 원본. `module.md` `내부기능`·`외부사용`은 export verify가 빈 본문을 거부해 골격 수준 내용을 채움 (설계는 Work_4)
- `게임개발_모듈_폴더_구성`: `FSM`·`CharacterPhysics`·`Bank`·`Delegate` `inAsset=true` patch(`ObjectPool`은 이미 inAsset) → export 4건 success → verify 4건 success → get 재조회 `inAsset=true`. `## 참조` 섹션 4건 모두 부재, `Bank` 의존(`Save`·`Number`·`Language`·`Deal`)은 inAsset 실측. 매니저 셋업은 씬 셋업 실행으로 갈음 — `[Global]`에 `[BankManager]`·`[DelegateManager]` 인스턴스 실측
- 리소스 체인: `게임개발_구성_리소스_타입_생성` 건너뜀(타입 3종 실재), `타입_구성`은 업무 1에서 완료. `Concept_Scene_Game/Overview`·`Concept_Scene_Lobby/Overview` create → prompt value patch → `codex_image` `Work_0002`·`Work_0003` `Completed` → 읽기 검수 합격(Game: 요리사 좌·적 3 우·HUD HP/방 배지·아이콘 열/Crumb, Lobby: 선택 카드·잠금 카드·START·설정) → IHDR 1672x941 → upload → pool `1.png`·`select=1.png`
- `게임개발_프리셋_파일_팝업_삭제` 건너뜀 (조건: 등재에서 빠진 팝업 없음 — `Popup_Quit`은 `Scene_Game` 문서에서 제외됐으나 Library 프리셋이라 삭제 대상 아님). `게임개발_프리셋_파일_팝업_구성`은 골격 메타 등록까지만 수행 (노드 구성·코드는 Job 계획상 Work_5 담당, `## 비고`)
- `유니티엔진_씬_셋업_실행`: UI 목록 등재 후 `Scene_Lobby`·`Scene_Game` 각각 open→setup `{"success":true}`→save. 실측 — `[Popup]` Lobby `Popup_Lobby`·`Popup_Quit`, Game `Popup_HUD`·`Popup_RoomSelect`·`Popup_Ability`·`Popup_Pause`·`Popup_Result`·`Popup_Notify`; `Main Camera` `UniversalAdditionalCameraData.m_Cameras` arrayLength 1(두 씬); `get_console_logs` 에러 0건; 두 씬 `isDirty=false`
- 컴파일: `RequestScriptCompilation`→`recompile_status completed`, `Game.dll`·`Library.dll` 02:55:38 갱신(마지막 스크립트 추가 이후), 콘솔 에러 0건

## 비고
- `Popup_Setting`(Game, `reuse=default`)은 `inAsset=true` 메타인데 `Assets` 사본이 없던 상태였고, export로 복원하자 `Popup_Setting.cs:1 using Game`·`:13 Control_GameFrame` CS0246 2건 발생. 백업 프리팹의 중첩 프리팹 guid `f5fd32b466e5d4845b1409b135bb3463`는 `Assets` `.meta` 전역 검색 0건(실체 없음). `preset_manage set inAsset=false`+export로 사본 제거해 컴파일 복구했고 `씬설정` 두 문서에 "포함(등재 대기)"로 기재 — Work_5에서 `Game` 프레임 `컨트롤`(`SetTitle`·`AddCloseListener` 계약) 확보 후 재구성 필요
- `Concept_Resource` 타입 `location=project` 이동 patch가 2회 모두 `{"success":true}`였으나 get `location`은 `shared` 그대로 (실체 `_WebForGameData/_Data/_Resource/File/Concept_Resource/type.json`) — 프롬프트는 공용 타입에 Kitchen Riot 문구로 저장된 상태. 도구 응답과 실효 불일치는 우회하지 않고 보고만 함
- `Object_*` 10건은 빈 골격 프리팹이며 `Setup_Object_*` 스크립트가 없어 셋업이 씬 배치를 건너뜀(`localObject` 등재는 완료). 오브젝트 코드·셋업 스크립트는 Work_5 `게임개발_프리셋_파일_오브젝트_코드_작성` 담당
- 팝업 6건은 `PopupBase` 파생 스텁·빈 프리팹 골격이며 `템플릿 test` 통과. 노드 구성·컨셉아트 대조·라벨 산정은 Work_5 담당이라 이번 Work의 `팝업_구성` 수동검증 항목(컨트롤 구성·배선)은 미판정
- 씬 셋업 전후 프리팹 오버라이드 `eval` 대조는 수행하지 않음 — 두 씬 모두 사용자 영역 오브젝트 0건(`Scene_Game` 셋업 전 루트 실측: 고정 영역만, `Scene_Lobby` 신규 빈 씬)이라 소실 대상이 없다는 근거로 갈음
- 컨셉아트 3장은 워커가 `size=1536x1024`를 무시하고 1672x941로 산출 — `Concept` 타입 규격 미고정이라 원본 반입, 규격이 정해지면 리사이즈 필요
- 셋업 경고 `[StatisManager]`·`[QuitManager]`·`[QuestManager]`·`[LinkManager]`·`[DateManager]` 프리팹 부재는 `notInAsset` 모듈의 등록값이며 이번 범위 밖(사용 모듈에 미등재)
- `confirmed`·`reuse` 값은 변경하지 않음
