using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using ControllerButtons;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInputs : MonoBehaviour
{
    public static Dictionary<Constants.ControllerButtons, IControllerButton> controllerButtons;
    public static Actions actions;
    public static ControllerJoystick leftJoystick;
    public static ControllerJoystick rightJoystick;

    void Awake()
    {
        actions ??= new Actions();
        if (controllerButtons == null)
        {
            controllerButtons = new Dictionary<Constants.ControllerButtons, IControllerButton>();
        }
        else
        {
            controllerButtons.Clear();
        }
        var classes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IControllerButton).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();
        foreach (var c in classes)
        {
            var instance = Activator.CreateInstance(c) as IControllerButton;
            if (instance == null) return;
            controllerButtons.Add(instance.Button, instance);
        }
        //Joysticks
        leftJoystick = new ControllerJoystick(actions.Menu.LeftJoystick);
        rightJoystick = new ControllerJoystick(actions.Menu.RightJoystick);
    }
    
    void OnEnable()
    {
        actions.Enable();
        foreach (var controllerButton in controllerButtons.Values)
        {
            controllerButton.InputAction.performed += controllerButton.OnPerformed;
        }
        //Joysticks
        leftJoystick.Enable();
        rightJoystick.Enable();
    }

    void OnDisable()
    {
        actions.Disable();
        foreach (var controllerButton in controllerButtons.Values)
        {
            controllerButton.InputAction.performed -= controllerButton.OnPerformed;
        }
        //Joysticks
        leftJoystick.Disable();
        rightJoystick.Disable();
    }

    void Update()
    {
        foreach (var controllerButton in controllerButtons.Values)
        {
            controllerButton.Update();
        }
    }
    
}