# [게임개발_구성_리소스_파일_업로드] "반전 Attack2 프레임 반영" 업무 레포트
## 요약
`Attack2` 6개 프레임을 오른쪽 방향 이미지로 교체하고 Unity 에셋까지 반영했다. 모든 최종 파일은 256x256 RGBA이며 리소스 원본과 Unity 사본의 해시가 일치한다.

## 완료업무

### 반전 출력물 등록과 선택
**산출물**
`_Data/Resource/File/AnimationSheet_Casual_Player/Attack2/frame_01/3.png`~`frame_06/3.png`
**작업내용**
- `frame_01`~`frame_06`에 반전 PNG를 각각 업로드했다.
- 각 프레임의 선택 출력물을 `3.png`로 변경했다.
- `confirmed: false`, `reuse: add`를 유지했다.

### Attack2 리소스 익스포트
**산출물**
`Assets/__Game/_Core/SpriteAnim/AnimationSheet_Casual_Player_Attack2_01.png`~`06.png`
**작업내용**
- `resource_file export`를 완료했다.
- `AssetDatabase.Refresh()` 성공을 확인했다.
- 리소스 원본과 Unity 에셋의 SHA-256이 여섯 쌍 모두 일치한다.
- 프레임 시각 확인에서 캐릭터가 오른쪽을 향하고 투명 배경이 유지된다.

## 비고
- `유니티엔진_씬_검증`은 사용자 지시에 따라 실행하지 않았다.
