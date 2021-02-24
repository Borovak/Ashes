using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using ControllerButtons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerButton : MonoBehaviour
{
    public Constants.ControllerButtons button;
    public bool canFill;
    public bool IsFilled => (_controllerButton?.PressedTime ?? 0f) >= Constants.BUTTON_FILLRATE;
    public bool IsPressed => _controllerButton?.IsPressed ?? false;
    public TextMeshProUGUI textControl;
    public Image backImageControl;
    public Image fillImageControl;

    private IControllerButton _controllerButton;
    private static readonly Color ColorWhenPressed = new Color(0.75f, 0.75f, 0.75f, 1f);
    private static readonly Color ColorWhenNotPressed = new Color(0.5f, 0.5f, 0.5f, 1f);
    
    // Update is called once per frame
    void Update()
    {
        if (_controllerButton == null || button != _controllerButton.Button)
        {
            _controllerButton = ControllerInputs.controllerButtons[button];
            textControl.text = _controllerButton.Text;
        }
        backImageControl.color = _controllerButton.IsPressed ? ColorWhenPressed : ColorWhenNotPressed;
        fillImageControl.fillAmount = canFill ? System.Math.Min(1f, _controllerButton.PressedTime / Constants.BUTTON_FILLRATE) : 0f;
    }
}
