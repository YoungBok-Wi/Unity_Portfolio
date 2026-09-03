# [오케스트레이터_워커_실행] "Job_001 Work_4 모듈 재사용 켜기·게임 모듈 코드 작성" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 1은 전 항목 실측상 이미 완료라 건너뜀, 업무 2는 `Room`·`Battle`·`Character` 설계 문서·스크립트 28건·매니저 프리팹 5건 작성, verify 3건·export 3건 `{"success":true}`, 컴파일 에러 0건
- 컴파일 실측: `recompile_status` `{"status":"up_to_date","failed":false,"errors":[]}`, `get_console_logs --severity=error` `total=0`, `Library/ScriptAssemblies/Game.dll` 09:01:26 갱신(마지막 스크립트 편집 09:01:09 이후)
- 씬 반영: `editor_util` setup 2회 `{"success":true}` — `Scene_Game` `[Global]`에 `[BattleManager]`·`[CharacterManager]`, `[Local]`에 `[LocalBattleManager]`·`[LocalCharacterManager]`·`[LocalRoomManager]` 실측, `Scene_Lobby` `[Local]`에 `[LocalCharacterManager]` 실측, 두 씬 저장 후 `isDirty=false`
- 플레이 검증 미수행 — 로컬 매니저 인스펙터(플레이어·적·보스·투사체 프리팹, 스폰 위치)가 비어 있어 `StartRun`이 예외로 끝나는 상태. 배선은 Work_5 오브젝트 스크립트 완성 후 가능 (`## 비고`)
- `confirmed`·`reuse` 미변경

## 완료업무

### 기존 모듈 재사용 켜기
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\module.json`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\module.json`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\module.json`
**작업내용**
- 수행 스킬: `게임개발_모듈_폴더_구성` (child `{}`)
- 건너뜀 — 대상 `게임개발_모듈_폴더_생성` / 조건 order.md "Work_1이 등록한 신규 모듈이 누락된 경우에만" / 실측 `module_manage` list Game `inAsset`에 `Battle`·`Character`·`Room` 3건 실재
- 건너뜀 — 대상 `inAsset=true` patch(`FSM`·`CharacterPhysics`·`Delegate`·`Bank`·`ObjectPool`) / 조건 스킬 "현재 inAsset이 요청 값과 같으면 스킵·보고" / 실측 list Library `Basic.inAsset`에 5건 전부 실재
- 건너뜀 — 대상 `Lv`·`Quest`·`Link`·`Statis` 켜기 / 조건 order.md "`씬설정` 사용 모듈에 있을 때만" / 실측 `_Data/Concept/Scene_Lobby|Scene_Game/concept.md` `## 사용 모듈` 소제목 grep 0건
- 신규 모듈 메타: get 재조회 `parentNodeId` `게임기능_진행`·`게임기능_전투`·`게임기능_캐릭터`, `description` 실재 → 변경 없이 확정 (설명은 업무 2 기획에서 동기화)
- 컴파일 확인(`유니티엔진_컴파일_실행`): `list_open_scenes` `Scene_Game` `isDirty=false` → `clear_console` → `recompile` `up_to_date` → `recompile_status` `failed=false` → 에러 `total=0`. 이번 업무 파일 변경 0건이라 `up_to_date` 합격 (dll 03:12:36 = 직전 Work 빌드). 콘솔 비움 외 되돌릴 대상 없음

