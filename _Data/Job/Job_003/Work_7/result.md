# [오케스트레이터_워커_실행] "Job_003 Work_7 플레이테스트 계획 갱신·전체 재평가·안정화 판정" 업무 레포트

## 요약
- Work 판정: 합격(절차 완수) — 시나리오 43단계 중 41 실행(합격 37·부분 3·불합격 1), 미실측 2(Knife 콤보 키 입력·`Start` 3연타), 비정상 입력 3종·회피 3종·저장 유지·반복 누수 판정. 플레이 3세션 전부 종료 `editor_status playMode: stopped`·`Scene_Lobby isDirty:false`, 게임 코드 콘솔 에러 0(마지막 세션 `total 1`은 제 CLI 탐색 `simulate_key` 인자 누락 로그 — `## 비고`)
- 안정화 판정: **출시 가능(진행 차단 0건)** — Job_002 체감 6·미관 4 중 9건 해소·1건(에디터 디버그 배지) 대상 외. 신규 체감 4건(벽 밀착 근접 교착, 로비 취소 입력의 `Popup_Quit` 토글, 접촉 시 플레이어 가려짐, 원거리 Banana 미발사)·미관 3건(보스방 BGM 속도 1.0, 공격 시작 SFX 미배선, 보스 높이 근사)은 출시 전 수정 권고 항목이며 어느 것도 루프를 막지 않는다 (`## 비고` 판정·대조표)
- Job_003 수정분 판정 전건 합격: 접지(스폰 첫 프레임 `fly=None`·`Idle_03`/`Idle_Gun_03`, 무입력 전 프레임 `Jump` 0건), 접촉 밀림(27.00s 1778프레임 비경직 `|vx|>0.01` 0건, 적↔플레이어 `IgnoreCollision` 전건 true), 스폰 `|x|≤11`(방 2~10 전 스폰 ±10.00/±11.00), 적 발 위치(`colMin −2.435`, Apple 잉크 바닥 857px vs 플레이어 863px), 로비 BGM(`BGM_Casual_Lobby playing=True`), 전조 가로 타원(2.00×0.70·y −2.39, 동일 프레임 캡처 실재), 해금 문구("Cream Gun unlocked!" Notify·`UnlockLabel`), `Popup_Quit` 번역문, Apple 6·무입력 생존 Knife ≥9.76s·Gun ≈7.3s, 로비 카메라 `ortho=4`
- 자연 플레이 방 클리어(치트 0회): Gun 방 1 `wave 1/2→2/2→Choosing` 7.9s, `hp=80/80`·`crumb=14` — 사격은 실제 `j` 키 홀드, 방향 전환만 코루틴이 public `SetFacing`으로 대체(`simulate_key` 1회 2.6s 실측으로 키 전환 불가) — 대체 경로 기재
- PlayerPrefs 초기화 첫 실행 검증 완료(레지스트리 게임 키 0건 → `gunUnlocked=false bestRoom=0 selected=Knife bgmVolume=1`), 최종 저장 `GunUnlocked=True BestRoom=11 SelectedCharacter=Gun BGMVolume=0.4`
- 산출물: `_Temp/QA/시나리오_Scene_Lobby.md`·`치트_Scene_Game.md`·`치트_Scene_Lobby.md`, 캡처 45장 `_Temp/Work_7_J3/cap/`, 스크립트 `_Temp/Work_7_J3/qa.sh`·`advance.sh`·`hook.cs`, 계층 `hier_lobby.json`·`hier_game.json`. 코드·씬·프리팹·테이블 무변경, `confirmed`·`reuse` 무변경, DataMCP `Fallback` 미사용, 사용자 질문 없음
- 다음 행동: 사용자가 `## 비고` "안정화 상태 판정"으로 출시·추가 루프를 결정한다 (권고: 체감 신규 4건 중 벽 교착·취소 입력 2건 수정 후 출시)

## 완료업무

