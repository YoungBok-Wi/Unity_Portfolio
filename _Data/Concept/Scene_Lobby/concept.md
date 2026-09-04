---
name: 씬컨셉
description: |
  Scene_Lobby의 역할과 사용 모듈, UI 구성을 정의하는 씬 컨셉 문서
---
# Scene_Lobby

## 개요
외부 도구(Unity 등)가 파싱하여 적용하는 씬 설정값을 포함한다.
- 역할: 로비 씬 — 캐릭터 선택·런 시작·설정 진입·앱 종료 (`게임컨셉` "런 루프"의 캐릭터 선택·시작 단계)
- 빌드 인덱스: 0
- 차원: 2D
- 카메라: Side
- 씬 경로: Assets/__Game/_Core/__Scene/Scene_Lobby.unity

## 설명
- 앱 시작 씬이자 런 종료(결과 팝업 확인) 후 복귀 씬이다. 씬 이름은 라이브러리 `SceneChangeManager`·`ShutdownManager` 기본값 `Scene_Lobby`와 같다.
- 화면 중앙에 선택된 요리사 일러스트(`Illust_Casual_Chef` Knife·Gun, `리소스컨셉` 규격)가 서고 좌우에 Knife·Gun 선택 카드, 하단에 시작 버튼, 우상단에 설정 버튼을 둔다 (배치 정본은 `Concept_Scene_Lobby` 컨셉아트).
- 카드 위 최고 도달 방 순번은 별 배지 `Icon_Casual_Room/Best` + 숫자로 표시한다 (`게임컨셉` "최고 순번 배지").
- Gun 카드는 `Character` 모듈의 해금 저장값이 false면 `UI_Casual_Mark/Mark_Lock` 잠금 마크를 켜고 선택을 막으며, 잠금 카드 클릭 시 `Popup_Notify`로 해금 조건을 알린다 (해금 조건은 `게임컨셉` "캐릭터 해금").
- 시작 버튼은 선택 캐릭터를 `Character` 모듈에 저장한 뒤 `SceneChangeManager`로 `Scene_Game`에 진입한다.
- 로비 BGM은 `BGM_Casual/Lobby`를 재생한다 (`리소스컨셉` "사운드컨셉").

## 사용 모듈

### Camera2D
- 2D 사이드뷰 카메라 매니저를 `[Local]` 하위에 세운다.

### Input
- 씬 입력 매니저 — 취소 입력(Esc·게임패드 Start)을 팝업 매니저에 전달한다.

### Popup
- 씬 단위 팝업 매니저와 UI 카메라를 세운다.

### Save
- Gun 해금 여부·마지막 선택 캐릭터·최고 도달 방 순번·볼륨의 저장 통로다.

### Sound
- BGM·SFX 재생과 볼륨 저장값 적용.

### Table
- `Character` 테이블 로드.

### Value
- 반응형 값 컨테이너 — 선택 캐릭터·해금 상태 구독.

### Number
- 최고 도달 방 순번 등 숫자 값 ID 조회.

### Language
- 로비 UI 문구 조회 (문구 정본은 Text 테이블).

### Icon
- 캐릭터·무기 아이콘(`Icon_Casual_Weapon`) 조회.

### Deal
- 거래 중개 — 로비에서는 등록만 유지한다.

### Bank
- Crumb 재화 저장소 등록 (로비에서는 표시 없음).

### Delegate
- 값 갱신 콜백 병합.

### Character
- 캐릭터 선택·Gun 해금 저장값을 읽고 쓰는 `게임모듈` (신규).

## UI

### Popup_Lobby
- 로비 화면 `베이스형` 팝업 — 캐릭터 선택 카드(Knife·Gun, 잠금 마크)·시작 버튼·설정 버튼·최고 도달 방 순번.

### Popup_Quit
- 앱 종료 확인 팝업.

### Popup_Setting
- 설정 팝업 — BGM·효과음 볼륨 슬라이더·전체 화면 토글·적용·기본값 (`Control_GameFrame`, 로비·게임 공유).

### Popup_Notify
- 단순 알림 팝업 (라이브러리 기본) — Gun 잠금 카드 클릭 시 해금 조건 알림·설정 적용 알림 표시.

## UI 상태
"UI" 목록 항목별 포함 상태·사유다.

**Popup_Lobby**
   - 상태: 포함
   - 사유: 로비 화면 `베이스형` 팝업 — `Game` 프리셋 골격이 실재한다 (노드 구성·코드는 프리셋 Work 담당).

**Popup_Setting**
   - 상태: 포함
   - 사유: 필수 팝업(설정) — BGM·SFX 볼륨·전체 화면 (로비·게임 공유). `Game` 프레임 `컨트롤` `Control_GameFrame`으로 재구성해 `inAsset` 복원.

**Popup_Quit**
   - 상태: 포함
   - 사유: 필수 팝업(종료) — 라이브러리 기본 팝업이 실재하고 `게임컨셉` "이탈 경로"의 종료 확인 팝업이다.

**Popup_Notify**
   - 상태: 포함
   - 사유: 라이브러리 기본 알림 팝업이 실재한다 — Gun 잠금 클릭·설정 적용 알림에 쓴다 (`Scene_Game`과 같은 팝업).

**Popup_Pause**
   - 상태: 제외
   - 사유: 필수 팝업(일시정지) — 로비는 진행 상태가 없는 씬이라 일시정지 대상이 없다.

## Object

### Object_Background
- 로비 배경 1장 (`Illust_Casual_Background` 로비, 사이드뷰 1920x1080 불투명).

## 취소 입력
- Popup_Quit (열린 팝업이 없을 때 취소 입력이 종료 확인 팝업을 연다. 열린 팝업이 있으면 그 팝업이 닫힌다)
