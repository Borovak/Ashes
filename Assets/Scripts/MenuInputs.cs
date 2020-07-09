using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputs : MonoBehaviour
{
    public event Action Select;
    public event Action Back;
    public event Action SelectionChangePositive;
    public event Action SelectionChangeNegative;
    public Actions _actions;

    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;

    void Awake()
    {
        _actions = new Actions();
        _pairingDictionary = new Dictionary<InputAction, Action<InputAction.CallbackContext>>
        {
            {_actions.Menu.Select, OnSelect},
            {_actions.Menu.Back, OnBack},
            {_actions.Menu.SelectionChange, OnSelectionChange}
        };
    }

    void OnEnable()
    {
        _actions.Enable();
        foreach (var item in _pairingDictionary)
        {
            item.Key.performed += item.Value;
        }
    }

    void OnDisable()
    {
        _actions.Disable();
        foreach (var item in _pairingDictionary)
        {
            item.Key.performed -= item.Value;
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        Select?.Invoke();
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        Back?.Invoke();
    }

    public void OnSelectionChange(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        var a = value > 0.1f ? SelectionChangePositive : SelectionChangeNegative;
        a?.Invoke();
    }
}
