# 업무지시서

## 1. 해금 완료 문구 ID 신설

**대상 스킬**: 게임개발_구성_데이터_테이블_작성

**"taskContent"**: `Text_Core_GunUnlocked`(해금 완료 알림·결과 라벨용) 행 추가

**업무**

- 근거: `_Data/Job/Job_002/Work_7/result.md` `## 비고` "[데이터] 해금 알림·결과 라벨 문구" — 현재 `Text_Core_GunUnlock`(조건 문구 `{0}`) 한 ID를 조건·알림·결과에 공유
- `Text` `Core` 시트에 `Text_Core_GunUnlocked`(한국어·영어, 예: "크림 건 해금!" / "Cream Gun unlocked!") 행을 추가한다. Work_3(모듈)·Work_5(프리셋)가 이 ID를 참조한다

## 2. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"taskContent"**: 전 종류 익스포트 — `Quit` 모듈 활성(Work_3) 후 `Text_Quit_*` 포함 반영

**업무**

- 익스포트 후 `TableText.json`에 `Text_Core_GunUnlocked`·`Text_Quit_Title`·`Text_Quit_Text` 실재를 확인하고 컴파일 에러 0을 확인한다. `Text_Quit_*`가 여전히 제외되면 제외 조건을 실측해 보고한다
- `confirmed`·`reuse` 값은 변경하지 않으며 DataMCP 무응답 시 `Fallback` 순서를 따른다
