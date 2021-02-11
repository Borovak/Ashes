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
        if (!DataHandling.TryConnectToDb(out var connection)) return;
        var shops = connection.Table<DB.Shop>();
        foreach(var shop in shops)
        {
            var shopController = new ShopController(shop.Id, shop.Name);
            _shopControllers.Add(shop.Id, shopController);
        }
        var shopItems = connection.Table<DB.ShopItem>();
        foreach (var shopController in _shopControllers.Values)
        {
            foreach (var shopItem in shopItems.Where(x => x.ShopId == shopController.Id))
            {
                var quantity = UnityEngine.Random.Range(shopItem.Min, shopItem.Max);
                shopController.ShopItemInfos.Add(shopItem.ItemId,
                    new ShopItemInfo
                    {
                        item = DropController.GetDropInfo(shopItem.ItemId), 
                        quantity = quantity, 
                        minimum = shopItem.Min, 
                        maximum = shopItem.Max
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
