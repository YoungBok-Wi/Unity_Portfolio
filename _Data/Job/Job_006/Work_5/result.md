# [QA_유니티게임개발_플레이테스트_테스트] "성능테스트·플레이테스트 재평가(연출·SFX·Crumb·일본어 표시)·안정화 판정" 업무 레포트

## 요약
- 성능(방 1·방 10 전투 8마리·보스방, 로거·폴링 없이 10s 표본): frameMsAvg 17.4 / 16.9 / 15.5(예산 16.6 — 경계, 에디터 플레이 모드), frameMsMax 74~140(평균 3배 초과 스파이크 — 방 전환·스폰 프레임), gcAllocPerFrameKB 5.6 / 13.3 / 10.8(예산 1KB **초과**), mono 679~699MB. 초과 원인 후보: `Object_Projectile.Update`의 `Physics2D.OverlapCircleAll` 매 프레임 배열 할당(활성 투사체당), 라이브러리 `CharacterPhysics2DSide` 접촉 조회, 에디터 오버헤드 — 신규 발견 ㉑
- 시나리오 68단계(Job_005 60 유지 + 신규 61~68) 중 66 실행 — 합격 63·참고 2(45 좌벽 첫 피격 7.02s, 52 Banana 높이)·**불합격 1(66 일본어 표시 — 한자 전건 □ 글리프 결손, 신규 발견 ⑳)**·미실측 2(21·36 도구 한계/조건 미성립, Job_005 동일). 콘솔 에러 전 세션 0(P·P2·Q·Q2·A·B), 종료 전건 `stopped`·`Scene_Lobby isDirty:false`
- Job_006 점검 항목 ⑭~⑲ 전건 해소, 재점검(`project_manage unused` 0건·씬 Missing 0·프리팹 미싱 0·컴파일 에러 0)
- **안정화 판정: 출시 가능(영어·한국어) / 일본어 표시 결함 1건(⑳)·성능 예산 초과 1건(㉑) 신규 발견 → 다음 회차 편성 대상**

## 완료업무

### 성능테스트
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\log_P2.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\log_P.txt`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_성능테스트_질문`(성능 예산 — PC 60fps 16.6ms·스파이크 평균 3배·GC 0 목표 1KB 이상 지속 시 후보 지목·안정 구간 60프레임 이상) → `QA_유니티게임개발_성능테스트_테스트`. 측정은 엔진 내 코루틴이 `Time.unscaledDeltaTime`·`Profiler.GetMonoUsedSizeLong`·`Time.frameCount`를 구간 시작·끝에서 직접 읽음(CLI `get_performance_stats` 폴링은 왕복 자체가 스파이크를 만들어 구간 밖에서 1회만 조회 — `cpuFrameTimeMs 20.1 gpu 4.9 drawCalls 42`)
- 1차(P, 로거 코루틴·CLI 폴링 동반): 21.1 / 19.7 / 21.4ms·GC 6.1 / 5.8KB — 계측 오염으로 폐기, 2차(P2, 로거·폴링 없음·안정 대기 2~3s 후 10s): 방 1 231프레임 17.40ms(57.5fps) max 74.3·GC 5.59KB, 방 11 전투 594프레임 16.85ms(59.4fps) max 139.6·GC 13.25KB, 보스방(Pumpkin) 646프레임 15.48ms(64.6fps) max 130.1·GC 10.79KB, alloc 1029~1041MB
- 판정: 프레임 평균 — 방 1·방 11은 예산 +0.8/+0.25ms 초과(에디터 플레이 모드 오버헤드 포함, 빌드 기준 아님 — 경계), 보스방 통과. 스파이크 — 3구간 전부 평균 3배 초과 1회 이상(방 전환·웨이브 스폰·프리팹 Instantiate 프레임). GC — 3구간 전부 1KB 이상 지속 → 원인 후보: `Object_Projectile.Update` `Physics2D.OverlapCircleAll`(활성 투사체당 매 프레임 배열 할당, 방 11·보스방 Banana·Gun 투사체), 라이브러리 `CharacterPhysics2DSide`(수정 금지 — 요청 항목), 에디터 전용 오버헤드(MCP 런타임·프로파일러). 방 1(투사체 0)에서도 5.6KB라 게임 코드 외 요인이 절반 이상 — 프로파일러 마커 실측이 다음 회차 선행 과제

