using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;

public class ShopController
{
    internal int Id;
    internal string Name;
    internal Dictionary<int, ShopItemInfo> ShopItemInfos;

    internal ShopController(int id, string name)
    {
        Id = id;
        Name = name;
        ShopItemInfos = new Dictionary<int, ShopItemInfo>();
    }
}


