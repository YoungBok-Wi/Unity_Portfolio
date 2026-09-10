# Job_008 업무 맥락

## 목적

폴리싱 7회차 — 사용자가 완성 게임을 직접 보고 내린 상세 오더 17건을 영역별(컨셉 → 데이터 → 리소스 → 모듈 → 프리셋 → 씬 → 커밋)로 일괄 반영하고 커밋한다. 회차 종료 시 README `폴리싱 작업` 절·개요·AI 방향성 절을 갱신한다.

## 사용자 지시 원문

"이번 세션에서 게임 폴리싱을 위해 어떤 작업을 했는지 Readme.md의 폴리싱 작업 섹션에 매번 작성해줘. 폴리싱 작업은 인간이 실제 완성된 게임을 보고 직접 상세 오더를 내려서 작업하는거다 뭐 그런 내용 써주고. 아 추가로 포트폴리오 개요에 전 과정은 Fable 5.1을 사용했다고 명시해줘.

실제 작업할 내용이야
- 모든 UI (Popup_XXX들)의 Canvas Scaler를 Scale With Screen Size & 1920x1080 & Expand로 설정해줘
- 모든 프레임이 있는 팝업형 팝업들의 연출을 Popup_Notify처럼 Blocker는 페이드, 프레임은 한쪽 방향에서 회전하면서 등장하는걸로 해줘
- 적들이 오른쪽으로 이동중엔 왼쪽을 보고, 왼쪽으로 이동중엔 오른쪽을 보는 문제가 있어 (공격시에는 제대로 된 방향을 봄) 이는 스프라이트 시트 일부가 왼쪽을 보도록 생성되어있기 때문이니까 수정해줘
- 비전투 스테이지 빈도가 너무 높고, 아무것도 없이 즉시 전환되어서 좀 이상해 비전투 스테이지를 한번 고르면 다음방은 전투 스테이지만 나오게 해줘
- 5스테이지마다 반드시 보스전투가 나오도록 해주고 (5스테이지마다 확정, 보스전투 1개 선택지만 존재)
- 적이 피격당해 밀려나는걸 급격히 밀려났다가 급격히 느려지는 AnimationCurve로 만들어줘 추가로 짧은 스턴시간도 만들어주고 관련 값들 수정 가능하게 알려줘
- 전투 스테이지 2개가 선택지로 나올 때는 같은 구성이 아니게 해줘
- 1,2,3타의 이펙트가 다르게 해줘 (기존 이펙트를 강화하는 느낌으로!)
- 현재 스프라이트 애니메이션들이 전부 Resources폴더에 들어있는데, Inspector사용하는쪽으로 변경하고, 리소스타입도 resources체크 풀어줘
- 모듈 구성은 수정해줘: PlayerCharacter(플레이어캐릭터), Room(전투 방), RoomSelect(방 선택지), Game(Battle, Room, PlayerCharacter 관리), Enemy(적 스크립트, 보스도 공용으로 사용), Boss(보스용 추가 스크립트들), Unit (플레이어 및 적 공용), Data(재화 및 해금 등)
- 플레이어 캐릭터는 고정으로 있기 떄문에 컨셉 셋업에 포함시켜서 Game씬에 처음부터 있게 해줘
- 씬전환이 심심한데, 씬전환 효과 하나 더 만들어서 이게임 캐릭터 얼굴이 대각선으로 뾰뵤뵤뵹 생겨나고 사라지면서 전환되는거 만들어줘
- 체력이 30% 미만일때 바깥쪽에 연하게 깜빡이는 연출로 체력 거의 없는거 알 수 있게 하는 연출 해줘
- 현재 플레이어 캐릭터 공격 애니메이션 선딜레이가 좀 있는데, 2프레임째에 플레이어 캐릭터가 칼을 완전히 내리는식으로 해서 줄여줘 그리고 다음 공격을 하려면 일정 시간 지난 뒤에 가능하도록 해줘 (그 전에 입력된것들은 무시) 칼 캐릭터의 경우 공격 범위를 넣어줘
- 플레이어 캐릭터는 FSM이 없는데, FSM기반으로 작동하도록 만들어줘 (입력에 따라 다른 State로 전환)
- 그 외 상속이 있는데 enum으로 해둔것들은 그렇게 하지 말고 override해서 처리해줘 (EUnitKind & EBattleTeam 삭제)

