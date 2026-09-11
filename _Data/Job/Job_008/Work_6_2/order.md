# 업무지시서

## 1. Delegate Assets 사용 복원

**대상 스킬**: 게임개발_모듈_폴더_구성

**"moduleId"**: `Delegate`

**"moduleNamespace"**: `Library`

**"inAsset"**: `true`

**업무**

- 사용자의 명시적 지시에 따라 `Delegate` 모듈을 `inAsset=true`로 설정한다.
- `confirmed`·`reuse`는 변경하지 않고 등록·백업을 보존한다.
- `module.md`의 의존 관계를 확인하고 결손 의존성이 있으면 설정 전에 보고한다.
- 완료 기준은 `module_manage get`의 `inAsset=true`다.

## 2. Delegate 익스포트와 재임포트

**대상 스킬**: 게임개발_모듈_폴더_익스포트

**"moduleId"**: `Delegate`

**"moduleNamespace"**: `Library`

**업무**

- 원본·백업 구조를 확인하고 `Assets`에 코드·`[DelegateManager].prefab`을 복원한다.
- 완료 기준은 익스포트 성공, 프리팹·스크립트와 `.meta` 존재, 등록 경로 보존이다.

## 3. 컴파일과 모듈 검증

**대상 스킬**: 유니티엔진_컴파일_실행

**"scope"**: `Delegate` 모듈 복원 영향

**업무**

- Unity 재컴파일 완료를 폴링하고 컴파일 오류를 확인한다.
- `module_manage verify`와 Unity 로드로 `[DelegateManager].prefab`의 매니저 컴포넌트를 확인한다.
- `runSetup=false`로 두고 씬 셋업은 후속 `Work_6_3`에서 수행한다.
- 완료 기준은 컴파일 오류 `0건`, 모듈 verify 성공, 프리팹 로드·Missing 스크립트 `0건`이다.
