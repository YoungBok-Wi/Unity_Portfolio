# [오케스트레이터_워커_실행] "Job_002 Work_7 플레이테스트 계획 갱신·전체 재평가" 업무 레포트

## 요약
- Work 판정: 합격(절차 완수) — 시나리오 39단계 중 37 실행(합격 27·불합격 6·부분 4), 미실측 2(Knife 콤보 키 입력·`ClearRoom` Ended 가드 재실측), 비정상 입력 3종 판정, 콘솔 에러 전 구간 0(`get_console_logs --severity=error` `total: 0` 플레이 종료 3회 전부), 종료 `editor_status playMode: stopped`·`list_open_scenes` `Scene_Lobby isDirty:false`
- 게임 루프 완주 실측(첫 실행 → 승리 → 저장 유지): PlayerPrefs 초기화 → 로비 `gunUnlocked=false bestRoom=0`·잠금 카드 회색·별 배지 0 → 잠금 클릭 알림 → 설정 0.4 적용 알림 → Start → Battle(무입력 사망 Knife 8.1~9.1s) → 방 선택 → 방 5 클리어 해금(`gun=True`) → 이력 8슬롯 탈락 → Ability 리롤 성공·실패·3연타 1스택 → 보스 Pineapple(hp 1125=450×2.5) 전조 렌더 → `KillBoss` → `Popup_Result Win crumbTotal=44` → 로비 해금·Gun 선택 → 재시작 후 `selected=Gun gunUnlocked=true bestRoom=11 bgmVolume=0.4` 유지 → Gun 정지 연사 → 일시정지 → 포기 → 로비
- 신규 결함(선행 Work 판정 밖, `## 비고` 결함 목록): 플레이어 접지 판정 실패로 상시 `Jump` 애니메이션(Idle·Move·Idle_Gun·Move_Gun 미표시), 적 접촉 밀림 0.47~0.70u/s 표류, 웨이브 등장 위치 x ±12~±13(벽 밖), 적 스프라이트 바닥 위 0.56u 부유, 해금 알림·결과 라벨이 해금 조건 문구, 로비 BGM 무음(`AudioSource` 0개), `Popup_Quit` ID 원문 2건
- 선행 레포트 판정: Work_4 7건 중 표류(부분 — 넉백 후 정지는 성립, 접촉 밀림 잔존)·벽(합격)·근접 슬롯(합격 2/1·2/2)·HitStop(미재측, 콘솔 에러 0)·명중 통지(합격 — `HitApplied` 프로브 수신)·치트 가드(Work_4 실측 유지, 재실측 NRE)·이력 8(합격) / Work_5 데미지 팝(합격 `dmgPop=2`·캡처 "8")·하트(합격)·점선(합격)·별 배지(합격)·요리사(합격)·Gun 시트(불합격 — `Attack_Gun`만 표시, `Idle_Gun`·`Move_Gun`은 상시 Jump로 미표시)·SFX 배선(합격)·BGM(전투 합격·로비 불합격)·Gun 무입력 사망 6.7s(유지) / Work_6 카메라 4.0(합격 `ortho=4 cam=±4.89`)·화면 높이 12.5%(합격 — Gun 사격 프레임 127px/1080)·디버그 콘솔(에디터 표시 유지)
- 산출물: `_Temp/QA/시나리오_Scene_Lobby.md`·`치트_Scene_Game.md`·`치트_Scene_Lobby.md`, 캡처 21장 `_Temp/Work_7/cap/`, 스크립트 `_Temp/Work_7/qa.sh`·`hook.cs`·`hook2.cs`·`hook3.cs`·`gv.cs`, 계층 `hier_lobby.json`·`hier_game.json`
- 저장 데이터 최종: `GunUnlocked=True`·`BestRoom=11`·`SelectedCharacter=Gun`·`BGMVolume=0.4`(레지스트리 실측). 코드·씬·프리팹·테이블 무변경, `confirmed`·`reuse` 무변경, DataMCP `Fallback` 미사용
- 다음 행동: 안정화 판정은 "루프 완주 가능·출시 불가"(`## 비고` 첫 항목) — 체감 결함 4건(접지·접촉 밀림·적 부유·로비 BGM) 수정 후 재QA가 출시 전제다

