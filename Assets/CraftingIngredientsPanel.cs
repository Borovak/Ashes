using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Classes;
using Interfaces;
using Player;
using Static;
using TMPro;
using UnityEngine;

public class CraftingIngredientsPanel : MonoBehaviour
{
    public Constants.PanelTypes panelType;
    public GameObject itemManagerObject;
    public GameObject craftButton;
    public GameObject craftMaxButton;

    private IItemManager _itemManager;
    private List<RecipeIngredient> _recipeIngredients;
    private Item _item;
    private List<KeyValuePair<Item, int>> _currentIngredients;

    void OnEnable()
    {
        if (_itemManager == null)
        {
            var recipeIngredients = new List<RecipeIngredient>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i);
                if (!t.TryGetComponent<RecipeIngredient>(out var recipeIngredient)) continue;
                recipeIngredients.Add(recipeIngredient);
            }
            _recipeIngredients = recipeIngredients.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ThenBy(x => x.transform.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        }
        PlayerInventory.InventoryChanged += SetCraftButtonsVisibility;
        _itemManager = itemManagerObject.GetComponent<IItemManager>();
        _itemManager.SelectedItemChanged += OnSelectedItemChanged;
        MenuInputs.OK += Craft;
        MenuInputs.Special += CraftMax;
    }

    void OnDisable()
    {
        PlayerInventory.InventoryChanged -= SetCraftButtonsVisibility;
        _itemManager.SelectedItemChanged -= OnSelectedItemChanged;
        MenuInputs.OK -= Craft;
        MenuInputs.Special -= CraftMax;
        _item = null;
    }

    private void OnSelectedItemChanged(Item item, Constants.PanelTypes panelType)
    {
        _item = item;
        if (item == null || !item.isCraftable || panelType != Constants.PanelTypes.Craftables)
        {
            ClearIngredients();
            return;
        }
        CheckForRecipe(item);
    }

    private void ClearIngredients()
    {
        foreach (var recipeIngredient in _recipeIngredients)
        {
            recipeIngredient.Item = null;
        }
        _currentIngredients = null;
        ChangeCraftButtonsVisibility(false, 0);
    }

    private void CheckForRecipe(Item item)
    {
        const string ingredientColumn = "ingredient";
        const string quantityColumn = "quantity";
        if (!DataHandling.GetInfo($"SELECT {ingredientColumn}, {quantityColumn} FROM recipes WHERE items_id = '{item.id}'", out var dtRecipe))
        {
            ClearIngredients();
            return;
        }
        var ingredients = new Dictionary<Item, int>();
        foreach (DataRow dr in dtRecipe.Rows)
        {
            var id = int.TryParse(dr[ingredientColumn].ToString(), out var tempId) ? tempId : 0;
            var quantity = int.TryParse(dr[quantityColumn].ToString(), out var tempQuantity) ? tempQuantity : 0;
            ingredients.Add(DropController.GetDropInfo(id), quantity);
        }
        _currentIngredients = ingredients.OrderBy(x => x.Key.id).ToList();
        for (int i = 0; i < _recipeIngredients.Count; i++)
        {
            if (i < _currentIngredients.Count)
            {
                _recipeIngredients[i].Item = _currentIngredients[i].Key;
                _recipeIngredients[i].Quantity = _currentIngredients[i].Value;
            }
            else
            {
                _recipeIngredients[i].Item = null;
            }
        }
        SetCraftButtonsVisibility();
    }

    private bool CanRecipeCanBeMade(out int itemCount)
    {
        if (!GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var playerInventory))
        {
            itemCount = 0;
            return false;
        }
        var possible = true;
        itemCount = int.MaxValue;
        foreach (var ingredient in _currentIngredients)
        {
            var playerQuantity = playerInventory.GetCount(ingredient.Key);
            var recipeIngredient = _recipeIngredients.First(x => x.Item == ingredient.Key);
            recipeIngredient.PlayerInventory = playerQuantity;
            if (ingredient.Value > playerQuantity)
            {
                possible = false;
            }
            else
            {
                itemCount = System.Math.Min(itemCount, Convert.ToInt32(playerQuantity / ingredient.Value));
            }
        }
        return possible;
    }

    private void SetCraftButtonsVisibility()
    {
        ChangeCraftButtonsVisibility(CanRecipeCanBeMade(out var itemCount), itemCount);
    }

    private void ChangeCraftButtonsVisibility(bool isVisible, int itemCount)
    {
        if (craftButton == null) return;
        craftButton.SetActive(isVisible);
        craftMaxButton.SetActive(isVisible);
        if (isVisible)
        {
            craftMaxButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Craft Max ({itemCount})";
        }
    }

    public void Craft()
    {
        if (_item == null || !_item.isCraftable) return;
        if (!GlobalFunctions.TryGetPlayerComponent<PlayerInventory>(out var playerInventory)) return;
        foreach (var ingredient in _currentIngredients)
        {
            playerInventory.Remove(ingredient.Key, ingredient.Value);
        }
        playerInventory.Add(_item);
    }

    public void CraftMax()
    {
        if (_item == null || !_item.isCraftable) return;
        while (CanRecipeCanBeMade(out _))
        {
            Craft();
        }
    }

}
