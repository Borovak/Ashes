using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static event Action InventoryChanged;
    public GameObject ItemGainedPrefab;
    public bool AddItemTest;
    public float timeBetweenItemGainedPanel = 0.5f;
    public int money = 0;

    private List<ItemBundle> _itemGainedQueue;
    private Dictionary<int, int> _items;
    private float _timeSinceLastItemGainedPanel = 0f;

    void Start()
    {
        _items = new Dictionary<int, int>();
        _itemGainedQueue = new List<ItemBundle>();
        SetInventoryFromString(SaveSystem.LastLoadedSave.Inventory);
    }

    public void Add(int id, int quantity = 1)
    {
        if (id == Constants.MONEY_ID)
        {
            money += quantity;
            return;
        }
        if (!_items.ContainsKey(id))
        {
            _items.Add(id, quantity);
        }
        else
        {
            _items[id] = _items[id] + quantity;
        }
        InventoryChanged?.Invoke();
        //Instantiate item gained panel
        if (GameController.gameState == GameController.GameStates.Running)
        {
            _itemGainedQueue.Add(new ItemBundle(id, quantity));
        }
    }

    public void Add(Item item, int quantity = 1)
    {
        Add(item.id, quantity);
    }

    public void Remove(int id, int quantity = 1)
    {
        if (id == Constants.MONEY_ID)
        {
            money = System.Math.Max(money - quantity, 0);
            return;
        }
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

    public void Remove(Item item, int quantity = 1)
    {
        Remove(item.id, quantity);
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
        return string.Join(";", lst) + $"|{money}";
    }


    public void SetInventoryFromString(string data)
    {
        _items.Clear();
        if (string.IsNullOrEmpty(data) || data == "|0") return;
        var groups = data.Split('|');
        var items = groups[0].Split(';');
        foreach (var item in items)
        {
            var x = item.Split(',');
            _items[int.Parse(x[0])] = int.Parse(x[1]);
        }
        if (groups.Length > 1)
        {
            money = Convert.ToInt32(groups[1]);
        }
    }

    public void GetItemsAndCounts(out List<ItemBundle> itemBundles)
    {
        itemBundles = new List<ItemBundle>();
        foreach (var entry in _items)
        {
            if (entry.Value == 0) continue;
            itemBundles.Add(new ItemBundle(entry.Key, entry.Value));
        }
    }

    public int GetCount(Item item)
    {
        return GetCount(item.id);
    }

    void Update()
    {
        if (_timeSinceLastItemGainedPanel <= 0 && GameController.gameState == GameController.GameStates.Running)
        {
            if (_itemGainedQueue.Count > 0)   
            {
                _timeSinceLastItemGainedPanel = timeBetweenItemGainedPanel;
                InstantiateItemGainedPanel(_itemGainedQueue[0]);
                _itemGainedQueue.RemoveAt(0);
            }
        }
        else
        {
            _timeSinceLastItemGainedPanel -= Time.deltaTime;
        }
    }

    private void InstantiateItemGainedPanel(ItemBundle itemBundle)
    {
        if (GameController.gameState != GameController.GameStates.Running) return;
        var gameUIGameObject = GameObject.FindGameObjectWithTag("GameUI");
        var itemGained = GameObject.Instantiate(ItemGainedPrefab, gameUIGameObject.transform);
        var itemGainedController = itemGained.GetComponent<ItemGainedController>();
        itemGainedController.itemBundle = itemBundle;
    }
}