# 업무지시서

## 1. 오브젝트 최종 재검증

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_구성

**"prefabId"**: `Object_Player_Knife`, `Object_Player_Gun`, `Object_Enemy_Apple`, `Object_Enemy_Watermelon`, `Object_Enemy_Banana`, `Object_Boss_Pumpkin`, `Object_Boss_Pineapple`

**업무**

- `Work_5_3`에서 정상 판정한 구조·물리·아이콘·FSM·클립 배선을 재사용하고 `Work_5_5` 반영 뒤 스프라이트 실측만 갱신한다.
- 7종 원본 프리팹의 Unity 로드, Missing 스크립트·오브젝트 참조, `SpriteAnimPlayer` 159프레임을 전수 검사한다.
- Move 불투명 높이 계층 `Apple 113px < Banana 123px < Watermelon 138px`과 접지 행 `184`를 확인한다.
- 완료 기준은 7종 로드 성공, Missing 스크립트·참조 `0`, Banana Move Unity 실측 `123px`다.

## 2. 프리셋 익스포트·재임포트

**대상 스킬**: 게임개발_프리셋_파일_익스포트

**"prefabType"**: `Popup`, `Control`, `Object`

**업무**

- `Work_5`에서 구성한 `Popup` 7종·변경 대상 `Control`·`Object` 7종 원본을 타입별 export 사본에 반영한다.
- Unity 재임포트 뒤 export 사본의 Missing 스크립트·오브젝트 참조를 전수 검사한다.
- `confirmed`·`reuse`를 변경하지 않고 `Assets/_Library/**`·`_Data/Module/Library/**`를 수정하지 않는다.
- 완료 기준은 타입별 export 성공, 재임포트 완료, Missing 스크립트·참조 `0`, 콘솔 오류 `0건`이다.
