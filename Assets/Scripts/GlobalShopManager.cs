using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Classes;
using Static;

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
            var path = connection.Table<DB.Npc>().FirstOrDefault(x => x.Id == shop.NpcId)?.Path ?? "";
            var shopController = new ShopController(shop.Id, shop.Name, path);
            _shopControllers.Add(shop.Id, shopController);
        }
        var shopItems = connection.Table<DB.ShopItem>();
        foreach (var shopController in _shopControllers.Values)
        {
            foreach (var shopItem in shopItems.Where(x => x.ShopId == shopController.Id))
            {
                shopController.ShopItemInfos.Add(shopItem.ItemId,
                    new ShopItemInfo
                    {
                        item = DropController.GetDropInfo(shopItem.ItemId), 
                        initial = shopItem.Initial, 
                        maximum = shopItem.Max
                    }
                );
            }
        }
        foreach (var shopController in _shopControllers.Values.Where(x => x.Inventory == null))
        {
            GlobalInventoryManager.AddShopInventory(shopController.Id, shopController.ShopItemInfos.Values.ToList());
        }
    }

    void Update()
    {
        var currentMinute = DateTime.Now.Minute;
        if (currentMinute == _lastUpdateOnMinute) return;
        _lastUpdateOnMinute = currentMinute;
        foreach (var shopController in _shopControllers.Values.Where(x => x.Id != currentShopId))
        {
            var itemBundlesToAdd = shopController.ShopItemInfos.Values.Join(
                shopController.Inventory.GetItemBundles(),
                x => x.item.id,
                y => y.Item.id,
                (x, y) => new ItemBundle(x.item, GetQuantityToAdd(y.Quantity, x.maximum))
                ).Where(x => x.Quantity > 0).ToList();
            foreach (var itemBundle in itemBundlesToAdd)
            {
                if (!GlobalInventoryManager.TryGetInventory(shopController.Id, out var inventory)) continue;
                inventory.Add(itemBundle);
            }
        }
    }

    private int GetQuantityToAdd(int quantity, int maximum)
    {
        var quantityToAdd = Convert.ToInt32(Math.Floor(Convert.ToSingle(maximum) * 0.1f));
        var newQuantity = quantity + quantityToAdd;
        return Math.Min(newQuantity, maximum) - quantity;
    }
    
    public static int GetItemQuantity(int shopId, int itemId)
    {
        return _instance._shopControllers.TryGetValue(shopId, out var shopController) ? shopController.Inventory.GetQuantity(itemId) : 0;
    }
    
    public static bool AddItemToShop(int shopId, int itemId, int quantity)
    {
        if (!_instance._shopControllers.TryGetValue(shopId, out var shopController)) return false;
        shopController.Inventory.Add(itemId, quantity);
        return true;
    }
    
    public static int RemoveItemFromShop(int shopId, int itemId, int quantity)
    {
        if (!_instance._shopControllers.TryGetValue(shopId, out var shopController)) return 0;
        return shopController.Inventory.Remove(itemId, quantity);
    }

    public static Sprite GetCurrentShopImage()
    {
        if (!_instance._shopControllers.TryGetValue(currentShopId, out var shopController)) return null;
        var sprite = Resources.Load<Sprite>($"Npc/{shopController.Path}");
        return sprite;
    }
}
