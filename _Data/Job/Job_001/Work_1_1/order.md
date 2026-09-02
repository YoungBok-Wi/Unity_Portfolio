# 업무지시서

## 1. Concept 타입 프롬프트 구성과 컨셉아트 제작

**대상 스킬**: 게임개발_구성_리소스_타입_구성

**"taskContent"**: `Concept` 노드 타입 3종의 프롬프트 결손·불일치 해소 후 게임 전체 컨셉아트 제작·업로드

**업무**

- 대상 타입: `Concept_Resource`(`basePromptText`·`workerValues`·`workerLiteralValues`·`automationType` 전부 빈 값), `Concept_Scene_Game`·`Concept_Scene_Lobby`(`basePromptText`가 다른 게임 SugarSlash·쿼터뷰·세로 1080x1920 문구)
- 세 타입의 `basePromptText`를 `리소스컨셉`(`_Data/Concept/Resource/concept.md`)의 Casual 화풍·2D 사이드뷰·1920x1080 가로 공통 지시로 구성하고, 즉시 제작이 가능하도록 워커 값·자동화 설정을 채운다. 타입 등록 자체는 있으므로 생성은 건너뛴다
- 이어서 `Concept/Concept_Resource/Overview` 파일(등록·프롬프트 완료, pool 비어 있음)을 `게임개발_구성_리소스_파일_구성`으로 점검한 뒤 `게임개발_구성_리소스_파일_이미지_제작` → `게임개발_구성_리소스_파일_업로드`로 컨셉아트를 확보한다 (`게임개발_구성_리소스_파일_생성`은 entry가 이미 있으므로 건너뛰고 사유를 보고한다)
- 완료 기준: `Concept_Resource/Overview`의 산출 슬롯 pool에 이미지가 반입되고 `select`가 확정된다

## 2. 씬설정 작성과 씬 파일·골격 셋업

**대상 스킬**: 게임개발_구성_컨셉_씬_생성

**"taskContent"**: `Scene_Lobby`·`Scene_Game` 두 씬의 `씬설정` 작성·검증·외부 적용, 씬 파일·모듈/프리셋 골격·씬 컨셉아트·씬 셋업 (Work_1 업무 4 전체)

**업무**

- 선행 조회: `게임개발_구성_컨셉_질문`(`게임컨셉` 정본 ID·씬 2건), `게임개발_모듈_질문`(`inAsset=false` 모듈 FSM·CharacterPhysics·Bank·Delegate 등 재사용 후보), `게임개발_프리셋_파일_질문`(필수 `팝업` 3종 현황), `게임개발_구성_리소스_질문`
- `씬설정` 문서는 두 씬 모두 이미 존재하므로 생성을 건너뛰고 사유를 보고한 뒤 `게임개발_구성_컨셉_씬_작성`으로 내용을 채운다. `Scene_Game`은 기존 내용(다른 게임 문구 포함 가능)을 이번 `게임컨셉`에 맞게 전면 수정한다
- 경로 정리: `Scene_Game.unity` 실제 경로 `Assets/__Game/_Core/__Scene/Scene_Game.unity`와 문서·빌드 설정 경로 `Assets/__Game/Scene/Scene_Game.unity`가 불일치한다 — `씬설정` 문서·빌드 설정을 실제 파일 위치 하나로 통일하고, 통일한 경로를 레포트에 적는다. `Scene_Lobby.unity`는 없으므로 `유니티엔진_씬_생성`으로 같은 폴더에 만든다
- "사용 모듈": 기존 `inAsset=true` 모듈(Popup·Save·Sound·Table·Value·Number·Language·Icon·Deal)과 `FSM`·`CharacterPhysics`·`Bank`·`Delegate`·`ObjectPool`을 우선 등재하고, 게임 고유 모듈(방 진행·전투·캐릭터 해금 등)은 최소 개수로 신규 등재한다
- UI: 로비(캐릭터 선택·시작·설정·해금 잠금 표시), 게임(HUD HP·방 순번·방 이력, 방 선택 팝업 + 적 미리보기, 능력 선택 팝업, 일시정지, 결과 팝업). 필수 `팝업` 3종 포함·제외와 취소 입력 주체를 확정한다
- Object: 플레이어 2종(Knife·Gun), 일반 적 3종(Apple·Watermelon·Banana), 보스 2종(Pumpkin·Pineapple), 투사체, 배경·바닥
- 후속 체인 순서: `게임개발_프리셋_파일_생성`(미제작 Object·팝업 더미) → `게임개발_프리셋_파일_오브젝트_삭제`(목록에서 빠진 것) → `게임개발_구성_컨셉_씬_검증` → `유니티엔진_씬_생성` → `게임개발_모듈_폴더_생성`·`게임개발_모듈_폴더_구성`(신규 모듈 골격 + `inAsset=false` 재사용 모듈 켜기, 컴파일 통과) → `게임개발_구성_리소스_타입_생성`(타입이 이미 있으므로 건너뜀)·`게임개발_구성_리소스_타입_구성`·파일 생성·구성·`게임개발_구성_리소스_파일_이미지_제작`·업로드(씬별 `Concept_Scene_{씬ID}` 컨셉아트) → `게임개발_프리셋_파일_팝업_삭제` → `게임개발_프리셋_파일_팝업_구성` → `유니티엔진_씬_셋업_실행`
- 완료 기준: 두 씬 파일이 빌드에 등록되고 `씬설정` 검증 합격, 셋업 통과, 컴파일 에러 0건
- 비고: 사용자가 자율 진행을 지시했으므로 기획 판단은 스스로 확정하고 근거를 레포트에 적는다. `confirmed`·`reuse` 값은 변경하지 않으며, 막힘·결함은 우회하지 않고 대상·에러 원문을 레포트로 보고한다
