# [오케스트레이터_워커_실행] "Job_001 Work_2 데이터 테이블·고정값 구조·값 작성과 익스포트" 업무 레포트

## 요약
- Work 판정: 합격 — 업무 1·2·3 전부 완료 기준 충족 (익스포트 11회 호출 전부 `{"success":true}`, `recompile_status` `status=completed`·`failed=false`, `get_console_logs` `total=0`)
- 테이블 6건(`Character`·`Enemy`·`Boss`·`Room`·`Ability`·`Wave`) 필드 구조 patch·행 입력 완료 — 행 수 Character 2·Enemy 3·Boss 2·Room 4·Ability 6·Wave 27 (`table_excel` get 실측), 행 ID는 `게임컨셉` 정본 ID
- `Text` 테이블 `Core` 시트에 표시 텍스트 28행 추가 (캐릭터·적 설명 5, 보스 이름·설명 4, 방 이름·설명 8, 능력 이름·설명 12 — Kor·Eng 입력, Jap 빈 값). 캐릭터·적 이름은 기존 행 `Text_Core_WeaponKnife`·`Text_Core_WeaponGun`·`Text_Core_EnemyApple`·`Text_Core_EnemyWatermelon`·`Text_Core_EnemyBanana` 재사용
- 고정값 15건 타입·값 입력 완료 — 기존 10건에 타입 지정, 누락분 5건(`Ability_ChoiceCount`·`Room_ChoiceSet1`~`Room_ChoiceSet4`) 신규 등록 (`const_data` get·`const_excel` get `_Core` 실측)
- 리소스 결손: 아이콘 참조 `Icon_Casual_Weapon/Knife`·`Gun`, `Icon_Casual_Upgrade/*` 전부 `resource_file` path "존재하지 않는 리소스" — 전 테이블 `Icon` 필드를 빈 값으로 두고 보고 (`## 비고`)
- 생성 코드 실측: `Assets/_Library/_Core/GenerateScript/Table_{테이블}.cs`·`Type_{테이블}Table.cs` 각 6건, `Table_Const.cs`에 15 프로퍼티, `TableManager_Generate.cs`에 `Ability`·`Boss`·`Character`·`Enemy`·`Room`·`Wave` 프로퍼티 등록; 런타임 값 `Assets/_Library/_Core/Resources/Table/Table{테이블}.json` 8건

## 완료업무

