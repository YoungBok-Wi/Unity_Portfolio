# 업무지시서

## 1. 보스 시트 현황·실측

**대상 스킬**: 게임개발_구성_리소스_질문

**"taskContent"**: `AnimationSheet/AnimationSheet_Casual_Boss` Pineapple 5동작(Idle·Move·Attack1·Attack2·Die) 파일 entry·프롬프트·pool·select·산출 경로 조회, 프레임별 잉크 높이 실측

**업무**

- 근거: `_Data/Job/Job_003/Work_7/result.md` `## 비고` 결함 ⑦(화면 219px, 규격 237±15 하한 미달), `리소스컨셉` "규격 AnimationSheet_Casual_Boss" 기준 높이 224px(캔버스 384)
- 실측: 각 프레임 PNG의 알파 경계로 잉크 높이(px)를 구해 동작별 최대·최소를 표로 남긴다. 기준 224px 대비 미달 동작만 재제작 대상으로 확정한다(전부 충족이면 이후 업무를 건너뛰고 "리소스 결함 아님 — 프리팹·카메라 쪽 확인 필요"로 보고)

## 2. 프롬프트 구성

**대상 스킬**: 게임개발_구성_리소스_파일_구성

**"taskContent"**: 재제작 대상 동작의 프롬프트에 기준 높이 224px(캔버스 세로 58%)·하단 피벗·Pumpkin과 같은 서열을 명시

**업무**

- 기존 프롬프트 문맥·화풍 문장은 유지하고 크기 규격 문장만 보강한다. `reuse`·`inAsset`·`select`는 이 단계에서 바꾸지 않는다

## 3. 시트 재제작

**대상 스킬**: 게임개발_구성_리소스_파일_애니메이션시트_제작

**"taskContent"**: 재제작 대상 동작 시트 생성(GPT 즉시)·프레임 분할

**업무**

- 완료 기준: 산출 프레임 잉크 높이 224±8px, 피벗 하단 중앙, 기존 프레임 수 유지(Idle 4·그 외 6)
- 생성 한도(Codex 사용량)·워커 실패 시 재시도 1회 후 그 사유와 남은 대상을 보고하고 중단한다(우회 금지). 이 경우 이후 업무는 미수행으로 남긴다

## 4. 반입·익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"taskContent"**: 분할 프레임 pool 반입·`select` 갱신 후 `게임개발_구성_리소스_파일_익스포트`로 `Resources/SpriteAnim` 반영

**업무**

- 익스포트 후 `.meta` 임포트 설정(피벗 하단 중앙 (0.5, 0)·PPU 128·`isReadable`)이 기존 프레임과 같은지 실측한다(`AutoTextureSettingOnImport.cs`에 `SpriteAnim` 규칙이 없어 재보정 필요 가능 — 다르면 기존 프레임 `.meta` 값으로 맞추고 사실을 보고)
- 완료 기준: 147건 로드 null 0(기존 실측 방식), `Object_Boss_Pineapple` 스폰 화면 높이 222~252px
- `confirmed`·`reuse` 무변경. DataMCP export 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
