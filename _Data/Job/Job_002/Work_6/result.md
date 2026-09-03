# [오케스트레이터_워커_실행] "Job_002 Work_6 Scene_Game 카메라 비율 교정·디버그 콘솔 숨김·컨셉아트 대조" 업무 레포트

## 요약
- Work 판정: 합격 — `Scene_Game`·`Scene_Lobby` 편집 후 `save_scene` 두 씬 `success:true`, 종료 시 `list_open_scenes` 두 씬 `isDirty:false`, `recompile_status` `up_to_date`·`failed:false`(스크립트 변경 0건), `get_console_logs --severity=error` `total:0`(컴파일 후·두 씬 플레이 중 각 1회, 매회 `clear_console` 선행), 두 씬 플레이 진입 `editor_status playMode:playing`·종료 `stopped` 실측
- `Scene_Game`: 메인 카메라 `orthographic size` 6.5→4.0(플레이 실측 `cam=4 aspect=1.778`), 바닥선 y −3.9→−2.4(화면 하단 20%), 스폰 3종 `PlayerSpawn`(0,−2.4)·`SpawnLeft`(−10,−2.4)·`SpawnRight`(10,−2.4), 배경 `Object_Background/View` Scale (2.6,1.6)→(1,1,1)·Draw Mode Simple→Tiled·Size (57.6,10.8). 플레이 실측 카메라 클램프 ±4.89, 벽 `WallLeft` x −12.5, 배경 bounds x ±28.8·y ±5.4(시야 ±12.0·±4.0 전부 덮음), 플레이어 화면 높이 ≈12.5%(캡처 135px/1080)
- `Scene_Lobby`: 메인 카메라 `orthographic size` 6.5→4.0, 배경 Scale (2.6,1.6)→(1,1,1) (플레이 실측 `Illust_Casual_Background_Lobby` bounds ±9.6·±5.4, 시야 ±7.1·±4.0 덮음)
- 디버그 콘솔: 두 씬 `[Global]/[LogManager]/DebugConsole` 씬 인스턴스는 이미 `activeSelf:false`(편집 불필요). 노출은 라이브러리 `LogManager.InitFirst`의 `#if UNITY_EDITOR || DEVELOPMENT_BUILD` `SetActive(true)`(개발용 토글)가 결정 — 릴리스 빌드에서 숨김, 에디터 플레이에서는 표시(실측 `debugConsoleActive=True`). 씬 편집으로는 숨길 수 없음(`## 비고`)
- 컨셉아트 대조 5종 두 씬 전건 기록(`## 완료업무`), `[Global]` 오버라이드 두 씬 0건(`PrefabUtility.GetObjectOverrides` 실측), Build Settings 등재 Lobby 0·Game 1 유지
- 산출물: 캡처 `_Temp/Work_6_J2/cap/`(game_1·game_2_right·game_3_left·lobby_1), 계층 JSON `_Temp/Work_6_J2/hier_game.json`·`hier_lobby.json`
- 다음 행동: `Scene_Game` 진입 후 무입력 시 플레이어가 약 70초 안에 사망하는 밸런스 결함(Work_5 비고 기재, 이번 플레이 `hp=0 state=Ended`)이 남아 있다

## 완료업무

