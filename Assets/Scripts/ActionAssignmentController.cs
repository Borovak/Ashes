using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Classes;
using Interfaces;
using Static;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ActionAssignmentController : MonoBehaviour
{
    public static event System.Action AssignmentChanged;
    
    private class PlayerActionButtonAssoc
    {
        internal int Id;

        internal void Act()
        {
            if (Id <= 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            var actionElement = actionElements.First(x => x.id == Id);
            if (actionElement == null) return;
            actionElement.Action?.Act();
        }
        
        internal void ActFinished()
        {
            if (Id <= 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            var actionElement = actionElements.First(x => x.id == Id);
            if (actionElement == null) return;
            actionElement.Action?.ActFinished();
        }
    }
    private static Dictionary<Constants.ControllerButtons, PlayerActionButtonAssoc> _playerActionButtonAssocs;
    public static List<DB.ActionElement> actionElements;
    
    // Start is called before the first frame update
    void Start()
    {
        actionElements = new List<DB.ActionElement>();
        _playerActionButtonAssocs = new Dictionary<Constants.ControllerButtons, PlayerActionButtonAssoc>();
        if (!DataHandling.TryConnectToDb(out var connection)) return;
        foreach (var actionElement in connection.Table<DB.ActionElement>().AsEnumerable())
        {
            actionElements.Add(actionElement);
            var t = Type.GetType($"ActionCode.Action{actionElement.Id.ToString("0000")}");
            if (t == null) continue;
            actionElement.Action = (IAction)Activator.CreateInstance(t);
        }
        Assign(Constants.ControllerButtons.X);
        Assign(Constants.ControllerButtons.B);
        Attach(Constants.ControllerButtons.X, 1);
        Attach(Constants.ControllerButtons.B, 2);
    }
    
    private static void Assign(Constants.ControllerButtons button)
    {
        var assoc = new PlayerActionButtonAssoc();
        _playerActionButtonAssocs.Add(button, assoc);
        ControllerInputs.controllerButtons[button].Pressed -= assoc.Act;
        ControllerInputs.controllerButtons[button].Released -= assoc.ActFinished;
        ControllerInputs.controllerButtons[button].Pressed += assoc.Act;
        ControllerInputs.controllerButtons[button].Released += assoc.ActFinished;
    }

    public static void Attach(Constants.ControllerButtons button, int itemId)
    {
        foreach (var assoc in _playerActionButtonAssocs.Where(x => x.Value.Id == itemId))
        {
            assoc.Value.Id = 0;
        }
        _playerActionButtonAssocs[button].Id = itemId;
        AssignmentChanged?.Invoke();
    }

    public static void Detach(Constants.ControllerButtons button)
    {
        _playerActionButtonAssocs[button].Id = 0;
    }
    
    // public static void Assign(Constants.ControllerButtons button, DB.Action action)
    // {
    //     if (_playerActionButtonAssocs.Any(x => x.button == button))
    //     {
    //         var item = _playerActionButtonAssocs.First(x => x.button == button);
    //         ControllerInputs.controllerButtons[item.button].Pressed -= item.action.Press;
    //         ControllerInputs.controllerButtons[item.button].Released -= item.action.Release;
    //         _playerActionButtonAssocs.Remove(item);
    //     }
    //     _playerActionButtonAssocs.Add(new PlayerActionButtonAssoc {button = button, action = action});
    //     ControllerInputs.controllerButtons[button].Pressed += action.Press;
    //     ControllerInputs.controllerButtons[button].Released += action.Release;
    //     AssignmentChanged?.Invoke();
    // }
    //
    // public static void Attach(int id, System.Action actionOnPressed, System.Action actionOnReleased = null)
    // {
    //     actions.First(x => x.Id == id).Pressed += actionOnPressed;
    //     if (actionOnReleased != null)
    //     {
    //         actions.First(x => x.Id == id).Released += actionOnReleased;
    //     }
    // }
    //
    // public static void Detach(int id, System.Action actionOnPressed, System.Action actionOnReleased = null)
    // {
    //     actions.First(x => x.Id == id).Pressed -= actionOnPressed;
    //     if (actionOnReleased != null)
    //     {
    //         actions.First(x => x.Id == id).Released -= actionOnReleased;
    //     }
    // }

    public static Sprite GetArt(Constants.ControllerButtons button, Transform parentTransform)
    {
        if (_playerActionButtonAssocs == null || !_playerActionButtonAssocs.TryGetValue(button, out var playerActionButtonAssoc) || actionElements.All(x => x.id != playerActionButtonAssoc.Id)) return null;
        var path = actionElements.First(x => x.id == playerActionButtonAssoc.Id).Path;
        var res = Resources.Load(path);
        if (res == null || !(res is Texture2D texture))
        {
            Debug.Log($"Missing resource: {path}");
            return null;
        }
        return Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
