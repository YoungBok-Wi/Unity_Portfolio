# [게임개발_구성_컨셉_밸런스_작성] "넉백 스턴 설정 소스 정정" 업무 레포트

## 요약
- `Balance`의 피격 경직 정본을 제거된 `Battle_HitStunSec` 고정값에서 `LocalGameManager.KnockbackStunSec` 인스펙터 설정으로 정정했습니다.
- 기본값 `0.1초`, 넉백 완료 후 FSM 행동·이동 입력 무시, 보스 면역 계약은 유지했습니다.
- `concept_manage verify` 결과가 `success=true`이고, 코드 소비 경로와 문서에서 기존 상수 참조가 남지 않았습니다.

## 완료업무

### 넉백 스턴 계약 대조
**산출물**
`Assets/__Game/Game/Script/LocalGameManager.cs`
`Assets/__Game/Unit/Script/FSMState_UnitKnockback.cs`
**작업내용**
- `m_KnockbackStunSec=0.1` 인스펙터 필드와 `KnockbackStunSec` 공개 속성을 FSM 상태가 소비하는 것을 확인했습니다.

### 밸런스 컨셉 정정
**산출물**
`_Data/Concept/Balance/concept.md`
**작업내용**
- 피격 경직의 설정 소스를 `LocalGameManager` 전투 설정 인스펙터 `KnockbackStunSec`로 바꿨습니다.
- `Battle_HitStunSec` 문자열이 컨셉과 소비 코드에서 `0건`임을 확인했습니다.

### 밸런스 컨셉 검증
**산출물**
`_Data/Concept/Balance/concept.md`
**작업내용**
- `concept_manage verify`가 `success=true`를 반환했습니다.

## 비고
- 대상 — 업무 3의 `게임개발_구성_컨셉_게임_작성`·프리셋 질문 체인. 조건 — 상위 게임 전제와 프리셋은 변경 대상이 아니면 제외. 실측 근거 — `Game`은 곡선·경직 시간을 인스펙터에서 조정하도록 이미 정의했고 실제 모듈 필드도 일치합니다.
