# Job_004 업무 맥락

## 목적

개선 루프 3회차 — Job_003 재평가(`_Data/Job/Job_003/Work_7/result.md` `## 비고`) 결함 7건(체감 4·미관 3)을 영역별로 합쳐 일괄 수정하고 전 범위 재평가·커밋한다. 종료 시 `AskUserQuestion`으로 다음 방향을 묻는다.

## 사용자 지시 원문

"job 1,2,3에 이어지게 개선루프 한번 더 진행해줘. 일반방식으로 자율진행해주고. 이번에는 전체적인 상황을 메인 에이전트가 직접 확인 후에 (모든 질문 스킬로 확인) 그걸 고려해서 작업 구성해줘"

## 운영 규칙

- 회차 1 Job, 컨셉 → 데이터 → 리소스 → 모듈 → 프리셋 → 플레이테스트 → 커밋 1루프
- 자율 진행, 실행 모드 `일반`, 안정화 우선
- 씬 셋업(`editor_util setup`) 실행 금지 — `Scene_Lobby` 카메라 4.0 오버라이드가 소멸한다 (씬설정 변경 없음)
- `confirmed`·`reuse` 무변경, 라이브러리(`Assets/_Library`·`_Data/Module/Library`) 수정 금지 — 게임 쪽 우회
- DataMCP export 후 장시간 무응답 반복 이력 — `Fallback`(curl)·사본 실측으로 확인

## 메인 에이전트 직접 확인 결과 (2026-09-05, 질문 스킬 절차 수행)

**컨셉** (`concept_manage list` 5건: Game·Balance·Resource·Scene_Lobby·Scene_Game)
- `밸런스컨셉` "근접 접근 슬롯": 좌·우 2마리 상한만 있고 배정 우선순위(거리순)·적↔적 통행 규칙 없음 → 결함 ①의 정본 결손
- `밸런스컨셉` "적 Banana": 유지 거리 5.0u만 있고 후퇴 경로가 막힐 때의 규칙 없음 → 결함 ④의 정본 결손
- `게임컨셉`·`리소스컨셉`: 플레이어·적 겹침 허용은 있으나 표시 우선순위(정렬 순서) 규칙 없음 → 결함 ③의 정본 결손
- `리소스컨셉` "사운드컨셉": 보스방 BGM 1.1배·공격 시작 `SFX_Casual_Battle/Attack` 이미 정본 → 결함 ⑤⑥은 구현 결손 (컨셉 개정 불필요). 단 `게임컨셉` "데이터 구동" 원칙상 1.1 리터럴 금지 → 고정값 신설 필요
- `리소스컨셉` "규격 AnimationSheet_Casual_Boss": 캔버스 384·기준 높이 224 → 결함 ⑦은 산출물 결손
- `씬설정 Scene_Lobby` "취소 입력": "열린 팝업이 있으면 그 팝업이 닫힌다" 정본 실재 → 결함 ②는 구현 결손

**데이터** (`table_data list` 7건, `const_data get` 16건, `type_manage list`)
- `Enemy` Banana `StopDistance=5 Range=7 MoveSpeed=3`, Apple `StopDistance=0.8` — 컨셉 일치
- 보스방 BGM 배속 고정값 없음 (신설 대상), 근접 대기 거리는 `BattleConst.MeleeWaitDistance` 코드 상수

**모듈** (`module_manage list`·`path`, 코드 실독)
- `Assets/__Game/Battle/Script/FSMState_EnemyMove.cs`: 근접은 `RequestMeleeSlot` 실패 시 대기, 성공 시 `StopDistance<dist`면 `Move` — 슬롯이 없는 최근접 개체가 벽 앞에서 정지(①)
- `LocalBattleManager.RequestMeleeSlot`: 선착순 배정(거리 무관), 적↔적 콜라이더는 `IgnoreContact` 대상 아님(적↔플레이어만)
- 원거리: `dist < StopDistance×0.7`이면 `Move(-dir)` — 뒤 개체·벽에 막히면 `Range` 안이어도 공격 전환 없음(④)
- `BattleManager.PlayBGM(AudioClip)`: pitch 인자 없음, 보스방 분기 없음(⑤)
- `LocalBattleManager` 필드 `m_SfxHit`·`m_SfxDie`·`m_Bgm`만 — 공격 시작 SFX 필드·재생 지점 없음(⑥). `Assets/__Game/_Core/SFX/SFX_Casual_Battle_Attack.ogg` 실재

