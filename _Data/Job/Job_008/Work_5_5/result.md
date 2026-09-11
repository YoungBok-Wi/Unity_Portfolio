# [게임개발_구성_리소스_파일_GPT_애니메이션시트_제작] "Banana 임포트 기준 프레임 재반입" 업무 레포트

## 요약

- Banana Move를 투명 `256x256`, PNG 알파 바운딩 박스 `(56,61,199,184)`로 정렬해 pool `5.png`로 선택·익스포트했다.
- Unity `GetPixels`의 `alpha > 0.01` 실측 높이는 `123px`이고 PPU `128`·피벗 `(128,71.68)`·기존 GUID 참조 3곳이 유지됐다.
- 재임포트 후 콘솔 오류는 `0건`이며 `confirmed:false`·`reuse:add`는 변경하지 않았다.

## 완료업무

### 프레임 검수와 정렬
**산출물**
`_Temp/Job_008_Work_5_4_Banana_Move_candidate.png`
**작업내용**
- `Work_5_4` 정규화 후보의 크기·알파를 유지하고 접지 행을 `184`로 정렬했다.
- 최종 PNG 실측은 `256x256`, 알파 바운딩 박스 `(56,61,199,184)`, 높이 `123px`다.

### pool 반입과 선택
**산출물**
`_Data/Resource/File/AnimationSheet_Casual_Enemy/Banana_Move/frame_01/5.png`
**작업내용**
- 기존 `1.png`~`4.png`를 보존하고 `outputs.frame_01.select`를 `5.png`로 갱신했다.
- 선택 원본과 Asset MD5는 `D1891522C8EE556A874363FF0290463C`로 일치한다.

### Asset 익스포트와 Unity 검증
**산출물**
`Assets/__Game/_Core/SpriteAnim/AnimationSheet_Casual_Enemy_Banana_Move_01.png`
**작업내용**
- Unity 로드 결과는 `loaded:true`, `alpha > 0.01` 높이 `123px`, PPU `128`, 피벗 `(128,71.68)`이다.
- `.meta` GUID `a89913cb0db86dd43b05835983574018`과 Banana 프리팹의 아이콘·기본 스프라이트·Move 프레임 참조 3곳이 유지됐다.
- 재임포트 후 콘솔 오류는 `0건`이다.
