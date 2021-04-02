using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public abstract class ControllerButtonBase : IControllerButton
    {
        public abstract Constants.ControllerButtons Button { get; }
        public virtual string Text => "";
        public virtual Sprite Sprite => null;
        public abstract InputAction InputAction { get; }
        public event Action Pressed;
        public event Action Released;
        public event Action<bool> StateChanged;
        public event Action Filled;
        public bool IsPressed { get; set; }
        public float PressedTime { get; set; }

        private bool _fillLatch;

        public void OnPerformed(InputAction.CallbackContext context)
        {
            IsPressed = context.ReadValueAsButton();
            if (IsPressed)
            {
                Pressed?.Invoke();
            }
            else
            {
                Released?.Invoke();
                _fillLatch = false;
            }
            StateChanged?.Invoke(IsPressed);
        }

        public void Update()
        {
            PressedTime = IsPressed ? PressedTime + Time.deltaTime : 0f;
            if (!_fillLatch && PressedTime > Constants.BUTTON_FILLRATE)
            {
                _fillLatch = true;
                Filled?.Invoke();
            }
        }
    }
}