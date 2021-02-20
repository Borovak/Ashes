using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Interfaces;
using UnityEngine;

public class InventoryController : MonoBehaviour, IItemManager
    {
    public GameObject panelObject;
    public Item selectedItem { get; set; }
    public event Action<Item, Constants.PanelTypes> SelectedItemChanged;

    private INavigablePanel _itemPanel;

    void OnEnable()
    {
        _itemPanel ??= panelObject.GetComponent<INavigablePanel>();
        _itemPanel.SelectedIndexChanged += OnSelectedIndexChanged;
        _itemPanel.SelectedItemChanged += OnSelectedItemChanged;
        _itemPanel.FocusRequested += SelectPanel;
        _itemPanel.HasFocus = true;
    }

    void OnDisable()
    {
        _itemPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
        _itemPanel.SelectedItemChanged -= OnSelectedItemChanged;
        _itemPanel.FocusRequested -= SelectPanel;
    } 

    private void OnSelectedIndexChanged(int selectedIndex, Constants.PanelTypes panelType)
    {
        if (selectedIndex >= 0) return;
        selectedItem = null;
        SelectedItemChanged?.Invoke(null, panelType);
    }

    private void OnSelectedItemChanged(Item item, Constants.PanelTypes panelType)
    {
        SelectedItemChanged?.Invoke(item, panelType);
    }

    private void SelectPanel(INavigablePanel panel)
    {
        _itemPanel.HasFocus = panel == _itemPanel;
    } }