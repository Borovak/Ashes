using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputs : MonoBehaviour
{
    public static event Action Start;
    public static event Action Select;
    public static event Action OK;
    public static event Action Back;
    public static event Action SelectionChangeUp;
    public static event Action SelectionChangeDown;
    public static event Action SelectionChangeLeft;
    public static event Action SelectionChangeRight;

    private Actions _actions;
    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;

    void Awake()
    {
        _actions = new Actions();
        _pairingDictionary = new Dictionary<InputAction, Action<InputAction.CallbackContext>>
        {
            {_actions.Menu.Start, OnStart},
            {_actions.Menu.Select, OnSelect},
            {_actions.Menu.OK, OnOK},
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

    public void OnStart(InputAction.CallbackContext context)
    {
        Start?.Invoke();
    }
    
    public void OnSelect(InputAction.CallbackContext context)
    {
        Select?.Invoke();
    }
    
    public void OnOK(InputAction.CallbackContext context)
    {
        OK?.Invoke();
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        Back?.Invoke();
    }

    public void OnSelectionChange(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        if (value.y < -0.1f){
            SelectionChangeUp?.Invoke();
        } else if (value.y > 0.1f){
            SelectionChangeDown?.Invoke();
        } else if (value.x < -0.1f){
            SelectionChangeLeft?.Invoke();
        } else if (value.x > 0.1f){
            SelectionChangeRight?.Invoke();
        }
    }
}
