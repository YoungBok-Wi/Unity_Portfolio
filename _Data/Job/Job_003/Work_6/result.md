# [오케스트레이터_워커_실행] "Job_003 Work_6 씬 오버라이드 재적용·검증" 업무 레포트

## 요약
- Work 판정: 합격 — `Scene_Lobby` 카메라 `orthographic size` 6.5→4.0 재적용·저장(`save_scene` `success:true`, `.unity` 245행 `orthographic size: 4`), `Scene_Game`은 정본 유지 확인(카메라 4·`PlayerSpawn`(0,−2.4)·`SpawnLeft`(−10,−2.4)·`SpawnRight`(10,−2.4), `get_serialized_fields` 실측)이라 무편집
- 소멸 실측: 두 씬 중 `Scene_Lobby`만 소멸(`.unity`에 `orthographic size` 오버라이드 없음, 조회 6.5 = 프리팹 기본값). Work_3 `editor_util setup`이 `Scene_Lobby`를 열고 실행돼 로비 `[Local]`만 재인스턴스화된 결과. 프리팹 원본 경로(`Assets/_Library/Camera2D/Prefabs/[LocalCameraManager].prefab`)는 수정 금지 영역이라 씬 오버라이드 유지(`## 비고`)
- 씬 검증(scope `Scene_Game`·`Scene_Lobby`): 불합격 없음(대상 2). `recompile` `up_to_date`·`failed:false`(스크립트 변경 0건 — `Game.dll` 16:34·`Library.dll` 15:33 갱신, 이번 Work 파일 변경 없음), `get_console_logs --severity=error` `total:0`(컴파일 후·플레이 3회 종료 시 각 1회, 매회 `clear_console` 선행), `[Global]` 오버라이드 두 씬 0건(`PrefabUtility.GetObjectOverrides` 실측), `unity_concept scene` 등재 모듈·팝업·오브젝트가 `get_scene_hierarchy` 계층과 일치, Build Settings Lobby 0·Game 1 `enabled:true`
- 플레이 실측(3회, `playing`→`stopped`, 종료 후 두 씬 `isDirty:false`): 로비 BGM `[BattleManager] clip=BGM_Casual_Lobby playing=True loop=True`, 게임 진입 `BGM_Casual_Battle` / 카메라 두 씬 `cam=4 aspect=1.778 screen=1920x1080` / 적 발 위치 Apple 3마리·Pumpkin·Pineapple 전건 `y −2.385 colMin −2.435 fly=None`(`floorTop −2.450`), 플레이어 `y −2.395 colMin −2.445` / 전조 `Telegraph(Clone)` y −2.39 = 보스 발 y, 월드 3.00×1.05(Slam)·2.00×0.70(Rain) 가로 타원, 캡처 `boss_tel_1.png`에 바닥선 위 가로 타원 2개 실재
- 컨셉아트 대조 5종 두 씬 전건 기록·평균 RGB 수치 대조 기록(`## 완료업무`)
- 산출물: 캡처 `_Temp/Work_6/cap/`(lobby_1·game_1·game_2·boss_tel·boss_tel_1~3), 계층 `_Temp/Work_6/hier_game.json`, 헬퍼 `_Temp/Work_6/qa.sh`
- 다음 행동: 없음 — 마지막 안정화 루프 완료. `Scene_Lobby.unity`가 git 작업 트리 수정 상태(커밋은 오케스트레이터 몫)

## 완료업무