## 완료업무

### 플레이테스트 대상 조회·계획 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Game.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\hier_lobby.json`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\hier_game.json`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_질문`(선행) → `QA_유니티게임개발_플레이테스트_계획`(child `{}`) → `QA_유니티게임개발_플레이테스트_치트_작성` 위임. 기대 동작 정본은 `_Data/Concept/{Balance,Game,Resource,Scene_Lobby,Scene_Game}/concept.md`(`concept_manage get` 5건 응답 `reuse:"add"`), 문서 불일치 없음
- 조작 항목 변화 조회(로비 플레이 `qa_play get`·`qa_cheat get`): `Popup_Notify` 실재·interaction `Close`·`Confirm`, `[LocalCharacterManager]` `Select_Gun`(해금 시)·`UnlockGun`(미해금 시만), `[LocalRoomManager]` `ReturnLobby`·`historyCount`·`roomHalfWidth`·`cameraClampX`, `[LocalBattleManager]` `paused`·`timeScale`·`meleeSlotLeft/Right`, `Popup_HUD` `historySlots`·`damagePopActive` 신규 노출. 계층: 로비 `[Popup]` 4종(Notify 신규)·`ChefKnife`·`ChefGun`·`BestRoom/Icon`, 게임 HUD `HpBar/Heart`·`DamagePopRoot/DamagePop`·`HistoryRoot/Item0~7`(`get_scene_hierarchy` 2씬)
- 시나리오 39단계(추가 13: 첫 실행·무입력 정지·벽·근접 슬롯·데미지 팝·이력 9방·전조·ID 노출·사운드·화면 비율·리롤 성공·Knife 콤보·경계 회피)·비정상 입력 3종·저장 유지 4건·컨셉아트 유사성(정본 `_Data/Resource/File/Concept_Scene_{Lobby,Game}/Overview/art/1.png`) 작성, 기존 파일 전건 갱신·실물 대조 절 기록
- 치트 명세: 등록 13종(실행 확인 6종 일치), 미등록 5종 위임(`JumpRoom`·`EnterBoss`·`SetPlayerHp`·`GiveCrumb`·`ResetSave`). 저장 초기화는 편집 모드 `eval PlayerPrefs.DeleteAll()`로 대체 — 레지스트리 4키 0건 → 재진입 `gunUnlocked=false bestRoom=0 selected=Knife bgmVolume=1` 일치
- 정의 결손: `TableText.json` 174행에 `Text_Quit_Title`·`Text_Quit_Text` 부재(Work_2 비고와 동일), 나머지 9건 실재

