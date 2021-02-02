using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LongSlotPanel : MonoBehaviour
{
    public static event Action<int> SelectedIndexChanged;
    public static event Action SelectionExitOnLeft;
    public static event Action SelectionExitOnRight;
    public static bool isFocused;
    public bool canExitOnLeft;
    public bool canExitOnRight;
    public bool refreshNeeded = true;
    public int selectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            SelectedIndexChanged?.Invoke(_selectedIndex);
        }
    }
    public int offset = 0;
    public Constants.PanelTypes panelType;
    public int slotCount;
    public GameObject slotPrefab;
    public int shopIndex;

    private bool _previousIsFocused;
    private List<LongSlotController> _slotControllers;
    private int _selectedIndex;

    void OnEnable()
    {
        if (_slotControllers == null)
        {
            var recipeSlotControllers = new List<LongSlotController>();
            for (int i = 0; i < slotCount; i++)
            {
                var slotGameObject = GameObject.Instantiate(slotPrefab, transform);
                var slotRectTransform = slotGameObject.GetComponent<RectTransform>();
                slotRectTransform.anchoredPosition = new Vector2(0, i * -100f - 150f);
                if (!slotGameObject.TryGetComponent<LongSlotController>(out var recipeSlotController)) continue;
                recipeSlotController.panelType = panelType;
                recipeSlotControllers.Add(recipeSlotController);
                recipeSlotController.index = i;
                recipeSlotController.panel = this;
            }
            _slotControllers = recipeSlotControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ToList();
        }
        ShopController.ShopModeChanged += OnShopModeChanged;
        LongSlotController.SlotClicked += OnSlotClicked;
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.SelectionChangeRight += OnSelectionChangeRight;
        if (panelType != Constants.PanelTypes.Craftables)
        {
            PlayerInventory.InventoryChanged += RequestRefresh;
        }
        _previousIsFocused = false;
        isFocused = true;
        refreshNeeded = true;
    }

    void OnDisable()
    {
        ShopController.ShopModeChanged -= OnShopModeChanged;
        LongSlotController.SlotClicked -= OnSlotClicked;
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.SelectionChangeRight -= OnSelectionChangeRight;
        if (panelType != Constants.PanelTypes.Craftables)
        {
            PlayerInventory.InventoryChanged += RequestRefresh;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshNeeded)
        {
            refreshNeeded = false;
            var items = GetItems(out var quantities);
            for (int i = 0; i < _slotControllers.Count; i++)
            {
                _slotControllers[i].Item = i < items.Count ? items[i] : null;
                if (quantities != null && _slotControllers[i].Item != null)
                {
                    _slotControllers[i].Count = quantities[i];
                }
            }
            var tempIndex = System.Math.Max(selectedIndex, 0);
            while (tempIndex >= 0 && _slotControllers[tempIndex].Item == null)
            {
                tempIndex--;
            }
            selectedIndex = tempIndex;
        }
        if (isFocused && !_previousIsFocused)
        {
            selectedIndex = 0;
        }
        _previousIsFocused = isFocused;
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
        if (!isFocused) return;
        selectedIndex = index;
    }

    private void OnSelectionChangeUp()
    {
        if (!isFocused || selectedIndex - 1 < 0 || _slotControllers[selectedIndex - 1].Item == null) return;
        var slot = _slotControllers[selectedIndex - 1];
        selectedIndex = slot.index;
    }

    private void OnSelectionChangeDown()
    {
        if (!isFocused || selectedIndex + 1 >= _slotControllers.Count || _slotControllers[selectedIndex + 1].Item == null) return;
        var slot = _slotControllers[selectedIndex + 1];
        selectedIndex = slot.index;
    }

    private void OnSelectionChangeLeft()
    {
        if (!canExitOnLeft) return;
        isFocused = false;
        SelectionExitOnLeft?.Invoke();
    }

    private void OnSelectionChangeRight()
    {
        if (!canExitOnRight) return;
        selectedIndex = -1;
        isFocused = false;
        SelectionExitOnRight?.Invoke();
    }

    private void OnShopModeChanged(ShopController.ShopModes shopMode)
    {
        panelType = shopMode == ShopController.ShopModes.Buy ? Constants.PanelTypes.ShopBuy : Constants.PanelTypes.ShopSell;
        selectedIndex = -1;
        RequestRefresh();
    }

    public void RequestRefresh()
    {
        refreshNeeded = true;
    }
}
