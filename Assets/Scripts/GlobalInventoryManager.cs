using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Classes;
using UnityEngine;

public class GlobalInventoryManager : MonoBehaviour
{
    
    private static Dictionary<int, Inventory> _inventories;
    //private static GlobalInventoryManager _instance;

    void OnEnable()
    {
        _inventories = new Dictionary<int, Inventory>();
    }
    
    public static void SetInventoriesFromXe(XElement xeInventories)
    {
        _inventories ??= new Dictionary<int, Inventory>();
        _inventories.Clear();
        if (xeInventories != null && xeInventories.Elements().Any())
        {
            foreach (var xeInventory in xeInventories.Elements())
            {
                var inventory = Inventory.GetInventoryFromXe(xeInventory);
                if (_inventories.ContainsKey(inventory.id))
                {
                    _inventories[inventory.id] = inventory;
                }
                else
                {
                    _inventories.Add(inventory.id, inventory);
                }
            }
        }
        else
        {
            var playerInventory = new Inventory(-1);
            _inventories.Add(playerInventory.id, playerInventory);
        }
    }
    
    public static XElement GetXeFromInventories()
    {
        var xeInventories = new XElement("Inventories");
        foreach (var inventory in _inventories.Values)
        {
            xeInventories.Add(inventory.GetInventoryXe());
        }
        return xeInventories;
    }

    public static bool TryGetInventory(int id, out Inventory inventory)
    {
        if (_inventories == null)
        {
            inventory = null;
            return false;
        }
        return _inventories.TryGetValue(id, out inventory);
    }

    public static void RegisterToInventoryChange(int id, Action onInventoryChanged)
    {
        if (_inventories == null || !_inventories.TryGetValue(id, out var inventory)) return;
        inventory.InventoryChanged += onInventoryChanged;
    }

    public static void UnregisterToInventoryChange(int id, Action onInventoryChanged)
    {
        if (_inventories == null || !_inventories.TryGetValue(id, out var inventory)) return;
        inventory.InventoryChanged -= onInventoryChanged;
    }

    public static void AddShopInventory(int id, List<ShopItemInfo> shopItemInfos)
    {
        if (_inventories.ContainsKey(id)) return;
        var inventory = new Inventory(id);
        _inventories.Add(id, inventory);
        foreach (var shopItemInfo in shopItemInfos)
        {
            inventory.Add(shopItemInfo.item, shopItemInfo.initial);
        }
    }
}