### 씬 오버라이드 재적용·검증
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Lobby.unity`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\qa.sh`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\hier_game.json`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\cap\lobby_1.png`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\cap\game_2.png`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6\cap\boss_tel_1.png`
**작업내용**
- 수행 스킬: `유니티엔진_씬_질문`(조회) → `유니티엔진_씬_구성`(child `{}`, 로비 카메라 재적용) → `유니티엔진_씬_검증`. 셋업 미수행(컨셉·모듈 등재 변경 없음)
- 조회 실측(편집 전): `Scene_Lobby` `/[Local]/[LocalCameraManager]/PosRoot/RotRoot/Main Camera` `Camera` `orthographic size` 6.5, `.unity`에 해당 `propertyPath` 없음(소멸) / `Scene_Game` 같은 경로 4(`.unity` 1321행 `value: 4`), `/[Local]/[LocalRoomManager]/PlayerSpawn`·`SpawnLeft`·`SpawnRight` `m_LocalPosition` (0,−2.4,0)·(−10,−2.4,0)·(10,−2.4,0) — 정본 일치, 무편집
- 재적용: `Scene_Lobby` 열기 → `set_serialized_field --component=Camera --field="orthographic size" --value=4.0` 응답 `parameters` 에코 `value:4` → 재조회 `[4]` → `save_scene` `success:true` → `list_open_scenes` `isDirty:false`. 정의처 판정: 두 씬 공통값이라 정본은 프리팹이나 원본이 `Assets/_Library`(수정 금지)라 "Global 오버라이드 금지" 예외로 `[Local]` 오버라이드 유지(Job_002 Work_6과 같은 경로)
- `[Local]` 잔존 오버라이드(비Transform, 두 씬 동일 2건): `[LocalCameraManager]` `Main Camera`의 `Camera`·`UniversalAdditionalCameraData` — 의도된 카메라 크기 오버라이드(후자는 URP가 Camera 편집 시 함께 기록). `[Global]` 0건
- 시야 좌표 판정(orthographicSize 4.0·aspect 1.778 → 반폭 7.11·반높이 4.0, 카메라 y 0): 플레이어 스폰 (0,−2.4) 안, 적 스폰 ±10은 시작 시야 밖(카메라 클램프 이동 시 노출), 바닥 y −2.4 이하 하단 20% 덮음(캡처 game_2 타일 띠 시작 row 866/1080 = 80.2%), 로비 배경 1장(±9.6·±5.4)이 시야(±7.11·±4.0) 덮음
- 컨셉아트 대조 `Scene_Game`(캡처 `game_2.png` vs `_Data/Resource/File/Concept_Scene_Game/Overview/art/1.png`): 구도 일치(HUD 좌상 HP·중앙 방 순번·우상 재화, 바닥선 하단 20%) / 크기 비율 일치(플레이어 sprH 2.0u = 화면 높이 25%, 적 Apple 2.0u 동급 — `리소스컨셉` 서열 보스 3.0u > 플레이어·적) / 색 대조(배경 상단 60% 평균 RGB 컨셉 (202,193,179) 휘도 194 vs 캡처 (173,164,154) 휘도 165 — 저채도 웜그레이 동계열, 캡처가 15% 어두움) 일치 / 스타일 일치(둥근 벡터 외곽선) / 텍스처 반복 주기 대상 아님(컨셉 바닥 무늬 없음; 실측 타일 2.6u 반복, 바닥 하단 20% 평균 RGB 캡처 (232,233,212) vs 컨셉 (138,126,114) — 컨셉의 조리대 회갈색과 다름, `리소스컨셉` 타일 에셋 채택값이라 그림 쪽 미적용)
- 컨셉아트 대조 `Scene_Lobby`(캡처 `lobby_1.png` vs `Concept_Scene_Lobby/Overview/art/1.png`): 구도 일치(별 배지 좌상·카드 좌우·요리사 중앙·시작 하단·설정 우상) / 크기 비율 일치(요리사 ≈45% 화면 높이, 카드 폭 ≈23%) / 색 일치(선택 카드 캡처 (74,158,218) vs 컨셉 (119,166,193) 파랑 계열, 시작 버튼 (161,202,90) vs (153,210,126) 초록 계열, 배경 상단 휘도 226 vs 239) / 스타일 일치 / 텍스처 반복 주기 대상 아님(배경 1장). 잠금 카드 회색·자물쇠는 저장값 해금 상태라 미표시(대조 제외)
- 캡처 대조(주요 요소 실재): Game — 배경 주방·화구 불·바닥 타일·플레이어·Apple 3·HUD HP·순번·재화 전부 그려짐 / Lobby — 배경·별 배지·두 카드·요리사·시작·설정 전부 그려짐 / Boss — 보스룸 배경·HUD 이력 8칸·플레이어·바닥선 위 붉은 가로 타원 전조 2개 그려짐(`boss_tel_1.png`, `Telegraph` alpha 0.35)
- 로비 BGM·전조·적 발 위치는 Work_3·Work_5_2 실측값과 동일(위 요약 실측값), 재수정 없음

## 비고
- `[Local]` 카메라 오버라이드는 다음 `editor_util setup`이 그 씬을 열고 실행되면 다시 소멸한다 — 복구는 이 레포트 "재적용" 절차(`set_serialized_field` `orthographic size` 4.0 → `save_scene`). 근본 해소는 `Assets/_Library/Camera2D/Prefabs/[LocalCameraManager].prefab` 기본 6.5→4.0(수정 금지 영역이라 미수행)
- `Scene_Game` 스폰 위치는 Work_3 셋업에서 소멸하지 않았다 — 셋업이 활성 씬(`Scene_Lobby`)만 재인스턴스화한 결과로 판단(실측: `Scene_Game.unity` 오버라이드 잔존). `[LocalRoomManager].prefab` 기본값(0,−3.9)·(±9,−3.9)→(0,−2.4)·(±10,−2.4) 변경은 여전히 모듈 프리팹 수정 몫(미수행)
- 에디터 플레이 캡처 우하단 로그 배지(`DebugConsole`)는 `LogManager.InitFirst`의 에디터 전용 표시(Job_002 Work_6 비고) — 대조 제외
- 첫 플레이(`game_1.png`)는 무입력 24초에 `hp=0 state=Ended`(Work_4_2 목표 범위 내 사망)라 `Popup_Result` 상태 — 대조는 회복 코루틴을 건 `game_2.png` 사용
- `_Temp/Work_6/cap/`에 이번 Work 이전 파일(`01_result_lose.png`~`39_gun_fire.png`, 09:58~10:33 생성)이 있었음 — 이번 산출물은 `lobby_1`·`game_1`·`game_2`·`boss_tel*`뿐
- `confirmed`·`reuse` 무변경, 사용자 질문 없음, DataMCP 전 호출 정상 응답(`Fallback` 미사용), 코드·프리팹·데이터 무변경
