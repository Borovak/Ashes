using System;
using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerButtons
{
    public class ControllerButtonX : ControllerButtonBase
    {
        public override Constants.ControllerButtons Button => Constants.ControllerButtons.X;
        public override string Text => "X";
        public override InputAction InputAction => ControllerInputs.actions.Menu.X;
    }
}