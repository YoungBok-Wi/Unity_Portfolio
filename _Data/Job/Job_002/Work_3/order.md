# 업무지시서

## 1. Gun 전용 애니메이션시트 제작

**대상 스킬**: 게임개발_구성_리소스_파일_애니메이션시트_제작

**"taskContent"**: Gun 캐릭터 전용 `Idle`·`Move` 시트 제작·업로드·익스포트

**업무**

- 근거: `_Data/Job/Job_001/Work_6/result.md` `## 비고` "리소스 제작" 항목, `리소스컨셉`(Work_1 갱신본) Gun 실루엣 규격
- `AnimationSheet_Casual_Player` 타입에 `Idle_Gun`·`Move_Gun` entry 생성·구성·제작(기존 `Attack_Gun` 프레임과 실루엣·규격 256x256·PPU 128·BottomCenter 일치)·업로드·익스포트·재임포트
- 완료 기준: 프레임이 `Resources/SpriteAnim`에 익스포트되고 Sprite 로드 null 0·콘솔 에러 0

## 2. 사운드 제작

**대상 스킬**: 게임개발_구성_리소스_파일_사운드_제작

**"taskContent"**: BGM 2·SFX 5 entry 제작·업로드·익스포트

**업무**

- 대상: `BGM_Casual/Lobby`·`Battle`, `SFX_Casual_Battle/Attack`·`Hit`·`Die`, `SFX_Casual_Progress/LevelUp`·`Unlock` — 타입은 실재하고 entry가 없으므로 `게임개발_구성_리소스_파일_생성` → 구성 → 사운드 제작(하위 파이썬BGM·파이썬SFX 위임) → 업로드 → 익스포트 순으로 진행한다
- 완료 기준: `Assets/__Game/_Core/Resources/{BGM,SFX}`에 파일 실재·임포트 에러 0

## 3. 로비 요리사 일러스트 제작

**대상 스킬**: 게임개발_구성_리소스_파일_이미지_제작

**"taskContent"**: 로비 중앙 요리사 일러스트 제작·익스포트 (컨셉아트 유사도 개선)

**업무**

- `리소스컨셉`(Work_1 갱신본)이 정한 규격으로 `Illust_Casual_*` 계열에 entry 생성·구성·제작·업로드·익스포트한다. 타입이 없으면 `게임개발_구성_리소스_타입_생성`·구성부터 한다
- 완료 기준: 익스포트 파일 실재·임포트 에러 0. 사용자가 자율 진행을 지시했으므로 프롬프트는 스스로 확정하고, Codex 사용량 한도 등 도구 실패는 우회하지 않고 대기·보고한다. `confirmed`·`reuse` 값은 변경하지 않는다
