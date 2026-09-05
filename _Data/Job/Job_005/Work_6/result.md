# [QA_유니티게임개발_플레이테스트_테스트] "플레이테스트 계획 갱신·전체 재평가·안정화 판정" 업무 레포트

## 요약
- 시나리오 60단계(Job_004 53 유지 + 52·53 재정의 + 신규 54~60) 중 58 실행 — 합격 56·참고 2(45 좌벽 첫 피격 7.02s vs 산식 6.97s, 52 Banana 높이 109 < Apple 113은 정본 척도(장축) 밖 관찰)·미실측 2(21 Reroll 재화 부족 분기 — Crumb 14로 조건 미성립, 36 Knife 콤보 키 — `simulate_key` 미반영 도구 한계·리플렉션 `StartStep` 대체 합격). 비정상 입력 3종·회피 3종·저장 유지·반복 누수 판정 완료. 플레이 6세션(A·B·C·D·E·F) 전부 `editor_status playMode: stopped`·`Scene_Lobby isDirty:false`, 콘솔 에러 총 0(세션별 `console_err_*.json` `total 0`, 드라이버 `Exception` 훅 0)
- Job_004 결함 ⑧~⑬ 전건 해소: ⑧ 방 6 3웨이브 연속 전멸 6·6·6 스폰·사망 연출 동시 13·예외 0 ⑨ 무입력 30s dx=1.67(최대 2.00 ≤ 6.0)·상한 도달 넉백 12회 거리 0·경직 455프레임·슬롯 좌우 동시 보유 1535/1535프레임 ⑩ Banana Move 장축 123 > Apple 113(정본 척도) ⑪ Pumpkin 관찰 15프레임 잉크 높이 146~258(Idle_01 225) 전건 허용 범위 안 ⑫ 두 씬 `[Global]` 자식 16개·`[DelegateManager]` 부재 ⑬ 로비 제목 "Kitchen Riot" 482.5/900px·1줄·잘림 없음
- **안정화 판정: 출시 가능** — 진행 차단 0·체감 0·미관 0(신규 결함 없음). 대상 외 유지 1건(에디터 한정 디버그 콘솔 배지)
- 산출물: `_Temp/QA/시나리오_Scene_Lobby.md`(60단계)·`치트_Scene_Game.md` 갱신, 캡처 24장 `_Temp/Work_6_J5/cap/`, 드라이버 `_Temp/Work_6_J5/driver.cs`·`qa.sh`·`run.sh`·`run_C.sh`, 로그 `log_A/B/E/F.txt`, `qa_*.json`·`ui_*.json`·`cheat_*.json`, 집계 `analyze.py`. 코드·씬·프리팹·테이블 무변경, `confirmed`·`reuse` 무변경, `editor_util setup` 미실행, 사용자 질문 없음

## 완료업무

### 플레이테스트 계획 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Game.md`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_질문`(정본 조회 — Work_1 개정 `밸런스컨셉` 무입력 넉백 누적 상한 1.5u·검산 6.0u, `리소스컨셉` 128·134·189행 허용 범위, Work_2 `Text_Core_GameTitle`, Work_4 `BattleConst.PlayerKnockbackDriftMax`·`m_EnemyPoolSize 12`) → `QA_유니티게임개발_플레이테스트_계획`(child `{}`) → `QA_유니티게임개발_플레이테스트_치트_작성`
- 시나리오: 헤더에 Job_005 판정 대상 ⑧~⑬ 추가, 52·53을 Job_005 기준으로 재정의(구 정의 보존), 신규 54(풀 소진)·55(표류)·56(반대쪽 슬롯)·57(보스 잉크 범위)·58(로비 제목)·59(거리 0 경직)·60(Knife 자연 처치). 실물 대조: 매니저·팝업 ID(qa_play get 항목 — `[LocalBattleManager]`·`[LocalRoomManager]`·`[LocalCharacterManager]`·Popup_Lobby/Notify/Setting/Quit/HUD/Ability/RoomSelect/Result/Pause), interactionId(SelectKnife·SelectGun·Setting·SetBgm·Apply·Close·Cancel·Start·Reroll·Select0·Confirm·GiveUp — 실행 세션 응답 전건 JSON), cheatId(`cheat_game.json` `[LocalBattleManager]` KillEnemies·HealPlayer·KillPlayer·Ability_×6, 보스 시 KillBoss — Job_004와 동일), 단계 수 53 → 60, 개체 수치(방 1 Apple 3·방 4 5·방 6 웨이브 6·6·6) 일치
- 치트 명세: Job_004 명세 유지 + "Job_005 갱신" 절(KillEnemies 매 프레임 반복 적용, 저장 데이터 전이). 등록 현황 `qa_cheat get` 재대조 일치, 미등록(JumpRoom·EnterBoss·SetPlayerHp·GiveCrumb·ResetSave) 위임 유지
- 저장 데이터: 실행 전 `BestRoom=11 SelectedCharacter=Gun GunUnlocked=True BGMVolume=0.4` → `PlayerPrefs.DeleteAll`(편집 모드 `eval`, `HasKey BestRoom=false`)로 첫 실행부터 진행

