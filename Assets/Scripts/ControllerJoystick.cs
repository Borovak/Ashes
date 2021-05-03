using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerJoystick
{
    public event Action<Vector2> ValueChanged;
    public event Action<float> AngleChanged;
    public Vector2 value;
    public float angle;

    private readonly InputAction _inputAction;

    public ControllerJoystick(InputAction inputAction)
    {
        _inputAction = inputAction;
    }

    public void Enable()
    {
        _inputAction.performed += OnChange;
    }

    public void Disable()
    {
        _inputAction.performed -= OnChange;
    }

    private void OnChange(InputAction.CallbackContext context)
    {
        var previousValue = value;
        var previousAngle = angle;
        value = context.ReadValue<Vector2>();
        var tempAngle = value == Vector2.zero ? -1f : Vector2.Angle(Vector2.up, value);
        angle = value.x >= 0f ? tempAngle : 360f - tempAngle;
        if (value != previousValue)
        {
            ValueChanged?.Invoke(value);
        }
        if (Math.Abs(angle - previousAngle) > 0.1f)
        {
            AngleChanged?.Invoke(angle);
        }
    }
}