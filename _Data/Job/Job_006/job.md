# Job_006 업무 맥락

## 목적

폴리싱 5회차 — 메인 세션이 전 영역을 직접 점검(정본 대 구현 대조·데이터 전수·미사용 에셋·프로젝트 설정·콘솔·씬 무결)해 발견한 문제를 일괄 수정하고 재QA·커밋한다. 재점검·재평가에서 더 발견되지 않을 때까지 회차를 반복한다(회차당 Job 1개).

## 사용자 지시 원문

"지금상태에서도 또 AI선에서 발견 가능한것들 찾고 그거 수정하는거 반복해줘. 더이상 문제가 발견되지 않을때까지 반복해줘" (세션 지시: 폴리싱 작업, `직접` 모드, README `폴리싱 작업` 절에 지시 기록)

## 운영 규칙

- Job_005와 동일: `직접` 모드, 셋업(`editor_util setup`) 금지, `confirmed`·`reuse` 무변경, 라이브러리 수정 금지, 자율 진행, 회차 종료 시 README 1줄

## 점검 결과 (2026-09-06, 메인 세션 직접 확인)

- 통과: 컨셉 verify 5문서 `success`, Battle·Room·Character 모듈 verify `success`, 컴파일 `up_to_date`·콘솔 로그 0, 두 씬 Missing 스크립트 0(오브젝트 254·349), `[Global]` Missing 0, `git` clean
- ⑭ [모듈 Battle] 정본 미배선 연출·SFX 4건 — `게임컨셉` 183행 처치 시 `Illust_Casual_Splatter/Death` 동시 재생, 187행 Knife 휘두름 `Illust_Casual_Slash/Knife` 궤적, 199행 해금 `SFX_Casual_Progress/Unlock`·능력 획득 `SFX_Casual_Progress/LevelUp`. 에셋 4건 실재하나 참조 0(`project_manage unused`), 코드에 재생 지점 없음
- ⑮ [모듈 Battle] Crumb 낙하·수거 미구현 — `게임컨셉` 41·113·183행 "처치 시 Crumb 낙하해 플레이어가 주우면 획득". 현행 `OnUnitDied` 즉시 적립(`단순화:` 주석 1건)
- ⑯ [데이터] `Text` 테이블 일본어 빈 값 44행 중 게임 사용 33행(`Text_Core_WeaponKnifeDesc`~`Text_Core_GunUnlocked`) — `LanguageConst.LanguageList`에 Japanese 포함이라 일본어 OS에서 빈 문구
- ⑰ [데이터] `Text` 테이블 미참조 112행(Core 시트 95 — Card*·Weapon*·Shop*·Mission*·Attend*·Difficulty*·Currency*·Tab*·Unlock*·Orange 등 이전 템플릿 잔재, `Popup_Setting` 시트 8, `Popup` 시트 9) — 런타임 JSON 부풀림
- ⑱ [리소스] 미사용 에셋 — `Assets/__Game/_Core/Image` 44건(UI_Casual_* 38·UI_Common_Shape 4·Icon_Casual_Stat_Strength/Time·Illust_Casual_Shadow_Ellipse), `Resources/Image` 무기 일러스트 4건(RollingPin·Skewer·Sprinkle·Whisk). Splatter·Slash·SFX 2건은 ⑭ 배선 후 사용
- ⑲ [유니티엔진 설정] `companyName` "DefaultCompany" — 빌드 메타데이터 기본값 잔존 (productName "Kitchen Riot"·0.1.0 정상)
- 범위 밖(스킬 없음): 플레이어 빌드 실행 검증 — 빌드 스킬이 없어 미커버로 보고

## 체인묶음 대응

- [데이터] 기준 `게임개발_구성_데이터_테이블_텍스트_작성`: 컨셉_질문 → 테이블_텍스트_작성(일본어 33행 입력·잔재 112행 제거) → 데이터_익스포트
- [모듈] 기준 `게임개발_모듈_폴더_작성`: 기획_작성 → 코드_작성(+매니저_로컬) → 프리팹_작성 → 컴파일 → 익스포트 (제외: 생성·구성 — 등록·메타 변동 없음)
- [리소스] 기준 `게임개발_구성_리소스_파일_구성`(cleanup): 리소스_질문 → 파일_구성 미사용 정리(승인: 사용자 지시 "발견한 것 수정") → 에셋_검증 (제외: 생성·제작·업로드·익스포트 — 산출물 변동 없음)
- [유니티엔진] 기준 `유니티엔진_설정_구성`: 설정_질문 → 설정_구성(companyName) → 설정_검증
- [유니티게임QA] 기준 `QA_유니티게임개발_플레이테스트_테스트`: 질문 → 계획 → 치트_작성 → 테스트 + `QA_유니티게임개발_성능테스트_테스트`(성능 예산)
- [깃] 기준 `깃_커밋_실행`: 커밋 → 푸시

## 체크리스트 ↔ Work

- c01 → Work_1(데이터) / c02·c03 → Work_2(모듈) / c04 → Work_3(리소스) / c05 → Work_4(엔진 설정) / c06·c07 → Work_5(QA) / c08 → Work_6(깃)
