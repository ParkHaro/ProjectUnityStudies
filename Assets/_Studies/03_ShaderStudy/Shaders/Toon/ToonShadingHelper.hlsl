#ifndef TOON_SHADING_HELPER_INCLUDED
#define TOON_SHADING_HELPER_INCLUDED

// Toon(셀) 셰이딩의 단계화된 밝기를 계산한다.
//
// 원리:
//   일반 Lit 셰이딩은 dot(Normal, LightDir)로 얻은 밝기(-1~1)를
//   그대로 라이팅에 사용해 부드러운 그라디언트를 만든다.
//   Toon은 이 연속값을 floor/step 연산으로 단계화(posterize)해서
//   셀 애니메이션 같은 끊긴 명암을 만든다.
//
// 입력:
//   Normal    — Object Space 또는 World Space 법선 (두 벡터 좌표계가 같기만 하면 됨)
//   LightDir  — 광원 방향 벡터 (Normal과 같은 좌표계)
//   Steps     — 밝기 단계 수 (예: 3 → 3단계 명암)
//
// 출력:
//   Out       — 단계화된 밝기 (0 ~ 1 범위, Steps 개의 이산값)
//
// 설계 메모:
//   - max(0, brightness)로 음수 구간을 잘라냄.
//     이는 "빛을 등진 면은 완전한 그림자"라는 Toon 스타일의 강한 대비와 일치.
//     (0~1로 remap하는 방식도 가능하나 만화적 느낌이 덜해짐.)
//   - clamp로 경계 케이스 처리: brightness=1.0일 때 floor 결과가 Steps가 되어
//     단계 수가 하나 더 나오는 문제를 Steps-1로 클램프해 해결.
//   - 마지막에 (Steps-1)로 나눠 최종 밝기가 [0, 1] 범위를 완전히 사용하게 함.
//     (Steps로 나누면 최대값이 (Steps-1)/Steps에 머물러 흰색이 절대 안 나옴.)
void ToonShading_float(
    float3 Normal,
    float3 LightDir,
    float Steps,
    out float Out)
{
    float brightness = dot(Normal, LightDir);
    brightness = max(0, brightness);

    float stepped = floor(brightness * Steps);
    stepped = clamp(stepped, 0, Steps - 1);

    Out = stepped / (Steps - 1);
}

#endif // TOON_SHADING_HELPER_INCLUDED