**프리셋** (`preset_manage list` Object 10·Popup 12·Control 12·Addon 6, 프리팹 YAML 실독)
- `Object_Player_Knife`·`Object_Player_Gun`·`Object_Enemy_Apple`·`Object_Boss_Pineapple`·`Object_Projectile` 전부 `m_SortingOrder: 0`(③)
- `Popup_Setting.prefab` `m_IsCloseByCancel: 0`, `Popup_Lobby` 0, `Popup_Pause` 1 — 로비에서 Setting·Notify 열림 중 취소 입력이 어느 팝업도 닫지 못하고 `LocalPopupManager.OnInputCancel`(라이브러리) 말단의 `Popup_Quit.Open()`으로 흐름(②). `Popup_Notify`는 라이브러리 프리팹(수정 금지)

**리소스** (`resource_node node` 12계열, `resource_file list` AnimationSheet·SFX)
- `AnimationSheet_Casual_Boss` Pineapple 5동작·Pumpkin 5동작 등록, 캔버스 실측 384x384. 화면 실측 219px는 잉크 높이 ≈207px(기준 224px 미달 ≈17px)(⑦)

**유니티엔진** (Unity CLI)
- `editor_status` ready·stopped, `Scene_Lobby` 열림 `isDirty:false`, 빌드 씬 Lobby(0)·Game(1), Unity 6000.4.2f1, 플레이어 설정 productName "Kitchen Riot" 0.1.0 Mono2x
- 에디터 도구 3건(`AutoAddressablesOnImport`·`AutoTextureSettingOnImport`·`PlayerPrefsViewerTool`), 유닛테스트 파일 0건(`Assets/_Test` 없음)

**깃**: `main` clean, HEAD `d61ff0e`(README 재구성)

## 수정 범위

- 컨셉: `밸런스컨셉` 근접 슬롯 거리순 배정·적↔적 통행 규칙, 원거리 후퇴 불가 시 사거리 안 발사 규칙, 보스방 BGM 배속 고정값 항목 / `게임컨셉` 또는 `리소스컨셉` 플레이어 > 적 표시 우선순위 규칙
- 데이터: 보스방 BGM 배속 고정값 신설·값 입력·익스포트
- 리소스: Pineapple 시트 잉크 높이 실측 후 기준 224px 미달 동작만 재제작·익스포트 (Codex 한도 시 사유 보고·미수행)
- 모듈 Battle: 근접 슬롯 거리순 재배정·벽 앞 교착 해소(①), 원거리 후퇴 불가 시 발사(④), 보스방 BGM pitch 고정값 적용(⑤), 공격 시작 SFX 필드·재생(⑥, `[LocalBattleManager]` 프리팹 배선 포함)
- 프리셋: 플레이어 정렬 순서 > 적(③), 로비 취소 입력이 최상단 팝업을 닫도록(② — `Popup_Setting` 취소 닫기·`Popup_Lobby` 코드 우회 중 원인에 맞는 쪽)
- QA: 전 시나리오 재평가·안정화 판정 (Job_003 7건 대조표 필수)
- 깃: 커밋·푸시

## 체인묶음 대응

- [컨셉] 기준 `게임개발_구성_컨셉_게임_작성`: 컨셉_질문 → 게임_작성 → 게임_검증 → 밸런스_작성 → 밸런스_검증 → 리소스_작성 → 리소스_검증 → 고정값_생성 (제외: 씬_작성·씬_검증·셋업 실행 — 씬설정 변경 없음, 테이블_생성 — 신규 테이블 없음, 리소스 파일 생성·제작 — 컨셉아트 변경 없음, 모듈·프리셋 생성/삭제 — 대상 변동 없음)
- [데이터] 기준 `게임개발_구성_데이터_고정값_작성`: 데이터_질문 → 고정값_구성 → 고정값_작성 → 데이터_익스포트
- [리소스] 기준 `게임개발_구성_리소스_파일_애니메이션시트_제작`: 리소스_질문 → 파일_구성 → 애니메이션시트_제작 → 파일_업로드 → 파일_익스포트 (제외: 파일_생성 — 등록 실재)
- [모듈] 기준 `게임개발_모듈_폴더_작성`: 모듈_폴더_구성 → 모듈_폴더_작성(코드 → 프리팹) → 컴파일 → 익스포트 (제외: 모듈_폴더_생성 — 등록 실재)
- [프리셋] 기준 `게임개발_프리셋_파일_오브젝트_구성`·`게임개발_프리셋_파일_팝업_구성`: 프리셋_질문 → 팝업_코드_작성 → 컴파일 → 오브젝트_구성 → 팝업_구성 → 익스포트 (제외: 노드 생성/구성·컨트롤·애드온·오브젝트_코드_작성 — 오브젝트 스크립트는 `모듈` 소속, 셋업 실행 — 오버라이드 소멸)
- [유니티게임QA] 기준 `QA_유니티게임개발_플레이테스트_테스트`: 질문 → 계획 → 치트_작성 → 테스트
- [깃] 기준 `깃_커밋_실행`: 커밋 → 푸시
