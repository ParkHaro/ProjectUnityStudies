#ifndef HOLOGRAM_HELPER_INCLUDED
#define HOLOGRAM_HELPER_INCLUDED

// Hologram 효과의 픽셀별 강도(Intensity)를 계산한다.
// 세 가지 효과를 합성해 최종 0~1 강도값을 반환.
//
// 1. Fresnel — 가장자리 빛남
//    시선 방향과 법선의 dot 결과를 1에서 뺀 뒤 pow로 지수 적용.
//
// 2. Scanline — Y축 기반 흐르는 줄무늬
//    sin 함수로 주기 패턴 생성, 시간을 더해 스캔라인이 아래로 흐르게.
//
// 3. Flicker — 시간 기반 불규칙 깜빡임
//    서로 공약수가 적은 3개 저주파 sin을 합성해 부드러운 요동 생성.
//    FlickerStrength로 영향력을 조절(0=깜빡임 없음, 1=최대).
//    주파수 설계: 2.3Hz / 4.7Hz / 7.1Hz — 초당 3~7회 미세 변동.
//    (초기 설계의 37/83/151Hz는 시각 피로 유발 수준이라 대폭 낮춤.)
//
// 입력:
//   Normal          — World Space 법선
//   ViewDir         — World Space 시선 방향 (표면 → 카메라)
//   WorldPos        — World Space 픽셀 위치
//   Time            — Shader Graph Time 노드의 Time 출력
//   FresnelPower    — Fresnel 지수 (권장 2~5)
//   ScanlineDensity — 스캔라인 촘촘함 (권장 20~80)
//   ScanlineSpeed   — 스캔라인 흐름 속도 (권장 1~5)
//   FlickerStrength — 깜빡임 강도 (0~1, 권장 0.1~0.3)
//
// 출력:
//   Intensity       — 홀로그램 밝기 강도 (0~1)
void Hologram_float(
    float3 Normal,
    float3 ViewDir,
    float3 WorldPos,
    float Time,
    float FresnelPower,
    float ScanlineDensity,
    float ScanlineSpeed,
    float FlickerStrength,
    out float Intensity)
{
    // 1. Fresnel
    float fresnel = 1.0 - dot(ViewDir, Normal);
    fresnel = pow(fresnel, FresnelPower);

    // 2. Scanline
    float scanline = sin(WorldPos.y * ScanlineDensity + Time * ScanlineSpeed);
    scanline = scanline * 0.5 + 0.5;

    // 3. Flicker — 저주파 3개 합성 후 0~1 remap, FlickerStrength로 감쇠
    float flicker = sin(Time * 2.3) + sin(Time * 4.7) + sin(Time * 7.1);
    flicker = flicker / 3.0 * 0.5 + 0.5;
    flicker = flicker * FlickerStrength;

    // 합성
    Intensity = saturate(fresnel + scanline + flicker);
}

#endif // HOLOGRAM_HELPER_INCLUDED