### 씬 구성
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Game.unity`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Lobby.unity`
`C:\_Projects\Unity_Portfolio\_Temp\Work_6_J2`
**작업내용**
- 수행 스킬: `유니티엔진_씬_질문`(두 씬 `get_scene_hierarchy`·`get_serialized_fields`) → `유니티엔진_씬_구성`(child 응답 `{}`) → `유니티엔진_씬_검증`. 셋업 미수행(컨셉·모듈·팝업 등재 변경 없음 — `unity_concept scene` `localModule`·`localPopup`·`localObject`가 씬설정 목록과 일치)
- 조회 실측(편집 전): `Scene_Game` 카메라 6.5·`m_CameraFixedY 0`·`Object_Floor` y −3.9·스폰 x ±9·배경 Simple Scale (2.6,1.6)·바닥 Tiled Size (185,8) Scale 0.325 / `Scene_Lobby` 카메라 6.5·배경 Simple Scale (2.6,1.6). 배경 에셋 `Illust_Casual_Background_*.png` 1920x1080 `spritePixelsToUnits 100`(=19.2x10.8u), 바닥 `Illust_Casual_Tile_Kitchen.png` 1024x1024 PPU 128
- 카메라 4.0 근거: `_Data/Concept/Resource/concept.md` "화면 비율"·`_Data/Concept/Scene_Game/concept.md` "설명". `set_serialized_field --field="orthographic size"` 응답 `parameters` 에코 확인 후 `get_serialized_fields` 재조회 `4`
- 바닥선 −2.4 근거: 카메라 Y 고정 0(`[LocalRoomManager]` `m_CameraFixedY 0`, 프리팹 값 유지) + 화면 −4.0..4.0의 하단 20% = 1.6u. `Object_Floor` y −2.4(콜라이더 top 플레이 실측 `floorTop=-2.40`), 바닥 타일 −2.4..−5.0 덮음(캡처 game_1 바닥 띠 하단 20%). 스폰 y −2.4, 적 스폰 x ±10(`밸런스컨셉` "방 구조", 지시서 확정값)
- 배경 반복 배치: Tiled·Scale 1·Size (57.6,10.8) = 가로 3장(±28.8, 시야 ±12.0 덮음)·세로 1장(±5.4, 시야 ±4.0 덮음). 이음선 x ±9.6은 클램프 끝(카메라 ±4.89)에서 화면 안(캡처 game_2_right·game_3_left) — 반복 배치의 한계. 로비는 시야 ±7.1이 1장(±9.6) 안이라 Simple 유지
- 바닥 타일: 셀 크기 미변경(8u×0.325 = 2.6u, `리소스컨셉` 규격에 `Illust_Casual_Tile` 셀 크기 항목 없음 — `## 비고`). 위치만 이동, Size (185,8)·Scale 0.325 유지(`get_serialized_fields` 재조회)
- 시야 좌표 판정(orthographicSize 4.0·aspect 1.778 → 반폭 7.11): 플레이어 스폰 (0,−2.4) 안, 적 스폰 ±10은 시작 시야 밖(클램프 이동 시 ±12.0까지 보임), 바닥 y −2.4..−5.0 하단 덮음, 배경 ±28.8·±5.4 전부 덮음. 플레이 실측 `campos=(4.89,0)`·`(-4.89,0)`에서 좌우 끝 캡처 빈 영역 없음
- 컨셉아트 대조 `Scene_Game`(캡처 game_1 vs `_Data/Resource/File/Concept_Scene_Game/Overview/art/1.png`): 구도 일치(HUD 좌상 HP·중앙 순번+이력·우상 재화, 바닥선 컨셉 78%·실측 80%) / 크기 비율 불일치(고친 값: 플레이어 7.7%→12.5%, 컨셉 ≈40%는 `리소스컨셉`이 미채택 확정) / 색 일치(저채도 배경·적 원색) / 스타일 일치(둥근 벡터 외곽선) / 텍스처 반복 주기 대상 아님(컨셉 바닥은 무늬 없는 조리대, 실측 타일 2.6u 반복)
- 컨셉아트 대조 `Scene_Lobby`(캡처 lobby_1 vs `Concept_Scene_Lobby/Overview/art/1.png`): 구도 일치(별 배지 좌상·카드 좌우·요리사 중앙·시작 하단·설정 우상) / 크기 비율 일치(요리사 ≈50% 화면 높이, 카드 폭 440) / 색 일치(선택 카드 파랑·시작 초록) / 스타일 일치 / 텍스처 반복 주기 대상 아님(배경 1장). 잠금 카드 회색·자물쇠는 저장값 `gunUnlocked:true`라 미표시(대조 제외)
- 캡처 대조(`ScreenCapture.CaptureScreenshot` 플레이 중, `Screen 1920x1080`): Game — 배경 주방·화구 불·바닥 타일·플레이어·Apple 3·HUD 하트·순번·이력·재화 전부 그려짐 / Lobby — 배경·별 배지·두 카드·요리사·시작·설정 전부 그려짐. 색 수치 대조 미수행(대조 항목이 요소 존재·비율 중심이라 평균 RGB 계측 생략)
- 씬 검증(`유니티엔진_씬_검증`, scope Scene_Game·Scene_Lobby): 불합격 없음(대상 2). `unity_concept scene` 두 씬 모듈·팝업·오브젝트 등재 일치, `get_scene_hierarchy` `[Local]` 매니저 인스턴스 실재, 편집 필드 재조회 요구값 일치, `recompile` `up_to_date`·에러 0, `[Global]` 오버라이드 0건, `get_build_settings` Lobby·Game `enabled:true`

