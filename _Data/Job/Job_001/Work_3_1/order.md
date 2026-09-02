# 업무지시서

## 1. 테이블 Icon 값 입력

**대상 스킬**: 게임개발_구성_데이터_테이블_작성

**"taskContent"**: Work_3가 익스포트한 아이콘 에셋명을 `Character`·`Ability`·`Room` 테이블의 `Icon` 필드에 입력하고 런타임 로드 실측

**업무**

- 값 근거: Work_3 레포트(`_Data/Job/Job_001/Work_3/result.md`) "리소스 타입·이미지 제작" 작업내용 끝의 테이블 `Icon` 값 표 — `Icon_Casual_Weapon_{Knife|Gun}`, `Icon_Casual_Upgrade_{Attack|AttackSpeed|MaxHp|MoveSpeed|HealMacaron|MultiHit}`, `Icon_Casual_Room_{Battle|Heal|Ability|Boss}` (`Assets/__Game/_Core/Resources/Icon/` 실재, `Resources.Load("Icon/{값}")` 통로)
- `Enemy`·`Boss` 테이블에 `Icon` 필드가 있으면 적 미리보기용으로 `AnimationSheet_Casual_{Enemy|Boss}_{ID}_Idle_01`(없으면 `Move_01`) 프레임명을 넣고, 필드가 없으면 값 입력 없이 레포트에 그 사실만 적는다 (구조 변경은 이 Work 범위 밖)
- 리소스 참조값은 런타임 로드 실측으로 검증하고 어긋난 값은 보정한다
- 완료 기준: 전 행 `Icon` 값이 실재 에셋과 일치

## 2. 데이터 익스포트

**대상 스킬**: 게임개발_구성_데이터_익스포트

**"taskContent"**: 값 확정분 익스포트로 런타임 값 반영

**업무**

- 익스포트 성공과 컴파일 에러 0건을 확인한다. `confirmed`·`reuse` 값은 변경하지 않으며 막힘은 우회하지 않고 보고한다
