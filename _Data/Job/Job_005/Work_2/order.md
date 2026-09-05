# 업무지시서

## 1. 제목 정본 조회

**대상 스킬**: 게임개발_구성_컨셉_질문

**"question"**: `게임컨셉` "프로젝트 설정"의 게임 제목(productName)과 로비 제목 라벨이 참조하는 텍스트 ID

**업무**

- 근거: `_Data/Job/Job_004/Work_6/result.md` `## 비고` 결함 ⑬ — `Text_Core_GameTitle` NameKor "게임"·NameEng "Game"·NameJap "ゲーム", 게임 제목 "Kitchen Riot"(플레이어 설정 productName)

## 2. 게임 제목 텍스트 수정

**대상 스킬**: 게임개발_구성_데이터_테이블_텍스트_작성

**"content"**: `Text` 테이블 `Core` 시트 `Text_Core_GameTitle` 3개 언어 값을 "Kitchen Riot"으로 수정

**업무**

- NameKor·NameEng·NameJap 전부 "Kitchen Riot"(고유명사 — 번역·음차하지 않는다)
- 같은 시트에 제목을 담은 다른 행("Game"·"게임" 값)이 있으면 목록으로 보고만 하고 고치지 않는다
- 완료 기준: 원본(`_Data/Table/Text`) 행 값 3개 반영

## 3. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"scope"**: `데이터` 전 종류

**업무**

- 완료 기준: `Assets/_Library/_Core/Resources/Table/TableText.json` `Text_Core_GameTitle` 3값이 "Kitchen Riot", 익스포트 verify 통과, 컴파일 `completed`·콘솔 에러 0
- 로비 라벨 표시 폭 초과 여부는 Work_6 QA가 판정한다 (여기서는 플레이하지 않는다)
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
