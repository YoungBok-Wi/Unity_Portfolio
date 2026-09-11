# [게임개발_구성_리소스_타입_구성] "Icon_Casual_Face 실제 규격 정정" 업무 레포트

## 요약
- `Icon_Casual_Face` 타입의 기준 높이를 `252px`, `fillRatio`를 `0.984375`로 정정했습니다.
- 전체 `get` 구성값에 수정값을 합친 `resource_type patch`로 출력 슬롯을 복구했고, 에셋과 기존 GUID도 복구됐습니다.
- Unity 재임포트가 성공했으며 `[Global].prefab`의 Face Sprite 참조와 `.meta` GUID가 `9b7955a211788da4a90792cebbf2e24b`로 일치합니다.

## 완료업무

### 얼굴 리소스 현황 조회
**산출물**
`_Data/Resource/File/Icon_Casual_Face/Chef/file.json`
`Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png`
**작업내용**
- `Chef`가 `inAsset`이고 프로젝트 에셋 경로에 존재함을 확인했습니다.
- 타입 정본 변경 전 값은 기준 높이 `232px`, `fillRatio=0.90625`이며 컨셉 실측은 `252px·0.984375`임을 확인했습니다.

### 얼굴 리소스 타입 정정
**산출물**
`_Data/Resource/File/Icon_Casual_Face/type.json`
**작업내용**
- 타입 설명의 기준 높이를 `252px`, 출력 슬롯의 `fillRatio`를 `0.984375`로 정정했습니다.
- 출력 슬롯은 `ext:.png`, `isPreview:true`, `resources:false`, `leaf:Icon`, `idPrefix:Icon_Casual_Face_`, `width:256`, `height:256`을 유지합니다.

### 에셋과 GUID 복구 검증
**산출물**
`Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png`
`Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png.meta`
**작업내용**
- 올바른 전체 출력 슬롯 구성을 반영하면서 에셋과 `.meta`가 다시 생성됐습니다.
- Unity 재임포트가 성공했고 `[Global].prefab`의 Face Sprite 참조와 `.meta` GUID가 일치합니다.

## 비고
- 이번 세션에 한해 사용자 승인에 따라 `resource_type patch`에 `get` 전체 구성값과 수정값을 함께 전달했습니다.
- `confirmed`와 `reuse`는 변경하지 않았습니다.
