# [게임개발_모듈_폴더_작성] "Battle 처치 스플래터·Knife 궤적·해금/능력 SFX 배선, Crumb 낙하·수거 구현" 업무 레포트

## 요약
- ⑭ 정본 미배선 4건 배선 완료 — 처치 스플래터(`Splatter.prefab`, 사망 위치 0.5s), Knife 휘두름 궤적(`Slash.prefab`, `PlaySlashEffect` 판정 박스 중심·좌향 X 반전, 0.2s), 능력 획득음(`AddAbility` → `m_SfxLevelUp`), Gun 해금음(`PlayUnlockSfx` ← `LocalRoomManager.ClearRoom` 해금 분기). 실측: 방 1 Knife 10s 전투 스플래터 동시 최대 3·궤적 `StartStep` 같은 프레임 활성, 일시정지 상태에서 `AddAbility("MaxHp")` 호출 `[SoundManager] playing False→True`·`PlayUnlockSfx()` `False→True`
- ⑮ Crumb 낙하·수거 구현 — `CrumbDrop.cs`(신규 클래스)·`CrumbDrop.prefab`, `OnUnitDied`가 처치 보상을 `CrumbDropMax` 8개 이하 낙하물로 흩뿌리고(위로 튀어 발 위치 y로 낙하) 플레이어 3.0u 안 흡인·0.6u 안 수거, 방 클리어·방 전환·처치 치트는 `CollectAllCrumbs`로 잔여 전량 적립. 실측: 처치 10회 낙하물 10개 생성·최대 동시 4, 첫 수거 4.4s(Crumb 2), 방 클리어 시 Crumb 14(= Apple 7마리 × 2)·잔여 0, `KillEnemies` 치트 낙하 18 → 즉시 0·Crumb 32→68
- 컴파일: `Game.dll` 02:03:35 갱신(소스 02:03:11 이후)·콘솔 에러 0, `module_manage export` Battle·Room `success`, verify Battle·Room `success`, 플레이 4세션 종료 `stopped`·`Scene_Lobby isDirty:false`

## 완료업무

### Battle 모듈 설계·코드·프리팹 수정
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\CrumbDrop.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\LocalBattleManager.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\BattleConst.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\Splatter.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\Slash.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\CrumbDrop.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\[LocalBattleManager].prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\module.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\_Object\Object_Player_Knife\Script\Object_Player_Knife.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\Script\LocalRoomManager.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Room\module.md`
**작업내용**
- 수행 스킬: `게임개발_모듈_폴더_작성` → `기획_작성`(Battle `module.md` 내부기능 2문장·외부사용 예시, Room `module.md` 참조 절) → `코드_작성`(상수 7건·클래스 `CrumbDrop`) → 서브스킬 `코드_매니저_로컬_작성`(`LocalBattleManager` 필드 7·메서드 5·MCPDetail `crumbDrops`) → `프리팹_작성`(HitEffect 복제 YAML 3건 + `[LocalBattleManager].prefab` 배선). 미선택: `코드_매니저_글로벌_작성`·`코드_셋업_작성`
- 프리팹: `Splatter`(스프라이트 guid `438a0998…`, 정렬 0 — 유닛 아래), `Slash`(guid `cbae618a…`, 640px/PPU128 = 5u라 View 스케일 0.4 → 2.0u = Knife 박스 폭, 정렬 20), `CrumbDrop`(아이콘 guid `623cbf55…` PPU100 스케일 0.35 ≈ 0.45u, 정렬 1, `CrumbDrop` 컴포넌트 guid `8351ea68…`). 새 프리팹 guid는 재임포트 후 `.meta`에서 읽어 배선(`8c572b3b…`·`5b34583b…`·`8a042e63…`), SFX guid `93487ab6…`(LevelUp)·`a5e9c06c…`(Unlock)
- `SpawnEffect(prefab, point, sec, facing)` 공용 헬퍼로 히트·스플래터·궤적 통합(프리팹 없으면 생략 — 기존 HitEffect 계약 유지). `SpawnCrumbDrops`는 프리팹 없으면 즉시 적립(보상 손실 방지), 낙하물 값은 `amount / count` 균등 + 나머지 앞쪽 분배(보스 30 → 8개 4·4·4·4·4·4·3·3)
- `Object_Player_Knife.StartStep`에 `Battle.PlaySlashEffect(GetAttackBox().center, Facing)` 1행, `LocalRoomManager.ClearRoom` 첫 줄 `CollectAllCrumbs()`·해금 분기 `PlayUnlockSfx()` (Room→Battle 참조는 기존)
- 소비처(`Assets` 전역): `CollectCrumb` CrumbDrop 1·매니저 내부, `CollectAllCrumbs` Room 1 + 매니저 내부 3, `PlaySlashEffect` Knife 1, `PlayUnlockSfx` Room 1, `CrumbDrop.Launch`·`Value` 매니저 2 — 0건 없음. 신규 스크립트 `CrumbDrop` 방어 대조: 베이스·형제 없음(단독 MonoBehaviour) — 매니저 부재 시 갱신 중단, 플레이어 사망 시 흡인·수거 중단(`IsDead`), 바닥 y 클램프
- 리터럴: 상수 7건 `BattleConst`, 개수는 테이블 `CrumbDrop`, 프리팹 수명은 인스펙터 값. 라이브러리 무변경
- 값·동작 실측(`_Temp/Work_2_J6/driver.cs`, 3세션 + 직접 호출 1세션): 위 요약. `qa_play` detail `crumbDrops=0`·`ability_Attack=1` 노출 확인

### 컴파일·익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_2_J6\driver.cs`
`C:\_Projects\Unity_Portfolio\_Temp\Work_2_J6\log.txt`
**작업내용**
- 재임포트 `AssetDatabase.Refresh` 3회(스크립트·프리팹 `.meta` 생성 → guid 배선 → 재임포트), `recompile_status` `up_to_date`·`failed:false`는 Refresh 시 이미 컴파일된 상태 — "CLI 컴파일 검증" 규칙대로 `Library/ScriptAssemblies/Game.dll` 갱신 시각(02:03:35)이 소스 저장(02:03:11) 이후임을 대조해 성립, `get_console_logs` 전 심각도 0
- `module_manage export` Battle·Room `{"success":true}`, verify `allErrors` `success:true`(신규 `CrumbDrop.cs` module-Class 템플릿 통과), 변경 경로 `.meta` 전건 실재

## 비고
- 낙하물 흡인·수거 거리(3.0u·0.6u)·투척 속도는 정본에 값이 없어 `BattleConst` 상수로 두었다 — `밸런스컨셉`에 항목이 없으므로 컨셉 보강 후보(수치 변경 요구 시 `밸런스컨셉` 선행)
- Knife 궤적 스프라이트가 640px 원본이라 스케일 0.4로 표시 — 규격 문서(`리소스컨셉` 규격)에 `Illust_Casual_Slash` 계열 캔버스·기준 길이가 없음(컨셉 결손 후보, 표시는 정상)
- `editor_util setup` 미실행, `confirmed`·`reuse` 무변경, 씬 무변경
