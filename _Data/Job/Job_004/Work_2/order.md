# 업무지시서

## 1. 고정값 현황 조회

**대상 스킬**: 게임개발_구성_데이터_질문

**"taskContent"**: 고정값 `Battle_BossBgmPitch` 등록 상태와 `Battle_*` 고정값 현재 값·익스포트 사본(`Assets/_Library/_Core/Resources/Table/TableConst.json`) 조회

**업무**

- Work_1이 등록한 골격이 없으면 `게임개발_구성_데이터_고정값_생성`으로 등록한 뒤 진행한다(사유 보고)

## 2. 고정값 구성·작성

**대상 스킬**: 게임개발_구성_데이터_고정값_작성

**"taskContent"**: `Battle_BossBgmPitch` 타입 float·설명 확정(구성), 값 1.1 입력(작성)

**업무**

- 근거: `밸런스컨셉`(Work_1 개정본) 보스방 BGM 배속 항목, `리소스컨셉` "사운드컨셉" 1.1배
- 선행 `게임개발_구성_데이터_고정값_구성`으로 타입·설명을 맞춘 뒤 값을 쓴다

## 3. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"taskContent"**: 고정값 익스포트, 사본 실측

**업무**

- 완료 기준: `TableConst.json`에 `Battle_BossBgmPitch: 1.1` 실재, 생성 스크립트(`Assets/_Library/_Core/GenerateScript`)에 필드 반영, `유니티엔진_컴파일_실행` 절차로 컴파일 통과
- DataMCP export 무응답 시 `Fallback`(curl) 순서를 따르고 사본 파일 실측으로 반영을 확인한다. `confirmed`·`reuse` 무변경. 사용자에게 질문하지 않는다
