using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{

    public bool refreshNeeded = true;

    private List<InventoryItemController> _inventoryItemControllers;

    // Start is called before the first frame update
    void Start()
    {
        var inventoryItemControllers = new List<InventoryItemController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if (!t.TryGetComponent<InventoryItemController>(out var inventoryItemController)) continue;
            inventoryItemControllers.Add(inventoryItemController);
        }
        _inventoryItemControllers = inventoryItemControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ThenBy(x => x.transform.GetComponent<RectTransform>().anchoredPosition.x).ToList();
    }

    void OnEnable(){
        refreshNeeded = true;
        PlayerInventory.InventoryChanged += RequestRefresh;
    }

    void OnDisable(){
        PlayerInventory.InventoryChanged -= RequestRefresh;
    }

    private void RequestRefresh(){
        refreshNeeded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        if (!TryGetComponent<PlayerInventory>(out var playerInventory)) return;
        playerInventory.GetItemsAndCounts(out var itemBundles);
        var index = 0;
        foreach (var inventoryItemController in _inventoryItemControllers)
        {
            inventoryItemController.Item = index < itemBundles.Count ? itemBundles[index].Item : null;
            inventoryItemController.Count = index < itemBundles.Count ? itemBundles[index].Quantity : -1;
            index++;
        }
    }
}
