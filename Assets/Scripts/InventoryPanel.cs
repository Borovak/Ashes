using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Interfaces;
using Player;
using Static;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : NavigablePanel
{
    public float placementBiasX;
    public float placementBiasY;
    public float placementMargin;
    public int placementXCount;
    public int placementYCount;
    public GameObject inventorySlotPrefab;
    
    private int _itemCount;
    private Vector2Int _index2D;

    protected override void OnEnableSpecific()
    {
        // if (_inventoryItemControllers == null)
        // {
        //     var inventoryItemControllers = new List<InventorySlotController>();
        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         var t = transform.GetChild(i);
        //         if (!t.TryGetComponent<InventorySlotController>(out var inventoryItemController)) continue;
        //         inventoryItemControllers.Add(inventoryItemController);
        //     }
        //     _inventoryItemControllers = inventoryItemControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ThenBy(x => x.transform.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        // }
        _index2D = Vector2Int.zero;
        PlayerInventory.InventoryChanged += RequestRefresh;
        SelectedIndexChanged += OnSelectedIndexChanged;
        refreshNeeded = true;
    }

    protected override void OnDisableSpecific()
    {
        PlayerInventory.InventoryChanged -= RequestRefresh;
        SelectedIndexChanged -= OnSelectedIndexChanged;
    }

    private void RequestRefresh()
    {
        refreshNeeded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        if (!GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var playerInventory)) return;
        playerInventory.GetItemsAndCounts(out var itemBundles);
        var index = 0;
        foreach (Transform t in transform)
        {
            if (!t.TryGetComponent<InventorySlotController>(out var inventoryItemController)) continue;
            inventoryItemController.Item = index < itemBundles.Count ? itemBundles[index].Item : null;
            inventoryItemController.Count = index < itemBundles.Count ? itemBundles[index].Quantity : -1;
            index++;
        }
        _itemCount = index;
    }

    protected override List<ItemSlot> GetItemSlots()
    {
        foreach (Transform t in transform)
        {
            if (!t.gameObject.name.Contains("InventorySlot")) continue;
            GameObject.Destroy(t.gameObject);
        }
        var index = 0;
        var slots = new List<ItemSlot>();
        for (int y = 0; y < placementYCount; y++)
        {
            for (int x = 0; x < placementXCount; x++)
            {
                var inventorySlot = GameObject.Instantiate(inventorySlotPrefab, transform);
                var rectTransform = inventorySlot.GetComponent<RectTransform>();
                var inventoryItemController = inventorySlot.GetComponent<InventorySlotController>();
                var ap = rectTransform.anchoredPosition;
                ap.x = placementBiasX + (placementMargin * x);
                ap.y = placementBiasY - (placementMargin * y);
                rectTransform.anchoredPosition = ap;
                inventoryItemController.index = index;
                slots.Add(inventoryItemController);
                index++;
            }
        }
        return slots;
    }

    protected override void OnSelectedItemChanged(Item item)
    {
        var slot = itemSlots.FirstOrDefault(x => x.Item == item);
        SelectedIndex = slot != null ? slot.index : -1;
    }

    protected override void OnSelectionChangeUp()
    {
        if (_index2D.y + 1 >= placementYCount)
        {
            TryExitUp();
            return;
        }
        var tempIndex = (_index2D.y + 1) * placementXCount + _index2D.x;
        if (tempIndex >= _itemCount || itemSlots[tempIndex].Item == null) return;
        SelectedIndex = tempIndex;
    }

    protected override void OnSelectionChangeDown()
    {
        if (_index2D.y - 1 < 0)
        {
            TryExitDown();
            return;
        }
        var tempIndex = (_index2D.y - 1) * placementXCount + _index2D.x;
        if (tempIndex >= _itemCount || itemSlots[tempIndex].Item == null) return;
        SelectedIndex = tempIndex;
    }

    protected override void OnSelectionChangeRight()
    {
        if (_index2D.x + 1 >= placementXCount)
        {
            TryExitRight();
            return;
        }
        var tempIndex = _index2D.y * placementXCount + _index2D.x + 1;
        if (tempIndex >= _itemCount || itemSlots[tempIndex].Item == null) return;
        SelectedIndex = tempIndex;
    }

    protected override void OnSelectionChangeLeft()
    {
        if (_index2D.x - 1 < 0)
        {
            TryExitLeft();
            return;
        }
        var tempIndex = _index2D.y * placementXCount + _index2D.x - 1;
        if (tempIndex >= _itemCount || itemSlots[tempIndex].Item == null) return;
        SelectedIndex = tempIndex;
    }

    private void OnSelectedIndexChanged(int index, Constants.PanelTypes panelType)
    {
        SetIndex2DFromIndex();
    }

    private void SetIndex2DFromIndex()
    {
        if (SelectedIndex == -1)
        {
            _index2D = Vector2Int.zero;
            return;
        }
        _index2D.x = SelectedIndex % placementXCount;
        _index2D.y = SelectedIndex / placementXCount;
    }
}
