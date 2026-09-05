# 업무지시서

## 1. 플레이어 설정 조회

**대상 스킬**: 유니티엔진_설정_질문

**"question"**: `player` 영역 현재 값(companyName·productName·bundleVersion·defaultScreen·resolution) 전체

**업무**

- 근거: `job.md` ⑲ — `companyName=DefaultCompany`

## 2. companyName 적용

**대상 스킬**: 유니티엔진_설정_구성

**"edits"**: `companyName` → "YoungBok Wi"(저장소 커밋 작성자명), 그 외 무변경

**업무**

- `set_player_settings --settings={"companyName":"YoungBok Wi"} --confirm=true`. PlayerPrefs 저장 경로가 companyName 하위 레지스트리 키라 바뀐 뒤 기존 저장(`DefaultCompany/Kitchen Riot`)은 읽히지 않는다 — 사실을 보고하고 새 키에서 첫 실행 상태로 시작됨을 Work_5 QA 전제로 남긴다

## 3. 설정 검증

**대상 스킬**: 유니티엔진_설정_검증

**"scope"**: `player` companyName 기대값 "YoungBok Wi", productName "Kitchen Riot" 유지

**업무**

- 완료 기준: `get_player_settings` 응답 일치, 컴파일 영향 없음(심볼 무변경)
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
