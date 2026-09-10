# [게임개발_구성_리소스_타입_구성] "리소스 생성 보완" 업무 레포트

## 요약

- `Work_4_1` 실패: `resource_type patch`의 실제 병합 동작이 스킬 문서와 달라 3개 AnimationSheet 타입의 출력 슬롯 필드가 초기화됐습니다.
- 문자열 로드 실측은 대상 147건 중 실패 0건이고 `Assets/__Game/**/*.cs`의 `Resources.Load` 호출은 0건입니다.
- 7개 전투 객체 프리팹의 `SpriteAnimPlayer.m_Clips` 배열 크기는 모두 0이라 인스펙터 참조 완료 조건도 미충족입니다.

## 완료업무

### SpriteAnim 문자열 로드 통로 검증
**산출물**
`Assets/__Game/Unit/Script/SpriteAnimPlayer.cs`
`Assets/__Game/Room/Script/RoomUtil.cs`
**작업내용**
- Unity CLI `Resources.Load<Sprite>` 실측 결과는 147건 중 null 0건입니다.
- `rg` 실측 결과 `Assets/__Game/**/*.cs`의 `Resources.Load` 호출은 0건입니다.
- `PrefabUtility.LoadPrefabContents` 실측 결과 7개 객체 프리팹의 `m_Renderer`는 전부 배선됐고 `m_Clips` 배열 크기는 전부 0입니다.

### 이동 전 메타 보존
**산출물**
`_Temp/Work_4_1_meta`
**작업내용**
- 기존 `Assets/__Game/_Core/Resources/SpriteAnim`의 PNG 메타 147건을 이동 전 백업했습니다.

## 비고

- 업로드 원본은 `_Data/Resource/File/AnimationSheet_Casual_Enemy`, `_Data/Resource/File/AnimationSheet_Casual_Boss`, `_Data/Resource/File/AnimationSheet_Casual_Player`에 남아 있습니다.
- 사용자 기존 변경 4건은 수정하지 않았습니다.

## 예외상황

- 막힌 지점: `게임개발_구성_리소스_타입_구성` 3단계 `resource_type patch`.
- 문서 명세: `C:/_Projects/_WebForGameData/_Data/Workflow/Skill/게임개발_구성_리소스_타입_구성/skill.md`는 patch가 부분 병합이며 담지 않은 필드를 보존한다고 명시합니다.
- 실제 응답 원문: `{"success":true}`.
- 실제 상태: 3개 타입의 `outputs.frame_01`~`frame_06`에서 전달하지 않은 `suffix`, `ext`, `idPrefix`, `isPreview`가 각각 빈 값·빈 값·빈 값·`false`로 초기화됐습니다.
- 영향 범위: 기존 `Assets/__Game/_Core/Resources/SpriteAnim`의 147개 PNG와 메타가 삭제 상태이며, 새 경로 익스포트와 후속 리소스 생성은 수행하지 않았습니다.
- 사용자 확인 요청: 전체 출력 슬롯 값을 명시하는 patch로 3개 타입을 복구·전환해도 되는지 확인이 필요합니다.
