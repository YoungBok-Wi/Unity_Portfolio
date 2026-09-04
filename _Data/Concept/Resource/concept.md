---
name: 리소스컨셉
description: |
  Kitchen Riot의 비주얼·사운드·UI 방향을 정의하는 리소스 컨셉 문서
---
# Kitchen Riot - 리소스컨셉

## 이미지컨셉

### 전체 스타일
**테마 마디 Casual 단일**
   - 월드·UI·아이콘 전부 `Casual` 그림체 하나로 통일한다 — 둥근 벡터 외곽선·광택 하이라이트·플랫 채색
   - 리소스 노드는 기존 `{계열}_Casual_{분류}` 타입을 재사용하고, 사이드뷰 전용 산출물만 같은 테마 마디 아래 신규 타입으로 둔다
   - 크기 차이는 배치 스케일이 아니라 계열별 캔버스·기준 높이로 낸다 (캐릭터 시트·타일 PPU 128, 배경·UI·아이콘 PPU 100 — 배경 에셋 임포트 실측값 우선)

**재사용 대상 (등록 `inAsset` 127건 중 `Casual` 계열)**
   - 적 이동·사망 시트 `AnimationSheet_Casual_Enemy` (Apple·Banana·Watermelon — Orange는 미사용)
   - 플레이어 이동 시트 `AnimationSheet_Casual_Player/Move`
   - 무기·재화·강화 아이콘 `Icon_Casual_Weapon`(Knife·Gun)·`Icon_Casual_Currency/Crumb`·`Icon_Casual_Upgrade`(Attack·AttackSpeed·MaxHp·MoveSpeed·HealMacaron·MultiHit)
   - 전투 이펙트 `Illust_Casual_Hit/Impact`·`Illust_Casual_Slash/Knife`·`Illust_Casual_Projectile/Gun`·`Illust_Casual_Splatter/Death`·`Illust_Casual_Shadow/Ellipse`
   - 바닥 `Illust_Casual_Tile/Kitchen`, UI 전량(`UI_Casual_Button`·`Panel`·`Gauge`·`Slider`·`Toggle`·`Mark`), 서체 `Font_Casual_GyeonggiTitle_*`, `BGM_Casual`·`SFX_Casual_*`

**신규 제작 대상 (계열 단위)**
   - 플레이어 공격 동작 — `AnimationSheet_Casual_Player`에 Knife 3단·Gun 사격·점프 동작 추가
   - 보스 시트 — 신규 타입 `AnimationSheet_Casual_Boss` (Pumpkin·Pineapple 동작별)
   - 사이드뷰 배경 — 신규 타입 `Illust_Casual_Background` (로비 1종 + 방종류 4종)
   - 방종류 아이콘 — 신규 타입 `Icon_Casual_Room` (Battle·Heal·Ability·Boss + 로비 최고 순번 별 배지 `Best`, `게임컨셉` "최고 순번 배지" 정본 ID `Icon_Casual_Room/Best`)
   - 원거리 적 투사체 — `Illust_Casual_Projectile`에 Banana 껍질 투사체 추가
   - 게임 전체 컨셉아트 — `Concept_Resource` 1장 (화풍·팔레트 정본)
   - Gun 전용 대기·이동 — `AnimationSheet_Casual_Player`에 `Idle_Gun`·`Move_Gun` 추가 (케첩 건을 든 실루엣, 기존 `Idle`·`Move`는 Knife 전용으로 확정해 Gun이 공유하지 않는다)
   - 로비 중앙 요리사 — 신규 타입 `Illust_Casual_Chef` (Knife·Gun 2종, 선택 캐릭터에 따라 교체)
   - 사운드 — `BGM_Casual`(Lobby·Battle)·`SFX_Casual_Battle`(Attack·Hit·Die)·`SFX_Casual_Progress`(LevelUp·Unlock) 7건 업로드 (`BGM_Casual`은 타입 정의(`type.json`)가 없어 타입 등록이 선행 — 규격은 "사운드컨셉")

