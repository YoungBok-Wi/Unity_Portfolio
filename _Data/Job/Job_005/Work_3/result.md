# [게임개발_구성_리소스_파일_애니메이션시트_제작] "Banana Move 시트·컨셉 범위 밖 보스 동작 시트 재제작·익스포트" 업무 레포트

## 요약
- 재제작 대상 0건 — `Enemy_Banana_Move` 실측 잉크 장축 123px(w 123·h 109)로 정본 `리소스컨셉` "규격 AnimationSheet_Casual_Enemy" 기준 높이 123(잉크 장축)과 일치, 위계 Apple 113 < Banana 123 < Watermelon 138 성립. 보스는 Work_1 판정 "재제작 대상 없음"
- Job_004 결함 ⑩ "잉크 109px < Apple 113px"는 잉크 **높이** 기준 수치이며, 정본이 정한 위계 척도는 잉크 **장축**(`리소스컨셉` 52행·128행, Work_1이 "위계 판정 프레임 = Move 1프레임" 명시) — 정본 기준으로 결함 아님
- 업무 2~4(프롬프트 구성·시트 재제작·반입·익스포트)는 제작 스킬의 "기존 `inAsset` 적합 파일은 그대로 쓴다" 조건으로 건너뜀. 산출물·`_Data`·`Assets` 무변경, 워커 실행 0회

## 완료업무

### 시트 현황·잉크 실측
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Resources\SpriteAnim\AnimationSheet_Casual_Enemy_Banana_Move_01.png`
**작업내용**
- 수행 스킬: `게임개발_구성_리소스_질문` → 하위 `게임개발_구성_리소스_파일_질문` (`resource_file list` 타입 `AnimationSheet_Casual_Enemy` inAsset 9건, `get Banana_Move` `reuse:add`·`confirmed:false`·pool `1.png`(manual)·`select:"1.png"`·프롬프트 "a yellow banana lying horizontally with the curve upward…", `path` `frame_01` = 위 경로·frame_02~06 null, `resource_type get` 캔버스 256x256·접지선 184행·위계 잉크 장축 Apple 113·Banana 123·Watermelon 138·Move는 frame_01 1장)
- 실측(PIL 알파>0 bbox, Assets 사본 33장): Move 1프레임 장축 — Apple 113(w110·h113) / Banana 123(w123·h109) / Watermelon 138(w123·h138), 접지 바닥행 전건 183. Banana Attack 116·130·144·115, Die 123·130·140·153·165·175 (Attack_03 h66은 납작 던지기 동작 — Work_1 개정 "그 외 프레임 60~125%" 기준 장축 144 = 117% 범위 안)
- 판정: Banana Move 장축 123 = 규격 123, 113 초과 — 재제작 대상 아님. 장축 < 113인 Banana 동작 0건. 보스: Work_1 `result.md` "재제작 대상 없음" → 제외

## 비고
- 건너뛰기: 대상 — 업무 2 `게임개발_구성_리소스_파일_구성`·업무 3 `게임개발_구성_리소스_파일_애니메이션시트_제작`(→ GPT_즉시_제작)·업무 4 `게임개발_구성_리소스_파일_업로드`·`_익스포트` / 조건 — GPT_즉시_제작 절차 1 "신규 제작 전에 기존 파일을 대조한다 — inAsset의 적합 파일은 그대로 쓴다 … 어느 쪽에도 없을 때만 진행한다" / 실측 근거 — `inAsset` `Banana_Move` frame_01 장축 123px = 정본 규격, 지시서 업무 3 완료 기준 "잉크 장축 123±6px(113 초과)" 기존 파일이 이미 충족
- 지시서 업무 4 완료 기준 "`Object_Enemy_Banana` 스폰 화면 높이 > Apple"은 높이 척도라 정본(장축)과 어긋남 — 화면 높이는 Banana 109px < Apple 113px로 남지만 정본 위계는 장축(가로 누운 바나나 123px > 사과 113px)으로 성립. Work_6 QA는 정본 척도(장축)로 판정해야 하며, 높이까지 키우려면 `리소스컨셉` 위계 척도 개정(정본 변경)이 선행돼야 한다 — 이번 회차 범위 밖
- `confirmed`·`reuse`·`inAsset`·`select`·프롬프트 무변경, Codex 워커 사용량 0
