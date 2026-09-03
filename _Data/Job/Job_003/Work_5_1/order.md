# 업무지시서

## 1. SpriteAnim 프레임 Read/Write 허용 재임포트

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"taskContent"**: `Resources/SpriteAnim` 전 프레임 임포트 설정 `isReadable=true` 적용·재임포트 (오브젝트 구성 절차의 스프라이트 실측 전제)

**업무**

- 근거: `_Data/Job/Job_003/Work_5/result.md` `## 예외상황` — `게임개발_프리셋_파일_오브젝트_구성` 절차 3의 `Texture2D.GetPixels`가 비-Readable 텍스처에서 실패
- Job_003 Work_2와 같은 방식(타입 정의에 임포트 설정 항목이 없으므로 `.meta` 일괄 보정 + 재임포트)으로 `AnimationSheet_Casual_{Player,Enemy,Boss}` 프레임 전건의 `isReadable`을 1로 설정한다. 피벗·PPU 등 기존 설정은 보존한다
- 완료 기준: 전 프레임 `Texture2D.isReadable=true` 실측(샘플 `GetPixels` 성공), 로드 null 0, 콘솔 에러 0. 산출물 재생성은 하지 않는다. `confirmed`·`reuse` 값은 변경하지 않으며 DataMCP 무응답 시 `Fallback` 순서를 따른다
