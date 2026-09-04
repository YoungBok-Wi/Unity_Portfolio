# [오케스트레이터_워커_실행] "[유니티게임QA] 플레이테스트 계획 갱신·전체 재평가·안정화 판정" 업무 레포트

## 요약
- Work 판정: 합격(절차 완수) — 시나리오 53단계(Job_003 43 유지 + 추가 44~53, 강화 1·13·18) 중 52 실행(합격 49·부분 1·불일치 2), 미실측 1(36 Knife 콤보 키 — `simulate_key` press 미반영, 리플렉션 대체 48 합격), 비정상 입력 3종·회피 3종·저장 유지·반복 누수 판정. 플레이 5세션 전부 종료 `editor_status playMode: stopped`·`Scene_Lobby isDirty:false`
- Job_003 결함 ①~⑦ 전건 해소: ① 벽 밀착 우/좌 12s 피격 12/13(첫 5.87/5.86s ≤ 6.97s) ② 로비 Notify+Setting 열림 중 `escape` 1회 Notify만 닫힘·Quit 미열림 ③ 정렬 Player 2·Apple 1, 접촉 캡처 전신 노출 ④ 방 3 Banana 15s 발사 6회(첫 1.69s) ⑤ 보스방 `pitch=1.10`(방 6 Pumpkin·방 11 Pineapple)·일반 방·로비 1.00 ⑥ 공격 SFX Knife `StartStep(1)`·Gun `Fire()` 같은 프레임 `[SoundManager] playing False→True` ⑦ Pineapple `Idle_01/03/04` 236.3/234.1/228.9px(222~252), `Idle_02` 263.7px 동작 신축 허용(Work_3 판정)
- 안정화 판정: **출시 보류 권고 — 진행 차단 후보 1건(신규)**. `InvalidOperationException: Apple 풀이 비었다 (크기 8)` — 사망 연출 중(풀 미반납) 개체 + 다음 스폰 합 > 풀 8일 때 `SpawnWave`가 예외를 던진다(치트 `KillEnemies` 연속으로 2회 실측, 자연 플레이는 후반 방 다단 히트 전멸 직후 웨이브 전환에서 도달 가능한 조건). 신규 체감 1건(무입력 넉백 일방 누적 표류 — 27s에 x −0.07→11.70 우벽 도달, Job_003 ±0.5 왕복 대비 악화), 미관 4건(Banana<Apple 크기 위계, 보스 모션 간 높이 편차, `[Global]/[DelegateManager]` Missing Prefab, 로비 제목 "Game") — `## 비고`
- 산출물: `_Temp/QA/시나리오_Scene_Lobby.md`(53단계)·`치트_Scene_Game.md`·`치트_Scene_Lobby.md`, 캡처 36장 `_Temp/Work_6_J4/cap/`, 드라이버 `_Temp/Work_6_J4/qa.sh`·`driver_a.cs`·`driver_a2~a4.cs`·`driver_b.cs`·`driver_c.cs`·`sfx2.cs`, 계층 `hier_lobby.json`·`hier_game.json`. 코드·씬·프리팹·테이블 무변경(`git status` 변경은 `_Temp`·`_Data/Job`뿐), `confirmed`·`reuse` 무변경, `editor_util setup` 미실행, DataMCP `Fallback` 미사용, 사용자 질문 없음
- 다음 행동: 오케스트레이터가 `## 비고` "안정화 상태 판정"의 진행 차단 후보(유닛 풀 고갈)와 표류 체감 결함을 다음 회차 편성 대상으로 판단한다

## 완료업무

