using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonDDown : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.DDown;
        public override string Text => "▼";
        public override InputAction InputAction => ControllerInputs.actions.Menu.DDown;
    }
}