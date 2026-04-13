using UnityEngine;
using UnityEngine.InputSystem;

namespace ParkHaro.Studies.QuarterViewInteraction
{
    /// <summary>
    /// 쿼터뷰/뒤 시점 공통으로 사용하는 플레이어 이동 컨트롤러.
    ///
    /// 이동은 월드 기준 앞뒤좌우 — 카메라 시점이 바뀌어도 조작 방향이 일관되게 유지된다.
    /// (카메라 기준 이동으로 만들면 다리 위에서 시점 전환 도중 조작이 꼬일 수 있어
    /// 본 Study에서는 의도적으로 월드 기준을 선택.)
    ///
    /// 점프는 접지 상태에서만 실행. 중력은 수동으로 매 프레임 누적.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("이동 속도 (유닛/초)")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Jump")]
        [Tooltip("점프 시 부여되는 초기 수직 속도 (유닛/초)")]
        [SerializeField] private float jumpSpeed = 6f;

        [Tooltip("중력 가속도 (절댓값, 유닛/초²). 실제로는 아래 방향으로 적용됨.")]
        [SerializeField] private float gravity = 20f;

        [Header("Input")]
        [Tooltip("Input Actions asset에서 생성된 PlayerControls. 비워두면 자체 인스턴스 생성.")]
        [SerializeField] private InputActionAsset inputAsset;

        private CharacterController _controller;
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private bool _jumpRequested;
        private float _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Move.performed += OnMove;
            _controls.Player.Move.canceled += OnMove;
            _controls.Player.Jump.performed += OnJump;
        }

        private void OnDisable()
        {
            _controls.Player.Move.performed -= OnMove;
            _controls.Player.Move.canceled -= OnMove;
            _controls.Player.Jump.performed -= OnJump;
            _controls.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            _moveInput = ctx.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            _jumpRequested = true;
        }

        private void Update()
        {
            // 수평 이동 (월드 기준 X/Z)
            Vector3 horizontal = new Vector3(_moveInput.x, 0f, _moveInput.y) * moveSpeed;

            // 중력 및 점프
            if (_controller.isGrounded)
            {
                // 접지 중에는 수직 속도를 살짝 음수로 유지해 경사면에서도 isGrounded가 안정적으로 유지되게 함.
                if (_verticalVelocity < 0f) _verticalVelocity = -2f;

                if (_jumpRequested)
                {
                    _verticalVelocity = jumpSpeed;
                }
            }
            else
            {
                _verticalVelocity -= gravity * Time.deltaTime;
            }

            _jumpRequested = false;

            Vector3 motion = horizontal;
            motion.y = _verticalVelocity;

            _controller.Move(motion * Time.deltaTime);
        }
    }
}
