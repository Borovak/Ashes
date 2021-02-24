using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using ControllerButtons;
using UnityEngine;

public class ControllerInputs : MonoBehaviour
{
    public static Dictionary<Constants.ControllerButtons, IControllerButton> controllerButtons;
    public static Actions actions;

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
    }
    
    void OnEnable()
    {
        actions.Enable();
        foreach (var controllerButton in controllerButtons.Values)
        {
            controllerButton.InputAction.performed += controllerButton.OnPerformed;
        }
    }

    void OnDisable()
    {
        actions.Disable();
        foreach (var controllerButton in controllerButtons.Values)
        {
            controllerButton.InputAction.performed -= controllerButton.OnPerformed;
        }
    }

    void Update()
    {
        foreach (var controllerButton in controllerButtons.Values)
        {
            controllerButton.PressedTime = controllerButton.IsPressed ? controllerButton.PressedTime + Time.deltaTime : 0f;
        }
    }
    
}