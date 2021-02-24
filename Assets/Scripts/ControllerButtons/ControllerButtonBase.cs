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
        public bool IsPressed { get; set; }
        public float PressedTime { get; set; }

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
            }
        }
    }
}