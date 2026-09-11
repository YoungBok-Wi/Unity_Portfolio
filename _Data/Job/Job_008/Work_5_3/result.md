# [게임개발_프리셋_파일_오브젝트_구성] "Work_5 오브젝트 최종 검증·익스포트 완료" 업무 레포트

## 요약

- 오브젝트 7종은 Unity 로드 성공, Missing 스크립트 `0건`, Missing 오브젝트 참조 `0건`이다.
- `SpriteAnimPlayer` 159프레임을 Unity 임포트 결과로 실측했으며 Banana Move가 요구 높이 `123px`보다 1px 작은 `122px`여서 최종 검증에 실패했다.
- 업무 1 실패로 업무 2 `Popup`·`Control`·`Object` 익스포트는 수행하지 않았다.

## 완료업무

### 오브젝트 배선과 프레임 전수 검사
**산출물**
`Assets/__Game/_Core/_Object`
**작업내용**
- 플레이어 2종·적 3종·보스 2종의 물리·FSM·아이콘·`SpriteAnimPlayer` 배선을 `prefab_object get`으로 확인했다.
- Unity 직렬화 전수 검사는 7종 모두 `loaded:true`·`missingScripts:0`·`missingReferences:0`, 클립 프레임 합계 `159`다.
- Move 프레임 불투명 높이는 Apple `113px`, Banana `122px`, Watermelon `138px`다.

## 비고

- 대상 — 업무 2 `게임개발_프리셋_파일_익스포트`; 조건 — 업무 1의 필수 규격 불합격 시 워커 예외 처리에서 남은 업무를 중단; 실측 근거 — Unity `GetPixels`의 `alpha > 0.01` 판정이 Banana Move `122px`다.

## 예외상황

- 대상 — `Assets/__Game/_Core/SpriteAnim/AnimationSheet_Casual_Enemy_Banana_Move_01.png`; 막힌 지점 — 업무 1 스프라이트 실측 대조; 에러 원문 — `Object_Enemy_Banana|clips=Move:1:122-122`; 필요한 처리 — Unity 임포트 후 판정 높이를 `123px`로 보정한 뒤 오브젝트 검증과 프리셋 익스포트를 다시 수행해야 한다.
