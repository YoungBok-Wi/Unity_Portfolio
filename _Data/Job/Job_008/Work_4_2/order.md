# 업무지시서

## 1. AnimationSheet 출력 슬롯 복구·전환

**대상 스킬**: 게임개발_구성_리소스_타입_구성

**"resourceType"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player

**업무**

- 실패한 `Work_4_1`에서 초기화된 출력 슬롯 6개의 전체 값을 변경 전 조회값으로 복구하면서 `resources:false`·`leaf:SpriteAnim`을 적용한다.
- 이번 결함에 한해 사용자가 승인한 전체 출력 슬롯 명시 patch를 사용한다. 다른 절차 결함에는 적용하지 않는다.
- `suffix`는 `_01`~`_06`, `ext`는 `.png`, `idPrefix`는 타입별 기존 접두어, `isPreview`는 `frame_01`만 true로 복구하고 가공 설정을 보존한다.
- 설명을 프리팹 인스펙터 `Sprite[]` 프레임 배열 참조 방식으로 갱신하고 `reuse`·`confirmed`는 변경하지 않는다.
- 완료 기준은 3종 get·list 검증에서 슬롯 전체 값과 파일 수가 일치하는 것이다.

## 2. 얼굴·비네트 리소스 타입 확보

**대상 스킬**: 게임개발_구성_리소스_타입_생성, 게임개발_구성_리소스_타입_구성

**"resourceType"**: Icon_Casual_Face, UI_Common_Gradient

**업무**

- 두 타입이 없으면 생성하고, 있으면 기존 타입을 재사용한다.
- 얼굴 아이콘은 256x256 PNG, 비네트는 512x512 PNG가 `Assets`에 반출되도록 출력 슬롯과 설명을 구성한다.
- 프롬프트·자동 참조는 두지 않고 `reuse`·`confirmed`는 변경하지 않는다.
- 완료 기준은 두 타입 get 검증 성공과 출력 경로 확정이다.

## 3. 리소스 파일 entry 생성·구성

**대상 스킬**: 게임개발_구성_리소스_파일_생성, 게임개발_구성_리소스_파일_구성

**"resourceId"**: Icon_Casual_Face/Chef, UI_Common_Gradient/Vignette

**업무**

- 두 파일을 `inAsset:true`로 등록하고 용도를 설명한다.
- 프롬프트·자동 참조는 두지 않고 업로드 뒤 실제 pool 키를 select로 지정한다.
- `reuse`·`confirmed`는 변경하지 않고 두 파일 verify `success:true`를 확인한다.

## 4. 산출 파일 합성·업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"sourceFiles"**: 좌향 프레임 반전본, Icon_Casual_Face/Chef 256x256, UI_Common_Gradient/Vignette 512x512

**업무**

- `Work_3_1/result.md`의 좌향 프레임 25장을 수평 반전해 같은 슬롯에 업로드하고 원본은 pool에 보존한다.
- 얼굴 아이콘은 `Illust_Casual_Chef/Knife` 원본의 모자 상단부터 턱 아래까지 정사각 크롭한다.
- 비네트는 512x512 백색이며 중심 알파 0에서 외곽 1로 `alpha=clamp((r-0.35)/0.65)^1.5`를 적용한다.
- 완료 기준은 대상 슬롯 업로드 성공과 실제 pool 키 선택이다.

## 5. 리소스 익스포트·GUID 복구·검증

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트, 유니티엔진_재임포트_실행, 유니티엔진_에셋_검증

**"scope"**: AnimationSheet 3종, Icon_Casual_Face/Chef, UI_Common_Gradient/Vignette

**업무**

- 5개 타입을 익스포트하고 `_Temp/Work_4_1_meta`의 이동 전 메타 147건을 새 SpriteAnim 경로의 동명 파일에 복원한 뒤 Refresh한다.
- `Assets/__Game/_Core/SpriteAnim`의 AnimationSheet 프레임 전건과 `Assets/__Game/_Core/Resources/SpriteAnim` 잔여 0건을 확인한다.
- 얼굴·비네트의 Sprite 임포트 설정과 새 경로를 확인하고 3개 AnimationSheet의 이동 전후 GUID 일치를 검증한다.
- `Assets/_Library/**`와 `_Data/Module/Library/**`는 수정하지 않는다.
