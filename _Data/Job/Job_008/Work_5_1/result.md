# [데이터] "미사용 고정값·테이블 열 삭제와 전체 익스포트" 업무 레포트

## 요약
- `Room_BossMin`·`Room_BossForce`·`Room_ChoiceSet3`, `Character.InputBuffer`, `Enemy.Icon`, `Boss.Icon`을 원본과 런타임 산출물에서 제거했다.
- `const_data get`은 삭제한 3개 ID에 모두 `경로를 찾을 수 없습니다`를 반환했고, `table_data get`·`table_excel get`은 삭제한 3개 열과 행 값을 반환하지 않았다.
- 데이터 export 11회가 전부 `success:true`였고, 생성 코드·JSON의 삭제 항목 검색 결과는 0건이다.

## 완료업무

### 삭제 대상 사용처 확인
**산출물**
`Assets/__Game`
`_Data/Concept/Balance/concept.md`
**작업내용**
- `Assets`의 수기 스크립트에서 삭제 대상 고정값 3개와 테이블 열 3개의 소비 코드가 0건임을 확인했다.
- `밸런스컨셉`의 폐기 ID 표기를 현재 규칙 표현으로 바꾸고 `concept_manage verify`의 `success:true`를 확인했다.

### 고정값 삭제
**산출물**
`Assets/_Library/_Core/GenerateScript/Table_Const.cs`
`Assets/_Library/_Core/Resources/Table/TableConst.json`
**작업내용**
- `const_data remove` 3회가 모두 `success:true`였다.
- 삭제 후 단건 조회는 `consts.Room_BossMin`·`consts.Room_BossForce`·`consts.Room_ChoiceSet3`에 모두 부재 에러를 반환했다.

### 테이블 열 삭제
**산출물**
`Assets/_Library/_Core/GenerateScript/Type_CharacterTable.cs`
`Assets/_Library/_Core/GenerateScript/Type_EnemyTable.cs`
`Assets/_Library/_Core/GenerateScript/Type_BossTable.cs`
**작업내용**
- `table_data patch`로 `Character.InputBuffer`·`Enemy.Icon`·`Boss.Icon`을 제거했으며 3회 모두 `success:true`였다.
- `table_excel get` 실측에서 `Character` 2행·`Enemy` 3행·`Boss` 2행에 삭제 열 값이 없고 나머지 값은 유지됐다.

### 데이터 전체 익스포트
**산출물**
`Assets/_Library/_Core/Resources/Table/TableCharacter.json`
`Assets/_Library/_Core/Resources/Table/TableEnemy.json`
`Assets/_Library/_Core/Resources/Table/TableBoss.json`
**작업내용**
- `type_manage` 1회, `table_data` 1회, `table_excel` 7회, `const_data` 1회, `const_excel` 1회 export가 모두 `success:true`였다.
- 생성 코드·JSON에서 고정값 3개·`InputBuffer`·적/보스 `Icon` 검색 결과는 0건이다.

## 비고
- 컴파일은 업무지시서 조건에 따라 `Work_6` 씬 검증에서 수행한다.
- `confirmed`·`reuse`는 변경하지 않았다.
