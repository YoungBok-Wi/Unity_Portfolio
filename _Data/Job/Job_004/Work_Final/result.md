# [오케스트레이터_오케스트레이션_실행] "Job_004 개선 3회차 최종 레포트" 업무 레포트

## 요약
- Job 판정: 개선 3회차 완료 — Work 9건(`Work_1`~`Work_7`, 보완 `Work_3_1`, `Work_Final`) 전부 `Done`, 체크리스트 c01~c08 `Done`, `origin/main` `fe2b68f`까지 푸시
- 편성 근거: 메인 에이전트가 질문 스킬 절차로 컨셉 5문서·데이터(테이블 7·고정값 16)·모듈 코드·프리셋 YAML·리소스 시트·Unity 상태를 직접 확인해 결함 7건의 원인(정본 결손 3·구현 결손 3·산출물 결손 1)을 특정한 뒤 편성 (`_Data/Job/Job_004/job.md` "메인 에이전트 직접 확인 결과")
- 안정화 판정(`Work_6/result.md` `## 비고`): Job_003 결함 7건 전건 해소. **출시 보류 권고** — 신규 진행 차단 후보 1건(유닛 풀 고갈 예외 `InvalidOperationException: Apple 풀이 비었다 (크기 8)`), 체감 1건(무입력 넉백 일방 표류), 미관 4건. 53단계 중 52 실행(합격 49·부분 1·불일치 2), 게임 코드 콘솔 에러는 풀 고갈 예외 외 0
- 예외 발생 Work 1건(`Work_3`: DataMCP 약 20분 무응답) → 보완 `Work_3_1`이 원인 특정(`resource_file export`의 엔트리별 전체 타입 재로드, 640회 × 1.5s ≈ 16분 동기 블로킹). 수정은 하지 않았고 제안 3건 기록
- 사용자 지시("한 번 더")대로 이번 Job까지만 수행했으며 추가 Job은 만들지 않았다. 다음 방향은 사용자 답변에 따른다

## 완료업무

### 컨셉·데이터 (Work_1·Work_2)
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Const\consts.json`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableConst.json`
**작업내용**
- 근접 슬롯 거리순 재배정·적↔적 통행 허용, Banana 후퇴 불가 시 발사, 겹침 표시 우선순위(플레이어 2·적·보스 1·그 외 0), Boss BGM 배속 고정값 정본 — `concept_manage verify` 3건 `success:true` (`Work_1/result.md`)
- 고정값 `Battle_BossBgmPitch` float 1.1 등록·익스포트 — `TableConst.json`·`Table_Const.cs` 반영, 컴파일 `completed` (`Work_2/result.md`)

### 리소스 (Work_3·Work_3_1)
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Resources\SpriteAnim`
`C:\_Projects\Unity_Portfolio\_Data\Job\Job_004\Work_3_1\result.md`
**작업내용**
- Pineapple 시트 28장 잉크 높이 실측으로 재제작 대상을 `Pineapple_Idle` 1동작으로 축소, 재제작 후 `Idle_03` 209 → 222px(화면 219 → 234.1px), 147건 로드 null 0. `Idle_02` 250px는 동작 신축으로 허용 (`Work_3/result.md`)
- DataMCP 무응답 원인 특정·수정 제안 3건 (`Work_3_1/result.md` `## 비고`)

### 모듈·프리셋 (Work_4·Work_5)
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_UI\Popup\Popup_Lobby\Script\Popup_Lobby.cs`
**작업내용**
- `RequestMeleeSlot` 거리순 회수·적↔적 `IgnoreContact`, Ranged 후퇴 불가 시 `Attack` 전환, `PlayBGM` pitch 인자·보스방 1.10, `m_SfxAttack` 배선·Knife/Gun 공격 시작 재생 — 벽 배치 12s 피격 16/13, Banana 15s 발사 7회, 방 8·11 `pitch=1.10` (`Work_4/result.md`)
- 오브젝트 7종 `m_SortingOrder` 규격 반영, `Popup_Lobby.OnInputCancel` override로 취소 입력이 최상단 팝업만 닫음(라이브러리 무수정) — Notify 열림 중 `escape` 1회 Notify만 닫힘·`Popup_Quit false` (`Work_5/result.md`)

### 재평가·푸시 (Work_6·Work_7)
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Job\Job_004\Work_6\result.md`
`C:\_Projects\Unity_Portfolio\_Temp\QA`
**작업내용**
- 53단계 시나리오 재평가, Job_003 항목 ①~⑦ 대조표 전건 해소, 신규 결함 목록·안정화 판정 (`Work_6/result.md`)
- `origin/main` `d61ff0e..d6a2008` 푸시, Work_7 레포트 커밋 `fe2b68f` 추가 푸시 (`Work_7/result.md`)

## 비고
- 다음 회차 후보(사용자 결정, `Work_6/result.md` `## 비고` 상세): [모듈 Battle] ⑧ 유닛 풀 고갈 예외(사망 연출 중 개체 + 다음 스폰 > 풀 8 — 사망 즉시 반납 또는 풀 크기 = 최대 동시 + 사망 연출), ⑨ 무입력 넉백 일방 누적 표류(적↔적 통행 허용·거리순 슬롯의 부작용, 컨셉 보강 병행) / [리소스 제작] ⑩ Banana Move 109px < Apple 113px 위계 역전, ⑪ 보스 모션 간 높이 편차 / [씬 구성] ⑫ `[Global]/[DelegateManager]` Missing Prefab / [데이터] ⑬ 로비 제목 `Text_Core_GameTitle` "Game"
- 도구·서버 결함(수정 없음): DataMCP `resource_file export` 16분 동기 블로킹(`Work_3_1/result.md` 수정 제안 3건 — `resource_type_shared.ts:1137` `inAsset` 조기 반환·타입 1회 로드, `resource_file_export.ts` 이벤트 루프 양보), `set_timescale` 무성실패(`eval` 대체), `simulate_key`가 `wasPressedThisFrame` 미반영(Job_001~003 동일)
- 저장 데이터(PlayerPrefs) 최종: `GunUnlocked=True BestRoom=11 SelectedCharacter=Gun BGMVolume=0.4`. 작업패턴 학습은 자율 진행 지시에 따라 "넘어가기". `confirmed`·`reuse` 값은 Job 전체에서 변경하지 않았고 `editor_util setup`은 실행하지 않았다
