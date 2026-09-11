# [유니티엔진_씬_질문] "씬 셋업·참조 이관·검증" 업무 레포트

## 요약

- `Scene_Game` 셋업 전 계층·직렬화 필드 스냅샷은 작성됐으나 `Scene_Lobby` 조회 단계에서 스킬 절차와 Unity CLI 실제 동작이 불일치해 Work를 실패 처리했다.
- 씬 셋업·참조 이관·검증은 선행 스냅샷 미완료로 실행하지 않아 씬 파일을 변경하지 않았다.

## 완료업무

### Scene Game 셋업 전 스냅샷
**산출물**
`_Temp/Work_6_J8/before_Scene_Game.json`
**작업내용**
- `get_scene_hierarchy`로 루트 6개와 전체 하위 계층을 기록했다.
- `get_serialized_fields`로 카메라, 플레이어 스폰, 로컬 매니저, 씬 전환 매니저, 게임·데이터 매니저 값을 기록했다.
- `get_build_settings`의 `scenes`에서 `Scene_Lobby`와 `Scene_Game`이 모두 `enabled=true`임을 확인했다.

## 비고

- 대상 — 업무 2 `유니티엔진_씬_셋업_실행`, 업무 3 `유니티엔진_씬_구성`, 업무 4 `유니티엔진_씬_검증`.
- 조건 — 업무지시서 순번상 업무 1의 두 씬 기준 스냅샷이 완료된 뒤 수행해야 하나 `Scene_Lobby` 조회가 실패했다.
- 실측 근거 — `_Temp/Work_6_J8/before_Scene_Game.json`만 생성됐고 `before_Scene_Lobby.json`은 생성되지 않았다.

## 예외상황

- 대상 — 업무 1 `유니티엔진_씬_질문`의 `Scene_Lobby` 전체 계층 조회.
- 막힌 지점 — 스킬은 `unity --format json cmd get_scene_hierarchy --path={대상 씬 에셋 경로}`로 씬 에셋 조회를 지시하지만, 실제 명령은 미개방 씬을 조회하지 못한다. `error.md`에는 처리 절차가 없다.
- 에러 원문 — `Pipeline server returned 400 Bad Request: Command Execution Failed. Command 'get_scene_hierarchy' failed: Scene 'Assets/__Game/_Core/__Scene/Scene_Lobby.unity' is not open.`
- 사용자 확인 요청 — `Scene_Lobby`를 임시로 열어 동일 조회를 수행하는 예외를 허용하거나, 다른 프로젝트에서 스킬·CLI 불일치를 수정한 뒤 재개해야 한다.
