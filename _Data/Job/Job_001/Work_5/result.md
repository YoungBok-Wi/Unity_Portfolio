# [오케스트레이터_워커_실행] "Job_001 Work_5 프리셋 코드 작성·구성·익스포트·씬 반영" 업무 레포트

## 요약
- Work 판정: 합격 — 컨트롤 5건 신규(생성·코드·구성·export), 팝업 7건(신규 6 + `Popup_Setting` 재구성) 코드·구성·export, 오브젝트 10건 코드·구성·export, 로컬 매니저 인스펙터 배선(YAML), 두 씬 셋업·저장, 플레이 진입 에러 0건
- 컴파일 실측: 최종 `recompile_status` `{"status":"completed","failed":false}`, `get_console_logs --severity=error` `total=0`
- 플레이 실측(`editor_play` 2회, `qa_play`·`qa_cheat`): `Scene_Lobby` 진입 → `Popup_Setting` 열기/슬라이더 `SetBgm 0.4`/닫기 → `Start` → `Scene_Game` `StartRun` 예외 없음(`[LocalCharacterManager].playerSpawned=true`, `[LocalBattleManager].aliveEnemy=3`) → `Popup_Pause` 열림 시 `Time.timeScale=0` 실측·`Resume` → `Popup_Result` `Confirm` → 로비 복귀 → 재시작 → `ClearRoom` 치트 → `Popup_RoomSelect`(`choice0 "Battle Applex6 Watermelonx2"`, `choice1 "Heal"`) → `Select1` → `Popup_RoomSelect`(`Heal/Ability`) → `Select1` → `Popup_Ability`(`choices "HealMacaron,MaxHp,MultiHit"`) → `Select1` → 전 구간 콘솔 에러 0건. 캡처 `_Temp/Work_5/{lobby,setting,game_result,game_pause,roomselect,ability}.png`
- 익스포트 실측: `preset_manage export` Control 5·Object 10·Popup 7 전건 `{"success":true}`(1차 `Object_Projectile` verify 불합격 → 계약 위치 변경 후 success), `module_manage export` `Battle`·`Room`·`Character` success, `module_manage verify Battle` success
- 씬 셋업 실측: `editor_util setup` `Scene_Game`·`Scene_Lobby` 각 `{"success":true}`, `get_scene_hierarchy` — Game `[Popup]` 7팝업(`Popup_Setting` 포함)·`[Stage]/Object_Background·Object_Floor`, Lobby `[Popup]/Popup_Lobby·Popup_Quit·Popup_Setting`·`[Stage]/Object_Background`; UI 카메라 스택 `cameraStack.Count=1:UICamera`(Lobby eval), `save_scene` 2건 success, `list_open_scenes` `isDirty=false`; `concept_manage verify` 두 씬 success, `unity_concept scene Scene_Game` `localPopup`에 `Popup_Setting` 실재
- `confirmed`·`reuse` 미변경 (`Popup_Setting`은 `inAsset`만 `true`로 복원)

## 완료업무