### 플레이테스트 계획 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Game.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\hier_lobby.json`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\hier_game.json`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_질문`(선행) → `QA_유니티게임개발_플레이테스트_계획`(child `{}`) → `QA_유니티게임개발_플레이테스트_치트_작성`. 정본 `concept_manage get` Balance·Scene_Lobby(`reuse:"add"`), 개정 문구 실독(`밸런스컨셉` 76~79·95·117행, `게임컨셉` 72·171행, `리소스컨셉` 47·52·84·134행, `씬컨셉 Scene_Lobby` 111행·`Scene_Game` 22행), 고정값 `const_data get consts.Battle_BossBgmPitch` `{"type":"float"}`·`TableConst.json` 1.1, `TableEnemy.json` Banana `AttackInterval=2 StopDistance=5 Range=7`. 문서 불일치·정의 결손 없음
- 계층 대조: `get_scene_hierarchy` 두 씬(`Scene_Game`은 열어서 조회 후 `Scene_Lobby` 재오픈 `isDirty:false`) — Job_003 대비 차이 2건: `[Global]/[DelegateManager]`가 `(Missing Prefab with guid: 834215310b3c54f4184233ac78424713)` 노드(`Transform`만)로 바뀜, `[PlatformManager]` 소멸(커밋 `a77ee68`). guid 참조처 `Assets/__Game/_Core/Prefab/[Global].prefab`, `DelegateManager` 코드·에셋 0건
- 계획 세션 실측(2026-09-05 06:55~07:02, `DeleteAll` 후 첫 실행): `qa_play get` 로비 첫 실행 상태 전건 일치(`[LocalCharacterManager]` detail에 `lobbyBgm` 추가 노출), `qa_cheat get` `UnlockGun` → apply `gunUnlocked=true bestRoom=5 Lock=False`(레지스트리 즉시 저장), 게임 `[LocalBattleManager]` 9종 → `KillEnemies` `alive 3→0 crumb 0→6`, `HealPlayer` 사망 후 무효. 종료 `stopped`·콘솔 에러 0
- 시나리오: 53단계 + 비정상 입력 3종 + 저장 유지 4건 + 컨셉아트 유사성, 추가 44~53(벽 밀착 좌우·Banana 발사·보스 BGM·공격 SFX·전신 노출·로비 취소 3단·보스 높이·크기 위계·Missing Prefab), 강화 1(PlayerPrefs 초기화 첫 실행)·13(넉백 누적 표류 기록)·18(자연 클리어 중 SFX 전이). 치트 명세 등록 13종·미등록 5종 위임 유지, 시간 제어 항목 추가

