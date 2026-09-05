# Job_007 업무 맥락

## 목적

폴리싱 6회차 — Job_006 재QA 신규 발견 2건(⑳ 일본어 한자 글리프 결손, ㉑ GC 매 프레임 할당 예산 초과)을 수정하고 재QA·재점검·커밋한다. 재점검에서 더 발견되지 않으면 반복을 끝낸다.

## 사용자 지시 원문

"지금상태에서도 또 AI선에서 발견 가능한것들 찾고 그거 수정하는거 반복해줘. 더이상 문제가 발견되지 않을때까지 반복해줘" (Job_006 이월)

## 운영 규칙

- Job_005·006과 동일: `직접` 모드, 셋업 금지, `confirmed`·`reuse` 무변경, 라이브러리 수정 금지, 자율 진행, 회차 종료 시 README 1줄

## 대상 (Job_006 Work_5 비고)

- ⑳ [리소스·유니티엔진 에셋 폰트] TMP 폰트 에셋 `Assets/TextMesh Pro/Resources/Fonts & Materials/DefaultFont.asset`(원본 `Font_Casual_GyeonggiTitle_Light_Default.ttf`, Dynamic, 폴백 `[]`)·`DefaultFont_Bold.asset`에 한자 글리프 없음 → 일본어 문구 한자 전건 □. 해소: OFL 서체 Noto Sans JP(`_Temp/Work_1_J7/NotoSansJP-Regular.otf` 4.5MB, SIL OFL 1.1 — `LICENSE_OFL.txt`)를 리소스 타입 `Font_Casual_NotoSansJP`로 등록·익스포트하고 TMP 폰트 에셋(Dynamic) 생성 후 두 DefaultFont의 `m_FallbackFontAssetTable`에 연결. `리소스컨셉` 서체 항목에 일본어 폴백 서체 추가
- ㉑ [모듈 Battle] GC 5.6~13.3KB/프레임 — `Object_Projectile.Update` `Physics2D.OverlapCircleAll` 매 프레임 배열 할당(활성 투사체당) → `Physics2D.OverlapCircle(pos, r, ContactFilter2D.NoFilter, List)` 재사용 버퍼, 스플래터·궤적·히트·낙하물 `Instantiate/Destroy` → 라이브러리 `ObjectPool` 풀링(수명 뒤 반납 코루틴)

## 체인묶음 대응

- [컨셉] 기준 `게임개발_구성_컨셉_리소스_작성`: 컨셉_질문 → 리소스_작성 → 리소스_검증
- [리소스] 기준 `게임개발_구성_리소스_타입_생성`: 리소스_질문 → 타입_생성 → 타입_구성 → 파일_생성 → 파일_업로드 → 파일_익스포트 (제외: 파일_구성 — 프롬프트 없음, 이미지_제작 — 서체는 반입)
- [유니티엔진] 기준 `유니티엔진_에셋_폰트_구성`: 에셋_질문 → 폰트_구성(신규 폰트 에셋 생성 + DefaultFont 2건 폴백) → 재임포트 → 에셋_검증 (제외: 에셋_생성 — 폰트 에셋은 폰트_구성의 eval 경로가 생성)
- [모듈] 기준 `게임개발_모듈_폴더_작성`: 작성(코드·프리팹) → 컴파일 → 익스포트
- [유니티게임QA] 기준 `QA_유니티게임개발_플레이테스트_테스트`: 질문 → 계획 → 치트 → 테스트 + 성능테스트_테스트(GC 재측정) + 재점검(unused·Missing·콘솔)
- [깃] 기준 `깃_커밋_실행`: 커밋 → 푸시

## 체크리스트 ↔ Work

- c01 → Work_1(컨셉) / c02 → Work_2(리소스) / c03 → Work_3(엔진 폰트) / c04 → Work_4(모듈) / c05·c06 → Work_5(QA) / c07 → Work_6(깃)
