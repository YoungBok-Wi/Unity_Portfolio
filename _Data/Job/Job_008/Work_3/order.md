# 업무지시서

## 1. 리소스 현황·시트 방향 실측

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `AnimationSheet_Casual_Enemy`·`Boss`·`Player` 타입 정의(출력 슬롯 `resources`·leaf·idPrefix)와 파일별 산출 슬롯 경로(pool·select), `Icon_Casual_*` 노드 트리·타입 규격, `UI_Common_Shape` 타입 정의·슬롯, `Illust_Casual_Chef/Knife` 원본 경로

**업무**

- 추가 실측: 적 3종(Apple·Watermelon·Banana)·보스 2종 Move·Idle·Attack·Die 프레임 원본을 열어 바라보는 방향을 판정한다 (사용자 보고: 이동 중 좌향 → Move 프레임이 좌향으로 생성됨). 좌향 프레임 목록(파일 ID·슬롯)을 표로 남긴다
- 완료 기준: 타입 3종 슬롯 표, 좌향 프레임 목록, 신규 타입·파일의 소속 노드·규격 확정

## 2. 얼굴 아이콘 타입 생성

**대상 스킬**: 게임개발_구성_리소스_타입_생성

**"resourceCategory"**: 1에서 확인한 `Icon_Casual_*`의 소속 노드

**"resourceType"**: Icon_Casual_Face

**"content"**: 씬 전환 타일용 요리사 얼굴 아이콘 (256x256 투명 배경, `Illust_Casual_Chef` 머리 크롭)

**업무**

- 완료 기준: 등록 응답 `success:true`

## 3. 타입 구성 (SpriteAnim resources 해제·얼굴 타입 규격)

**대상 스킬**: 게임개발_구성_리소스_타입_구성

**"resourceCategory"**: 각 타입의 소속 노드

**"resourceType"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player, Icon_Casual_Face (대상만 달리해 4회)

**"content"**: 시트 3종 — 출력 슬롯 전부 `resources: false`(leaf `SpriteAnim` 유지 → `Assets/__Game/_Core/SpriteAnim/`), 설명의 "`Resources` 문자열 로드 통로" 문구를 "프리팹 인스펙터 프레임 배열 참조" 로 교체. `Icon_Casual_Face` — 출력 슬롯 `icon`(.png, 256x256, `resources: false`, idPrefix `Icon_Casual_Face_`, 가공 자동화 없음), 자동화 없음(반입 원본이 최종), 프롬프트 없음

**업무**

- 슬롯 `resources` 해제 뒤 익스포트 경로가 `Assets/__Game/_Core/SpriteAnim/`으로 바뀌는지 응답으로 확인하고, 기존 `Resources/SpriteAnim/` 사본 정리가 익스포트(8)에서 자동으로 되는지 여부를 기록한다 (안 되면 8에서 처리)
- `reuse`는 바꾸지 않는다 (신규 타입은 생성 초기값만)
- 완료 기준: 4건 verify `success:true`

## 4. 타입 기본값 업로드

**대상 스킬**: 게임개발_구성_리소스_타입_업로드

**"resourceTarget"**: Icon_Casual_Face

**"sourceFiles"**: (기본값 불필요)

**업무**

- 스킬 조건대로 기본값 파일이 필요 없으면 건너뛰고 사유를 남긴다 (얼굴 파일 1건이 곧 산출물)

## 5. 파일 entry 생성

**대상 스킬**: 게임개발_구성_리소스_파일_생성

**"resourceCategory"**: 각 타입의 소속 노드

**"resourceType"**: Icon_Casual_Face, UI_Common_Shape (대상만 달리해 2회)

**"resourceId"**: Chef, Vignette

**"content"**: `Icon_Casual_Face/Chef` — 요리사 얼굴 타일. `UI_Common_Shape/Vignette` — 저체력 경고용 외곽 비네트

**업무**

- 완료 기준: 2건 등록 `success:true`

## 6. 파일 구성

**대상 스킬**: 게임개발_구성_리소스_파일_구성

**"resourceCategory"**: 각 타입의 소속 노드

**"resourceType"**: Icon_Casual_Face, UI_Common_Shape (대상만 달리해 2회)

**"resourceId"**: Chef, Vignette

**"content"**: 설명·`inAsset` true, 프롬프트 없음(코드 합성 반입), 산출 선택은 7 업로드 뒤 슬롯 select

**업무**

- 완료 기준: 2건 verify `success:true` (select는 7 뒤 확정)

## 7. 산출 파일 합성·업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"sourceFiles"**: (a) 1에서 확정한 좌향 프레임의 수평 반전본 (b) `Icon_Casual_Face/Chef` 256x256 (c) `UI_Common_Shape/Vignette` 512x512

**업무**

- 합성은 `python -X utf8` + Pillow(설치돼 있으면)로 `_Temp/Work_3_J8/`에 만든다. Pillow가 없으면 표준 라이브러리로 PNG를 직접 쓰는 최소 코드로 대체한다
- (a) 반전: 원본 pool 파일을 열어 좌우 반전만 하고 캔버스·피벗 규격은 유지, 같은 슬롯에 반입해 select를 반전본으로 옮긴다 (원본은 pool에 남긴다)
- (b) 얼굴: `Illust_Casual_Chef_Knife.png`(640x960)에서 모자 상단~턱 아래(대략 x 30~480, y 50~610, 칼은 제외)를 정사각으로 잘라 256x256으로 축소, 투명 배경 유지
- (c) 비네트: 512x512, 중심에서 반지름 비례 알파 0 → 외곽 1(백색), 감마 완만(alpha = clamp((r-0.35)/0.65)^1.5)
- 완료 기준: 업로드 응답 `success:true`, 각 파일 select 확정
- `confirmed` 무변경 — 반전본 select 변경으로 컨펌 이력을 건드리지 않는다 (confirmed 항목은 조회만)

## 8. 파일 익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"resourceTarget"**: AnimationSheet_Casual_Enemy, AnimationSheet_Casual_Boss, AnimationSheet_Casual_Player, Icon_Casual_Face, UI_Common_Shape (타입 단위 5회)

**업무**

- 완료 기준: `Assets/__Game/_Core/SpriteAnim/`에 시트 3종 전 프레임 존재, `Assets/__Game/_Core/Resources/SpriteAnim/` 잔존 0건(자동 정리가 안 되면 사본 삭제 후 재임포트), `Icon_Casual_Face_Chef.png`·`UI_Common_Shape_Vignette.png` 임포트 완료(Sprite 타입)
- 익스포트 동기 블로킹 이력(약 16분, `Job_004/Work_3_1/result.md`) — 타임아웃 시 사본 실측으로 확인
- 라이브러리(`Assets/_Library/**`) 수정 금지. DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
