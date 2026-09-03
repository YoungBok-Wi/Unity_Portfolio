# [오케스트레이터_워커_실행] "Job_002 Work_1 컨셉 4종 갱신·씬 셋업 반영" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 1·2·3 전부 수행, `concept_manage verify` `Game`·`Balance`·`Resource`·`Scene_Lobby`·`Scene_Game` 5건 `{"success":true}`(errors 필드 없음), `editor_util setup` 두 씬 `{"success":true}`, `save_scene` 두 씬 `success:true`·`isDirty:false`, `get_console_logs --severity=error` `total: 0`, `recompile_status` `up_to_date`·`failed:false`(스크립트 변경 0건)
- `게임컨셉`(`_Data/Concept/Game/concept.md`) 추가 4건: 방 이력 상한 "최근 N개"(스크롤·압축 제외), 최고 순번 별 배지 정본 ID `Icon_Casual_Room/Best`, 무입력 정지·넉백 후 자동 정지 원칙, 방 좌우 벽 원칙. 정본 ID 목록 무변경
- `밸런스컨셉`(`_Data/Concept/Balance/concept.md`) 추가: "플레이어 공통"(피격 넉백 0.5u/0.15s, 무입력 감속 0s, 무입력 생존 목표 8~15s, HUD 이력 슬롯 N=8), "방 구조"(방 폭 24u·벽 x ±12u·적 등장 x ±10u·카메라 X 클램프 ±4.9u), 근접 슬롯 대기 3.0u 좌우 독립 명시, 검산 2건 추가
- `리소스컨셉`(`_Data/Concept/Resource/concept.md`) 추가: "화면 비율" 섹션(관계식·`orthographicSize` 4.0·계열별 화면 높이 비율 — 플레이어 12.5%·적 11.0~13.5%·보스 21.9%, 조정 주체 씬 카메라), Gun 전용 `Idle_Gun`·`Move_Gun` 제작 목록, 신규 타입 `Illust_Casual_Chef` 규격(640x960, 기준 높이 864, 로비 화면 50%), 사운드 7건 제작 목록·길이·포맷
- `씬설정` `Scene_Lobby`에 `Popup_Notify` 등재(UI·UI 상태)·요리사 일러스트·별 배지 서술, `Scene_Game`에 카메라 `orthographicSize` 4.0·벽 정본 참조·HUD 이력 상한·Gun 전용 시트 서술. 셋업 결과 `Scene_Lobby` `[Popup]`에 `Popup_Notify` 신규 인스턴스(`get_scene_hierarchy` 실측), 오버라이드 소실 두 씬 0건
- 다음 행동: `Scene_Game` 메인 카메라 `orthographicSize`를 문서값 4.0으로 바꾸는 씬 구성 작업이 남아 있다 (현재 실측 6.5 — Job_001 Work_6 기준, 이번 지시서 범위 밖)

## 완료업무

