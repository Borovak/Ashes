using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonDRight : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.DRight;
        public override string Text => "►";
        public override InputAction InputAction => ControllerInputs.actions.Menu.DRight;
    }
}