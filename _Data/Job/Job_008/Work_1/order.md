# 업무지시서

## 1. 컨셉 정본 전수 조회

**대상 스킬**: 게임개발_구성_컨셉_질문

**"question"**: `게임컨셉`·`밸런스컨셉`·`리소스컨셉`·`씬설정`(Scene_Game·Scene_Lobby)에서 이번 개정이 건드리는 항목의 현재 원문 — 방 선택·보스 출현·런 종료·공격 리듬(선입력)·넉백·경직·히트 이펙트·애니메이션·씬 전환·사용 모듈·Object 목록·애니메이션시트 규격·팝업 연출

**업무**

- 목표: `_Data/Job/Job_008/job.md` "해석·가정"과 충돌하는 정본 문구를 전부 찾아 개정 대상 목록을 만든다
- 완료 기준: 4문서 6건(게임·밸런스·리소스·Scene_Game·Scene_Lobby) 항목별 현재 원문과 개정 방향 표

## 2. 게임컨셉 개정

**대상 스킬**: 게임개발_구성_컨셉_게임_작성

**"content"**: 방 선택 규칙·보스 주기·런 종료·공격 리듬·넉백 곡선·경직·타격 연출 단계·저체력 경고·씬 전환 연출·플레이어 FSM·플레이어 씬 상주·SpriteAnim 참조 방식 개정

**업무**

- 방 선택: "선택지 세트 [Battle/Heal]·[Heal/Ability]·[Battle/Battle] 중 랜덤, 비전투(Heal·Ability) 방 클리어 직후는 [Battle/Battle] 고정, [Battle/Battle]의 두 선택지는 서로 다른 적 구성(웨이브 변형)" — [Battle/Boss] 세트와 "Boss 최소 순번·강제 순번" 문구 삭제
- 보스: "방 순번이 `Room_BossCycle`(5) 배수면 다음 방은 Boss 확정, 선택지 1개(Boss)만 제시. 보스 처치는 방 클리어이며 런은 계속된다"
- 런 종료: "종료 조건 — 플레이어 HP 0 (패배)만" — "Boss 방 클리어 = 승리" 문구를 전부 제거하고 결과 팝업은 도달 순번·Crumb 총량·Gun 해금 알림, 짧은 런 원칙은 "보스 주기 5방으로 순환, 런 길이는 생존 시간" 취지로 수정
- 공격 리듬: "공격 판정은 모션 2번째 프레임, 공격 중·쿨타임(`AttackInterval`) 중 입력 무시(선입력 없음), 쿨타임 뒤 `ComboWindow` 안 재입력이면 다음 단, 아니면 1단부터" — "모션 후반 선입력 허용" 문구 교체
- 넉백·경직: "넉백은 급가속 후 급감속 곡선(`AnimationCurve`)으로 이동, 완료 뒤 `Battle_HitStunSec` 경직" 추가
- 타격 연출: "Knife 궤적·히트 이펙트는 1·2·3단 순으로 커지고 진해진다(단계 배율은 `밸런스컨셉`)"
- 저체력 경고: 장르 요소에 "HP 비율 < `Battle_LowHpRatio` 이면 HUD 외곽 비네트가 연하게 깜빡인다" 추가
- 씬 전환: 장르 요소에 "런 종료·시작 씬 전환은 요리사 얼굴 타일이 화면 대각선 방향으로 순차 등장해 가리고 순차 소멸해 걷는 연출(`SceneChangeAni_Face`)" 추가
- 플레이어: "플레이어도 FSM(Idle·Move·Jump·Attack·Hit·Die)으로 동작하고 입력에 따라 상태를 전환한다", "플레이어 오브젝트 2종은 씬에 상주하며 런 시작 시 선택 캐릭터만 활성"
- 기획 원칙 "데이터 구동"에 "유닛 종류 분기는 enum이 아니라 파생 클래스 override로 한다" 한 줄 추가, "애니메이션" 항목에 "프레임은 Resources 로드가 아니라 프리팹 인스펙터 배열" 명시
- 완료 기준: verify `success:true`

## 3. 게임컨셉 검증

**대상 스킬**: 게임개발_구성_컨셉_게임_검증

**"scope"**: 2에서 개정한 항목 전부

**업무**

- 완료 기준: verify `success:true`, 불합격 0

## 4. 데이터 현황 조회

**대상 스킬**: 게임개발_구성_데이터_질문

**"question"**: 고정값 `Room_*`·`Battle_*`·`Ability_*` 전체 정의·값, `Wave` 테이블 열 구조·RoomMax 최댓값, `Character` 열(`InputBuffer`·`AttackInterval`), `Enemy`·`Boss` `Icon` 열 사용처

**업무**

- 목표: 밸런스컨셉 개정에 적을 고정값·테이블 현재값 확보 (Work_2 대상 확정 근거)

## 5. 모듈 현황 조회

**대상 스킬**: 게임개발_모듈_질문