### 플레이테스트 계획 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Game.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7_J3\hier_lobby.json`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7_J3\hier_game.json`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_질문`(선행) → `QA_유니티게임개발_플레이테스트_계획`(child `{}`) → `QA_유니티게임개발_플레이테스트_치트_작성`. 정본 `_Data/Concept/{Balance,Game,Resource,Scene_Lobby,Scene_Game}/concept.md`(`concept_manage get` 5건 `reuse:"add"`), 값 대조 `Assets/_Library/_Core/Resources/Table/TableEnemy.json`(Apple `Attack=6`)·`TableText.json`(`Text_Core_GunUnlocked`·`Text_Quit_Title`·`Text_Quit_Text` 실재 — Job_002 결손 2건 해소)·`TableConst.json`. 문서 불일치·정의 결손 없음
- 두 씬 조작 항목: 계층 `get_scene_hierarchy` 2씬(Job_002와 차이 없음, `Scene_Game`은 열어서 조회 후 `Scene_Lobby` 재오픈 `isDirty:false`), 로비·게임 플레이 `qa_play get`·`qa_cheat get`(첫 실행 `[LocalCharacterManager] UnlockGun` 노출, 게임 `[LocalBattleManager]` 9종). 치트 실행 확인: `UnlockGun` → `gunUnlocked=true bestRoom=5 Lock=False`, `KillEnemies` → `alive 3→0 crumb 0→6`(`Ended` 상태에서도 적용), `HealPlayer` 사망 후 무효(`hp=0` 유지)
- 시나리오 43단계(Job_002 39 + 추가 4: Quit 문구·무입력 27s·자연 클리어·스폰 위치, 강화 4: 접지·발 위치·전조·Gun 시트)·비정상 입력 3종·저장 유지 4건·컨셉아트 유사성 작성, 실물 대조 절(계획 시점·실행 중) 기록. 치트 명세 등록 13종·미등록 5종 위임 유지
- 저장 초기화는 편집 모드 `eval PlayerPrefs.DeleteAll()`(계획·실행 전 2회) — 초기화 전 `GunUnlocked=True BestRoom=11 SelectedCharacter=Knife`·`BGMVolume` 키 없음

