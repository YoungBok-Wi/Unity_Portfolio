# [게임개발_구성_컨셉_게임_작성] "게임·밸런스·리소스·씬설정 정본 개정" 업무 레포트

## 요약
- 컨셉 정본 5문서 개정 완료 — `게임컨셉` 13곳·`밸런스컨셉` 11곳·`리소스컨셉` 7곳·`Scene_Game` 8곳·`Scene_Lobby` 4곳 구간 교체, 5문서 전부 `concept_manage verify` `success:true`
- 핵심 개정: 방 선택 3세트+비전투 직후 [Battle/Battle]+보스 주기 `Room_BossCycle` 5(단일 선택지, 런 계속)·런 종료는 HP 0만, 공격 판정 2번째 프레임·쿨타임 입력 무시·`ComboWindow` 0.4s, 넉백 곡선(`KnockbackCurve` (0,0)·(0.25,0.85)·(1,1))·경직 `Battle_HitStunSec` 0.1·저체력 `Battle_LowHpRatio` 0.3·타격 단계 배율 1.0/1.25/1.5, `Wave` `Variant` 2종, SpriteAnim Inspector 참조·시트 우향 규칙, 신규 타입 `Icon_Casual_Face`·`UI_Common_Gradient`(비네트), 팝업 Scaler·연출 규격, 사용 모듈 8종 재편·플레이어 2종 씬 배치
- 외부 적용(setup)은 업무 2에서 1회 실행 후 씬 오버라이드 소실을 실측해 git으로 복원했고, 업무 12에서는 건너뛰었다 (비고)
- 환경 변경: Unity CLI가 요구하는 `com.unity.pipeline` 0.3.1-exp.1 → 0.6.0-exp.1 업그레이드 (비고)

## 완료업무

### 컨셉 정본 전수 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Lobby\concept.md`
**작업내용**
- `concept_manage list`: Global Game·Resource·Balance, Scene Scene_Game·Scene_Lobby. `get` scopes 템플릿 `concept-게임컨셉`·`밸런스컨셉`·`리소스컨셉`·`씬컨셉`, reuse·confirmed 무변경
- 개정 대상 실측(개정 전 행): 게임컨셉 34·35·36·45·80·82·90·106·108·158·176·188·196행, 밸런스컨셉 238·250~251·258~260·267·275·283·290·343·365·373·438~442행, 리소스컨셉 14·33·53·75·141·186·192행, Scene_Game 478·485·488·489·490·542~549·566·611~615행, Scene_Lobby 20·21·22·65~66행

### 게임컨셉 개정·검증
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
**작업내용**
- 구간 교체 13곳(전부 1회 일치 확인): 런 구조·방 선택·방 종류·Knife 콤보·플레이어 오브젝트 상주·플레이어 FSM·종류 분기 원칙·짧은 주기·이동/공격 리듬·방 진행·종료·종료 조건·넉백/경직·공격 연출·애니메이션 + 장르 요소 "저체력 경고"·"씬 전환 연출" 신설
- 액션 장르 표준 5항목: 루프·파워 판타지·타격 3요소·이동/공격 리듬(정지 공격 + 이동 회피, 선입력 폐기·쿨타임 무시·콤보 창으로 재확정)·보스 문법 전부 반영 유지. 전투 파라미터 4항목(공격 주기·이동속도·넉백·범위 판정) 항목명·방식만 기재(값 없음)
- 필수 판정 6항목 전부 확정: 진행 변화 구간(방 순번)·판 간 진행(같은 씬)·일시정지·이탈 경로·종료 조건(HP 0만, 승리 종료 없음 사유 기재)·화면 추적 기준
- `concept_manage verify Game` `success:true`(errors 없음). `unity_concept game`: title "Kitchen Riot"·resolution 1920x1080·orientation Landscape·tech 빈 값 = 문서 값 일치. 정본 ID 열거 18건 변경 없음, 문서 내 ID 전부 정본 소속
- 외부 적용 `editor_util setup` 응답 `{"success":true}` — 부작용은 비고 참조