## 비고
- `[Local]` 오버라이드 잔존(의도): `Scene_Game`·`Scene_Lobby` `Main Camera` `orthographic size 4.0`(프리팹 `Assets/_Library/Camera2D/Prefabs/[LocalCameraManager].prefab` 기본 6.5), `Scene_Game` `[LocalRoomManager]` `PlayerSpawn`·`SpawnLeft`·`SpawnRight` 위치(프리팹 `Assets/__Game/Room/Prefab/[LocalRoomManager].prefab` 기본 (0,−3.9)·(±9,−3.9)). 씬마다 다른 배치값이라 씬에 둠. 소멸 조건: `editor_util setup` 실행 시 — 복구는 위 값 재적용. 근본 해소는 `[LocalRoomManager].prefab` 스폰 기본값을 (0,−2.4)·(±10,−2.4)로 바꾸는 모듈 프리팹 수정(정본 `밸런스컨셉` ±10u)
- 디버그 콘솔 숨김 미완(에디터 한정): `Assets/_Library/Log/Script/LogManager.cs` `InitFirst`가 에디터·개발 빌드에서 `m_DebugConsole.SetActive(true)` 강제 — 씬 인스턴스 비활성만으로는 숨겨지지 않음. 릴리스 빌드는 자동 숨김. 에디터에서도 팝업 배지를 없애려면 라이브러리 프리팹 `[LogManager].prefab` `DebugLogManager.popupVisibility` 0(Always)→2(Never) 변경 필요(`toggleKey 96` 백쿼트 토글 유지) — `_Library` 수정 금지 영역이고 `[Global]` 오버라이드 금지라 미수행
- `리소스컨셉` 결손 2건: `Illust_Casual_Tile` 셀 월드 크기 규격 없음(바닥 Scale 0.325·셀 2.6u는 기존값 유지 — 스킬 "Scale 1" 항목 불충족 상태로 남김) / 배경 규격 "15.0u × 8.44u(PPU 128)·세로 0.95배"가 실제 에셋 PPU 100(19.2x10.8u)과 불일치 — 이번 배치는 실제 에셋 크기 기준·Scale 1, 0.95배 미적용
- 배경 이음선: 3장 반복이라 x ±9.6에 이음선이 있고 카메라 클램프 끝에서 화면 안에 들어옴(캡처 game_2_right x≈1600px). 무이음 배경은 방 폭 24u+화면 반폭을 덮는 단일 에셋 제작(리소스 계열) 몫
- `Scene_Game` 플레이 중 무입력 사망(`hp=36` at 21s → `hp=0 Ended` 75s) — Work_5 비고와 같은 Battle·Room 모듈/밸런스 범위, 이번 미수정. 게임 씬 캡처 game_2·game_3은 `Popup_Result` 열린 상태
- 두 씬 `.unity`가 git 작업 트리에서 수정 상태(이번 Work 편집). `confirmed`·`reuse` 무변경, DataMCP 전 호출 정상 응답(`Fallback` 미사용), 저장 데이터·코드·프리팹 무변경
