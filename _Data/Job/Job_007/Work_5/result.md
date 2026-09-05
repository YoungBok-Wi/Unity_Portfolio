# [QA_유니티게임개발_플레이테스트_테스트] "폴리싱 6회차 재QA — 성능·일본어·풀링·회귀·재점검" 업무 레포트

## 요약
- 성능(폴링 없는 엔진 내 계측 `_Temp/Work_1_J7/log_P3q.txt`): 프레임 15.6/15.8/14.9ms(Job_006 17.4/16.85/15.48), 게임 코드 GC 0.4~0.8KB/프레임(예산 1KB 이내, `ProfilerDriver` 집계 `_Temp/Work_1_J7/gcprof.txt`) — Job_006 의 12~13KB/프레임은 에디터 Repaint·eval 컴파일 몫으로 분리
- 신규 시나리오 69(일본어 한자)·70(풀링) 합격, 회귀 세션 A·B 결과 Job_006 과 동일(`log_A7`·`log_B7`), 재점검 미싱 0·고아 meta 0·콘솔 에러 0
- 신규 발견 ㉒(Bold 폴백 글리프 외곽선·밑판 뒤틀림) → 보완 Work_3_1 에서 즉시 수정·재캡처 합격. 그 외 신규 0건

## 완료업무

### 성능 재측정
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_1_J7\log_P3q.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_1_J7\gcprof.txt`
**작업내용**
- 3구간(방 1 4s·방 10 전투 10s·보스방 10s) 결과와 Job_006 대조
  - 방 1: 15.59ms(max 45.7) / Job_006 17.40 — 힙 증가 5.5KB/f(동일)
  - 방 10 전투: 15.78ms(max 23.6) / 16.85 — 힙 증가 12.6KB/f(13.3)
  - 보스방: 14.92ms(max 23.6) / 15.48 — 힙 증가 11.1KB/f(10.8)
- 힙 증가 지표는 에디터 할당을 포함 — 프로파일러 계층(플레이어 루프만) self GC 합 0.37KB/f(방 10)·0.76KB/f(에디터 포함 세션), 전부 라이브러리 `CharacterPhysics2DSide` `Collision2D` 콜백 변환. 게임 코드 `GC.Alloc` 마커 0건
- CLI 폴링(3s `eval`)이 있으면 컴파일로 1.1~1.3s 스파이크·77MB 증가 — 계측 창 안 폴링 금지 규칙으로 기록(Work_4 레포트)

### 신규 시나리오 69·70
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\cap_q2\`
**작업내용**
- 69 일본어: 세션 Q(`run_Q7`) 캡처 9화면(lobby·notify·setting·quit·unlock·roomselect·hud·pause·result) + ability 는 Result 대체(무입력 사망 후) — □ 0, `qa_ui issueOnly` 전 화면 `{}`, 로그 배지 경고 0. `TableText.json` 일본어 197자 `DefaultFont`·`DefaultFont_Bold` `HasCharacters(…, true, true)` 모두 true(`_Temp/Work_1_J7/hascheck.cs`), 실행 중 활성 TMP 19건 결손 0
  - 1차 캡처에서 ㉒ 발견(Bold 폴백 `お知らせ` 知 그림자 사각·`閉じる` 閉 흰 밑판) → Work_3_1 수정 후 재캡처 `notify_ja_fix2`·`pause_ja_fix2`·`result_ja_fix2`·`quit_ja_fix2` 합격
- 70 풀링: 방 1 `KillEnemies` 직후 이펙트 활성 2·비활성 30 → 1.5s 뒤 활성 0·비활성 32(풀 16×2), Crumb 적립 6(= 처치 합), `ClearRoom` 뒤 활성 0

### 회귀·재점검
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_1_J7\log_A7.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_1_J7\log_B7.txt`
**작업내용**
- 세션 A(Knife 생존·표류·벽·SFX·자연 클리어·Banana·보스)·B(Gun 생존·SFX·풀·경계·보스·포기·재시작) RESULT 라인 Job_006 과 전건 동일(FAIL 3건도 동일 — 드라이버의 방 13 overshoot·afterGiveUp 대기 한계, 게임 결함 아님), 콘솔 에러 0
- `project_manage unused preview`: unused 1 = `Font_Casual_NotoSansJP_OFL.txt`(라이선스 동봉 문서 — 참조 없음이 정상, 유지), 프리팹 33건 미싱 스크립트 0, 두 씬 미싱 0, 고아 meta 0
- 콘솔 경고 1: `DontDestroyOnLoad only works for root GameObjects` — `IngameDebugConsole`(플러그인) `Awake`, 라이브러리 `LogManager.InitFirst` 경유 — 라이브러리·플러그인 몫(범위 밖)

## 비고
- Job_007 항목 대조: ⑳ 일본어 한자 — 해소(Work_1~3, 3_1), ㉑ GC — 해소(Work_4: 게임 코드 할당 예산 이내, 잔여는 에디터)
- 안정화 상태 판정: 안정 — 신규 발견 ㉒ 1건은 본 회차 안에서 수정·재검증, 미해소 결함 0
- 신규 발견(㉒부터): ㉒ 해소. 추가 발견 0건 → 반복 종료 근거
- 범위 밖 보고: 라이브러리 `CharacterPhysics2DSide` `Collision2D` 콜백 할당 0.4~0.8KB/f, `IngameDebugConsole` DontDestroyOnLoad 경고, 플레이어 빌드 실행 검증(빌드 스킬 없음), 미해석 GUID 11건(라이브러리·URP)
