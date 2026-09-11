# 업무지시서

## 1. Global 프리팹 중복 상태 확인

**대상 스킬**: 유니티엔진_에셋_질문

**"assetPath"**: `Assets/__Game/_Core/Prefab/[Global].prefab`

**업무**

- 원본의 직속 자식과 중첩 프리팹 원본을 조회해 구 `[BattleManager]`·`[CharacterManager]`와 새 `[GameManager]`·`[DataManager]`의 동시 존재를 확인한다.
- 구 매니저 참조값이 새 매니저에 보존됐는지 대조한다.
- 완료 기준은 제거 대상 2개와 보존 대상 전건이 실측된 상태다.

## 2. Global 프리팹 구 매니저 제거

**대상 스킬**: 유니티엔진_에셋_프리팹_구성

**"assetPath"**: `Assets/__Game/_Core/Prefab/[Global].prefab`

**"content"**: 직속 구 `[BattleManager]`·`[CharacterManager]` 중첩 프리팹 인스턴스 제거

**업무**

- 원본 프리팹에서 구 매니저 2개만 제거하고 새 `[GameManager]`·`[DataManager]`와 나머지 전역 매니저를 보존한다.
- 열린 씬은 변경하지 않는 프리팹 원본 편집 경로를 사용한다.
- 완료 기준은 원본 직속 자식에서 구 이름 `0건`, 새 이름 각 `1건`이다.

## 3. Global 프리팹 재임포트

**대상 스킬**: 유니티엔진_재임포트_실행

**"changedPaths"**: `Assets/__Game/_Core/Prefab/[Global].prefab`

**업무**

- 원본 프리팹을 재임포트하고 기존 `.meta` GUID를 보존한다.
- 완료 기준은 Refresh 성공과 `.meta` 존재다.

## 4. Global 프리팹 무결성 검증

**대상 스킬**: 유니티엔진_에셋_검증

**"assetPath"**: `Assets/__Game/_Core/Prefab/[Global].prefab`

**업무**

- 프리팹 로드·Missing 스크립트·Missing 참조·중복 매니저를 검증한다.
- 완료 기준은 로드 성공, Missing `0건`, 구 매니저 `0건`, 새 매니저 각 `1건`이다.

## 5. 두 씬 셋업 재실행

**대상 스킬**: 유니티엔진_씬_셋업_실행

**"sceneName"**: `Scene_Game`, `Scene_Lobby`

**업무**

- 정리된 `[Global].prefab` 기준으로 두 씬 setup을 다시 실행한다.
- 완료 기준은 두 setup 응답 성공, UI 카메라 스택 `1`, 얼굴 전환 배열 `2`, 구 전역·로컬 매니저 `0건`이다.

## 6. 씬 고유 값 최종 복원

**대상 스킬**: 유니티엔진_씬_구성

**"sceneName"**: `Scene_Game`, `Scene_Lobby`

**"content"**: 카메라 `orthographicSize=4.0`, `Scene_Game` 상주 플레이어 2종 배치·비활성화·배선과 참조값 보존

**업무**

- 마지막 setup 뒤 두 씬 카메라 허용 오버라이드를 복원한다.
- `Scene_Game`의 플레이어 2종과 `LocalPlayerCharacterManager.m_Players`를 복원하고 새 매니저 참조값을 재확인한다.
- 완료 기준은 두 씬 저장 성공과 요구 직렬화 값 일치다.

## 7. 두 씬 최종 검증

**대상 스킬**: 유니티엔진_씬_검증

**"scope"**: `Scene_Game`·`Scene_Lobby` 계층·직렬화 필드·컨셉·컴파일·빌드 등재

**업무**

- Missing 스크립트·참조, 매니저 중복, 카메라 스택, 얼굴 전환, 플레이어 배선, 빌드 등재를 검증한다.
- `confirmed`·`reuse`와 승인 범위 밖 설정은 변경하지 않는다.
- 완료 기준은 검증 오류 `0건`과 컴파일 오류 `0건`이다.
