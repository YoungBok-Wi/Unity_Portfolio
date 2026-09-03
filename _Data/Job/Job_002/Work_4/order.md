# 업무지시서

## 1. 모듈 결함 수정

**대상 스킬**: 게임개발_모듈_폴더_작성

**"taskContent"**: `Battle`·`Room` 모듈의 플레이테스트 결함 일괄 수정과 명중 통지 API 추가

**업무**

- 근거: `_Data/Job/Job_001/Work_6/result.md` `## 비고` "모듈" 항목, `밸런스컨셉`(Work_1 갱신본) 수치
- 수정 목록:
  - 피격 후 무입력 표류: `Object_UnitBase.TakeHit` 넉백 종료 후 속도 0으로 정지, `Object_PlayerBase` 입력 없을 때 수평 속도 0 (Gun "정지 연사" 준수 포함). 재현: 시작 → 무입력 10초 → x 0→4.4
  - 방 경계: 바닥 양끝에 벽 콜라이더 생성(카메라 클램프와 일치), 플레이어·적 모두 화면 밖 이동 불가
  - 근접 슬롯: `LocalBattleManager` `m_MeleeSlots` 판정 수정 — 빈 슬롯이 있으면 대기 거리에서 정지하지 않게
  - HitStop: `Time.timeScale` 소유자를 한 곳(`BattleManager` 등)으로 모아 일시정지·슬로모와 충돌하지 않게 — 복원값을 저장값 기준으로
  - 명중 통지 API: `SHit` 이벤트를 모듈 API로 노출(데미지 팝 HUD가 구독)
  - `LocalRoomManager.MCPCheatApply("ClearRoom")`: `Ended` 상태에서 무시
  - 방 이력 상한: `게임컨셉`(Work_1 갱신본) 정책대로 이력 데이터 제공 방식 조정
- `AutoTextureSettingOnImport.cs`의 `SpriteAnim` 임포트 규칙(Sprite·PPU 128·BottomCenter)은 에디터 스크립트 소속을 `게임개발_모듈_질문`으로 확인해 담당 모듈이 있으면 그 모듈 코드로 추가하고, 없으면 결손으로만 보고한다
- 후속 `유니티엔진_컴파일_실행` 통과 → `게임개발_모듈_폴더_익스포트`
- 완료 기준: 컴파일 에러 0건, 익스포트 success, `module.md` 갱신(`Object_Unit` 구 표기 → `Object_UnitBase`). 사용자가 자율 진행을 지시했으므로 설계 판단은 스스로 확정한다. `confirmed`·`reuse` 값은 변경하지 않으며 DataMCP 무응답 시 `Fallback` 순서를 따른다
