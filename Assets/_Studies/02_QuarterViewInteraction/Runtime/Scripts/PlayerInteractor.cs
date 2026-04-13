using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ParkHaro.Studies.QuarterViewInteraction
{
    /// <summary>
    /// 상호작용 주체. 트리거 영역에 들어온 IInteractable 후보들을 관리하고,
    /// Interact 입력이 들어오면 가장 가까운 대상에게 Interact(this.gameObject)를 호출한다.
    ///
    /// 전제:
    /// - 이 컴포넌트가 붙은 GameObject는 Collider (IsTrigger=true) 를 가져야 함.
    /// - Rigidbody(IsKinematic=true) 를 같이 붙여야 OnTriggerEnter/Exit이 정상 동작.
    ///   (Unity의 Trigger는 두 콜라이더 중 최소 하나가 Rigidbody를 가져야 한다.)
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PlayerInteractor : MonoBehaviour
    {
        private PlayerControls _controls;
        private readonly List<IInteractable> _candidates = new();

        private void Awake()
        {
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Interact.performed += OnInteract;
        }

        private void OnDisable()
        {
            _controls.Player.Interact.performed -= OnInteract;
            _controls.Player.Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 다른 Trigger와 자기 자신의 이벤트는 무시
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                if (!_candidates.Contains(interactable))
                {
                    _candidates.Add(interactable);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                _candidates.Remove(interactable);
            }
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (_candidates.Count == 0) return;

            IInteractable closest = FindClosest();
            closest?.Interact(gameObject);
        }

        /// <summary>
        /// 후보 중 현재 위치로부터 가장 가까운 대상을 반환.
        /// null 대상(파괴됨 등)은 자동 제거.
        /// </summary>
        private IInteractable FindClosest()
        {
            IInteractable closest = null;
            float closestSqrDist = float.MaxValue;
            Vector3 myPos = transform.position;

            for (int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                if (candidate is not MonoBehaviour mb || mb == null)
                {
                    _candidates.RemoveAt(i);
                    continue;
                }

                float sqrDist = (mb.transform.position - myPos).sqrMagnitude;
                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    closest = candidate;
                }
            }

            return closest;
        }
    }
}
