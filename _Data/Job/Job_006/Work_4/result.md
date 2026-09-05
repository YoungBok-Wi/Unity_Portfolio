# [유니티엔진_설정_구성] "플레이어 설정 companyName 구성·검증" 업무 레포트

## 요약
- `set_player_settings --settings={"companyName":"YoungBok Wi"} --confirm=true` `applied:true` — 재조회 `companyName=YoungBok Wi`, `productName=Kitchen Riot`·`bundleVersion=0.1.0`·`Mono2x`·`NET_Standard_2_0` 무변경, `ProjectSettings/ProjectSettings.asset` 15행 반영
- 검증(`유니티엔진_설정_검증`): 기대값 일치, 심볼 무변경이라 컴파일 영향 없음 — 불합격 없음(대상 1)

## 완료업무

### 플레이어 설정 조회
**산출물**
`C:\_Projects\Unity_Portfolio\ProjectSettings\ProjectSettings.asset`
**작업내용**
- `get_player_settings`(`unity --format json list`의 `player` 영역): `companyName=DefaultCompany`, `productName=Kitchen Riot`, `bundleVersion=0.1.0`, `buildTarget=Standalone`, `scriptingBackend=Mono2x`, `apiCompatibilityLevel=NET_Standard_2_0` — 응답이 돌려주는 6키가 전부(defaultScreen·resolution은 미반환)

### companyName 적용
**산출물**
`C:\_Projects\Unity_Portfolio\ProjectSettings\ProjectSettings.asset`
**작업내용**
- 변경 대상 1키(companyName), 나머지 요청 값과 동일하므로 제외. 적용 응답 `Set player settings: companyName 'DefaultCompany' -> 'YoungBok Wi'`, `requiresDomainReload:false`
- 영향: PlayerPrefs 레지스트리 키가 `HKCU\Software\Unity\UnityEditor\YoungBok Wi\Kitchen Riot`로 바뀌어 기존 `DefaultCompany\Kitchen Riot` 저장(`BestRoom=12 …`)은 더 읽히지 않는다 — Work_5 QA는 첫 실행 상태에서 시작한다(되돌리려면 companyName 복원)

### 설정 검증
**산출물**
`C:\_Projects\Unity_Portfolio\ProjectSettings\ProjectSettings.asset`
**작업내용**
- `get_player_settings` 재조회 `companyName=YoungBok Wi`·`productName=Kitchen Riot` — 기대값 일치, 스크립팅 심볼 무변경(컴파일 검증 대상 아님)

## 비고
- 없음
