# 업무지시서

## 1. Delegate Odin 전용 코드 제거

**대상 스킬**: 게임개발_모듈_폴더_코드_작성

**"moduleId"**: `Delegate`

**"moduleNamespace"**: `Library`

**업무**

- `module_manage path`로 확인한 원본 `DelegateManager.cs`에서 삭제된 Odin Inspector에만 필요한 `using`과 속성을 제거한다.
- Unity 기본 `[SerializeField]`와 필드·동작은 보존하고 다른 라이브러리 코드는 수정하지 않는다.
- 완료 기준은 원본 코드에 `Sirenix`·`TabGroup`·`SuffixLabel` 참조가 없고 기존 직렬화 필드가 유지되는 상태다.

## 2. Delegate 재익스포트와 재임포트

**대상 스킬**: 게임개발_모듈_폴더_익스포트

**"moduleId"**: `Delegate`

**"moduleNamespace"**: `Library`

**업무**

- 수정한 원본을 엔진 사본에 반영하고 Unity 에셋을 재임포트한다.
- `confirmed`·`reuse`·`inAsset`은 변경하지 않는다.
- 완료 기준은 익스포트 성공과 원본·엔진 사본의 Odin 전용 선언 제거 일치다.

## 3. 컴파일과 매니저 프리팹 검증

**대상 스킬**: 유니티엔진_컴파일_실행

**"scope"**: `Delegate` Odin 의존 제거 영향

**업무**

- Unity 재컴파일 완료를 확인하고 컴파일 오류를 수집한다.
- `module_manage verify`와 Unity 로드로 `[DelegateManager].prefab`의 매니저 컴포넌트와 Missing 스크립트를 확인한다.
- `runSetup=false`로 두고 씬 셋업은 후속 Work에서 수행한다.
- 완료 기준은 컴파일 오류 `0건`, 모듈 verify 성공, 프리팹 로드·Missing 스크립트 `0건`이다.
