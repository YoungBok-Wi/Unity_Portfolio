# [오케스트레이터_워커_실행] "Job_003 Work_2 스프라이트 피벗 정본 재익스포트" 업무 레포트

## 요약
- Work 판정: 합격 — 적 프레임 33건 `.meta` 피벗을 정본 (0.5, 0.28)로 보정(`eval` `total=33 changed=33`), 플레이어 58건·보스 56건은 정본 (0.5, 0)과 이미 일치해 무변경
- 완료 기준 실측: `Resources/SpriteAnim` 147건 `Resources.Load<Sprite>` `ok=147 null=0`, 로드 피벗 Player (0.5,0) x58·Enemy (0.5,0.28) x33·Boss (0.5,0) x56·PPU 128 전건, `clear_console` 후 `get_console_logs` `total=0`
- 익스포트 대조: 원본(`_Data/Resource/File/AnimationSheet_Casual_*/{ID}/{슬롯}/{select}`)↔사본 md5 `total=147 missing=0 diff=0 extra=0`, `.meta` 147 = png 147(고아 0)
- 피벗은 타입 정의 항목이 아님 — `resource_type get` 3건 출력 슬롯 필드는 `suffix`·`ext`·`resources`·`leaf`·`idPrefix`·`processAutomationId`·`processLiteralValues`뿐. 타입·원본·산출물 무변경, `confirmed`·`reuse` 무변경
- 다음 행동: 후속 Work에서 프리팹 발 콜라이더 offset (0, 0.45)·`Telegraph.prefab`·`LocalRoomManager` 스폰 클램프를 Work_1 정본대로 적용한다

## 완료업무

### 스프라이트 피벗 정본 재익스포트
**산출물**
`C:\_Projects\Unity_Portfolio\Assets\__Game\_Core\Resources\SpriteAnim`
`C:\_Projects\Unity_Portfolio\_Temp\Work_2\compare.py`
**작업내용**
- 선행 `게임개발_구성_리소스_질문` → `게임개발_구성_리소스_타입_질문`(`resource_type get` `AnimationSheet/AnimationSheet_Casual_{Player,Enemy,Boss}`): 출력 슬롯 6개 전부 `resources:true`·`leaf:"SpriteAnim"`·`processAutomationId:""`, 피벗·임포트 설정 필드 없음(description에만 규격 서술 — Player·Boss "(0.5, 0)", Enemy "(0.5, 0.28)") → 지시서 분기 "`.meta` 전건 보정" 적용
- 보정 전 실측(`Assets/__Game/_Core/Resources/SpriteAnim/*.png.meta`): 147건 전부 `alignment: 7`·`spritePivot: {x: 0.5, y: 0.5}`(BottomCenter, 무시값)·`spritePixelsToUnits: 128`·`textureType: 8`
- `게임개발_구성_리소스_파일_익스포트` 절차 1 `resource_file export`: MCP 타임아웃 → 스킬 규칙대로 전수 대조 우선(`_Temp/Work_2/compare.py`, `type.json` `idPrefix`+ID+`suffix`+`ext` 조합 147건 md5 원본 일치·누락 0·범위 밖 0) → 통과라 재시도 없이 마침. 절차 2 `유니티엔진_재임포트_실행`: `unity cmd eval 'UnityEditor.AssetDatabase.Refresh(); return true;'` `success`(1.0s), `.meta` 147건 실재
- 보정(`unity cmd clear_console` → `eval`, Job_002 Work_3 방식): `AssetDatabase.FindAssets("t:Texture2D", SpriteAnim)` 중 `AnimationSheet_Casual_Enemy_*` 33건 `TextureImporterSettings.spriteAlignment=Custom(9)`·`spritePivot=(0.5,0.28)` → `SaveAndReimport` `total=33 changed=33`(21.2s). 재실측 `.meta` Enemy 33건 `alignment: 9`·`spritePivot: {x: 0.5, y: 0.28}`, Player 58·Boss 56건 `alignment: 7` 유지, PPU 128 전건
- 완료 기준 실측: `eval` `Resources.Load<Sprite>("SpriteAnim/"+파일명)` `files=147 ok=147 null=0`, `sprite.pivot/rect` Boss (0.5,0) x56·Enemy (0.5,0.28) x33·Player (0.5,0) x58, `pixelsPerUnit` 128. `get_console_logs` `{"total":0,"returned":0}`
- 익스포트 스킬 완료조건: `resource_file get`(Enemy `Apple_Move`) `select:"1.png"`·`location:"project"`(응답에 `assetPath` 필드 없음), `path` `frame_01` = `Assets\__Game\_Core\Resources\SpriteAnim\AnimationSheet_Casual_Enemy_Apple_Move_01.png` 실재. `inAsset.json` `AnimationSheet_Casual_*` 30건(Player 11·Enemy 9·Boss 10 = 파일 ID 전건). 옛 경로 사본·고아 `.meta` 0건(`.meta` 147 = png 147)

## 비고
- `AutoTextureSettingOnImport.cs`(`Assets/_Editor/Editor/Script/`) `OnPreprocessTexture`는 `Image`·`Images`·`Icon`·`Textures` 폴더만 처리하고 `SpriteAnim` 규칙·피벗 설정이 없다 — 이번 보정은 `.meta` 값이라 재익스포트(같은 파일 덮어쓰기)에는 유지되지만, 적 프레임을 새 파일명으로 추가하면 `.meta`가 BottomCenter(기본)로 생성되어 같은 `eval` 보정이 다시 필요하다(코드 수정은 지침 범위 밖)
- 열린 씬 `Scene_Lobby` `isDirty:false` 유지(작업 전후 씬 무편집). 원본·타입·프리팹·코드 무변경(임포트 설정만)
- DataMCP 무응답: `export` 호출 직후 MCP `get` 3회·`tree` 2회 타임아웃 → `Fallback` 규칙대로 `curl`(`_Temp/Work_2/*.json` 요청 본문) 전환, 1회차 exit 28 → 2회차(`-m 280`) `get`·`tree`·`path`·`job_work path` 응답. `ping`은 `projectID` 파라미터를 거부(`'ping' 액션이 인식하지 못하는 파라미터: projectID`) — 응답이 온 시점이라 재호출 불필요
- `export` 성공 응답은 미수신(타임아웃) — 반영 여부는 md5 전수 대조로 판정(`success` 미확인)
