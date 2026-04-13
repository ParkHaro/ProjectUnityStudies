#ifndef OUTLINE_HELPER_INCLUDED
#define OUTLINE_HELPER_INCLUDED

// Inverted Hull 외곽선의 Vertex 위치 오프셋을 계산한다.
//
// 원리:
//   메시의 각 정점을 그 정점의 법선(Normal) 방향으로 Width만큼 밀어낸다.
//   이렇게 부풀린 메시를 앞면 컬링(Cull Front)으로 렌더하면,
//   원본 메시 뒤쪽으로 삐져나온 부분이 외곽선처럼 보인다.
//
// 입력:
//   Position — Object Space의 원래 정점 위치
//   Normal   — Object Space의 정점 법선 (정규화되어 있다고 가정)
//   Width    — 외곽선 두께 (0.01 ~ 0.1 권장)
//
// 출력:
//   Out      — 부풀린 Object Space 위치
//
// 설계 메모:
//   - Object Space에서 처리하므로 모델의 스케일에 비례해 외곽선이 굵어진다.
//     스케일에 무관한 외곽선을 원하면 World Space 또는 View Space에서
//     카메라 거리 기반으로 보정하는 방식이 있지만 본 Study는 단순화.
//   - Normal이 정규화되지 않았다면 결과가 불균일해질 수 있어
//     호출 측(Shader Graph)에서 Normalize 노드를 거쳐 전달하는 걸 권장.
void OutlineOffset_float(
    float3 Position,
    float3 Normal,
    float Width,
    out float3 Out)
{
    Out = Position + Normal * Width;
}

#endif // OUTLINE_HELPER_INCLUDED
