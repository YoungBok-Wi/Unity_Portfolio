# [게임개발_프리셋_파일_오브젝트_코드_작성] "총기 점프 중 전용 외형 유지" 업무 레포트
## 요약
`Object_Player_Gun`의 `Jump` 요청을 기존 총기 전용 `Idle_Gun` 클립으로 변환했다. 점프 물리는 유지하면서 칼 캐릭터 점프 프레임으로 바뀌는 현상을 제거했다.

## 완료업무

### 총기 점프 외형 분기 작성
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Gun/Script/Object_Player_Gun.cs`
**작업내용**
- `ResolveAnim`에서 `UnitConst.AnimJump`를 `Idle_Gun`으로 변환한다.
- 기존 `Idle_Gun`·`Move_Gun` 분기와 다른 액션 반환을 보존했다.
- 신규 직렬화 필드나 프리팹 배선은 추가하지 않았다.

### 컴파일과 익스포트
**산출물**
`Object_Player_Gun`
**작업내용**
- Unity 재컴파일 결과는 `status: completed`, `failed: false`, 에러 로그 0건이다.
- `preset_manage export` 단건 결과 `success: true`를 확인했다.
- `AssetDatabase.Refresh()` 결과 `success: true`, `result: true`를 확인했다.

## 비고
- 전용 `Jump_Gun` 리소스가 추가되기 전까지 점프 중 `Idle_Gun` 외형을 유지하는 단순화다.
- `유니티엔진_씬_검증`은 사용자 지시에 따라 실행하지 않았다.
