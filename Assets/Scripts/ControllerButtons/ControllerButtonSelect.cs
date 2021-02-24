using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonSelect : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.Select;
        public override string Text => "Select";
        public override InputAction InputAction => ControllerInputs.actions.Menu.Select;
    }
}