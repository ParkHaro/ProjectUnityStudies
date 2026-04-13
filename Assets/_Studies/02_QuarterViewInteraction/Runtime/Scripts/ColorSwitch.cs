using UnityEngine;

namespace ParkHaro.Studies.QuarterViewInteraction
{
    /// <summary>
    /// IInteractable 구현체. 상호작용할 때마다 MeshRenderer의 머티리얼 색상을 순환시킨다.
    /// Study 2에서 인터랙션이 실제로 동작함을 시각적으로 확인하기 위한 대상.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class ColorSwitch : MonoBehaviour, IInteractable
    {
        [Tooltip("순환할 색상 목록. 최소 2개 이상.")]
        [SerializeField] private Color[] colors = { Color.white, Color.red, Color.blue };

        [Tooltip("UI 힌트 등에 표시할 라벨")]
        [SerializeField] private string label = "색 바꾸기";

        private MeshRenderer _renderer;
        private int _index;

        public string InteractionLabel => label;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void Interact(GameObject interactor)
        {
            if (colors == null || colors.Length == 0) return;

            _index = (_index + 1) % colors.Length;

            // 인스턴스 머티리얼에 쓰기 (공유 머티리얼 변경 방지).
            _renderer.material.color = colors[_index];
        }
    }
}
