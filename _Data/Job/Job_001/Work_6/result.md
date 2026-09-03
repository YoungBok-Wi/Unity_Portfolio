# [오케스트레이터_워커_실행] "Job_001 Work_6 플레이테스트 조회·계획·실행" 업무 레포트

## 요약
- Work 판정: 합격(절차 완수) — 시나리오 26단계 전부 판정 완료(합격 19·불합격 7), 비정상 입력 3종·회피 3종·저장 유지·반복 누수 판정 완료, 콘솔 에러 전 구간 0건(`get_console_logs --severity=error` `total: 0` 8회 실측), `qa_play`·`qa_cheat` 예외 문자열 0건
- 게임 루프 실측 합격: 로비 선택 → Start → Battle 웨이브(2/2·3/3) → `Popup_RoomSelect`(2택, 4세트, 적 미리보기 종류·마릿수 = `TableWave` 합계) → Heal(+50) → Ability(3택·MaxHp 125·HealMacaron 회복·리롤 잔액 부족 알림) → 5번째 방 해금(`gun=True best=5`) → 보스 2종 FSM(Pineapple Idle→Skill2(Rain, telegraph)→Skill1(Spike, 투사체)→Enrage / Pumpkin Idle→추격(9→0.99)→Skill1(Slam, telegraph)→Enrage `spd 3→4`) → `KillBoss` → `Popup_Result` Win(`crumb 30`) → 로비. 방 성장식 HP 전건 일치, Crumb 낙하 수치 일치
- 주요 불합격(상세 `## 비고`): 무입력 시 플레이어가 피격 후 x+ 방향으로 1.85u/s 표류해 카메라 클램프(8)·바닥(±30) 밖으로 이탈·낙하(모듈), 캐릭터 화면 높이 7.7%(플레이어)로 컨셉아트(≈40%) 대비 과소(씬 구성/컨셉 규격 결손), 보스 전조 `Telegraph`가 활성인데 화면에 안 그려짐(프리셋 구성), 텍스트 ID 원문 노출 11건(데이터), 로비에 `Popup_Notify` 부재로 Gun 잠금 클릭 알림 미표시(씬 구성), 컨셉아트 유사도 로비 75%·게임 75%(80% 미만)
- 산출물: `_Temp/QA/시나리오_Scene_Lobby.md`, `_Temp/QA/치트_Scene_Game.md`, `_Temp/QA/치트_Scene_Lobby.md`, 캡처 39장 `_Temp/Work_6/cap/`, 조작 스크립트 `_Temp/Work_6/qa.sh`
- 플레이 종료 실측: `editor_stop` → `editor_status playMode: stopped`, `list_open_scenes` `Scene_Lobby isDirty:false`. 코드·씬·프리팹 무변경(`AssetDatabase` 편집 0건). 저장 데이터(PlayerPrefs)만 변경: `gunUnlocked` false→true, `bestRoom` 3→10, `selectedId` Knife, BGM 0.4→0.7

## 완료업무

