# [오케스트레이터_워커_실행] "[컨셉] 근접 슬롯 거리순·원거리 후퇴·표시 우선순위·보스 BGM 배속 고정값 항목 개정" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 5건 전부 수행, 컨셉 3문서(`게임`·`밸런스`·`리소스`) 개정 후 `concept_manage verify`(allErrors) 3건 전부 `{"success":true}`, 고정값 `Battle_BossBgmPitch` 골격 등록 `get` 반환 확인
- 개정 요지: `게임컨셉` 겹침 표시 우선순위(플레이어 > 적·보스 > 그 외, 값은 `리소스컨셉` 위임)·Boss 방 BGM 배율 고정값 정본 명시 / `밸런스컨셉` 근접 슬롯 거리순 재배정·적↔적 통행 허용·벽 밀착 기대값, Banana 후퇴 불가 시 발사, Boss BGM 배속 1.1(고정값 정본), 검산 3건(정합 재계산 1·신설 2) / `리소스컨셉` 정렬 순서 규격(플레이어 2·적·보스 1·그 외 0), 보스 시트 기준 높이 224 실측 대조 문장, 사운드컨셉 1.1 정본 참조
- 실측 핵심(Work_3 입력): Pineapple 시트 알파>0 경계 잉크 높이 — 기준 프레임 `Idle_01`·`Idle_04`·`Die_01` 224px = 규격 정합, `Idle_03` 209px(−15), Move 145~249, Attack1 184~272, Attack2 164~240. 화면 219px 실측은 `Idle_03` 209px(1.63u = 20.4%)에 대응 — 시트 전면 재제작 근거 없음, 대상은 `Idle_03` 1프레임 수준
- 무변경: `씬설정 Scene_Lobby`(개정 대상 아님 — "취소 입력" 정본 실재), 코드·프리팹·테이블 값, `confirmed`·`reuse`(4문서 `reuse:"add"`·`confirmed` null 유지), `editor_util setup` 미실행, DataMCP `Fallback` 미사용, 사용자 질문 없음
- 다음 행동: Work_2가 `Battle_BossBgmPitch` 타입(float)·값 1.1 입력·익스포트를 수행한다

## 완료업무

### 컨셉 현황 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
`C:\_Projects\Unity_Portfolio\_Data\Concept\Scene_Lobby\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_질문` → 하위 `게임_질문`·`밸런스_질문`·`리소스_질문`·`씬_질문` 4건 전부(조회 대상 4문서). `concept_manage list` 5건(Game·Resource·Balance·Scene_Game·Scene_Lobby), `path` 4건, `get` 4건 전부 `reuse:"add"`·`confirmed` null(Scene_Lobby는 `{}`)
- 개정 전 서술(대조 근거): `밸런스컨셉` "적 그룹 공통" 근접 접근 슬롯(76행) — 좌·우 2마리 상한·3.0u 대기·좌우 독립만, 배정 순서·재배정·적↔적 통행 문구 없음 / "적 Banana"(91행) — 유지 거리 5.0u·사거리 7.0u만, 후퇴 막힘 규칙 없음 / Boss BGM 배속 항목 없음
- `게임컨셉` "조작 입력" 적 접촉(71행) — 겹침 허용만, 표시 우선순위 없음 / "장르 요소"에 정렬 순서 없음 / "타격 사운드"(170행) 공격 시작 `SFX_Casual_Battle/Attack` 실재 / "데이터 구동"(78행) 리터럴 금지 실재
- `리소스컨셉` "사운드컨셉"(88행) 보스방 1.1배 실재 / "규격 AnimationSheet_Casual_Boss"(128~133행) 캔버스 384x384·기준 높이 224·피벗 하단 중앙 / 정렬 순서 규격 없음
- `씬설정 Scene_Lobby` "취소 입력"(111행) "열린 팝업이 있으면 그 팝업이 닫힌다" 실재 — `_Data/Job/Job_004/job.md` "메인 에이전트 직접 확인 결과"와 전건 일치