### 전체 재평가·안정화 판정
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\cap`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\qa.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\driver_a.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\driver_a4.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\driver_b.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J4\driver_c.cs`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_테스트`(child `{}`, `error.md` 빈 파일). 플레이 진입·종료는 `유니티엔진_게임_실행` 절차(`clear_console`→`editor_play`→`playing` 폴링 / `get_console_logs --severity=error`→`editor_stop`→`stopped` 폴링). Game 뷰 1920x1080 고정(`_Temp/Work_7/gv.cs`, `Screen 1920x1080` 실측). 팝업·로비는 `qa_play` interaction, Battle 방은 엔진 내 코루틴 드라이버가 셸 플래그(`w6_go`)에서 멈춰 `qa_play`·`simulate_key`·캡처를 끼워 넣는 방식. `Start`는 스폰 프레임 로그가 필요한 3회에 한해 `eval MCPInteract`(대체 경로)
- A 로비(첫 실행, `DeleteAll` 후 레지스트리 게임 키 0건): 1 합격(`selected=Knife gunUnlocked=false bestRoom=0 bgmVolume=1`, `Lock=True` (0.55,0.55,0.55), `BestText=0`, `qa_ui` Popup_Lobby 7건 이상 0, lobby_01) / 2 합격("Clear room 5 to unlock the Cream Gun", lobby_02) / 3 합격 / 4 합격(6건 1줄, lobby_03) / 5 합격(`bgmVolume=0.4`·AudioSource `vol=0.40`·슬라이더 0.40, lobby_04) / 6 합격("Settings applied", 레지스트리 `BGMVolume=0.4`, lobby_05) / 7 합격(50의 `escape` 2회로 두 팝업 닫힘) / 8 합격(`clip=BGM_Casual_Lobby playing=True loop=True`) / 9 합격("Quit Game"·"Are you sure you want to quit the game?"·Yes·No, lobby_06) / 10 합격(`room=1 Battle Playing wave 1/2 alive=3 hp=100/100`, `BGM_Casual_Battle vol=0.40 pitch=1.00`, game_10)
- B 전투방 1(Knife): 11 합격(스폰 첫 프레임 `Idle_01 fly=None y −2.397`, 무입력 전 프레임 `Jump` 0) / 12 합격(스폰→hp 0 11.37s·첫 피격 2.88s·17타×6, 목표 8~15s) / 13 **부분**(27s 1790프레임 비경직 `|vx|>0.01` 0건·접촉 729프레임 — 물리 밀림 없음 합격, 넉백 49회 전부 한 방향으로 누적돼 x −0.067→11.695 우벽 도달 — `## 비고` 결함 ⑨) / 14 합격(`a` 홀드 `Move_01~06` 41건·`vx=−5.00`, 해제 `vx=0`·`Idle`) / 15 합격(`d` 홀드 벽 정지 `px=11.695 colMax=11.995`, `WallRight` 12.5) / 16 합격(벽 밀착 시 슬롯 좌 2/우 0) / 17 합격(피격 중 `damagePopActive=2`, 하트 캡처 실재) / 18 합격(Gun 런, 치트 0회: `j` 홀드 + `SetFacing` 코루틴, 9.59s에 `wave 1/2→2/2→Choosing`·`hp=44`·`crumb=14`, SFX 전이 12회·투사체 최대 3, game_18)
- C 방 선택·능력: 19 합격(Heal 51→100·88→100 = min(max, h0+50)) / 20 합격(`MultiHit,Attack,HealMacaron`·`rerollCost=10`·`crumb=14`, 9건 이상 0, game_20) / 21 합격("Not enough Crumbs"·선택지 유지, game_21) / 22 합격(→`MultiHit,Attack,AttackSpeed`·`rerollCost=15`·`crumb 14→4`, game_22) / 23 합격(`Select0`×3 → 1회 success·2회 도구 가드 거부, `ability_MultiHit=1`·RoomSelect 열림, game_23) / 24 합격(RoomSelect 열림 중 `escape` → Pause 미열림·`ts=1.00`)
- D 해금·이력·스폰·보스: 25 합격(방 5 도달 → "Cream Gun unlocked!"·`gunUnlocked=true bestRoom=5`·레지스트리 저장, game_25) / 26 합격(방 2~11 스폰 프레임 전건 ±10.00/±11.00, 웨이브 방 6·7 3/6마리·방 8·9 3/7) / 27 합격(스폰 0.5s 뒤 `col=−2.435 fly=None`, 잉크 바닥 = 피벗) / 28 합격(방 9 `history` 8·첫 Battle 탈락·`Item0~7` 활성, game_28) / 29 합격(`Telegraph(Clone)` 2.00×0.70·y −2.39 = 보스 발 y; 1개가 x −14.70 벽 밖 — 참고) / 30 합격(`Win roomIndex=11 crumbTotal=30`·"Order Complete", `gunNewlyUnlocked=false`는 이전 런 해금이라 정상·`UnlockLabel` 비활성, game_30) / 31 합격(`best=11 Lock=False`, 로비 BGM `pitch=1.00`, lobby_31) / 32 합격(`sel=Gun ChefGun=True`, lobby_32) / 33 합격(종료 후 레지스트리 `BestRoom=11 GunUnlocked=True SelectedCharacter=Gun BGMVolume=0.4`, 재진입 `selected=Gun bestRoom=11 bgmVolume=0.4`·AudioSource 0.40, lobby_33)
- E Gun·경계·화면·텍스트: 34 합격(`Idle_Gun_*`·`Attack_Gun_*` 50건, Knife 시트 0건) / 35 합격(Gun 무입력 7.71s·첫 피격 2.86s, 목표 6~12s) / 36 미실측(`simulate_key j` press ×3 `Attack` 0건 — 도구 한계, 리플렉션 대체 48) / 37 합격(x=11.0 배치 5s: 11.067~11.733 ≤ 12.0, 적 |x| ≤ 11.39) / 38 합격(로비·게임 `ortho=4`, x 11.0에서 `camx=4.889`) / 39 합격(잉크×135/128 환산: 플레이어 127~130 → 134~137px 12.5%, Apple 113 → 119px, Watermelon 138 → 146px, Banana 109 → 115px; 보스 캡처 픽셀 231px 21.4%·플레이어 140px) / 40 합격(9팝업 `qa_ui` 전수 — Lobby 7·Notify 3·Setting 6·Quit 4·Result 5·Ability 9·RoomSelect 8·HUD 4·Pause 4, `issue` 0·`Text_` 0·한 줄 라벨 전건 1줄·설명 2~3줄 `renderH ≤ rectH`, fontSize 역할 상이라 기준 없음) / 41 합격(`m_SfxAttack`=`SFX_Casual_Battle_Attack`·`m_SfxHit`=Hit·`m_SfxDie`=Die·`m_Bgm`=`BGM_Casual_Battle` guid 대조) / 42 합격(`KillPlayer` → `playerDead=true result=Lose`, game_42) / 43 합격(`escape` → `Popup_Pause paused=true ts=0.00` hp·time 2s 고정, game_43; `GiveUp` → `Scene_Lobby`·`ts=1.00`·로비 BGM)
- F Job_004 수정분: 44 합격(우벽 12s 피격 12·첫 5.87s, 슬롯 2 `Attack`·대기 1 x=8.78, game_44) / 45 합격(좌벽 13·첫 5.86s, game_45) / 46 합격(방 3 15s 발사 6회 1.69·4.27·6.95·9.53·12.20·14.94s, Banana `vx=0 st=Attack`, game_46) / 47 합격(방 11 Pineapple·방 6 Pumpkin `[BattleManager] pitch=1.10`·`bgmPitch=1.10`, 방 9 `pitch=1.00`, 로비 복귀 2회 1.00) / 48 합격(`ts=0`으로 타격음 정지 후 호출: Knife `StartStep(1)`·Gun `Fire()` 같은 프레임 `playing False→True`, +0.5s False) / 49 합격(정렬 Player 2·Apple 1, 슬로모 캡처 전신 노출, game_49) / 50 합격(escape 1회 Notify만·Setting 유지·Quit 미열림, 2회 Setting 닫힘·Quit 미열림, 3회 Quit 열림, lobby_50_1~3) / 51 합격(Pineapple `Idle_01` 236.3·`Idle_03` 234.1·`Idle_04` 228.9px, `Idle_02` 263.7 허용; Pumpkin Idle 235.2·237.3·237.3·224.6) / 52 **불일치**(Banana Move 109 < Apple 113; 보스 Move Pumpkin 150~236·Pineapple 145~249 vs Idle 225/224) / 53 **불일치**(두 씬 `[Global]/[DelegateManager]` Missing Prefab 컴포넌트 1)
- 비정상 입력: 취소 — 로비 3단(50)·팝업 없음 → Quit·게임 → Pause·RoomSelect 열림 중 무시 합격. 연타 — `Select0`×3 1스택, `Start`×3 1회 success·2회 "대상을 찾을 수 없음: Popup_Lobby", 씬 1개·`LocalRoomManager` 1개·`roomIndex=1` 1회 합격. 중도 이탈 — Pause `GiveUp` 로비 복귀 합격
- 회피 3종: 공간 끝 대기 — 합격(경계: 44·45 피격 12/13, 평지: 13 접촉 729프레임 피격 49; 정지 거리 Apple 0.7~0.8u·대기 개체 간격 2.9u) / 사거리 밖 유지 — 합격(평지 시작 → 벽 도달까지 발사 6회, Banana 정지 거리 4.94→1.34u·발사 지속) / 무입력 방치 — 합격(Knife 11.37s·Gun 7.71s, 산식 8.19·7.07)
- 반복 누수: 자연 클리어 2s 뒤 `units 0/26 proj 0/24`, `ClearRoom` 10방 이상·보스 2회 뒤 로비 복귀 정상, `KillEnemies`→다음 웨이브 스폰은 `## 비고` 풀 고갈 결함
- 컨셉아트 유사성: 로비(lobby_01 vs `Concept_Scene_Lobby/Overview/art/1.png`) 8/8, 게임(game_46·game_51 vs `Concept_Scene_Game/Overview/art/1.png`) 8/8. 팝업 9종 캡처 화풍 3특징(둥근 벡터 외곽선·픽셀 격자 없음·플랫+광택) 전건 일치
- 콘솔 에러: 세션 A `total 1` = `InvalidOperationException: Wave 테이블에 방 순번 100 구간이 없다`(드라이버가 방 100까지 밀어 유발, 자연 도달 불가) / 세션 A2 `total 2` = 풀 고갈 예외(`## 비고` ⑧) / 계획·B·연타 세션 0. `qa_play`·`qa_cheat` 응답 예외 문자열 0(도구 가드 메시지 2건은 정상 거부)

