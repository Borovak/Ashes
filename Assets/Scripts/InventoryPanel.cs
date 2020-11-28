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
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        var playerInventory = GlobalFunctions.GetPlayerInventory();
        playerInventory.GetItemsAndCounts(out var items, out var counts);
        var index = 0;
        foreach (var inventoryItemController in _inventoryItemControllers)
        {
            inventoryItemController.Item = index < items.Count ? items[index] : null;
            inventoryItemController.Count = index < counts.Count ? counts[index] : -1;
            index++;
        }
    }
}
