#ifndef DISSOLVE_HELPER_INCLUDED
#define DISSOLVE_HELPER_INCLUDED

// Dissolve 효과의 경계선 가중치와 discard 여부를 한 번에 계산한다.
//
// 입력:
//   NoiseValue    — 노이즈 텍스처에서 샘플링한 값 (0~1)
//   DissolveAmount— 용해 진행도 (0 = 원본 유지, 1 = 완전히 사라짐)
//   EdgeWidth     — 경계선 두께 (threshold 위로 얼마까지 경계로 간주할지)
//
// 출력:
//   EdgeFactor    — 경계선 가중치 (0~1). threshold 근처에서 1에 가까워짐.
//                   Emission 색상에 곱해 경계가 빛나게 연출.
//   ShouldDiscard — 1이면 픽셀을 버려야 함 (noise < threshold).
//                   Shader Graph의 Alpha Clip과 연결해 실제 discard 수행.
//
// 설계 메모:
//   - smoothstep을 사용해 경계 가중치를 부드럽게 계산 (계단 현상 방지).
//   - discard 판정은 호출 측에서 Alpha Clip Threshold로 처리하도록
//     값만 반환 (HLSL에서 직접 discard 호출하면 Shader Graph의 파이프라인과 충돌).
void DissolveWeight_float(
    float NoiseValue,
    float DissolveAmount,
    float EdgeWidth,
    out float EdgeFactor,
    out float ShouldDiscard)
{
    float threshold = DissolveAmount;

    // threshold 아래에서는 discard 대상
    ShouldDiscard = (NoiseValue < threshold) ? 1.0 : 0.0;

    // threshold ~ threshold+EdgeWidth 구간에서 1 → 0으로 감소
    // threshold 바로 위 영역이 가장 밝게 빛나도록 1.0 - smoothstep 사용
    EdgeFactor = 1.0 - smoothstep(threshold, threshold + EdgeWidth, NoiseValue);
}

#endif // DISSOLVE_HELPER_INCLUDED