### 캐릭터
**플레이어 (요리사)**
   - 머리 + 다리 2개의 2등신 데포르메, 흰 조리복·요리사 모자, 오른쪽을 보는 방향 1종 (좌측은 X축 반전)
   - Knife는 손에 식칼, Gun은 케첩 건을 들며 실루엣으로 구분한다 — 대기·이동·공격 전 동작에서 구분이 유지되도록 Gun은 `Idle_Gun`·`Move_Gun`·`Attack_Gun` 전용 시트를 쓴다
   - 화면 높이 비율: 플레이어 잉크 높이 1.00u는 화면 높이의 12.5% ("화면 비율" 섹션)

**접지·피벗 규칙 (플레이어·적·보스 공통)**
   - 피벗 y = 잉크 접지선 (시트별 확정값은 "규격" — 플레이어·보스 캔버스 최하단, 적 캔버스 183행)
   - 접지 판정 정본은 라이브러리 `CharacterPhysics2DSide`의 "접촉점 평균 y < transform y" 조건이다 — 라이브러리는 수정하지 않는다
   - 콜라이더 오프셋 조정 주체는 프리팹이다 — 발 콜라이더 하단 = 피벗 − 0.05u (offset y = 콜라이더 높이 ÷ 2 − 0.05, 예: `Object_Player_Knife`·`Object_Player_Gun` 크기 (0.6, 1) → offset (0, 0.45)), 적·보스 프리팹도 같은 식
   - `Object_Floor` 프리팹은 바닥 콜라이더 상단을 타일 시각 상단보다 0.05u 아래 두어 잉크 접지선이 바닥 시각선에 놓이게 한다

**일반 적 (반란 과일)**
   - 몸 없이 굴러오는 과일 머리 단독 — Apple(근접)·Watermelon(탱킹)·Banana(원거리)
   - 크기 위계는 잉크 장축으로 표현한다 (Apple 113px < Banana 123px < Watermelon 138px)

**보스 (거대 과일)**
   - Pumpkin은 일반 적의 1.75배 기준 높이의 근접형, Pineapple은 가시 왕관이 두드러지는 원거리형
   - 과일 머리 규칙을 유지하되 눈썹·이빨로 위협감을 더하고 광폭화 시 눈 색이 붉게 바뀐다

### 환경
**주방 사이드뷰 배경**
   - 화면 전체를 덮는 1920x1080 불투명 1장, 바닥선은 화면 하단 20% 높이 고정
   - 방종류별 톤 — Battle 조리대·불 켜진 화구, Heal 냉장고·파란 조명, Ability 향신료 선반·보라 조명, Boss 거대 오븐·붉은 조명, 로비는 밝은 주방 카운터
   - 채도 25% 이하·명도 75~85%의 저채도로 두어 캐릭터·적과 분리한다

**바닥·소품**
   - 바닥은 `Illust_Casual_Tile/Kitchen`(1024x1024, PPU 128 = 8.0u)을 `Object_Floor` 프리팹 타일 드로우로 반복 — 셀 2.6u(스케일 0.325), 가로 23셀(60u)·세로 1셀(두께 2.6u)
   - 소품은 배경 그림에 포함해 별도 오브젝트를 두지 않는다

### UI
**HUD·팝업**
   - 기존 `UI_Casual_Panel`·`Button`·`Gauge` 재사용, HP 게이지는 `Gauge/Horizontal_Red`, 잠금 표시는 `Mark/Mark_Lock`
   - 방 이력은 `Icon_Casual_Room` 아이콘을 좌→우로 나열하고 현재 방을 `Panel/Select`로 강조한다 (슬롯 초과 시 최근 N개 — N은 `밸런스컨셉`)
   - 로비 중앙에는 선택 캐릭터의 `Illust_Casual_Chef` 일러스트를 세우고, 카드 위 최고 도달 방 순번은 `Icon_Casual_Room/Best` 별 배지 + 숫자로 표시한다 (방 종류 아이콘 재사용 금지)
   - 방 선택 팝업 선택지는 방종류 아이콘 + 적 미리보기(적 아이콘 대신 `AnimationSheet_Casual_Enemy` 첫 프레임 축소 표시 + 마릿수)

