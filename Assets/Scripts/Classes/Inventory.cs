using System.Collections.Generic;

public static class Inventory
{
    private static Dictionary<int, int> _items = new Dictionary<int, int>();

    public static void Add(int id)
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

    public static int GetCount(int id)
    {
        return _items.TryGetValue(id, out var count) ? count : 0;
    }

    public static string GetInventoryString()
    {
        var lst = new List<string>();
        foreach (var item in _items)
        {
            if (item.Value == 0) continue;
            lst.Add($"{item.Key},{item.Value}");
        }
        return string.Join(";", lst);
    }

    public static void SetInventoryString(string data)
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

    public static bool GetItemAtIndex(int index, out Item item, out int count)
    {
        item = null;
        count = -1;
        if (index >= _items.Count) return false;
        var i = 0;
        foreach (var k in _items)
        {
            if (index < i)
            {
                i++;
                continue;
            }            
            item = DropController.GetDropInfo(k.Key);
            count = k.Value;
            return true;
        }
        return false;
    }
}