### 프리셋 현황·골격 확인
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_GameFrame`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_RoomChoice`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_RoomHistoryItem`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_AbilityCard`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_EnemyPreview`
`C:\_Projects\Unity_Portfolio\_Data\Preset\Popup\Popup_Setting\Popup_Setting.prefab`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_질문` → 하위 `팝업_질문`·`컨트롤_질문`·`오브젝트_질문`, `게임개발_구성_컨셉_질문`(`concept_manage list`, 두 `씬설정`·`게임컨셉`·`밸런스컨셉` 본문), `게임개발_모듈_질문`(`module_manage list`, `Room`·`Battle`·`Character` `module.md`·스크립트 전문), `게임개발_구성_리소스_질문`(`resource_file list UI`, `Assets/__Game/_Core/Image`·`Resources/Icon`·`Resources/SpriteAnim` 실측), `게임개발_프리셋_파일_생성` → `컨트롤_생성` 5건
- 건너뜀 — 대상 `게임개발_프리셋_노드_생성`·`노드_구성` / 조건 order.md "노드 트리와 규칙이 없으면" / 실측 `preset_node node` 최상위 `Control`·`Object`·`Popup` 3노드 실재(하위 분류 없음, 규칙 문서 없음 — 고정 최상위는 생성 불가)
- 현황 실측(`preset_manage list`): Popup Game `inAsset` 6건·`notInAsset` `Popup_Setting`, Library 5건; Control Game 0건·Library 8건; Object Game 10건. 재사용: `Popup_Notify`(해금·잔액 부족 알림)·`Popup_Quit`·`Control_Blocker`(프레임형 뒤 막기)·`Control_Text` 미사용. 프레임은 Library `Control_Frame` 대신 `Game` 전용 `Control_GameFrame` 신규(스킬 "프레임·슬라이더·탭 Game 전용본" 규칙)
- 컨트롤 생성: `prefab_control create` 5건 → `preset_manage set` description 5건 success → get 재조회 5건 조회됨. create가 동명 스크립트를 부착하지 않아(실측 `get` 루트에 스크립트 없음) 코드 작성 후 부분 patch로 컴포넌트 추가·배선
- `Popup_Setting` 재구성 전제: 원본은 `preset_manage path` `dir=_Data/Preset/Popup/Popup_Setting`. 구 프리팹(2823행, 미존재 guid `f5fd32b4…` 중첩 인스턴스)을 `Popup_Lobby` 골격 YAML을 복제해 `m_Script`를 `Popup_Setting.cs.meta` guid `e761c2db…`로 결선한 120행 골격으로 교체 → `set inAsset=true` → export success → `Assets/__Game/_Core/_UI/Popup/Popup_Setting` 복원 실측. 스크립트(`namespace Library`, `Control_GameFrame` `SetTitle`·`AddCloseListener` 사용)는 무변경

