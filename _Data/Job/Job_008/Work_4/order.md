# 업무지시서

## 1. 신규 모듈 등록

**대상 스킬**: 게임개발_모듈_폴더_생성

**"moduleId"**: Unit, Enemy, Boss, PlayerCharacter, RoomSelect (대상만 달리해 5회)

**"moduleNamespace"**: Game

**"content"**: Unit — 플레이어·적 공용 유닛 베이스(HP·피격·넉백 곡선·경직·사망·프레임 애니메이터). Enemy — 적 오브젝트 베이스·FSM 상태(Move·Attack·Die), 보스도 공용. Boss — 보스 오브젝트 베이스·FSM 상태(Idle·Move·Skill1·Skill2·Enrage). PlayerCharacter — 플레이어 베이스·입력·FSM 상태(Idle·Move·Jump·Attack·Hit·Die)·씬 상주 플레이어 활성 로컬 매니저. RoomSelect — 방 선택지 세트·보스 주기·웨이브 변형 배정 로컬 매니저

**업무**

- 소속 노드는 Work_1 결과의 모듈 노드 후보(`_Data/Job/Job_008/Work_1/result.md`)를 따른다
- 완료 기준: 5건 등록 `success:true`, `Assets/__Game/{모듈}/` 골격 존재

## 2. 기존 모듈 ID 변경·메타 확정

**대상 스킬**: 게임개발_모듈_폴더_구성

**"moduleId"**: Battle→Game, Character→Data, Room, Unit, Enemy, Boss, PlayerCharacter, RoomSelect (대상만 달리해 8회)

**"moduleNamespace"**: Game

**"content"**: Battle → `Game`(전투 판정·풀·연출·능력·일시정지·BGM·씬 전환 연출 — Battle·Room·PlayerCharacter를 총괄하는 로컬/전역 매니저), Character → `Data`(선택 캐릭터·Gun 해금·최고 순번 저장, Crumb 재화·누적 — 전역 매니저), Room 설명을 "방 진행·웨이브·벽·카메라(선택지는 RoomSelect)"로 갱신, 신규 5건 설명·소속 노드 확정

**업무**

- ID 변경은 원본·백업·등록·씬 컨셉 참조가 새 ID로 함께 이전되는지 응답으로 확인한다 (매니저 프리팹 이름 `[BattleManager]`·`[LocalBattleManager]`·`[CharacterManager]`·`[LocalCharacterManager]`는 3에서 새 이름으로 교체)
- `inAsset`·정의 심볼은 바꾸지 않는다
- 완료 기준: 8건 메타 verify `success:true`

## 3. 모듈 본문 작성 (기획 → 코드 → 프리팹)

**대상 스킬**: 게임개발_모듈_폴더_작성

**"moduleId"**: Unit, Enemy, Boss, PlayerCharacter, Room, RoomSelect, Game, Data (대상만 달리해 8회 — 이 순서)

**"moduleNamespace"**: Game

**"content"**: 아래 설계대로 코드 이전·개정, 각 모듈 `module.md`(기획·내부기능·외부사용·참조) 작성, 매니저 프리팹 갱신

**업무**

