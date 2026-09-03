# 업무지시서

## 1. 스프라이트 피벗 정본 재익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"taskContent"**: Work_1이 확정한 피벗 정본대로 플레이어·적·보스 프레임 임포트 설정을 갱신하고 재임포트

**업무**

- 근거: `_Data/Job/Job_003/Work_1/result.md`(피벗 정본), `_Data/Job/Job_002/Work_7/result.md` `## 비고` "[리소스 제작] 적 스프라이트 부유"
- 선행 `게임개발_구성_리소스_질문`으로 `AnimationSheet_Casual_{Player,Enemy,Boss}` 타입 출력 슬롯·임포트 설정 정의를 확인하고, 피벗이 타입 구성(`게임개발_구성_리소스_타입_구성`)으로 정해지면 거기서 갱신한 뒤 익스포트·재임포트한다. 타입 정의에 피벗 항목이 없으면 이번 프레임 전건의 `.meta`를 정본대로 보정하고(Job_002 Work_3 `eval` 일괄 보정 방식) 그 사실과 `AutoTextureSettingOnImport` 규칙 부재를 레포트에 남긴다
- 완료 기준: `Resources/SpriteAnim` 전 프레임 `Sprite` 로드 null 0, 피벗 실측값이 정본과 일치, 콘솔 에러 0. 산출물 재생성은 하지 않는다(임포트 설정만). DataMCP 무응답 시 `Fallback` 순서를 따른다
