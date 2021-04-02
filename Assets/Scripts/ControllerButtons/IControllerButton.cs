using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public interface IControllerButton
    {
        Constants.ControllerButtons Button { get; }
        string Text { get; }
        Sprite Sprite { get; }
        InputAction InputAction { get; }
        event Action Pressed;
        event Action Released;
        event Action<bool> StateChanged;
        event Action Filled;
        bool IsPressed { get; set; }
        float PressedTime { get; set; }
        void OnPerformed(InputAction.CallbackContext context);
        void Update();

    }
}