**"question"**: 게임 모듈(Battle·Room·Character) 등록 메타(소속 노드·설명·inAsset·매니저 프리팹)와 모듈 노드 트리 — 재편 8모듈(PlayerCharacter·Room·RoomSelect·Game·Enemy·Boss·Unit·Data)의 소속 노드 후보

**업무**

- 목표: 씬설정 "사용 모듈" 개정에 쓸 모듈 ID·노드 확정 (Work_4 대상 확정 근거)

## 6. 프리셋 현황 조회

**대상 스킬**: 게임개발_프리셋_파일_질문

**"question"**: 팝업 7종·컨트롤 5종·오브젝트 10종 등록 메타와 씬 컨셉 UI·Object 등재 상태, `Object_Player_Knife`·`Object_Player_Gun`의 배치 방식(런타임 스폰) 기재 위치

**업무**

- 목표: 씬설정 Object 항목을 "씬 배치"로 바꿀 대상과 팝업 연출 규칙 기재 대상 확정

## 7. 밸런스컨셉 개정

**대상 스킬**: 게임개발_구성_컨셉_밸런스_작성

**"content"**: 해금·난이도 곡선의 보스 규칙 교체, 웨이브 변형, 넉백 곡선·경직, 공격 쿨타임·콤보 창, 저체력 임계, 타격 연출 단계 배율, 무한 진행 성장식 상한

**업무**

- 해금 곡선: "6번째 방 이후 [Battle/Boss] 25%"·"10번째 방 클리어 후 고정" 삭제 → "보스 주기 `Room_BossCycle` = 5 (순번 5·10·15… Boss 확정, 단일 선택지)", "비전투 방 직후 선택지 [Battle/Battle] 고정", Gun 해금 5번째 방(첫 보스) 클리어 유지
- 난이도 곡선: 성장식은 유지하되 "순번 11 이상은 `Wave` 마지막 구간(RoomMax 999) 구성 반복 + 성장식만 상승, 보스도 매 주기 같은 성장식" 명시, 보스 출현 구간 문구 교체
- 웨이브 변형: "`Wave` 테이블 `Variant`(1·2) — 같은 순번 구간에 구성이 다른 변형 2종, [Battle/Battle] 두 선택지는 변형 1·2를 하나씩, 단독 Battle은 무작위" + 변형 2 구성 원칙(같은 총 마릿수 ±1, 종 비율 교체)
- 넉백·경직: "넉백 진행 곡선 — 급가속 후 급감속(`AnimationCurve`, 정본은 `LocalGameManager` 인스펙터 `KnockbackCurve`, 기본 키 (0,0)·(0.25,0.85)·(1,1)), 넉백 완료 후 경직 고정값 `Battle_HitStunSec` = 0.1s(float), 보스 면역 유지"
- 공격 리듬: Knife "공격 주기 0.5s = 쿨타임(입력 무시), 판정 시점 모션 2번째 프레임(= 1/FPS s), 콤보 창 `ComboWindow` 0.4s(쿨타임 종료 후 재입력 허용 창, `Character` 열 — `InputBuffer` 대체)", Gun `ComboWindow` 0
- 저체력: "저체력 경고 임계 고정값 `Battle_LowHpRatio` = 0.3(float, 미만), 깜빡임 주기 1.0s·알파 0.15~0.45"
- 타격 단계: "Knife 궤적·히트 이펙트 배율 1단 1.0 / 2단 1.25 / 3단 1.5, 3단만 히트스톱(기존)" — 정본은 `LocalGameManager` 인스펙터
- 방 선택 세트 카테고리 수량: 4종 → 3종([Battle/Heal]·[Heal/Ability]·[Battle/Battle]) 및 검산 갱신, 런 길이 검산은 "생존 시간 기준"으로 교체
- 완료 기준: verify `success:true`

## 8. 밸런스컨셉 검증

**대상 스킬**: 게임개발_구성_컨셉_밸런스_검증

**"scope"**: 7에서 개정한 항목·검산 전부

**업무**

- 완료 기준: verify `success:true`, 불합격 0

## 9. 리소스 현황 조회

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `AnimationSheet_Casual_Enemy`·`Boss`·`Player` 타입 출력 슬롯(`resources`·leaf·assetPath)과 파일 목록, `Illust_Casual_Chef` 타입 규격(캔버스·PPU), `UI_Common_Shape` 타입 규격·파일, `Icon_Casual_*` 노드·타입 규격(아이콘 캔버스)

**업무**

- 목표: 리소스컨셉에 적을 신규 타입(`Icon_Casual_Face`)·신규 파일(`UI_Common_Shape/Vignette`) 규격과 SpriteAnim 익스포트 경로 변경 근거 확보

## 10. 리소스컨셉 개정

**대상 스킬**: 게임개발_구성_컨셉_리소스_작성