### 데이터 테이블 구조·값 작성
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Table\Character\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Character\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Enemy\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Enemy\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Boss\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Boss\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Room\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Room\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Ability\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Ability\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Wave\table.json`
`C:\_Projects\Unity_Portfolio\_Data\Table\Wave\Core.xlsx`
`C:\_Projects\Unity_Portfolio\_Data\Table\Text\Core.xlsx`
**작업내용**
- 수행 스킬: `게임개발_구성_데이터_테이블_구성`(child `{}`) → `게임개발_구성_데이터_테이블_작성`. `게임개발_구성_데이터_테이블_생성` 건너뜀 (조건: order.md "골격이 누락돼 있으면 먼저 등록", 실측: `table_data` list에 6건 전부 실재·`values` 빈 객체)
- 선행 조회: `게임개발_구성_데이터_질문`→`테이블_질문`(`table_data` list 7건, get 6건 `values={}`·`reuse=add`·`confirmed={}`; `Text`는 `reuse=fixed`·시트 `Core` 포함), `게임개발_구성_컨셉_질문`→`밸런스_질문`·`게임_질문`(`concept_manage` path `_Data/Concept/Balance/concept.md`·`_Data/Concept/Game/concept.md` 읽음). `type_manage` list — 게임 타입 없음(Default·`TextData`·UnityEngine 타입만)이라 열거형 필드(`WeaponType`·`Group`·`AttackType`·`Category`·`StackMode`·`Skill1Id`·`Skill2Id`)는 `string`으로 두고 허용값을 description에 기재 (타입 정의 등록 선행 필요 — `## 비고`)
- 구조 (`table_data` patch 6건 `success`, get 재조회 반영): Character 21필드(무기 종류·HP·콤보 3단 공격력·공격 주기·선입력·이동속도·박스 폭/높이·투사체 속도·명중 상한·관통·일반/마무리 넉백 거리·시간·해금 방 순번), Enemy 15필드(그룹·HP·공격력·주기·이동속도·정지 거리·사거리·투사체 속도·히트박스·간격·넉백 배율·Crumb), Boss 28필드(유형·HP·이동속도·Enrage 이동속도·유지 거리·Enrage 비율·Crumb·Skill1(주 패턴) 9·Skill2(보조 패턴) 9), Room 3필드(Name·Desc·Icon), Ability 8필드(계열·스택 방식·Value·ValueSub·MaxStack), Wave 9필드(RoomMin·RoomMax·WaveIndex·적 슬롯 3×ID·Count). 배열 필드 없음 — "배열 필드(isArray) 제약" 규칙으로 Wave 적 슬롯·Boss 패턴을 고정 슬롯으로 설계
- 행 값 (`table_excel` patch 6건 `success`, get 실측 일치): `밸런스컨셉` `## 핵심 메카닉` 수치를 그대로 입력 — Knife HP 100/12·12·18/0.5s/5.0u/s/박스 2.0×1.5/명중 5/넉백 0.4u·0.15s·0.8u·0.2s, Gun HP 80/6/0.25s/5.5u/s/비행 8.0/속도 15/관통 0/넉백 0.3u·0.1s/UnlockRoom 5; Apple 30/8/1.0s/3.5/0.8u, Watermelon 90/12/1.5s/2.0/1.0u/넉백 0.5, Banana 25/6/2.0s/3.0/유지 5.0/사거리 7.0/투사체 8.0; Pumpkin 600/3.0(Enrage 4.0)/Slam 0.8s·25·3.0u·3.0s(Enrage 2.0s)/Charge 0.6s·18·4.0u·8.0u/s, Pineapple 450/3.5/유지 6.0/Spike 0.5s·8×3연발 0.2s·10u/s·2.5s/Rain 1.0s·20·폭 2.0·2곳(Enrage 3곳)·5.0s; Ability Attack 0.2·상한 5, AttackSpeed 0.12·4, MaxHp 25(+즉시 25)·무제한, MoveSpeed 0.1·3, HealMacaron 0.6·즉시, MultiHit 2(Gun 1)·3; Crumb 낙하 2/4/3/보스 30
- 자율 확정 값 (문서 미기재분): Banana `HitboxWidth` 0.8·`Spacing` 0.6(Apple과 동일 — 원거리라 근접 슬롯 미진입), Pumpkin `Skill2Interval` 3.0(Slam 패턴 간격과 동일), Pineapple `EnrageMoveSpeed` 3.5(문서에 Enrage 이동속도 변경 없음)·`Skill1Range` 0(투사체 패턴이라 미사용), Wave 순번별 구성 — R1 Apple 3·4, R2 Apple 3+Watermelon 1(×2), R3 Apple 2+Watermelon 1+Banana 1(×2), R4~R9 웨이브 3개(적 수 3+floor(n/2) = 5·5·6·6·7·7), R10+(RoomMax 99) 8마리 Apple 4+Watermelon 2+Banana 2
- 진행 변화 구간 판정 (Wave): 순번 2(Watermelon 종류 추가)·순번 3(Banana 종류 추가)·순번 4(웨이브 수 2→3 배치 주기 변경) — 성격 변화 3건, 순번 5 이후는 수량 단조 증가. `밸런스컨셉` 검산 "웨이브당 적 수 상한" 8 = `Battle_MaxEnemyOnScreen` 8과 일치
- 텍스트: `Text` `Core` 시트 patch `success`, get `Text_Core_BossPumpkin` Kor "호박 대장"·`Text_Core_AbilityMultiHitDesc` Kor "나이프 명중 상한 +2, 건 관통 +1 (최대 3중첩)" 실측. 능력 이름은 기존 `Text_Core_Card*Name` 한글명(매운맛·손놀림·든든한 한 끼·경쾌한 발·회복 마카롱·다지기) 재사용, 설명은 `밸런스컨셉` 수치로 새 행 작성 (기존 `Card*Desc` 행은 +8%·5중첩 등 다른 게임 값이라 참조하지 않음)
- 리소스 참조값: `resource_file` path `Icon/Icon_Casual_Weapon` [`Knife`,`Gun`]·`Icon/Icon_Casual_Upgrade` 6건 전부 `errors` "존재하지 않는 리소스"; `resource_file` list(`Icon`)에 두 타입 미출현, `_Data/Resource/File/Icon_Casual_Weapon`은 `type.json`만 실재, `Assets/__Game/_Core/Resources/Icon` 빈 폴더 → 규칙 "대응 리소스가 없으면 빈 값으로 두고 보고"로 `Icon` 전 행 빈 값. 로드 실측은 참조값 0건이라 대상 아님
- 값 판정 항목: 참조 실재 — 텍스트 행 ID 전건 `Text` `Core` 시트 실재(신규 28 + 기존 5), 아이콘 결손 보고; 밸런스 정합 — 종수(캐릭터 2·적 3·보스 2·방 4·능력 6) 정본 일치, 수치 문서값 일치; 소비·명명 — 소비 코드 없음(`grep Table_(Character|…)` `Assets/__Game` 0건, 모듈 `Room`·`Battle`·`Character`는 Work_1_1 골격), 아이콘 계열명 `Icon_Casual_*`는 `리소스컨셉` 계열과 일치
- 더미 행 제거·미작성 전수 조회 건너뜀 (조건: 스킬 "content가 개별 행 ID로 확정되어 있으면 건너뛴다"·"더미 행이 있을 때만", 실측: 6 테이블 `table_excel` get 초기값 `{}`)