### 팝업·컨트롤 코드 작성
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Lobby\Script\Popup_Lobby.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_HUD\Script\Popup_HUD.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_RoomSelect\Script\Popup_RoomSelect.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Ability\Script\Popup_Ability.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Pause\Script\Popup_Pause.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Result\Script\Popup_Result.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_GameFrame\Script\Control_GameFrame.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_RoomChoice\Script\Control_RoomChoice.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_RoomHistoryItem\Script\Control_RoomHistoryItem.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_AbilityCard\Script\Control_AbilityCard.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_EnemyPreview\Script\Control_EnemyPreview.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\LocalBattleManager.cs`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_팝업_코드_작성`(child `{}`) 6건, `게임개발_프리셋_파일_컨트롤_코드_작성` 5건. `애드온_코드_작성` 건너뜀 — 조건 order.md "필요 시에만" / 실측 컨트롤 최상위 노출(`Set`·`AddClickListener`·`ControlRoot` 슬롯)만으로 팝업 요구 충족, 애드온 대상 0건
- 템플릿 verify: `template_manage test` `module-Popup` 7건(`Popup_Setting` 포함)·`module-Control` 5건 전건 `errors: []`
- 컨트롤 계약: `Control_GameFrame` `SetTitle(string)`·`AddCloseListener(Action<UIWrapper_Button>)`·`SetCloseVisible(bool)`·`ControlRoot`; `Control_RoomChoice` `Set(Sprite,string,string)`·`AddClickListener`·`ControlRoot`(적 미리보기 슬롯 — 컨트롤 안 중첩 프리팹 금지라 미리보기 인스턴스는 팝업이 슬롯에 배치); `Control_AbilityCard` `Set`·`AddClickListener`; `Control_RoomHistoryItem` `Set(Sprite)`; `Control_EnemyPreview` `Set(Sprite,int)`. 번역 로직 없음(팝업 `OnLanguageChanged` 일괄)
- 팝업 로직·MCP: 6건 모두 `MCPDetail`·`MCPInteraction`·`MCPInteract` override(치트 없음 — 매니저 치트로 충분). Lobby: `CharacterManager` `SelectedId`·`GunUnlocked`·`BestRoom` 구독, Gun 미해금 클릭 → `Popup_Notify`(`Text_Core_GunUnlock` 포맷 `{0}`=`Room_GunUnlock`), `Start`→`SceneChangeManager.GameSceneID`(프리팹 실측 `Scene_Game`), `Setting`→`TryOpen("Popup_Setting")`. HUD: `RoomIndex` 변경마다 `LocalBattleManager.Player` HP 재구독(플레이어는 런 시작 시 스폰), 이력 `IconManager.GetIcon(kind)`(반출명 접미 `_Battle` 등 폴백 실측 표시), `WaveIndex/WaveCount`, `BattleManager.Crumb`. RoomSelect·Ability: `OnOpen`에서 `Choices`·`AbilityChoices` 표시, `RerollCount` 구독으로 리롤 후 갱신, 잔액 부족 시 `Popup_Notify`(`Text_Core_NotEnoughCrumb`). Result: `InitGame` 시점 `GunUnlocked` 저장 → 열릴 때 비교로 해금 알림 표시. Pause: `OnInputCancel` — 닫힌 상태 `performed` 취소 입력에서 `m_BlockingPopups`(RoomSelect·Ability·Result) 열림이면 무시, 아니면 열기(씬설정 "취소 입력" 준수)
- 중복 진입 가드 판정: `Popup_Pause` — `OnOpen` 첫 줄 `wasOpened` 가드 + `OnClose` 짝 가드(대상: 시간 정지). 나머지 6팝업 — `OnOpen`이 표시 갱신만 하고 외부 상태를 바꾸지 않아 "대상 아님"
- timeScale 소유자 단일화(지시서 고유 주의사항): `LocalBattleManager`에 `SetPaused(bool)`·`IsPaused` 추가, `HitStopRoutine` 복원값을 `m_IsPaused ? 0 : 1`로, `HitStop`은 일시정지 중 무시, `OnDestroy`에서 `timeScale=1` 복원. `Popup_Pause`는 매니저 null 체크 후 `SetPaused` 위임(없으면 직접). 플레이 실측 Pause 열림 `Time.timeScale=0`, `Resume` 후 진행
- 신규 `[SerializeField]`와 배선 노드(전건 `get` 재조회로 배선 확인): Lobby 14필드(`KnifeCard`·`GunCard`·`StartButton`·`SettingButton`·`Title`·`BestRoom/Text` 등), HUD 7필드(`HpBar/Fill (UIWrapper_Guage)`·`HistoryRoot/Item0~7`·`CrumbRow/Text`·`PauseButton` 등), RoomSelect 4필드(`Frame`·`Choice0/1`·`Preview` 6건·`m_PreviewPerChoice=3`), Ability 5필드(`Card0~2`·`RerollButton`·`CrumbRow/Text`), Pause 8필드, Result 7필드, Setting 11필드(`Frame`·`BgmSlider`·`SfxSlider`·`FullscreenToggle`·`ApplyButton`·`DefaultButton`·라벨 5)
- 도구 실측 결함(우회 아님, 명명 변경으로 대응): `prefab_*` patch에서 노드명 `Name`·필드명 `m_Name`은 `GameObject.m_Name`(리네임)으로 해석되어 배선이 무성 실패(`get` null, jsonPath 리네임도 미반영). 노드를 YAML로 `NameLabel`로 개명하고 컨트롤 필드를 `m_NameLabel`로 바꿔 배선 성공