## 비고
**안정화 상태 판정**
- 출시 가능 여부: **출시 보류 권고** — Job_003 결함 7건 전건 해소·전 경로(첫 실행→해금→승리→저장 유지→재시작→Gun 런·자연 클리어·포기 복귀) 완주했으나 신규 진행 차단 후보 1건(유닛 풀 고갈 예외)이 자연 플레이 조건 안에 있어 수정 후 출시를 권고. 체감 1건(넉백 일방 표류)은 출시 전 수정 권고, 미관 4건은 출시 후 보정 가능
- 진행 차단(후보 1건): ⑧ 유닛 풀 고갈 예외
- 체감(신규 1건): ⑨ 무입력 넉백 일방 누적 표류
- 미관(신규 4건): ⑩ Banana 크기 위계 역전 ⑪ 보스 모션 간 높이 편차 ⑫ `[Global]/[DelegateManager]` Missing Prefab ⑬ 로비 제목 "Game"
- 대상 외(유지 1건): 에디터 한정 디버그 콘솔 배지

**Job_003 Work_7 판정 항목 대조 (체감 4·미관 3)**
| 분류 | Job_003 항목 | 이번 판정 | 실측 근거 |
|---|---|---|---|
| 체감 | ① 벽 밀착 근접 교착 | 해소 | 우벽 12s 피격 12·첫 5.87s, 좌벽 13·5.86s(≤ 6.97s), 슬롯 2 `Attack`·대기 1 x=±8.8 |
| 체감 | ② 로비 취소 입력 `Popup_Quit` 토글 | 해소 | Notify+Setting 열림 `escape` 1회 → Notify만 닫힘·Quit `opened=false`, 3회째 Quit 열림 |
| 체감 | ③ 접촉 시 플레이어 가려짐 | 해소 | `sortingOrder` Player 2·Apple 1, game_49 전신 노출 |
| 체감 | ④ 원거리 Banana 미발사 | 해소 | 방 3 15s 발사 6회·첫 1.69s, `vx=0`에서 `Attack` |
| 미관 | ⑤ 보스방 BGM 속도 1.0 | 해소 | 방 6·11 `pitch=1.10`, 방 9 1.00, 로비 1.00 |
| 미관 | ⑥ 공격 시작 SFX 미배선 | 해소 | `m_SfxAttack=SFX_Casual_Battle_Attack`, Knife·Gun 호출 같은 프레임 `playing False→True` |
| 미관 | ⑦ 보스 화면 높이 219px | 해소 | Pineapple `Idle_01/03/04` 236.3/234.1/228.9px(222~252), `Idle_02` 263.7 허용 |

