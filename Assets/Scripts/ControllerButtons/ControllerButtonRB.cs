using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonRB : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.RB;
        public override string Text => "RB";
        public override InputAction InputAction => ControllerInputs.actions.Menu.RB;
    }
}