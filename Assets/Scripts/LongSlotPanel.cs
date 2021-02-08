using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Interfaces;
using Player;
using Static;
using UnityEngine;

public class LongSlotPanel : NavigablePanel
{
    public int offset = 0;
    public Constants.PanelTypes panelType;
    public int slotCount;
    public GameObject slotPrefab;
    public int shopIndex;

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
        ShopController.ShopModeChanged += OnShopModeChanged;
        
    }

    protected override void OnDisableSpecific()
    {
        ShopController.ShopModeChanged -= OnShopModeChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        var items = GetItems(out var quantities);
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].Item = i < items.Count ? items[i] : null;
            if (quantities != null && itemSlots[i].Item != null)
            {
                itemSlots[i].Count = quantities[i];
            }
        }
        var tempIndex = System.Math.Max(SelectedIndex, 0);
        while (tempIndex >= 0 && itemSlots[tempIndex].Item == null)
        {
            tempIndex--;
        }
        SelectedIndex = tempIndex;
    }

    private List<Item> GetItems(out List<int> quantities)
    {
        quantities = null;
        switch (panelType)
        {
            case Constants.PanelTypes.Craftables: return DropController.GetCraftables();
            case Constants.PanelTypes.ShopSell:
                if (!GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var playerInventory)) return new List<Item>();
                playerInventory.GetItemsAndCounts(out var itemBundles);
                quantities = itemBundles.Select(x => x.Quantity).ToList();
                return itemBundles.Select(x => x.Item).ToList();
        }
        return new List<Item>();

    }

    private void OnSlotClicked(int index)
    {
        if (!HasFocus) return;
        SelectedIndex = index;
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

    private void OnShopModeChanged(ShopController.ShopModes shopMode)
    {
        panelType = shopMode == ShopController.ShopModes.Buy ? Constants.PanelTypes.ShopBuy : Constants.PanelTypes.ShopSell;
        SelectedIndex = -1;
        refreshNeeded = true;
    }
}
