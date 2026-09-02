# [오케스트레이터_워커_실행] "Job_001 Work_1 컨셉 문서 작성" 업무 레포트

## 요약
- Work 판정: 실패 — 업무 1·2 합격, 업무 3은 컨셉 문서 합격 후 컨셉아트 제작 단계에서 타입 구성 결손으로 중단, 업무 4 미수행
- `게임컨셉` 최초 작성 완료 (`_Data/Concept/Game/concept.md`, `concept_manage` verify `success=true`, 정본 ID 18건 + 씬 2건 등재)
- `밸런스컨셉` 최초 작성 완료 (`_Data/Concept/Balance/concept.md`, verify `success=true`), 테이블 골격 6건·고정값 골격 10건 등록 (`table_data` list 7건, `const_data` get 10건 실측)
- `리소스컨셉` 최초 작성 완료 (`_Data/Concept/Resource/concept.md`, verify `success=true`), `Concept/Concept_Resource/Overview` 파일 골격·프롬프트 등록 (pool 비어 있음)
- 막힌 지점: `Concept_Resource` 타입의 `basePromptText`·`workerValues`·`workerLiteralValues`·`automationType` 전부 빈 값 — 즉시·예약 제작 스킬 모두 "타입 구성 결손 보고" 조건에 걸려 컨셉아트 미제작 (`## 예외상황`)

## 완료업무

### 게임컨셉 작성
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
**작업내용**
- 선행 조회: `concept_manage` list → `Global.Game=""`(비어 있음), get → `reuse=add`·`confirmed=null`; `unity_concept` game 응답 전 필드 빈 값 → 최초 작성으로 `Write`
- 수행 스킬: `게임개발_구성_컨셉_게임_작성` + 하위 `게임개발_구성_컨셉_게임_액션_작성` (방치형 하위는 장르 불일치로 미선택)
- 자율 확정한 기획값과 근거: 프로젝트명 `Kitchen Riot`; 보스 `Pumpkin`(근접·Idle/Chase/Slam/Charge/Enrage)·`Pineapple`(원거리·Idle/Retreat/Spike/Rain/Enrage) — 과일 반란 테마와 근접·원거리 대비; 재화 `Crumb` 단일 (기존 `Icon_Casual_Currency/Crumb` 실재, 소비처는 능력 리롤); 능력 6종 `Attack`·`AttackSpeed`·`MaxHp`·`MoveSpeed`·`HealMacaron`·`MultiHit` (전부 `Icon_Casual_Upgrade`에 아이콘 실재); 이동/공격 리듬 "정지 공격 + 이동 회피"; 회피 대시 없음(점프로 대체); 해상도 1920x1080 Landscape·`StandaloneWindows64` 단일 타겟; 기술 URP만 채택(어드레서블·뒤끝·포톤 패키지 부재 — `Packages/manifest.json` 실측)
- 필수 판정 6항목 전부 "확정" (문서 `## 필수 판정`), 장르 요소 15건 소비처 포함, 액션 표준 5항목·전투 파라미터 4항목 문서 `## 기획 원칙` 그룹에 판정 기재
- verify 1차 실패(70행 그룹 항목 콜론 누락 extra 2건) → 수정 후 `success=true`; `unity_concept` game 응답 `title=Kitchen Riot`·`resolution 1920x1080`·`orientation=Landscape`·`buildTarget StandaloneWindows64` 컨셉값과 일치
- 외부 적용(`editor_util` setup) 생략 — `씬설정` 문서 `Scene_Lobby`의 씬 파일 부재 (`find Assets -name "Scene_*.unity"` 결과 `Assets/__Game/_Core/__Scene/Scene_Game.unity` 1건뿐)
- 이어서 작성할 문서: `밸런스컨셉`·`리소스컨셉`(본 Work 업무 2·3에서 완료), `씬설정` `Scene_Lobby`·`Scene_Game`(업무 4, 미수행)

### 밸런스컨셉 작성
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Table\Character\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Enemy\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Boss\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Room\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Ability\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Wave\Core.xlsx`
**작업내용**
- 선행 조회 실측: `table_data` list → `Text`만 등록; `const_data` get → `consts` 빈 객체; `type_manage` list → 게임 타입 없음; `module_manage` list → `inAsset=false` 후보 `FSM`·`CharacterPhysics`·`Bank`·`Delegate`·`ObjectPool`(inAsset) 실재, 게임 모듈 `Lv`(notInAsset)만; `preset_manage` list → `Object` 0건, 게임 `Popup_Setting` 1건
- 하위 스킬 `밸런스_방치형_작성`은 장르 불일치로 미선택, 대상 스킬 4개 지침(그룹 단위·동시 추격 배치·강화 설계·수치 품질) 반영
- 필수 판정: 수치 하한(선택지 풀 3<6, 진행 난이도 성장식) 확정, 체감 지표(처치 시간·웨이브 소멸 시간) 확정, 개수 검산 확정 — 문서 `## 검산` 7건 전부 "일치"
- `게임컨셉` 정본 대조: 캐릭터 2·적 3·보스 2·방종류 4·능력 6·재화 1 = 18건 문서 ID와 쌍별 일치 (구식 값 0건)
- 검증: `concept_manage` verify(`Balance`) `success=true`; 테이블 create 6건 응답 `success=true`, `table_data` list에 `Ability`·`Boss`·`Character`·`Enemy`·`Room`·`Wave` 반환; 고정값 patch 10건(`Room_GrowthHp`·`Room_GrowthAtk`·`Room_GunUnlock`·`Room_BossMin`·`Room_BossForce`·`Room_HealRatio`·`Battle_MaxEnemyOnScreen`·`Battle_MeleeSlotPerSide`·`Ability_RerollBaseCost`·`Ability_RerollCostStep`) `const_data` get에 설명 반환·`type` 빈 값
- 후속 담당: 테이블 필드 구조·행 값과 고정값 타입·값 입력은 Work_2 (`order.md` 업무 2 기재)

