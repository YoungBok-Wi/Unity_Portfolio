# [오케스트레이터_워커_실행] "Job_002 Work_5 팝업·컨트롤·오브젝트 코드 수정, 프리셋 구성·익스포트" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 1·2·3 전부 완료 기준 충족. 컴파일 `recompile_status` `completed`·`failed:false`·`errors:[]`(코드 수정 2회 모두, `Game.dll` 12:46:49·12:49:34 갱신), `get_console_logs --severity=error` `total:0`(컴파일 후·`Scene_Game` 플레이 3회·`Scene_Lobby` 플레이 1회 — 매회 `clear_console` 선행), `preset_manage export` 6회·`module_manage export Battle` 1회 전부 `{"success":true}`, 두 씬 플레이 진입 `editor_status playMode:playing` 실측
- 업무 1(코드): `Popup_HUD` 명중 통지 `LocalBattleManager.HitApplied` 구독 → `Control_DamagePop`(신규, 팝업 전용 컨트롤) 풀(`ObjectPool` 16) 데미지 팝(월드→`LocalCameraManager.CurCam` 스크린→팝 루트 로컬), 이력 상한은 매니저 `RoomConst.HistoryMax`(8)=HUD 슬롯 8과 일치(코드 무변경, `historySlots` MCP 노출 추가), `Popup_Lobby` 선택 카드 파란 스프라이트 교체·미해금 회색 틴트·요리사 일러스트 전환, `Popup_Result.cs:85` `{0}` 포맷 수정, `Control_RoomHistoryItem` 점선 `m_Link`. `template_manage test` 5파일 `errors:[]`
- 업무 2(오브젝트·모듈): `Object_PlayerBase.ResolveAnim` 훅(Battle 모듈) + `Object_Player_Gun` override로 `Idle`→`Idle_Gun`·`Move`→`Move_Gun` — 플레이 실측 `idleAnim=Idle_Gun/AnimationSheet_Casual_Player_Idle_Gun_01`. `Telegraph.prefab`은 편집·플레이 양쪽에서 렌더 실측(`_Temp/Work_5/telegraph_probe.png`·`play_telegraph3.png` 붉은 타원) — 결함 미재현, 프리팹 무변경. `[LocalBattleManager].prefab` `m_SfxHit`·`m_SfxDie`·`m_Bgm` 배선 → 플레이 실측 `sfxHit=SFX_Casual_Battle_Hit sfxDie=SFX_Casual_Battle_Die bgm=BGM_Casual_Battle`, `BattleManager` `AudioSource` `playing=True clip=BGM_Casual_Battle`
- 업무 3(구성·익스포트): HUD 하트 아이콘(`HpBar/Heart`)·데미지 팝 템플릿(`DamagePopRoot/DamagePop`)·이력 점선(`Link/Dot0·Dot1`, `HistoryRoot` 폭 760·간격 24), 로비 `BestRoom/Icon`→`Icon_Casual_Room_Best`·`ChefKnife`/`ChefGun`(360x540) 배치·신규 필드 7건 배선. 1920x1080 `qa_ui text` 두 씬 `truncated:false`·`overflow:false` 전건(`issueOnly:true` 응답 `{}`). 캡처 `_Temp/Work_5/play_hud.png`(하트·데미지 팝 "7")·`play_hud2.png`(이력 2개 점선)·`play_lobby_knife.png`/`play_lobby_gun.png`(파란 카드·별 배지·요리사 전환)
- `confirmed`·`reuse` 무변경. DataMCP 전 호출 정상 응답(`Fallback` 미사용). 씬 저장 없음(`save_scene` 미호출, 종료 시 `Scene_Game` `isDirty:false`)
- 다음 행동: `Scene_Game`에서 플레이어가 무입력 ~7초에 사망(`hp=32` at 5.0s → `Ended`, Gun 80HP·Apple 3마리)하는 밸런스·모듈 결함(범위 밖, `## 비고`)을 다음 Work에서 다룬다

## 완료업무

