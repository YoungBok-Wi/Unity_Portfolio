# [오케스트레이터_워커_실행] "Job_003 Work_1 컨셉 결손 보완·피벗 정본 확정" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 2건 완료, `concept_manage verify` Game·Balance·Resource 3건 전부 `{"success":true}`(errors 부재), `confirmed`·`reuse` 무변경(3건 모두 `reuse:"add"`·`confirmed:{"concept.md":null}` 조회값 그대로), 라이브러리·코드·씬·프리팹 무변경
- `게임컨셉`(`_Data/Concept/Game/concept.md`): 보스 전조 형태 "바닥에 납작한 가로 타원(폭 > 높이, 중심은 발밑 바닥 y)" 확정(핵심 컨셉 보스 공통·장르 요소 보스 전조 2곳), 적 접촉 규칙 "서로 밀지 않음 — 겹침 허용" 신설(조작 입력 그룹)
- `밸런스컨셉`(`_Data/Concept/Balance/concept.md`): 무입력 생존 목표 캐릭터별 정의 — Knife 8~15s 유지, Gun 6~12s(Knife 목표 × HP 비율 0.8, 실측 6.7s 범위 안), 적 등장 위치 "기준 ±10.0u, 마리당 오프셋 포함 |x| ≤ 11.0u 클램프·벽 밖 등장 금지", 검산 2건 추가(Gun 하한 5.96s, 등장 위치 11.0u = 11.0u)
- `리소스컨셉`(`_Data/Concept/Resource/concept.md`): 피벗 정본 = "잉크 접지선" — 플레이어 (0.5, 0)·적 (0.5, 0.28)·보스 (0.5, 0) 실측 확정, 콜라이더 오프셋 조정 주체 프리팹(발 콜라이더 하단 = 피벗 − 0.05u, `Object_Floor` 콜라이더 상단 = 시각 상단 − 0.05u), 타일 셀 2.6u 규격 신설, 배경 PPU 100 실측 정합
- 건너뜀 2건(사유는 `## 비고`): `editor_util setup`(씬설정 무변경), 골격 생성 후속 체인(대상 없음)
- 다음 행동: 후속 Work에서 적 시트 재임포트 피벗 (0.5, 0.28)·프리팹 콜라이더 offset (0, 0.45)·`Telegraph.prefab` 가로 타원·`LocalRoomManager` 스폰 클램프를 이 정본대로 적용한다

## 완료업무

