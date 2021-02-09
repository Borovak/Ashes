using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Classes;
using Static;
using UI;

public class GlobalShopManager : MonoBehaviour
{
    public static int currentShopId = -1;
    
    private static GlobalShopManager _instance;
    private Dictionary<int, ShopController> _shopControllers;
    private int _lastUpdateOnMinute = -1;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        _shopControllers = new Dictionary<int, ShopController>();
        if (!Static.DataHandling.GetTable("shops", out var dtShops))
        {
            Debug.Log("Couldn't get data from table 'shops'");
            return;
        }

        foreach (DataRow dr in dtShops.Rows)
        {
            var shopId = Convert.ToInt32(dr["id"]);
            var shopName = dr["id"].ToString();
            var shopController = new ShopController(shopId, shopName);
            _shopControllers.Add(shopId, shopController);
        }
        if (!Static.DataHandling.GetTable("shop_items", out var dtShopItems))
        {
            Debug.Log("Couldn't get data from table 'shop_items'");
            return;
        }
        foreach (var shopController in _shopControllers.Values)
        {
            foreach (DataRow dr in dtShopItems.Rows)
            {
                var shopId = Convert.ToInt32(dr["shops_id"]);
                if (shopId != shopController.Id) continue;
                var itemId = Convert.ToInt32(dr["items_id"]);
                var minimum = Convert.ToInt32(dr["min"]);
                var maximum = Convert.ToInt32(dr["max"]);
                var quantity = UnityEngine.Random.Range(minimum, maximum);
                shopController.ShopItemInfos.Add(itemId,
                    new ShopItemInfo
                    {
                        item = DropController.GetDropInfo(itemId), 
                        quantity = quantity, 
                        minimum = minimum, 
                        maximum = maximum
                    }
                );
            }
        }
    }

    void Update()
    {
        var currentMinute = DateTime.Now.Minute;
        if (currentMinute == _lastUpdateOnMinute) return;
        _lastUpdateOnMinute = currentMinute;
        foreach (var shopController in _shopControllers.Values)
        {
            if (currentShopId == shopController.Id) continue;
            foreach (var shopItemInfo in shopController.ShopItemInfos.Values.Where(x => x.quantity < x.maximum))
            {
                var quantityToAdd = Convert.ToInt32(Convert.ToSingle(shopItemInfo.maximum - shopItemInfo.minimum) * 0.1f);
                var newQuantity = shopItemInfo.quantity + quantityToAdd;
                shopItemInfo.quantity = System.Math.Min(newQuantity, shopItemInfo.maximum);
            }
        }
    }

    public static bool TryGetShopInfo(int shopId, out List<ShopItemInfo> shopItemInfos)
    {
        if (!_instance._shopControllers.TryGetValue(shopId, out var shopController))
        {
            shopItemInfos = null;
            return false;
        }
        shopItemInfos = shopController.ShopItemInfos.Values.ToList();
        return true;
    }
    
    public static int GetItemQuantity(int shopId, int itemId)
    {
        if (!_instance._shopControllers.TryGetValue(shopId, out var shopController)) return 0;
        if (!shopController.ShopItemInfos.TryGetValue(itemId, out var shopItemInfo)) return 0;
        return shopItemInfo.quantity;
    }
    
    public static int AddItemToShop(int shopId, int itemId, int quantity)
    {
        if (!_instance._shopControllers.TryGetValue(shopId, out var shopController)) return 0;
        if (!shopController.ShopItemInfos.TryGetValue(itemId, out var shopItemInfo)) return 0;
        shopItemInfo.quantity += quantity;
        return quantity;
    }
    
    public static int RemoveItemFromShop(int shopId, int itemId, int quantity)
    {
        if (!_instance._shopControllers.TryGetValue(shopId, out var shopController)) return 0;
        if (!shopController.ShopItemInfos.TryGetValue(itemId, out var shopItemInfo)) return 0;
        if (quantity > shopItemInfo.quantity)
        {
            var quantityRemoved = shopItemInfo.quantity;
            shopItemInfo.quantity = 0;
            return quantityRemoved;
        }
        shopItemInfo.quantity -= quantity;
        return quantity;
    }
}
