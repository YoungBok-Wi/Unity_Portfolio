# 업무지시서

## 1. 시트 현황·잉크 실측

**대상 스킬**: 게임개발_구성_리소스_질문

**"question"**: `AnimationSheet/AnimationSheet_Casual_Enemy` Banana(Move·Attack·Die) 파일 entry·프롬프트·pool·select·산출 경로, 그리고 Work_1 `result.md` `## 비고`가 지목한 보스 재제작 대상 동작의 entry

**업무**

- 근거: `_Data/Job/Job_004/Work_6/result.md` `## 비고` 결함 ⑩(`Enemy_Banana_Move` 잉크 장축 109px < Apple 113px, 정본 Banana 123px)·⑪(Work_1 개정 범위 밖 동작만)
- 실측: Banana 3동작 전 프레임 잉크 장축(px)을 표로 남긴다. 재제작 대상 = Move(필수) + 장축 < 113px인 다른 동작 + Work_1 지목 보스 동작. Work_1이 "재제작 대상 없음"이면 보스는 제외

## 2. 프롬프트 구성

**대상 스킬**: 게임개발_구성_리소스_파일_구성

**"content"**: 재제작 대상 entry 프롬프트에 크기 규격(Banana 잉크 장축 123px·캔버스 256·접지선 183행, 보스는 기준 224px·캔버스 384·하단 접지)과 "Apple보다 크고 Watermelon보다 작다" 서열을 명시

**업무**

- 기존 화풍·문맥 문장은 유지하고 크기 규격 문장만 보강한다. `reuse`·`inAsset`·`select`는 이 단계에서 바꾸지 않는다

## 3. 시트 재제작

**대상 스킬**: 게임개발_구성_리소스_파일_애니메이션시트_제작

**"content"**: 재제작 대상 시트 생성(GPT 즉시)·프레임 분할

**업무**

- 완료 기준: Banana Move 잉크 장축 123±6px(113 초과 필수), 접지선 183행 ±3, 프레임 수 유지(이동 1프레임 + 코드 회전 규격). 보스는 Work_1 개정 범위 안
- 생성 한도(Codex 사용량)·워커 실패 시 재시도 1회 후 사유와 남은 대상을 보고하고 중단한다(우회 금지). 이 경우 이후 업무는 미수행으로 남긴다

## 4. 반입·익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"content"**: 분할 프레임 pool 반입·`select` 갱신 후 `게임개발_구성_리소스_파일_익스포트`로 `Resources/SpriteAnim` 반영

**업무**

- 익스포트 후 `.meta` 임포트 설정(피벗·PPU 128·`isReadable`)이 기존 프레임과 같은지 실측한다 (다르면 기존 프레임 `.meta` 값으로 맞추고 사실을 보고)
- 완료 기준: 스프라이트 로드 null 0(Job_004 Work_3 실측 방식), `Object_Enemy_Banana` 스폰 화면 높이가 Apple보다 크다(수치)
- DataMCP `resource_file export`는 약 16분 동기 블로킹 이력(`_Data/Job/Job_004/Work_3_1/result.md`) — 무응답 시 `Fallback`(curl)·사본 실측으로 완료를 확인하고 대기 시간을 보고한다
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