### 게임컨셉·밸런스컨셉 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_질문`(선행 — `concept_manage list` Global 3·Scene 2, `get` Game·Balance `reuse:"add"`·`confirmed:null`) → `게임개발_구성_컨셉_게임_작성`(child `게임개발_구성_컨셉_게임_액션_작성` 선택 — 보스 문법·접촉 규칙이 액션 장르 몫) → `게임개발_구성_컨셉_밸런스_작성` → 게임·밸런스 검증 스킬 완료조건 수행
- `게임컨셉` 변경 3곳: 핵심 컨셉 "보스 공통"(전조 형태 가로 타원), 조작 입력 "적 접촉" 신설(겹침 허용 — 적↔플레이어 물리 충돌 해소 없음, 접촉 피해는 정지 거리·판정 트리거만, 플레이어 위치는 이동 입력·피격 넉백으로만 변화), 장르 요소 "보스 전조"(세로 타원·부양 표시 금지). 정본 ID 무변경
- `밸런스컨셉` 변경 4곳: 플레이어 공통 "무입력 생존 시간 목표" 캐릭터별(Knife 8~15s, Gun 6~12s = 8×0.8=6.4 내림 6·15×0.8=12), 방 구조 "적 등장 위치"(±10.0u 기준 + 오프셋 |x| ≤ 11.0u = 벽 12.0u − 개체 최소 간격 1.0u), 진행 속도 지표(Knife 실측 8.1~12s·Gun 실측 6.7s 범위 안), 검산 "무입력 생존 시간 하한 (Gun)"·"적 등장 위치 벽 안쪽" 추가. `게임컨셉` 정본 값 무개정
- 게임 검증(스킬 `게임개발_구성_컨셉_게임_검증` 완료조건): verify success. 필수 판정 6항목 원문 재발췌 전부 확정(진행 변화 구간 "방 순번"·판 간 진행 "같은 씬 안 다음 방"·일시정지 "Esc·Start 팝업"·이탈 경로 "포기→로비, 종료 확인→앱 종료"·종료 조건 "보스 처치/HP 0"·화면 추적 "X 추적·Y 고정·클램프"). `unity_concept game` 대조: title "Kitchen Riot"=문서 제목, resolution 1920x1080=문서 21행, orientation Landscape=문서 22행 일치, tech `{}`(문서 Unity·URP는 템플릿 고정줄). 정본 ID 별칭·한글 병기 0건. 불합격 없음(대상 3항목)
- 밸런스 검증(스킬 `게임개발_구성_컨셉_밸런스_검증` 완료조건): verify success. 필수 판정 — 수치 하한(선택지 풀 3<6·진행 난이도 성장식) 확정, 체감 지표(무입력 생존 Knife·Gun, 처치 시간) 확정, 개수 검산(정본 ID 18=18) 확정. 독립 재계산: Gun 하한 (10.0−0.8)/3.5 + 80/24 = 2.629 + 3.333 = 5.96s(문서 5.96 일치), Knife 하한 2.629 + 4.167 = 6.80s(일치), 등장 위치 10.0+1.0 = 11.0 = 12.0−1.0(일치), Gun 목표 6.4→6·12(일치). 정본 대조: Knife HP 100·Gun HP 80·벽 ±12.0u·화면 내 최대 적 8은 문서 내 값과 쌍별 일치, 사용 ID 18종 전부 `게임컨셉` 정본 목록 안·밖 ID 0건. 불합격 없음(대상 6항목)

