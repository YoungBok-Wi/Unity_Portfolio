# 업무지시서

## 1. 오브젝트 최종 검증

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_구성

**"prefabId"**: Object_Player_Knife, Object_Player_Gun, Object_Enemy_Apple, Object_Enemy_Watermelon, Object_Enemy_Banana, Object_Boss_Pumpkin, Object_Boss_Pineapple

**업무**

- `Work_5`에서 완료한 7종 프리팹 구조·`SpriteAnimPlayer` 클립·아이콘·FSM·물리 참조를 최종 검증한다.
- `Work_5_2` 교정 뒤 `Banana Move_01` 불투명 높이 123px와 `Apple` 113px < `Banana` 123px < `Watermelon` 138px 계층을 확인한다.
- 선행 `Work_5`에서 질문·코드 작성·컴파일·팝업 구성·컨트롤 구성이 완료됐으므로 해당 체인 스킬은 제외하고 기존 산출물을 재사용한다.
- 완료 기준은 7종 원본 프리팹의 배선·프레임 GUID 정상, Missing 스크립트·참조 0이다.

## 2. 프리셋 익스포트·재임포트

**대상 스킬**: 게임개발_프리셋_파일_익스포트

**"prefabType"**: Popup, Control, Object

**업무**

- `Work_5`에서 구성한 `Popup` 7종·변경 대상 `Control`·`Object` 7종 원본을 각 타입의 export 사본에 반영한다.
- Unity 재임포트를 완료하고 export 사본의 Missing 스크립트·오브젝트 참조가 0인지 확인한다.
- `confirmed`·`reuse`를 변경하지 않고 `Assets/_Library/**`와 `_Data/Module/Library/**`를 수정하지 않는다.
- 완료 기준은 타입별 export 성공, 재임포트 완료, Missing 참조 0이다.
