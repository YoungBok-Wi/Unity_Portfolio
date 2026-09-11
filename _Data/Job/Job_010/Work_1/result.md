# [게임개발_모듈_폴더_작성] "넉백 Hit 클립 예외와 이펙트 중단 수정" 업무 레포트

## 요약
- `FSMState_UnitKnockback.OnStart`의 공통 `Hit` 재생을 제거해 일반 적의 클립 누락 예외를 해소했다.
- 플레이어 `Hit` 애니메이션은 `Object_PlayerBase.OnHit`의 기존 재생 경로를 유지한다.
- `module_manage export`는 `success:true`, Unity 컴파일은 `status:completed`·`failed:false`, 콘솔 에러는 0건이다.

## 완료업무

### 넉백 피격 흐름 수정
**산출물**
`Assets/__Game/Unit/Script/FSMState_UnitKnockback.cs`
`Assets/__Game/Unit/module.md`
**작업내용**
- `FSMState_UnitKnockback`은 설계대로 넉백 이동과 경직만 처리하며 애니메이션 클립을 요구하지 않는다.
- `Object_PlayerBase.cs:66`의 `OnHit`이 플레이어 `Hit`을 재생하므로 기존 플레이어 피격 표현은 유지된다.
- 프리팹 실측에서 `Object_Player_Knife`·`Object_Player_Gun`은 `Hit`을 보유하고, `Object_Enemy_Apple`·`Object_Enemy_Watermelon`·`Object_Enemy_Banana`는 `Move`·`Attack`·`Die`만 보유한다.
- `LocalGameManager.cs:477`의 `TakeHit` 뒤 `LocalGameManager.cs:479`에서 명중 이펙트를 재생하므로, 넉백 예외 제거로 이펙트 호출이 중단되지 않는다.
- `module.md`의 범용 `target.Anim.Play(UnitConst.AnimHit, false)` 사용 예시를 제거해 공통 유닛 계약과 일치시켰다.

### export와 컴파일 검증
**산출물**
`Assets/__Game/Unit`
**작업내용**
- `module_manage export` 응답은 `success:true`다.
- `recompile_status` 응답은 `status:completed`·`failed:false`·`errors:[]`다.
- `get_console_logs` 응답은 `total:0`·`returned:0`이다.