### 전체 재평가·안정화 판정
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_7_J3\cap`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7_J3\qa.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7_J3\advance.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7_J3\hook.cs`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_테스트`(child `{}`, `error.md` 빈 파일). 플레이 진입·종료는 `유니티엔진_게임_실행` 절차(`clear_console`→`editor_play`→`playing` 폴링 / `get_console_logs --severity=error`→`editor_stop`→`stopped` 폴링). Game 뷰 `_Temp/Work_7/gv.cs` eval로 1920x1080 고정(`Screen 1920x1080`·`aspect 1.778` 실측). 시간 민감 단계·Battle 방은 `eval` 직접 호출(`MCPInteract`·`MCPCheatApply`·`SelectRoom`·프레임 로거 `hook.cs`), 팝업·로비는 `qa_play` interaction
- A 로비(첫 실행): 1 합격(`selected=Knife gunUnlocked=false bestRoom=0`, `Lock=True`, `GunCard` (0.55,0.55,0.55), `BestText=0`, `qa_ui` Popup_Lobby 8건 이상 0, lobby_01) / 2 합격("Clear room 5 to unlock the Cream Gun", lobby_02) / 3 합격 / 4 합격(라벨 6건 1줄, lobby_03) / 5 합격(`bgmVolume=0.4`·`AudioSource vol=0.40` 즉시) / 6 합격("Settings applied", 레지스트리 `BGMVolume=0.4`, lobby_04) / 7 합격 / 8 합격(`[BattleManager] clip=BGM_Casual_Lobby playing=True loop=True`) / 9 합격("Quit Game"·"Are you sure you want to quit the game?"·Yes·No, ID 원문 0, lobby_05) / 10 합격(`room=1 Battle Playing wave 1/2 alive=3 hp=100/100`, `BGM_Casual_Battle playing=True vol=0.40`)
- B 전투방 1(Knife): 11 합격(스폰 첫 프레임 `Idle_03 fly=None y −2.395`, 31s 전 프레임 `fly=None`, `Jump` 0건) / 12 합격(훅 기준 사망 9.76s·첫 피격 1.85s, 17타×6 = 102, 훅이 스폰 ≈1s 뒤라 스폰 기준 ≥9.8s — 목표 8~15s) / 13 합격(27.00s 1778프레임: 비경직 `|vx|>0.01` 0건, 0.8u 내 접촉 1705프레임, x −0.467~0.333 넉백 왕복만, keep-alive 8회) / 14 합격(`d` 홀드 `Move_01~06`·`fly=None`, 해제 `Idle`) / 15 합격(우벽 정지 `x=11.70`, 콜라이더 max 12.0, 적 max 11.59) / 16 합격(개활지 `2/1`, 벽 `2/0`) / 17 합격(`damagePopActive` max 3, 캡처 "6", `Heart` `Icon_Casual_Stat_HP`) / 18 합격(자연 클리어 — 위 "요약")
- C 방 선택·능력: 19 합격(Heal Knife 52→100, Gun 66→80 = min(max, h0+50%)) / 20 합격(`MultiHit,HealMacaron,Attack`·`rerollCost=10`·`crumb=14`, 9건 이상 0, game_14) / 21 합격("Not enough Crumbs"·choices 유지, game_16) / 22 합격(→`MultiHit,HealMacaron,AttackSpeed`·`rerollCost=15`·`crumb 14→4`) / 23 합격(`Select0`×3 → `ability_MultiHit=1`) / 24 합격(Ability·RoomSelect 열림 중 `escape` → `Popup_Pause` 미열림·`timeScale=1.00`)
- D 해금·이력·스폰·보스: 25 합격(방 5 `gun=True best=5`, Notify "Cream Gun unlocked!" 1줄, game_08; SFX 재생 순간 미계측) / 26 합격(방 2~10 스폰 프레임 전건 x ±10.00/±11.00, 웨이브 수 방 6 6·방 9 7·방 10 8) / 27 합격(적 `colMin −2.435 fly=None sprMin −2.945`(캔버스 하단, 잉크선 = 피벗) 전건) / 28 합격(방 9 `history` 8·`Item0~7`, 첫 Battle 탈락, game_09) / 29 합격(`Telegraph(Clone)` 2.00×0.70 y −2.39 = 보스 발 y, 동일 프레임 캡처 game_18_tel_ingame_0_1·1_1에 바닥선 위 붉은 가로 타원 2개; `unitscap` 캡처는 eval 프레임과 어긋나 미노출) / 30 합격(`Win roomIndex=11 crumbTotal=44 gunNewlyUnlocked=true`, `UnlockLabel` "Cream Gun unlocked!", 6건 이상 0, game_11) / 31 합격(`Lock=False` (1,1,1) `best=11`, 로비 BGM 복귀) / 32 합격(`sel=Gun ChefGun=True`) / 33 합격(종료 후 레지스트리 `0.4/11/True/Gun`, 재진입 `sel=Gun gun=True best=11 vol=0.40`)
- E Gun·경계·화면·텍스트: 34 합격(`Idle_Gun_03`→`Attack_Gun_01~04`, `Idle`/`Move`(Knife) 0건) / 35 합격(입력 없이 훅 기준 6.79s·첫 피격 1.79s, 스폰 기준 ≈7.3~7.8s — 목표 6~12s) / 36 미실측(`simulate_key j` press×3에 `Attack` 프레임 0건 — 도구 한계, Job_002 리플렉션 합격 유지) / 37 합격(벽 정지 90s `dx=0`, 적 `|x|≤11.59`) / 38 합격(로비·게임 `ortho=4`, `cam.x=4.89`) / 39 부분(플레이어 132~133px = 12.3%, Apple 111px(규격 119±12), Apple 바닥 857 vs 플레이어 863px; 보스 219px = 20.3% — 규격 237±15 하한 3px 미달) / 40 합격(팝업 8종 `qa_ui` 전수 `Text_` 접두 0건·`issue` 0건, `Popup_Quit` 포함) / 41 부분(`m_SfxHit=SFX_Casual_Battle_Hit m_SfxDie=SFX_Casual_Battle_Die m_Bgm=BGM_Casual_Battle` 배선·전투 BGM 재생 합격, 공격 시작 SFX 필드 없음·보스방 `pitch=1.00` — 불일치 2건) / 42 합격(`KillPlayer` → `Lose`, 자연 사망 Lose 3회) / 43 합격(`escape` → `Popup_Pause paused=true timeScale=0` hp·time 2s 고정, game_21; `GiveUp` → `Scene_Lobby`·로비 BGM)
- 비정상 입력: 취소 — **불합격**(로비 Notify+Setting 열림 중 `escape` → 상단 팝업이 닫히지 않고 `Popup_Quit opened=true`(순서 4 < Notify 83이라 화면 미노출, lobby_05b), 재입력으로 Quit 닫힘 — `씬컨셉 Scene_Lobby` "취소 입력" 위반), 팝업 없음 → Quit 열림·`Cancel` 닫힘 합격, 게임 `escape` → Pause 합격, RoomSelect·Ability·Result 열림 중 무시 합격. 연타 — RoomSelect `Select0`×3 응답 3건 `success`·`room 2→3` 1회·팝업 중복 0, Ability `Select0`×3 1스택, `Start`×3 미실측. 중도 이탈 — Pause `GiveUp` 로비 복귀 합격
- 회피 3종: 공간 끝 대기 — **불일치**(벽 밀착 시 근접 교착, `## 비고`), 사거리 밖 유지 — 방 4 Banana 15s 표본 `proj=0`·d 0.44~1.16u `Move`(유지 거리 5u 미달, 후퇴가 뒤 개체에 막힘) 불일치, 무입력 방치 — Knife ≥9.8s·Gun ≈7.5s 합격(산식 8.19·7.07)
- 반복 누수: 투사체 풀 자연 플레이 연사 후 `proj 0/24`, 유닛 풀 `units 5/27`(활성 = 생존 개체), 방 15회 ClearRoom 후 `alive` 정상 — 누수 없음. 콘솔 에러 게임 코드 0
- 컨셉아트 유사성: 로비(lobby_01·lobby_07 vs `_Data/Resource/File/Concept_Scene_Lobby/Overview/art/1.png`) 구도○·배치○·크기 비율○·색○·스타일○·요소 존재○·텍스처 대상 없음○·라벨 여백○ = 8/8. 게임(game_06·game_19 vs `Concept_Scene_Game/Overview/art/1.png`) 구도○·배치○(플레이어·적 발 동일 바닥선, Idle 포즈 — Job_002 ✕ 해소)·크기 비율○(규격값)·색○·스타일○·요소 존재○(하트·점선·데미지 팝)·텍스처○·라벨 여백○ = 8/8. 팝업 8종 화풍 3특징(둥근 벡터 외곽선·픽셀 격자 없음·플랫+광택) 전건 일치
- 개체 수치: 방 2 Apple 34·Watermelon 104, 방 5 48/40/144, 방 9 66/55/198, 방 10 71/212, Pineapple 방 8 922·방 11 1125 — 성장식 전건 일치. 피격 Apple 6(방 1)·8(방 4)·Watermelon 16(방 4)