### 데이터·모듈·프리셋 현황 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Job\Job_008\Work_1\result.md`
**작업내용**
- 고정값(`const_data get`·`const_excel get _Core`) 16건: `Room_BossMin` 6·`Room_BossForce` 10·`Room_ChoiceSet1~4`(`Battle/Heal`·`Heal/Ability`·`Battle/Boss`·`Battle/Battle`)·`Room_GunUnlock` 5 등. 신규 ID `Room_BossCycle`·`Battle_HitStunSec`·`Battle_LowHpRatio` 중복 없음
- 테이블(`table_data get`): `Wave` 열 RoomMin·RoomMax·WaveIndex·Enemy1~3 Id/Count, 27행(R01~R09 구간 1방씩, R10 구간 RoomMax 99 = 상한 없음), 시트 `Core` 1개. `Character`에 `InputBuffer`·`Icon`, `Enemy`·`Boss`에 `Icon`(값은 SpriteAnim 파일명 — leaf `SpriteAnim`이라 `IconManager` 폴백 대상 아님, `RoomUtil.LoadUnitIcon`이 직접 로드)
- 모듈(`module_manage list/get/path`): Game 네임스페이스 Battle(`게임기능_전투`, 전역 `Prefab/[BattleManager]`·로컬 `Prefab/[LocalBattleManager]`)·Room(`게임기능_진행`, 로컬 `Prefab/[LocalRoomManager]`)·Character(`게임기능_캐릭터`, 전역·로컬) 전부 reuse add·inAsset true·confirmed 없음. 재편 소속 노드 확정: Unit·Enemy·Boss·Game → `게임기능_전투`, PlayerCharacter → `게임기능_캐릭터`, Room·RoomSelect → `게임기능_진행`, Data → `게임기능_세이브`
- 프리셋(`preset_manage list/get`): 팝업 7·컨트롤 5·오브젝트 10 전부 inAsset, `Object_Player_Knife`·`Gun` 설명이 "런타임 스폰"(씬설정 Object 항목과 함께 Work_5에서 설명 갱신 대상)

### 밸런스컨셉 개정·검증
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
**작업내용**
- 구간 교체 11곳: 짧은 주기 원칙·재화 순환(주기당)·해금 곡선(보스 주기·비보스 선택지·웨이브 변형)·난이도 곡선(순번 11 이상·보스 주기 성장)·Knife 판정 시점/콤보 창·Gun 콤보 창 0·플레이어 공통(넉백 곡선·피격 경직·저체력 경고·타격 단계)·방 종류 Boss·카테고리 수량(세트 3종+[Boss], 웨이브 변형 54행)·세션 길이(주기 205s)·검산(주기 길이·웨이브 변형 행 수·보스 주기 대 Gun 해금·넉백 곡선 급가속/급감속 신설)
- 필수 판정: 선택지 풀 3 < 6 확정, 진행 난이도 성장식 확정, 체감 지표(처치 타수·생존 시간) 확정, 개수 검산 확정
- 독립 재계산: 3×40+10+75 = 205(문서 205 일치), 27×2 = 54(일치), 0.85÷0.25 = 3.4·0.15÷0.75 = 0.2(일치), Room_GunUnlock 5 = Room_BossCycle 5(일치). 정본 대조: Battle·Heal·Ability·Boss / Knife·Gun / Apple·Watermelon·Banana / Pumpkin·Pineapple / 능력 6종 / Crumb 전부 `게임컨셉` 정본 소속
- `concept_manage verify Balance` `success:true`

### 리소스 현황 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Job\Job_008\Work_1\result.md`
**작업내용**
- `resource_node node`: 계열 12종, `Icon` → `Icon_Casual`(타입 Room·Upgrade·Weapon·Currency 등)·`AnimationSheet` → `AnimationSheet_Casual`(Boss·Enemy·Player)·`UI` → `UI_Casual`·`UI_Common`(`UI_Common_Shape`)
- `resource_type get`: 시트 3종 슬롯 frame_01~06 전부 `resources: true`·leaf `SpriteAnim`·idPrefix `AnimationSheet_Casual_{계열}_`·reuse add. `Icon_Casual_Room` 128x128·leaf `Icon`·`resources: true`(테이블 문자열 로드용)·가공 `image_normalize`. `UI_Common_Shape` reuse `default`·location `shared`·파일 reuse `fixed`(Circle128) → 게임 전용 비네트를 공유 저장소 타입에 넣지 않고 프로젝트 로컬 신규 타입 `UI_Common_Gradient`(노드 `UI_Common`, leaf `Image`, `resources` false)로 결정 (지시서의 `UI_Common_Shape/Vignette` 대체, 비고)
- `resource_file path/source Illust_Casual_Chef/Knife`: `Assets/__Game/_Core/Image/Illust_Casual_Chef_Knife.png`, pool `_Data/Resource/File/Illust_Casual_Chef/Knife/art/1.png`(640x960)

### 리소스컨셉 개정·검증
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
**작업내용**
- 구간 교체 7곳: 전체 스타일에 "애니메이션시트 참조 방식" 항목 신설, 신규 제작 대상에 `Icon_Casual_Face`·`UI_Common_Gradient/Vignette` 2건 추가(총 13건), 일반 적 "시트 방향" 규칙, UI 절 4항목(보스 단일 선택지·Canvas Scaler·프레임형 연출·저체력 경고), 규격 절 `Icon_Casual_Face`(256x256·기준 232·중심 피벗)·`UI_Common_Gradient`(512x512) 신설(계열 10종), 연출 요구 "타격 단계"·"팝업 등장"·"저체력 경고"·"씬 전환" 신설, 애니메이션 규격 "프레임 참조"·"시트 방향" 추가
- 필수 판정: 테마 선택 확정(Casual 유지), 규격 확정(10계열 캔버스·기준 높이·피벗·점유율·서열 전부 수치), 개수 검산 확정(신규 제작 대상 13 = 기존 11 + 2, 규격 10 = 기존 8 + 2), 연출 요구 확정(8항목)
- `concept_manage verify Resource` `success:true`. 정본 대조: 캐릭터·적·보스·방종류 ID 변경 없음

