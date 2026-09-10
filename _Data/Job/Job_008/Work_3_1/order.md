# 업무지시서

## 1. 리소스 현황·시트 방향 실측

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: 애니메이션시트 3종·신규 타입 2종·대상 파일의 타입 정의, 출력 슬롯, pool·select, 에셋 경로와 프레임 방향

**업무**

- `AnimationSheet_Casual_Enemy`·`Boss`·`Player`의 `resources`·`leaf`·`idPrefix`와 파일별 pool·select를 조회한다.
- Apple·Watermelon·Banana와 보스 2종의 Move·Idle·Attack·Die 원본 프레임을 열어 좌향 파일 ID·슬롯을 확정한다.
- `Icon_Casual_Face`·`UI_Common_Gradient` 미등록 상태와 `Illust_Casual_Chef/Knife` 원본 경로를 확인한다.

## 2. 리소스 타입 생성

**대상 스킬**: 게임개발_구성_리소스_타입_생성

**"resourceType"**: Icon_Casual_Face, UI_Common_Gradient

**업무**

- `Icon_Casual_Face`는 `Icon/Icon_Casual`에 씬 전환용 256x256 투명 얼굴 아이콘 타입으로 등록한다.
- `UI_Common_Gradient`는 `UI/UI_Common`에 저체력 외곽 비네트용 512x512 투명 그라디언트 타입으로 등록한다.
- 두 등록 응답의 `success:true`를 확인한다.

## 3. 리소스 타입 구성

**대상 스킬**: 게임개발_구성_리소스_타입_구성

**"resourceType"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player, Icon_Casual_Face, UI_Common_Gradient

**업무**

- 애니메이션시트 3종은 출력 슬롯 전부 `resources:false`·`leaf:SpriteAnim`을 유지하고 설명을 프리팹 인스펙터 프레임 배열 참조 방식으로 갱신한다.
- `Icon_Casual_Face`는 PNG `icon` 슬롯, 256x256, `resources:false`, `idPrefix:Icon_Casual_Face_`, 자동화·프롬프트 없음으로 구성한다.
- `UI_Common_Gradient`는 PNG `image` 슬롯, 512x512, `resources:false`, `leaf:Image`, `idPrefix:UI_Common_Gradient_`, 자동화·프롬프트 없음으로 구성한다.
- `reuse`·`confirmed`는 변경하지 않고 5종 verify `success:true`를 확인한다.

## 4. 타입 기본값 확인

**대상 스킬**: 게임개발_구성_리소스_타입_업로드

**"resourceTarget"**: Icon_Casual_Face, UI_Common_Gradient

**업무**

- 기본값 파일이 필요 없으면 스킬 조건대로 건너뛰고 대상·조건·실측 근거를 보고한다.

## 5. 리소스 파일 entry 생성

**대상 스킬**: 게임개발_구성_리소스_파일_생성

**"resourceId"**: Icon_Casual_Face/Chef, UI_Common_Gradient/Vignette

**업무**

- 얼굴 타일과 저체력 외곽 비네트 파일 entry를 등록하고 두 응답의 `success:true`를 확인한다.

## 6. 리소스 파일 구성

**대상 스킬**: 게임개발_구성_리소스_파일_구성

**"resourceId"**: Icon_Casual_Face/Chef, UI_Common_Gradient/Vignette

**업무**

- 설명과 `inAsset:true`를 반영한다. 프롬프트·자동 참조는 두지 않고 업로드 뒤 pool 키를 select로 지정한다.
- `reuse`·`confirmed`는 변경하지 않는다.

## 7. 산출 파일 합성·업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"sourceFiles"**: 좌향 애니메이션 프레임 반전본, Icon_Casual_Face/Chef 256x256, UI_Common_Gradient/Vignette 512x512

**업무**

- `_Temp/Work_3_1_J8/`에 Pillow로 좌우 반전·얼굴 크롭·비네트 합성본을 만든다. Pillow 미설치 시 스킬 허용 범위의 표준 라이브러리 PNG 경로를 쓴다.
- 좌향 프레임은 원본 pool을 수평 반전하고 같은 슬롯에 업로드한다. 얼굴은 `Illust_Casual_Chef_Knife.png`의 모자 상단~턱 아래를 정사각 크롭해 256x256으로 만든다.
- 비네트는 512x512 백색이며 중심 알파 0에서 외곽 1로 `alpha=clamp((r-0.35)/0.65)^1.5`를 적용한다.
- 업로드 뒤 새 pool 키를 조회해 select로 지정하고 `confirmed`는 변경하지 않는다.

## 8. 리소스 파일 익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"resourceTarget"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player, Icon_Casual_Face, UI_Common_Gradient

**업무**

- 5개 타입을 각각 익스포트한다. `Assets/__Game/_Core/SpriteAnim/`에 시트 프레임 전건이 있고 `Assets/__Game/_Core/Resources/SpriteAnim/` 잔존이 0건인지 확인한다.
- `Icon_Casual_Face_Chef.png`와 `UI_Common_Gradient_Vignette.png`의 에셋 경로·Sprite 임포트를 확인한다.
- `Assets/_Library/**`는 수정하지 않는다. 원본 `Work_3`의 미수행 범위를 이 Work에서 완결한다.
