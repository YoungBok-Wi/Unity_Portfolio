# 업무지시서

## 1. Banana 임포트 보정 후보 리사이즈

**대상 스킬**: 파일_이미지_리사이즈_실행

**"imagePath"**: `_Temp/Job_008_Work_5_4_Banana_Move_candidate.png`

**업무**

- `Work_5_2`의 선택 원본 `_Data/Resource/File/AnimationSheet_Casual_Enemy/Banana_Move/frame_01/3.png`을 임시 경로에 복제해 입력으로 사용한다.
- `image_normalize`만 사용해 투명 `256x256` 캔버스와 비율을 유지하면서 알파 바운딩 박스 높이 `124px`인 보정 후보를 만든다.
- 원본 pool·Asset·`.meta`는 수정하지 않는다.
- 완료 기준은 임시 후보가 PNG `256x256`, 알파 바운딩 박스 높이 `124px`이고 투명 배경을 유지하는 것이다.