### 리소스컨셉 작성
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Resource\File\Concept_Resource\Overview`
**작업내용**
- 선행 조회 실측: `resource_node` node 최상위 12계열; `resource_type` list `Casual` 계열 타입 다수(`AnimationSheet_Casual_Enemy`·`Player`, `Icon_Casual_Weapon`·`Upgrade`·`Currency`, `Illust_Casual_Hit`·`Slash`·`Projectile`·`Splatter`·`Tile` 등); `_Data/Resource/inAsset.json` 127건; `Concept` 계열에 `Concept_Resource`·`Concept_Scene_Game`·`Concept_Scene_Lobby` 타입 실재, `resource_file` list(`Concept`) 빈 객체
- 하위 스킬 `리소스_캐주얼_탑뷰_작성`은 탑뷰 규격 담당이라 사이드뷰 불일치로 미선택
- 재사용·신규 구분을 문서 `### 전체 스타일`에 계열 단위로 기재, 신규 타입 3종(`AnimationSheet_Casual_Boss`·`Illust_Casual_Background`·`Icon_Casual_Room`) 규격 확정
- 필수 판정: 테마 선택(`Casual`) 확정, 규격 확정(6계열 캔버스·기준 높이·피벗·점유율·서열), 개수 검산(신규 타입 3 + 기존 타입 추가 파일 2 + 컨셉아트 1 = 6 대상 열거 일치), 연출 요구 확정(4건)
- 실측 우선 보고: `AnimationSheet_Casual_Enemy` 이동은 1프레임 + 코드 회전(타입 설명 실측)이라 "한 동작 4~8프레임" 지침과 어긋나 예외로 문서에 기재
- 검증: `concept_manage` verify(`Resource`) `success=true`
- 후속 체인: `게임개발_구성_리소스_파일_생성` → `Concept/Concept_Resource/Overview` create `success=true`; `게임개발_구성_리소스_파일_구성` → 프롬프트 patch `success=true`, get 응답 `prompts.prompt.value` 반영·`outputs.art.pool` 빈 객체; `게임개발_구성_리소스_파일_이미지_제작` 이하 미수행 (`## 예외상황`)

## 비고
- 업무 4(`게임개발_구성_컨셉_씬_생성`) 미수행 — 업무 3 체인 중단으로 워커 절차 6에 따라 남은 업무를 수행하지 않음
- `Scene_Game.unity` 실제 경로 `Assets/__Game/_Core/__Scene/Scene_Game.unity`가 `씬설정` `Scene_Game` 문서·`ProjectSettings/EditorBuildSettings.asset`의 `Assets/__Game/Scene/Scene_Game.unity`와 불일치 (업무 4 체인에서 정리 필요)
- `Concept_Scene_Game` 타입 `basePromptText`가 다른 게임(SugarSlash·베이커리·쿼터뷰·모바일 세로 1080x1920) 문구 — 업무 4 씬 컨셉아트 제작 전 타입 구성 갱신 필요
- `confirmed`·`reuse` 값은 변경하지 않음

## 예외상황
- 대상: `게임개발_구성_리소스_파일_이미지_제작`(즉시 모드) → `Concept/Concept_Resource/Overview` 컨셉아트 제작
- 막힌 지점: `resource_type` get(`Concept`, `Concept_Resource`) 응답 `prompts.prompt.basePromptText=""`, `workerValues={}`, `workerLiteralValues={}`, `automationType=null`, `automationId=""` — 즉시 제작 지침 "값이 없으면 직접 쓰지 말고 타입 구성 결손으로 보고한다", 예약 제작은 `automationType`(Generate) 부재, 배치는 대상 1건이라 불성립
- 에러 원문: 도구 에러 없음 (지침 조건에 의한 중단), 해당 스킬 `error.md`(`이미지_제작` 빈 파일, `즉시_제작` 없음)에 처리 항목 없음
- 사용자 확인 요청: `Concept_Resource` 타입에 `basePromptText`(Casual 화풍·사이드뷰·1920x1080 공통 지시) 구성을 `게임개발_구성_리소스_타입_구성`으로 수행할지 지시 필요 — 구성 후 업무 3 체인(이미지 제작 → 업로드)과 업무 4를 재개 가능
