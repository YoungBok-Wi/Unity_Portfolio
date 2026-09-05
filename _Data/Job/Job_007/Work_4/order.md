# 업무지시서

## 1. Battle 모듈 GC 할당 제거

**대상 스킬**: 게임개발_모듈_폴더_작성

**"content"**: 투사체 판정 NonAlloc 전환, 이펙트·낙하물 풀링

**업무**

- 근거: `_Data/Job/Job_006/Work_5/result.md` 성능 ㉑ — GC 5.6~13.3KB/프레임, 후보 `Object_Projectile.Update` `Physics2D.OverlapCircleAll`(활성 투사체당 매 프레임 배열), `SpawnEffect`·`SpawnCrumbDrops`의 `Instantiate/Destroy`
- ㉑-1 `Object_Projectile`(오브젝트 프리셋 스크립트 — `모듈` 소속 규칙): `Physics2D.OverlapCircle(pos, radius, ContactFilter2D.NoFilter(), s_Buffer)`로 정적 `List<Collider2D>` 재사용, `m_HitList.Contains` 유지. `preset_manage export Object_Projectile`도 통과
- ㉑-2 `LocalBattleManager`: 히트·스플래터·궤적·낙하물 프리팹마다 라이브러리 `ObjectPool`(`Get`/`Return`, 크기 — 히트 16·스플래터 8·궤적 4·낙하물 32) 생성, `SpawnEffect`는 `Get` 후 수명 코루틴(`WaitForSeconds` 캐시 또는 타이머 리스트)으로 `Return`, `CrumbDrop`은 `CollectCrumb`에서 `Return`. `ClearUnits`가 활성 이펙트·낙하물을 전부 반납. `flipX`는 반납 시 원복
- 단순화 허용: `SpawnEffect` 수명 관리는 `List<(GameObject, ObjectPool, float endTime)>`를 `Update`에서 순회(할당 없음) — `단순화:` 주석
- 완료 기준(플레이 실측, 로거 없이): 방 1 무입력 10s·방 10 전투 10s·보스방 10s `gcPerFrameKB`가 Job_006 대비 감소(목표 게임 코드 몫 0 — 투사체 활성 구간 Job_006 13.3 → 방 1 수준 이하), 처치 연출·낙하물·궤적 동작 회귀 없음(스플래터·궤적·낙하 수거·잔여 0), 예외 0
- 라이브러리 수정 금지(`ObjectPool` 계약 그대로), CS 템플릿 제약 준수

## 2. 컴파일·익스포트

**대상 스킬**: 유니티엔진_컴파일_실행

**"scope"**: 컴파일 통과 후 `게임개발_모듈_폴더_익스포트`(Battle)·`preset_manage export`(Object_Projectile) → 재임포트

**업무**

- 완료 기준: `recompile_status completed`·콘솔 에러 0, export `success`, 플레이 실측 수치 레포트
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