### 리소스컨셉 갱신
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
`C:\_Projects\Unity_Portfolio\_Temp\Work_1\ink.py`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_리소스_작성`(child `게임개발_구성_컨셉_리소스_캐주얼_탑뷰_작성`은 탑뷰 전용이라 미선택 — search.md "캐주얼 탑뷰 시각 규격", 본 문서는 사이드뷰) → `게임개발_구성_컨셉_리소스_검증` 완료조건 수행
- 잉크 접지선 실측(`_Temp/Work_1/ink.py`, `_Data/Resource/File/AnimationSheet_Casual_*` 147프레임 알파>32 bbox): Player 11동작 전 프레임 잉크 하단 = 255행(캔버스 256 최하단) → 피벗 y 0, Enemy 3종 9동작 전 프레임 = 183행(하단에서 72px, 72÷256 = 0.281) → 피벗 (0.5, 0.28), Boss 2종 10동작 전 프레임 = 383행(캔버스 384 최하단) → 피벗 y 0
- 임포트 실측(`Assets/__Game/_Core/Resources/SpriteAnim/*.png.meta`): 3계열 전부 `alignment: 7`(BottomCenter)·`spritePixelsToUnits: 128` — 플레이어·보스는 정본과 일치, 적만 불일치(72px = 0.5625u 부유, Job_002 Work_7 실측 0.56u와 일치). `_Editor/Editor/Script/AutoTextureSettingOnImport.cs`는 피벗을 바꾸지 않음
- 접지 규칙 정본(`Assets/_Library/CharacterPhysics/Script/CharacterPhysics2DSide.cs` 104~136행 `0 < avgNor.y && avgPos.y < transform.position.y`, 라이브러리 무수정): 접촉점이 피벗보다 아래여야 하므로 "피벗 = 잉크 접지선 + 프리팹 발 콜라이더 하단 = 피벗 − 0.05u"로 확정. 현재 `Object_Player_Knife.prefab`·`Object_Player_Gun.prefab` 발 콜라이더 offset (0, 0.5)·size (0.6, 1)은 하단 = 피벗(0)이라 조건 불성립 → 정본 offset (0, 0.45). 바닥 시각선 정합은 `Object_Floor.prefab` 콜라이더 상단을 시각 상단 − 0.05u로 명시
- 규격 결손 보완: 타일 `Illust_Casual_Tile/Kitchen` 1024x1024·PPU 128 = 8.0u, `Object_Floor.prefab` View 스케일 0.325·타일 드로우 size (185, 8) → 셀 2.6u·가로 60.1u(23셀)·두께 2.6u(1셀) 실측으로 규격 섹션 `Illust_Casual_Tile` 신설. 배경 `Illust_Casual_Background_*.png.meta` `spritePixelsToUnits: 100`(문서 128과 불일치) → 실측 우선으로 PPU 100 = 19.2u × 10.8u, `Object_Background.prefab` View 스케일 (2.6, 1.6)·DrawMode 0(단일) = 49.9u × 17.3u로 "화면 비율" 배경 항목·전체 스타일 PPU 문구 갱신(기존 "0.95배·반복 배치" 서술 폐기)
- 연출 요구 "보스 전조"를 `게임컨셉` 갱신값(가로 타원·발밑 바닥 y)과 정합
- 리소스 검증(스킬 `게임개발_구성_컨셉_리소스_검증` 완료조건): verify success. 필수 판정 — 테마 선택(Casual) 확정, 규격 확정(계열 8종 캔버스·기준 높이·피벗·점유율·서열 전부 수치) 확정, 개수 검산(규격 섹션 8 = Player·Enemy·Boss·Chef·Background·Room·Projectile·Tile 열거 8, 신규 제작 대상 10항목 열거 10) 확정, 연출 요구 확정(4항목). 재계산: 적 피벗 72÷256 = 0.28125(문서 0.281 일치), 타일 셀 1024÷128×0.325 = 2.6u(일치), 배경 1920÷100×2.6 = 49.92u·1080÷100×1.6 = 17.28u(문서 49.9·17.3 일치), 정본 대조: 캐릭터 Knife·Gun, 적 Apple·Watermelon·Banana, 보스 Pumpkin·Pineapple, 방종류 4, 능력 6 전부 `게임컨셉` 정본 안(밖 ID 0건, "Orange"는 기존 "미사용" 표기 유지). 불합격 없음(대상 9항목)

## 비고
- 건너뜀 — 대상: `editor_util setup`(게임_작성 절차 3 외부 적용) / 조건: 지시서 "씬설정 변경이 없으면 셋업을 실행하지 않는다" / 실측 근거: `concept_manage list` Scene `Scene_Game`·`Scene_Lobby` 2건 그대로, 이번 Work 편집 파일은 Game·Balance·Resource `concept.md` 3건뿐(씬설정 문서 무편집)
- 건너뜀 — 대상: 골격 생성 후속 체인(리소스컨셉 갱신 후속) / 조건: 지시서 "골격 생성 후속 체인은 대상이 없으므로 건너뛴다" / 실측 근거: 신규 리소스 타입·씬 정의 0건(`Illust_Casual_Tile` 규격 섹션은 기존 에셋 `Assets/__Game/_Core/Image/Illust_Casual_Tile_Kitchen.png` 실측 기재)
- 자율 확정 판단(지시서 "사용자 질문 금지"): 적 접촉은 "겹침 허용" 채택(정지 거리·근접 슬롯이 이미 겹침을 막아 정지 방식보다 구현이 적음), Gun 목표는 "HP 80 기준 목표값" 채택(Knife 한정 명시보다 실측 판정이 가능)
- 리소스 노드 "규격 표기" 규칙에 따른 어긋난 기준 보고: 문서 기존 "PPU 128 고정"·"배경 0.95배 반복 배치"가 에셋 실측(PPU 100·단일 스케일 (2.6, 1.6))과 어긋나 실측값으로 교체
- `_Temp/Work_1/ink.py`는 실측 스크립트(임시 산출물)
- DataMCP `Fallback` 미사용(전 호출 1회 성공)
