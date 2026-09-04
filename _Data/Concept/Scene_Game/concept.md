---
name: 씬컨셉
description: |
  Scene_Game의 역할과 사용 모듈, UI 구성을 정의하는 씬 컨셉 문서
---
# Scene_Game

## 개요
외부 도구(Unity 등)가 파싱하여 적용하는 씬 설정값을 포함한다.
- 역할: 게임 플레이 씬 — 방 단위 전진·전투·능력 선택·보스전을 씬 전환 없이 진행하고 런 종료 시 로비로 돌아간다 (`게임컨셉` "런 루프"·"전투 루프")
- 빌드 인덱스: 1
- 차원: 2D
- 카메라: Side
- 씬 경로: Assets/__Game/_Core/__Scene/Scene_Game.unity

## 설명
- 로비에서 시작하면 진입하며 1번째 방은 항상 Battle이다. 방 클리어 → `Popup_RoomSelect`(2택·적 미리보기) → 같은 씬 안에서 다음 방을 구성·입장한다 (방 종류·선택지 세트는 `게임컨셉`, 수치는 `밸런스컨셉`).
- 카메라는 플레이어 X를 추적하고 Y는 고정하며 방 좌우 경계에서 클램프한다 (`게임컨셉` "화면 추적 기준"). 메인 카메라 `orthographicSize`는 4.0이다 (`리소스컨셉` "화면 비율" — 화면 높이 비율의 조정 주체는 이 씬 카메라이고 스프라이트 스케일은 손대지 않는다). 바닥선은 화면 하단 20% 높이다 (`리소스컨셉` "환경").
- 방 좌우 벽·적 등장 위치·카메라 X 클램프 값은 `밸런스컨셉` "방 구조"가 정본이며, 벽 콜라이더는 `Room` 모듈이 방 구성 시 세운다 (씬에 미리 두지 않는다). 플레이어는 무입력 시 정지하고 피격 넉백 후 자동 정지한다 (`게임컨셉` "무입력 정지", 값은 `밸런스컨셉` "플레이어 공통").
- 플레이어는 `Character` 모듈의 선택 캐릭터로 방 입장 시 스폰하고, 적·보스·투사체는 `Room`·`Battle` 모듈이 `ObjectPool`로 런타임 스폰한다 (씬에 미리 두지 않는다).
- 보스 FSM(Pumpkin·Pineapple 5상태)과 적 상태는 `FSM` 모듈, 이동·점프·접지는 `CharacterPhysics` 모듈을 쓴다. Crumb 획득·리롤 소모는 `Bank` 재화 `Crumb`으로 처리한다.
- 런 종료(보스 처치 또는 HP 0)는 `Popup_Result`로 도달 순번·Crumb 총량·Gun 해금 알림을 보인 뒤 `Scene_Lobby`로 전환한다. 전투 BGM은 `BGM_Casual/Battle`이며 보스방은 재생 속도 1.1배다.

## 사용 모듈

### Camera2D
- 2D 사이드뷰 카메라 매니저를 `[Local]` 하위에 세운다 (플레이어 X 추적·방 경계 클램프).

### Input
- 씬 입력 매니저 — 이동·점프·공격·취소 입력(Input System).

### Popup
- 씬 단위 팝업 매니저와 UI 카메라를 세운다.

### Save
- Gun 해금·최고 도달 방 순번 저장 통로.

### Sound
- 전투 BGM·타격·처치·능력 획득 SFX 재생.

### Table
- `Character`·`Enemy`·`Boss`·`Room`·`Ability`·`Wave` 테이블 로드.

### Value
- 반응형 값 컨테이너 — HP·방 순번·능력 누적 구독.

### Number
- HP·방 순번 등 숫자 값 ID 조회 (HUD 표시).

### Language
- 팝업·HUD 문구 조회 (문구 정본은 Text 테이블).

### Icon
- 방종류(`Icon_Casual_Room`)·능력(`Icon_Casual_Upgrade`)·재화 아이콘 조회.

### Deal
- Crumb 리롤 지불 등 거래 중개.

### Bank
- Crumb 재화 잔액 관리 (런 종료 시 소멸).

### Delegate
- 같은 프레임 값 갱신 콜백 병합 (HUD 갱신 부하 분산).

### FSM
- 적·보스 상태 기계 (Idle·Chase·Slam·Charge·Enrage / Idle·Retreat·Spike·Rain·Enrage).

### CharacterPhysics
- 플레이어·적의 좌우 이동·점프·접지 판정.

### ObjectPool
- 적·보스·투사체·히트 이펙트 풀링.

### Room
- 방 진행 `게임모듈` (신규) — 방 순번·이력·선택지 세트·웨이브 스폰·클리어 판정·능력 선택·결과.

### Battle
- 전투 `게임모듈` (신규) — HP·데미지·넉백·히트스톱·투사체·Crumb 낙하.

### Character
- 캐릭터 `게임모듈` (신규) — 선택 캐릭터 스폰·Gun 해금 저장.

