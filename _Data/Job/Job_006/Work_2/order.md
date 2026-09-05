# 업무지시서

## 1. Battle 모듈 설계·코드·프리팹 수정

**대상 스킬**: 게임개발_모듈_폴더_작성

**"content"**: 정본 미배선 연출·SFX 4건 배선과 Crumb 낙하·수거 구현

**업무**

- 정본: `게임컨셉` 41행(Crumb 낙하·수거)·183행(처치: Die 애니메이션 + `Illust_Casual_Splatter/Death` 동시 재생, Crumb 낙하)·187행(Knife 휘두름 `Illust_Casual_Slash/Knife` 궤적)·199행(해금 `SFX_Casual_Progress/Unlock`, 능력 획득 `SFX_Casual_Progress/LevelUp`), `리소스컨셉` 182행(스플래터 소스 레드, 지면 잔존물 없음)
- ⑭-1 스플래터: `LocalBattleManager.OnUnitDied`에서 적·보스 사망 위치에 스플래터 프리팹을 띄운다 — `Assets/__Game/Battle/Prefab/HitEffect.prefab`(스크립트 없는 스프라이트 프리팹, `View` 자식)을 복제해 `Splatter.prefab`을 만들고 스프라이트를 `Illust_Casual_Splatter_Death`(guid `438a099813d8247469850627753b288a`)로, 수명은 사망 연출 0.5s와 같게. 인스펙터 필드 `m_SplatterPrefab`·`m_SplatterSec` 추가·`[LocalBattleManager].prefab` YAML 배선
- ⑭-2 Knife 궤적: `Object_Player_Knife.StartStep`이 부르는 공개 메서드 `LocalBattleManager.PlaySlashEffect(Vector2 _center, int _facing)`를 두고 `Slash.prefab`(HitEffect 복제, 스프라이트 `Illust_Casual_Slash_Knife` guid `cbae618a6cb815c4a8ca60b216f62312`, 좌향이면 X 반전)을 판정 박스 중심에 공격 주기 길이만큼 띄운다. 필드 `m_SlashPrefab`·`m_SlashSec`
- ⑭-3 SFX: `LocalBattleManager`에 `m_SfxLevelUp`·`m_SfxUnlock` 필드(guid `93487ab63a2d9dc4eb0e367ee6ee8578`·`a5e9c06c584b277469c11432f225d1f4`)를 두고 `AddAbility` 성공 시 LevelUp 재생, 공개 `PlayUnlockSfx()`를 `LocalRoomManager.ClearRoom`의 해금 분기(`OnRoomCleared` true)에서 호출 — `Room` 모듈은 이미 `Battle`을 참조하므로 `module.md` 참조 절만 갱신
- ⑮ Crumb 낙하·수거: `OnUnitDied`의 즉시 적립을 낙하 오브젝트로 바꾼다 — 새 클래스 `CrumbDrop : MonoBehaviour`(`Battle/Script/CrumbDrop.cs`, 모듈 코드 종류 "클래스")와 `CrumbDrop.prefab`(HitEffect 복제 + 스프라이트 `Icon_Casual_Currency_Crumb`, PPU 100 아이콘이라 스케일 0.4). 동작: 사망 위치에서 마리당 `CrumbDrop` 개수만큼(값 1) 위로 튀어 바닥 y(`Object_UnitBase` 발 위치)로 떨어지고, 플레이어와 x 거리 ≤ 0.6u면 수거(`BattleManager.AddCrumb(1)` + `HitApplied`와 별개), 3.0u 안이면 플레이어 쪽으로 8u/s 흡인, 방 클리어·`ClearUnits` 시 잔여 크럼은 즉시 전량 적립(잔존물 없음 — `리소스컨셉` 182행). 풀 없이 Instantiate/Destroy(웨이브당 최대 8마리 × 4 = 32개). 값·속도는 `BattleConst` 상수, 개수는 테이블 `CrumbDrop`
- `module.md` 내부기능·외부사용 갱신, `MCPDetail`에 `crumbDrops`(활성 낙하 수) 노출, `KillEnemies` 치트는 낙하분 자동 적립까지 책임(치트 적용 후 Crumb 즉시 반영 — QA 회귀 유지)
- 완료 기준(플레이 실측): 방 1 Apple 처치 시 스플래터 오브젝트 활성 ≥ 1·0.5s 후 소멸, Knife `StartStep(1)` 호출 시 `Slash(Clone)` 활성 1, 능력 선택 시 `[SoundManager] playing False→True`, 방 5 클리어 해금 시 재생 1회, 처치 후 낙하 오브젝트 생성·플레이어 접근 시 수거로 Crumb 증가·방 클리어 시 잔여 0
- 라이브러리 수정 금지, CS 템플릿 제약(`게임개발_모듈` 노드 규칙) 준수, 리터럴 금지(상수·테이블)

## 2. 컴파일·익스포트

**대상 스킬**: 유니티엔진_컴파일_실행

**"scope"**: 컴파일 통과 후 `게임개발_모듈_폴더_익스포트`로 원본 반영·재임포트 (`Room` 모듈도 export)

**업무**

- 완료 기준: `recompile_status completed`·콘솔 에러 0, `module_manage export` Battle·Room `success`, verify `success`, 플레이 실측으로 1번 업무 완료 기준 확인(종료 `stopped`·`Scene_Lobby isDirty:false`)
- 씬 셋업(`editor_util setup`) 실행 금지. `confirmed`·`reuse` 무변경. 라이브러리(`Assets/_Library/**`·`_Data/Module/Library/**`) 코드 수정 금지. DataMCP 무응답 시 `Fallback`. 사용자에게 질문하지 않는다