### 플레이테스트 대상 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\hier_Scene_Lobby.json`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\hier_Scene_Game.json`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_질문`(child `{}`). 구성 근거: `_Data/Concept/{Scene_Lobby,Scene_Game,Game,Balance,Resource}/concept.md`, `Assets/__Game/{Room,Battle,Character}/module.md`, `Assets/_Library/_Core/Resources/Table/Table{Const,Text,Wave,Room,Character,Enemy,Boss,Ability}.json`
- 계층 실측(`get_scene_hierarchy`): Lobby `[Popup]` = Popup_Lobby·Popup_Quit·Popup_Setting, `[Local]`에 `[LocalCharacterManager]`만, `[Stage]/Object_Background`; Game `[Popup]` = HUD·RoomSelect·Ability·Pause·Result·Notify·Setting, `[Local]`에 Battle·Character·Room 매니저, `[Stage]/Object_Background·Object_Floor`. HUD 이력 슬롯 `HistoryRoot/Item0~7` 8개
- MCP 노출 실측(`qa_play get`·`qa_cheat get`, 플레이 중): 팝업 interaction — Lobby `SelectKnife·SelectGun·Start·Setting`, HUD `Pause`, RoomSelect `Select0/1`, Ability `Select0~2·Reroll`, Pause `Resume·Setting·GiveUp`, Result `Confirm`, Setting `SetBgm·SetSfx·SetFullscreen·Apply·Default`, Notify `Confirm`, Quit `Confirm·Cancel`; 매니저 cheat — `[LocalBattleManager]` KillEnemies·KillBoss·HealPlayer·KillPlayer·Ability_{6종}, `[LocalRoomManager]` ClearRoom·WinRun·LoseRun, `[LocalCharacterManager]` UnlockGun
- 텍스트 결손 실측: 코드 사용 ID 중 `TableText.json`(157건) 미존재 9건 — `Text_Core_Confirm`·`Text_Core_GunUnlock`·`Text_Core_RoomSelectTitle`·`Text_Popup_Setting_{Applied,Apply,BGM,Default,Fullscreen,SE}`. 라이브러리 `Popup_Quit`의 `Text_Quit_Title`·`Text_Quit_Text`도 화면 실측 미존재(캡처 07)

### 플레이테스트 계획 작성
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Game.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Lobby.md`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_계획`(child `{}`) → `QA_유니티게임개발_플레이테스트_치트_작성` 위임. 시나리오 5절(진입 조건·조작 순서 26단계·비정상 입력 3종·저장 유지 대상 4건·컨셉아트 유사성) 작성, 미확인 항목은 `{미확인: 조회 수단}` 표기 후 실행 중 "실물 대조" 절에 갱신
- 컨셉아트 정의처: `resource_file source Concept/Concept_Scene_Game/Overview` → `_Data/Resource/File/Concept_Scene_Game/Overview/art/1.png`(1672x941), Lobby·Resource 동일 경로 패턴 (`source` 액션은 `slot` 인자 미지원 — 인자 없이 호출)
- 치트 명세: 등록 13종 중 실행 확인 8종(일치 8), 미실행 2종(KillPlayer·UnlockGun — 자연 경로로 동일 상태 도달), 미등록 5종 위임(`JumpRoom{n}`·`EnterBoss{id}`·`SetPlayerHp{n}`·`GiveCrumb{n}`·`ResetUnlock`). `ClearRoom`이 `Ended` 상태에서도 적용되는 결함 기록
- 문서 불일치: 없음(Heal 비율·세트 4종·해금 순번 컨셉=테이블 일치). 정의 결손: `리소스컨셉` 규격에 캐릭터의 "화면 높이 비율"이 없음(캔버스 점유율만 있음) — 컨셉아트 크기 비율 판정의 기준값 부재

