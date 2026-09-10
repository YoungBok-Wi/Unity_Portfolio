# 업무지시서

## 1. 데이터 정의 현황 조회

**대상 스킬**: 게임개발_구성_데이터_질문

**"question"**: 고정값 전체 정의(ID·타입·값·systemName), `Wave`·`Character`·`Enemy`·`Boss` 테이블 필드 구조와 행 전체, 타입 export 산출 경로(`Table_Const`·테이블 클래스)

**업무**

- 목표: 신규 고정값 ID 중복 없음 확인, `Wave` 행 키 규칙(`Wave_R{순번}_W{웨이브}`)·마지막 구간 RoomMax 확인, 열 추가 대상 확정
- 이 Work는 추가 전용이다 — 기존 고정값·열 삭제·이름 변경은 하지 않는다 (Work_5_1 담당, 컴파일 보호)

## 2. 고정값 생성

**대상 스킬**: 게임개발_구성_데이터_고정값_생성

**"constId"**: Room_BossCycle, Battle_HitStunSec, Battle_LowHpRatio (대상만 달리해 3회)

**"content"**: 보스 확정 주기 / 피격 경직 시간 / 저체력 경고 임계

**업무**

- systemName은 기존 `Room_*`·`Battle_*` 고정값과 같은 값을 쓴다 (1에서 확인)
- 완료 기준: 3건 등록 응답 `success:true`

## 3. 고정값 구성

**대상 스킬**: 게임개발_구성_데이터_고정값_구성

**"constId"**: Room_BossCycle, Battle_HitStunSec, Battle_LowHpRatio (대상만 달리해 3회)

**"content"**: 타입·설명 — `Room_BossCycle` int "방 순번이 이 값의 배수면 다음 방은 Boss 확정(단일 선택지)", `Battle_HitStunSec` float "넉백 완료 뒤 추가 경직 시간(초)", `Battle_LowHpRatio` float "HP 비율이 이 값 미만이면 HUD 저체력 경고 깜빡임"

**업무**

- 완료 기준: 3건 정의 verify `success:true`

## 4. 값 근거 조회

**대상 스킬**: 게임개발_구성_컨셉_질문

**"question"**: `밸런스컨셉` 개정본의 보스 주기·경직 시간·저체력 임계·웨이브 변형 구성 원칙·`ComboWindow` 값

**업무**

- 목표: 5·7의 입력값을 정본에서 옮겨 적는다 (Work_1 결과 `_Data/Job/Job_008/Work_1/result.md` 참조)

## 5. 고정값 작성

**대상 스킬**: 게임개발_구성_데이터_고정값_작성

**"constId"**: Room_BossCycle, Battle_HitStunSec, Battle_LowHpRatio (대상만 달리해 3회)

**"content"**: 5 / 0.1 / 0.3

**업무**

- 완료 기준: 3건 값 verify `success:true`

## 6. 테이블 구조 추가

**대상 스킬**: 게임개발_구성_데이터_테이블_구성

**"tableId"**: Wave, Character (대상만 달리해 2회)

**"content"**: `Wave`에 `Variant`(int, 웨이브 변형 번호 1·2) 열 추가, `Character`에 `ComboWindow`(float, 쿨타임 종료 후 다음 단 재입력 허용 창 초) 열 추가

**업무**

- 열 추가만 한다 — `InputBuffer`·`Icon` 열은 유지 (Work_5_1에서 제거)
- 기존 `Wave` 행의 `Variant`는 1로 보정, 기존 `Character` 행의 `ComboWindow`는 Knife 0.4·Gun 0
- 완료 기준: 두 테이블 verify `success:true`

## 7. 테이블 행 작성

**대상 스킬**: 게임개발_구성_데이터_테이블_작성

**"tableId"**: Wave, Character (대상만 달리해 2회)

**"content"**: `Wave` 변형 2 행 전 구간 추가·마지막 구간 RoomMax 999, `Character` `ComboWindow` 값

**업무**

- `Wave` 변형 2: 구간(RoomMin~RoomMax)마다 기존 행과 웨이브 수가 같고 총 마릿수 ±1 이내, 종 비율을 바꾼 행(`Wave_R{순번}_W{웨이브}_V2` 키, `Variant` 2). 순번 1은 Apple만(예: 4·3), 순번 2 이상은 밸런스컨셉 변형 원칙대로
- 기존 마지막 구간(1에서 확인한 최댓값)의 RoomMax를 변형 1·2 모두 999로 넓힌다 (순번 상한 없는 진행)
- `Character`: Knife `ComboWindow` 0.4, Gun 0
- 완료 기준: verify `success:true`, 행 수 = 기존 + 변형 2 행 수

## 8. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"tableId"**: (전체)

**업무**

- 완료 기준: `Assets/_Library/_Core/GenerateScript/` 구조 코드와 `Resources/Table/*.json`에 신규 고정값 3건·`Variant`·`ComboWindow` 반영, 컴파일은 기존 코드가 신규 필드를 쓰지 않으므로 통과 상태 유지 (익스포트 응답 에러 0)
- `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 수정 금지 (export 산출물 갱신은 제외). DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
