using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public event Action Attack;
    public event Action AttackSpell;
    public event Action Interact;
    public event Action Jump;
    public event Action JumpRelease;
    public event Action DropThrough;
    public event Action Dash;
    public event Action GroundBreak;
    public event Action SelfSpell;
    public event Action Shield;
    public event Action ShieldRelease;
    public Vector2 movement;
    public Actions _actions;

    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;


    void Awake()
    {
        _actions = new Actions();
        _pairingDictionary = new Dictionary<InputAction, Action<InputAction.CallbackContext>>
        {
            {_actions.Player.Attack, OnAttack},
            {_actions.Player.AttackSpell, OnAttackSpell},
            {_actions.Player.SelfSpell, OnSelfSpell},
            {_actions.Player.Interact, OnInteract},
            {_actions.Player.Jump, OnJump},
            {_actions.Player.Movement, OnMovement},
            {_actions.Player.Dash, OnDash},
            {_actions.Player.Shield, OnShield},
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        Attack?.Invoke();
    }

    public void OnAttackSpell(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        AttackSpell?.Invoke();
    }

    public void OnSelfSpell(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        SelfSpell?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        Interact?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        Action a;
        if (movement.y < 0f && context.ReadValue<float>() > 0.1f)
        {
            a = DropThrough;
        }
        else
        {
            a = context.ReadValue<float>() > 0.1f ? Jump : JumpRelease;
        }
        a?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        movement = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        if (movement.y < -0.1f)
        {
            GroundBreak?.Invoke();
        }
        else
        {
            Dash?.Invoke();
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        var a = context.ReadValue<float>() > 0.1f ? Shield : ShieldRelease;
        a?.Invoke();
    }
}