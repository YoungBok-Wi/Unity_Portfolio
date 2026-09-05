# 업무지시서

## 1. 폴백 SDF 에셋 분리 재생성

**대상 스킬**: 유니티엔진_에셋_폰트_구성

**"assetPath"**: `Assets/__Game/_Core/Font/Font_Casual_NotoSansJP_Regular SDF.asset`·`Font_Casual_NotoSansJP_Regular_Bold SDF.asset`

**업무**

- 근거: Work_5 재QA 신규 발견 ㉒ — Bold 서체(`DefaultFont_Bold` 64pt·패딩 2)에 36pt·패딩 9 폴백을 물리면 외곽선·밑판 머티리얼이 폴백 글리프에서 뒤틀린다(`お知らせ` 知 그림자 사각, `閉じる` 閉 흰 밑판)
- "content": 기존 36/9 에셋 삭제 후 `DefaultFont`용 96pt·패딩 4, `DefaultFont_Bold`용 64pt·패딩 2로 각각 생성(SDFAA·1024·Dynamic·멀티 아틀라스), 폴백 테이블을 각 에셋으로 재연결, `리소스컨셉` 서체 항목에 샘플링·패딩 정합 규칙 추가
- 완료 기준: 두 DefaultFont 폴백 GUID가 각각의 새 에셋, 일본어 캡처(Notify·Pause·Result·Quit)에서 폴백 글리프가 주 서체 글리프와 같은 외곽선·색, 일본어 197자 `HasCharacters` 전건 true, 콘솔 에러 0
