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
    public event Action Roll;
    public event Action GroundBreak;
    public event Action SelfSpell;
    public Vector2 movement;
    public Actions _actions;

    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;
    private GameController _gameController;

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
            {_actions.Player.Roll,OnRoll}
        };
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
        if (_gameController.gameState != GameController.GameStates.Running) return;
        Attack?.Invoke();
    }

    public void OnAttackSpell(InputAction.CallbackContext context)
    {
        if (_gameController.gameState != GameController.GameStates.Running) return;
        AttackSpell?.Invoke();
    }

    public void OnSelfSpell(InputAction.CallbackContext context)
    {
        if (_gameController.gameState != GameController.GameStates.Running) return;
        SelfSpell?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (_gameController.gameState != GameController.GameStates.Running) return;
        Interact?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_gameController.gameState != GameController.GameStates.Running) return;
        var a = context.ReadValue<float>() > 0.1f ? Jump : JumpRelease;
        a?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (_gameController.gameState != GameController.GameStates.Running) return;
        movement = context.ReadValue<Vector2>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (_gameController.gameState != GameController.GameStates.Running) return;
        if (movement.y < -0.1f){
            GroundBreak?.Invoke();
        } else {            
            Roll?.Invoke();
        }
    }
}