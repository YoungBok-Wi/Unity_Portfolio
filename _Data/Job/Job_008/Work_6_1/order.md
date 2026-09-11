# 업무지시서

## 1. 열린 Scene Lobby 스냅샷 보완

**대상 스킬**: 유니티엔진_씬_질문

**"question"**: `Scene_Lobby` 전체 계층과 직렬화 필드, 메인 카메라 `orthographicSize` `4.0`, 빌드 등재 상태

**업무**

- 사용자가 `Scene_Lobby`를 연 상태에서 `_Temp/Work_6_J8/before_Scene_Lobby.json`을 작성한다.
- `Work_6`의 `_Temp/Work_6_J8/before_Scene_Game.json`과 함께 셋업 전 기준으로 사용한다.
- 완료 기준은 두 스냅샷 존재와 `Scene_Lobby` 메인 카메라 `orthographicSize=4.0`, 두 씬 빌드 등재 확인이다.

## 2. 씬 셋업 실행

**대상 스킬**: 유니티엔진_씬_셋업_실행

**"sceneName"**: `Scene_Game`, `Scene_Lobby`

**"modules"**: `Scene_Game`은 씬설정 사용 모듈 전부와 재편 8모듈, `Scene_Lobby`는 씬설정 사용 모듈 전부와 `Data`·`Game`

**"popups"**: 각 씬설정 `UI` 목록

**"objects"**: `Scene_Game` 씬설정 `Object` 목록과 `Object_Player_Knife`·`Object_Player_Gun`

**업무**

- 기존 씬 파일을 사용하므로 `유니티엔진_씬_생성`은 제외한다.
- `Work_1`, `Work_4_3`, `Work_5_6`에서 완료한 컨셉·모듈·프리셋을 재사용하므로 컨셉 서브 선행 체인은 제외한다.
- `[Global]`에 `[GameManager]`·`[DataManager]`, `[Local]`에 `[LocalGameManager]`·`[LocalRoomManager]`·`[LocalRoomSelectManager]`·`[LocalPlayerCharacterManager]`가 구성되고 구 매니저가 남지 않는지 확인한다.
- `[SceneChangeManager]/Face`와 배열, 플레이어 2종 배치와 `m_Players` 배선을 확인한다.
- 완료 기준은 두 씬 setup 응답 성공과 셋업 검증 통과다.

## 3. 씬 고유 참조 이관과 오버라이드 복원

**대상 스킬**: 유니티엔진_씬_구성

**"sceneName"**: `Scene_Game`, `Scene_Lobby`

**"content"**: 셋업 전 스냅샷과 대조해 씬 고유 참조를 새 매니저로 이관하고 `Scene_Lobby` 카메라 `orthographicSize=4.0` 등 소실된 오버라이드를 복원

**업무**

- 적·보스 프리팹, 투사체, 이펙트, SFX, BGM, 스폰 위치·반폭을 새 매니저에 보존한다.
- 플레이어 프리팹 목록은 상주 플레이어 2종 참조로 이관하고 위치를 `m_PlayerSpawn`에 맞춘 뒤 비활성 초기값으로 둔다.
- 완료 기준은 구 매니저·Missing 스크립트 `0건`, 저장된 씬 YAML의 새 매니저 배선, 스냅샷 대비 의도한 변경만 존재하는 상태다.

## 4. 씬 검증

**대상 스킬**: 유니티엔진_씬_검증

**"scope"**: `Scene_Game`·`Scene_Lobby` 계층·직렬화 필드·컨셉 반영·컴파일·빌드 등재

**업무**

- 완료 기준은 verify 통과, 컴파일 에러 `0건`, Missing 참조 `0건`, 두 씬 빌드 등재 유지다.
- `confirmed`·`reuse`와 `Assets/_Library/**`·`_Data/Module/Library/**`를 변경하지 않는다.
