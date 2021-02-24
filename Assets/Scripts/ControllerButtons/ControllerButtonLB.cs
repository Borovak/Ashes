using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonLB : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.LB;
        public override string Text => "LB";
        public override InputAction InputAction => ControllerInputs.actions.Menu.LB;
    }
}