**결함 목록 (신규, 수정하지 않음 — 재현 절차·실측값·담당 영역)**
- ⑧ [`모듈` Battle] 유닛 풀 고갈 예외 — 원문 `InvalidOperationException: Apple 풀이 비었다 (크기 8)` at `LocalBattleManager.SpawnUnit`(230행) ← `SpawnWave`(266행) ← `LocalRoomManager.NextWave`(196행) ← `EnterRoom`/웨이브 전환. 재현: `KillEnemies`로 웨이브를 연속 전멸(방 1 2웨이브 직후 방 2 선택, 방 6 Apple 9마리 3웨이브) — 사망 개체는 `FSMState_UnitDie`가 Die 애니메이션 길이(기본 0.5s)만큼 기다린 뒤 `Despawn`하므로 그 창 안에 다음 스폰이 오면 `m_EnemyPoolSize=8`(`[LocalBattleManager].prefab`) 초과. 자연 플레이 조건: 후반 방(웨이브 5~8마리) 다단 히트 전멸 직후 웨이브 전환·즉시 방 선택. 영향: 예외로 스폰 중단(방 6 웨이브 3이 `alive=2`로 부분 스폰), 잠재 진행 정지. 담당 `모듈` Battle(사망 즉시 풀 반납 또는 스폰 대기·풀 크기 = 최대 동시 개체 + 사망 연출 개체)
- ⑨ [`모듈` Battle] 무입력 넉백 일방 누적 표류 — 실측 방 1 Knife 27s: 넉백 49회 전부 `dir=+1`, x −0.067→11.695(우벽, 물리 밀림 0), 12 런 x 0→6.00/11.4s, Gun 0→1.60/7.7s, 방 3 15s 0→−9.40. Job_003 동일 조건 x −0.47~0.33 왕복. 원인 추정: 적↔적 통행 허용·거리순 슬롯으로 적 3마리가 한쪽에 몰려 넉백 방향 고정. 정본에 넉백 누적 제한 없음(`게임컨셉` "피격 넉백으로만 위치 변화"라 규격 위반은 아님) — 체감 판정. 담당 `모듈` Battle(양측 슬롯 유지 또는 넉백 방향 규칙), 컨셉 정본 보강 병행 권고
- ⑩ [`리소스 제작`] Banana 크기 위계 역전 — 시트 `Enemy_Banana_Move` 잉크 109px < `Enemy_Apple_Move` 113px(`리소스컨셉` 52행 Apple 113 < Banana 123 < Watermelon 138), 화면 환산 115px는 123±12 안. Banana `Attack_03` 66px
- ⑪ [`리소스 제작`] 보스 모션 간 높이 편차 — Pumpkin Idle 225·Move 150~236·Attack1 146~258, Pineapple Idle 217~250·Move 145~249·Attack1 184~272(`리소스컨셉` 189행 "프레임 공통 기준 높이 유지" 대조). Work_3 "동작 신축 허용" 판정과의 정합은 컨셉 판단
- ⑫ [`씬 구성`] `[Global]/[DelegateManager] (Missing Prefab with guid: 834215310b3c54f4184233ac78424713)` — 두 씬 `[Global].prefab` 참조, 컴포넌트 `Transform`만, 코드·에셋 0건, 동작 영향 없음(콘솔 에러 0)
- ⑬ [`데이터`] 로비 제목 라벨 "Game" — `TableText.json` `Text_Core_GameTitle` NameEng "Game"(Kor "게임"), 게임 제목 "Kitchen Riot"·컨셉아트에 제목 없음

