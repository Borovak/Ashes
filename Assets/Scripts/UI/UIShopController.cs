using System;
using Classes;
using Interfaces;
using Player;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIShopController : MonoBehaviour, IItemManager
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
        public static UIShopController instance;

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
        public Image shopPortraitImageControl;
        public TextMeshProUGUI playerWalletValueControl;
        public TextMeshProUGUI shopWalletValueControl;
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
            _itemListPanel.SelectedIndexChanged += OnSelectedIndexChanged;
            _itemListPanel.SelectedItemChanged += OnSelectedItemChanged;
            _itemListPanel.HasFocus = true;
            GlobalInventoryManager.RegisterToInventoryChange(-1, OnInventoryChanged);
            if (GlobalShopManager.currentShopId != -1)
            {
                GlobalInventoryManager.RegisterToInventoryChange(GlobalShopManager.currentShopId, OnInventoryChanged);
                shopPortraitImageControl.sprite = GlobalShopManager.GetCurrentShopImage();
            }
            OnInventoryChanged();
        }

        void OnDisable()
        {
            MenuInputs.SectionPrevious -= OnSectionPrevious;
            MenuInputs.SectionNext -= OnSectionNext;
            MenuInputs.Back -= OnBack;
            _itemListPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
            _itemListPanel.SelectedItemChanged -= OnSelectedItemChanged;
            GlobalInventoryManager.UnregisterToInventoryChange(-1, OnInventoryChanged);
            if (GlobalShopManager.currentShopId != -1)
            {
                GlobalInventoryManager.UnregisterToInventoryChange(GlobalShopManager.currentShopId, OnInventoryChanged);
            }
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
            shopMode = newShopMode;
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

        public void BuyOne()
        {
            if (_item == null) return;
            GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var inventory);
            Buy(_item, 1);
        }

        public void BuyMax()
        {
            if (_item == null) return;
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            var quantityInInventory = GlobalShopManager.GetItemQuantity(GlobalShopManager.currentShopId, _item.id);
            var playerMoney = inventory.GetMoneyQuantity();
            var quantityThatPlayerCanAfford = Convert.ToInt32(Math.Floor(Convert.ToSingle(playerMoney) / Convert.ToSingle(_item.value)));
            var quantity = System.Math.Min(quantityInInventory, quantityThatPlayerCanAfford);
            Buy(_item, quantity);
        }

        private void Buy(Item item, int quantity)
        {
            var cost = item.value * quantity;
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            if (inventory.GetMoneyQuantity() < cost) return; //Check if player can afford
            if (GlobalShopManager.GetItemQuantity(GlobalShopManager.currentShopId, item.id) < quantity) return; //Check if shop has enough items
            GlobalShopManager.RemoveItemFromShop(GlobalShopManager.currentShopId, item.id, quantity);
            GlobalShopManager.AddItemToShop(GlobalShopManager.currentShopId, Constants.MONEY_ID, cost);
            inventory.Remove(Constants.MONEY_ID, cost);
            inventory.Add(item, quantity);
        }

        public void SellOne()
        {
            if (_item == null) return;
            Sell(_item, 1);
        }

        public void SellMax()
        {
            if (_item == null) return;
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            var quantityInInventory = inventory.GetQuantity(_item);
            var shopMoney = GlobalShopManager.GetItemQuantity(GlobalShopManager.currentShopId, Constants.MONEY_ID);
            var quantityThatShopCanAfford = Convert.ToInt32(Math.Floor(Convert.ToSingle(shopMoney) / Convert.ToSingle(_item.value)));
            var quantity = System.Math.Min(quantityInInventory, quantityThatShopCanAfford);
            Sell(_item, quantity);
        }

        private void Sell(Item item, int quantity)
        {
            var cost = item.value * quantity;
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            if (GlobalShopManager.GetItemQuantity(GlobalShopManager.currentShopId, Constants.MONEY_ID) < cost) return; //Check if shop can afford
            if (inventory.GetQuantity(item.id) < quantity) return; //Check if player has enough items
            GlobalShopManager.AddItemToShop(GlobalShopManager.currentShopId, item.id, quantity);
            GlobalShopManager.RemoveItemFromShop(GlobalShopManager.currentShopId, Constants.MONEY_ID, cost);
            inventory.Remove(_item, quantity);
            inventory.Add(Constants.MONEY_ID, cost);
        }

        private void OnInventoryChanged()
        {
            if (!GlobalInventoryManager.TryGetInventory(-1, out var playerInventory)) return;
            if (!GlobalInventoryManager.TryGetInventory(GlobalShopManager.currentShopId, out var shopInventory)) return;
            playerWalletValueControl.text = playerInventory.GetMoneyQuantity().ToString();
            shopWalletValueControl.text = shopInventory.GetMoneyQuantity().ToString();
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
}
