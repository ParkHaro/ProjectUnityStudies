# ProjectUnityStudies

Unity 기반 학습 Study들을 모아 둔 저장소.
각 Study는 독립적인 주제를 깊게 다루며 README로 설계 의도를 정리한다.

## 환경

- Unity 6000.3.10f1 (Unity 6 LTS)
- Universal Render Pipeline (URP) 17.3.0
- 주요 패키지: Timeline, Cinemachine, Input System, Shader Graph, TextMeshPro

## Studies

### [Study 01 — Timeline Custom Track](Assets/_Studies/01_TimelineCustomTrack/)
Unity Timeline의 `TrackAsset` / `PlayableAsset` / `PlayableBehaviour`를 
직접 작성해 대사 시스템을 시간축 위에서 편집할 수 있도록 구현. 
Track 기본값 + Clip override, requestId 기반 경합 방어.

### [Study 02 — QuarterView Interaction](Assets/_Studies/02_QuarterViewInteraction/)
쿼터뷰 이동과 Cinemachine 기반 카메라 시점 전환. 특정 영역 진입 시 
priority 토글만으로 부드러운 카메라 전환을 Brain의 Default Blend에 
위임. Input System으로 키보드·게임패드 동시 대응.

### [Study 03 — Shader Study](Assets/_Studies/03_ShaderStudy/)
URP 환경에서 Shader Graph + HLSL Custom Function 혼합 방식으로 
Dissolve / Outline / Toon Shading / Hologram 네 셰이더 구현.

## 실행

각 Study 폴더의 `Scenes/` 하위에 데모 씬이 있다. Unity에서 해당 씬을 
열고 Play하면 동작 확인 가능.

- Study 01: `Assets/_Studies/01_TimelineCustomTrack/Scenes/DialogueDemo.unity`
- Study 02: `Assets/_Studies/02_QuarterViewInteraction/Scenes/QuarterViewDemo.unity`
- Study 03: `Assets/_Studies/03_ShaderStudy/Scenes/ShaderShowcase.unity`

## 라이선스

- 코드: [MIT License](LICENSE)
- 폰트: Noto Sans KR (SIL Open Font License 1.1)