### 게임컨셉·밸런스컨셉 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_질문`(→ `게임_질문`·`밸런스_질문`으로 문서·`get` 조회, `Game`·`Balance` 모두 `reuse:"add"`·`confirmed:{"concept.md":null}` — 값 무변경) → `게임개발_구성_컨셉_게임_작성`(child `게임개발_구성_컨셉_게임_액션_작성` 적용, `방치형_작성`은 장르 불일치로 미적용) → `게임개발_구성_컨셉_밸런스_작성` → `게임_검증`·`밸런스_검증`
- `게임컨셉` 편집 위치: "핵심 컨셉"에 `방 이력 상한`·`방 경계`·`최고 순번 배지` 항목 추가, "조작 입력" 그룹에 `무입력 정지` 추가, "장르 요소" `넉백·경직`·`방 이력` 내용 보강. 수치는 전부 `밸런스컨셉` 위임 표기(정본 표기 규칙 준수)
- 별 배지 아이콘 ID 결정 근거: `_Data/Resource/File`·`Assets/__Game` 전수에 별 아이콘 없음(`find -iname "*star*"` 결과 `UI_Casual_Button_Restart.png` 1건뿐) — 로비 `Popup_Lobby`가 `Resources.Load("Icon/…")` 통로의 `Icon_Casual_Room` 타입을 이미 쓰므로 같은 타입에 `Best` 항목을 두는 것으로 확정
- `밸런스컨셉` 편집 위치: "핵심 메카닉"에 `플레이어 공통`·`방 구조` 그룹 신설, `적 그룹 공통` 근접 슬롯 문구 보강, "진행 속도"에 무입력 생존 시간 8~15s(실측 12s 범위 안), "검산"에 `무입력 생존 시간 하한`·`방 폭 대 원거리 교전 배치` 추가
- `게임개발_구성_컨셉_게임_검증` 판정: 합격, 불합격 없음(대상 항목 4). `verify Game` `{"success":true}`. 필수 판정 6항목 문서 원문 재발췌 — 진행 변화 구간(방 순번)·판 간 진행(같은 씬 내 방 구성)·일시정지(Esc·Start 팝업)·이탈 경로(포기·종료 확인)·종료 조건(보스 처치·HP 0)·화면 추적 기준(X 추적·Y 고정·경계 클램프) 전부 "확정". `unity_concept game` 대조 — `title` "Kitchen Riot"·`resolution` 1920x1080·`orientation` Landscape·`buildTarget` StandaloneWindows64 문서와 일치, `tech.techs` `{}`는 컨셉 기술 항목(Unity·URP 고정줄)과 무관한 옵션 미사용이라 정합. 정본 ID 열거 영문 단일 표기 유지. 액션 장르 표준 5항목·전투 파라미터 4항목 문서 기존 판정 유지, 넉백 항목에 플레이어 피격 넉백 채택 추가
- `게임개발_구성_컨셉_밸런스_검증` 판정: 합격, 불합격 없음(대상 항목 6). `verify Balance` `{"success":true}`. 재계산 — 무입력 하한 (10.0−0.8)/3.5 + 100/24 = 2.63 + 4.17 = 6.80s(문서 6.8s 일치), 방 폭 검산 5.0+3.0+1.0 = 9.0 ≤ 10.0(일치), 카메라 클램프 12.0−7.1 = 4.9(일치), 화면 반폭 4.0×16/9 = 7.11(문서 7.1 일치). 정본 대조 — 캐릭터 2·적 3·보스 2·방종류 4·능력 6·재화 1 전부 `게임컨셉` 정본 ID 안, 정본 값(방 선택 세트 4종·5번째 방 해금·6종 3택) 옮겨 적기 일치, 구식 값 없음. 필수 판정 — 선택지 풀 3<6 확정·진행 난이도 성장식 확정·체감 지표(처치 시간·생존 시간) 확정·개수 검산 18=18 확정
- 외부 적용: `editor_util setup` `{"success":true}` (당시 활성 씬 `Scene_Lobby`, `Scene_*.unity` 2건 실재)

