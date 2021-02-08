using System;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputs : MonoBehaviour
{
    public static event Action Start;
    public static event Action Select;
    public static event Action OK;
    public static event Action Back;
    public static event Action Special;
    public static event Action SelectionChangeUp;
    public static event Action SelectionChangeDown;
    public static event Action SelectionChangeLeft;
    public static event Action SelectionChangeRight;
    public static event Action MapZoomIn;
    public static event Action MapZoomOut;
    public static event Action SectionPrevious;
    public static event Action SectionNext;
    public static event Action Crafting;
    public static event Action Map;
    public static Vector2 movement;


    private Actions _actions;
    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;

    void OnEnable()
    {
        if (_actions == null)
        {
            _actions = new Actions();
            _pairingDictionary = new Dictionary<InputAction, Action<InputAction.CallbackContext>>
            {
                {_actions.Menu.Start, OnStart},
                {_actions.Menu.Select, OnSelect},
                {_actions.Menu.OK, OnOK},
                {_actions.Menu.Back, OnBack},
                {_actions.Menu.Special, OnSpecial},
                {_actions.Menu.Movement, OnMovement},
                {_actions.Menu.SelectionChange, OnSelectionChange},
                {_actions.Menu.SectionNext, OnSectionNext},
                {_actions.Menu.SectionPrevious, OnSectionPrevious},
                {_actions.Menu.MapZoomIn, OnMapZoomIn},
                {_actions.Menu.MapZoomOut, OnMapZoomOut},
                {_actions.Menu.Crafting, OnCrafting},
                {_actions.Menu.Map, OnMap},
            };
        }
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

    public void OnSpecial(InputAction.CallbackContext context)
    {
        Special?.Invoke();
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

    public void OnSectionNext(InputAction.CallbackContext context)
    {
        SectionNext?.Invoke();
    }

    public void OnSectionPrevious(InputAction.CallbackContext context)
    {
        SectionPrevious?.Invoke();
    }

    public void OnMapZoomIn(InputAction.CallbackContext context)
    {
        MapZoomIn?.Invoke();
    }

    public void OnMapZoomOut(InputAction.CallbackContext context)
    {
        MapZoomOut?.Invoke();
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
