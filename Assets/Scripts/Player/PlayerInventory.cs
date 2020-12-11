using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private class ItemGainedQueueItem
    {
        internal int id;
        internal int quantity;
    }
    
    public static event Action InventoryChanged;
    private Dictionary<int, int> _items = new Dictionary<int, int>();
    public GameObject ItemGainedPrefab;
    public bool AddItem;

    private List<ItemGainedQueueItem> _itemGainedQueue;

    void Start()
    {
        _itemGainedQueue = new List<ItemGainedQueueItem>();
        SetInventoryFromString(SaveSystem.LastLoadedSave.Inventory);
    }

    public void Add(int id, int quantity = 1)
    {
        if (!_items.ContainsKey(id))
        {
            _items.Add(id, quantity);
        }
        else
        {
            _items[id] = _items[id] + quantity;
        }
        InventoryChanged?.Invoke();
        //Instantiate item gained paneld
        var gameUITransform = GameObject.FindGameObjectWithTag("GameUI").transform;
        var itemGained = GameObject.Instantiate(ItemGainedPrefab, gameUITransform);
        var itemGainedController = itemGained.GetComponent<ItemGainedController>();
        itemGainedController.itemId = id;
        itemGainedController.itemQuantity = quantity;
    }

    public void Remove(int id, int quantity = 1)
    {
        if (!_items.ContainsKey(id))
        {
            _items.Add(id, 0);
        }
        else
        {
            _items[id] = System.Math.Max(_items[id] - quantity, 0);
        }
        InventoryChanged?.Invoke();
    }

    public int GetCount(int id)
    {
        return _items.TryGetValue(id, out var count) ? count : 0;
    }

    public string GetInventoryString()
    {
        var lst = new List<string>();
        foreach (var item in _items)
        {
            if (item.Value == 0) continue;
            lst.Add($"{item.Key},{item.Value}");
        }
        return string.Join(";", lst);
    }

    public void SetInventoryFromString(string data)
    {
        _items.Clear();
        if (string.IsNullOrEmpty(data)) return;
        var items = data.Split(';');
        foreach (var item in items)
        {
            var x = item.Split(',');
            _items[int.Parse(x[0])] = int.Parse(x[1]);
        }
    }

    public void GetItemsAndCounts(out List<Item> items, out List<int> counts)
    {
        items = new List<Item>();
        counts = new List<int>();
        foreach (var entry in _items)
        {
            if (entry.Value == 0) continue;
            var item = DropController.GetDropInfo(entry.Key);
            var count = entry.Value;
            items.Add(item);
            counts.Add(count);
        }
    }

    public void Add(Item item, int quantity = 1)
    {
        Add(item.id, quantity);
    }

    public void Remove(Item item, int quantity = 1)
    {
        Remove(item.id, quantity);
    }

    public int GetCount(Item item)
    {
        return GetCount(item.id);
    }

    void Update(){
        if (!AddItem) return;
        AddItem = false;
        Add(1,1);
    }
}