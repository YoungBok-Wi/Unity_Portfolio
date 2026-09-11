# [게임개발_구성_리소스_파일_GPT_애니메이션시트_제작] "Banana Move 기준 프레임 규격 교정" 업무 레포트

## 요약

- `AnimationSheet_Casual_Enemy_Banana_Move_01.png`을 투명 `256x256` 프레임으로 교정하고 기존 슬롯 `frame_01`의 `3.png`로 선택·익스포트했다.
- Asset 알파 바운딩 박스는 `(67, 61, 209, 184)`로 높이 `123px`이며 `Apple 113px < Banana 123px < Watermelon 138px`과 공통 접지 행 `184`를 충족한다.
- Unity 임포트는 `Sprite`·PPU `128`·피벗 `(0.5, 0.28)`이고, Banana 프리팹의 기존 GUID 참조 3곳이 유지되며 재임포트 후 콘솔 오류는 `0건`이다.

## 완료업무

### 대상 리소스와 규격 확정
**산출물**
`_Data/Resource/File/AnimationSheet_Casual_Enemy/type.json`
**작업내용**
- `resource_type get`과 `resource_file get`으로 `AnimationSheet/AnimationSheet_Casual_Enemy/Banana_Move/frame_01`을 확인했다. `confirmed:false`·`reuse:add`는 변경하지 않았다.
- `_Data/Concept/Resource/concept.md` 및 이동 프레임 실측은 Apple `(73,71,183,184)`·Watermelon `(66,46,189,184)`로 각각 높이 `113px`·`138px`, 접지 행 `184`다.

### 투명 프레임 교정
**산출물**
`_Data/Resource/File/AnimationSheet_Casual_Enemy/Banana_Move/frame_01/3.png`
**작업내용**
- 내장 이미지 생성 결과의 알파 배경을 확인하고 `image_normalize` `Work_0022`로 캔버스·크기를 보정한 뒤 프레임 정렬로 접지 행을 맞췄다.
- 최종 PNG 실측은 캔버스 `256x256`, 알파 바운딩 박스 `(67,61,209,184)`, 불투명 높이 `123px`다.

### pool 선택과 Asset 익스포트
**산출물**
`Assets/__Game/_Core/SpriteAnim/AnimationSheet_Casual_Enemy_Banana_Move_01.png`
**작업내용**
- 기존 `1.png`·`2.png`는 보존하고 새 원본 `3.png`를 선택했다. `resource_file get` 응답의 `outputs.frame_01.select`는 `3.png`다.
- 원본과 Asset의 MD5는 모두 `6D65B845D7D97DCDEDE2EB239D901F45`이고 `.meta` GUID `a89913cb0db86dd43b05835983574018`이 유지됐다.
- Unity 로드 실측은 `256x256`, PPU `128`, 피벗 `(128,71.68)`이며 Banana 프리팹의 GUID 참조는 3곳, 재임포트 후 콘솔 오류는 `0건`이다.

## 비고

- 캐릭터 형태는 투명 배경 재생성 결과로 교체했으며, 사용자가 이번 항목에 한해 형태 차이를 허용했다.
