using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public static event Action<int> SelectedIndexChanged;
    public static bool isFocused;
    public float placementBiasX;
    public float placementBiasY;
    public float placementMargin;
    public int placementXCount;
    public int placementYCount;
    public GameObject inventorySlotPrefab;
    public bool refreshNeeded = true;
    public int selectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            SelectedIndexChanged.Invoke(value);
        }
    }

    private bool _previousIsFocused;
    private List<InventoryItemController> _inventoryItemControllers;
    private int _selectedIndex;
    private int _itemCount;
    private int _x;
    private int _y;

    void OnEnable()
    {
        if (_inventoryItemControllers == null)
        {
            Place();
            var inventoryItemControllers = new List<InventoryItemController>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i);
                if (!t.TryGetComponent<InventoryItemController>(out var inventoryItemController)) continue;
                inventoryItemControllers.Add(inventoryItemController);
            }
            _inventoryItemControllers = inventoryItemControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ThenBy(x => x.transform.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        }
        PlayerInventory.InventoryChanged += RequestRefresh;
        LongSlotController.SelectedSlotChanged += OnSelectedRecipeChanged;
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.SelectionChangeLeft += OnSelectionChangeLeft;
        MenuInputs.SelectionChangeRight += OnSelectionChangeRight;
        refreshNeeded = true;
    }

    void OnDisable()
    {
        PlayerInventory.InventoryChanged -= RequestRefresh;
        LongSlotController.SelectedSlotChanged -= OnSelectedRecipeChanged;
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.SelectionChangeLeft -= OnSelectionChangeLeft;
        MenuInputs.SelectionChangeRight -= OnSelectionChangeRight;
    }

    private void RequestRefresh()
    {
        refreshNeeded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshNeeded)
        {
            refreshNeeded = false;
            if (!GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var playerInventory)) return;
            playerInventory.GetItemsAndCounts(out var itemBundles);
            var index = 0;
            _inventoryItemControllers.Clear();
            foreach (Transform t in transform)
            {
                var inventoryItemController = t.GetComponent<InventoryItemController>();
                _inventoryItemControllers.Add(inventoryItemController);
                inventoryItemController.Item = index < itemBundles.Count ? itemBundles[index].Item : null;
                inventoryItemController.Count = index < itemBundles.Count ? itemBundles[index].Quantity : -1;
                index++;
            }
            _itemCount = index;
        }
        if (isFocused && !_previousIsFocused)
        {
            _inventoryItemControllers[0].Select();
            selectedIndex = 0;
            _x = 0;
            _y = 0;
        }
        _previousIsFocused = isFocused;
    }

    private void Place()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
        var index = 0;
        for (int y = 0; y < placementYCount; y++)
        {
            for (int x = 0; x < placementXCount; x++)
            {
                var inventorySlot = GameObject.Instantiate(inventorySlotPrefab, transform);
                var rectTransform = inventorySlot.GetComponent<RectTransform>();
                var inventoryItemController = inventorySlot.GetComponent<InventoryItemController>();
                var ap = rectTransform.anchoredPosition;
                ap.x = placementBiasX + (placementMargin * x);
                ap.y = placementBiasY - (placementMargin * y);
                rectTransform.anchoredPosition = ap;
                inventoryItemController.index = index;
                index++;
            }
        }
    }

    private void OnSelectedRecipeChanged(Item item)
    {
        if (!isFocused) return;
        var slot = _inventoryItemControllers.FirstOrDefault(x => x.Item == item);
        selectedIndex = slot != null ? slot.index : -1;
    }

    private void OnSelectionChangeUp()
    {
        if (!isFocused || _y + 1 >= placementYCount) return;
        var tempIndex = (_y + 1) * placementXCount + _x;
        if (tempIndex >= _itemCount || _inventoryItemControllers[tempIndex].Item == null) return;
        _y += 1;
        selectedIndex = tempIndex;
        _inventoryItemControllers[tempIndex].Select();
    }

    private void OnSelectionChangeDown()
    {
        if (!isFocused || _y - 1 < 0) return;
        var tempIndex = (_y - 1) * placementXCount + _x;
        if (tempIndex >= _itemCount || _inventoryItemControllers[tempIndex].Item == null) return;
        _y -= 1;
        selectedIndex = tempIndex;
        _inventoryItemControllers[tempIndex].Select();
    }

    private void OnSelectionChangeRight()
    {
        if (!isFocused || _x + 1 >= placementXCount) return;
        var tempIndex = _y * placementXCount + _x + 1;
        if (tempIndex >= _itemCount || _inventoryItemControllers[tempIndex].Item == null) return;
        _x += 1;
        selectedIndex = tempIndex;
        _inventoryItemControllers[tempIndex].Select();
    }

    private void OnSelectionChangeLeft()
    {
        if (_x - 1 < 0)
        {
            isFocused = false;
            LongSlotPanel.isFocused = true;
            return;
        }
        if (!isFocused) return;
        var tempIndex = _y * placementXCount + _x - 1;
        if (tempIndex >= _itemCount || _inventoryItemControllers[tempIndex].Item == null) return;
        _x -= 1;
        selectedIndex = tempIndex;
        _inventoryItemControllers[tempIndex].Select();
    }
}