## 비고
**안정화 상태 판정**
- 출시 가능 여부: **출시 가능** — 진행 차단 0건, 첫 실행→해금→승리→저장 유지→재시작→Gun 런·자연 클리어·포기 복귀 전 경로 완주. 신규 체감 4건은 출시 전 수정 권고(특히 벽 교착·취소 입력), 미관 3건은 출시 후 보정 가능
- 진행 차단(0건): 없음
- 체감(신규 4건): ① 벽 밀착 근접 교착 ② 로비 취소 입력의 `Popup_Quit` 토글 ③ 접촉 시 플레이어가 적에 가려짐 ④ 원거리 Banana가 근접 열에 끼여 미발사
- 미관(신규 3건): ⑤ 보스방 BGM 속도 1.0(컨셉 1.1) ⑥ 공격 시작 SFX 미배선 ⑦ 보스 화면 높이 219px(규격 하한 222)
- 대상 외(유지 1건): 에디터 한정 디버그 콘솔 배지(캡처 우측, 릴리스 판정 아님)

**Job_002 Work_7 판정 항목 대조 (체감 6·미관 4)**
| 분류 | Job_002 항목 | 이번 판정 | 실측 근거 |
|---|---|---|---|
| 체감 | 플레이어 접지 실패(상시 Jump, Gun 시트 미표시) | 해소 | 스폰 첫 프레임 `fly=None`·`Idle_03`/`Idle_Gun_03`, `Move_01~06`·`Attack_Gun_01~04`, 무입력 31s `Jump` 0건 |
| 체감 | 적 접촉 밀림 표류 | 해소 | 27.00s 1778프레임 비경직 `|vx|>0.01` 0건, x −0.47~0.33, `IgnoreCollision` 전건 true |
| 체감 | 적 스프라이트 부유 0.56u | 해소 | Apple `colMin −2.435`, 잉크 바닥 857px vs 플레이어 863px |
| 체감 | 로비 BGM 무음 | 해소 | `[BattleManager] clip=BGM_Casual_Lobby playing=True loop=True`, 복귀 시 재개 |
| 체감 | 웨이브 등장 벽 밖(±12~13) | 해소 | 방 2~10 스폰 프레임 전건 ±10.00/±11.00 |
| 체감 | Gun 무입력 생존 6.7s(하한 미달) | 해소 | Gun ≈7.3~7.8s(목표 6~12), Knife ≥9.8s(8~15) |
| 미관 | 해금 알림·결과 라벨 조건 문구 | 해소 | Notify·`UnlockLabel` "Cream Gun unlocked!" |
| 미관 | `Popup_Quit` ID 원문 2건 | 해소 | "Quit Game"·"Are you sure you want to quit the game?" |
| 미관 | 전조 세로 타원(y −1.5) | 해소 | 2.00×0.70 가로, y −2.39 = 발 y, 동일 프레임 캡처 |
| 미관 | 디버그 콘솔 배지 | 대상 외 | 에디터 한정(Work_6 비고) |

