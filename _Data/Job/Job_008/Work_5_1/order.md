# 업무지시서

## 1. 삭제 대상 사용처 확인

**대상 스킬**: 게임개발_구성_데이터_질문

**"question"**: 고정값 `Room_BossMin`·`Room_BossForce`·`Room_ChoiceSet3`, `Character.InputBuffer`, `Enemy.Icon`·`Boss.Icon` 열의 정의와 코드·컨셉 사용처(`Assets/__Game/**` grep 0건 확인)

**업무**

- 목표: Work_4·Work_5 뒤 참조가 남아 있지 않음을 확인한다. 남아 있으면 삭제하지 않고 남은 참조를 레포트 예외로 보고한다

## 2. 고정값 삭제

**대상 스킬**: 게임개발_구성_데이터_고정값_삭제

**"constId"**: Room_BossMin, Room_BossForce, Room_ChoiceSet3 (대상만 달리해 3회)

**업무**

- 완료 기준: 3건 삭제 `success:true`

## 3. 테이블 열 삭제

**대상 스킬**: 게임개발_구성_데이터_테이블_구성

**"tableId"**: Character, Enemy, Boss (대상만 달리해 3회)

**"content"**: `Character.InputBuffer` 열 삭제, `Enemy.Icon`·`Boss.Icon` 열 삭제 (행 값도 함께 제거)

**업무**

- 완료 기준: 3테이블 verify `success:true`

## 4. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"tableId"**: (전체)

**업무**

- 완료 기준: 구조 코드·JSON에서 삭제 항목 0건, 익스포트 응답 에러 0. 컴파일 통과는 Work_6 씬 검증이 확인한다 (에러가 나면 참조 잔존이므로 예외 보고)
- `confirmed`·`reuse` 무변경. DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
