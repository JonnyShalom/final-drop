using UnityEngine;
using UnityEngine.EventSystems;
using FinalDrop.Weapons;

namespace FinalDrop.Player
{
    public class MobileInputManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMovementController movement;
        [SerializeField] private TouchJoystick moveJoystick;
        [SerializeField] private WeaponController weapon;

        [Header("Look drag area (right half of screen)")]
        [SerializeField] private RectTransform lookDragArea;
        [SerializeField] private float lookSensitivity = 0.15f;

        private int _lookTouchId = -1;
        private Vector2 _lastLookPos;

        private void Reset()
        {
            movement = GetComponent<PlayerMovementController>();
        }

        private void Update()
        {
            if (movement == null) return;

            movement.MoveInput = moveJoystick != null ? moveJoystick.Value : Vector2.zero;

            HandleLookDrag();
        }

        private void HandleLookDrag()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                bool inLookArea = lookDragArea != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(lookDragArea, touch.position);

                if (!inLookArea) continue;

                if (touch.phase == TouchPhase.Began)
                {
                    _lookTouchId = touch.fingerId;
                    _lastLookPos = touch.position;
                }
                else if (touch.fingerId == _lookTouchId && touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.position.x - _lastLookPos.x;
                    movement.ApplyLookYaw(deltaX * lookSensitivity);
                    _lastLookPos = touch.position;
                }
                else if (touch.fingerId == _lookTouchId &&
                         (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    _lookTouchId = -1;
                }
            }
        }

        public void OnSprintButtonDown() => movement.SprintHeld = true;
        public void OnSprintButtonUp() => movement.SprintHeld = false;
        public void OnCrouchButtonPressed() => movement.CrouchToggle = true;
        public void OnProneButtonPressed() => movement.ProneToggle = true;
        public void OnJumpButtonPressed() => movement.JumpPressed = true;

        public void OnFireButtonDown() => weapon.FireHeld = true;
        public void OnFireButtonUp() => weapon.FireHeld = false;
        public void OnReloadButtonPressed() => weapon.ReloadPressed = true;
    }
}
