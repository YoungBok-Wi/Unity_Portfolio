# 업무지시서

## 1. 오브젝트 콜라이더 구성 재개

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_구성

**"taskContent"**: Work_5에서 중단된 업무 2를 절차 3(스프라이트 실측 대조)부터 재개 — 익스포트·재임포트·플레이 실측

**업무**

- 근거: `_Data/Job/Job_003/Work_5/result.md` — 프리팹 8건 콜라이더 patch는 완료(플레이어 캡슐 (0, 0.45), Apple (0, 0.40), Banana·Watermelon (0, 0.45), 보스 (0, 0.80), `Object_Floor` (0, −1.35)). Work_5_1이 텍스처 Read/Write를 허용했으므로 절차 3 실측이 가능하다
- 실측 결과 콜라이더가 잉크 접지선과 어긋나면 정본(`_Data/Job/Job_003/Work_1/result.md`: 발 콜라이더 하단 = 피벗 − 0.05u, 바닥 콜라이더 상단 = 시각 상단 − 0.05u)대로 보정한다
- `게임개발_프리셋_파일_익스포트` → 재임포트 → 플레이 실측: 스폰 직후 `FlyState` 접지·`Idle`/`Move`(Gun `Idle_Gun`/`Move_Gun`) 재생, 적·보스 발이 바닥선에 닿음, 콘솔 에러 0
- 완료 기준: 익스포트 success, 플레이 실측 합격, 두 씬 `isDirty=false`. 사용자에게 질문하지 않으며 `confirmed`·`reuse` 값은 변경하지 않는다. DataMCP 무응답 시 `Fallback` 순서를 따른다