### 리소스컨셉 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_리소스_작성` → `게임개발_구성_컨셉_리소스_검증`. child `게임개발_구성_컨셉_리소스_캐주얼_탑뷰_작성`은 건너뜀 — 대상: 업무 2 하위 스킬 / 조건: 하위 스킬 "처리가능"이 탑뷰 시각 규격 한정 / 실측 근거: `게임컨셉` "장르" `2D 사이드뷰`, `씬설정` 두 씬 "카메라: Side"
- 화면 높이 비율 결정: 컨셉아트 ≈40%는 미채택 — 40%면 `orthographicSize` 1.25·화면 반폭 2.2u라 `밸런스컨셉` Banana 유지 거리 5.0u·Pineapple 6.0u가 화면 밖. `orthographicSize` 4.0(반폭 7.1u)이 두 값을 화면 안에 두는 최소값이라 확정. 관계식·조정 주체(씬 카메라, 스프라이트 스케일 고정)를 "화면 비율" 섹션에 기재, 계열별 규격 "점유율"에 화면 높이 비율 병기
- 제작 목록 추가: `AnimationSheet_Casual_Player` `Idle_Gun`·`Move_Gun`(기존 `Idle`·`Move`는 Knife 전용 확정, `_Data/Resource/File/AnimationSheet_Casual_Player` 실측 항목 Idle·Move·Attack_Gun·Attack_Knife·Attack2·Attack3·Hit·Die·Jump), 신규 타입 `Illust_Casual_Chef`(Knife·Gun), `Icon_Casual_Room/Best`, 사운드 7건
- 사운드 규격: `SFX_Casual_Battle`·`SFX_Casual_Progress` `type.json` 실측(`.ogg` 44.1kHz 모노, 전투 0.3초 상한·진행 1.0초 상한, 업로드 전용)을 옮겨 적고 항목별 길이 확정. `BGM_Casual`은 `.ogg` 44.1kHz 스테레오 60~90초 루프·-16 LUFS로 신규 정의
- `게임개발_구성_컨셉_리소스_검증` 판정: 합격, 불합격 없음(대상 항목 5). `verify Resource` `{"success":true}`. 재계산 — 플레이어 128/128/8.0 = 12.5%, Apple 113/128/8.0 = 11.0%, Banana 123/128/8.0 = 12.0%, Watermelon 138/128/8.0 = 13.5%, 보스 224/128/8.0 = 21.9%, 배경 1080/128 = 8.44u → 8.0/8.44 = 0.95배, `Illust_Casual_Chef` 864/960 = 90%·540/1080 = 50% 전부 문서값 일치. 개수 검산 — 신규 제작 대상 11항목 열거 = 표기 11, 사운드 2+3+2 = 7. 정본 대조 — Knife·Gun·Apple·Watermelon·Banana·Pumpkin·Pineapple·Battle·Heal·Ability·Boss 전부 `게임컨셉` 정본 ID 안, `Icon_Casual_Room/Best`는 리소스 ID(정본 ID 대상 아님). 필수 판정 — 테마 선택(Casual)·규격 확정(7계열 수치)·개수 검산·연출 요구 전부 "확정"

### 씬설정 갱신·셋업
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Lobby\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Game\concept.md`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Lobby.unity`
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\__Scene\Scene_Game.unity`
`C:\_Projects\Unity_Portfolio\_Temp\Work_1`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_씬_작성`(child 없음 — `child` 응답 `{}`) → `게임개발_구성_컨셉_씬_검증` → `유니티엔진_씬_셋업_실행`(두 씬). `Scene_Lobby`·`Scene_Game` `get` `reuse:"add"`·`confirmed:{}` 무변경
- `Scene_Lobby` 편집: "UI"에 `Popup_Notify` 추가, "UI 상태"에 `Popup_Notify` 포함(라이브러리 기본 팝업 실재) 추가, "설명"에 요리사 일러스트·별 배지·잠금 클릭 알림 문장. `Scene_Game` 편집: "설명"에 카메라 `orthographicSize` 4.0·벽 정본 참조·무입력 정지, `Popup_HUD`·`Object_Player_Gun` 설명 보강 (모듈·UI·Object 목록 무변경)
- `게임개발_구성_컨셉_씬_검증` 판정: 두 씬 합격, 불합격 없음. `verify Scene_Lobby`·`Scene_Game` `{"success":true}`. `unity_concept scene` — `Scene_Lobby` `buildIndex` "0"·`localModule` 14종·`localPopup` Popup_Lobby·Quit·Setting·Notify(문서 UI 4종 일치)·`localObject` Object_Background; `Scene_Game` `buildIndex` "1"·`localModule` 20종·`localPopup` 7종·`localObject` 10종 문서 목록 일치. 필수 판정 — Lobby: Setting 포함·Quit 포함·Pause 제외(사유 있음), 취소 주체 `Popup_Quit` 확정 / Game: Setting·Pause 포함·Quit 제외(사유 있음), 취소 주체 `Popup_Pause` 확정. 정본 대조 — 씬 ID 2건 `게임컨셉` "씬" 목록 안, 옮겨 적은 값(방 종류·해금·이탈 경로) 일치
- 이중 통로 대조: `Camera2D` 등록 프리팹 `[LocalCameraManager]` = `Setup_Camera2D.cs:16` `InstantiatePrefabChild(_root, PrefabPath, "[LocalCameraManager]")` 동명 일치, `Popup`은 `Setup_Popup.cs`가 매니저를 만들지 않음, `Input`은 프리팹 없음 — 이중 생성 대상 없음
- `Scene_Lobby` 셋업: 시작 상태 `list_open_scenes` `Scene_Lobby isDirty:true`(업무 1 setup 잔여) → 오버라이드 기록(17행) → `setup` `{"success":true}` → 오버라이드 18행, 소실 0건·추가 1건(`[Popup]/Popup_Notify` `CanvasScaler.m_ReferenceResolution`, 신규 인스턴스 자체 값) → `[Popup]` 하위 Popup_Lobby·Quit·Setting·Notify 4종, `[Local]` 매니저 Camera·Input·Popup·Character 실재, `Main Camera`(`/[Local]/[LocalCameraManager]/PosRoot/RotRoot/Main Camera`) `m_Cameras[0]` = `/[Local]/[LocalPopupManager]/UICamera` → `get_console_logs --severity=error` `total:0` → `save_scene` `success:true`
- `Scene_Game` 셋업: `open_scene` → `isDirty:false` → 오버라이드 41행 기록 → `clear_console` → `setup` `{"success":true}` → 오버라이드 41행 소실 0·추가 0 → `[Local]` Camera·Input·Popup·Battle·Character·Room 매니저·`[Popup]` 7종·`[Stage]` Object_Background·Object_Floor 실재(구분선 배치 유지, 두 번째 구분선 아래 신규 오브젝트 없음), `m_Cameras` 길이 1 → 에러 0건 → `save_scene` `success:true`·`isDirty:false`
- 컴파일 검증: `clear_console` → `recompile` → `recompile_status` `up_to_date`·`failed:false` → 에러 0건 (이번 Work 스크립트·에셋 변경 0건이라 `up_to_date` 합격)