그리고 추가로 섹션 마지막에 하나 더 만들어서 GPT6 Astra 및 이후의 AI방향성에 대해서 작성할 수 있게 공간 해주고. GPT6 Astra에서는 프롬프트 준수율이 지금까지의 발전 속도와 비교했을 때 눈에 띄게 좋아졌기 때문에 하네스를 통해 개인/팀의 구조를 AI에게 강제하는것은 더 유용해질 것으로 생각하지만 반대로 하네스가 AI의 능력을 제한할 수 있는 가능성도 더 커졌기 때문에 좀 더 세심한 설계가 필요하다 머 이런내용 작성해줘"

## 운영 규칙

- 실행 모드 `직접` (개선 작업) — Job·Work 편성, 서브에이전트 없이 본 세션이 워커 절차 수행·레포트 작성
- DataMCP MCP 연결 실패 → 세션 내내 `Fallback` `curl` 직접 호출 (`_Temp` 대신 스크래치 `dm.py` 헬퍼, projectID `Unity_Portfolio`)
- `confirmed`·`reuse` 무변경, 라이브러리(`Assets/_Library`·`_Data/Module/Library`) 수정 금지 — 게임 쪽 우회 (Job_005 규칙 승계)
- 씬 셋업(`editor_util setup`)은 이번 회차 필수(플레이어 씬 상주·모듈 재편 등재) — Job_005의 금지는 해제하되 셋업 전후 계층 대조로 오버라이드 소실(로비 카메라 4.0 등)을 검출·복원한다
- 플레이테스트·성능테스트는 편성하지 않는다 — "명시 요청 한정" 규칙(사용자가 검증을 요청하지 않음). 완결 조건은 컴파일 통과·익스포트·씬 검증
- 회차 종료 시 README 갱신 3건: `## 폴리싱 작업` 절(성격 설명 + 회차 기록), `## 포트폴리오 개요`에 전 과정 Fable 5.1 사용 명시, 마지막에 `## GPT6 Astra 이후의 AI 방향성` 절 신설 (스킬 매칭 대상 아님 — 오케스트레이터 운영 규칙, Job_005 선례)

## 진행 메모 (Work_1 이후)

- `editor_util setup`은 활성 씬의 `[Local]`·`[Popup]`·`[Stage]` 오브젝트를 새 fileID로 재생성해 씬 오버라이드(Floor `View` Tiled 185x8·scale 0.325, 카메라 `orthographic size` 4)를 지운다 — Work_1에서 실행 뒤 git 복원. Work_6은 셋업 스킬 절차(setup 전 오버라이드 기록 → setup → 재적용·결손 보고)를 반드시 지키고, Work_1~5는 setup을 실행하지 않는다
- Unity CLI는 `--project-path "C:/_Projects/Unity_Portfolio"` 명시 호출 (에디터 인스턴스 2개), `com.unity.pipeline`을 0.6.0-exp.1로 올려 CLI 정상화 (Work_1 비고)
- 비네트 파일은 `UI_Common_Shape`(shared·default) 대신 신규 타입 `UI_Common_Gradient/Vignette` (Work_3·Work_5 지시서의 해당 ID를 이 값으로 읽는다)
- 노드 규칙 "한 판 경계"(전투·진행 한 모듈)보다 사용자 지시(8모듈 분리)를 우선한다 — Work_4 `module.md`에 사유 기재

## 해석·가정 (사용자 부재로 확정, 레포트에 명시)

