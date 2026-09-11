# 업무지시서

## 1. 셋업 전 상태 재확인

**대상 스킬**: 유니티엔진_씬_질문

**"question"**: `Work_6`·`Work_6_1`의 두 씬 스냅샷과 현재 열린 씬·빌드 등재·`Delegate` Assets 복원 상태

**업무**

- `_Temp/Work_6_J8/before_Scene_Game.json`과 `before_Scene_Lobby.json`을 셋업 전 기준으로 재사용한다.
- `Scene_Lobby`가 열린 상태, 두 씬 빌드 등재, `Assets/_Library/Delegate/Prefab/[DelegateManager].prefab` 존재를 확인한다.
- 완료 기준은 기준 스냅샷 2개와 셋업 선행조건 확인이다.

## 2. 씬 셋업 실행

**대상 스킬**: 유니티엔진_씬_셋업_실행

**"sceneName"**: `Scene_Game`, `Scene_Lobby`

**"modules"**: 각 씬설정 `사용 모듈` 전부

**"popups"**: 각 씬설정 `UI` 목록

**"objects"**: 각 씬설정 `Object` 목록

**업무**

- `[Global]`·`[Local]` 매니저, `[SceneChangeManager]/Face`, 팝업, `Scene_Game` 플레이어 2종을 셋업한다.
- setup 전후 프리팹 오버라이드를 대조하고 허용 예외 복원값을 기록한다.
- 완료 기준은 두 씬 setup 응답 성공, 등록된 매니저 인스턴스 전건 존재, UI 카메라 스택 배선이다.

## 3. 씬 고유 참조 이관과 오버라이드 복원

**대상 스킬**: 유니티엔진_씬_구성

**"sceneName"**: `Scene_Game`, `Scene_Lobby`

**"content"**: 스냅샷 기준 씬 고유 참조 이관, 구 매니저 정리, `Scene_Lobby` 카메라 `orthographicSize=4.0` 복원, 상주 플레이어 배치·비활성화·배선

**업무**

- 적·보스 프리팹, 투사체, 이펙트, SFX, BGM, 스폰 위치·반폭을 새 매니저에 보존한다.
- `[LocalPlayerCharacterManager].m_Players`에 씬 상주 플레이어 2종을 배선하고 `m_PlayerSpawn` 위치에 둔다.
- 구 `[BattleManager]`·`[CharacterManager]`·`[LocalBattleManager]`·`[LocalCharacterManager]`를 제거한다.
- 완료 기준은 저장된 씬 YAML의 새 매니저 배선, Missing 스크립트 `0건`, 스냅샷 대비 의도한 변경만 존재하는 상태다.

## 4. 씬 검증

**대상 스킬**: 유니티엔진_씬_검증

**"scope"**: `Scene_Game`·`Scene_Lobby` 계층·직렬화 필드·컨셉 반영·컴파일·빌드 등재

**업무**

- 완료 기준은 verify 통과, 컴파일 오류 `0건`, Missing 참조 `0건`, 두 씬 빌드 등재 유지다.
- `confirmed`·`reuse`를 변경하지 않고 이번에 승인된 `Delegate inAsset=true` 외 설정은 바꾸지 않는다.
