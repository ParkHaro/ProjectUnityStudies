using UnityEngine;

namespace ParkHaro.Studies.QuarterViewInteraction
{
    /// <summary>
    /// 상호작용 가능한 모든 대상의 계약.
    /// Interactor가 이 인터페이스를 기준으로 대상을 다룬다.
    ///
    /// 전략 패턴 관점: 구현체마다 다른 동작(색 토글, 문 열기, 아이템 획득 등)을
    /// 같은 진입점(Interact)을 통해 처리할 수 있게 한다.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 상호작용을 실행한다.
        /// </summary>
        /// <param name="interactor">상호작용 주체 GameObject (플레이어 등).
        /// 구현체가 필요시 활용 (방향 바라보기, 상태 체크 등).</param>
        void Interact(GameObject interactor);

        /// <summary>
        /// UI 힌트 등에 표시할 상호작용 설명 라벨.
        /// 현재 Study에서는 미사용이나 확장 대비.
        /// </summary>
        string InteractionLabel { get; }
    }
}