### 씬설정 개정·검증 (Scene_Game·Scene_Lobby)
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Lobby\concept.md`
**작업내용**
- Scene_Game 8곳: 역할·설명(보스 주기·플레이어 상주·플레이어 FSM·씬 전환 연출·런 종료 HP 0), 사용 모듈 `Room`·`Battle`·`Character` → `Unit`·`Enemy`·`Boss`·`PlayerCharacter`·`Room`·`RoomSelect`·`Game`·`Data`, `Popup_Result` 승패 삭제, `Object_Player_Knife`·`Gun` "씬 배치(선택 캐릭터만 활성)"
- Scene_Lobby 4곳: `Character` 참조 2곳 → `Data`, 로비 BGM 주체 `Game`, 사용 모듈 `Character` → `Data`·`Game`
- 필수 판정: Scene_Game — 필수 팝업 3종(Pause 포함·Setting 포함·Quit 제외 사유) 확정, 취소 입력 주체 `Popup_Pause` 확정. Scene_Lobby — Quit 포함·Setting 포함·Pause 제외 사유 확정, 취소 입력 주체 `Popup_Quit` 확정
- `concept_manage verify` Scene_Game·Scene_Lobby 둘 다 `success:true`. `unity_concept scene`: buildIndex 1·0 일치, localModule에 신규 8종·`Data`·`Game` 등재(globalManagerPrefab·localManagerPrefab 빈 값 — 미등록 상태, Work_4 등록 뒤 채워짐), localPopup은 UI 목록과 일치. 정본 씬 목록(Scene_Lobby·Scene_Game) 소속
- 외부 적용(setup) 건너뜀 — 대상: 업무 12 `게임개발_구성_컨셉_씬_작성` 절차 5 setup. 조건: 지시서 "모듈 미등록·프리팹 미배선 상태라 Work_6에서 수행". 실측 근거: `unity_concept scene` localModule 신규 8종 매니저 프리팹 빈 값, 업무 2의 setup 실행에서 `Scene_Game` 오브젝트 재생성·오버라이드 소실 실측(비고)

## 비고
- setup 부작용(업무 2): `editor_util setup` 성공 응답 뒤 `git diff`에서 `Scene_Game.unity` 1784행·`[Global].prefab` 576행 변경 — 씬 오브젝트 전부 새 fileID로 재생성되며 `Object_Floor/View` `SpriteRenderer` `m_DrawMode Tiled`·`m_Size 185x8`·scale 0.325 오버라이드와 메인 카메라 `orthographic size` 4 오버라이드가 소실됨(HEAD 대비 `propertyPath: m_Size.x/m_DrawMode/orthographic size` 0건). 처리: `git checkout` 3파일(`Scene_Game.unity`·`[Global].prefab`·TMP `DefaultFont.asset`) 복원 후 `unity cmd open_scene`으로 재로드, `get_serialized_fields` 실측 DrawMode Tiled·Size 185x8·scale 0.325·orthographic size 4 복구 확인, `list_open_scenes` isDirty false. Work_6 셋업은 스킬 절차대로 setup 전 오버라이드 목록을 기록하고 setup 뒤 재적용·결손 보고해야 한다 (Floor 타일 값은 프리팹 원본 정의처 이전이 결손 후보)
- 환경 변경: Unity CLI 1.0.0-beta.8이 프로젝트 `com.unity.pipeline` 0.3.1-exp.1을 거부("too old to parse command lines") → CLI가 처방한 `unity pipeline upgrade --project-path`로 0.6.0-exp.1 적용(`Packages/manifest.json`·`packages-lock.json` 변경, 에디터 창 활성화로 패키지 해석 유도). 이후 CLI 정상. 에디터 인스턴스가 2개(Unity_AgenticTestAction 7800·Unity_Portfolio 7801)라 CLI 호출은 `--project-path` 명시
- 비네트 파일 위치 변경: 지시서 `UI_Common_Shape/Vignette` → `UI_Common_Gradient/Vignette` 신규 타입 (사유는 리소스 현황 조회 참조). Work_3·Work_5 지시서의 해당 ID를 이 값으로 읽는다
- `게임기능_전투` 노드 규칙 "한 판 경계"(전투·진행 한 모듈)와 사용자 지시(8모듈 분리)가 어긋남 — 사용자 지시 우선, Work_4 `module.md`에 분리 사유 기재 예정
- `Wave` RoomMax는 테이블 규약(99 = 상한 없음)이 이미 무한 진행을 허용하나, 지시서대로 Work_2에서 999로 넓힌다(순번 100 이상 예외 방지)