## UI

### Popup_HUD
- `베이스형` HUD — HP 게이지(`UI_Casual_Gauge/Horizontal_Red`)·방 순번·지나온 방 종류 아이콘 열(슬롯 초과 시 최근 N개, N은 `밸런스컨셉` "플레이어 공통")·Crumb 잔액·데미지 숫자.

### Popup_RoomSelect
- `프레임형` 방 선택 팝업 — 2택 선택지(방 종류 아이콘 + 다음 방 적 종류·마릿수 미리보기), 취소 불가.

### Popup_Ability
- `프레임형` 능력 선택 팝업 — 6종 중 무작위 3택1 카드(`Icon_Casual_Upgrade`)·Crumb 소모 리롤 버튼, 취소 불가.

### Popup_Pause
- `프레임형` 일시정지 팝업 — 게임 시간 정지, 재개·설정·포기(로비로) 버튼.

### Popup_Result
- `프레임형` 결과 팝업 — 승패·도달 방 순번·Crumb 총량·Gun 해금 알림, 확인 시 `Scene_Lobby`로.

### Popup_Notify
- 단순 알림 팝업 (라이브러리 기본) — 설정 적용·해금 알림 표시.

### Popup_Setting
- 설정 팝업 — BGM·효과음 볼륨 슬라이더·전체 화면 토글·적용·기본값 (`Control_GameFrame`, 로비·게임 공유).

## UI 상태
"UI" 목록 항목별 포함 상태·사유다.

**Popup_HUD**
   - 상태: 포함
   - 사유: `Game` 프리셋 골격이 실재한다 (노드 구성·코드는 프리셋 Work 담당).

**Popup_RoomSelect**
   - 상태: 포함
   - 사유: `Game` 프리셋 골격이 실재한다 (노드 구성·코드는 프리셋 Work 담당).

**Popup_Ability**
   - 상태: 포함
   - 사유: `Game` 프리셋 골격이 실재한다 (노드 구성·코드는 프리셋 Work 담당).

**Popup_Pause**
   - 상태: 포함
   - 사유: 필수 팝업(일시정지) — `Game` 프리셋 골격이 실재하며 취소 입력 소비 주체다.

**Popup_Result**
   - 상태: 포함
   - 사유: `Game` 프리셋 골격이 실재한다 (노드 구성·코드는 프리셋 Work 담당).

**Popup_Setting**
   - 상태: 포함
   - 사유: 필수 팝업(설정) — `Popup_Pause`의 설정 버튼으로 연다. `Game` 프레임 `컨트롤` `Control_GameFrame`으로 재구성해 `inAsset` 복원.

**Popup_Notify**
   - 상태: 포함
   - 사유: 라이브러리 기본 알림 팝업이 실재한다 — 설정 적용·Gun 해금 알림에 쓴다.

**Popup_Quit**
   - 상태: 제외
   - 사유: 필수 팝업(종료) — `게임컨셉` "이탈 경로"대로 게임 씬의 이탈은 `Popup_Pause`의 포기(로비로)이고 앱 종료 확인은 `Scene_Lobby`가 담당한다.

## Object

### Object_Player_Knife
- Knife 요리사 플레이어 — 근접 3단 콤보·점프 (`AnimationSheet_Casual_Player`).

### Object_Player_Gun
- Gun 요리사 플레이어 — 정지 연사·점프, 투사체 발사 (`AnimationSheet_Casual_Player` Gun 전용 `Idle_Gun`·`Move_Gun`·`Attack_Gun`, Knife 시트 공유 금지).

### Object_Enemy_Apple
- 근접 공격형 일반 적 (`AnimationSheet_Casual_Enemy/Apple_*`), 런타임 스폰.

### Object_Enemy_Watermelon
- 탱킹형 일반 적, 몸통 박치기, 런타임 스폰.

### Object_Enemy_Banana
- 원거리형 일반 적, 껍질 투사체 발사, 런타임 스폰.

### Object_Boss_Pumpkin
- 근접형 보스 (FSM 5상태, `AnimationSheet_Casual_Boss`), 런타임 스폰.

### Object_Boss_Pineapple
- 원거리형 보스 (FSM 5상태), 런타임 스폰.

### Object_Projectile
- 투사체 공통 오브젝트 — Gun 탄·Banana 껍질·Pineapple 가시를 변형으로 둔다 (`Illust_Casual_Projectile`), 런타임 스폰.

### Object_Background
- 방종류별 사이드뷰 배경 1장 (`Illust_Casual_Background` Battle·Heal·Ability·Boss 변형), 씬 배치.

### Object_Floor
- 바닥 타일 반복 (`Illust_Casual_Tile/Kitchen`), 바닥선 화면 하단 20%, 씬 배치.

## 취소 입력
- Popup_Pause (열린 팝업이 없을 때 취소 입력이 일시정지 팝업을 연다. `Popup_RoomSelect`·`Popup_Ability`·`Popup_Result`가 열려 있으면 취소 입력을 무시한다)
