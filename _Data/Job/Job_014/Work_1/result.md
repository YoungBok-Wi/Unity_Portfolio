# [게임개발_구성_리소스_질문] "Attack2 프레임 오른쪽 방향 수정" 업무 레포트

## 요약
`Attack2` 6프레임의 원본·사본 경로와 왼쪽 방향을 확인했지만, 등록된 자동화에 X축 반전 기능이 없어 반전·업로드·익스포트를 수행하지 못했다.

## 완료업무

### Attack2 리소스 원본과 반영 구조 조회
**산출물**
`_Data/Resource/File/AnimationSheet_Casual_Player/Attack2`
`Assets/__Game/_Core/SpriteAnim`
**작업내용**
- `resource_file get`에서 `Attack2`, `reuse: add`, `confirmed: false`, 선택 산출물 `frame_01`~`frame_06`의 `2.png`를 확인했다.
- `resource_file source`에서 여섯 원본이 각 `frame_01`~`frame_06` 폴더의 `2.png`임을 확인했다.
- `resource_file path`에서 엔진 사본이 `AnimationSheet_Casual_Player_Attack2_01.png`~`06.png`임을 확인했다.
- 원본 `frame_01/2.png` 시각 조회에서 캐릭터가 왼쪽을 향함을 확인했다.

### 이미지 반전 자동화 조회
**산출물**
`C:/_Projects/_WebForGameData/_Data/Automation`
**작업내용**
- `automation_manage list`에서 전체 자동화를 조회했다.
- `Process`에는 `image_edge`·`image_normalize`만 있고, `image_normalize` 설명은 리사이즈·여백·배경 처리로 X축 반전을 지원하지 않는다.

## 비고
- 대상 — 업무 3·4·5의 반전·업로드·익스포트. 조건 — 업무 2에서 적합한 자동화가 없으면 다른 가공 수단으로 우회하지 않고 예외 보고. 실측 근거 — `automation_manage list`에 X축 반전 자동화가 없다.

## 예외상황
- 대상 — `AnimationSheet_Casual_Player_Attack2_01`~`06`. 에러 원문 — `automation_manage list`의 `Process` 목록에 X축 반전 자동화 없음. 막힌 지점 — 기존 PNG의 픽셀·알파·크기를 보존하는 결정론적 반전 실행. 처리 요청 — X축 반전 자동화를 보완 Work로 생성·검증한 뒤 본 리소스 반영을 재수행해야 한다.
