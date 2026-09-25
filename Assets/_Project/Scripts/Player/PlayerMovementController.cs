using UnityEngine;

namespace FinalDrop.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Speeds (m/s)")]
        [SerializeField] private float walkSpeed = 3.2f;
        [SerializeField] private float sprintSpeed = 5.6f;
        [SerializeField] private float crouchSpeed = 1.8f;
        [SerializeField] private float proneSpeed = 0.9f;
        [SerializeField] private float jumpHeight = 1.1f;
        [SerializeField] private float gravity = -18f;

        [Header("Turning")]
        [SerializeField] private float rotationSpeed = 720f;

        private CharacterController _controller;
        private Vector3 _velocity;
        private MovementState _state = MovementState.Idle;

        public enum MovementState { Idle, Walking, Sprinting, Crouching, Prone, Jumping }
        public MovementState CurrentState => _state;

        public Vector2 MoveInput { get; set; }
        public bool SprintHeld { get; set; }
        public bool CrouchToggle { get; set; }
        public bool ProneToggle { get; set; }
        public bool JumpPressed { get; set; }

        private bool _isCrouching;
        private bool _isProne;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleStanceToggles();
            HandleMovement();
        }

        private void HandleStanceToggles()
        {
            if (CrouchToggle)
            {
                _isCrouching = !_isCrouching;
                if (_isCrouching) _isProne = false;
                CrouchToggle = false;
            }

            if (ProneToggle)
            {
                _isProne = !_isProne;
                if (_isProne) _isCrouching = false;
                ProneToggle = false;
            }
        }

        private void HandleMovement()
        {
            bool grounded = _controller.isGrounded;
            if (grounded && _velocity.y < 0f) _velocity.y = -2f;

            Vector3 moveDir = transform.right * MoveInput.x + transform.forward * MoveInput.y;
            moveDir = Vector3.ClampMagnitude(moveDir, 1f);

            float targetSpeed = walkSpeed;
            _state = MovementState.Walking;

            if (_isProne)
            {
                targetSpeed = proneSpeed;
                _state = MovementState.Prone;
            }
            else if (_isCrouching)
            {
                targetSpeed = crouchSpeed;
                _state = MovementState.Crouching;
            }
            else if (SprintHeld && MoveInput.y > 0.1f)
            {
                targetSpeed = sprintSpeed;
                _state = MovementState.Sprinting;
            }
            else if (MoveInput.sqrMagnitude < 0.01f)
            {
                _state = MovementState.Idle;
            }

            _controller.Move(moveDir * targetSpeed * Time.deltaTime);

            if (JumpPressed && grounded && !_isCrouching && !_isProne)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _state = MovementState.Jumping;
            }
            JumpPressed = false;

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        public void ApplyLookYaw(float yawDelta)
        {
            transform.Rotate(Vector3.up, yawDelta * rotationSpeed * Time.deltaTime);
        }
    }
}
