# [컨셉] "넉백 FSM·Face 타일 요구 정본 반영" 업무 레포트

## 요약
- `Game` 정본에 플레이어·일반 적의 `Knockback` FSM 상태, 곡선 이동·완료 후 경직 책임, 인스펙터 설정 주체를 반영했습니다. `concept_manage verify` 결과는 `success=true`입니다.
- `Resource` 정본에 `SceneChangeAni_Face` 타일 `12x7=84`개 전부의 `Icon_Casual_Face/Chef` 사용, 투명 여백·종횡비 유지, 대각선 연속 팝 연출을 반영했습니다. `concept_manage verify` 결과는 `success=true`입니다.
- 원본 얼굴 PNG 실측은 `256x256`, 알파 범위 `0~255`, 투명 픽셀 `26,380`, 알파 경계 `204x252`이며 문서 규격을 실측값 `252px`, `98.4%`로 맞췄습니다.

## 완료업무

### 게임 컨셉 수정·검증
**산출물**
`_Data/Concept/Game/concept.md`
**작업내용**
- 플레이어 상태를 `Idle·Move·Jump·Attack·Hit·Knockback·Die`, 일반 적 상태를 `Idle·Chase·Attack·Hit·Knockback·Die`로 확정했습니다.
- `Knockback` 상태가 곡선 이동과 완료 후 경직을 전담하고 플레이어는 `Idle·Move`, 일반 적은 `Chase`로 복귀하도록 정의했습니다.
- 곡선·경직 시간은 각 유닛 전투 설정 인스펙터가 조정하고 값은 `밸런스컨셉`을 정본으로 사용하도록 정의했습니다.
- DataMCP `unity_concept game` 응답의 `Kitchen Riot`, `1920x1080`, `Landscape`, `StandaloneWindows64`가 문서와 일치하며 UnityGameMCP `editor_util setup` 결과는 `success=true`입니다.
- 필수 판정 6항목은 문서의 `필수 판정`에서 모두 값과 이유가 확정되어 있으며 결손은 없습니다.

### 리소스 컨셉 수정·검증
**산출물**
`_Data/Concept/Resource/concept.md`
`_Data/Resource/File/Icon_Casual_Face/Chef/icon/1.png`
**작업내용**
- `SceneChangeAni_Face`의 모든 타일에 투명 배경 `Icon_Casual_Face/Chef`를 원본 종횡비로 배치하고 빈 타일·단색 대체 타일을 두지 않도록 확정했습니다.
- 타일은 행+열 순 지연으로 스케일 `0→1` 팝하고 새 씬에서 같은 순서로 `1→0` 소멸하도록 확정했습니다.
- `12x7=84`, `1920÷12=160`, 원본 알파 높이 `252÷256=98.4375%`를 재계산해 문서의 타일 수·표시 크기·점유율과 일치함을 확인했습니다.
- 대상 씬 `Scene_Lobby`·`Scene_Game`과 캐릭터 `Knife`·`Gun`은 `Game` 정본 ID 목록에 포함되며 목록 밖 대상은 없습니다.

## 비고
- `Game`·`Resource`의 `confirmed`와 `reuse` 설정은 변경하지 않았습니다.
