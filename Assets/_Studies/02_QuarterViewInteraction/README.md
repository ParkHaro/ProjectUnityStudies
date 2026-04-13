# Study 02 — QuarterView Interaction

쿼터뷰 이동과 Cinemachine 기반 카메라 시점 전환을 학습하는 씬.
Waltz and Jam 데모에서 관찰한 "다리를 건널 때 3인칭 뒤 시점으로 전환되고
통과 후 쿼터뷰로 복귀"하는 연출을 재현.

## 목적

특정 영역 진입 시 카메라 시점이 부드럽게 전환되는 게임 연출을,
커스텀 보간 코드 없이 Cinemachine의 priority + Brain 블렌드로 구현할 수 
있는지 체득하기 위한 Study.

## 구조

```
씬
├ Player        CharacterController 기반 이동 + 점프
│  └ InteractionArea  Sphere Trigger로 IInteractable 후보 감지
├ MainCamera    CinemachineBrain (Default Blend: 2s EaseInOut)
├ QuarterViewVCam  평상시 쿼터뷰 (priority 10)
├ BehindVCam       다리 위 뒤 시점 (priority 0 → 20 토글)
└ Level
   ├ Hurdles            점프해서 넘는 장애물 3개
   ├ Bridge             길이 13m 다리
   ├ FarSide            건너편 랜드마크 + ColorSwitch
   └ BridgeCameraTrigger  다리 전체 덮는 Trigger + CameraTrigger
```

| 파일 | 역할 |
|---|---|
| `PlayerController.cs` | CharacterController 이동 + 중력 + 점프 |
| `PlayerInteractor.cs` | Trigger 영역 내 IInteractable 후보 관리, 최근접 대상 상호작용 |
| `IInteractable.cs` | 상호작용 계약 |
| `ColorSwitch.cs` | IInteractable 구현체. 색상 순환 |
| `CameraTrigger.cs` | 영역 진입/이탈 시 CinemachineCamera priority 토글 |
| `PlayerControls.inputactions` | Input Actions asset (Move/Jump/Interact) |

## 주요 설계 결정

**Cinemachine priority 토글로 시점 전환, 블렌드는 Brain에 위임**  
`CameraTrigger`는 영역 진입/이탈 시 대상 카메라의 `Priority` 값만 바꾼다.
카메라 간 보간은 `CinemachineBrain`의 Default Blend 설정(2초 EaseInOut)이 
자동 처리. 커스텀 lerp 코드 없음.

**월드 기준 이동**  
카메라 시점이 바뀌어도 조작 방향이 일관되도록 이동을 월드 좌표 기준으로 구현.
카메라 기준 이동은 다리 위 시점 전환 도중 조작 방향이 꼬일 수 있어 의도적으로 배제.

**IInteractable + PlayerInteractor 분리**  
상호작용 대상은 인터페이스만 구현하면 되고, 상호작용 주체(Player)는 
대상의 구체 타입을 모름. 새로운 상호작용 종류(문, 스위치, NPC 대화 등)는 
`IInteractable` 구현만 추가하면 됨.

**PlayerControls 직접 생성**  
`PlayerInput` 컴포넌트 대신 스크립트에서 `new PlayerControls()`로 직접 
생성하고 Action 구독을 명시적으로 처리. 구독·해제 라이프사이클이 코드에서 
가시화되어 학습·설명이 용이.

## 재생 방법

- Unity 6000.3.10f1 + URP
- `Scenes/QuarterViewDemo.unity` 열고 Play
- 이동: WASD / 왼쪽 스틱 / D-Pad
- 점프: Space / Button South (A/Cross)
- 상호작용: E / Button North (Y/Triangle)

## 범위 밖

의도적으로 제외한 것:

- **PlayerInput 컴포넌트 통합** — 학습·설명 용이성을 위해 직접 생성 방식 채택. 
  `PlayerController`와 `PlayerInteractor`가 각자 `PlayerControls` 인스턴스를 
  가지는 중복은 수용.
- **카메라 기준 이동** — 시점 전환 중 조작 일관성을 위해 월드 기준 고정.
- **UI 힌트 / 사운드 / 캐릭터 회전·애니메이션** — Cinemachine과 상호작용 
  패턴 학습이 주목적이라 표현 요소는 최소.