### 플레이테스트 계획 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\QA\시나리오_Scene_Lobby.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA\치트_Scene_Game.md`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_질문` → `계획` → `치트_작성`. 헤더에 Job_006 판정 대상 ⑭~⑲, 신규 61(스플래터)·62(궤적)·63(LevelUp SFX)·64(Unlock SFX)·65(Crumb 낙하·수거)·66(일본어 표시)·67(잔재 문구)·68(첫 실행). 치트 명세에 `KillEnemies`·`KillBoss`의 낙하물 즉시 적립 갱신, 저장 데이터 전이(companyName 변경 → 새 키 첫 실행)
- 실물 대조: 매니저·팝업·interactionId·cheatId Job_005와 동일(`qa_cheat get` 재조회 `[LocalBattleManager]` 9종 동일), 단계 수 60 → 68, 개체 수치 동일

### 전체 재평가·안정화 판정
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\cap`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\driver_q.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\run_q.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\log_Q.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\log_A.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_5_J6\log_B.txt`
**작업내용**
- 수행 스킬: `QA_유니티게임개발_플레이테스트_테스트`. 세션 Q(1차 — Gun 선택 오류로 전투 항목 무효, 일본어 UI 수치·캡처는 유효)·Q2(첫 실행·Knife 전투·일본어 진행)·A·B(Job_005 회귀 드라이버 재실행)·P·P2(성능). 조작은 `eval MCPInteract`(대체 경로)·`qa_play get`·`qa_ui text`(curl Fallback)·엔진 내 드라이버, 언어 전환은 `LanguageManager.SetLanguage`(직접 호출 — Setting 팝업에 언어 항목 없음, 종료 시 English 복원)
- 신규 61~68: 61 합격(방 1 처치 `Splatter(Clone)` 동시 최대 2, 0.5s 소멸) / 62 합격(`StartStep` 같은 프레임 `Slash(Clone)` 1) / 63 합격(Work_2 직접 호출 실측 — `timeScale 0` 1.5s 후 `AddAbility("MaxHp")` `[SoundManager] playing False→True`; 세션 Q2는 선택지 세트에 Ability 방이 나오지 않아 팝업 경로 미실측) / 64 합격(Work_2 `PlayUnlockSfx()` False→True + Q2 방 5 클리어 시 Notify "Cream Gun unlocked!" 열림·`gun=True`; 같은 프레임 SE는 처치음과 겹쳐 분리 불가 — 코드 경로 `ClearRoom` 해금 분기 1행) / 65 합격(처치 10회 낙하 10개·동시 최대 4, 첫 수거 2.46s Crumb 2, 방 클리어 시 Crumb 14·`crumbDrop=0`, `KillEnemies` 후 낙하 0·즉시 적립) / 66 **불합격**(일본어 전환 후 로비·Notify·Setting·Quit·해금·RoomSelect·HUD·Pause·Result `qa_ui` truncated/overflow/offScreen 0·`Text_` 0이나 캡처 실독 결과 한자 전건 □ — 로비 카드 "シェフナイフ"는 가나라 표시, 설명 "□□3□コンボ…"·버튼 "□□□□"(戦闘開始), Pause "□□□□/□□/□□/□□□□"; 원인 `Assets/TextMesh Pro/Resources/Fonts & Materials/DefaultFont.asset` `m_FallbackFontAssetTable: []`로 한자 글리프 없음, 에디터 경고 배지 100 → 531 증가(TMP 글리프 누락 경고)) / 67 합격(전 팝업 `Text_` 원문 0, `TableText.json` 76행) / 68 합격(companyName 변경 직후 `HasKey BestRoom=false`, Q2 첫 실행 `sel=Knife gun=False best=0 lang=English`)
- 회귀(세션 A·B, Job_005 60단계 자동 구간): 13·55 합격(30s dx 1.33·최대 1.80, 넉백 0 12회·경직 502프레임·양측 88%·슬롯 양측 100%) / 44·45(우벽 18·3.37s, 좌벽 5·7.02s — 참고 유지) / 48 합격(Knife·Gun SFX 같은 프레임 False→True) / 60·18 합격(Knife·Gun 자연 클리어 웨이브 2 `Choosing`·Crumb 14) / 46 합격(Banana 15s 6회·첫 1.97s) / 54 합격(방 6 6·6·6·동시 사망 13·예외 0) / 28 합격(방 9 이력 8) / 37 합격(11.000~11.733) / 47·51·57 합격(방 11 Pumpkin `pitch=1.10`, 13프레임 잉크 150~258 ∈ 134~280·Idle_01 225) / 30·31·32·33 합격(Win crumb 95 → 로비 `best=12 gun=True`, SelectGun → Gun, 세션 B 재시작 `sel=Gun gun=True best=12`) / 12·35 무입력 생존 세션 A 6.27s·B 5.43s는 Start 선행 편향 표본(Job_005 E·F 8.67/7.72s 유지, 코드 무변경) / 42·43 Job_005 C·D 판정 유지(코드 무변경 — 세션 B 드라이버 타이밍 결함으로 이번엔 미실측) / 40 합격(팝업 9종 영어·일본어 `qa_ui` issue 0) / 53 합격(`[Global]` 자식 16)
- 비정상 입력 3종: 취소 입력 합격(로비 escape 2단·Quit·일본어 상태 동일), 연타·중도 이탈 Job_005 판정 유지(코드 무변경) / 회피 3종 합격(경계·Banana 유지·무입력 Job_005 표본 유지) / 반복 누수: 낙하물 `CrumbDrop(Clone)` 생성 10·수거·클리어 후 0, 스플래터·궤적 `Destroy` 수명 후 0, 풀 6/6/6 복귀 / 저장 유지: 세션 B 재시작 전후 `Gun/True/12` 동일
- 화면·화풍: 캡처 lobby_ja·pause_ja 실독(한자 □ — 66 불합격 근거), 그 외 Job_005 판정 유지(리소스 무변경)
- 재점검: `project_manage unused` preview `unused: []`(candidateCount 208), 두 씬 Missing 0, 프리팹 81건 미싱 0, 콘솔 에러 0, `git status` `_Temp`·`_Data/Job` 외 변경 없음

