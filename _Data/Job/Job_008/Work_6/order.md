# 업무지시서

## 1. 셋업 전 씬 상태 스냅샷

**대상 스킬**: 유니티엔진_씬_질문

**"question"**: `Scene_Game`·`Scene_Lobby` 계층 전체와 [Global]·[Local]·[Popup]·오브젝트 루트의 직렬화 필드 값 — 특히 `Scene_Lobby` 메인 카메라 `orthographicSize`(4.0 오버라이드), `Scene_Game` 카메라·`[LocalRoomManager]` 스폰 위치·반폭·`[LocalGameManager]` 프리팹 목록·이펙트·SFX·BGM 배선, `[SceneChangeManager]` 배열, 빌드 등재

**업무**

- 목표: 셋업 뒤 대조할 기준 스냅샷을 `_Temp/Work_6_J8/before_{씬}.json`에 남긴다 (Job_005 로비 카메라 오버라이드 소실 사례 대비)

## 2. 씬 셋업 실행

**대상 스킬**: 유니티엔진_씬_셋업_실행

**"sceneName"**: Scene_Game, Scene_Lobby (대상만 달리해 2회)

**"modules"**: Scene_Game — 씬설정 "사용 모듈" 전부(재편 8모듈 포함). Scene_Lobby — 씬설정 "사용 모듈" 전부(`Data`·`Game` 포함)

**"popups"**: 씬설정 "UI" 목록

**"objects"**: Scene_Game — 씬설정 "Object" 목록(`Object_Player_Knife`·`Object_Player_Gun` 씬 배치 포함)

**업무**

- 씬 컨셉 등재는 Work_1 결과를 전제로 하고, 등재 대기 상태였던 모듈은 이번 실행에서 확정한다
- `[Global]` 셋업으로 `[GameManager]`·`[DataManager]`가 서고 구 `[BattleManager]`·`[CharacterManager]` 인스턴스가 정리되는지, `[SceneChangeManager]` 아래 `Face` 자식·배열 배선(Game 셋업 스크립트)이 되는지 확인
- `[Local]`에 `[LocalGameManager]`·`[LocalRoomManager]`·`[LocalRoomSelectManager]`·`[LocalPlayerCharacterManager]`가 서고 구 `[LocalBattleManager]`·`[LocalCharacterManager]`가 남지 않는지 확인
- 플레이어 2종이 오브젝트 루트에 배치되고 `[LocalPlayerCharacterManager].m_Players`에 배선되는지 확인 (셋업이 배선하지 않으면 3에서 한다)
- 완료 기준: 두 씬 setup 응답 성공, 셋업 검증 통과

## 3. 씬 고유 참조 이관·오버라이드 복원

**대상 스킬**: 유니티엔진_씬_구성

**"sceneName"**: Scene_Game, Scene_Lobby (대상만 달리해 2회)

**"content"**: 1의 스냅샷과 대조해 소실된 오버라이드(카메라 `orthographicSize` 4.0 등) 복원, 구 매니저의 인스펙터 값(적·보스 프리팹 목록·투사체·이펙트·SFX·BGM·스폰 위치·반폭·플레이어 프리팹 목록→상주 플레이어 참조)을 새 매니저로 이관, 플레이어 2종 위치를 `m_PlayerSpawn`에 맞추고 비활성 초기값 설정

**업무**

- 이관 뒤 구 매니저 오브젝트·Missing 스크립트 0건
- 완료 기준: 저장된 씬 YAML에서 새 매니저 필드 배선 확인, 스냅샷 대조 차이는 의도한 변경만

## 4. 씬 검증

**대상 스킬**: 유니티엔진_씬_검증

**"scope"**: Scene_Game·Scene_Lobby 계층·직렬화 필드·컨셉 반영 대조·컴파일

**업무**

- 완료 기준: verify 통과, 컴파일 에러 0(Work_5_1 삭제 반영 포함), Missing 참조 0, 두 씬 빌드 등재 유지
- `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`) 수정 금지. DataMCP는 `Fallback`(curl) 사용 중. 사용자에게 질문하지 않는다
