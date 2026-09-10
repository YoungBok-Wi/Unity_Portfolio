# 업무지시서

## 1. 대상 리소스·정본 조회

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `AnimationSheet_Casual_Enemy_Banana_Move_01` 파일 entry·출력 슬롯·원본·선택 pool·assetPath와 `리소스컨셉`의 적 기준 높이·방향·캔버스·피벗 규격

**업무**

- `Work_5`에서 확인한 불투명 높이 109px와 정본 123px의 차이를 재확인한다.
- 기존 파일 entry와 출력 슬롯이 있으므로 파일 생성·구성 스킬은 제외하고 `confirmed`·`reuse`는 변경하지 않는다.
- 완료 기준은 수정할 원본·pool 슬롯·export 경로와 보존할 규격이 확정되는 것이다.

## 2. Banana Move 기준 프레임 교정

**대상 스킬**: 게임개발_구성_리소스_파일_GPT_애니메이션시트_제작

**"resourceTarget"**: `AnimationSheet_Casual_Enemy_Banana_Move_01`

**업무**

- 목표는 투명 256x256 캔버스에서 불투명 픽셀 높이를 정확히 123px로 교정하는 것이다.
- 기존 오른쪽 방향·지면 기준 피벗·도트 스타일·색·실루엣을 보존하고 다른 프레임이나 리소스는 수정하지 않는다.
- 완료 기준은 산출 PNG 실측이 256x256, 불투명 높이 123px이며 `Apple` 113px < `Banana` 123px < `Watermelon` 138px 계층을 만족하는 것이다.

## 3. 교정본 업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"sourceFile"**: 2의 검수 완료 PNG

**업무**

- 교정본을 기존 `AnimationSheet_Casual_Enemy_Banana_Move_01` 대응 pool 슬롯에 반입한다.
- 기존 원본은 pool에 보존하고 새 업로드 키를 해당 파일의 선택 산출물로 연결한다.
- 완료 기준은 업로드 성공과 선택 pool 키 확인이다.

## 4. 리소스 익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"resourceTarget"**: `AnimationSheet_Casual_Enemy_Banana_Move_01`

**업무**

- 선택·확정된 교정본을 기존 assetPath로 익스포트하고 Unity 임포트를 완료한다.
- 기존 `.meta` GUID·PPU 128·Sprite 임포트 설정을 보존한다.
- 완료 기준은 export 성공, assetPath 실측 불투명 높이 123px, Missing 참조 0이다.
