# [게임개발_프리셋_파일_익스포트] "플레이어 프리셋 개별 익스포트 복구" 업무 레포트

## 요약
- `Object_Player_Knife`, `Object_Player_Gun`을 문자열 `prefabId`로 각각 익스포트했으며 두 응답 모두 `success: true`였다.
- 후속 재임포트 대상 프리팹 경로와 현재 GUID를 확인했다.

## 완료업무

### 플레이어 프리셋 개별 익스포트
**산출물**
`Assets/__Game/_Core/_Object/Object_Player_Knife/Object_Player_Knife.prefab`
`Assets/__Game/_Core/_Object/Object_Player_Gun/Object_Player_Gun.prefab`
**작업내용**
- `preset_manage export`를 `Object_Player_Knife`와 `Object_Player_Gun`에 순차 호출했고 각 응답은 `success: true`였다.
- `Object_Player_Knife.prefab.meta`의 GUID는 `39793c6aef3b1c74bb71f8766fa03be4`이다.
- `Object_Player_Gun.prefab.meta`의 GUID는 `6fe10a802e879844db3a31ac0bc47891`이다.

## 비고
- 재임포트와 컴파일 검증은 `Work_1_2`에서 수행한다.