### 팝업·컨트롤 코드 수정
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_HUD\Script\Popup_HUD.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_HUD\Script\Control_DamagePop.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Lobby\Script\Popup_Lobby.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Result\Script\Popup_Result.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_RoomHistoryItem\Script\Control_RoomHistoryItem.cs`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_팝업_코드_작성`(child `{}`) + 지시서 명시 `게임개발_프리셋_파일_컨트롤_코드_작성` → `유니티엔진_컴파일_실행`
- `Popup_HUD` 신규 `[SerializeField]`: `m_HpIcon`(Image)→`HpBar/Heart`, `m_DamagePopTemplate`(Control_DamagePop)→`DamagePopRoot/DamagePop`, `m_DamagePopRoot`(RectTransform)→`DamagePopRoot`, `m_DamagePopPoolSize`(16). `InitUIOnce`에서 `HitApplied +=`·풀 생성, `OnShutdown`에서 `-=`, `InitUI`에서 `Clear`. 색: 적 명중 노랑(1,0.95,0.4)·플레이어 피격 빨강(1,0.3,0.3). MCP detail `historySlots`·`damagePopActive` 추가
- `Control_DamagePop`(`Popup_HUD/Script/`, "팝업 전용 컨트롤" 규칙 — 프리팹 없음): `m_Label`(TMP_Text)·`m_LifeSec` 0.6·`m_RiseDist` 60, `Show(localPos, damage, color, onDone)`, `Update`가 상승·알파 감쇠 후 콜백. `UIWrapper_Text` 대신 `TMP_Text` 직접 참조("비활성 계층 Instantiate 함정" 회피)
- `Popup_Lobby` 신규 필드: `m_KnifeCardImage`·`m_GunCardImage`(Image)→`KnifeCard`/`GunCard`, `m_CardNormal`(Sprite)→`UI_Casual_Panel_Item`, `m_CardSelected`(Sprite)→`UI_Casual_Button_Box_Blue`, `m_GunGrayTargets`(Graphic[])→`GunCard`·`GunCard/Icon`, `m_ChefKnife`·`m_ChefGun`(GameObject). `RefreshCards`가 선택 스프라이트 교체·일러스트 전환·미해금 틴트 (0.55,0.55,0.55) 적용. 잠금 클릭 `Popup_Notify` 호출·`UnlockText()` `{0}` 포맷은 기존 유지
- `Popup_Result.cs:85`: `string.Format(language.Get(RoomConst.TextGunUnlock), TableManager.instance.Const.Room_GunUnlock)`
- `Control_RoomHistoryItem`: `m_Link`(GameObject) 추가, `Set(Sprite, bool _showLink)` — HUD가 첫 슬롯 외 `true`
- 중복 진입 가드 판정: `Popup_HUD`·`Popup_Lobby` 모두 `OnOpen`/`OnClose` override 없음(외부 상태 변경 없음 — 대상 아님), `Popup_Result` 미변경
- `template_manage test`: `module-Popup` HUD·Lobby·Result, `module-Control` DamagePop·RoomHistoryItem 전부 `{"errors":[]}`. `recompile_status` `completed`·`failed:false`, `get_console_logs` `total:0`, `Game.dll` 12:46:49. 콘솔 비움 외 되돌릴 대상 없음
- `Control_AbilityCard`: 지시서·컨셉·코드에서 추가 필드 필요 항목 없음 — 미수정