### 게임 모듈 코드 작성
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\[BattleManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\[LocalBattleManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\Script`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\Prefab\[LocalRoomManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\Script`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\Prefab\[CharacterManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\Prefab\[LocalCharacterManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Game.unity`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Lobby.unity`
**작업내용**
- 수행 스킬: `게임개발_모듈_폴더_작성` → 하위 `기획_작성`·`코드_작성`(→`코드_매니저_글로벌_작성`·`코드_매니저_로컬_작성`)·`프리팹_작성` → `유니티엔진_컴파일_실행` → `게임개발_모듈_폴더_익스포트`. `코드_셋업_작성` 건너뜀 — 조건 "프리팹 우선" 규칙(단순 매니저 생성은 셋업을 쓰지 않음) / 실측 세 모듈 모두 매니저 간 참조·씬 객체 주입 없음
- 기획: `module.md` 3건 `module-Info` 구조로 작성(각 70줄 이하, 통지 통로 계약 `## 외부사용`에 기재), `description` patch 3건 → get 재조회 일치, verify 3건 success
- 자율 확정 설계: 유닛 공용 베이스 `Object_Unit`(테이블 스탯·HP·넉백·경직·사망) + 적 FSM 상태 3종(`Move`·`Attack`·`Die`, 그룹 `Melee`·`Tank`·`Ranged` 분기) + 보스 FSM 상태 5종(`Idle`·`Move`·`Skill1`·`Skill2`·`Enrage`, `Skill*Id`·`AttackType` 분기, HP 비율 Enrage 1회); 능력은 `FloatFactor`(Attack 가산·AttackSpeed 곱·MoveSpeed 가산)와 스택 사전(MultiHit·MaxHp·HealMacaron); Crumb는 `BattleManager`(전역)가 `Bank`에 저장 없이 등록; 플레이어 스폰·해금은 `Character`, 방·웨이브·선택지·런 종료는 `Room`이 담당. 값은 전부 `TableManager.instance.{Character|Enemy|Boss|Room|Ability|Wave|Const}`만 참조 (코드 리터럴 없음, `BattleConst`·`RoomConst`는 ID·상태명)
- 코드 종류·파일: 열거형 4(`EBattleTeam`·`EUnitKind`·`ERoomState`·`ERunResult`), 구조체 4(`SHit`·`SProjectile`·`SEnemyPreview`·`SRoomChoice`), 인터페이스 1(`IProjectile`), 상수 2, 유틸 1(`RoomUtil`), 클래스 10(`SpriteAnimPlayer`·`FSMState_*` 9), 오브젝트 1(`Object_Unit`), 전역 매니저 2(`BattleManager`·`CharacterManager`), 로컬 매니저 3(`LocalBattleManager`·`LocalRoomManager`·`LocalCharacterManager`). 로컬 매니저 3건 모두 `MCPDetail`·`MCPInteraction`/`MCPCheats` override 작성
- 템플릿 verify 1차 불합격 3종 수정: `IProjectile`·`SEnemyPreview`·`SRoomChoice` 헤더 누락 → using/파일 주석 추가; `Object_Unit` `autoProperty.list[].summary` 누락(파서가 식 프로퍼티 뒤 첫 자동 프로퍼티 summary를 소실, `template_manage parse` 실측) → 자동 프로퍼티 6건을 필드 기반 식 프로퍼티로 교체. 이후 verify 3건 success
- 컴파일 1차 실패 16건 전부 CS0507(`FSMState.OnStart/OnUpdate`가 `protected internal`이라 다른 어셈블리 override는 `protected`만 허용) → `FSMState_*` 9파일 `protected override`로 수정 → `completed`·`failed=false`·에러 0
- 프리팹: `copy_asset`으로 `[BankManager].prefab` 복제 5건 → `m_Script` guid를 `.cs.meta` 실측값(`BattleManager` dc48c816…, `LocalBattleManager` a806c33d…, `LocalRoomManager` cd8e702b…, `CharacterManager` 79fc3505…, `LocalCharacterManager` f0e830a1…)으로 결선·`m_Name` 교체 → `eval` `LoadAssetAtPath` 5건 `GetComponent<ManagerBase>()` 타입이 각각 `Game.{클래스}` 실측; `module.json` `globalManagerPrefab`·`localManagerPrefab` patch 5건(한 필드씩) → get 재조회 일치. 열린 씬 미변경(`save_scene`은 셋업 후에만 호출)
- 셋업: `Scene_Game`(열려 있던 씬, `isDirty=false`) setup success → 계층 실측 → save; `Scene_Lobby` open → setup success → `[Local]` 실측 → save → `Scene_Game` 재오픈 `isDirty=false`. 에러 로그 0건
- 익스포트: export 3건 `{"success":true}` (Game·`inAsset=true`라 `_Data` 사본 없음, export가 `module.json`·프리팹 `.meta` 갱신 실측). 재임포트는 `AssetDatabase.Refresh()` + 컴파일 검증으로 수행
- 소비처 대조(범위 `Assets` 전역 + `_Data/Module` .cs 445파일, public 멤버 149건): 0건 34건 중 자기 클래스 안에서만 쓰는 6건은 private로 좁힘(`HitStop`·`GetMultiHit`·`StartRun`·`DespawnPlayer`·`GetTable`, `AttackScale` 프로퍼티 제거). 나머지 28건은 order.md "오브젝트 전용 스크립트·팝업은 Work_5 담당"이 명시한 외부 소비처(팝업: `LocalRoomManager` 값·`SelectRoom`·`SelectAbility`·`RerollAbility`·`ReturnLobby`·`RoomUtil.LoadUnitIcon`·`SRoomChoice.Enemies`·`BattleManager.CrumbTotal`·`AbilityStacks`; 오브젝트: `*Factor`·`GetPlayer*`·`Object_Unit.Fsm/CharacterData`·`SpriteAnimPlayer.CurAction/IsFinished`·`SProjectile.Velocity/MaxDistance`)라 Work_5를 근거로 판정 보류
- 방어 항목 대조(베이스 `ObjectBase`·`LocalManagerBase`·형제 `LvManager`·`BankManager`): null 매니저 가드 — 해당(`Object_Unit.InitSingleton` `LocalBattleManager.instance` null 체크, `LocalRoomManager.OnShutdown` battle null 체크); 초기화 전 등록 제약 — 해당(`BankManager.Create`·`NumberManager.Create`를 `Init` 안에서만 호출); 값 클램프 — 해당(`Object_Unit.Heal`/`TakeHit` 0~MaxHp); 정적 참조 해제 — 해당(로컬 매니저 3건 `OnDestroy`); 잘못된 입력 예외 — 해당(풀 미등록 ID·범위 밖 선택지·미제시 능력·미해금 캐릭터 전부 throw)
- 단순화 주석 2곳: `LocalBattleManager.HitStopRoutine`(일시정지와 timeScale 공유), `OnUnitDied`(Crumb 낙하·수거 없이 즉시 적립)

## 비고
- 플레이 검증 미확인: `LocalRoomManager.StartRun`→`LocalCharacterManager.SpawnPlayer`가 인스펙터 `m_PlayerPrefabs` 빈 배열이라 예외로 끝난다. 프리셋 스텁(`Object_Player_*`·`Object_Enemy_*`·`Object_Projectile`)은 아직 `ObjectBase` 빈 클래스라 `Object_Unit`·`IProjectile`을 구현하지 않아 배선 자체가 불가 — Work_5에서 오브젝트 스크립트 작성 후 `[LocalBattleManager]`(적·보스·투사체 프리팹, 히트 이펙트·전조·SFX)·`[LocalCharacterManager]`(플레이어 프리팹)·`[LocalRoomManager]`(스폰 위치 3종) 인스펙터를 YAML 직접 기입으로 채워야 한다 ("매니저 프리팹 도구 제약" 규칙)
- 통지 통로 결손 없음. 텍스트 결손: `RoomConst.TextGunUnlock`(`Text_Core_GunUnlock`)·`TextConfirm`(`Text_Core_Confirm`) 행이 `Text` 테이블에 없어 `LanguageManager.Get`이 ID를 그대로 돌려준다 — 데이터 Work에서 행 추가 필요
- 적·보스 애니메이션 동작명은 `BattleConst`(적 `Move`·`Attack`·`Die`, 보스 `Idle`·`Move`·`Attack1`·`Attack2`·`Die`)로 Work_3_2 프레임명과 일치시켰고, 플레이어 `Hit`·`Die`만 `Object_Unit`이 재생한다 (공격·이동 재생은 Work_5 플레이어 스크립트)
- `AutoTextureSettingOnImport.cs` `SpriteAnim` 임포트 규칙 추가는 에디터 스크립트 영역이라 미수행 (Work_3_2 비고 그대로 결손)
- `template_manage parse` 실측상 `module-Object` 파서가 `Update`·`FixedUpdate`를 `initUIOnce`·`initUI` 슬롯으로 읽는다 — verify는 통과하지만 템플릿 재생성 도구가 쓰이면 어긋난다 (도구 결함 보고, 우회 없음)
- 히트스톱은 `Time.timeScale`을 쓰므로 일시정지 팝업(Work_5)이 같은 값을 쓰면 소유자를 한 곳으로 모아야 한다 (`단순화:` 주석)
