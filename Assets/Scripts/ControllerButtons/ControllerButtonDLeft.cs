using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonDLeft : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.DLeft;
        public override string Text => "◄";
        public override InputAction InputAction => ControllerInputs.actions.Menu.DLeft;
    }
}