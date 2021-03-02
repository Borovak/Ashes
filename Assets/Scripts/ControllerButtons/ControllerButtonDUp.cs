using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonDUp : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.DUp;
        public override string Text => "▲";
        public override InputAction InputAction => ControllerInputs.actions.Menu.DUp;
    }
}