### 오브젝트 코드 작성
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\Object_UnitBase.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\Object_PlayerBase.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\ProjectileBase.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\SpriteAnimPlayer.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\BattleConst.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Player_Knife\Script\Object_Player_Knife.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Player_Gun\Script\Object_Player_Gun.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Enemy_Apple\Script\Object_Enemy_Apple.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Enemy_Watermelon\Script\Object_Enemy_Watermelon.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Enemy_Banana\Script\Object_Enemy_Banana.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Boss_Pumpkin\Script\Object_Boss_Pumpkin.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Boss_Pineapple\Script\Object_Boss_Pineapple.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Projectile\Script\Object_Projectile.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Background\Script\Object_Background.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Background\Editor\Script\Setup_Object_Object_Background.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Floor\Editor\Script\Setup_Object_Object_Floor.cs`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_오브젝트_코드_2D_사이드뷰_캐릭터_작성`(child `{}`) 플레이어 2·적 3·보스 2, `게임개발_프리셋_파일_오브젝트_코드_작성` 투사체·배경·바닥, `유니티엔진_컴파일_실행` 4회(완료 `completed`·`failed=false`·에러 0)
- 모듈 변경(공유 로직 분리, `module-Object` 템플릿 베이스 제약 `^\w+Base$` 대응): `Object_Unit`→`Object_UnitBase` 개명(참조 7파일 일괄, `.meta` 동반 이동, `module.md` 갱신); 플레이어 공용 `Object_PlayerBase`(입력 폴링·이동·점프·모션 단일 통로 `PlayAnim`·공격 범위 활성·`GetAttackBox`); `ProjectileBase : ObjectBase, IProjectile`(계약 소유); `SpriteAnimPlayer.SetFlip`을 `flipX`에서 렌더러 노드 `scale.x` ±1로 변경(사이드뷰 규칙); `BattleConst` 애니 동작명 5건 추가. `module_manage verify Battle` success
- 사이드뷰 규약 준수 실측: 반전 = `View` 노드 `localScale.x` ±1(`flipX` 사용 0건, `grep flipX` 0), 접지 y = `[LocalRoomManager]` 스폰 위치 `-3.9`(플레이 실측 유닛 y `-3.90`), 이동·거리 판정 x 단일(`FSMState_UnitBase.DistX`·`Physics.Move`), 모션 전환 `Object_PlayerBase.PlayAnim` 단일 함수(동일 동작 재진입 차단), 공격 판정 `View/AttackRange`(반전 루트 자식, 공격 중만 활성) 월드 박스를 `LocalBattleManager.HitBox`에 전달
- Knife: `AttackPressed`→1단, `m_Interval - InputBuffer` 이후 선입력으로 2·3단 연결, 50% 시점 `HitBox`(3단 `isFinish`·`KnockbackDistFinish`), `HitMax`. Gun: `AttackHeld` 동안 정지·`AttackInterval`마다 `Fire(SProjectile)`(`RangeWidth`=최대 비행). 값은 전부 `CharacterData`·`GetPlayer*` 경유
- 입력: `LocalInputManager.Create`가 `LocalManagerBase` 호출자를 요구해(`LocalInputManager.cs:70`) 오브젝트가 등록 불가 → `Keyboard/Mouse/Gamepad.current` 직접 폴링(A/D·←/→ 이동, Space/W/↑ 점프, J·마우스 좌·패드 West 공격, 패드 좌스틱·South)
- 투사체: `Object_Projectile : ProjectileBase`, `Launch`에서 소유 ID별 스프라이트(Gun·Banana·Pineapple), 진행 방향 `scale.x` 반전, `OverlapCircleAll` 명중 → `LocalBattleManager.Hit`, `Pierce` 초과·`MaxDistance` 도달 시 `ReturnProjectile`
- 배경·바닥: `Object_Background` `LocalRoomManager.RoomKind` 구독(없으면 로비 배경); `Setup_Object_*` 2건 `ObjectSetupBase` 상속·`SetupName` = `Object_Background`/`Object_Floor`, `[Stage]` 그룹에 배치 (씬설정 `localObject` 등재 실측 `unity_concept scene`)
- 런타임 결함 수정(플레이 실측 발견): 로비 복귀 시 `SpriteAnimPlayer.Update` `IndexOutOfRangeException` 7건(`SpriteAnimPlayer.cs:48`, 씬 로드 직후 deltaTime 급증으로 루프 프레임 인덱스 초과) → 루프 타이머 `%=` 로 접기, 재현 구간 재플레이 에러 0
- 방어 항목 대조(베이스 `Object_UnitBase`·형제 `FSMState_*`): null 매니저 가드 — 해당(`Object_PlayerBase.Battle` null 체크, `Object_Projectile.Finish` 매니저 부재 시 비활성); 값 클램프 — 베이스 담당(`Object_UnitBase.Heal/TakeHit`); 잘못된 입력 예외 — 해당(`Object_Projectile.PickSprite` 미대응 소유 ID throw); 정적 참조 — 대상 아님(싱글톤 없음); 사망 후 조작 차단 — 해당(`CanControl`)
- 템플릿: `template_manage test module-Object` 8건 `errors: []`. 1차 export 불합격: 스킬 "계약 인터페이스 분리"대로 만든 `Object_ProjectileContract.cs`가 같은 템플릿 glob에 걸려 `regionMissing Event`·`extra`(`: IProjectile` 선언) 2건 → 계약을 모듈 베이스 `ProjectileBase`로 옮기고 파일 삭제, 재export success

