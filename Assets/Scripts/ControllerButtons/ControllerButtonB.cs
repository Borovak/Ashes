using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonB : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.B;
        public override string Text => "B";
        public override InputAction InputAction => ControllerInputs.actions.Menu.B;
    }
}