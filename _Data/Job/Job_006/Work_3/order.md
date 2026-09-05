# 업무지시서

## 1. 미사용 에셋 등록 대조

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `job.md` ⑱의 미사용 에셋 48건(`_Core/Image` 44건 + `Resources/Image` 무기 일러스트 4건)이 어느 리소스 타입·entry에 대응하는지(`resource_file search` files 역조회), Work_2가 배선한 Splatter·Slash·SFX 2건은 제외

**업무**

- 근거: `project_manage unused` preview(2026-09-06) — `unused` 44건·`resources` 중 `Illust_Casual_Weapon_RollingPin/Skewer/Sprinkle/Whisk` 4건. Work_2 배선 후 `project_manage unused`를 다시 받아 대상을 확정한다

## 2. 미사용 리소스 정리

**대상 스킬**: 게임개발_구성_리소스_파일_구성

**"content"**: 대상 전건 `inAsset` 끄기(cleanup) — 승인: 사용자 지시 "발견 가능한 것들 찾고 수정" (2026-09-06)에 따라 승인된 것으로 본다, 범위 = 1번 업무가 확정한 목록 전건

**업무**

- 스킬 절차의 "실행 전 기록"(`_Temp/미사용리소스_정리대상.md`)·"역참조 대조"(에셋 검증 참조 실측 — `usage` verdict `used`·`unknown`은 제외)·미대응 파일 보고를 지킨다. `unknown`(Resources 하위·코드 문자열)은 끄지 않는다 — 무기 일러스트 4건은 코드·테이블 문자열 참조 0건을 `Grep`으로 실증했을 때만 대상
- 완료 기준: 대상 전건 `notInAsset`, `Assets` 사본·`.meta` 제거, 컴파일 에러 0, 끊긴 참조 0(에셋 검증 "참조 실측")

## 3. 에셋 무결 검증

**대상 스킬**: 유니티엔진_에셋_검증

**"scope"**: 프로젝트 전역 미싱·GUID·고아meta + 정리 대상 참조 실측

**업무**

- 완료 기준: 미싱 프리팹·스크립트 0, 미해석 GUID 0, 고아 `.meta` 0, 콘솔 에러 0
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
