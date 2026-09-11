# [유니티엔진] "Global 프리팹·씬 최종 보완" 업무 레포트

## 요약
- `[Global].prefab`의 구 `[BattleManager]`·`[CharacterManager]`를 제거하고 새 `[GameManager]`·`[DataManager]`를 각 1개로 유지했다.
- `Scene_Game`·`Scene_Lobby` 셋업과 고유 값 복원을 완료했다. 최종 검증은 씬별 실패 `0건`, Missing 스크립트·참조 `0건`, 콘솔 오류 `0건`이다.
- 런타임 캡처에서 컨셉 대조 5항목을 확인했다. 캡처는 `_Temp/Work_6_J8/Scene_Game_final.png`·`_Temp/Work_6_J8/Scene_Lobby_final.png`이다.

## 완료업무

### Global 프리팹 정리
**산출물**
`Assets/__Game/_Core/Prefab/[Global].prefab`
**작업내용**
- 원본 조회에서 구 매니저 2개와 새 매니저 2개의 동시 존재 및 보존 값 일치를 확인했다.
- 원본 직속 자식의 구 매니저를 제거했다. 검증값은 `[BattleManager]=0`, `[CharacterManager]=0`, `[GameManager]=1`, `[DataManager]=1`이다.
- 재임포트 후 기존 `.meta`와 GUID가 유지됐다.
- 프리팹 로드 성공, Missing 스크립트·참조·중첩 프리팹 `0건`을 확인했다.

### 씬 셋업과 고유 값 복원
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Game.unity`
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
**작업내용**
- 두 씬의 setup 응답은 `success=true`다. 구 전역·로컬 매니저는 각각 `0건`, UI 카메라 스택은 `1`, 얼굴 전환 배열은 `2`다.
- 두 씬 카메라 `orthographic size=4`를 복원하고 저장했다.
- `Scene_Game`의 플레이어 2종을 로컬 위치 `(0,-2.4,0)`·비활성 상태로 유지했다.
- `LocalPlayerCharacterManager.m_Players`는 `Object_Player_Knife`·`Object_Player_Gun` 순서의 `2개` 참조다.

### 씬 최종 검증
**산출물**
`_Temp/Work_6_J8/Scene_Game_final.png`
`_Temp/Work_6_J8/Scene_Lobby_final.png`
**작업내용**
- `unity_concept scene` 응답의 모듈·팝업과 실제 `[Global]`·`[Local]`·`[Popup]` 계층이 두 씬 모두 일치한다.
- 씬별 최종 판정은 실패 `0건`, Missing 스크립트 `0건`, 끊긴 직렬화 참조 `0건`, `[Global]` 하위 오버라이드 `0건`이다.
- Build Settings는 `Scene_Lobby`·`Scene_Game` 모두 `enabled=true`이며 컨셉의 인덱스 `0`·`1` 순서다.
- 컨셉 대조 결과는 두 씬 모두 주 요소 구도, 카드·캐릭터·HUD 크기 비율, 따뜻한 베이지와 강조색, 굵은 외곽선의 카툰 스타일, 배경·바닥 텍스처 반복 주기에 단절이 없다.
- 스크립트 변경이 없는 검증이므로 `recompile_status`는 `status=up_to_date`, `failed=false`이며 `get_console_logs`는 `0건`이다. 콘솔 버퍼 외 되돌릴 산출물은 없다.
