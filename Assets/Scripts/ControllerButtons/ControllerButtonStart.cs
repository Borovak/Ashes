using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonStart : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.Start;
        public override string Text => "Start";
        public override InputAction InputAction => ControllerInputs.actions.Menu.Start;
    }
}