### 데이터 고정값 정의·값 작성
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Const\const.json`
`C:\_Projects\Unity_Portfolio\_Data\Const\_Core.xlsx`
**작업내용**
- 수행 스킬: `게임개발_구성_데이터_고정값_구성`(child `{}`) → `게임개발_구성_데이터_고정값_생성`(누락 5건) → `게임개발_구성_데이터_고정값_작성`
- 선행 조회: `게임개발_구성_데이터_질문`→`고정값_질문`(`const_data` get 10건 `type=""`, `const_excel` list `_Core`, get 전건 빈 값)
- 신규 등록 5건 (`const_data` patch description → type patch): `Ability_ChoiceCount`(int)·`Room_ChoiceSet1`~`4`(string, 좌/우 Room 행 ID를 `/`로 연결). order.md 예시 "런 최대 방 수"는 `밸런스컨셉`에 값이 없어 미등록 (`Room_BossForce` 10 이후 [Battle/Boss] 고정으로 상한을 관리)
- 타입·값 (`const_data` get·`const_excel` get `_Core` 실측): `Room_GrowthHp` float 0.15, `Room_GrowthAtk` float 0.1, `Room_GunUnlock` int 5, `Room_BossMin` int 6, `Room_BossForce` int 10, `Room_HealRatio` float 0.5, `Battle_MaxEnemyOnScreen` int 8, `Battle_MeleeSlotPerSide` int 2, `Ability_RerollBaseCost` int 10, `Ability_RerollCostStep` int 5, `Ability_ChoiceCount` int 3, `Room_ChoiceSet1` "Battle/Heal", `Room_ChoiceSet2` "Heal/Ability", `Room_ChoiceSet3` "Battle/Boss", `Room_ChoiceSet4` "Battle/Battle" — 근거 `밸런스컨셉` `### 해금 곡선`·`### 난이도 곡선`·`**적 그룹 공통**`·`**Crumb**`·`**방 종류**`
- 값 판정 항목: 참조 실재 — `Room_ChoiceSet*`의 Room ID 4종 `Room` 테이블 행 실재, 리소스 참조 대상 아님; 밸런스 정합 — 문서값 일치; 소비·명명 — 소비 코드 없음(생성 코드 `Table_Const.cs` 프로퍼티만), 리소스 계열 대상 아님