## 비고
- `Scene_Game` 메인 카메라 실제 `orthographicSize`는 이번 Work에서 바꾸지 않았다 — 지시서 업무 3이 씬설정 문서·셋업 반영까지라 값 적용(씬 구성)은 범위 밖. 문서값 4.0과 씬 실측 6.5(Job_001 Work_6 기준, 이번 미재측)가 어긋난 상태
- `BGM_Casual`은 `_Data/Resource/File/BGM_Casual`에 `type.json`이 없는 빈 폴더 — 지시서의 "기존 타입" 전제와 불일치. 문서에 규격만 정했고 타입 등록·업로드는 리소스 계열 몫
- 컨셉아트 `Concept_Scene_Game`(플레이어 ≈40%)·`Concept_Scene_Lobby`(요리사·별 배지)는 확정 규격과 어긋나 재생성 대상 — 이번 Job 주의사항대로 컨셉아트 체인은 미수행
- 업무 1의 `editor_util setup`(스킬 절차 3)은 `Scene_Lobby`가 열린 채 실행되었고 사전 오버라이드 기록 절차가 그 스킬에 없어 당시 소실 여부는 미확인 — 업무 3 셋업 전 기록(17행)에 `[Popup]` 텍스트 색·`CanvasScaler` 오버라이드가 남아 있어 실질 소실은 없는 것으로 판단
- 골격 생성 후속(테이블·고정값·씬 파일·모듈·프리셋·컨셋아트) 건너뜀 — 대상: `게임개발_구성_컨셉_게임_작성` 후속 체인 / 조건: 지시서 주의사항 "이미 완료된 골격 생성은 대상이 없으면 건너뛴다" / 실측 근거: `Scene_*.unity` 2건 실재, `unity_concept scene` 모듈·팝업·오브젝트 목록이 문서와 일치, 정본 ID 목록 무변경
- 저장 데이터·코드·프리팹 무변경. 임시 산출물은 `_Temp/Work_1/`(오버라이드 전후 JSON 4건·계층 JSON 2건)
