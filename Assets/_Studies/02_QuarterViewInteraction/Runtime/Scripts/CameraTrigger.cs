using UnityEngine;
using Unity.Cinemachine;

namespace ParkHaro.Studies.QuarterViewInteraction
{
    /// <summary>
    /// 지정된 영역에 플레이어가 들어오고 나갈 때 대상 CinemachineCamera의 priority를 바꿔,
    /// 어느 카메라가 활성(Live)일지 토글한다.
    ///
    /// 실제 카메라 전환의 부드러운 블렌딩은 CinemachineBrain의 Default Blend 설정이 담당하고,
    /// 이 스크립트는 단순히 priority 값만 조작한다. 즉 코드는 트리거 판정 + 숫자 하나 변경이 전부이며
    /// 카메라 시점 전환의 복잡한 보간 로직은 Cinemachine 시스템에 위임한다.
    ///
    /// 전제:
    /// - 이 컴포넌트가 붙은 GameObject는 Collider(IsTrigger=true)를 가져야 함.
    /// - 플레이어 GameObject에 playerTag로 지정한 태그가 설정되어 있어야 함.
    /// - targetCamera는 Cinemachine 3.x의 CinemachineCamera 컴포넌트를 참조한다.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class CameraTrigger : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("priority를 조작할 대상 CinemachineCamera")]
        [SerializeField] private CinemachineCamera targetCamera;

        [Header("Priority Values")]
        [Tooltip("플레이어가 영역 내에 있을 때 targetCamera의 priority 값")]
        [SerializeField] private int activePriority = 20;

        [Tooltip("플레이어가 영역 밖으로 나갔을 때 targetCamera의 priority 값")]
        [SerializeField] private int inactivePriority = 0;

        [Header("Player Identification")]
        [Tooltip("플레이어로 간주할 GameObject의 태그")]
        [SerializeField] private string playerTag = "Player";

        /// <summary>
        /// 컴포넌트가 추가될 때 에디터에서 자동으로 Collider를 Trigger로 설정.
        /// </summary>
        private void Reset()
        {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            if (targetCamera != null) targetCamera.Priority = activePriority;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            if (targetCamera != null) targetCamera.Priority = inactivePriority;
        }
    }
}
