# Study 01 — Timeline Custom Track

Unity Timeline의 `TrackAsset` / `PlayableAsset` / `PlayableBehaviour`를 
직접 작성해보며 Timeline 확장 API를 체득한 학습 씬. 주제는 대사 시스템.

## 목적

이전 프로젝트(Oz:ReWrite)에서는 공용 Track을 재활용하고 런타임 바인딩만 
동적으로 연결하는 Processor 패턴으로 상위 오케스트레이션을 다뤘다. 반대 
방향 — Timeline 하위 확장 API 직접 작성 — 을 학습하기 위한 Study.

## 구조

```
DialogueTrack  ─ defaultTypingSpeed, Clip 전처리
  └ DialogueClip  ─ speaker, text, typingSpeedOverride
      └ DialogueBehaviour  ─ OnBehaviourPlay / ProcessFrame / OnBehaviourPause

[TrackBindingType: DialogueUI (abstract)]
  ├ WindowDialogueUI  (하단 대화창)
  └ BubbleDialogueUI  (화자 머리 위 말풍선)
```

| 파일 | 역할 |
|---|---|
| `DialogueTrack.cs` | TrackAsset. Track 속성 + Clip 순회 전처리 |
| `DialogueClip.cs` | PlayableAsset. Inspector 필드, Behaviour 생성 |
| `DialogueBehaviour.cs` | PlayableBehaviour. 라이프사이클 훅 |
| `IDialogueUI.cs` / `DialogueUI.cs` | UI 계약 + 바인딩용 추상 MonoBehaviour |
| `WindowDialogueUI.cs` / `BubbleDialogueUI.cs` | UI 구현체 2종 |

## 주요 설계 결정

**Track 기본값 + Clip override로 typingSpeed 두 레벨 관리**  
Clip의 `typingSpeedOverride ≤ 0`이면 Track의 `defaultTypingSpeed`를 사용. 
주입은 `DialogueTrack.CreateTrackMixer`에서 Clip을 순회하며 처리.

**인터페이스 + 추상 MonoBehaviour 조합**  
`[TrackBindingType]`은 Component 서브클래스를 요구하므로 인터페이스 단독 
불가. 인터페이스로 계약을 분리하고 추상 MonoBehaviour로 Timeline 요구사항을 
충족해 구현체를 교체 가능하게 함.

**requestId 기반 경합 방어**  
Clip A→B 연속 배치 시 A의 `Hide`와 B의 `ShowLine`이 같은 프레임에 발생할 
수 있음. UI가 `ShowLine`마다 id를 증가·반환하고, `Hide(id)`는 현재 id와 
일치할 때만 실제 숨김.

## 재생 방법

- Unity 6000.3.10f1 + URP
- `Scenes/DialogueDemo.unity` 열고 Play
- Timeline 편집: `DialogueDirector` 선택 → Timeline 창에서 Clip 수정

## 범위 밖

의도적으로 제외한 것:

- **Clip 블렌딩** — Clip 단위 Behaviour로만 구현해 TrackMixerBehaviour 미사용.
- **인라인 태그** — Oz에서 쓰던 `<delay>`, `<wait>` 태그 파서는 학습 범위 외.
- **스킵 / 보이스 / 감정 / 초상화** — 최소 필드(speaker / text / typingSpeed)만 유지.

## Oz:ReWrite 경험과의 대비

|  | Oz (Processor 패턴) | 이 Study (커스텀 Track) |
|---|---|---|
| 레이어 | 상위 오케스트레이션 | 하위 확장 API |
| Track | 공용 Track 재활용 | TrackAsset 직접 상속 |
| 데이터 | 외부 테이블 | PlayableAsset 필드 |
| 강점 | 기존 Track 재활용 | 에디터 시간축 편집·프리뷰 |