### 프리셋 구성·익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\[LocalBattleManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\HitEffect.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\Telegraph.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Character\Prefab\[LocalCharacterManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\Prefab\[LocalRoomManager].prefab`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Lobby\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Game\concept.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Game.unity`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Lobby.unity`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_팝업_구성`(child `{}`) 7건 → `컨트롤_구성` 5건 → `오브젝트_구성` 10건 → `게임개발_프리셋_파일_익스포트` → `유니티엔진_씬_셋업_실행` 2씬. 애드온 구성 건너뜀(애드온 0건)
- 기준 해상도: 팝업 `CanvasScaler` `m_ReferenceResolution` `1920x1080`(골격 YAML 실측), 컨셉아트 캔버스 1672x941 → 환산 배율 1.148. 컨셉아트 대조(Game·Lobby `Overview/art/1.png`): 구도 — HUD 좌상 HP바·중앙 상단 방 배지+이력 열·우상 Crumb, 로비 좌상 최고순번·우상 설정·좌우 카드·하단 START 일치; 크기 비율 — HP바 480x56·카드 440x560·START 560x130(그림 대비 ±10%); 색 — 카드 `UI_Casual_Panel_Item`, START `Button_Box_Green`, 문구색 `(0.33,0.25,0.09)` 계열 일치; 스타일 — `UI_Casual_*` 캐주얼 유지; 텍스처 반복 주기 — 대상 아님(바닥 타일만 `Object_Floor` Tiled 8u)
- 라벨 최장 문구 기준(영·한·일 중 최장, `Text` 테이블 실측): `Text_Core_GameStart` "Start Battle"(START 560x130, 자동 축소 32~64), 방 이름 `Text_Core_RoomAbility`/설명 `Text_Core_RoomHealDesc` "Instantly restore 50% of max HP"(RoomChoice Desc 420x72 자동 축소 18~26), 능력 설명 `Text_Core_AbilityMultiHitDesc`(AbilityCard Desc 284x154 자동 축소 18~26), `Text_Core_Resume`·`Text_Popup_Setting`·`Text_Core_GiveUp`(Pause 버튼 400x100 24~40), `Text_Core_StageResultTitle`(프레임 Title 자동 축소 28~48)
- 팝업 형태·정렬: 베이스형 `Popup_Lobby`·`Popup_HUD`(`m_IsDefaultOpen=true`·`m_FixedOrder=0`), 프레임형 5건(`Control_Blocker` + `Control_GameFrame`, `m_FixedOrder=-1`), `Popup_Pause` `m_IsCloseByCancel=true`. 취소 소비 대조: Lobby — `Popup_Quit` `true`(Library get 실측), Game — `Popup_Pause` `true`, 나머지 `false`(씬설정 "취소 입력"과 일치)
- 런타임 생성 0건: 이력 항목 8·미리보기 6·능력 카드 3·선택 강조·잠금 마크·해금 라벨 모두 프리팹에 `Active:false`로 사전 배치. 알파 0 저장 요소 없음(`PopupAni_Alpha_Smooth`는 `Popup_Setting`만, 초기값은 런타임 연출)
- 도구 한계 대응(규칙 명시 경로): `Slider` 관리 자식 `Fill`·`Handle` 앵커가 patch마다 정규화(실측 `AnchorMax (0,0)`) → YAML 직접 기입으로 `Fill` 전체 스트레치·`Handle` 세로 스트레치 44px, `SfxSlider` `m_FillRect`·`m_HandleRect`·`m_TargetGraphic`도 YAML 결선(재조회 get 정상); `Toggle` `graphic`(dropped key) → YAML; `SpriteRenderer` `m_DrawMode`(Floor Tiled, `m_Size` 185x8)·`m_SortingOrder`(Background -10, Floor -5) → YAML
- 오브젝트 구성: 플레이어 — `Rigidbody2D`(중력 3·회전 고정)·`CapsuleCollider2D` 0.6x1.0·`CharacterPhysics2DSide`(`DefaultPhyicsMat2D_Move`)·`View`(`SpriteRenderer`+`SpriteAnimPlayer` prefix `AnimationSheet_Casual_Player`)·`View/AttackRange`(Knife 2.0x1.5 오프셋 (1,0.75), Gun 1.0x0.5); 적·보스 — 루트 `Active:false`(풀), `BoxCollider2D`(테이블 `HitboxWidth` 기준), `FSM`(`States` 노드에 상태 컴포넌트·`m_ID`, 기본 상태 적 `Move`/보스 `Idle`), `Object_*` 필드 `m_Kind`·`m_Id`·`m_Physics`·`m_Anim`·`m_Fsm`; 투사체 — 루트 비활성·`View` 스케일 0.3·스프라이트 3종 배선; 배경 — `View` 스케일 (2.6,1.6)·방 종류별 스프라이트 5종; 바닥 — y `-3.9`·`BoxCollider2D` 60x2.6·타일 스케일 0.325. `Animator`·`SpriteRenderer` 전건 자식 `View`, 스프라이트 실측 `SpriteRenderer.m_Size` 플레이어 2.0x2.0(PPU128)·배경 19.2x10.8(PPU100)·바닥 8x8
- 스프라이트 실측 높이 대조: 미실측(`eval` 픽셀 스캔 미수행) — Work_3·3_2 레포트의 잉크 높이(플레이어 128px=1.0u, 보스 224px=1.75u, 적 113~138px≈0.9~1.1u, 시트 단위 균일 배율)를 근거로 갈음. 플레이 캡처에서 플레이어·사과 서열 확인
- 연출 요구 3종: 타격 이펙트 — 배선됨(`HitEffect.prefab` `Illust_Casual_Hit_Impact`, `[LocalBattleManager].m_HitEffectPrefab`), 보스 전조 — 배선됨(`Telegraph.prefab` `UI_Common_Shape_Circle128` 틴트 `(1,0.2,0.2,0.35)` 스케일 0.78125=1u, `m_TelegraphPrefab`), 카메라 반응 — 대상 아님(컨셉 미지정), 시간 반응 — 배선됨(히트스톱 `m_HitStopSec=0.06`)
- 매니저 인스펙터(YAML 직접 기입, 프리팹 루트 fileID·guid 결선): `[LocalBattleManager]` `m_EnemyPrefabs` 3·`m_BossPrefabs` 2·`m_ProjectilePrefab`·풀 8/24·`m_SfxHit/Die` null; `[LocalCharacterManager]` `m_PlayerPrefabs` Knife·Gun; `[LocalRoomManager]` 자식 `PlayerSpawn(0,-3.9)`·`SpawnLeft(-9,-3.9)`·`SpawnRight(9,-3.9)` 결선·`m_CameraClampX=8`. 플레이 실측 `playerSpawned=true`·적 3 스폰·바닥 접지
- 씬 셋업: 씬설정 두 문서 `## UI`에 `### Popup_Setting` 추가·"등재 대기"→"포함", `concept_manage verify` success. setup 전 오버라이드 `eval` 기록(엔진 자동 항목 `Canvas.m_Camera`·TMP 해시뿐, 사용자 오버라이드 0) → setup(`Scene_Game` 1차는 컨셉 편집과 동일 배치 실행으로 미반영, 재실행 시 반영) → 저장. `[Global].prefab`은 setup이 갱신(git `M`) — 사용자 오버라이드 없음
- 익스포트: `preset_manage export` 22건 success(`Popup_Setting` 2회 — YAML 결선 후 재export), `module_manage export` 3건 success, `AssetDatabase.Refresh()` `success:true`, `.meta` 신규 전건 실재

