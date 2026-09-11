# 업무지시서

## 1. Banana 프레임 임포트 기준 정렬

**대상 스킬**: 게임개발_구성_리소스_파일_GPT_애니메이션시트_제작

**"resourceTarget"**: `AnimationSheet/AnimationSheet_Casual_Enemy/Banana_Move/frame_01`

**업무**

- `Work_5_2`의 내장 이미지 생성 투명 프레임과 `Work_5_4`의 정규화 후보를 재사용해 프레임 검수·정렬을 잇는다.
- 알파 바운딩 박스 높이 `124px`를 유지한 채 접지 행을 Apple·Watermelon과 같은 `184`로 맞춘다.
- 기존 entry·프롬프트·`confirmed`·`reuse`는 변경하지 않으므로 질문·파일 생성·파일 구성 체인은 제외한다.
- 완료 기준은 투명 PNG `256x256`, 알파 바운딩 박스 높이 `124px`, 접지 행 `184`, 우향·기존 생성 형태 유지다.

## 2. 교정 후보 재업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"sourceFiles"**: 업무 1의 검수 완료 PNG를 `Banana_Move/frame_01`에 반입

**업무**

- 기존 pool `1.png`·`2.png`·`3.png`를 보존하고 새 업로드 키를 선택 산출물로 연결한다.
- 공용 타입의 `processAutomationId`·`processLiteralValues`와 `confirmed`·`reuse`는 변경하지 않는다.
- 완료 기준은 업로드 성공, 신규 pool 키와 `select` 일치 확인이다.

## 3. Unity 임포트 기준 익스포트 검증

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"resourceTarget"**: `AnimationSheet/AnimationSheet_Casual_Enemy/Banana_Move`

**업무**

- 선택 원본을 기존 Asset 경로로 익스포트하고 Unity 재임포트를 완료한다.
- `.meta` GUID·PPU `128`·피벗 `(0.5,0.28)`과 Banana 프리팹 참조를 보존한다.
- 완료 기준은 Unity `GetPixels`의 `alpha > 0.01` 불투명 높이 `123px`, 접지 행 `184`, Missing 참조 `0`, 콘솔 오류 `0건`이다.