### 화면 비율
**카메라 관계식**
   - 화면 높이(u) = 2 × `Scene_Game` 메인 카메라 `orthographicSize`, 잉크 높이(u) = 기준 높이 px ÷ PPU 128, 화면 높이 비율 = 잉크 높이 ÷ 화면 높이
   - 조정 주체는 씬 카메라다 — 스프라이트 스케일·PPU·캔버스 규격은 고정하고 `orthographicSize`만 바꾼다 (`Scene_Game` 씬설정에 값 기재)
   - `orthographicSize` 4.0 확정 → 화면 8.0u × 14.2u (16:9), 반폭 7.1u — Banana 유지 거리 5.0u·Pineapple 유지 거리 6.0u(`밸런스컨셉`)가 화면 안에 들어오는 최소 화면이다

**계열별 화면 높이 비율 (orthographicSize 4.0 기준)**
   - 플레이어: 1.00u = 12.5%
   - 일반 적: Apple 0.88u = 11.0%, Banana 0.96u = 12.0%, Watermelon 1.08u = 13.5%
   - 보스: 1.75u = 21.9%
   - 배경: 화면 100% (1920x1080, PPU 100 = 19.2u × 10.8u 1장을 `Object_Background` 프리팹 스케일 (2.6, 1.6)으로 49.9u × 17.3u 단일 배치 — 방 폭 24u·카메라 클램프 ±4.9u 구간을 전부 덮어 반복 배치 없음)
   - 컨셉아트 `Concept_Scene_Game`의 플레이어 ≈40%는 채택하지 않는다 — 40%면 화면 반폭 2.2u라 원거리 적 유지 거리가 화면 밖이 되어 교전이 성립하지 않는다 (컨셉아트는 이 비율로 재생성 대상)

## 사운드컨셉

### BGM
**로비·전투**
   - 로비 `BGM_Casual/Lobby`, 전투 `BGM_Casual/Battle` 2건, 보스방은 전투 BGM 재생 속도 1.1배로 긴장감만 올린다 (신규 곡 종류 없음)
   - 규격: `.ogg`, 44.1kHz, 스테레오, 루프 구간 끊김 없는 60~90초, 통합 음량 -16 LUFS 기준
   - 톤: 캐주얼 밝은 톤 — 로비는 우쿨렐레·마림바 계열 느린 템포(90~100 BPM), 전투는 브라스·드럼 계열 빠른 템포(140~150 BPM)

### SFX
**전투·진행**
   - 타격 `SFX_Casual_Battle/Hit`, 공격 `Attack`, 사망 `Die`, 능력 획득 `SFX_Casual_Progress/LevelUp`, Gun 해금 `Unlock` 5건 제작 (등록 entry 0건 — 타입 규격은 `.ogg`, 44.1kHz, 모노)
   - 길이: Attack 0.15초·Hit 0.2초·Die 0.3초 (전투 계열 0.3초 상한), LevelUp 0.5초·Unlock 0.8초 (진행 계열 1.0초 상한)
   - 톤: 전투는 주방 소재(칼 부딪힘·과일 으깨짐·물기 소리), 진행은 밝은 벨·차임 상승음
   - 보스 전조는 별도 SFX 없이 바닥 범위 표시만 쓴다 (0.3초 상한 계열에 긴 경고음을 넣지 않는다)

## 참고작품
**Dead Cells (Motion Twin)**
   - 방 단위 진행과 사이드뷰 근접·원거리 전환 리듬

**Overcooked (Ghost Town Games)**
   - 주방 소품·색감과 캐주얼 요리사 실루엣

## 테마 선택
- 테마: Casual
- 등록 리소스 127건이 전부 `Casual`(일부 `Common`·`Twemoji`) 마디라 같은 마디를 쓰면 재사용률이 가장 높다
- 신규 타입은 `{계열}_Casual_{용도}` 규약으로 기존 테마 노드 직속에 붙인다

## 규격
계열 단위 시각 규격 확정값이다.

### AnimationSheet_Casual_Player
- 캔버스: 256x256
- 기준 높이: 128
- 피벗: 하단 중앙 (0.5, 0) = 잉크 접지선 (캔버스 최하단 255행, 11동작 전 프레임 공통 실측)
- 점유율: 캔버스 세로 50%, 화면 높이 12.5%
- 서열: 1.00 유닛 기준

### AnimationSheet_Casual_Enemy
- 캔버스: 256x256
- 기준 높이: 113 (Apple) / 123 (Banana) / 138 (Watermelon) 잉크 장축
- 피벗: (0.5, 0.28) = 잉크 접지선 캔버스 183행 (하단에서 72px, 72 ÷ 256 = 0.281, 3종·전 동작 공통 실측) — 하단 중앙 (0.5, 0) 임포트 금지 (0.56u 부유)
- 점유율: 캔버스 세로 44~54%, 화면 높이 11.0~13.5%
- 서열: 플레이어 0.88~1.08배

