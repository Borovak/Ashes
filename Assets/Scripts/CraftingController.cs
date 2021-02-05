using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour, IItemManager
{
    public Item selectedItem { get; set; }
    public event Action<Item> SelectedItemChanged;

    private Item _item;

    void OnEnable()
    {
        LongSlotController.SelectedSlotChanged += OnSelectedSlotChanged;
        LongSlotPanel.SelectedIndexChanged += OnSelectedIndexChanged;
    }

    void OnDisable()
    {
        LongSlotController.SelectedSlotChanged -= OnSelectedSlotChanged;
        LongSlotPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
    }
    
    private void OnSelectedSlotChanged(Item item)
    {
        _item = item;
        SelectedItemChanged?.Invoke(item);
    }    

    private void OnSelectedIndexChanged(int selectedIndex)
    {
        if (selectedIndex >= 0) return;
        _item = null;
        SelectedItemChanged?.Invoke(null);
    }
}
