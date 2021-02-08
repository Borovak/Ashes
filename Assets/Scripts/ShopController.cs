using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Interfaces;
using Player;
using Static;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour, IItemManager
{

    [Serializable]
    public enum ShopModes
    {
        Buy,
        Sell
    }

    public static event Action<ShopModes> ShopModeChanged;
    public static event Action OpenShopRequired;
    public static event Action CloseShopRequired;
    public event Action<Item, Constants.PanelTypes> SelectedItemChanged;
    public static ShopController instance;

    public static ShopModes shopMode
    {
        get => _shopMode;
        set
        {
            _shopMode = value;
            instance._menuGroup.ActiveButton = instance.modeButtons[(int)shopMode];
            ShopModeChanged?.Invoke(value);
        }
    }

    public MenuButton[] modeButtons;
    public GameObject[] buyButtons;
    public GameObject[] sellButtons;
    public GameObject itemListPanelObject;
    public TextMeshProUGUI walletValueControl;
    public Item selectedItem { get; set; }

    private MenuGroup _menuGroup;
    private static ShopModes _shopMode;
    private Item _item;
    private INavigablePanel _itemListPanel;

    void OnEnable()
    {
        if (_menuGroup == null)
        {
            _menuGroup = GetComponent<MenuGroup>();
            _itemListPanel = itemListPanelObject.GetComponent<INavigablePanel>();
        }
        instance = this;
        shopMode = ShopModes.Buy;
        MenuInputs.SectionPrevious += OnSectionPrevious;
        MenuInputs.SectionNext += OnSectionNext;
        MenuInputs.Back += OnBack;
        PlayerInventory.InventoryChanged += OnInventoryChanged;
        _itemListPanel.SelectedIndexChanged += OnSelectedIndexChanged;
        _itemListPanel.SelectedItemChanged += OnSelectedItemChanged;
        _itemListPanel.HasFocus = true;
        OnInventoryChanged();
    }

    void OnDisable()
    {
        MenuInputs.SectionPrevious -= OnSectionPrevious;
        MenuInputs.SectionNext -= OnSectionNext;
        MenuInputs.Back -= OnBack;
        PlayerInventory.InventoryChanged -= OnInventoryChanged;
        _itemListPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
        _itemListPanel.SelectedItemChanged -= OnSelectedItemChanged;
    }

    private void OnSectionPrevious()
    {
        shopMode = shopMode == ShopModes.Buy ? ShopModes.Sell : ShopModes.Buy;
    }

    private void OnSectionNext()
    {
        shopMode = shopMode == ShopModes.Buy ? ShopModes.Sell : ShopModes.Buy;
    }

    private void OnBack()
    {
        CloseShopRequired?.Invoke();
    }

    public void ChangeShopMode(ShopModes newShopMode)
    {
        ShopController.shopMode = newShopMode;
    }

    public void ChangeShopMode(int newShopMode)
    {
        ChangeShopMode((ShopModes)newShopMode);
    }

    public static void Open()
    {
        OpenShopRequired?.Invoke();
    }

    private void OnSelectedItemChanged(Item item, Constants.PanelTypes panelType)
    {
        _item = item;
        SelectedItemChanged?.Invoke(item, panelType);
        UpdateBuySellButtons(item != null);
    }

    public void Buy()
    {

    }

    public void BuyMax()
    {

    }

    public void Sell()
    {
        if (_item == null) return;
        GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var inventory);
        inventory.Remove(_item, 1);
        inventory.Add(Constants.MONEY_ID, _item.value);
    }

    public void SellMax()
    {
        if (_item == null) return;
        GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var inventory);
        var quantityTotal = inventory.GetCount(_item);
        var quantityRemoved = inventory.Remove(_item, quantityTotal);
        inventory.Add(Constants.MONEY_ID, _item.value * quantityRemoved);
    }

    private void OnInventoryChanged()
    {
        if (!GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var inventory)) return;
        walletValueControl.text = inventory.GetCount(Constants.MONEY_ID).ToString();
    }

    private void OnSelectedIndexChanged(int selectedIndex, Constants.PanelTypes panelType)
    {
        if (selectedIndex >= 0) return;
        _item = null;
        UpdateBuySellButtons(false);
        SelectedItemChanged?.Invoke(null, Constants.PanelTypes.ShopBuy);
    }

    private void UpdateBuySellButtons(bool canBeVisible)
    {
        buyButtons[0].transform.parent.gameObject.SetActive(canBeVisible);
        foreach (var button in buyButtons)
        {
            button.SetActive(canBeVisible && shopMode == ShopModes.Buy);
        }
        foreach (var button in sellButtons)
        {
            button.SetActive(canBeVisible && shopMode == ShopModes.Sell);
        }
    }
}
