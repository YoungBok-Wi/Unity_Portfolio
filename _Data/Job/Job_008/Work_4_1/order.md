# 업무지시서

## 1. SpriteAnim 문자열 로드 통로 재검증

**대상 스킬**: 유니티엔진_에셋_검증

**"scope"**: `Assets/__Game/_Core/Resources/SpriteAnim/` 전건

**"items"**: 로드

**업무**

- 선행 `Work_4` 완료 뒤 `SpriteAnimPlayer`·`RoomUtil`과 `Assets/__Game/**`의 SpriteAnim `Resources.Load` 통로가 0건인지 확인한다.
- 문자열 로드 대상이 남으면 AnimationSheet `resources`를 변경하지 않고 예외 대상을 보고한다.
- 완료 기준은 `Resources.Load` 통로 0건과 인스펙터 `Sprite[]` 참조 구조 확인이다.

## 2. AnimationSheet 타입 구성

**대상 스킬**: 게임개발_구성_리소스_타입_구성

**"resourceType"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player

**업무**

- 출력 슬롯 전부를 `resources:false`·`leaf:SpriteAnim`으로 바꾸고 설명을 프리팹 인스펙터 프레임 배열 참조 방식으로 갱신한다.
- 경로 이동 전후 Sprite `.meta` GUID를 보존하고 `reuse`·`confirmed`는 변경하지 않는다.
- 완료 기준은 3종 verify `success:true`와 기존 프리팹 Sprite 참조 유지다.

## 3. 리소스 파일 entry 생성·구성

**대상 스킬**: 게임개발_구성_리소스_파일_생성, 게임개발_구성_리소스_파일_구성

**"resourceId"**: Icon_Casual_Face/Chef, UI_Common_Gradient/Vignette

**업무**

- `Icon_Casual_Face/Chef`와 `UI_Common_Gradient/Vignette`를 `inAsset:true`로 등록하고 용도를 설명한다.
- 프롬프트·자동 참조는 두지 않고 업로드 뒤 실제 pool 키를 select로 지정한다.
- `reuse`·`confirmed`는 변경하지 않고 두 파일 verify `success:true`를 확인한다.

## 4. 산출 파일 합성·업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"sourceFiles"**: 좌향 프레임 반전본, `Icon_Casual_Face/Chef` 256x256, `UI_Common_Gradient/Vignette` 512x512

**업무**

- `Work_3_1/result.md`의 좌향 프레임 25장을 수평 반전해 같은 슬롯에 업로드하고 원본은 pool에 보존한다.
- 얼굴 아이콘은 `Illust_Casual_Chef/Knife` 원본의 모자 상단부터 턱 아래까지 정사각 크롭해 256x256 투명 PNG로 만든다.
- 비네트는 512x512 백색이며 중심 알파 0에서 외곽 1로 `alpha=clamp((r-0.35)/0.65)^1.5`를 적용한다.
- 완료 기준은 전 대상 업로드 `success:true`와 새 pool 키 select 확정이다.

## 5. 리소스 파일 익스포트·임포트 검증

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트, 유니티엔진_에셋_검증

**"resourceTarget"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player, Icon_Casual_Face, UI_Common_Gradient

**업무**

- 5개 타입을 각각 익스포트하고 Unity 재임포트를 완료한다.
- `Assets/__Game/_Core/SpriteAnim/`에 AnimationSheet 프레임 전건이 있고 `Assets/__Game/_Core/Resources/SpriteAnim/` 잔존이 0건인지 확인한다.
- `Icon_Casual_Face_Chef.png`·`UI_Common_Gradient_Vignette.png`의 경로와 Sprite 임포트 설정을 확인한다.
- `Assets/_Library/**`는 수정하지 않는다.