## 비고
**안정화 상태 판정**
- 출시 가능 여부: **영어·한국어 출시 가능** — Job_006 점검 6건 전건 해소, 전 경로 회귀 합격, 콘솔 에러 0. **일본어는 표시 결함(⑳)** — 지원 언어 목록에 Japanese가 있어 일본어 OS에서 첫 실행 시 한자 □ 노출
- 진행 차단 0 / 체감 1(⑳ 일본어 OS 한정) / 미관 0 / 성능 1(㉑ GC 예산 초과, 프레임 경계) / 대상 외 1(에디터 디버그 콘솔 배지)

**Job_006 점검 항목 대조**
| 분류 | 항목 | 이번 판정 | 실측 근거 |
|---|---|---|---|
| 정본 미배선 | ⑭ 스플래터·궤적·LevelUp·Unlock SFX | 해소 | 61·62·63·64 |
| 정본 미구현 | ⑮ Crumb 낙하·수거 | 해소 | 65 |
| 데이터 | ⑯ 일본어 33행 | 해소(데이터) / 표시는 ⑳ | 문구 값 존재·가나 표시, 한자 글리프 결손 |
| 데이터 | ⑰ 잔재 101행 | 해소 | 67 |
| 리소스 | ⑱ 미사용 에셋 44건 | 해소 | `unused` 0 |
| 설정 | ⑲ companyName | 해소 | 68 |

**신규 발견 (수정하지 않음)**
- ⑳ [리소스 제작·유니티엔진 에셋 폰트] 일본어 한자 글리프 결손 — TMP `DefaultFont.asset`(한글·라틴·가나 포함, 한자 없음) 폴백 테이블 비어 있음. 해소 방향: 일본어 글리프를 가진 OFL 폰트(예: Noto Sans JP)로 TMP 폰트 에셋을 만들어 `DefaultFont`·`DefaultFont_Bold` 폴백에 연결(`유니티엔진_에셋_폰트`), 또는 지원 언어에서 Japanese 제외(라이브러리 `LanguageConst` — 수정 금지 대상이라 요청 기록 필요). 캡처 `cap/lobby_ja.png`·`pause_ja.png`
- ㉑ [모듈 Battle] GC 매 프레임 할당 5.6~13.3KB(예산 1KB) — 게임 코드 후보 `Object_Projectile.Update` `Physics2D.OverlapCircleAll`(NonAlloc 버퍼로 대체 가능), 나머지는 라이브러리·에디터 몫 추정. 스파이크(방 전환·스폰 Instantiate)는 풀링 대상(스플래터·궤적·낙하물 `Instantiate/Destroy`)

**미실측·도구 한계**
- 63·64 팝업 경로 SFX — Ability 방 미출현·처치음 중첩으로 직접 호출 실측(Work_2)으로 대체
- 12·35 무입력 생존 — Start 선행 편향(Job_005와 동일 원인), 42·43 — 세션 B 드라이버 타이밍(플레이어 사망 선행), Job_005 C·D 판정 유지
- `qa_ui`는 글리프 결손(□)을 잡지 못한다 — 캡처 실독이 유일한 판정 수단(도구 개선 후보: TMP `HasCharacters` 대조)
- 세션 Q 1차는 해금 상태에서 `SelectGun`이 실제 선택돼 Knife 전투 항목 무효 → `DeleteAll` 후 Q2 재실행

**기타**
- 저장 데이터 최종(새 키 `YoungBok Wi\Kitchen Riot`): `BestRoom=12 GunUnlocked=True SelectedCharacter=Gun BGMVolume=0.4 lang_lng=English`
- 세션 8회 전부 `editor_stop` → `stopped`, `Scene_Lobby isDirty:false`, 산출물 무수정
