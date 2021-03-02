using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionItemController : MonoBehaviour
{
    public GameOption GameOption
    {
        get => _gameOption;
        set
        {
            _gameOption = value;
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
    public GameObject selectionObject;
    
    private int Index => GameOption.index;
    private bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            selectionObject.SetActive(_isSelected);
        }

    }
    
    private bool _isSelected;
    private IOptionItemControl _optionItemControl;
    private GameOption _gameOption;

    private void OnEnable()
    {
        selectionObject.SetActive(false);
        OptionsMenuController.SelectedIndexChanged += OnSelectedIndexChanged;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DLeft].Pressed += OnDLeftPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DRight].Pressed += OnDRightPressed;
    }

    private void OnDisable()
    {
        OptionsMenuController.SelectedIndexChanged -= OnSelectedIndexChanged;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DLeft].Pressed -= OnDLeftPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DRight].Pressed -= OnDRightPressed;
    }

    private void OnSelectedIndexChanged(int selectedIndex)
    {
        IsSelected = selectedIndex == Index;
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