## 비고
- 텍스트 결손(데이터 영역, 미수정): `Text_Core_GunUnlock`·`Text_Core_Confirm`(Work_4 기재), 신규 사용 `Text_Core_RoomSelectTitle`(방 선택 제목, 플레이 캡처에 ID 노출). `Text_Popup_Setting_BGM/SE/Fullscreen/Apply/Default`는 `Text` xlsx `Popup_Setting` 시트에 행이 있으나 런타임 `LanguageManager.Get`이 ID를 반환(캡처 실측) — 익스포트 JSON 반영 여부 데이터 Work 확인 필요. 최고 도달 순번·방 순번·웨이브·Crumb는 숫자만 표시(문구 ID 미사용)
- 사운드 결손: `Assets/__Game/_Core/{BGM,SFX,Resources/BGM,Resources/SFX}` 전부 빈 폴더 — `[LocalBattleManager].m_SfxHit/m_SfxDie` null(`SoundManager.PlaySE` null 무시), BGM 재생 미구현
- 데미지 숫자 팝(HUD): 미구현 — `Battle` 모듈에 명중 통지 통로(`SHit` 이벤트)가 없어 모듈 API 추가 필요
- 밸런스 관찰(플레이 실측, 판정은 Work_6): 조작 없는 플레이어가 Apple 3마리에 약 5~7초 내 사망(24 dps 계산치와 근접)
- 에디터 Game 뷰가 세로(1080x1920)라 `qa_ui text`가 카드 라벨 4건을 `offScreen`으로 보고 — `CanvasScaler` 폭 기준 스케일 특성이며 16:9 뷰에서 재확인 필요(프리팹 결함 아님, 캡처 참고)
- 프레임형 팝업 여닫기 연출은 `Popup_Setting`만 `PopupAni_Alpha_Smooth` 배선(설명 문구 요구), 나머지 5건은 연출 없음
- Library `Control_Blocker`·`Popup_Notify`·`Popup_Quit`는 그대로 사용(프레임·슬라이더·탭 아님). 슬라이더·토글은 `Popup_Setting` 안 raw 노드로 구성(별도 `Game` 슬라이더 컨트롤 미생성)
- 스킬 "계약 인터페이스 분리"(`Script/{prefabId}Contract.cs` partial)는 `module-Object` 템플릿 verify(glob `Object_*.cs`, 베이스 `^\w+Base$`)와 충돌해 export 불합격 — 계약을 모듈 베이스로 옮겨 통과. 스킬 문서 갱신 필요
- `prefab_*` patch: 노드명 `Name`·필드명 `m_Name` 배선 불가(도구 결함), jsonPath 단일 키 리네임 미동작 — YAML 개명으로 대응
- 오브젝트 스프라이트 픽셀 실측(`eval` 바운딩 박스 스캔)은 미수행 — Work_3 레포트 잉크 높이로 갈음
- `LocalInputManager` 액션 라우팅 미사용(플레이어가 장치 직접 폴링) — 라이브러리 `Create`가 `LocalManagerBase` 호출자 전제. 액션 맵 경유가 필요하면 `Battle` 모듈에 입력 값 노출 API 추가 필요
- `Object_Unit`→`Object_UnitBase` 개명으로 Work_4 레포트·`module.md`의 `Object_Unit` 표기는 구 명칭이다
