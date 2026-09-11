# [유니티엔진_씬_질문] "두 씬 셋업·런타임 최종 검증" 업무 레포트

## 요약
- `Scene_Lobby`만 열린 상태에서 `Scene_Game`의 setup 전 계층 조회가 실패해 이후 셋업·복원·런타임 검증을 수행하지 않았습니다.
- Unity 명령 정의는 `get_scene_hierarchy`가 열린 씬만 조회한다고 명시하지만, 씬 질문 스킬은 닫힌 대상 씬을 여는 절차 없이 `--path` 조회를 지시합니다.
- 이번 세션에 허용된 우회는 `resource_type patch` 전체값 전달뿐이므로 다른 수단으로 씬을 열지 않고 중단했습니다.

## 완료업무

### 두 씬 파일과 열린 상태 확인
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Game.unity`
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
**작업내용**
- 두 씬 파일이 존재하고 현재 열린 씬은 `Scene_Lobby`, `isDirty=false`임을 확인했습니다.
- `get_scene_hierarchy` 명령 스키마가 조회 대상을 열린 씬으로 제한함을 확인했습니다.

## 비고
- 업무 2~4는 업무 1의 setup 전 실측값이 없어 씬 고유 값을 안전하게 보존할 수 없으므로 수행하지 않았습니다.

## 예외상황
- 대상 — 업무 1 `유니티엔진_씬_질문`, `Scene_Game`. 에러 원문 — `Pipeline server returned 400 Bad Request: Command Execution Failed. Command 'get_scene_hierarchy' failed: Scene 'Assets/__Game/_Core/__Scene/Scene_Game.unity' is not open.`
- 막힌 지점 — 스킬은 `get_scene_hierarchy --path={대상 씬 에셋 경로}`를 지시하지만 명령 스키마는 `Path of the open scene to snapshot`으로 닫힌 씬을 지원하지 않고, 스킬에는 `open_scene` 절차가 없습니다. `Scene_Game`을 여는 우회를 승인받기 전에는 진행할 수 없습니다.