**참고(결함 아님)**
- Banana 발사 간격 실측 2.58~2.74s(테이블 `AttackInterval=2` + 발사 후 후퇴 재판정) — 15s 6회(산식 7회)
- 보스 Rain 전조 1개가 x −14.70(벽 −12 밖)에 생성 — 화면 밖
- `Popup_Result` `gunNewlyUnlocked`는 런 단위(이전 런 해금 시 false), 보스방 HUD `WaveText` 빈 문자열
- `KillEnemies`는 `Ended`에서도 적용, `HealPlayer`는 사망 후 무효(Job_003 동일)

**미실측·도구 한계**
- 36 Knife 콤보 키: `simulate_key j` press ×3 `Attack` 0건(Job_001~003 동일) — 리플렉션 `StartStep(1)` 대체(48). 이동 키 `--action=down/up`은 반영(14)
- 캡처: 씬 전환 직후 캡처(game_39_gun_spawn)는 페이드 백색 프레임, 보스 넉백 중 캡처(game_51_boss_idle)는 카메라 밖 — 재촬영본 game_51_boss_enter(Pumpkin)·game_37_boundary로 대체. Pineapple 캡처는 수치(`WorldToScreenPoint`)로 완결
- 드라이버 결함으로 무효 처리한 구간: 세션 A 방 4 정지 조건 미성립 → 방 99까지 진행(`Wave 테이블 방 순번 100` 예외 유발, 런 폐기·`DeleteAll` 후 재시작), 세션 B 방 1 표식 잔존 → 셸 `ClearRoom` 1회 대체. 게임 코드·데이터 무변경
- SFX 클립명은 `PlayOneShot`이라 소스에 남지 않아 `isPlaying` 전이로만 판정, HitStop 배율·`SFX_Casual_Progress_Unlock` 재생 순간 미계측

**기타**
- 저장 데이터: 계획 전 `True/11/Gun/0.4` → `DeleteAll` 3회(계획 전·실행 전·세션 A 폐기 후) → 최종 `GunUnlocked=True BestRoom=11 SelectedCharacter=Gun BGMVolume=0.4`(되돌릴 수단 없음)
- 세션 5회 전부 `editor_stop` → `stopped`, `list_open_scenes` `Scene_Lobby isDirty:false`. 코루틴·훅은 플레이 종료로 소멸
- 하네스 병렬 지시에 따라 독립 도구 호출은 묶어 요청함(프로젝트 규칙 "병렬을 요청하면 그 지시를 우선")
