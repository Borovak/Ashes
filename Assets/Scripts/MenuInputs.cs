using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MenuInputs : MonoBehaviour
{
    public static event Action SelectionChangeUp;
    public static event Action SelectionChangeDown;
    public static event Action SelectionChangeLeft;
    public static event Action SelectionChangeRight;
    public static event Action Inventory;
    public static event Action Crafting;
    public static event Action Map;
    
    public static Vector2 movement;


    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;
    
    void OnEnable()
    {
        _pairingDictionary ??= new Dictionary<InputAction, Action<InputAction.CallbackContext>>
        {
            {ControllerInputs.actions.Menu.Movement, OnMovement},
            {ControllerInputs.actions.Menu.SelectionChange, OnSelectionChange},
            {ControllerInputs.actions.Menu.Inventory, OnInventory},
            {ControllerInputs.actions.Menu.Crafting, OnCrafting},
            {ControllerInputs.actions.Menu.Map, OnMap},
        };
        foreach (var item in _pairingDictionary)
        {
            item.Key.performed += item.Value;
        }
    }

    void OnDisable()
    {
        foreach (var item in _pairingDictionary)
        {
            item.Key.performed -= item.Value;
        }
    }

    

    public void OnSelectionChange(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        if (value.y > 0.1f)
        {
            SelectionChangeUp?.Invoke();
        }
        else if (value.y < -0.1f)
        {
            SelectionChangeDown?.Invoke();
        }
        else if (value.x < -0.1f)
        {
            SelectionChangeLeft?.Invoke();
        }
        else if (value.x > 0.1f)
        {
            SelectionChangeRight?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        Inventory?.Invoke();
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        Map?.Invoke();
    }

    public void OnCrafting(InputAction.CallbackContext context)
    {
        Crafting?.Invoke();
    }
}
