using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonRT : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.RT;
        public override string Text => "RT";
        public override InputAction InputAction => ControllerInputs.actions.Menu.RT;
    }
}