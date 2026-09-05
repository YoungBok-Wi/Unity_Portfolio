# [게임개발_모듈_폴더_작성] "Battle 유닛 풀 고갈 예외·무입력 넉백 표류 수정" 업무 레포트

## 요약
- ⑫ `[Global].prefab`에서 `Delegate` 잔여 `[DelegateManager]` PrefabInstance 블록·자식 참조 제거 — guid `834215310b3c54f4184233ac78424713` 0건, 에디터 `[Global]` 자식 15개 중 Missing 0
- ⑧ 유닛 풀: `[LocalBattleManager].prefab` `m_EnemyPoolSize` 8 → 12(화면 최대 8 + 웨이브 종별 최대 4), `SpawnUnit`이 풀 소진 시 예외 대신 대기열(`AliveEnemyCount`에 선반영, `Despawn` 반납 시 순서대로 스폰). 실측 방 6 3웨이브(6·6·6, 종별 Apple 3·Watermelon 2·Banana 1) `KillEnemies` 연속 전멸 → 웨이브별 스폰 6·6·6, 사망 연출 개체 18 동시 존재, 예외 0
- ⑨ 표류: `밸런스컨셉` 규칙을 "무입력 넉백 누적 상한 1.5u(마지막 이동 입력 위치 기준 순 밀림)"로 재개정(verify `success:true`) 후 코드 반영. 실측 방 1 Knife 무입력 30s dx=−0.73u·최대 |dx| 1.80u(기준 ≤ 6.0u), 적 양측 배치 프레임 92%(1421/1546), 피격 우 39·좌 26
- 컴파일 `completed`·`failed:false`·콘솔 에러 0, `module_manage export`·`verify` `success:true`, 재임포트 `.meta` 7건 실재, 플레이 종료 `stopped`·`Scene_Lobby isDirty:false`

## 완료업무

