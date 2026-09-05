# [게임개발_모듈_작성] "Battle 모듈 GC 할당 제거 — 투사체 NonAlloc 판정·이펙트·낙하물 풀링" 업무 레포트

## 요약
- ㉑-1 `Object_Projectile.Update` `Physics2D.OverlapCircleAll` → `Physics2D.OverlapCircle(pos, r, s_NoFilter, s_Overlaps)`(정적 버퍼, 배열 할당 0)
- ㉑-2 `LocalBattleManager` 히트·스플래터·궤적 이펙트를 프리팹별 `ObjectPool`(16)·낙하물 `ObjectPool`(32)로 전환 — `Instantiate`/`Destroy` 제거, 수명은 `m_ActiveEffects` 타이머를 `Update` 에서 회수, 풀 소진 시 이펙트는 가장 오래된 것 회수·낙하물은 즉시 적립, `ClearUnits` 가 전량 반납
- 실측 판정: 게임 코드 관리 힙 할당 0.4~0.8KB/프레임(예산 1KB 이내). Job_006 의 12~13KB/프레임은 에디터(GameView·SceneView Repaint ≈11KB, eval 컴파일 220KB 스파이크) 몫으로 게임 결함이 아님 — ㉑ 해소(원인 분리)

## 완료업무

### 투사체 NonAlloc 판정
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Projectile\Script\Object_Projectile.cs`
**작업내용**
- 정적 `s_Overlaps`(`List<Collider2D>`)·`s_NoFilter`(`ContactFilter2D.NoFilter()`) 로 판정, 순회는 `for` 인덱스 — 명중·관통·Finish 로직 무변경

### 이펙트·낙하물 풀링
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\LocalBattleManager.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\BattleConst.cs`
**작업내용**
- `BattleConst.EffectPoolSize 16`·`CrumbDropPoolSize 32` 추가, `m_EffectPools`(프리팹→풀, 첫 사용 시 생성)·`m_ActiveEffects`(`SActiveEffect { Go, Pool, EndTime }`)·`m_CrumbDropPool`
- `SpawnEffect`: 풀 Get → 위치·`flipX`(`GetComponentsInChildren(s_Sprites)` 비할당) → 타이머 등록. `Update` 가 `EndTime` 경과분을 뒤에서부터 반납. `SpawnCrumbDrops`: 풀 Get, 소진 시 `AddCrumb` 즉시 적립. `CollectCrumb`: 반납. `ClearUnits`: 이펙트 풀 `Clear`·타이머 비움
- 컴파일 에러 0, 플레이 실측: `KillEnemies` 직후 이펙트 활성 2·비활성 30(풀 16×2) → 1.5초 뒤 활성 0·비활성 32(전량 반납), Crumb 적립 6, 콘솔 에러 0

### GC 실측·원인 분리
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_1_J7\gcprof.txt`
`C:\_Projects\Unity_Portfolio\_Temp\Work_1_J7\log_P3q.txt`
**작업내용**
- CLI 폴링 없는 세션(`log_P3q`): 프레임 15.6/15.8/14.9ms, `GetMonoUsedSizeLong` 증가 5.5/12.6/11.1KB/프레임(Job_006 과 동일 수준 — 이 지표는 에디터 할당을 포함)
- `ProfilerDriver` 계층 데이터(`_Temp/Work_1_J7/gcprof.cs`, 마지막 301프레임 self GC 집계): 플레이어 루프 = `Physics2D.ConvertCollision2DForScript` 0.37~0.76KB/프레임(라이브러리 `CharacterPhysics2DSide` 의 `OnCollision*2D` 콜백, 라이브러리 읽기 전용) 뿐. 에디터 포함(`profileEditor true`) 시 GameView Repaint 7.6+0.37+0.36, SceneView Repaint 2.4+1.1+0.37+0.16KB, `eval` 컴파일 220KB(1프레임)
- CLI `st` 폴링을 3초마다 하던 세션(`log_P3a`·`P3b`)은 폴링 eval 컴파일로 1.1~1.3초 스파이크·77MB 증가가 나온다 — 성능 실측은 창 안에서 폴링 금지(`_Temp/Work_5_J6/driver_q.cs` `PERFDETAIL` 라인에 30ms 초과 프레임 시각 기록 추가)

## 비고
- `module_manage export Battle`·`preset_manage export Object Object_Projectile` `success:true`
- 라이브러리 `CharacterPhysics2DSide` 의 `Collision2D` 콜백 할당(0.4~0.8KB/프레임)은 라이브러리 몫 — 라이브러리 수정 요청 대상(범위 밖)
