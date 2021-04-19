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
        internal Constants.ControllerButtons button;
        internal DB.Action action;
    }
    private static List<PlayerActionButtonAssoc> _playerActionButtonAssocs;
    private static List<DB.Action> _actions;
    
    // Start is called before the first frame update
    void Start()
    {
        _actions = new List<DB.Action>();
        _playerActionButtonAssocs = new List<PlayerActionButtonAssoc>();
        if (!DataHandling.TryConnectToDb(out var connection)) return;
        foreach (var item in connection.Table<DB.Action>().AsEnumerable())
        {
            _actions.Add(item);
        }
        Assign(Constants.ControllerButtons.X, _actions.First(x => x.Id == 1));
        Assign(Constants.ControllerButtons.B, _actions.First(x => x.Id == 2));
        Assign(Constants.ControllerButtons.LB, _actions.First(x => x.Id == 3));
        Assign(Constants.ControllerButtons.RB, _actions.First(x => x.Id == 4));
    }

    public static void Assign(Constants.ControllerButtons button, DB.Action action)
    {
        if (_playerActionButtonAssocs.Any(x => x.button == button))
        {
            var item = _playerActionButtonAssocs.First(x => x.button == button);
            ControllerInputs.controllerButtons[item.button].Pressed -= item.action.Press;
            ControllerInputs.controllerButtons[item.button].Released -= item.action.Release;
            _playerActionButtonAssocs.Remove(item);
        }
        _playerActionButtonAssocs.Add(new PlayerActionButtonAssoc {button = button, action = action});
        ControllerInputs.controllerButtons[button].Pressed += action.Press;
        ControllerInputs.controllerButtons[button].Released += action.Release;
        AssignmentChanged?.Invoke();
    }

    public static void Attach(int id, System.Action actionOnPressed, System.Action actionOnReleased = null)
    {
        _actions.First(x => x.Id == id).Pressed += actionOnPressed;
        if (actionOnReleased != null)
        {
            _actions.First(x => x.Id == id).Released += actionOnReleased;
        }
    }

    public static void Detach(int id, System.Action actionOnPressed, System.Action actionOnReleased = null)
    {
        _actions.First(x => x.Id == id).Pressed -= actionOnPressed;
        if (actionOnReleased != null)
        {
            _actions.First(x => x.Id == id).Released -= actionOnReleased;
        }
    }

    public static GameObject GetArtObject(Constants.ControllerButtons button, Transform parentTransform, GameObject imageGameObject)
    {
        if (_playerActionButtonAssocs == null) return null;
        var playerActionButtonAssoc = _playerActionButtonAssocs.FirstOrDefault(x => x.button == button);
        if (playerActionButtonAssoc == null) return null;
        var res = Resources.Load(playerActionButtonAssoc.action.Path);
        if (res == null)
        {
            Debug.Log($"Missing resource: {playerActionButtonAssoc.action.Path}");
            return null;
        }
        if (res is Texture2D texture)
        {
            var image = imageGameObject.GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), new Vector2(0.5f, 0.5f));;
            return imageGameObject;
        } 
        if (res is GameObject o)
        {
            Destroy(imageGameObject);
            var newObject = Instantiate(o, parentTransform);
            newObject.transform.SetSiblingIndex(0);
            return newObject;
        }
        return null;
    }
}