### Delegate 잔여 매니저 인스턴스 정리
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Prefab\[Global].prefab`
**작업내용**
- 수행 스킬: `게임개발_모듈_폴더_구성` 절차 1 `module_manage get Delegate/Library` — `inAsset:false`·`globalManagerPrefab: Prefab/[DelegateManager]`·`reuse:fixed` (변경 없음, 지시서대로 메타·inAsset 무변경)
- 정리: 절차 7의 setup 경로는 지시서 금지(`Scene_Lobby` 카메라 오버라이드 소멸)라 "매니저 프리팹 도구 제약" 규칙의 YAML 직접 편집으로 수행 — `[Global]` `m_Children`의 `{fileID: 651195669867624837}` 1줄과 `--- !u!1001 &2042600002405187441` PrefabInstance·stripped Transform 블록(2666자) 제거
- 실측: 파일 내 guid 0건·PrefabInstance 35개(36 → 35), 재임포트 후 에디터 `eval` `[Global]` 자식 = BankManager·DealManager·IconManager·LanguageManager·LogManager·NumberManager·PlayerPrefsSaveManager·QuitManager·SceneChangeManager·ShutdownManager·SoundManager·TableManager·TimeManager·구분선·BattleManager·CharacterManager (`PrefabUtility.IsPrefabAssetMissing` 전건 false), 두 씬 파일 guid 0건(원래 0)
- `module.json` `confirmed` 키는 파일 목록과 1:1 유지(모듈 파일 변동 없음)

### Battle 모듈 코드·프리팹·설계 수정
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\BattleConst.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\LocalBattleManager.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\FSMState_EnemyMove.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\Object_PlayerBase.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\Object_UnitBase.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\[LocalBattleManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\module.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
**작업내용**
- 수행 스킬: `게임개발_모듈_폴더_작성` → `기획_작성`(module.md 4문장) → `코드_작성`(상수·FSM 상태·오브젝트 베이스) → 서브스킬 `코드_매니저_로컬_작성`(LocalBattleManager) → `프리팹_작성`(YAML 직접 기입). 미선택: `코드_매니저_글로벌_작성`·`코드_셋업_작성`(변경 없음)
- ⑧ `SpawnUnit`: 풀 `Get` null이면 `m_PendingSpawns`에 넣고 `UpdateAliveCount(true)` 후 `Debug.Log` 남기고 null 반환. `Despawn` 말미 `FlushPendingSpawns`(반납 즉시 대기분 스폰), `AliveEnemyCount` = 생존 + 대기(`UpdateAliveCount` 한 곳), `ClearUnits`가 대기열 비움. 프리팹 `m_EnemyPoolSize: 12` — 근거 `Battle_MaxEnemyOnScreen` 8 + `TableWave` 27행 종별 최대 Apple 4
- ⑨-1 반대쪽 슬롯 이동: `RequestMeleeSlot(_unit, _side, _allowSteal = true)`에 회수 여부 인자, `GetMeleeSlotSide` 신설. `FSMState_EnemyMove` 근접 분기 — 보유 슬롯 쪽 ≠ 현재 쪽이면 플레이어를 지나 계속 이동, 같은 쪽 실패 시 반대쪽을 회수 없이 요청해 성공하면 건너감, 둘 다 실패면 기존 대기
- ⑨-2 넉백 누적 상한: `BattleConst.PlayerKnockbackDriftMax = 1.5f`. `Hit` 플레이어 분기 — 기준점(`m_PlayerKnockAnchorX`, 첫 피격 위치·`ResetPlayerKnockbackDrift`가 이동 입력마다 현재 위치로 갱신·`ClearUnits`에서 초기화) 대비 순 밀림이 넉백 방향으로 1.5u 이상이면 `KnockbackDist` 0. `Object_UnitBase.TakeHit` 조건 `0 < KnockbackDist` → `0 <=`로 거리 0에서도 경직 0.15s 유지(cheats의 시간 0은 그대로 미적용). `Object_PlayerBase.FixedUpdate` 이동 입력 시 `Battle.ResetPlayerKnockbackDrift()` 1행
- 1차 구현(같은 방향 연속 3회 초과 감쇠 0)은 실측 실패(30s dx=10.87u — 양측 교대 피격이 연속 횟수를 매번 초기화해 감쇠 미발동, 우 47·좌 24회 차 × 0.5u = 11.5u 순 밀림)라 폐기하고 `밸런스컨셉` "플레이어 공통"·검산 항목을 순 밀림 상한 규칙으로 재개정(`게임개발_구성_컨셉_밸런스_작성`·`검증` 재수행, verify `success:true`, 검산 3 × 0.5 = 1.5u ≤ 6.0u)
- MCPDetail에 `pendingSpawn`·`knockDrift` 노출. 소비처(`Assets` 전역 grep): `GetMeleeSlotSide` FSM 1 + 드라이버, `RequestMeleeSlot` 3번째 인자 FSM 1, `ResetPlayerKnockbackDrift` PlayerBase 1, `UpdateAliveCount`·`FlushPendingSpawns` private — 0건 없음. 베이스 가상 멤버 변경 없음, 신규 스크립트 없음. 리터럴은 `BattleConst` 상수(기존 `PlayerKnockbackDist`와 같은 정의처)·프리팹 직렬화 값
- 라이브러리 무변경 (`ObjectPool.Get` null 반환 계약 그대로 사용)

### 컴파일·익스포트·플레이 실측
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_4_J5\driver.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_4_J5\qa.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_4_J5\log.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_4_J5\log_run2.txt`
**작업내용**
- 컴파일(`유니티엔진_컴파일_실행`, 2회): `list_open_scenes` `Scene_Lobby isDirty:false` → `clear_console` → `recompile` → `recompile_status` `{"status":"completed","failed":false,"errors":[]}` → `get_console_logs --severity=error` `total 0`. 콘솔 비움 외 되돌릴 대상 없음
- 익스포트(`게임개발_모듈_폴더_익스포트` → `유니티엔진_재임포트_실행`): `module_manage export Battle/Game` `success:true`(2회), `verify allErrors:true` `success:true`, `AssetDatabase.Refresh()` `success`, 변경 7경로 `.meta` 전건 실재
- 플레이 실측(최종 3회차, `eval_file` 드라이버 코루틴): 로비 `SelectKnife`·`Start` → 방 1 무입력 30s `RESULT drift x0=0.00 x1=−0.73 dx=−0.73 maxAbsDx=1.80 hits+=39 hits−=26 bothSides=True bothFrames=1421/1546`, 종료 시 Apple 3마리 슬롯 −1·+1·−1(양측) → `ClearRoom`·`SelectRoom`으로 방 6 Battle → `RESULT pool waves=3 w1=6 w2=6 w3=6 spawnedPerWave w1=6 w2=6 w3=6 errors=0 state=Choosing waveIdx=3/3`(웨이브 전환 사이 `Die` 상태 개체 12~18 동시 존재) → 콘솔 에러 `total 0` → `editor_stop` `stopped`·`Scene_Lobby isDirty:false`
- 1회차(연속 감쇠 구현) 실측 `log_run2.txt`: drift dx=10.87 실패 → 재구현 근거. 풀 테스트는 1회차도 6·6·6·예외 0

## 비고
- 미확인 경로: `SpawnUnit`의 풀 소진 대기열 분기 — 풀 12로 방 6 3웨이브(종별 최대 동시 사망 연출 9)에서 소진이 일어나지 않아 `Debug.Log "풀 소진"`·`FlushPendingSpawns` 스폰 경로는 실행되지 않았다 (컴파일·정적 검토만). 소진 조건은 종별 12마리 초과 동시 존재라 현행 `TableWave`(종별 웨이브 최대 4 × 3웨이브)에서는 도달 불가 — Work_6 QA에서 별도 재현 불요
- 드라이버 1회차 결함: 방 도달 조건이 "정확히 6"이라 선택지에 Battle이 없는 회차에 지나쳐 방 100까지 진행(`Wave 테이블 방 순번 100` 예외 1건 — 드라이버 예외, 게임 코드·데이터 무변경). 이 과정에서 PlayerPrefs `BestRoom`이 99로 저장돼 `PlayerPrefs.SetString`으로 Job 시작 전 값(`BestRoom=11`·`SelectedCharacter=Gun`)으로 되돌렸다 — 최종 `11/Gun/True/0.4`
- `editor_util setup` 미실행, `confirmed`·`reuse`·`inAsset` 무변경, 씬 파일 무변경, 사용자 질문 없음