### 게임컨셉 개정·검증
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Game\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_게임_작성` + 하위 `게임개발_구성_컨셉_게임_액션_작성`(장르 액션·"장르 요소" 갱신) → `게임개발_구성_컨셉_게임_검증`. 하위 `게임_방치형_작성` 미선택(처리가능 방치형 장르 — `## 개요` 장르 "2D 사이드뷰 캐주얼 로그라이트 액션")
- 편집 2건: "조작 입력"에 "겹침 표시 우선순위" 항목 신설(72행 — 플레이어 > 적·보스 > 투사체·전조·히트 이펙트, 값은 `리소스컨셉` 규격 위임 — 노드 규칙 "구체 수치 본문 미기재" 준수) / "장르 요소" 사운드(199행)에 Boss 방 BGM 배율 고정값 `Battle_BossBgmPitch`(float) 정본·코드 리터럴 금지·값 `밸런스컨셉` 위임 추가
- 액션 장르 표준 5항목·전투 파라미터 4항목·이동/공격 리듬은 기존 "액션 장르 표준 판정"·"전투 파라미터 항목"(85~97행) 유지, 타격 연출 요소 소비처 기재 유지
- 검증: `verify` Game allErrors → `{"success":true}`. 필수 판정 6항목 원문 재발췌(153~158행) 전부 "확정". `unity_concept game` → title "Kitchen Riot"·resolution 1920x1080·orientation Landscape·buildTarget StandaloneWindows64·tech `{}` = 컨셉 값(6·21·22·25행, 기술 어드레서블 미표기) 일치. 정본 ID 대조: 신설 문구의 ID 없음(Battle_BossBgmPitch는 고정값명, 대상 ID 아님) — 불합격 없음(대상 항목 2)

### 밸런스컨셉 개정·검증
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Balance\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_밸런스_작성` → `게임개발_구성_컨셉_밸런스_검증`. 하위 `밸런스_방치형_작성` 미선택(방치형 곡선 전용)
- 편집 4건: "적 그룹 공통"에 근접 슬롯 배정 순서(거리순·매 이동 판정 재배정·선착순 금지·앞에 서면 슬롯 인계, 77행), 적↔적 통행(콜라이더 서로 밀지 않음·최소 간격은 등장 오프셋에만, 78행), 벽 밀착 기대값((11.7 + 10.0 − 0.8) / 3.5 + 1.0 = 6.97s ≤ 12s, 79행) / "적 Banana" 후퇴 불가 시 발사(사거리 7.0u 안 발사 주기 2.0s, 15s 표본 발사 기회 7회 → 투사체 ≥ 1, 95행) / "방 종류"에 Boss BGM 배속 1.1(고정값 `Battle_BossBgmPitch` float = 1.1, 값 정본, 117행) / 검산 "동시 추격 배치 정합" 우변에 간격 0~1.0u 대역 재계산(하한 1.0u·상한 2.0u ≤ 2.0u), "벽 밀착 근접 공격 성립"·"원거리 후퇴 불가 시 발사 기회" 신설
- 검증: `verify` Balance allErrors → `{"success":true}`. 검산 독립 재계산 — (11.7+10.0−0.8)/3.5 = 20.9/3.5 = 5.971 → +1.0 = 6.97 ✓, floor(15/2.0) = 7 ✓, 1.0+1.0×1 = 2.0 ≤ 2.0 ✓. 필수 판정: 선택지 풀 3 < 6 확정, 진행 난이도 성장식 확정, 체감 지표 산출(무입력 생존 8.19·7.07s) 확정, 개수 검산 18 = 정본 ID 18 확정. 정본 값 대조: `게임컨셉` 적 접촉 겹침 허용 ↔ 적↔적 통행 문구 인용 일치, 정본 목록 소속 — 신설 문구 ID Apple·Banana·Boss·Pumpkin·Pineapple 전부 정본 열거 안. 불합격 없음(대상 항목 6)

### 리소스컨셉 개정·검증
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Concept\Resource\concept.md`
**작업내용**
- 수행 스킬: `게임개발_구성_컨셉_리소스_작성` → `게임개발_구성_컨셉_리소스_검증`. 하위 `리소스_캐주얼_탑뷰_작성` 미선택(탑뷰 전용 — 본 게임 사이드뷰)
- 편집 3건: "캐릭터"에 "정렬 순서" 항목 신설(46~48행 — 플레이어 2·적·보스 1·투사체·전조·히트 이펙트 0 현행 유지, 프리팹별 값 `Object_Player_*` 2·`Object_Enemy_*`·`Object_Boss_*` 1·`Object_Projectile` 0) / "규격 AnimationSheet_Casual_Boss" 기준 높이 224 유지 + 실측 대조 문장(134행) / "사운드컨셉" BGM 1.1배 문구에 정본 `밸런스컨셉` 참조 추가(92행)
- 실측: `Assets/__Game/_Core/Resources/SpriteAnim/AnimationSheet_Casual_Boss_Pineapple_*.png` 28장 + Pumpkin 28장, PIL 알파>0 경계 bbox(스크립트 scratchpad `measure.py`). 전 프레임 캔버스 384x384·바닥행 383(피벗 하단 정합). Pineapple 동작별 잉크 높이: Idle 224·253·209·224 / Move 183·216·249·232·145·236 / Attack1 229·272·184·205·220·228 / Attack2 164·226·240·223·220·225 / Die 224·199·171·143·102·63. Pumpkin Idle 225·225·213·223
- 검증: `verify` Resource allErrors → `{"success":true}`. 규격 재계산 224/128 = 1.75u = 21.9%(8.0u 화면) ✓, 209/128 = 1.633u = 20.4% ≈ 219/1080 = 20.3% ✓. 필수 판정: 테마 선택 Casual 확정, 규격 확정(계열 8건 전부 5필드), 개수 검산 — 신규 제작 대상 계열 열거 유지, 연출 요구 확정. 정본 목록 소속: 신설 문구 ID Pineapple·Pumpkin 정본 안. 불합격 없음(대상 항목 3)