### 플레이테스트 실행
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\cap`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\qa.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\poll.log`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_테스트`(child `{}`, `error.md` 빈 파일), 플레이 진입·종료는 `유니티엔진_게임_실행` 절차(`open_scene`·`clear_console`·`editor_play`·`editor_status playing` 폴링 / `get_console_logs --severity=error`·`editor_stop`·`stopped` 폴링). Game 뷰를 eval로 `GameViewSizes`에 1920x1080 고정 항목 선택 → `Screen 1920x1080` 실측 후 진행(Work_5 세로 비율 문제 해소)
- 조작 통로: 팝업·`Choosing` 상태는 `qa_play interaction`·`qa_cheat apply`. Battle 방 단계는 MCP 왕복 지연(호출 간 10~20초 실측, `poll.log` real 시각)이 무입력 생존(~10~12초)을 넘어 `unity cmd eval`로 `MCPInteract`·`MCPCheatApply`를 직접 호출(스킬 "코드 진입점 직접 호출" 대체 경로, `qa.sh` `interact`·`cheat`·`det2`)
- 단계 판정(시나리오 번호): 1 합격(`SelectGun` 노출·`Lock` active=true 캡처 02) / 2 불합격 — 로비에 `Popup_Notify` 없어 알림 미표시, `selected` 유지(캡처 08, `Popup_Lobby.cs:100` null 가드) / 3 대상 없음 / 4 합격(`knifeSel=True gunSel=False`) / 5 불합격 — 라벨 5건 ID 노출·2줄(캡처 09) / 6 합격 `bgmVolume 0.4→0.7` 슬라이더 표시 갱신(캡처 10) / 7 합격 — 게임 씬에서 `Popup_Notify` `Text_Popup_Setting_Applied`(ID 노출, 캡처 33), 로비에서는 알림 없음 / 8 합격(`Close`) / 9 합격(`room=1 Battle Playing wave=1/2 alive=3 hp=100/100`, 캡처 11, 플레이어 y -3.895·적 스폰 ±9~10) / 10 합격(적 x 7.24→0.89, 3초) / 11 합격(`wave=2/2 alive=4`, 낙하 Crumb 6) / 12 합격(`Choosing`, `choice0 Battle Applex6 Watermelonx2`=`Wave_R02` 합, `choice1 Heal`, 캡처 13) / 13 합격(`room=2 kind=Battle hist=Battle,Battle`, 캡처 14) / 14 합격(HP 19 → Heal 입장 69, `Choosing`, 배경 Heal 청색, 캡처 17) / 15 합격(`HealMacaron,AttackSpeed,MoveSpeed` `rerollCost 10`, 캡처 19) / 16 합격(잔액 0 → `Popup_Notify` "Not enough Crumbs", 캡처 20; 리롤 성공 경로는 Crumb 지급 수단 부재로 미수행) / 17 합격(HealMacaron 86→100, MaxHp `125/125 ability_MaxHp=1`, 캡처 21·22) / 18 합격(5번째 방 클리어 시 `gun=True best=5`, `Popup_Notify` 열림 — 문구 `Text_Core_GunUnlock` ID 노출·버튼 `Text_Core_Confirm` 잘림, 캡처 23) / 19 합격(`editor_stop`→재진입 `selected=Knife gun=True best=10 bgm=0.7`, `lock=False`, 캡처 38) / 20 합격(방 9 `Battle/Boss:Pineapple`, 방 11 `Battle/Boss` 고정, 방 6~8 Heal/Ability 세트) / 21 합격(Pineapple `hp 1058`=450×2.35 방10, `1125` 방11; Pumpkin `1140`=600×1.9 방7) / 22 부분 불합격 — FSM 전환은 합격(위 요약), `Telegraph(Clone)` 활성·위치 (0,-3)·scale 3·sortingOrder -1·색 (1,0.2,0.2,0.35) 실측인데 캡처 25_3·30_3·34_7에 그려지지 않음 / 23 합격(`bossDead` → `result=Win`, `Popup_Result` `crumbTotal=30`, 캡처 35) / 24 합격(로비 복귀 `scene=Scene_Lobby`) / 25 합격(Pineapple 2회·Pumpkin 1회 실측) / 26 합격(`LoseRun`·무입력 사망 모두 `result=Lose` `Popup_Result`, 캡처 01·26; `gunNewlyUnlocked=true` 방10 패배 시)
- 비정상 입력: 취소 — 로비 escape → `Popup_Quit` 열림(캡처 07, 문구 ID 노출)·`Cancel` 닫힘; 게임 escape → `Popup_Pause` `ts=0 paused=True` HP 정지(캡처 31)·재 escape → 닫힘 `ts=1`; `Popup_Result` 열림 중 escape → Pause 미열림(무시) 합격; `Popup_RoomSelect`·`Popup_Ability` 열림 중 escape는 미실측. 연타 — `Popup_Lobby.Start` 3회 → 씬 로드 1회·에러 0; `Popup_RoomSelect.Select0` 3회 → `room` +1만(2·3회는 `InvalidOperationException` 문자열 반환, 콘솔 에러 0) 합격; `Popup_Ability.Select0` 3회 미실측. 중도 이탈 — Pause `GiveUp` → `Scene_Lobby` 복귀 합격
- 회피 3종(평지, 경계는 아래 표류 결함으로 표본 무효): 공간 끝 대기 — x=8(클램프)로 강제 후 4표본(3초 간격): 플레이어가 8.42→25.17로 표류하며 Apple 3마리가 d=-0.7(Attack)·-1.5·-2.4(Move)로 추종 — 정지 거리 0.7≈0.8u·개체 간격 0.8u(Apple 0.6u 이상) 합격, 표류는 불합격(모듈). 사거리 밖 유지 — 방 3에서 Banana d=4.92(유지 거리 5.0u) Attack 상태 5표본 유지 합격; Watermelon은 d=-2.97에서 Move 상태로 12초 정지(좌측 슬롯 Apple 1마리 사용 중, `Battle_MeleeSlotPerSide=2`) — 불일치 후보(모듈 슬롯 판정). 무입력 방치 — Start 후 hp 100→84→52→20→0, 사망 real 518.0→530.4 = 약 12초(`poll.log`); 계산치(접근 2.6초 + 100/24dps 4.2초 ≈ 7초)보다 길지만 `밸런스컨셉`에 무입력 생존 목표가 없어 판정 불가(컨셉 정의 결손)
- 반복 누수: 투사체 풀 `projTotal=24` 고정, Gun 연사 중 `projActive=2` → 해제 2초 뒤 `0` 합격; 유닛 풀 `unitsTotal=27`(적 24·보스 2·플레이어 1) 12방 진행 후 동일 합격
- 공격 조작: Gun — `simulate_key j down` 중 `Attack_Gun_02` 스프라이트·투사체 2·Apple 1 처치(Crumb 2) 합격, 단 사격 중에도 표류 계속(`px 8.62→14.86`, 컨셉 "정지 연사" 불합격). Knife — `j press` 직후 스프라이트 `Jump_03`·`AttackRange` 비활성(캡처 36 플레이어 화면 밖) — 판정 불가(표류·피격 중 표본)
- 화면 렌더: 캡처 39장 중 팝업 전수 — Lobby(02)·Quit(07)·Setting(09·10·32)·Notify(20·23·33)·HUD(11·14)·RoomSelect(13·17·21·24)·Ability(19)·Pause(31)·Result(01·26·35). 분홍 머티리얼·빈 화면 없음. 화풍 3특징(둥근 벡터 외곽선·픽셀 격자 없음·플랫 채색+광택) 월드(캐릭터·배경·타일)·UI 전 캡처 일치. 화면 높이 비율: 플레이어 잉크 1.0u/13u(ortho 6.5)=7.7%, Apple ≈6.9%, Pumpkin 13.5%, 배경 100%, 바닥선 20% — `리소스컨셉` 규격 없음(컨셉 담당)
- 컨셉아트 유사성: 로비(캡처 02 vs `Concept_Scene_Lobby`) 구도 ○·배치 ○·크기 비율 ○(카드 440:START 560)·색 ✕(선택 테두리 노랑 vs 파랑, 잠금 카드 회색 처리 없음)·스타일 ○·요소 존재 ✕(중앙 요리사 없음, 최고순번 배지 별 아이콘 대신 호박 아이콘)·텍스처 ○(대상 아님)·라벨 여백 ○ = 6/8 75% 불합격. 게임(캡처 11 vs `Concept_Scene_Game`) 구도 ○·배치 ○·크기 비율 ✕(플레이어 7.7% vs 컨셉 ≈40%, 적 동급)·색 ○·스타일 ○·요소 존재 ✕(HP바 하트 아이콘·이력 점선 없음)·텍스처 ○·라벨 여백 ○ = 6/8 75% 불합격
- UI 텍스트(`qa_ui text`, 전수 17건 로비·11건 게임 HUD+RoomSelect·9 Ability·4 Pause·6 Result·3 Notify·6 Setting): `issue` 있음 1건 — `Popup_Notify/.../Control_Button_Box_Green/AddonSlot/Addon_Button_Box__Text` "Text_Core_Confirm" `truncated·overflow` 198x50, 2줄. 크기·줄 수 직접 계산 — 한 줄 전제 라벨 2줄: `Popup_Setting` BgmLabel·SfxLabel·FullscreenLabel(fontSize 24)·ApplyButton/Label·DefaultButton/Label(22) 5건 불합격(ID 문구 길이 원인, 데이터); 폰트 0.8배 미만 없음(역할별 최대 대비: 카드 이름 44/36/42·설명 28/26·버튼 64/40/36·제목 80/48/72). 테두리 안쪽 폭: 9-slice `spriteBorder` 미조회로 "기준 없음"(`Control_GameFrame`·`UI_Casual_Button` 프리팹 지목)
- 표시 갱신 대조: SetBgm 0.7 → 슬라이더 위치 갱신(캡처 10), MaxHp → HUD `125/125`(캡처 22·23), Crumb 수거 → HUD 14·34(캡처 13·17), 이력 아이콘 열 방 종류별 아이콘·배경 톤(Battle 회색·Heal 청색·Ability 보라·Boss 적색) 일치. 이력 9방 이상은 최근 8개만 표시(캡처 24, 첫 Battle 탈락)
- 시간 갱신 항목: HP(직후·2초 뒤) 무입력 시 3초당 약 -30 변화; 보스 FSM 3초 간격 표본 ≥5회

## 비고
**컨셉**
- `리소스컨셉` 규격에 캐릭터·적·보스의 "화면 높이 비율"이 없어 크기 판정 기준값 부재 — 컨셉아트는 플레이어 ≈40%(380/941px), 실측 7.7%. 카메라 `orthographicSize 6.5`(화면 13u)와 PPU128·잉크 1.0u의 조합이 정본과 어긋남 — 컨셉에 기준을 두고 씬 구성(카메라)·프리셋(스케일) 중 조정 주체 결정 필요
- `밸런스컨셉`에 무입력 생존 시간·플레이어 피격 넉백 여부·방 좌우 경계(벽) 정의 없음 — 실측 무입력 사망 약 12초, 플레이어 넉백 있음, 바닥 60u 폭에 벽 없음
- `씬컨셉 Scene_Lobby` UI에 `Popup_Notify` 미등재 — Gun 잠금 클릭 알림·설정 적용 알림이 로비에서 표시 불가 (코드는 null 가드로 무성 생략)

**데이터**
- `Text` 테이블 미존재 ID 11건(화면 노출 실측): `Text_Core_GunUnlock`(로비 Gun 카드 설명·해금 알림·결과 팝업 해금 라벨)·`Text_Core_Confirm`(Notify 버튼, 잘림)·`Text_Core_RoomSelectTitle`·`Text_Popup_Setting_{BGM,SE,Fullscreen,Apply,Default,Applied}`·`Text_Quit_Title`·`Text_Quit_Text`. `_Data/Table/Text` xlsx `Popup_Setting` 시트(Work_5 기재)는 `TableText.json`에 미반영 — 익스포트 필요
- 로비 최고 순번 배지 아이콘이 호박(`Icon_Casual_Room_Boss` 추정)으로 표시 — 컨셉아트는 별 배지. 아이콘 ID 지정 확인 필요

**리소스 제작**
- Gun 캐릭터의 대기·이동 스프라이트가 Knife 실루엣(`AnimationSheet_Casual_Player_Idle/Move` 공유) — 사격 동작(`Attack_Gun_*`)만 별도. `리소스컨셉` "케첩 건 실루엣 구분" 불일치
- 사운드 에셋 0건 유지(Work_5 기재) — BGM·SFX 재생 검증 불가(대상 없음)

**모듈**
- 플레이어 표류: 피격(넉백) 후 입력 없이 x+ 방향 약 1.85u/s로 계속 이동, 카메라 클램프 8을 넘어 화면 밖으로 나가고 바닥(±30) 끝에서 낙하(y -783 속도, 사망 없음). 재현: Start → 무입력 10초 → `eval` `Player.transform.position.x` 0→4.4; 텔레포트 x=0 후 피격 없으면 정지. 담당: `Battle` `Object_UnitBase.TakeHit` 넉백 종료 처리 / `Object_PlayerBase` 이동 입력 폴링
- Gun "정지 연사" 미준수: 사격 중 표류 계속(위 결함과 동일 원인 가능)
- 방 경계 없음: 바닥 60u·카메라 클램프 8u만 있고 벽 콜라이더 없음 → 플레이어·적이 화면 밖 이동 가능
- 근접 슬롯: Watermelon이 좌측 슬롯 1개만 사용 중인데 3.0u 대기 거리에서 12초 정지(`LocalBattleManager` `m_MeleeSlots` 판정 확인 필요)
- `LocalRoomManager.MCPCheatApply("ClearRoom")`가 `Ended` 상태에서도 방을 넘김(치트 한정)
- `HitStopRoutine`이 매 명중마다 `timeScale=1` 복원 → 외부 슬로모(`set_timescale`) 무효. QA 계측 한계이자 향후 슬로모 연출 충돌 지점
- `Popup_Lobby` `Text_Core_GunUnlock` 포맷이 Gun 카드 설명 위치에 표시 — 미해금 시 설명 대신 해금 조건 노출 의도면 정상, 아니면 `Popup_Lobby.cs` 표시 로직 확인
- 데미지 숫자 팝 미구현 유지(Work_5 기재) — 명중 통지 API 부재

**프리셋 구성**
- `Telegraph.prefab`: 활성·위치·틴트 정상인데 렌더 안 됨(캡처 25_3·30_3·34_7). `sortingOrder -1/Default`, 바닥 `-5`·배경 `-10`보다 앞이라 정렬 원인 아님 — 스프라이트 참조(`UI_Common_Shape_Circle128`)·머티리얼·스케일 3 확인 필요
- `Popup_Notify`(Library) 버튼 라벨 198x50에 긴 ID가 2줄로 잘림 — 문구 데이터 보정 후 재확인
- `Popup_HUD` 이력 슬롯 8개 — 9방 이상이면 앞 이력 탈락(컨셉 "지나온 방 이력 상시 표시"와 어긋남, 상한 정책 컨셉 확인)
- `Popup_HUD` HP바 하트 아이콘·방 배지 이력 점선(컨셉아트) 없음, 로비 중앙 요리사 없음, 선택 테두리 색 노랑(컨셉 파랑)·잠금 카드 회색 처리 없음 — 유사도 75%
- 로비 `Popup_Quit`·게임 `[LogManager]` 인게임 디버그 콘솔 버튼(우측 파란 배지, 캡처 전건)이 플레이 화면에 노출 — 씬 구성(`DebugConsole` 활성) 확인

**절차·도구**
- MCP 왕복 지연(10~20초/호출)으로 Battle 방 단계는 `eval` 직접 호출로 수행 — 결과 값은 동일 API(`MCPInteract`·`MCPCheatApply`)라 판정 근거 유효. `Popup_RoomSelect`·`Popup_Ability` 열림 중 취소 입력, `Popup_Ability` 연타, 리롤 성공, Knife 콤보 판정, 경계 조건 회피 표본은 미실측
- `resource_file source`는 `slot` 인자를 거부(`'source' 액션이 인식하지 못하는 파라미터: slot`) — 스킬 문서의 "`art` 슬롯으로 조회" 표기와 불일치, 인자 없이 호출해 `art.dir`을 얻음
- `qa_cheat get`이 로비 단독 상태에서는 미조회(게임 씬 플레이 중 조회로 갈음)
- 이번 세션 저장 데이터 변경(되돌릴 수단 없음): `gunUnlocked` true·`bestRoom` 10·BGM 0.7 — 첫 실행 상태 검증이 필요하면 PlayerPrefs 초기화 필요
