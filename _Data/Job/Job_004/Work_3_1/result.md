# [오케스트레이터_워커_실행] "[AI구성] Work_3 DataMCP 약 20분 무응답 원인 조회" 업무 레포트

## 요약
- Work 판정: 합격 — 원인 특정. `resource_file export`가 Node 이벤트 루프를 약 16분간 동기 점유해 서버 전 HTTP 요청이 무응답이 됐고, 루프 종료로 자연 복구됨. 서버 프로세스(PID 50428, `next dev -p 9400`)는 9/4 23:40:57 기동 후 재시작 없이 유지(크래시 아님)
- 블로킹 코드 경로 (`C:\_Projects\_WebForGameData\src\mcp\tools\resource_type\resource_type_shared.ts`): `refreshAllAssets` 1397~1409행이 전체 엔트리 640건을 동기 순회 → `copySelectedToAssets` 1137행이 엔트리마다 `getMergedResourceTypes` 호출(1139행 `inAsset` 검사보다 앞) → `loadResourceType` 917~933행이 타입 78건마다 `resourceTypeCategory`(585행) → `loadEntityNodes`(`src/lib/entityNodes.ts:141`) 재실행
- 실측: `getMergedResourceTypes` 1회 1,514ms(5회 7,353ms), `loadEntityNodes` 1회 약 17ms, `loadResourceFile` 1,717ms, `entries 640 / inAsset 135` → 640 × 1.5s ≈ 960s. trace 기록 `/api/editor/resources/refresh`(같은 함수) 869s·563s·528s, `/api/mcp/call` 718s·700s·678s와 일치
- 배제: Unity 임포트(`Editor.log` PNG 4건 각 0.015s·Refresh 0.18s), 파일 크기(4건 289KB), `next.config.ts` 감시 범위(설정 없음), `verifyWorker`(비동기 4s tick), 서버 로그 예외(`next-development.log` 기동 8줄뿐)
- 수정 없음. 수정 제안은 `## 비고`

## 완료업무

### DataMCP 무응답 원인 조회
**산출물**
`C:\_Projects\Unity_Portfolio\_Data\Job\Job_004\Work_3_1\result.md`
**작업내용**
- 수행 스킬: `AI구성_MCP_DataMCP_질문`(하위 스킬 없음 — `child` 응답 `{}`, `error.md` 빈 파일). 절차 1 코드 파악·절차 2 실측·절차 3 답변(대화 출력) 수행
- 호출 경로: `src/mcp/tools/resource_file/resource_file_export.ts:11` `refreshAllAssets(projectPath)` 동기 호출 → `src/app/api/mcp/adapter.ts:143` `transport.handleRequest`가 핸들러 완료까지 대기. `src/app/api/mcp/route.ts:5` `maxDuration = 300`은 Vercel 힌트라 dev 서버에서 중단 효과 없음. `/api/mcp/call`(`src/app/api/mcp/call/route.ts`)·MCP 엔드포인트 모두 같은 프로세스라 함께 무응답
- 비용 구조: `copySelectedToAssets`가 엔트리 640건마다 `getMergedResourceTypes` → `loadResourceType`(타입 78건 × `resourceTypeCategory` → `loadEntityNodes`(노드 폴더 25건 readdir·existsSync·readFile) + `resolveItemDir` existsSync + `readJsonFile`). 엔트리당 약 4,700회 동기 fs 호출 × 640 ≈ 300만 회. `inAsset:false` 505건도 같은 비용(1139행 조기 반환이 1137행 뒤)
- 사건 타임라인(`.next/dev/trace` `handle-request` 스팬, 로컬 시각): 04:44:12~04:44:34 `upload` 4건 5.67·7.24·7.39·7.25s, 04:44:59~04:45:12 `patch select` 3건 4.99·6.37·6.00s(각각 `syncEntryExports` 안에서 `getMergedResourceTypes`·`loadResourceFile` 수 회), `file.json` 04:45:21 저장 후 `export` 호출 → trace 마지막 기록 04:45:18, 이후 스팬 없음. `Work_3` 완료 05:36(`completedAt` 2026-09-04T20:36:31Z)
- 실측 방법: `node --import tsx` 단독 프로세스(`_WebForGameData/_Temp/__w31_time.ts`, 읽기 전용 함수만 호출, 실행 후 삭제)에서 `loadResourceFile`·`getMergedResourceTypes`·`loadEntityNodes`·`getFileDir` 소요 측정. 프로젝트 데이터·서버 코드·설정 무변경
- `curl http=000`: 이벤트 루프 점유 중 TCP 연결은 커널 백로그에 쌓이고 HTTP 응답이 없어 `curl`이 타임아웃으로 `000`을 반환하는 상태와 일치(응답 원문 없음이라 정합성 판단, 직접 재현은 미수행)
- 재현 조건: 대용량 PNG·임포트 트리거와 무관. 엔트리 수(640)와 타입 수(78)에 비례하는 동기 fs 호출량이 원인이라 어떤 `export` 호출이든 같은 시간이 걸림(trace 09-02·09-03·08-22·08-21 반복 기록)

## 비고
- 수정 제안 1 — 위치: `resource_type_shared.ts:1136~1139` `copySelectedToAssets` / 현상: 엔트리마다 `getMergedResourceTypes` 재로드, `inAsset` 조기 반환이 그 뒤 / 제안: `inAsset` 검사를 1137행 앞으로 올리고 `refreshAllAssets`가 `getMergedResourceTypes` 1회 결과를 인자로 넘김(640회 → 1회, 예상 16분 → 수 초)
- 수정 제안 2 — 위치: `resource_type_shared.ts:917~933` `loadResourceType`·`585~597` `resourceTypeCategory` / 현상: 타입마다 `loadEntityNodes` 재실행(호출당 약 85% 비용) / 제안: `loadResourceType` 안에서 `loadEntityNodes` 1회 결과를 `resourceTypeCategory`에 전달
- 수정 제안 3 — 위치: `resource_file_export.ts:8~21` / 현상: 장시간 동기 작업이 요청 핸들러 안에서 실행돼 서버 전체가 멈춤 / 제안: 진행 단위마다 `await setImmediate` 등으로 이벤트 루프를 양보하거나 워커(`automationWorker` 방식)로 분리
- trace 파일이 04:45:18 이후 기록을 멈춘 이유(이후 요청은 정상 처리됨)는 질문 범위 밖이라 미조사
- 같은 증상 이력(`Job_003/Work_Final/result.md` `## 비고` "export 후 장시간 무응답 반복")도 같은 원인으로 설명됨(trace 09-02 869s·09-03 718s)