### 오브젝트 코드 수정
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\Object_PlayerBase.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\[LocalBattleManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Player_Gun\Script\Object_Player_Gun.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Player_Gun\Object_Player_Gun.prefab`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_오브젝트_코드_2D_사이드뷰_캐릭터_작성`(child `{}`) + `게임개발_모듈_폴더_작성`(Battle) → child `기획_작성`(module.md 1문장)·`코드_작성`(`Object_PlayerBase`)·`프리팹_작성`(`[LocalBattleManager].prefab` YAML) → `게임개발_모듈_폴더_익스포트`(+`유니티엔진_재임포트_실행`) → `유니티엔진_컴파일_실행`
- 모션명 소유: `Object_PlayerBase.UpdateMotion`이 `BattleConst.AnimIdle/AnimMove`를 고정 호출하고 `PlayAnim`이 non-virtual이라 오브젝트 스크립트만으로는 전환 불가(캐릭터 테이블에 모션 키 없음 — `TableCharacter.json` Gun 행 실측) → `protected virtual string ResolveAnim(string)` 훅을 `PlayAnim` 단일 통로에 삽입. 파생 전수: `Object_Player_Knife`(override 없음, 기본 동작)·`Object_Player_Gun`(`Idle_Gun`·`Move_Gun` 매핑). `PlayAnim` 소비처 12건 시그니처 무변경
- 플레이 실측: `p.PlayAnim(AnimIdle)` 후 `Anim.CurAction=Idle_Gun`·sprite `AnimationSheet_Casual_Player_Idle_Gun_01`, 넉백 중 `Jump`·피격 `Hit`·사망 `Die`는 그대로. 프레임 불투명 높이 `Idle_01 128px`·`Idle_Gun_01 127px`·`Move_Gun_01 133px`·`Attack_Gun_01 126px`(ppu 128, 모션 간 차이 있음·같은 오브젝트 ≈1u 유지). `Object_Player_Gun.prefab` `View` 초기 스프라이트 guid를 `Idle_Gun_01`(`59e046c0…`)로 YAML 교체(`m_Size` 보존 목적)
- `Telegraph.prefab`(`Battle` 모듈 소속, 매니저 프리팹 아님 — `프리팹_작성` 처리가능 밖): 원인 실측 — 편집 모드 인스턴스 `sprite=UI_Common_Shape_Circle128 bounds Extents(1.56,0.47) mat=Sprites-Default supported=True layer=0 cullingMask=87 pipeline=PC_RPAsset` 렌더됨(`telegraph_probe.png`), 플레이 중 `ShowTelegraph` 호출 시 `play_telegraph3.png`에 붉은 타원 렌더됨. Job_001 Work_6의 "안 그려짐"은 미재현 — 수정 없음
- `[LocalBattleManager].prefab` YAML: `m_SfxHit`→`SFX_Casual_Battle_Hit`(guid `32f899c6…`), `m_SfxDie`→`SFX_Casual_Battle_Die`(`e776d05f…`), `m_Bgm`→`BGM_Casual_Battle`(`3d5d4592…`, 행 신규). 플레이 재조회(reflection) 3건 일치, BGM `AudioSource.isPlaying=True`
- `module_manage export Battle` `{"success":true}`, `verify allErrors` `{"success":true}`, `get` `localManagerPrefab:"Prefab/[LocalBattleManager]"` 유지. `AssetDatabase.Refresh` 후 변경 5파일 `.meta` 실재. `recompile_status` `completed`·`failed:false`·`Game.dll` 12:49:34, 콘솔 에러 0

### 프리셋 구성·익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_HUD\Popup_HUD.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Lobby\Popup_Lobby.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Control\Control_RoomHistoryItem\Control_RoomHistoryItem.prefab`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5`
**작업내용**
- 수행 스킬: `게임개발_프리셋_파일_팝업_구성`(child `{}`) → `컨트롤_구성` → `오브젝트_구성` → `게임개발_프리셋_파일_익스포트`(+`유니티엔진_재임포트_실행`). `유니티엔진_씬_셋업_실행`은 신규 팝업·모듈 등재 없음(두 씬 `[Popup]` 기존 인스턴스 그대로)으로 미수행
- `Popup_HUD`: `HpBar/Heart`(Image `Icon_Casual_Stat_HP` 72x72, 왼쪽 중앙 앵커), `DamagePopRoot`(full-stretch)/`DamagePop`(`Active:false`, TMP 40 Bold, `Control_DamagePop.m_Label` 자기 TMP), `HistoryRoot` 폭 700→760·`m_Spacing` 8→24. `get` 재조회로 필드 5건(`m_HpIcon`·`m_DamagePopTemplate`·`m_DamagePopRoot`·`m_DamagePopPoolSize`·`Control_DamagePop.m_Label`) 배선 확인. 1차 플레이에서 `m_Label` 미배선으로 `Control_DamagePop.Update` NRE 1000건 → 배선 후 재플레이 `total:0`·`damagePopActive` 동작
- `Control_RoomHistoryItem`: `Link`(24x24, 루트 왼쪽 밖 pivot(1,0.5))/`Dot0`·`Dot1`(`UI_Common_Shape_Circle16` 8x8, 틴트 (0.33,0.25,0.09) — 원본 무채색). `m_Link` 배선 `get` 확인. 플레이 `history=Battle,Battle` 캡처에서 두 아이콘 사이 점 2개 표시
- `Popup_Lobby`: `BestRoom/Icon` `m_Sprite`→`Icon_Casual_Room_Best`(get 확인), `ChefKnife`(활성)·`ChefGun`(`Active:false`) 360x540 중앙 PosY -10(카드 -290..270 안), 필드 7건 배선 get 확인. 플레이 `SelectGun`/`SelectKnife` 조작으로 파란 카드·일러스트 전환 캡처 2장
- 컨셉아트 대조(기준 해상도 1920x1080, 캔버스 1672x941 → 배율 1.148): 구도 일치(하트+HP바 좌상·순번+이력 중앙·Crumb 우상 / 로비 별 배지 좌상·카드 좌우·요리사 중앙·시작 하단·설정 우상), 크기 비율 근사(요리사 540px ≈ 컨셉 470px×1.148), 색 일치(선택 카드 파랑·점선 갈색), 스타일 일치(캐주얼 패널), 텍스처 반복 대상 없음. 차이: 선택 프레임은 기존 노랑 `Select` 유지(컨셉의 노란 반짝임 대응)
- 라벨 기준: 신규 라벨은 `DamagePop`(숫자 최대 3자리, 160x60·40px 1줄)뿐. `GunCard/Desc` 최장 `Text_Core_GunUnlock` Eng "Clear room 5 to unlock the Cream Gun"(400x186·28px 자동축소, 실측 2줄) 기존 유지
- `Popup_Notify` 버튼 라벨: 로비 잠금 클릭은 `Text_Core_Close`("Close"/"닫기"), 해금 알림은 `Text_Core_Confirm`("OK"/"확인") — 198x50 라벨에 1줄. 잠금 상태 실제 열림은 저장값 `gunUnlocked:true`라 미재현
- 취소 소비 대조: 두 팝업 `m_IsCloseByCancel:false` 유지(베이스형, 기존 씬 구성 무변경 — 재판정 대상 아님)
- 익스포트: `preset_manage export` Popup_HUD(2회)·Popup_Lobby·Popup_Result·Control_RoomHistoryItem·Object_Player_Gun 전부 `{"success":true}`, `set` description 4건 `success`. `Refresh` 후 `recompile_status` `up_to_date`(export 후 `.cs` 갱신 0건 — `find -newer Game.dll` 실측)
- 플레이 검증(1920x1080 실측 `Screen 1920x1080`): `Scene_Game` `qa_play get Popup_HUD` `historySlots:8`·`damagePopActive:11→0`, `qa_ui text` 전건 `truncated:false overflow:false`; `Scene_Lobby` `qa_play get Popup_Lobby` `selected` 전환 실측, `qa_ui text`(Popup_Lobby 7건) 전건 이상 없음. 두 씬 콘솔 에러 0

## 비고
- 플레이어 조기 사망: `Scene_Game` 진입 후 무입력 시 Gun(80HP)이 Apple 3마리에게 ~10HP/s 피해를 받아 5.0s `hp=32`·~7s `Ended`(`HealPlayer`·`KillEnemies` 치트 후 웨이브 2에서도 반복). Battle·Room 모듈/밸런스 범위 — 이번 Work 미수정
- 로비 BGM(`BGM_Casual_Lobby`) 재생 호출처 없음: `팝업` 스크립트의 매니저 권한은 읽기("모듈 매니저 읽기 권한" 규칙)라 `Popup_Lobby`에서 `BattleManager.PlayBGM` 호출 불가 — `Scene_Lobby` 로컬 매니저(`Character` 모듈) 측 배선이 필요
- `SFX_Casual_Battle_Attack`·`SFX_Casual_Progress_LevelUp`·`SFX_Casual_Progress_Unlock`: 소비 필드 없음(`LocalBattleManager` 인스펙터는 `m_SfxHit`·`m_SfxDie`뿐) — 미배선
- Gun 미해금 회색·잠금 마크 화면은 저장값 `gunUnlocked:true`라 미실측(코드 경로만 반영)
- `Assets/__Game/_Core/__Scene/Scene_Game.unity`가 git 작업 트리에서 수정 상태(카메라 앵커·`m_Camera` 참조·오브젝트 추가 76+/16-) — 이번 Work는 `save_scene` 미호출·종료 시 `isDirty:false`로 이 변경의 주체가 아님
- `Object_Player_Gun` 오브젝트 구성의 연출 3종(타격·카메라·시간)은 매니저 소관으로 프리팹 배선 대상 아님, 인스턴스화 경로는 `LocalCharacterManager.m_PlayerPrefabs` 기존 배선
- 데미지 팝 풀 복제는 템플릿 노드 사전 배치 + `ObjectPool` 런타임 복제(런타임 생성 예외 — 개수 가변)
