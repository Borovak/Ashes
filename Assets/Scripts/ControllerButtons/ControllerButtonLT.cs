using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonLT : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.LT;
        public override string Text => "LT";
        public override InputAction InputAction => ControllerInputs.actions.Menu.LT;
    }
}