### AnimationSheet_Casual_Boss
- 캔버스: 384x384
- 기준 높이: 224
- 피벗: 하단 중앙 (0.5, 0) = 잉크 접지선 (캔버스 최하단 383행, 2종·전 동작 공통 실측)
- 점유율: 캔버스 세로 58%, 화면 높이 21.9%
- 서열: 플레이어 1.75배

### Illust_Casual_Chef
- 캔버스: 640x960
- 기준 높이: 864 (상하 48px 여백)
- 피벗: 하단 중앙 (0.5, 0)
- 점유율: 캔버스 세로 90%, 로비 화면 높이 50% (표시 높이 540px, PPU 100 UI 계열)
- 서열: 로비 카드(440px)보다 크고 화면 중앙 단독

### Illust_Casual_Background
- 캔버스: 1920x1080
- 기준 높이: 1080
- 피벗: 중심 (0.5, 0.5)
- 점유율: 100% (PPU 100 = 19.2u × 10.8u, 배치는 "화면 비율" 배경 항목)
- 서열: 화면 전면

### Illust_Casual_Tile
- 캔버스: 1024x1024
- 기준 높이: 1024
- 피벗: 중심 (0.5, 0.5)
- 점유율: 셀 2.6u (PPU 128 = 8.0u × 스케일 0.325), 바닥 두께 1셀
- 서열: 플레이어 2.6배 (셀 한 변)

### Icon_Casual_Room
- 캔버스: 128x128
- 기준 높이: 112
- 피벗: 중심 (0.5, 0.5)
- 점유율: 87.5%
- 서열: 아이콘 공통 (HUD 64x64 표시)

### Illust_Casual_Projectile
- 캔버스: 512x64
- 기준 높이: 64 (기준 길이 384)
- 피벗: 좌측 중앙 (0, 0.5)
- 점유율: 가로 75%
- 서열: 플레이어 0.5배 (길이 3 유닛)

## 연출 요구
**타격 3요소 동시성**
   - 명중 프레임에 `Illust_Casual_Hit/Impact` 팝 스케일 + `SFX_Casual_Battle/Hit` + 데미지 숫자를 동시 재생한다

**보스 전조**
   - Slam·Charge·Rain 전조는 `UI_Common_Shape/Circle128`를 붉은 틴트로 바닥에 깔아 범위를 보인다 — 스케일 x > y의 납작한 가로 타원, 중심은 대상 발밑 바닥 y (`게임컨셉` "보스 전조", 전조 시간은 `밸런스컨셉`)

**처치 연출**
   - 사망 6프레임 찌그러짐 + `Illust_Casual_Splatter/Death`(소스 레드 `#D93B3B` 단일 예외색) 동시 재생, 지면 잔존물 없음

**컨셉아트 산출**
   - `Concept_Resource` 1장 — 전투 방 한 장면에 요리사(Knife)·Apple·Watermelon·Banana·Pumpkin을 한 화면에 담아 팔레트·크기 서열 정본으로 쓴다

## 애니메이션 규격
- 시트 구성: 한 동작 = 파일 1건, 프레임 슬롯 6개 (4~8프레임, 남는 슬롯 비움)
- 프레임 공통: 같은 캔버스·피벗·기준 높이 유지, 좌우는 X축 반전
- 적 이동 예외: `AnimationSheet_Casual_Enemy` 이동은 실측대로 1프레임 + 런타임 코드 회전 (기존 산출물 실측 우선)
- 클립 길이: 공격·패턴 클립 길이는 `밸런스컨셉` 공격 주기·전조 시간이 정본
- 보스 동작: Idle·Move·Attack1(Slam/Spike)·Attack2(Charge/Rain)·Die 5동작
- 플레이어 추가 동작: Attack1·Attack2·Attack3(Knife), Shoot(Gun), Jump
- Gun 전용 동작: Idle_Gun·Move_Gun — `Attack_Gun`과 같은 케첩 건 실루엣·같은 캔버스 규격, Knife의 Idle·Move와 프레임 수·타이밍 동일