**결함 목록 (수정 대상 — 재현 절차·실측값·담당 영역, 수정하지 않음)**
- ① [`모듈` Battle] 벽 밀착 근접 교착 — 재현: 방 1 Apple 3마리와 접촉 중 `d`/`a` 홀드로 무리를 통과해 벽(x ±11.70)에 정지. 실측: 좌벽 12s 피격 0건(로그 288→288), 우벽 1회차 t 374.64 이후 90s 피격 0건 — 최근접 Apple d=0.11 `slot=none`·`Move_01`·`vx=0`, 슬롯 보유 2마리 d=0.92/1.74 `vx≈0`(앞 개체에 막힘); 우벽 2회차는 최근접이 슬롯 보유 → 12s 11타 정상(배정 순서 의존). 코드: `Assets/__Game/Battle/Script/FSMState_EnemyMove.cs` 슬롯 없으면 대기·있으면 `StopDistance<dist`에 `Move`, `LocalBattleManager.RequestMeleeSlot` 배정이 거리순이 아님 + 적↔적 콜라이더 통행 차단. 영향: 벽에 서면 무적(exploit)·적이 멈춰 보임. 캡처 game_04·game_05
- ② [`모듈` Input/Popup] 로비 취소 입력이 열린 팝업을 닫지 않음 — 재현: `Popup_Setting`→`Apply` Notify 열림 상태 `escape`. 실측: `Popup_Quit opened=true`(Notify·Setting 유지), Quit `sortingOrder 4` < Notify 83·Setting 2 사이라 화면에 미노출(lobby_05b), `escape` 재입력 → Quit 닫힘(토글). 기대: `씬컨셉 Scene_Lobby` "열린 팝업이 있으면 그 팝업이 닫힌다". 담당: `모듈`(로비 취소 입력 → `LocalPopupManager` 최상단 닫기 전달)
- ③ [`프리셋 구성`] 접촉 시 플레이어가 적에 가려짐 — 실측: `Object_Player_*`·`Object_Enemy_*` 프리팹 `m_SortingOrder` 0 동일, 겹침 허용으로 Apple 3마리가 플레이어를 덮어 모자만 보임(game_01·game_06). 담당: `프리셋 구성`(플레이어 정렬 순서 > 적)
- ④ [`모듈` Battle] 원거리 Banana 미발사 — 재현: 방 4(Apple 3·Watermelon·Banana) 무입력 15s. 실측: Banana d 0.44~1.16u `Move_01`·`vx=-0.26`, `proj=0` 전 표본 — 후퇴(`dist<StopDistance×0.7 → Move(-dir)`)가 뒤 Apple·Watermelon에 막혀 유지 거리 5u 복귀 실패. 담당: `모듈` Battle(원거리 후퇴 경로·적↔적 충돌)
- ⑤ [`모듈` Battle] 보스방 BGM 속도 — 실측 방 8·11 `[BattleManager] pitch=1.00`, `씬컨셉 Scene_Game` "보스방은 재생 속도 1.1배". 담당: `모듈` Battle
- ⑥ [`모듈` Battle] 공격 시작 SFX 미배선 — 실측 `LocalBattleManager` 필드 `m_SfxHit`·`m_SfxDie`·`m_Bgm`만, `Assets/__Game/_Core/SFX/SFX_Casual_Battle_Attack.ogg` 실재·코드 참조 0(`게임컨셉` "타격 사운드": 공격 시작에 `SFX_Casual_Battle/Attack`). 담당: `모듈` Battle(+`프리셋` 배선)
- ⑦ [`리소스 제작`] 보스 화면 높이 — 실측 Pineapple 219px/1080(game_19 잎 끝~바닥, 20.3%), 규격 237±15(21.9%) 하한 3px 미달, Job_002 215px. 담당: `리소스 제작`(`AnimationSheet_Casual_Boss` 기준 높이 224px 확인)

