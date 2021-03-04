using Classes;
using Interfaces;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionItemController : SelectableItem
{
    
    public GameOption GameOption
    {
        get => _gameOption;
        set
        {
            _gameOption = value;
            index = GameOption.index;
            FindOptionItemControl();
            _optionItemControl.ValueChanged += OnValueChanged;
            if (GameOption != null)
            {                
                parameterTextControl.text = GameOption.name;
                _optionItemControl.SetValue(GameOption.value);
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

    protected override void OnEnableAfter()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DLeft].Pressed += OnDLeftPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DRight].Pressed += OnDRightPressed;
    }

    protected override void OnDisableAfter()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DLeft].Pressed -= OnDLeftPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DRight].Pressed -= OnDRightPressed;
    }

    private void OnValueChanged(string value)
    {
        if (GameOption == null) return;
        GameOption.value = value;
    }

    private void OnDLeftPressed()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!IsSelected) return;
        _optionItemControl.OnDLeftPressed();
    }

    private void OnDRightPressed()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!IsSelected) return;
        _optionItemControl.OnDRightPressed();
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
