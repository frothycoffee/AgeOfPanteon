using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public enum MouseButton
    {
        Left,
        Right
    }

    public class MouseUser : MonoBehaviour
    {
        private InputActions _inputActions;

        public Vector2 mousePosition { get; private set; }
        public Vector2 mouseInWorldPosition => Camera.main.ScreenToWorldPoint(mousePosition);

        private bool _isLeftMouseButtonPressed;
        private bool _isRightMouseButtonPressed;

        private void OnEnable()
        {
            _inputActions = InputActions.Instance;
            _inputActions.Game.MousePosition.performed += OnMousePositionPerformed;
            _inputActions.Game.LeftMouseButtonAction.performed += OnLeftMouseButtonActionPerformed;
            _inputActions.Game.LeftMouseButtonAction.canceled += OnLeftMouseButtonActionCanceled;
            _inputActions.Game.RightMouseButtonAction.performed += OnRightMouseButtonActionPerformed;
            _inputActions.Game.RightMouseButtonAction.canceled += OnRightMouseButtonActionCanceled;
        }

        private void OnDisable()
        {
            _inputActions.Game.MousePosition.performed -= OnMousePositionPerformed;
            _inputActions.Game.LeftMouseButtonAction.performed -= OnLeftMouseButtonActionPerformed;
            _inputActions.Game.LeftMouseButtonAction.canceled -= OnLeftMouseButtonActionCanceled;
            _inputActions.Game.RightMouseButtonAction.performed -= OnRightMouseButtonActionPerformed;
            _inputActions.Game.RightMouseButtonAction.canceled -= OnRightMouseButtonActionCanceled;
        }

        private void OnMousePositionPerformed(InputAction.CallbackContext ctx)
        {
            mousePosition = ctx.ReadValue<Vector2>();
        }

        private void OnLeftMouseButtonActionPerformed(InputAction.CallbackContext ctx)
        {
            _isLeftMouseButtonPressed = true;
            EventManager.Brodcast(GameEvent.LeftMouseButtonActionPerformed);
        }

        private void OnLeftMouseButtonActionCanceled(InputAction.CallbackContext ctx)
        {
            _isLeftMouseButtonPressed = false;
        }

        private void OnRightMouseButtonActionPerformed(InputAction.CallbackContext ctx)
        {
            _isRightMouseButtonPressed = true;
            EventManager.Brodcast(GameEvent.RightMouseButtonActionPerformed);
        }

        private void OnRightMouseButtonActionCanceled(InputAction.CallbackContext ctx)
        {
            _isRightMouseButtonPressed = false;
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            return button == MouseButton.Left ? _isLeftMouseButtonPressed : _isRightMouseButtonPressed;
        }
    }
}