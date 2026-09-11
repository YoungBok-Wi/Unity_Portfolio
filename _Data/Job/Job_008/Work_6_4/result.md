# [유니티엔진] "씬 셋업 재시도" 업무 레포트

## 요약
- `Scene_Lobby`·`Scene_Game` setup은 `success=true`이고 UI 카메라 스택 `1`, 얼굴 전환 배열 `2`, Missing 스크립트 `0건`이다.
- `Scene_Game`의 상주 플레이어 2종을 배치·비활성화하고 새 `LocalPlayerCharacterManager.m_Players`에 배선했다.
- 두 씬 카메라는 `orthographicSize=4.0`으로 복원했으나 `[Global].prefab` 원본의 구 매니저 중복 때문에 최종 씬 검증은 미수행이다.

## 완료업무

### 셋업 전 상태 재확인
**산출물**
`_Temp/Work_6_J8/before_Scene_Game.json`
`_Temp/Work_6_J8/before_Scene_Lobby.json`
**작업내용**
- 두 스냅샷과 `Assets/_Library/Delegate/Prefab/[DelegateManager].prefab` 존재를 확인했다.
- `Scene_Lobby`는 활성·비오염 상태였고 빌드 목록은 `Scene_Lobby` 인덱스 `0`, `Scene_Game` 인덱스 `1`, 양쪽 `enabled=true`다.

### 두 씬 셋업 실행
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
`Assets/__Game/_Core/__Scene/Scene_Game.unity`
**작업내용**
- 두 씬의 `editor_util setup` 응답은 각각 `success=true`다.
- 두 씬의 `concept_manage verify` 응답은 각각 `success=true`다.
- `Scene_Game`은 새 `DataManager`·`GameManager`·`LocalGameManager`·`LocalPlayerCharacterManager`·`LocalRoomManager`·`LocalRoomSelectManager`가 존재한다.
- `Scene_Lobby`는 새 `DataManager`·`GameManager`·`LocalGameManager`가 존재한다.

### 씬 고유 참조와 오버라이드 부분 복원
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Game.unity`
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
**작업내용**
- 새 `GameManager`의 로비 BGM, 새 `DataManager`의 저장 테이블·기본 캐릭터, 새 `LocalGameManager`의 적·보스·투사체·이펙트·SFX·BGM 참조가 스냅샷 값과 일치한다.
- `Object_Player_Knife`·`Object_Player_Gun`을 `[Stage]` 아래 `(0,-2.4,0)`에 배치하고 둘 다 비활성화했다.
- 새 `LocalPlayerCharacterManager.m_Players` 배열 크기를 `2`로 만들고 Knife·Gun을 순서대로 배선했다.
- 두 씬의 라이브러리 카메라 인스턴스 허용 오버라이드를 `orthographicSize=4.0`으로 복원했다.

## 비고
- 구 로컬 `[LocalBattleManager]`·`[LocalCharacterManager]`는 마지막 setup이 제거하고 새 이름으로 재인스턴스화했다.
- 최종 씬 검증은 필수 구조 정리 뒤 다시 수행해야 하므로 실행하지 않았다.

## 예외상황
- 대상: `Assets/__Game/_Core/Prefab/[Global].prefab`의 `[BattleManager]`·`[CharacterManager]`와 두 씬의 동일 인스턴스.
- 막힌 지점: `유니티엔진_씬_구성`의 `Global 오버라이드 금지` 규칙 때문에 씬 인스턴스 삭제로 처리할 수 없고, 원본 프리팹 구성은 현재 Work의 대상 스킬 범위 밖이다.
- 실측 근거: 원본 YAML에 `[BattleManager]`·`[CharacterManager]`와 새 `[GameManager]`·`[DataManager]`가 함께 있으며 두 씬 setup 후 계층에도 네 오브젝트가 모두 남는다.
- 필요한 처리: 게임 소유 `[Global].prefab` 원본에서 구 매니저 2종을 제거한 뒤 두 씬 setup·카메라 예외 복원·상주 플레이어 배선·최종 검증을 다시 수행한다.
