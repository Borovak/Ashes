using System.Collections.Generic;
using System.Linq;
using Classes;
using Static;
using UI;
using UnityEngine;

public class LongSlotPanel : NavigablePanel
{
    public int offset = 0;
    public Constants.PanelTypes panelType;
    public int slotCount;
    public GameObject slotPrefab;

    protected override List<ItemSlot> GetItemSlots()
    {
        var recipeSlotControllers = new List<ItemSlot>();
        for (int i = 0; i < slotCount; i++)
        {
            var slotGameObject = GameObject.Instantiate(slotPrefab, transform);
            var slotRectTransform = slotGameObject.GetComponent<RectTransform>();
            slotRectTransform.anchoredPosition = new Vector2(0, i * -100f - 150f);
            if (!slotGameObject.TryGetComponent<LongSlotController>(out var recipeSlotController)) continue;
            recipeSlotControllers.Add(recipeSlotController);
            recipeSlotController.index = i;
        }
        return recipeSlotControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ToList();
    }
    
    protected override void OnEnableSpecific()
    {
        UIShopController.ShopModeChanged += OnShopModeChanged;
        refreshNeeded = true;

    }

    protected override void OnDisableSpecific()
    {
        UIShopController.ShopModeChanged -= OnShopModeChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        var itemBundles = GetItemBundles();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].Item = i < itemBundles.Count ? itemBundles[i].Item : null;
            if (itemSlots[i].Item != null)
            {
                itemSlots[i].Count = itemBundles[i].Quantity;
            }
        }
        var tempIndex = System.Math.Max(SelectedIndex, 0);
        while (tempIndex >= 0 && itemSlots[tempIndex].Item == null)
        {
            tempIndex--;
        }
        SelectedIndex = tempIndex;
    }

    private List<ItemBundle> GetItemBundles()
    {
        switch (panelType)
        {
            case Constants.PanelTypes.Craftables: return DropController.GetCraftables();
            case Constants.PanelTypes.ShopBuy:
                if (!GlobalInventoryManager.TryGetInventory(GlobalShopManager.currentShopId, out var shopInventory)) return new List<ItemBundle>();
                return shopInventory.GetItemBundles(false);
            case Constants.PanelTypes.ShopSell:
                if (!GlobalInventoryManager.TryGetInventory(-1, out var playerInventory)) return new List<ItemBundle>();
                return playerInventory.GetItemBundles(false);
        }
        return new List<ItemBundle>();

    }

    protected override void OnSelectionChangeUp()
    {
        if (SelectedIndex - 1 < 0 || itemSlots[SelectedIndex - 1].Item == null)
        {
            TryExitUp();
            return;
        }
        var slot = itemSlots[SelectedIndex - 1];
        SelectedIndex = slot.index;
    }

    protected override void OnSelectionChangeDown()
    {
        if (SelectedIndex + 1 >= itemSlots.Count || itemSlots[SelectedIndex + 1].Item == null) 
        {
            TryExitDown();
            return;
        }
        var slot = itemSlots[SelectedIndex + 1];
        SelectedIndex = slot.index;
    }

    protected override void OnSelectionChangeLeft()
    {
        TryExitLeft();
    }

    protected override void OnSelectionChangeRight()
    {
        TryExitRight();
    }

    protected override void OnSelectedItemChanged(Item item)
    {
        var slot = itemSlots.FirstOrDefault(x => x.Item == item);
        SelectedIndex = slot != null ? slot.index : -1;
    }

    private void OnShopModeChanged(UIShopController.ShopModes shopMode)
    {
        panelType = shopMode == UIShopController.ShopModes.Buy ? Constants.PanelTypes.ShopBuy : Constants.PanelTypes.ShopSell;
        SelectedIndex = -1;
        refreshNeeded = true;
    }
}
