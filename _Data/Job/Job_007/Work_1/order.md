# 업무지시서

## 1. 서체 정본 조회

**대상 스킬**: 게임개발_구성_컨셉_질문

**"question"**: `리소스컨셉` 서체 항목(재사용 대상 21행 `Font_Casual_GyeonggiTitle_*`, "UI" 절 서체 규정)과 지원 언어 근거

**업무**

- 근거: `_Data/Job/Job_006/Work_5/result.md` 비고 ⑳ — 한자 글리프 결손, 지원 언어 English·Korean·Japanese(라이브러리 `LanguageConst`)

## 2. 일본어 폴백 서체 항목 개정

**대상 스킬**: 게임개발_구성_컨셉_리소스_작성

**"content"**: 서체 규정에 "본문·표시 서체 `Font_Casual_GyeonggiTitle_*`(한글·라틴·가나) + 일본어 한자 폴백 `Font_Casual_NotoSansJP`(SIL OFL 1.1, TMP 폴백 테이블 연결, Dynamic 아틀라스)" 명시, 신규 제작(반입) 대상 목록에 서체 1건 추가

**업무**

- 같은 화풍 판정: Noto Sans JP는 획이 고른 산세리프라 경기천년제목(둥근 제목체)과 다르지만 한자 폴백 전용(가나·한글·라틴은 주 서체 유지)임을 근거로 적는다
- 완료 기준: verify `success:true`, 개수 검산(신규 제작 대상 계열 수 +1) 갱신

## 3. 리소스컨셉 검증

**대상 스킬**: 게임개발_구성_컨셉_리소스_검증

**"scope"**: `리소스컨셉` 서체·신규 제작 대상 항목

**업무**

- 완료 기준: verify `success:true`, 불합격 0
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
