using System.Collections.Generic;
using Classes;

public class ShopController
{
    internal int Id;
    internal string Name;
    internal string Path;
    internal Dictionary<int, ShopItemInfo> ShopItemInfos;
    internal Inventory Inventory => GlobalInventoryManager.TryGetInventory(Id, out var inventory) ? inventory : null;

    internal ShopController(int id, string name, string path)
    {
        Id = id;
        Name = name;
        Path = path;
        ShopItemInfos = new Dictionary<int, ShopItemInfo>();
    }
    
}