- 보스 주기: 방 순번 5·10·15…는 보스방 확정, 선택지 1개(Boss)만 제시. 보스 처치 후 런은 계속되며(다음 순번 선택지) 런 종료는 플레이어 사망뿐 — `ERunResult.Win` 경로 제거, 결과 팝업은 도달 순번·Crumb 총량·Gun 해금 알림
- 비전투(Heal·Ability) 방 클리어 후 선택지는 [Battle/Battle] 고정, 그 외 비보스 순번은 [Battle/Heal]·[Heal/Ability]·[Battle/Battle] 균등. [Battle/Boss] 세트·`Room_BossMin`·`Room_BossForce` 폐기
- [Battle/Battle]: Wave 테이블에 `Variant` 열(1·2)을 두고 두 선택지에 서로 다른 변형을 배정, 단독 Battle은 무작위 변형. 순번 11 이상은 마지막 구간 행(RoomMax 999)으로 이어 무한 진행
- 넉백: 거리·시간은 테이블 유지, 진행 곡선은 `AnimationCurve`(급가속·급감속) — 정본은 `LocalGameManager` 인스펙터. 피격 경직은 넉백 종료 후 고정값 `Battle_HitStunSec`(0.1s) 추가 정지
- 플레이어 공격: 판정은 공격 모션 2번째 프레임 시점, 모션 프레임 배열에서 준비 프레임을 빼 2번째 프레임이 칼을 완전히 내린 자세가 되게 배선. 공격 후 `AttackInterval` 동안 입력 무시(선입력 폐기 — Character `InputBuffer` → `ComboWindow`), 종료 후 `ComboWindow` 안 입력이면 다음 단, 아니면 1단
- 플레이어 FSM: `Idle`·`Move`·`Jump`·`Attack`·`Hit`·`Die` 자식 상태(라이브러리 `FSM`), 입력은 `Object_PlayerBase`가 읽어 상태가 전환
- enum 제거: `Object_UnitBase`의 종류 분기는 파생(`Object_PlayerBase`·`Object_EnemyBase`·`Object_BossBase`)의 override(테이블 로드·진영 판정·넉백 면역·아이콘)로 대체
- 플레이어 씬 상주: `Object_Player_Knife`·`Object_Player_Gun` 둘 다 씬 컨셉 Object "씬 배치"로 두고 런 시작 시 선택 캐릭터만 활성
- 씬전환 연출: Game 모듈 `SceneChangeAni_Face`(얼굴 타일 격자, 대각선 순차 등장·소멸) 프리팹을 Game 셋업 스크립트가 [Global]/[SceneChangeManager]에 자식·배열로 배선. 얼굴은 `Illust_Casual_Chef` 머리 크롭 신규 타입 `Icon_Casual_Face`
- 저체력: HP 비율 < 고정값 `Battle_LowHpRatio`(0.3)이면 HUD 외곽 비네트(`UI_Common_Shape/Vignette` 합성 PNG) 알파 깜빡임
- 타격 이펙트: 단별 궤적·히트 이펙트 배율·색(1단 기본, 2단 1.25배, 3단 1.5배+히트스톱)은 `LocalGameManager` 인스펙터
- SpriteAnim: 타입 3종(`AnimationSheet_Casual_Enemy`·`Boss`·`Player`) 출력 슬롯 `resources` 해제 → `Assets/__Game/_Core/SpriteAnim/`으로 익스포트, `SpriteAnimPlayer`는 동작별 프레임 배열 인스펙터 배선, 적 미리보기 아이콘은 프리팹 `Icon` 필드(Enemy·Boss 테이블 `Icon` 열 삭제)
- 적 방향: 좌향으로 생성된 Move 프레임을 수평 반전해 재업로드(대상은 리소스 Work에서 실측)

## 대상 (번호는 README 기록·레포트 공용)

- ① [프리셋 팝업] Canvas Scaler 7종 Scale With Screen Size·1920x1080·Expand
- ② [프리셋 팝업] 프레임형 5종(Ability·Pause·Result·RoomSelect·Setting) Blocker 페이드 + 프레임 회전 등장 (`PopupAni_Alpha_Smooth`·`PopupAni_Rotation_Dynamic` 애드온)
- ③ [리소스] 적 Move 시트 좌향 → 우향 반전
- ④ [컨셉·데이터·모듈] 비전투 방 뒤 선택지 [Battle/Battle] 고정
- ⑤ [컨셉·데이터·모듈] 5순번마다 보스 확정·단일 선택지, 보스 후 런 계속
- ⑥ [컨셉·데이터·모듈] 넉백 AnimationCurve + 피격 경직, 조정 값 안내
- ⑦ [컨셉·데이터·모듈] [Battle/Battle] 구성 상이 (Wave `Variant`)
- ⑧ [모듈·프리셋] 1·2·3타 이펙트 단계 강화
- ⑨ [리소스·모듈·프리셋] SpriteAnim Resources → Inspector, 타입 `resources` 해제
- ⑩ [모듈] 모듈 재편 8종 (PlayerCharacter·Room·RoomSelect·Game·Enemy·Boss·Unit·Data)
- ⑪ [컨셉·프리셋·씬] 플레이어 씬 상주 (컨셉 셋업)
- ⑫ [리소스·모듈·씬] 씬전환 얼굴 타일 연출
- ⑬ [리소스·데이터·프리셋] HP 30% 미만 외곽 깜빡임
- ⑭ [리소스·데이터·모듈·프리셋] 공격 선딜 단축(2프레임 판정)·쿨타임 중 입력 무시·Knife 공격 범위 노드
- ⑮ [모듈·프리셋] 플레이어 FSM
- ⑯ [모듈] `EUnitKind`·`EBattleTeam` 삭제 → override
- ⑰ [README] 폴리싱 절·개요 Fable 5.1·GPT6 Astra 절 (운영 규칙)

