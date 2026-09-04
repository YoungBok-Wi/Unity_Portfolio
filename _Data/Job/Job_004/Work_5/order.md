# 업무지시서

## 1. 프리셋 현황 조회

**대상 스킬**: 게임개발_프리셋_파일_질문

**"taskContent"**: `Object_Player_Knife`·`Object_Player_Gun`·`Object_Enemy_*`·`Object_Boss_*`·`Object_Projectile` `SpriteRenderer` 정렬 값, `Popup_Setting`·`Popup_Lobby` 취소 플래그(`m_IsCloseByCancel`)·스크립트 구조, `Popup_Notify`(라이브러리) 취소 플래그 조회

**업무**

- 근거: `_Data/Job/Job_004/job.md` "프리셋" 확인 결과(정렬 순서 전부 0, `Popup_Setting` 0), `_Data/Job/Job_003/Work_7/result.md` `## 비고` 결함 ②③, `씬설정 Scene_Lobby` "취소 입력"

## 2. 로비 취소 입력 코드 수정

**대상 스킬**: 게임개발_프리셋_파일_팝업_코드_작성

**"taskContent"**: 로비에서 팝업이 열려 있을 때 취소 입력이 최상단 팝업을 닫고 `Popup_Quit`를 열지 않도록 수정

**업무**

- 원인에 맞는 최소 수정을 고른다: (가) `Popup_Setting` 취소 닫기 허용만으로 정본("열린 팝업이 있으면 그 팝업이 닫힌다")이 충족되면 코드 수정 없이 업무 3의 구성으로 처리 (나) 라이브러리 `Popup_Notify`가 취소를 소비하지 않아 Notify 열림 중 Quit가 열리면 `Popup_Lobby` 스크립트(게임 소속)에서 `OnInputCancel` override로 최상단 열린 팝업을 닫아 소비한다. 라이브러리 수정 금지
- 완료 기준: Setting→Apply Notify 열림 상태 `escape` 1회에 최상단 팝업 1개 닫힘·`Popup_Quit opened=false`, 팝업 없음 상태 `escape`는 `Popup_Quit` 열림 유지
- 후속 `유니티엔진_컴파일_실행` 통과

## 3. 오브젝트·팝업 구성

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_구성

**"taskContent"**: 플레이어 프리팹 정렬 순서 > 적·보스(Work_1 `리소스컨셉` 규격값), `Popup_Setting` 취소 닫기 플래그(업무 2 결정에 따름) — 팝업은 `게임개발_프리셋_파일_팝업_구성`으로

**업무**

- 대상 오브젝트: `Object_Player_Knife`·`Object_Player_Gun`(플레이어 값), `Object_Enemy_Apple`·`Object_Enemy_Watermelon`·`Object_Enemy_Banana`·`Object_Boss_Pumpkin`·`Object_Boss_Pineapple`(적·보스 값), `Object_Projectile`·전조·히트 이펙트는 규격에 적힌 값만 적용
- 완료 기준: 프리팹 YAML `m_SortingOrder` 실측이 규격과 일치, 플레이 캡처에서 Apple 3마리 접촉 중 플레이어 전신 노출
- 절차 3 스프라이트 실측이 비-Readable 텍스처로 막히면 Job_003 Work_5_1과 같이 `isReadable` 전제를 확인하고 사유를 보고한다

## 4. 익스포트

**대상 스킬**: 게임개발_프리셋_파일_익스포트

**"taskContent"**: 수정 프리팹 원본 익스포트·재임포트

**업무**

- 완료 기준: export verify 통과, 플레이 실측으로 업무 2·3 완료 기준 확인(플레이 종료 `stopped`·`Scene_Lobby isDirty:false`)
- 제외 스킬: 프리셋 노드 생성·구성, 컨트롤·애드온 구성, `게임개발_프리셋_파일_오브젝트_코드_작성`(오브젝트 스크립트는 `Battle` 모듈 소속 — Work_4 담당), `유니티엔진_씬_셋업_실행`(오버라이드 소멸)
- `confirmed`·`reuse` 무변경. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
