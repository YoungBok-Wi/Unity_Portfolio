# 업무지시서

## 1. Quit 모듈 켜기

**대상 스킬**: 게임개발_모듈_폴더_구성

**"taskContent"**: 라이브러리 `Quit` 모듈 `inAsset=true` 전환 (`Popup_Quit` 텍스트 익스포트 전제)

**업무**

- 근거: `_Data/Job/Job_002/Work_2/result.md` 비고 — `Text_Quit_*`가 `Quit` 모듈 미포함으로 익스포트 제외
- 켠 뒤 `유니티엔진_컴파일_실행` 통과와 `Scene_Lobby` `Popup_Quit` 동작 전제를 확인한다. 씬설정 "사용 모듈" 등재가 필요하면 그 사실을 레포트에 남긴다(문서 수정은 컨셉 영역)

## 2. 모듈 결함 수정

**대상 스킬**: 게임개발_모듈_폴더_작성

**"taskContent"**: 접지 판정·적 접촉 밀림·스폰 클램프·로비 BGM·전조 형태·해금 문구 ID 수정

**업무**

- 근거: `_Data/Job/Job_002/Work_7/result.md` `## 비고` 결함 목록, `_Data/Job/Job_003/Work_1/result.md` 정본
- 접지 판정: 피벗 정본(Work_1)과 `CharacterPhysics2DSide.cs:104~136` 접지 조건을 대조해 라이브러리 규칙 수정 또는 `Object_PlayerBase` 접지 판정 보완 중 정본이 지정한 쪽을 수행한다. 라이브러리 수정이면 `CharacterPhysics` 모듈 코드 작성·익스포트로 처리한다. 결과: 스폰 직후 `FlyState=Ground`·`Idle`/`Move`(Gun `Idle_Gun`/`Move_Gun`) 재생
- 적 접촉 밀림: 적↔플레이어 물리 접촉이 서로 밀지 않게(레이어 충돌 매트릭스·`Rigidbody2D` 질량/제약·`StopHorizontal` 뒤 물리 해소 순서 중 원인에서 해결). 결과: 무입력 27초 x 변화 0
- 웨이브 스폰: `LocalRoomManager` 스폰 오프셋을 벽 안쪽(정본 ±11u 이내)으로 클램프
- 로비 BGM: `Scene_Lobby` 로컬 매니저(`LocalCharacterManager`)에서 `BattleManager.PlayBGM(BGM_Casual_Lobby)` 호출 — `BattleManager`가 로비에 없으면 BGM 재생 책임을 글로벌 매니저로 옮긴다
- 전조: `Assets/__Game/Battle/Prefab/Telegraph.prefab`을 바닥 가로 타원(x 스케일 > y, 발밑 y 오프셋)으로
- 해금 문구: `LocalRoomManager`의 해금 알림을 Work_4가 신설할 `Text_Core_GunUnlocked`로 교체 (ID 문자열만 참조, 데이터는 Work_4)
- 후속 `유니티엔진_컴파일_실행` 통과 → `게임개발_모듈_폴더_익스포트`. 플레이 실측으로 각 결과를 확인해 레포트에 적는다. 사용자에게 질문하지 않는다. `confirmed`·`reuse` 값은 변경하지 않으며 DataMCP 무응답 시 `Fallback` 순서를 따른다