**참고(결함 아님)**
- 방 4 혼합군 무입력 표류(x 0→−8.27/22s): 프레임 로그에서 위치 변화가 경직 프레임에만 ±0.4~0.5u(Watermelon 16 피격이 항상 우측) — `게임컨셉` "피격 넉백으로만 위치 변화" 범위, 물리 접촉 0(`GetContacts` 바닥만)
- 스폰 오프셋 클램프로 같은 x 겹침(방 5 Watermelon·Banana −11.00) → 물리 분리로 Banana −11.59(벽 안). `밸런스컨셉` 범위 안
- `KillEnemies`가 `Ended` 상태에서도 적용(웨이브 진행 없음), `HealPlayer`는 사망 후 무효 — 치트 가드 참고

**미실측·도구 한계**
- Knife 콤보 키 입력(36): `simulate_key j` press 3회에 `Attack` 프레임 0건(Job_001·002 동일), 리플렉션 대체는 Job_002 합격 유지. `Start` 3연타 미실행
- `simulate_key` 1회 2.6s·`eval` 왕복 3~5s: 자연 플레이 방향 전환은 public `SetFacing` 코루틴 대체, Pause 진입 2회는 `escape` 도달 전 사망(재시도로 완료)
- 캡처 프레임 ≠ eval 프레임(`ScreenCapture` 지연): 전조·HUD 대조는 게임 내 코루틴 동일 프레임 캡처로 판정
- `SFX_Casual_Progress_Unlock`·타격 SFX 재생 순간, HitStop 배율 미계측

**기타**
- 콘솔 에러: 마지막 세션 `total 1` = `ExecuteCommandByName: Parameter validation failed: Required parameter 'key' is missing or empty`(제 `simulate_key` 인자 탐색 호출, 파이프라인 서버 로그) — 게임 코드 예외 0, `qa_play`·`qa_cheat` 응답 예외 문자열 0
- 저장 데이터: 계획 전 `True/11/Knife/없음` → `DeleteAll`(계획·실행 전 2회) → 최종 `GunUnlocked=True BestRoom=11 SelectedCharacter=Gun BGMVolume=0.4`(되돌릴 수단 없음)
- `eval` 코루틴·`HitApplied` 구독은 플레이 종료로 소멸, `Scene_Lobby isDirty:false` 3회 전부