### 전체 재평가·안정화 판정
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\cap`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\driver.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\run.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\log_A.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\log_B.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\log_E.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J5\log_F.txt`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_테스트`(child `{}`, `error.md` 빈 파일). 플레이 진입·종료는 `유니티엔진_게임_실행` 절차(`clear_console`→`editor_play`→`playing` 폴링 / `get_console_logs --severity=error`→`editor_stop`→`stopped` 폴링). 조작은 `qa_play` interaction 대신 `eval MCPInteract`(대체 경로 — 팝업 상태·응답 JSON 동일), 상태는 `qa_play get`(curl Fallback, MCP 스키마 미로드)·`qa_ui text`·엔진 내 코루틴 드라이버(`eval_file`, 시간 민감 구간을 같은 프레임에 처리 — 지시서 허용). 세션 A(첫 실행 로비→Knife 런→능력→해금→방 12)·B(저장 재확인→Gun 런→풀→방 9 이력→보스→승리→사망)·C(재시작 유지·KillPlayer)·D(일시정지·재개·포기)·E(무입력 생존 Knife·이동 키)·F(무입력 생존 Gun)
- A 로비 첫 실행: 1 합격(`selected=Knife gunUnlocked=false bestRoom=0 bgmVolume=1`·`Lock=True`·`BestText=0`, lobby_01) / 2 합격(Notify "Clear room 5 to unlock the Cream Gun", lobby_02) / 3 합격 / 4 합격(Setting 열림, ui 23건 issue 0, lobby_03) / 5 합격(`bgmVolume=0.4`·AudioSource `vol=0.40`, lobby_04) / 6 합격("Settings applied", lobby_05) / 7·50 합격(escape 1회 → Notify만 닫힘·Setting 유지·Quit 미열림, 2회 → Setting 닫힘, 3회 → `Popup_Quit` 열림 — 세션 A·B 2회 동일) / 8 합격(`clip=BGM_Casual_Lobby playing=True loop=True`) / 9 합격(Quit 문구 "Quit Game"·"Are you sure you want to quit the game?", `Text_` 0건, Cancel → 닫힘, lobby_06) / 58 합격(`[Popup]/Popup_Lobby/Title` text "Kitchen Riot" renderW 482.5 < rectW 900·lines 1·truncated false·overflow false, 캡처 lobby_01 중앙 상단 표시) / 10 합격(`Scene_Game` room 1 Battle Playing wave 1/2 alive 3 hp 100)
- 전투방 1 Knife(세션 A·E): 11 합격(스폰 첫 표본 `Player_Idle_01`·`fly=None`·`col=−2.448`, 적 ±10/±11) / 12 합격(세션 E 스폰→hp 0 8.67s·첫 피격 3.24s·17타, 목표 8~15s — 세션 A 7.18s는 Start 조작이 계측 시작보다 약 2~3s 앞선 편향 표본이라 폐기) / 13·55 합격(무입력 30s x 0.00→1.67·최대 |dx| 2.00 ≤ 6.0, 피격 74(우 45·좌 29), 넉백 거리 0 피격 12회·경직 프레임 455 — 59 합격, 양측 배치 프레임 913/1535 = 59% ≥ 50%, 슬롯 좌우 동시 보유 1535/1535 — 56 합격, 종료 시 슬롯 −1·+1·−1) / 14 합격(세션 E `d` 홀드 표본 `Move_Gun_02`·`vx=5.50`, 해제 후 `Idle_Gun`·`vx=0`) / 16 합격(`meleeSlotLeft=2 meleeSlotRight=1`) / 17 합격(`damagePopActive=3`, game_17_hud 하트·HP바) / 44 합격(우벽 x=11.7 12s 피격 18·첫 3.16s) / 45 참고(좌벽 12s 피격 5·첫 7.02s — 기준 "12s 안 ≥ 1" 충족, 산식 6.97s 대비 +0.05s는 대기 개체 배치 편차) / 48 합격(Knife `StartStep(1)` 같은 프레임 `[SoundManager] playing False→True`·다음 프레임 `Attack_Knife_02`) / 49 합격(정렬 `so` Player 2·Apple 1, game_49_contact_slow 캡처에서 요리사가 사과 2마리 앞에 전신 노출) / 60 합격(`StartStep` 0.5s 간격 21회·치트 0 — 웨이브 1→2→`Choosing`, Crumb 0→14, hp 10 생존)
- 방 선택·능력·해금(세션 A): 19 합격(Heal 60→100 즉시 `Choosing`) / 20 합격(`choices=MultiHit,MaxHp,Attack`·`rerollCost=10`·Crumb 14, game_20_ability 3카드·Retry 10 표시) / 21 미실측(Crumb 14 ≥ 10이라 부족 분기 미성립 — Job_004 합격 유지) / 22 합격(Reroll → `MoveSpeed,Attack,MaxHp`·`rerollCost=15`·Crumb 14→4) / 23 합격(Select0 ×3 → 1회 성공·2회 `nopopup`, RoomSelect 열림) / 24 합격(RoomSelect 열림 중 escape → `Popup_Pause` 미열림·`paused=false`) / 25 합격(방 5 클리어 → `gunUnlocked=true`·Notify "Cream Gun unlocked!"·`gunNewlyUnlocked=true`, game_25_unlock) / 26 합격(방 2~12 입장 첫 프레임 적 |x| = 10.00·11.00) / 27 합격(적 `col=−2.435`·`fly=None`, 스폰 프레임만 `Float`) / 30 합격(세션 B 보스 처치 → `result=Win`·`crumbTotal=95`·`gunNewlyUnlocked=false`(이전 런 해금), game_30_win "Order Complete 11 / 95") / 31 합격(로비 `bestRoom=12`·`Lock=False`, lobby_31) / 32 합격(SelectGun → `selected=Gun`·`ChefGun=True`) / 33 합격(세션 B·C 재시작 전 PlayerPrefs `12/Gun/True/0.4` → 재진입 로비 `sel=Gun gun=True best=12`·`vol=0.40`, lobby_33)
- Gun·경계·화면(세션 B·E·F): 34 합격(`Idle_Gun_0x`·`Move_Gun_02`·hp 80/80, game_39_gun_spawn) / 35 합격(세션 F 스폰→hp 0 7.72s·첫 피격 3.17s, 목표 6~12s) / 18 합격(Gun `Fire()` 0.25s 간격 34회·치트 0 — 웨이브 1→2→`Choosing`, Crumb 0→14, hp 56) / 36 미실측(도구 한계 — 48 리플렉션 대체) / 37 합격(x=11.0 배치 5s x 11.000~11.733 ≤ 12.0) / 38 합격(`cam=4`, x 11.0에서 `camx=4.89`) / 39 합격(시트 잉크 → 화면 ×1.0547: 플레이어 128 → 135px 12.5%, Apple 113 → 119px, Watermelon 138 → 146px, Banana 장축 123 → 130px, Pumpkin Idle_01 225 → 237px 21.9%·`boundsScreen 405`) / 40 합격(팝업 9종 `qa_ui` 전수 — Lobby 17·Notify 20·Setting 23·Quit 21·HUD 15·Ability 23·RoomSelect 22·Result 19·Pause 19건, 게임 UI issue 0·`Text_` 0·truncated/overflow/offScreen 0, 다중 줄은 설명 라벨(Desc 2~3줄 `renderH ≤ rectH`)뿐, 유일한 issue `[LogManager]/DebugConsole` 입력 필드 빈 문자열 — 에디터 한정 대상 외) / 41 합격(`sfxAttack=SFX_Casual_Battle_Attack`, 전투 BGM `BGM_Casual_Battle playing=True`, 로비 복귀 `BGM_Casual_Lobby`) / 42 합격(세션 C `KillPlayer` → `playerDead=true`·`result=Lose`·Result 열림, Confirm → 로비, game_42_lose) / 43 합격(세션 D escape → `Popup_Pause opened=true`·`paused=true`·`timeScale=0.00`·hp 80 2s 고정, escape 재입력 → Pause 닫힘·`ts=1`, GiveUp → `Scene_Lobby`·`ts=1`, game_43_pause)
- 방 6 이상·보스(세션 B): 54 합격(방 6 웨이브 6·6·6 매 프레임 `KillEnemies` → 웨이브별 신규 생존 6·6·6, `Die` 동시 최대 13, 예외 0, `state=Choosing waveIdx=3/3`, `pendingSpawn=0`) / 28 합격(방 10 `history` 8건 `Heal,Battle,Heal,Heal,Battle,Battle,Battle,Heal`·HUD 아이콘 8, game_51 캡처) / 29 합격(`Telegraph(Clone)` 3.00×1.05 가로 타원·y −2.39 = 보스 발 y) / 47 합격(방 11 Pumpkin `pitch=1.10`·`BgmPitch=1.10`, 로비 1.00) / 51·57 합격(Pumpkin 관찰 15프레임 잉크 높이 — Idle 225·225·213·223(Idle_01 225 ∈ 217~231), Move 151·236·226·222·150, Attack1 218·258·248·217·146·230, Die 224 — 전건 134~280, 화면 237px) / 46 합격(방 4 무입력 15s Banana 발사 6회·첫 2.06s·간격 2.0~2.5s) / 52 참고(Move 잉크 Apple 110×113·Banana 123×109·Watermelon 123×138 — 정본 척도 장축 113 < 123 < 138 정합, 높이는 Banana 109 < Apple 113으로 남음) / 53 합격(플레이 중 `[Global]` 자식 16개 열거 — `[DelegateManager]` 부재, 로비·게임 동일)
- 비정상 입력: 취소 입력 합격(로비 3단·RoomSelect 열림 중·전투 중 Pause·Pause 열림 중 재입력 전부 처리 주체 있음·예외 0) / 연타 합격(Select0 ×3 1회만 반영, 중복 팝업 없음) / 중도 이탈 합격(Pause → GiveUp → 로비, `timeScale=1`)
- 회피 3종: 공간 끝 대기 합격(우벽·좌벽 12s 피격 18/5, x=11.0 경계 5s 적 접근·밀림 0 — 경계 조건) / 사거리 밖 유지 합격(방 4 Banana 유지 거리에서 15s 발사 6회, Apple·Watermelon 접근·공격 상태 전이 — 평지 조건) / 무입력 방치 합격(Knife 8.67s·Gun 7.72s 산식 8.19/7.07s ±1.0s 안, 정지 거리 Apple 0.8u·개체 간격 0.5~0.7u(슬롯 2마리 x 1.12/1.13 vs 플레이어 1.67) 한 표본)
- 반복 누수: 유닛 풀(크기 12) 방 6 3웨이브 18마리 스폰·사망 → `alive=0`·`pendingSpawn=0` 복귀, 투사체 방 4 15s 발사 6회 → 활성 `proj` 0~1 유지, 세션 A→B→C 3런 반복 후 `[Global]` 자식 수 16 유지
- 화면·화풍: 캡처 24장 실독(lobby_01·game_49·game_51·game_20·game_30·game_43 판정, 그 외 상태 기록) — 월드·UI 전부 둥근 외곽선·플랫 채색·광택 하이라이트 3특징 일치, 분홍·빈 화면·잘림 0. 컨셉아트 유사성은 이번 회차 리소스 산출물 무변경(Work_3 재제작 0건)이라 Job_004 판정(합격) 유지 — 재판정 대상 아님

## 비고
**안정화 상태 판정**
- 출시 가능 여부: **출시 가능** — Job_004 결함 6건 전건 해소, 전 경로(첫 실행→설정→해금→능력→보스 승리→저장 유지→재시작→Gun 런·자연 클리어·풀 소진·일시정지·포기·사망) 완주, 콘솔 에러 0, 신규 결함 0
- 진행 차단 0 / 체감 0 / 미관 0 / 대상 외 1(에디터 한정 디버그 콘솔 배지)

**Job_004 Work_6 판정 항목 대조**
| 분류 | Job_004 항목 | 이번 판정 | 실측 근거 |
|---|---|---|---|
| 진행 차단 후보 | ⑧ 유닛 풀 고갈 예외 | 해소 | 방 6 3웨이브 연속 전멸 스폰 6·6·6, 사망 연출 동시 13, 예외 0, 풀 12 |
| 체감 | ⑨ 무입력 넉백 일방 누적 표류 | 해소 | 30s dx 1.67·최대 2.00(기준 ≤ 6.0), 거리 0 넉백 12회, 슬롯 좌우 동시 보유 100% |
| 미관 | ⑩ Banana 크기 위계 역전 | 해소(정본 척도) | Move 장축 Apple 113 < Banana 123 < Watermelon 138 |
| 미관 | ⑪ 보스 모션 간 높이 편차 | 해소(허용 범위) | Pumpkin 15프레임 146~258 ∈ 134~280, Idle_01 225 ∈ 217~231 |
| 미관 | ⑫ `[Global]/[DelegateManager]` Missing Prefab | 해소 | 두 씬 `[Global]` 자식 16개·Missing 0 |
| 미관 | ⑬ 로비 제목 "Game" | 해소 | "Kitchen Riot" 482.5/900px 1줄 |

**참고(결함 아님)**
- 좌벽 첫 피격 7.02s(산식 6.97s +0.05s) — 대기 개체 초기 배치 편차, 12s 기준 충족
- Banana 시트 잉크 높이 109 < Apple 113 — 정본 위계 척도가 장축(가로 누운 형태)이라 결함 아님. 높이까지 키우려면 `리소스컨셉` 척도 개정(정본 변경)이 선행돼야 함
- 해금 Notify가 방 5 클리어 직후 RoomSelect 위에 열리고 사용자가 닫지 않으면 다음 방(Ability) 팝업과 겹쳐 남음 — 닫기 1회로 해소되는 표시 순서, 정본에 자동 닫힘 규정 없음

**미실측·도구 한계**
- 21 Reroll 재화 부족 분기(Crumb 14 ≥ 10) — Job_004 합격 유지, 코드 무변경
- 36 Knife 콤보 키(`simulate_key j` press 미반영, Job_001~004 동일) — 리플렉션 `StartStep` 대체
- `SpawnUnit` 풀 소진 대기열 분기 — 풀 12로 현행 `TableWave`에서 소진 미발생(Work_4 비고와 동일), 코드 정적 검토만
- 세션 A의 무입력 생존(7.18s)·세션 E Gun(4.80s)은 Start 조작이 계측 시작보다 앞선 편향 표본이라 폐기하고 세션 E Knife·F Gun 표본으로 대체
- 세션 A는 드라이버가 능력 팝업 뒤 방 13까지 진행(`Advance` 정지 조건 오류)해 Knife 런 보스 관찰이 없음 — 보스·해금 이후 항목은 세션 B(Gun 런)에서 전건 실측. 드라이버 오류로 방 13 도달 시 `BestRoom=12`가 저장됨(게임 정상 동작)
- 드라이버 컴파일 실패 2회(`FindObjectsSortMode` obsolete 경고 속 `long→int` 변환 오류)·CLI `Network error` 1회로 세션 A를 3회 재시작 — 게임 코드·데이터 무관

**기타**
- 저장 데이터 최종: `BestRoom=12 SelectedCharacter=Gun GunUnlocked=True BGMVolume=0.4`(실행 전 11/Gun/True/0.4 — 되돌릴 수단 없음, Job_004와 같은 처리)
- 세션 6회 전부 `editor_stop` → `stopped`, `list_open_scenes` `Scene_Lobby isDirty:false`
- 하네스 병렬 지시에 따라 독립 도구 호출은 묶어 요청함
