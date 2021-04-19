using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Static;
using UnityEngine;

public class ItemAssignmentController : MonoBehaviour
{
    public static event Action AssignmentChanged;
    
    private class PlayerItemButtonAssoc
    {
        internal int itemId;
        internal int Quantity => GlobalInventoryManager.TryGetInventory(-1, out var inventory) ? inventory.GetQuantity(itemId) : -1;

        internal void Use()
        {
            if (itemId <= 0) return;
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            if (inventory.GetQuantity(itemId) <= 0) return;
            var item = DropController.GetDropInfo(itemId);
            if (item == null) return;
            item.itemAction.Use();
            inventory.Remove(itemId, 1);
        }
    }
    private static Dictionary<Constants.ControllerButtons, PlayerItemButtonAssoc> _playerItemButtonAssocs;
    
    void Awake()
    {
        _playerItemButtonAssocs = new Dictionary<Constants.ControllerButtons, PlayerItemButtonAssoc>();
        Assign(Constants.ControllerButtons.DUp);
        Assign(Constants.ControllerButtons.DDown);
        Assign(Constants.ControllerButtons.DLeft);
        Assign(Constants.ControllerButtons.DRight);
        Attach(Constants.ControllerButtons.DUp, 1000);
    }

    private static void Assign(Constants.ControllerButtons button)
    {
        var assoc = new PlayerItemButtonAssoc();
        _playerItemButtonAssocs.Add(button, assoc);
        ControllerInputs.controllerButtons[button].Pressed += assoc.Use;
    }

    public static void Attach(Constants.ControllerButtons button, int itemId)
    {
        foreach (var assoc in _playerItemButtonAssocs.Where(x => x.Value.itemId == itemId))
        {
            assoc.Value.itemId = 0;
        }
        _playerItemButtonAssocs[button].itemId = itemId;
        AssignmentChanged?.Invoke();
    }

    public static void Detach(Constants.ControllerButtons button)
    {
        _playerItemButtonAssocs[button].itemId = 0;
    }

    public static Sprite GetArt(Constants.ControllerButtons button)
    {
        if (_playerItemButtonAssocs == null || !_playerItemButtonAssocs.ContainsKey(button)) return null;
        var assoc = _playerItemButtonAssocs[button];
        var item = DropController.GetDropInfo(assoc.itemId);
        if (item == null || item.Id == Constants.MONEY_ID) return null;
        return item.GetArt();
    }

    public static int GetQuantity(Constants.ControllerButtons button)
    {
        if (_playerItemButtonAssocs == null || !_playerItemButtonAssocs.ContainsKey(button)) return -1;
        var assoc = _playerItemButtonAssocs[button];
        return assoc.Quantity;
    }
}