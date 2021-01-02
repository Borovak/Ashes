using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionItemController : MonoBehaviour
{
    public GameOption gameOption
    {
        get => _gameOption;
        set
        {
            _gameOption = value;
            FindOptionItemControl();
            _optionItemControl.ValueChanged += OnValueChanged;
            if (gameOption != null)
            {                
                parameterTextControl.text = gameOption.name;
                _optionItemControl.SetValue(gameOption.value);
            }
            else
            {
                Debug.Log("Option not found");
                parameterTextControl.text = "Option not found";
            }
        }
    }
    public TextMeshProUGUI parameterTextControl;

    private IOptionItemControl _optionItemControl;
    private GameOption _gameOption;

    private void OnValueChanged(string value)
    {
        if (gameOption == null) return;
        gameOption.value = value;
    }

    private void FindOptionItemControl()
    {
        if (_optionItemControl != null) return;
        var types = new[] { typeof(ToggleControlController), typeof(SliderControlController) };
        foreach (Transform child in transform)
        {
            foreach (var type in types)
            {
                if (!child.TryGetComponent(type, out var component)) continue;
                _optionItemControl = (IOptionItemControl)component;
                return;
            }
        }
    }
}
