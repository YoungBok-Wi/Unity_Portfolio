# 업무지시서

## 1. 반전 출력 6개 업로드

**대상 스킬**: 게임개발_구성_리소스_파일_업로드

**"resourceTarget"**: `AnimationSheet/AnimationSheet_Casual_Player/Attack2`의 `frame_01`~`frame_06`

**업무**
- `Work_1_1`의 반전 출력 여섯 파일을 대응 슬롯의 새 pool 산출물로 반입한다.
- 기존 원본·선택 산출물·메타를 보존한다.

## 2. 반전 산출물 선택

**대상 스킬**: 게임개발_구성_리소스_파일_구성

**"resourceId"**: `Attack2`

**업무**
- 여섯 슬롯 모두 새로 업로드한 반전 산출물을 선택한다.
- `confirmed`·`reuse`·`inAsset`·프롬프트는 변경하지 않는다.

## 3. Attack2 리소스 익스포트

**대상 스킬**: 게임개발_구성_리소스_파일_익스포트

**"resourceId"**: `Attack2`

**업무**
- `Attack2`를 개별 문자열 ID로 export하고 Unity 재임포트를 완료한다.
- 최종 `AnimationSheet_Casual_Player_Attack2_01.png`~`06.png`의 256x256 크기·알파·오른쪽 방향을 확인한다.
- `유니티엔진_씬_검증`은 사용자 지시에 따라 실행하지 않는다.