- 공통 규칙: `EUnitKind`·`EBattleTeam` 삭제. 종류 분기(`Kind == …`, `switch (m_Kind)`)는 전부 파생 override로 바꾼다 — `Object_UnitBase`(Unit)에 `protected abstract (int hp, float moveSpeed) LoadBase()`, `public abstract bool IsPlayerSide { get; }`(진영: 같은 값끼리 피해 없음), `public virtual bool IsKnockbackImmune => false`, `public virtual float KnockbackRate => 1f`, `public virtual int CrumbDrop => 0`, `protected virtual void OnHit(SHit)`. 플레이어 판정(`Player == unit`)은 매니저가 등록 시 `Object_PlayerBase` 타입으로 구분한다. `SpriteAnimPlayer`·`SHit`·`SProjectile`·`IProjectile`·`ProjectileBase`·`CrumbDrop`·`BattleConst`의 유닛 상수는 Unit으로, 매니저 상수는 Game으로 옮긴다(`GameConst`)
- Unit: `Object_UnitBase` — 넉백을 `AnimationCurve` 진행(거리 × 곡선(t/시간) 차분을 `Physics.Move` 통로로 적용)으로 바꾸고, 완료 뒤 `Battle_HitStunSec` 경직(`IsStunned` = 넉백 중 또는 경직 중)을 둔다. 곡선은 `LocalGameManager.KnockbackCurve`에서 읽는다. `SpriteAnimPlayer`는 `Resources.Load` 제거 — `[SerializeField] SAnimClip[] m_Clips`(`name`·`Sprite[] frames`·`fps` 선택) 인스펙터 배열, `Play(action, loop)`은 이름으로 클립 조회(없으면 예외), `GetLength`·`SetFlip` 유지, `IsFinished`·`CurAction` 유지, `CurFrame` 프로퍼티 추가. `Icon` — `[SerializeField] Sprite m_Icon` + `public Sprite Icon` (미리보기용)
- Enemy: `Object_EnemyBase : Object_UnitBase`(Enemy 테이블 로드, `EnemyData`, `KnockbackRate` override, `CrumbDrop` override, `IsPlayerSide=false`), `FSMState_UnitBase`(플레이어 조회는 `LocalGameManager.Player`)·`FSMState_EnemyMove`·`FSMState_EnemyAttack`·`FSMState_UnitDie` 이전. 스폰 시 기본 상태(`Move`)는 `protected virtual string SpawnState` override
- Boss: `Object_BossBase : Object_EnemyBase`(Boss 테이블 로드, `BossData`, `IsKnockbackImmune=true`, `CrumbDrop` Boss 값, `SpawnState=Idle`, `IsEnraged`/`SetEnraged` 이동), `FSMState_Boss*` 5종 이전. Enemy 테이블 로드가 아니라 Boss 테이블만 읽도록 `LoadBase` override
- PlayerCharacter: `Object_PlayerBase : Object_UnitBase`(Character 테이블, `IsPlayerSide=true`, 입력 폴링·`Facing`·공격 판정 범위·모션 해석 유지) + 자식 FSM 상태 `FSMState_PlayerIdle`·`Move`·`Jump`·`Attack`·`Hit`·`Die`(라이브러리 `FSMState` 파생, ID는 `UnitConst` 상태 ID 재사용). 전환 규칙: Idle↔Move(이동 입력), Idle/Move→Jump(점프 입력·접지), 공중→Idle(착지), Idle/Move→Attack(공격 입력, `IsAttackReady`), Attack은 `AttackInterval`(AttackSpeed 반영) 동안 유지하며 입력 무시, 종료 시 `ComboWindow` 안 재입력이면 다음 단(3단 뒤 1단), 아니면 Idle. 피격→Hit(Hit 모션 끝나면 Idle, 경직 동안 이동 불가), 사망→Die. 공격 방식은 `Object_PlayerBase`의 `abstract IPlayerAttack`이 아니라 파생 프리셋 스크립트(`Object_Player_Knife`·`Gun`)가 `OnAttackStart(step)`·`OnAttackFrame(step, frame)`·`OnAttackEnd()`를 override — Knife는 `CurFrame == 1`(2번째 프레임)에 `HitBox` 1회, Gun은 유지 입력 동안 `AttackInterval` 주기 발사(기존). `LocalPlayerCharacterManager` — 인스펙터 `m_Players`(씬 상주 `Object_PlayerBase[]`)에서 `DataManager.SelectedId`와 `Id`가 같은 것만 활성해 `Player`로 노출, 나머지 비활성. 로비 BGM 재생은 Game으로 이전
- Room: `LocalRoomManager` — 선택지·세트·보스 추첨(`RollChoices`·`MakeChoice`·`Choices`·`SelectRoom`)을 RoomSelect로 이전. 방 입장은 `EnterRoom(SRoomChoice)`(Battle은 `choice.Variant`로 `RoomUtil.GetWaves(index, variant)`), 보스 클리어는 승리가 아니라 일반 클리어(`ClearRoom` 공통 → RoomSelect 호출), 런 종료는 사망뿐(`ERunResult`·`Result` 삭제, `State` Ended 유지). `StartRun`은 플레이어 스폰 대신 `LocalPlayerCharacterManager.instance.Player` 사용. `RoomUtil.LoadUnitIcon` 삭제(→ `LocalGameManager.GetUnitIcon`)
- RoomSelect: `LocalRoomSelectManager` — `Choices`·`RollChoices(nextIndex, prevKind)` 규칙: `nextIndex % Room_BossCycle == 0` → [Boss] 1개(보스 무작위); prevKind가 Heal·Ability → [Battle/Battle]; 그 외 `Room_ChoiceSet1`·`Set2`·`Set4` 균등. Battle 선택지는 `Variant`를 배정(두 Battle이면 1·2를 무작위 순서로, 하나면 무작위) 하고 미리보기는 `RoomUtil.GetPreview(index, variant)`. `SRoomChoice`에 `Variant` 추가. `SelectRoom(index)` → `LocalRoomManager.EnterNext(choice)`. `Room_BossMin`·`Room_BossForce`·`Room_ChoiceSet3` 참조 0건
- Game: `LocalGameManager`(구 `LocalBattleManager`) — 인스펙터에 `KnockbackCurve`(기본 키 (0,0)·(0.25,0.85)·(1,1)), 단계 배율 `m_SlashStepScale`(1.0·1.25·1.5)·`m_SlashStepColor`(백색 → 연노랑 → 주황), `m_HitStepScale`(1.0·1.2·1.4) 추가, `PlaySlashEffect(center, facing, step)`·`Hit`의 히트 이펙트 배율 적용(`SHit.Step` 추가), `GetUnitIcon(id)`(등록 프리팹의 `Object_UnitBase.Icon`), `Player`는 `Object_PlayerBase` 등록 시 갱신(씬 상주라 활성 전환 시 `SetPlayer`). `GameManager`(구 `BattleManager`) — BGM 재생(`PlayBGM`)·로비 BGM(인스펙터 클립, 활성 씬이 로비일 때)만 남기고 Crumb는 Data로. `SceneChangeAni_Face : SceneChangeAni`(Prefab `SceneChangeAni_Face.prefab` — Overlay Canvas·Scaler 1920x1080 Expand, 타일 `Image` 격자 12x7, `Icon_Casual_Face_Chef` 스프라이트, StartAni: (row+col) 순 대각선 지연으로 scale 0→1 팝(총 0.6s) 뒤 `PostChange`, EndAni: 같은 순으로 1→0 뒤 `PostEnd`, unscaled 시간). `Editor/Script/Setup_Game.cs : ModuleSetupBase` — `OnSetupGlobal`에서 `[Global]/[SceneChangeManager]` 아래 `Face` 자식을 `InstantiatePrefabChild`로 두고 `m_SceneChangeAni` 배열에 `Default`+`Face`를 배선(재실행 안전). `LocalRoomManager.ReturnLobby`·로비 시작 전환은 `SceneChange(scene, "Face")`
- Data: `DataManager`(구 `CharacterManager`) — `SelectedId`·`GunUnlocked`·`BestRoom`·`IsUnlocked`·`Select`·`OnRoomCleared` 유지 + Crumb 잔액(`Bank`)·`CrumbTotal`·`AddCrumb`·`ResetRun` 이전
- 팝업·컨트롤·오브젝트 스크립트(프리셋 소속)는 이 Work에서 컴파일이 통과할 만큼만 참조를 바꾼다(`LocalBattleManager`→`LocalGameManager`, `CharacterManager`→`DataManager`, `BattleManager.PlayBGM`→`GameManager`, `Object_Enemy_*: Object_EnemyBase`, `Object_Boss_*: Object_BossBase`, `Object_Player_*` 공격 훅 override) — 노드·배선·연출은 Work_5
- `module.md` 8건: 기획·내부기능·외부사용(코드 예시)·참조를 새 구조로 쓰고, "수정 가능 값" 안내(넉백 곡선·`Battle_HitStunSec`·`Character` 넉백 거리/시간·`ComboWindow`·단계 배율·`Battle_LowHpRatio`·`Room_BossCycle`)를 Game·Unit `module.md`에 남긴다
- 완료 기준: 8모듈 `module.md`·코드·프리팹 verify `success:true`, `Assets/__Game/Battle`·`Character` 폴더 잔존 0, `EUnitKind`·`EBattleTeam`·`ERunResult` 파일 0, `Resources.Load` 호출 0 (아이콘 `IconManager` 경유 제외)

## 4. 컴파일

**대상 스킬**: 유니티엔진_컴파일_실행

**"changedPaths"**: Assets/__Game/**

**업무**

- 완료 기준: 에러 0 (에러는 3으로 돌아가 수정 후 재실행), `up_to_date`가 아니라 실제 컴파일 수행 확인

## 5. 모듈 익스포트

**대상 스킬**: 게임개발_모듈_폴더_익스포트

**"moduleId"**: Unit, Enemy, Boss, PlayerCharacter, Room, RoomSelect, Game, Data (대상만 달리해 8회)

**"moduleNamespace"**: Game

**업무**

- 완료 기준: 8건 export 응답 `success:true`, 재임포트 완료
- `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 수정 금지 — `SceneChangeManager` 배열 배선은 Game 셋업 스크립트가 [Global] 인스턴스에 건다. DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
