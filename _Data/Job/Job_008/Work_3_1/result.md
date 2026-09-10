# [게임개발_구성_리소스_타입_구성] "신규 타입 구성 및 AnimationSheet Resources 해제 선행조건 확인" 업무 레포트

## 요약
- `Icon_Casual_Face`·`UI_Common_Gradient` 타입은 프로젝트 로컬로 생성·구성했다.
- AnimationSheet 3종은 런타임 문자열 로드가 활성이라 `resources:false` 변경 전에 중단했다.
- 파일 entry 생성·이미지 합성·업로드·익스포트는 수행하지 않았다.

## 완료업무

### 리소스 현황·방향 실측
**산출물**
`C:\_Projects\Unity_Portfolio\_Temp\Work_3_1_J8\enemy_contact.png`
`C:\_Projects\Unity_Portfolio\_Temp\Work_3_1_J8\boss_contact.png`
`C:\_Projects\Unity_Portfolio\_Temp\Work_3_1_J8\player_contact.png`
**작업내용**
- AnimationSheet 3종 30개 파일의 `pool`·`select`와 선택 원본 경로를 조회했다.
- 좌향 프레임은 `Apple_Move/frame_01`, `Apple_Die/frame_01~06`, `Pineapple_Attack2/frame_01~06`, `Pumpkin_Die/frame_01~06`, `Player/Attack2/frame_01~06`이다.
- `Illust_Casual_Chef/Knife` 원본은 `C:\_Projects\Unity_Portfolio\_Data\Resource\File\Illust_Casual_Chef\Knife\art\1.png`, 반출본은 `Assets/__Game/_Core/Image/Illust_Casual_Chef_Knife.png`다.

### 신규 타입 생성·구성
**산출물**
`Icon/Icon_Casual_Face`
`UI/UI_Common_Gradient`
**작업내용**
- `Icon_Casual_Face`를 `Icon_Casual` 하위, `icon` PNG 256x256, `resources:false`, `leaf:Icon`, `idPrefix:Icon_Casual_Face_`, 자동화·프롬프트 없음으로 구성했다.
- `UI_Common_Gradient`를 `UI_Common` 하위, `image` PNG 512x512, `resources:false`, `leaf:Image`, `idPrefix:UI_Common_Gradient_`, 자동화·프롬프트 없음으로 구성했다.
- 두 타입은 `location:project`이며 `reuse:add`·`confirmed:{}`를 변경하지 않았다.

## 비고
- AnimationSheet 출력 슬롯·설명은 변경하지 않아 현재 `resources:true` 상태다.
- 후속 `Work_4`의 `Resources.Load` 제거와 프리팹 인스펙터 배열 배선이 끝난 뒤 해제·합성·업로드·익스포트를 재개해야 한다.

## 예외상황
- 대상 — `AnimationSheet_Casual_Enemy`·`AnimationSheet_Casual_Boss`·`AnimationSheet_Casual_Player` 출력 슬롯 전건의 `resources:false` 변경.
- 실측 원문 — `{"total":147,"loaded":147,"nulls":[]}`.
- 막힌 지점 — `SpriteAnimPlayer.cs:61`과 `RoomUtil.cs:86`이 `Resources.Load<Sprite>("SpriteAnim/...")`를 사용하며, 리소스 규칙은 non-null 항목이 하나라도 있으면 `resources`를 끄지 못하게 한다.
- 처리 방향 — `Work_4`에서 문자열 로드를 제거한 뒤 남은 업무를 보완 Work로 수행한다.
