using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    public static bool isFocused;
    public bool refreshNeeded = true;
    public int selectedIndex = 0;
    public int offset = 0;

    private bool _previousIsFocused;

    private List<RecipeSlotController> _recipeSlotControllers;

    void OnEnable()
    {
        if (_recipeSlotControllers == null)
        {
            var recipeSlotControllers = new List<RecipeSlotController>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i);
                if (!t.TryGetComponent<RecipeSlotController>(out var recipeSlotController)) continue;
                recipeSlotControllers.Add(recipeSlotController);
                recipeSlotController.index = i;
            }
            _recipeSlotControllers = recipeSlotControllers.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ToList();
        }
        RecipeSlotController.SelectedRecipeChanged += OnSelectedRecipeChanged;
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.SelectionChangeRight += OnSelectionChangeRight;
        _previousIsFocused = false;
        isFocused = true;
        refreshNeeded = true;
    }

    void OnDisable()
    {
        RecipeSlotController.SelectedRecipeChanged -= OnSelectedRecipeChanged;
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.SelectionChangeRight -= OnSelectionChangeRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshNeeded)
        {
            refreshNeeded = false;
            var items = DropController.GetCraftables();
            //Debug.Log($"{items.Count} craftables found");
            for (int i = 0; i < _recipeSlotControllers.Count; i++)
            {
                _recipeSlotControllers[i].Item = i < items.Count ? items[i] : null;
            }
        }
        if (isFocused && !_previousIsFocused)
        {
            _recipeSlotControllers[0].Select();
            selectedIndex = 0;
        }
        _previousIsFocused = isFocused;
    }

    private void OnSelectedRecipeChanged(Item item)
    {
        if (!isFocused) return;
        var slot = _recipeSlotControllers.FirstOrDefault(x => x.Item == item);
        selectedIndex = slot != null ? slot.index : -1;
    }

    private void OnSelectionChangeUp()
    {
        if (!isFocused || selectedIndex - 1 < 0 || _recipeSlotControllers[selectedIndex - 1].Item == null) return;
        var slot = _recipeSlotControllers[selectedIndex - 1];
        slot.Select();
        selectedIndex = slot.index;
    }

    private void OnSelectionChangeDown()
    {
        if (!isFocused || selectedIndex + 1 >= _recipeSlotControllers.Count || _recipeSlotControllers[selectedIndex + 1].Item == null) return;
        var slot = _recipeSlotControllers[selectedIndex + 1];
        slot.Select();
        selectedIndex = slot.index;
    }

    private void OnSelectionChangeRight()
    {
        isFocused = false;
        InventoryPanel.isFocused = true;
    }
}