**"content"**: 애니메이션시트 참조 방식·경로, 시트 방향 규칙, 얼굴 아이콘 타입, 비네트 파일, 팝업 연출·Canvas Scaler 규격 추가

**업무**

- 애니메이션시트: "`Resources` 문자열 로드 폐기 — 출력 슬롯 `resources` 해제, 익스포트 경로 `Assets/__Game/_Core/SpriteAnim/`, 프리팹 인스펙터 프레임 배열 참조" 로 규격 교체 (타입 3종 공통)
- 시트 방향: "모든 시트 원본 프레임은 우향. 좌향으로 생성된 프레임은 수평 반전본으로 교체 반입한다(대상은 리소스 Work 실측)"
- 얼굴 아이콘: 신규 타입 `Icon_Casual_Face` — "씬 전환 타일용 요리사 얼굴, `Illust_Casual_Chef_Knife` 머리(모자+얼굴) 크롭 256x256 투명 배경, 파일 `Chef` 1건" 제작 목록 추가
- 비네트: `UI_Common_Shape` 파일 `Vignette` — "512x512 중심 투명 → 외곽 백색 알파 1 방사형 그라데이션(코드 합성), 저체력 경고용" 추가
- 팝업: "UI" 절에 "Canvas Scaler — Scale With Screen Size·1920x1080·Expand(match 0) 전 팝업 공통", "프레임형 팝업 연출 — Blocker 알파 페이드(`PopupAni_Alpha_Smooth`) + 프레임 회전 등장(`PopupAni_Rotation_Dynamic`, 열기 시작 z -14°·닫기 끝 z -14°, `Popup_Notify` 동일)" 추가
- 개수 검산(제작 대상 계열·파일 수) 갱신
- 완료 기준: verify `success:true`

## 11. 리소스컨셉 검증

**대상 스킬**: 게임개발_구성_컨셉_리소스_검증

**"scope"**: 10에서 개정한 항목·검산 전부

**업무**

- 완료 기준: verify `success:true`, 불합격 0

## 12. 씬설정 개정 (Scene_Game·Scene_Lobby)

**대상 스킬**: 게임개발_구성_컨셉_씬_작성

**"conceptId"**: Scene_Game, Scene_Lobby (대상만 달리해 2회)

**"content"**: 사용 모듈을 재편 8모듈로 교체, Object 플레이어 2종 씬 배치, 설명 문구를 보스 주기·런 종료·씬 전환 연출에 맞춤

**업무**

- Scene_Game "사용 모듈": `Room`·`Battle`·`Character` → `Unit`(플레이어·적 공용 베이스, 순수 코드)·`Enemy`(적 스크립트·FSM 상태, 보스 공용)·`Boss`(보스 추가 스크립트)·`PlayerCharacter`(플레이어 FSM·입력, 로컬 매니저 `LocalPlayerCharacterManager` — 씬 상주 플레이어 활성·참조)·`Room`(방 진행·웨이브·벽·카메라)·`RoomSelect`(선택지 세트·보스 주기·변형 배정, 로컬 매니저)·`Game`(전투 판정·풀·연출·능력·일시정지·BGM·씬 전환 연출, 전역+로컬 매니저)·`Data`(선택 캐릭터·Gun 해금·최고 순번 저장·Crumb 재화·누적, 전역 매니저) — 각 항목 설명 한 줄
- Scene_Game "Object": `Object_Player_Knife`·`Object_Player_Gun`을 "씬 배치 — 런 시작 시 `Data` 선택 캐릭터만 활성, 나머지 비활성" 으로 교체, 나머지 Object 유지
- Scene_Game 설명: "보스방 클리어 시 런 승리" → "보스 처치도 방 클리어, 순번 `Room_BossCycle` 배수 뒤 Boss 확정", 결과 팝업 설명에 승패 삭제, 씬 전환은 `SceneChangeAni_Face` 사용 명시, `FSM` 모듈 설명에 플레이어 포함
- Scene_Lobby "사용 모듈": `Character` → `Data`(선택·해금·최고 순번) + `Game`(로비 BGM 재생 `GameManager.PlayBGM`), 설명의 씬 전환 연출 명시 (`SceneChangeAni_Face`)
- 완료 기준: 두 문서 verify `success:true`. 외부 적용(셋업 실행)은 이 Work에서 하지 않는다 (Work_6 담당) — 스킬 절차에 셋업 단계가 있으면 "모듈 미등록·프리팹 미배선 상태라 Work_6에서 수행"으로 사유를 남기고 건너뛴다

## 13. 씬설정 검증

**대상 스킬**: 게임개발_구성_컨셉_씬_검증

**"scope"**: Scene_Game·Scene_Lobby 개정 항목 (사용 모듈은 "포함(등재 대기)" 상태 허용)

**업무**

- 완료 기준: verify `success:true` 또는 미등록 모듈 사유만 남은 불합격(Work_4 등록 뒤 해소) — 그 외 불합격 0
- `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 수정 금지. DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
