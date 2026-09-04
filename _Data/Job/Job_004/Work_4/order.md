# 업무지시서

## 1. Battle 모듈 메타 확인

**대상 스킬**: 게임개발_모듈_폴더_구성

**"taskContent"**: `Game/Battle` 모듈 등록·소속 노드·`localManagerPrefab` 확인 (변경 없으면 건너뛰고 사유 보고)

**업무**

- 근거: `_Data/Job/Job_004/job.md` "모듈" 확인 결과, `_Data/Job/Job_003/Work_7/result.md` `## 비고` 결함 ①④⑤⑥

## 2. Battle 모듈 코드·프리팹 수정

**대상 스킬**: 게임개발_모듈_폴더_작성

**"taskContent"**: 근접 슬롯 거리순 재배정·벽 교착 해소, 원거리 후퇴 불가 시 발사, 보스방 BGM pitch 고정값 적용, 공격 시작 SFX 필드·재생·프리팹 배선

**업무**

- 정본: Work_1 개정 `밸런스컨셉`("적 그룹 공통"·"적 Banana"·보스 BGM 배속), `게임컨셉` "타격 사운드"(공격 시작에 `SFX_Casual_Battle/Attack`), `리소스컨셉` "사운드컨셉"
- ① 근접 슬롯: `Assets/__Game/Battle/Script/LocalBattleManager.cs` `RequestMeleeSlot`를 플레이어 거리순 배정으로 바꾼다(같은 쪽 슬롯 개체보다 요청 개체가 가까우면 가장 먼 보유 개체의 슬롯을 회수해 넘긴다). `FSMState_EnemyMove.cs`는 슬롯 없는 개체가 `MeleeWaitDistance` 안에 있으면 `FacePlayer` 대기 유지. 적↔적 비트리거 콜라이더는 `IgnoreContact`로 충돌 무시(스폰 시 등록 — 기존 적↔플레이어 처리와 같은 경로). 완료 기준: 방 1 Apple 3마리 접촉 중 플레이어를 우벽·좌벽(x ±11.7)에 세워 각 12s 안에 피격 ≥ 1
- ④ 원거리: `FSMState_EnemyMove.cs` Ranged 분기에서 후퇴 이동이 막혀(`|vx|≈0` 또는 벽·개체 접촉) 유지 거리 미달이어도 `dist <= Range`면 `StateAttack`으로 전환한다. 완료 기준: 방 4 혼합군 무입력 15s에 Banana 투사체 ≥ 1
- ⑤ BGM pitch: `BattleManager.PlayBGM`에 pitch 인자(기본 1.0)를 두고, `LocalBattleManager`가 보스방 진입 시 `TableManager.instance.Const.Battle_BossBgmPitch`(Work_2 익스포트)로, 일반 방 복귀·로비에서 1.0으로 재생한다. 같은 클립 재생 중이어도 pitch가 다르면 갱신. 완료 기준: 방 8·11 `[BattleManager] pitch=1.10`, 로비 1.00
- ⑥ 공격 SFX: `LocalBattleManager`에 `m_SfxAttack` 필드를 두고 플레이어 공격 시작(Knife 각 단 휘두름 시작·Gun 발사)에 재생한다. `[LocalBattleManager].prefab` 배선은 `게임개발_모듈_폴더_프리팹_작성`으로 `Assets/__Game/_Core/SFX/SFX_Casual_Battle_Attack.ogg`를 꽂는다. 완료 기준: 공격 입력 시 `AudioSource` 재생 로그·`qa_play` detail에 `sfxAttack` 배선 노출
- 라이브러리(`Assets/_Library/**`) 수정 금지 — 원인이 거기면 게임 쪽 우회·`_Temp/라이브러리_수정요청.md` 기록. 리터럴 금지(고정값·테이블 참조). CS 템플릿 제약(`게임개발_모듈` 노드 규칙) 준수

## 3. 컴파일·익스포트

**대상 스킬**: 유니티엔진_컴파일_실행

**"taskContent"**: 컴파일 통과 후 `게임개발_모듈_폴더_익스포트`로 원본 반영·재임포트

**업무**

- 완료 기준: `recompile_status completed`·콘솔 에러 0, 익스포트 verify 통과, 플레이 실측으로 ①④⑤⑥ 완료 기준 각각 확인해 레포트에 수치로 남긴다(플레이 종료 `stopped`·`Scene_Lobby isDirty:false`)
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