## 체인묶음 대응

- [컨셉] 기준 `게임개발_구성_컨셉_게임_작성`: 컨셉_질문 → 게임_작성 → 게임_검증 → 데이터_질문 → 모듈_질문 → 프리셋_파일_질문 → 밸런스_작성 → 밸런스_검증 → 리소스_질문 → 리소스컨셉_작성 → 리소스컨셉_검증 → 씬_작성(Scene_Game·Scene_Lobby) → 씬_검증 (제외: 테이블_생성·고정값_생성 — 데이터 Work가 담당, 씬_생성·유니티엔진_씬_생성·모듈_폴더_생성/구성·리소스 타입/파일 생성·구성·이미지_제작·업로드·프리셋 생성/삭제·팝업_구성·셋업_실행 — 각 영역 Work가 담당)
- [데이터] 기준 `게임개발_구성_데이터_고정값_구성`: 데이터_질문 → 고정값_생성 → 고정값_구성 → 컨셉_질문 → 고정값_작성 → 고정값_삭제(Room_BossMin·Room_BossForce·Room_ChoiceSet3) → 테이블_구성(Wave `Variant` 추가, Character `InputBuffer`→`ComboWindow`, Enemy·Boss `Icon` 삭제) → 테이블_작성 → 데이터_익스포트
- [리소스] 기준 `게임개발_구성_리소스_타입_구성`: 리소스_질문 → 타입_생성(`Icon_Casual_Face`) → 타입_구성(`resources` 해제 3종, 신규 타입) → 타입_업로드 → 파일_생성(`Icon_Casual_Face/Chef`, `UI_Common_Shape/Vignette`) → 파일_구성 → 파일_업로드(반전 Move 프레임·크롭·비네트 합성) → 파일_익스포트 (제외: 이미지_제작 — 산출은 코드 합성)
- [모듈] 기준 `게임개발_모듈_폴더_구성`: 모듈_폴더_생성(PlayerCharacter·RoomSelect·Enemy·Boss·Unit) → 모듈_폴더_구성(Battle→Game·Character→Data ID 변경, 메타) → 모듈_폴더_작성(기획 → 코드 → 프리팹) → 컴파일 → 익스포트 (제외: 씬_셋업_실행 — 씬 Work)
- [프리셋] 기준 `게임개발_프리셋_파일_팝업_구성`: 프리셋_파일_질문 → 컨셉_질문 → 모듈_질문 → 리소스_질문 → 팝업_코드_작성 → 컨트롤_코드_작성 → 오브젝트_코드_작성(2D 사이드뷰 캐릭터 하위) → 컴파일 → 팝업_구성 → 컨트롤_구성 → 오브젝트_구성 → 프리셋_익스포트 (제외: 노드 생성/구성 — 노드 변동 없음, 파일_생성·애드온 코드/구성 — 신규 프리셋 없음)
- [유니티엔진] 기준 `유니티엔진_씬_셋업_실행`: 씬_질문 → 셋업_실행(Scene_Game·Scene_Lobby) → 씬_구성(참조 이관·오버라이드 복원) → 씬_검증 (제외: 씬_생성 — 씬 실재)
- [깃] 기준 `깃_커밋_실행`: 커밋 → 푸시

## 체크리스트 ↔ Work

- c01 → Work_1(컨셉) / c02 → Work_2(데이터) / c03 → Work_3(리소스) / c04 → Work_4(모듈) / c05 → Work_5(프리셋) / c06 → Work_6(씬) / c07 → Work_7(깃)
- ⑰ README는 Work_7 직전 오케스트레이터가 직접 수행
