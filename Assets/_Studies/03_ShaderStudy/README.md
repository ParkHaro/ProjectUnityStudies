# Study 03 — Shader Study

URP 환경에서 Shader Graph와 HLSL Custom Function을 혼합해 네 가지 
셰이더를 구현하며 렌더링·셰이딩 기초를 학습한 Study.

## 구조

```
씬 (ShaderShowcase.unity)
├ Ground
├ DissolveCube   Dissolve 머티리얼 + DissolveDemoDriver
├ OutlineSphere  OutlineBodyMaterial + OutlineMaterial (Two-Pass)
├ ToonCapsule    Toon 머티리얼
├ HologramCube   Hologram 머티리얼
└ TMP 라벨 4종
```

| 폴더 | 구성 |
|---|---|
| `Shaders/Dissolve/` | `DissolveHelper.hlsl`, `Dissolve.shadergraph` |
| `Shaders/Outline/` | `OutlineHelper.hlsl`, `Outline.shadergraph` |
| `Shaders/Toon/` | `ToonShadingHelper.hlsl`, `Toon.shadergraph` |
| `Shaders/Hologram/` | `HologramHelper.hlsl`, `Hologram.shadergraph` |
| `Materials/` | 각 셰이더 머티리얼 + Outline 본체용 Lit 머티리얼 |
| `Textures/` | `DissolveNoise.png` |
| `Runtime/Scripts/` | `DissolveDemoDriver.cs` |

## 네 가지 셰이더

**Dissolve** — URP Lit + Alpha Clipping. 노이즈 텍스처와 threshold를 
비교해 픽셀을 discard하고, threshold 근처는 발광. HLSL에서 smoothstep으로 
경계 가중치 계산.

**Outline** — URP Unlit + Render Face: Back. Vertex 단계에서 법선 방향으로 
부풀린 메시의 뒷면만 렌더. Mesh Renderer Materials 배열에 [본체, 외곽선] 
순서로 할당해 Two-Pass 구성.

**Toon Shading** — URP Lit. `dot(Normal, LightDir)`를 
`floor(x*Steps)/(Steps-1)` 공식으로 단계화. Base Color와 Shadow Color를 
단계화된 밝기로 lerp해 그림자 영역에 색감 추가.

**Hologram** — URP Unlit + Transparent + Render Face: Both. 
Fresnel(`(1-dot(view,normal))^power`) + Scanline(`sin(y*density + time*speed)`) 
+ Flicker(저주파 3개 sin 합성)를 `saturate(sum)`으로 합성. FlickerStrength로 
깜빡임 강도 조절.

## 구현 메모

**Shader Graph + HLSL Custom Function 혼합**  
네 셰이더 모두 핵심 계산 로직은 HLSL 헬퍼 파일(`*_float` 함수)로 분리하고, 
입출력·색상 합성은 Shader Graph에서 처리. 모든 HLSL 파일에 `#ifndef` 
include guard 적용.

**시간 값 전달**  
HLSL 함수에서 `_Time.y`를 직접 참조하지 않고 Shader Graph의 Time 노드를 
통해 파라미터로 전달.

## 재생 방법

- Unity 6000.3.10f1 + URP
- `Scenes/ShaderShowcase.unity` 열고 Play
- 각 Material의 Property를 실시간 조절해 효과 확인 가능

## 범위 밖

- **Screen-space Outline** — Depth/Normal buffer를 Sobel 필터로 처리하는 
  방식. Scriptable Renderer Feature 작성이 필요해 범위 밖.
- **Custom Lighting 모델** — `GetMainLight()` 등 URP 라이팅 함수를 HLSL로 
  직접 호출하는 방식은 범위 밖.
- **포스트 프로세싱** — Bloom 등 카메라 레벨 후처리는 범위 밖.
