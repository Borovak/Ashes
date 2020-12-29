using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionItemController : MonoBehaviour
{
    public string parameterId;
    public TextMeshProUGUI parameterTextControl;

    private IOptionItemControl _optionItemControl;
    private GameOption _gameOption;

    void OnEnable()
    {
        FindOptionItemControl();
        _optionItemControl.ValueChanged += OnValueChanged;   
        if (GameOptionsManager.TryGetOption(parameterId, out _gameOption))
        { 
            parameterTextControl.text = _gameOption.name;
            _optionItemControl.SetValue(_gameOption.value);
        }
        else
        {
            parameterTextControl.text = "Option not found";
        }
    }

    void OnDisable()
    {
        FindOptionItemControl();
        _optionItemControl.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(string value)
    {
        _gameOption.value = value;
    }

    private void FindOptionItemControl()
    {
        if (_optionItemControl != null) return;
        var types = new[] { typeof(ToggleControlController) };
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