### 데이터 익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\GenerateScript`
`C:\_Projects\Unity_Portfolio\Assets\_Library\_Core\Resources\Table`
**작업내용**
- 수행 스킬: `게임개발_구성_데이터_익스포트`(child `{}`) — `type_manage` export 1, `table_data` export 1, `table_excel` export 7(`Character`·`Enemy`·`Boss`·`Room`·`Ability`·`Wave`·`Text`), `const_data` export 1, `const_excel` export 1 전부 `{"success":true}`·errors 없음
- 산출 실측: `GenerateScript`에 `Table_*.cs` 8·`Type_*Table.cs` 7·`TableManager_Generate.cs`(신규 테이블 6 프로퍼티 등록), `Resources/Table`에 `Table{Ability|Boss|Character|Const|Enemy|Room|Text|Wave}.json` 8건; `TableConst.json` 15항목, `TableText.json`에 신규 행 ID 24건 grep 일치(Ability·Room·Boss 접두)
- 컴파일 (`유니티엔진_컴파일_실행` 절차, "CLI 컴파일 검증" 규칙): `list_open_scenes` `Scene_Game` `isDirty=false` → `clear_console` → `recompile` 03:12:27 → `recompile_status` `{"status":"completed","failed":false,"errors":[]}` → `get_console_logs` `total=0`; `Library/ScriptAssemblies/Game.dll`·`Library.dll` 03:12:36 갱신. 콘솔 버퍼 비움 외 되돌릴 대상 없음
- `confirmed`·`reuse` 값은 변경하지 않음 (`Text` `reuse=fixed` 유지, 6 테이블 `reuse=add`·`confirmed={}` 유지)

## 비고
- 아이콘 리소스 결손: Work_1 레포트의 "`Icon_Casual_Weapon/Knife`·`Gun`, `Icon_Casual_Upgrade/*` 실재"는 실측과 불일치 — 타입만 있고 entry가 없다 (`resource_file` path errors, `_Data/Resource/File/Icon_Casual_Weapon`에 `type.json`만). `Room` 아이콘 타입 `Icon_Casual_Room`은 `resource_type` list(`Icon`)에 미등록. 후속 Work에서 `게임개발_구성_리소스_파일_생성`~`업로드`로 entry 제작 후 각 테이블 `Icon` 값(익스포트 파일명 `Icon_Casual_Weapon_Knife` 등)을 `게임개발_구성_데이터_테이블_작성`으로 채워야 한다 (행 ID가 `Icon_{행 ID}` 폴백 규칙과 다르므로 필드값 필수)
- 열거형 필드는 게임 데이터 타입이 없어 `string`으로 두었다 — Enum 타입(`WeaponType`·`EnemyGroup`·`StackMode` 등)을 `게임개발_구성_데이터_타입_생성`으로 등록해 교체하려면 타입 등록이 선행돼야 한다 (현재 소비 코드 없어 교체 비용 없음)
- `Text` `Core` 시트의 이전 게임 잔재 행(`Text_Core_Card*`·`Text_Core_Weapon{RollingPin|Whisk|Skewer|Sprinkle}`·`Text_Core_Difficulty*`·미션·상점·뽑기 문구 등)은 삭제 지시가 없어 그대로 두었다 (`reuse=fixed`)
- 테이블 `Name`·`Desc`는 `string`(Text 행 ID)이며 `TextData` 타입을 쓰지 않았다 — 소비 코드가 `TableManager.Text.Data[id]`로 조회하는 구조를 전제한다
