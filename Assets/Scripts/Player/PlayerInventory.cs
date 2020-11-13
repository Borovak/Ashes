using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<int, int> _items = new Dictionary<int, int>();

    void Start()
    {
        SetInventoryFromString(SaveSystem.LastLoadedSave.Inventory);
    }

    public void Add(int id)
    {
        if (!_items.ContainsKey(id))
        {
            _items.Add(id, 1);
        }
        else
        {
            _items[id] = _items[id] + 1;
        }
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
}