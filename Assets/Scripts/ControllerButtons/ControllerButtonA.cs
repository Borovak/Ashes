using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonA : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.A;
        public override string Text => "A";
        public override InputAction InputAction => ControllerInputs.actions.Menu.A;
    }
}