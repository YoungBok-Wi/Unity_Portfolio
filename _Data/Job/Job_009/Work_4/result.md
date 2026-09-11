# [유니티엔진_씬_셋업_실행] "두 씬 반영·넉백 FSM·Face 연출 검증" 업무 레포트

## 요약
- `Scene_Game`과 `Scene_Lobby`의 셋업·고유값 보존·저장을 완료했습니다.
- 최종 검증 담당으로 지정된 `유니티엔진_씬_검증` 스킬이 존재하지 않아 Missing 참조·연출·런타임·콘솔 검증은 수행하지 않았습니다.

## 완료업무

### Scene_Game 셋업과 고유값 복구
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Game.unity`
**작업내용**
- 셋업 후 삭제된 `Object_Player_Knife`·`Object_Player_Gun`을 `[Stage]` 아래에 원본 프리팹 인스턴스로 복구했습니다.
- 두 플레이어를 위치 `(0, -2.4, 0)`·비활성으로 저장하고 `LocalPlayerCharacterManager.m_Players` 두 참조를 연결했습니다.
- 셋업 전 카메라 `orthographic size=4`를 복구했습니다.

### Scene_Lobby 셋업과 고유값 보존
**산출물**
`Assets/__Game/_Core/__Scene/Scene_Lobby.unity`
**작업내용**
- 셋업 전후 카메라 `orthographic size=6.5`, `[Global]` 의미 있는 오버라이드 `0건`, 배경 계층이 같습니다.
- Face 타일 `84개`와 얼굴 스프라이트 `Assets/__Game/_Core/Icon/Icon_Casual_Face_Chef.png` GUID `9b7955a211788da4a90792cebbf2e24b` 참조를 유지했습니다.

## 비고
- 승인된 `open_scene` 우회로 닫힌 씬 조회를 수행했습니다.
- 셋업 결과 `Assets/__Game/_Core/Prefab/[Global].prefab`과 두 씬이 다시 직렬화되었습니다.

## 예외상황
- 대상 — 업무 4 `유니티엔진_씬_검증`. DataMCP 에러 원문 — `존재하지 않는 스킬: 유니티엔진_씬_검증`.
- 원본 스킬 저장소 `C:/_Projects/_WebForGameData/_Data/Workflow/Skill`에도 해당 폴더가 없으며, `유니티엔진_씬_질문`·`유니티엔진_씬_스크린샷제작` 등으로 대체하는 절차가 지시서에 없습니다.
- 검증 미수행 범위 — Missing 스크립트·참조·구 매니저·`[Global]` 오버라이드, Face 연출 캡처, 넉백 FSM 진입·곡선 이동·스턴·정상 상태 복귀, 컴파일·런타임 콘솔 오류.
