# [오케스트레이터_오케스트레이션_실행] "Job_006 폴리싱 5회차 최종 레포트" 업무 레포트

## 요약
- Job 판정: 폴리싱 5회차 완료 — Work 7건 전부 `Done`, 체크리스트 c01~c08 `Done` 8, `origin/main` `1075f27`까지 푸시(+ 본 레포트 커밋). 실행 모드 `직접`
- 점검 발견 6건(⑭ 정본 미배선 연출·SFX 4건, ⑮ Crumb 낙하·수거, ⑯ 일본어 문구 33행, ⑰ 잔재 문구 101행, ⑱ 미사용 에셋 44건, ⑲ companyName) 전건 수정·재QA 해소
- 재QA 신규 발견 2건 → 사용자 지시("발견되지 않을 때까지 반복")에 따라 다음 회차 Job_007로 이월: ⑳ 일본어 한자 글리프 결손(TMP 폰트 폴백 없음), ㉑ GC 매 프레임 할당 5.6~13KB(예산 1KB, 투사체 `OverlapCircleAll` 등)
- 예외 발생 Work 0건 — 작업패턴 학습은 자율 진행 지시로 "넘어가기"

## 완료업무

### 데이터 (Work_1)
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Table\Text\Core.xlsx`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table\TableText.json`
**작업내용**
- 게임 문구 33행 일본어 입력, 이전 템플릿 잔재 101행 제거(177 → 76행), 전 종류 익스포트 `success`·컴파일 에러 0

### 모듈 (Work_2)
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\CrumbDrop.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Script\LocalBattleManager.cs`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\Splatter.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\Slash.prefab`
`C:\_Projects\Unity_Portfolio\Assets\__Game\Battle\Prefab\CrumbDrop.prefab`
**작업내용**
- `게임컨셉` 183·187·199행 정본대로 처치 스플래터·Knife 궤적·능력 획득음·Gun 해금음 배선, 41·113행 Crumb 낙하(위로 튐 → 바닥) → 흡인(3.0u) → 수거(0.6u), 방 클리어·전환·치트 시 잔여 전량 적립 — 플레이 실측 전건 확인, `module_manage verify` Battle·Room `success`

### 리소스·설정 (Work_3·Work_4)
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Resource\inAsset.json`
`C:\_Projects\Unity_Portfolio\ProjectSettings\ProjectSettings.asset`
**작업내용**
- 미사용 에셋 44건 `inAsset` 해제(승인: 사용자 지시 인용, 실행 전 기록 `_Temp/미사용리소스_정리대상.md`) — `unused` 0·미싱 0·고아 meta 0
- `companyName` DefaultCompany → YoungBok Wi(PlayerPrefs 키 변경 → 첫 실행 상태 확인)

### 재평가·커밋 (Work_5·Work_6)
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Job\Job_006\Work_5\result.md`
`C:\_Projects\Unity_Portfolio\README.md`
**작업내용**
- 성능 3구간(프레임 15.5~17.4ms 경계·스파이크·GC 초과), 시나리오 68단계 합격 63·참고 2·불합격 1(⑳)·미실측 2, 회귀 세션 A·B Job_005와 동일 합격, 콘솔 에러 0
- README `폴리싱 작업` 5회차 결과 1줄, 커밋 8건·푸시

## 비고
- 다음 회차(Job_007) 편성 대상: ⑳ 일본어 글리프 — OFL 폰트(예: Noto Sans JP) TMP 폰트 에셋 생성 후 `DefaultFont`·`DefaultFont_Bold` 폴백 연결(`유니티엔진_에셋_폰트`, 리소스 `Font` 계열 등록) 또는 Japanese 지원 제외(라이브러리 `LanguageConst` 수정 요청) / ㉑ GC — `Object_Projectile.Update` `OverlapCircleAll` → NonAlloc 버퍼, 스플래터·궤적·낙하물 Instantiate 풀링, 프로파일러 마커 실측 선행
- 범위 밖 보고: 플레이어 빌드 실행 검증(빌드 스킬 없음 — 미커버), 미해석 GUID 11건(라이브러리·URP 패키지 내부)
- `confirmed`·`reuse` 무변경, `editor_util setup` 미실행, 라이브러리 무변경. 저장 데이터 새 키 `BestRoom=12 GunUnlocked=True SelectedCharacter=Gun BGMVolume=0.4`
