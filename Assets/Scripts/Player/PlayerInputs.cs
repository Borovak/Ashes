using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputs : MonoBehaviour
    {
        public event Action Interact;
        public event Action Jump;
        public event Action JumpRelease;
        public event Action DropThrough;
        public event Action Dash;
        public event Action GroundBreak;
        public Vector2 movement;
        public Actions _actions;

        private Dictionary<InputAction, Action<InputAction.CallbackContext>> _pairingDictionary;


        void Awake()
        {
            _actions = new Actions();
            _pairingDictionary = new Dictionary<InputAction, Action<InputAction.CallbackContext>>
            {
                {_actions.Player.Interact, OnInteract},
                {_actions.Player.Jump, OnJump},
                {_actions.Player.Movement, OnMovement},
                {_actions.Player.Dash, OnDash},
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (GameController.gameState != GameController.GameStates.Running) return;
            Interact?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (GameController.gameState != GameController.GameStates.Running || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            Action a;
            var buttonState = context.ReadValueAsButton();
            if (movement.y < 0f && buttonState)
            {
                a = DropThrough;
            }
            else
            {
                a = buttonState ? Jump : JumpRelease;
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
    }
}