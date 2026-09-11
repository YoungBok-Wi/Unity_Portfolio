# [게임개발_구성_리소스_타입_구성] "Icon_Casual_Face 실제 규격 정정" 업무 레포트

## 요약
- `Icon_Casual_Face`의 설명과 `fillRatio` 수정 호출은 성공했지만, 문서상 부분 병합인 `resource_type patch`가 미지정 출력 슬롯 필드를 기본값으로 초기화했습니다.
- 현재 타입의 `outputs.icon`에서 `ext·leaf·idPrefix·isPreview`와 `width·height`가 손상됐고, 익스포트 에셋과 `.meta`도 삭제됐으며 우회 복구 금지 규칙에 따라 작업을 중단했습니다.
- pool 원본 `icon/1.png`는 남아 있고 `confirmed·reuse`는 변경하지 않았으며, 후속 익스포트는 수행하지 않았습니다.

## 완료업무

### 얼굴 리소스 현황 조회
**산출물**
`_Data/Resource/File/Icon_Casual_Face/Chef/file.json`
`Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png`
**작업내용**
- `Chef`가 `inAsset`이고 프로젝트 에셋 경로에 존재함을 확인했습니다.
- 타입 정본 변경 전 값은 기준 높이 `232px`, `fillRatio=0.90625`이며 컨셉 실측은 `252px·0.984375`임을 확인했습니다.

## 예외상황
- 대상 — `resource_type patch`, `Icon/Icon_Casual_Face.outputs.icon`. 막힌 지점 — 설명과 `outputs.icon.processLiteralValues.options.fillRatio`만 전달한 부분 수정 직후 재조회. 에러 원문 — 호출 응답은 `{"success":true}`였지만 재조회 값이 `ext:""`, `isPreview:false`, `leaf:""`, `idPrefix:""`로 바뀌고 `width·height`가 사라졌습니다. 이어서 `Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png`와 `.meta`도 삭제됐습니다. 문서의 "patch는 부분 병합이라 담지 않은 필드는 보존된다"와 실제 동작이 불일치합니다.
- 복구 필요 — pool 원본 `_Data/Resource/File/Icon_Casual_Face/Chef/icon/1.png`를 보존한 채 변경 전 `ext:.png`, `isPreview:true`, `leaf:Icon`, `idPrefix:Icon_Casual_Face_`, `width:256`, `height:256`과 수정값 `fillRatio:0.984375`, 기준 높이 `252px`를 함께 복구하고 에셋·`.meta`를 재익스포트해야 합니다. 절차 결함 우회가 금지되어 사용자 처리 방향 확인 전에는 수정·익스포트할 수 없습니다.
