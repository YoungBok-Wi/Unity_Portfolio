# 업무지시서

## 1. 팝업 코드 수정

**대상 스킬**: 게임개발_프리셋_파일_팝업_코드_작성

**"taskContent"**: `Popup_Result` 해금 라벨을 `Text_Core_GunUnlocked`로 교체, 로비 해금 알림 문구 확인

**업무**

- 근거: `_Data/Job/Job_002/Work_7/result.md` `## 비고` "[데이터] 해금 알림·결과 라벨 문구", Work_4 신설 ID
- `Popup_Result` 해금 라벨은 `Text_Core_GunUnlocked`, 로비 잠금 카드 설명은 기존 `Text_Core_GunUnlock`(`{0}`) 유지
- 후속 `유니티엔진_컴파일_실행` 통과

## 2. 오브젝트 코드·구성 수정

**대상 스킬**: 게임개발_프리셋_파일_오브젝트_코드_2D_사이드뷰_캐릭터_작성

**"taskContent"**: 피벗 정본에 따른 플레이어·적·보스 프리팹 콜라이더 오프셋 조정과 접지 확인

**업무**

- 근거: `_Data/Job/Job_003/Work_1/result.md`(피벗·콜라이더 조정 주체), `Work_2`(재임포트 결과), `Work_3`(접지 판정 수정)
- 정본이 프리팹 콜라이더 조정을 요구하면 `Object_Player_{Knife,Gun}`·`Object_Enemy_*`·`Object_Boss_*` 콜라이더 오프셋·크기를 잉크 기준으로 맞추고(`게임개발_프리셋_파일_오브젝트_구성`), 플레이 실측으로 접지·발 위치를 확인한다
- `게임개발_프리셋_파일_익스포트`로 반영. 완료 기준: 컴파일 에러 0, 익스포트 success, 두 씬 플레이 진입 에러 0, 스폰 직후 `Idle` 재생·적 발이 바닥선에 닿음. 사용자에게 질문하지 않는다. `confirmed`·`reuse` 값은 변경하지 않으며 DataMCP 무응답 시 `Fallback` 순서를 따른다