### 보스 BGM 배속 고정값 등록
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Const`
**작업내용**
- 수행 스킬: `게임개발_구성_데이터_고정값_생성`(하위 없음, `error.md` 빈 파일). 기존 확인 `const_data get consts.Battle_BossBgmPitch` → `"경로를 찾을 수 없습니다"`(미등록) → `patch` `{"consts":{"Battle_BossBgmPitch":{"description":"[Battle] 보스방 BGM 재생 속도 배율"}}}` → `success:true`
- 완료조건 `get` → `{"type":"","description":"[Battle] 보스방 BGM 재생 속도 배율"}`. 이어질 정의 작성 대상: 타입 float 지정(고정값_구성)·값 1.1 입력(고정값_작성)·익스포트 — Work_2 담당

## 비고
- 건너뛰기: 대상 — 업무 2 `게임개발_구성_컨셉_게임_작성` 절차 3 외부 적용(`editor_util setup`) / 조건 — order.md "제외 스킬: `유니티엔진_씬_셋업_실행`(셋업은 `Scene_Lobby` 카메라 오버라이드를 지우므로 실행 금지)" / 실측 근거 — `job.md` "운영 규칙" 동일 조항, `씬설정` 2문서·씬 파일 존재는 셋업 생략 사유가 아니므로 지시서 금지 조항만이 근거
- 건너뛰기: 대상 — 업무 2·3·4 후속 검증의 반복 루프 / 조건 — "합격까지 반복" / 실측 근거 — 1회차 `verify` 3건 전부 `success:true`라 반복 없음
- Pineapple 시트 기준 높이는 규격과 정합(기준 프레임 224px)이라 Work_3 재제작 대상은 정지 프레임 `Idle_03`(209px, −15) 수준으로 좁혀진다 — 동작 중 신축 프레임(Move·Attack·Die)의 편차는 애니메이션 표현이며 규격 미달로 보지 않는다(판정은 Work_3 몫)
- 편집은 CRLF 문서라 스크립트(scratchpad `edit2.py`)로 정확 일치 치환(각 대상 1건 단언) — 줄 끝 보존
- `Battle_BossBgmPitch` 값 1.1은 `밸런스컨셉` "방 종류" Boss BGM 배속이 수치 정본, `리소스컨셉`·`게임컨셉`은 참조로 정리