### 전체 재평가 실행
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\cap`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\qa.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\hook.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\hook2.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\hook3.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_7\gv.cs`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_테스트`(child `{}`), 플레이 진입·종료는 `유니티엔진_게임_실행` 절차(`clear_console`→`editor_play`→`playing` 폴링 / `get_console_logs --severity=error`→`editor_stop`→`stopped` 폴링, 3회 전부 `total:0`·`stopped`·`isDirty:false`). Game 뷰는 `gv.cs` eval로 `GameViewSizes` 고정 1920x1080 선택(`Screen 1920x1080` 실측)
- 조작 통로: MCP 호출 간 4~7초·`eval` 왕복 3~5초가 무입력 생존(Knife 8~9초·Gun 6.7초)을 넘어 Battle 방은 `eval` 직접 호출(`MCPInteract`·`MCPCheatApply`·`SelectRoom`)과 `LocalBattleManager.StartCoroutine` 프레임 로거(`hook*.cs` — `HitApplied` 구독·프레임별 위치/속도/스턴·keep-alive `HealPlayer`·적 벽 고정)로 수행. Knife 콤보는 `simulate_key j`(press·down/up)가 `Keyboard.current.jKey.wasPressedThisFrame`에 반영되지 않아(`Object_PlayerBase.ReadInput`) 리플렉션 `Object_Player_Knife.StartStep(1~3)` 직접 호출로 대체
- A 로비(첫 실행): 1 합격(`gunUnlocked=false bestRoom=0`, `Lock=True`, `GunCard` 색 (0.55,0.55,0.55), `ChefKnife` 활성, `BestRoom/Icon=Icon_Casual_Room_Best`·"0", `qa_cheat get` `UnlockGun` 노출, 캡처 lobby_01) / 2 합격(`Popup_Notify` "Clear room 5 to unlock the Cream Gun", `selected=Knife` 유지, lobby_02) / 3 합격 / 4 합격(`qa_ui` 라벨 BGM·SFX·Fullscreen·Apply·Defaults 1줄·ID 미노출, lobby_03) / 5 합격(`bgmVolume=0.4`) / 6 합격("Settings applied", lobby_04) / 7 합격 / 8 불합격 — 로비 `AudioSource` 0개(`FindObjectsByType<AudioSource>` 결과 없음, `씬컨셉 Scene_Lobby` "로비 BGM" 불일치) / 9 합격(`room=1 Battle Playing wave=1/2 alive=3 hp=100/100`, `[BattleManager]` `BGM_Casual_Battle playing=True vol=0.40`)
- B 전투방 1: 10 부분 — 넉백은 규격 일치(`kb0.5/0.15`, 스턴 0.15s 후 `vx=0.00` 1프레임)이나 직후 적 접촉으로 `vx=+0.47`(반대편 `-0.70`) 등속 밀림이 다음 피격까지 지속(프레임 로그, 최근접 적 거리 0.70 고정) → 27초에 x 1.2→9.06 표류. 무입력 생존 Knife: 첫 피격 2.0s·사망 8.1s(훅 기준, 스폰은 ≤1s 이전 → 8.1~9.1s) 목표 8~15s 하한 근접 합격 / 11 합격 — `WallLeft` x −12.5 `BoxCollider2D (1,40)`, 좌벽에서 `Physics.Move(1)` 3s 동안 `colMin=-12.00` 유지 / 12 합격(`slots=2/1`, 웨이브 2 `2/2`, 대기 정지 없음) / 13 합격(`dmgPop=2`, 캡처 game_03·game_15 빨간 "8") / 14 합격(`Heart` active `Icon_Casual_Stat_HP`) / 15·16 합격(`wave=2/2 alive=4` → `Choosing`, `choice0/1`이 세트 4종 안, `history=Battle`, Crumb 0→14)
- C 방 선택·능력: 17 부분 — Heal 방 2회 통과(hist `Battle,Battle,Battle,Heal,Heal`) 회복량 미실측(`ClearRoom` 루프 중 hp 조회 누락), 이력 점선 `Item1~7 link=True`·`Item0 link=False` 합격 / 18 합격(`MoveSpeed,HealMacaron,Attack` `rerollCost=10 crumb=14`, game_09) / 19 합격(리롤 실패 "Not enough Crumbs", game_10) / 20 합격(리롤 성공 → `MultiHit,Attack,MoveSpeed` `rerollCost=15 crumb=4`) / 21 합격(`Select0` 3연타 응답 3건 `success` → `ability_MultiHit=1`) / 22 합격(RoomSelect·Ability 열림 중 escape → 팝업 유지 `ts=1`)
- D 해금·이력·보스: 23 합격(방 5 클리어 → `gun=True best=6`, `Popup_Notify` 열림 — 단 문구가 해금 조건 "Clear room 5 to unlock the Cream Gun", `SFX_Casual_Progress_Unlock` 재생 미확인) / 24 합격(방 9 `history` 8개·`historyCount=9`·`Item0~7` 전부 active, 첫 Battle 탈락, game_08) / 25 합격(방 11 Pineapple `Idle→Attack1(Spike)→Idle→Telegraph 1.0s→Attack2(Rain)` 반복, `Telegraph(Clone)` (-5.7,-1.5) scale 2 → 캡처 game_11_boss_tel1~4 붉은 타원 2곳) / 26 합격(`bossDead=True result=Win`, `Popup_Result` `Win roomIndex=11 crumbTotal=44 gunNewlyUnlocked=true`, `UnlockLabel` "{0}" 미노출 — 단 문구가 해금 조건, game_13) / 27 합격(로비 `Lock=False` 색 (1,1,1) `best=11`) / 28 합격(`sel=Gun ChefGun=True GunCard/Select=True`, lobby_06) / 29 합격(종료→재진입 `selected=Gun gunUnlocked=true bestRoom=11 bgmVolume=0.4`, 레지스트리 4키 일치)
- E Gun·콤보·경계: 30 부분 — `hp=80/80`, `j down` 중 `Attack_Gun_01~04` 반복·`proj=2/24`·`vx=0.00 px=0.00`(정지 연사 합격, game_14)이나 대기·이동 시 `Idle_Gun`·`Move_Gun` 미표시(상시 `Jump_01~06`) / 31 참고 — Gun 무입력 첫 피격 2.3s·사망 6.7s(`Gun:8` 10회) / 32 부분 — 리플렉션 `StartStep` 1→2→3 0.45s 간격 → `Attack_Knife_02~05`·`Attack2_01~05`·`Attack3_01~05` 순 재생·`IsAttacking=True`, 키 입력 경로는 판정 불가(도구 한계) / 33 부분 — 적은 `|x| ≤ 11.59` 유지·슬롯 진입, 플레이어는 접촉 밀림으로 좌벽 x −11.70까지 밀림 / 34 합격(`cam=±4.89`, `ortho=4`) / 35 부분 — 플레이어 Gun 사격 프레임 127px(11.8%)·Pineapple ≈215px(19.9%) 규격 근사, Apple 잉크 바닥이 플레이어 발보다 ≈76px 위(스프라이트 pivot (128,0)=하단 vs `리소스컨셉` (0.5,0.28)) / 36 합격 — 팝업 전수 캡처·`qa_ui` ID 원문 0건, 예상값 `Popup_Quit` `Text_Quit_Title`·`Text_Quit_Text` 2건만 노출 / 37 부분 — `m_SfxHit=SFX_Casual_Battle_Hit m_SfxDie=SFX_Casual_Battle_Die m_Bgm=BGM_Casual_Battle` 배선·전투 BGM 재생 실측, SFX 재생 순간은 미계측 / 38 합격(무입력 사망 `playerDead=true result=Lose Popup_Result Lose roomIndex=1 crumbTotal=0`, game_02; `LoseRun` 2회) / 39 합격(escape → `Popup_Pause` `ts=0` hp 2초 고정, game_15 → `GiveUp` → `Scene_Lobby ts=1` → 재시작 `room=1`)
- 비정상 입력: 취소 — 로비 escape → `Popup_Quit`(lobby_05, `Cancel` 닫힘), Result·RoomSelect·Ability 열림 중 escape 무시, 전투 escape → Pause 합격. 연타 — RoomSelect `Select0` 3회 → `room=2` 1회 진행·팝업 중복 없음(응답 문자열은 출력 파싱 손실로 미기록), Ability 3회 → 1스택, `Start` 3연타 미실측. 중도 이탈 — Pause `GiveUp` 로비 복귀 합격
- 컨셉아트 유사성: 로비(lobby_01·lobby_06 vs `Concept_Scene_Lobby`) 구도○·배치○(요리사 540px)·크기 비율○·색○(선택 파랑·잠금 회색·START 초록, 선택 테두리 노랑은 컨셉 반짝임 대응)·스타일○·요소 존재○(잠금·톱니·별·요리사)·텍스처 대상 없음·라벨 여백○ = 8/8 합격. 게임(game_04·game_12 vs `Concept_Scene_Game`·`리소스컨셉` 화면 비율) 구도○·배치✕(적 부유·플레이어 상시 점프 포즈)·크기 비율○(규격값 기준)·색○·스타일○·요소 존재○(하트·점선·데미지 팝)·텍스처○(타일 이음새 없음, 클램프 끝 game_05 배경 이음선 미노출)·라벨 여백○ = 7/8 87.5% 합격. 팝업 8종 화풍 3특징 전건 일치
- 개체 수치: 방 2 Apple 34, 방 7 Apple 57·Banana 48·Watermelon 171, 방 9 Apple 66·Watermelon 198·Banana 55, 방 11 Pineapple 1125 — 성장식 전건 일치. 웨이브 구성 방 7 `3+floor(7/2)=6`·방 8·9 7마리 일치
- 반복 누수: 투사체 풀 `proj 2/24`(사격 중), 유닛 풀 재사용(방 11까지 `alive` 정상 감소), 콘솔 에러 0

## 비고
**안정화 상태 판정**
- 출시 가능 여부: 출시 불가 — 진행 차단 결함 0건(첫 실행→승리→저장 유지→재시작 전 경로 완주 실측)이나 체감 결함 4건이 첫 방부터 상시 노출된다. 체감 4건 수정 후 재QA 1회가 출시 전제다
- 진행 차단(0건): 없음 — 단 "웨이브 등장 위치 벽 밖"은 적이 벽에 끼어 방 클리어가 막힐 가능성이 있어 수정 전 자연 플레이 실측이 필요하다(이번은 `ClearRoom`으로 통과해 미확인)
- 체감(6건): 플레이어 접지 판정 실패(상시 점프 포즈·Gun 전용 대기/이동 시트 미표시), 적 접촉 밀림 표류, 적 스프라이트 부유, 로비 BGM 무음, 웨이브 등장 위치 벽 밖, Gun 무입력 생존 6.7s(목표 하한 8s 미만 — 컨셉 목표가 Knife 기준이라 정의 확인 필요)
- 미관(4건): 해금 알림·결과 라벨의 조건 문구, `Popup_Quit` ID 원문 2건, 보스 전조 타원이 바닥이 아닌 세로 타원(y −1.5 중심), 에디터 한정 디버그 콘솔 배지

**결함 목록 (수정 대상 — 재현 절차·실측값·담당 영역)**
- [모듈] 플레이어 접지 판정 실패 — 재현: `Scene_Lobby` Start → 스폰 직후 `eval` `Player.Physics.FlyState`. 실측: 스폰 t=955.40부터 8초간 적 없이 `FlyState=Float`·`Anim=Jump`·y −2.40, `Rigidbody2D.IsTouching(floor)=True`인데 `m_GroundCol=0`; 리플렉션 `AddGroundCol(floor)` 직후 `None`이나 다음 `OnCollisionStay2D`에서 `Float` 복귀. 원인: `Assets/_Library/CharacterPhysics/Script/CharacterPhysics2DSide.cs:104~136` 접지 조건 `avgPos.y < transform.position.y`가 하단 피벗(콜라이더 min −2.395 = transform y)에서 성립하지 않음. 영향: `Object_PlayerBase.UpdateMotion`이 `Idle`·`Move`(Gun `Idle_Gun`·`Move_Gun`) 대신 `Jump`만 재생, 캡처 전건 웅크린 포즈. 담당: `모듈`(라이브러리 `CharacterPhysics` 접지 규칙 또는 `Object_Player_*` 프리팹 피벗·콜라이더 오프셋 — 정본 결정 필요)
- [모듈] 적 접촉 밀림 표류 — 재현: Start 후 무입력, 프레임 로그(`hook.cs`). 실측: 넉백 종료 프레임 `vx=0.00` 뒤 `vx=+0.14→0.70→0.47` 등속(반대편 −0.70), 최근접 Apple 거리 0.70 고정(정지 거리 0.8 안), 적 제거 시 즉시 `vx=0`. 27초 x 1.2→9.06, 벽까지 밀림 −11.70. 담당: `모듈` Battle(`Object_UnitBase` 적↔플레이어 콜라이더 물리 접촉 — 레이어 충돌 매트릭스·질량·`FixedUpdate` `StopHorizontal` 뒤 물리 해소 순서)
- [모듈] 웨이브 등장 위치 벽 밖 — 재현: 방 7·8·9 입장 직후 `eval` 적 x. 실측: `Banana@12.00`·`Banana@-12.00`·`Banana@-13.00`·`Watermelon@-12.00`·`Watermelon@11.00`(등장 기준 ±10 + 마리당 1u 오프셋). `밸런스컨셉` "방 구조" 등장 ±10·벽 ±12 위반. 담당: `모듈` Room(`LocalRoomManager` 스폰 오프셋을 벽 안쪽으로 클램프)
- [리소스 제작] 적 스프라이트 부유 — 실측: Apple `sprite.pivot=(128,0)`(하단 중앙)·PPU 128·`sprMin=-2.39`(바닥)이나 잉크 접지선이 캔버스 184px 행(`리소스컨셉` `AnimationSheet_Casual_Enemy` 피벗 (0.5,0.28))이라 화면에서 발 위치가 플레이어보다 ≈76px 위(game_04·game_14). 담당: `리소스 제작`(`SpriteAnim` 임포트 피벗 — Work_3_2·Work_3 일괄 보정이 전 시트를 BottomCenter로 통일, `AutoTextureSettingOnImport` 규칙 부재 Work_4 비고)
- [씬 구성] 로비 BGM 무음 — 실측: `Scene_Lobby` 플레이 중 `AudioSource` 0개, `[BattleManager]`는 게임 씬에서만 생성. `씬컨셉 Scene_Lobby` "로비 BGM `BGM_Casual/Lobby`" 위반(Work_5 비고 유지). 담당: `씬 구성`/`모듈` Character(`Scene_Lobby` 로컬 매니저에서 `BattleManager.PlayBGM(BGM_Casual_Lobby)` 배선)
- [데이터] 해금 알림·결과 라벨 문구 — 실측: 방 5 클리어 `Popup_Notify` text·`Popup_Result` `UnlockLabel` 모두 "Clear room 5 to unlock the Cream Gun"(`Text_Core_GunUnlock` 한 ID를 조건·알림·결과에 공유, Work_2 비고). 담당: `데이터`(해금 완료용 ID 신설) + `모듈` Room·`프리셋` Result의 ID 교체
- [데이터] `Popup_Quit` ID 원문 — 실측 `qa_ui` `Text_Quit_Title`·`Text_Quit_Text`(lobby_05). `Quit` 모듈 `inAsset=false`로 `Text` 익스포트 제외(Work_2 비고). 담당: `데이터`
- [프리셋 구성] 보스 전조 형태 — 실측 `Telegraph(Clone)` 위치 y −1.5·scale 2·세로 긴 타원(game_11) — `게임컨셉` "바닥에 범위 표시"와 어긋나는 세로 타원. 담당: `프리셋 구성`(`Telegraph.prefab` 스케일 x>y·y 오프셋 바닥)
- [컨셉] Gun 무입력 생존 목표 — `밸런스컨셉` "무입력 생존 시간 목표(순번 1, Knife)"만 정의, Gun 실측 6.7s(80HP). 담당: `컨셉`(Gun 기준 정의 또는 Knife 한정 명시)

**미실측·도구 한계**
- Knife 콤보 키 입력: `simulate_key --key=j`(press·down/up 모두)가 `wasPressedThisFrame`에 반영되지 않아 입력 경로 판정 불가(Job_001과 동일). `d` 키 `isPressed`는 반영(`vx=5.00`)되나 반영 시점이 수 초 지연되는 사례 있어 이동 키 판정도 보류. 콤보 자체는 리플렉션 호출로 합격
- `ClearRoom` Ended 가드 재실측 프로브가 `Object reference not set`으로 실패(사망 직후 `Popup_RoomSelect` 조회 경합 추정) — Work_4 실측 유지
- Heal 회복량·`Start` 3연타·`SFX` 재생 순간·`HitStop` 배율은 이번 미계측
- MCP 왕복 지연: `qa_play`·`qa_cheat` 호출 간 4~7초, `eval` 3~5초 — 시간 민감 단계는 `eval`+코루틴으로 수행(지시서 허용)

**기타**
- 저장 데이터: 검증 전 `GunUnlocked=True BestRoom=10 SelectedCharacter=Knife BGMVolume=0.7` → `DeleteAll` → 검증 후 `True/11/Gun/0.4`(되돌릴 수단 없음, 첫 실행 재검증은 `DeleteAll` 재실행)
- 디버그 콘솔 배지는 캡처 전건 우측에 노출(`LogManager` 에디터 한정, Work_6 비고) — 릴리스 빌드 판정 대상 아님
- `eval` 코루틴·`Application.onBeforeRender` 구독은 플레이 종료로 소멸, 코드·씬·프리팹 무변경(`isDirty:false`)
