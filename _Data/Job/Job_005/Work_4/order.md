# 업무지시서

## 1. Delegate 잔여 매니저 인스턴스 정리

**대상 스킬**: 게임개발_모듈_폴더_구성

**"content"**: 라이브러리 `Delegate` 모듈(`inAsset=false`)이 `Assets/__Game/_Core/Prefab/[Global].prefab`에 남긴 `[DelegateManager]` PrefabInstance(guid `834215310b3c54f4184233ac78424713`, Missing Prefab) 제거

**업무**

- 근거: `_Data/Job/Job_004/Work_6/result.md` `## 비고` 결함 ⑫ — `inAsset` 끄기의 "매니저 인스턴스 정리"가 `[Global].prefab`에는 적용되지 않은 잔여분. 두 씬은 `[Global].prefab` 인스턴스만 참조(씬 파일에 guid 0건)
- `Delegate` 모듈의 `inAsset`·메타는 바꾸지 않는다 (이미 false) — 잔여 인스턴스 정리 절차만 수행. 셋업(`editor_util setup`)으로 재구성하지 않는다(`Scene_Lobby` 카메라 4.0 오버라이드 소멸)
- 완료 기준: `[Global].prefab`에 guid `834215310b3c54f4184233ac78424713` 참조 0건, 두 씬 계층에 Missing Prefab 0건, 다른 매니저 인스턴스 수 변동 없음

## 2. Battle 모듈 코드·프리팹 수정

**대상 스킬**: 게임개발_모듈_폴더_작성

**"content"**: 유닛 풀 고갈 예외 해소, 대기 개체 반대쪽 슬롯 이동, 같은 방향 연속 넉백 감쇠

**업무**

- 정본: Work_1 개정 `밸런스컨셉`("적 그룹 공통" 양측 슬롯 유지·"피격 넉백" 연속 감쇠 N·R), `게임컨셉` "적 접촉"·"무입력 정지" 원칙
- ⑧ 풀 고갈: 원인은 `FSMState_UnitDie`가 Die 애니메이션 길이만큼 기다린 뒤 `Despawn`해 사망 연출 개체가 풀을 점유하는 동안 다음 웨이브 `SpawnWave`가 오면 `m_EnemyPoolSize=8` 초과. 수정 — `[LocalBattleManager].prefab` `m_EnemyPoolSize`를 "화면 내 최대 적 수(`Battle_MaxEnemyOnScreen`) + 웨이브 최대 마릿수(`TableWave` 실측)"로 올리고, `SpawnUnit`은 풀이 비면 예외 대신 `Despawn` 완료를 기다렸다 스폰하는 지연 큐로 바꾼다(지연 발생 시 원인·대기 시간 로그). 완료 기준: 방 6 Apple 9마리 3웨이브를 `KillEnemies`로 연속 전멸시켜도 예외 0·전 웨이브 정원 스폰
- ⑨ 표류: `LocalBattleManager.RequestMeleeSlot`·`FSMState_EnemyMove`에서 같은 쪽 슬롯이 찼고 반대쪽이 비면 대기 개체가 반대쪽 슬롯을 받아 플레이어를 지나 이동한다. 플레이어 피격 처리(`LocalBattleManager` 313행 부근)에 같은 방향 연속 넉백 카운트를 두고 N회 초과분은 거리 × R, 반대 방향 피격·이동 입력 시 초기화. N·R은 `BattleConst`에 상수로 둔다(리터럴 금지). 완료 기준: 방 1 Knife 무입력 30s 플레이어 |Δx| ≤ 6.0u, 적이 좌·우 양측에 배치됨(실측 x)
- 라이브러리 수정 금지 — 원인이 거기면 게임 쪽 우회·`_Temp/라이브러리_수정요청.md` 기록. CS 템플릿 제약(`게임개발_모듈` 노드 규칙) 준수

## 3. 컴파일·익스포트

**대상 스킬**: 유니티엔진_컴파일_실행

**"scope"**: 컴파일 통과 후 `게임개발_모듈_폴더_익스포트`로 원본 반영·재임포트

**업무**

- 완료 기준: `recompile_status completed`·콘솔 에러 0, 익스포트 verify 통과, 플레이 실측으로 1·2번 업무의 완료 기준을 각각 확인해 레포트에 수치로 남긴다(플레이 종료 `stopped`·`Scene_Lobby isDirty:false`)
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
