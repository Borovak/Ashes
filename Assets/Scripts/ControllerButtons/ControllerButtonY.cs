using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonY : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.Y;
        public override string Text => "Y";
        public override InputAction InputAction => ControllerInputs.actions.Menu.Y;
    }
}