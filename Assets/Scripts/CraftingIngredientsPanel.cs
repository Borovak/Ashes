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
    private List<UI.UIRecipeIngredient> _uiRecipeIngredients;
    private Item _item;
    private List<ItemBundle> _requiredIngredients;

    void OnEnable()
    {
        if (_itemManager == null)
        {
            var recipeIngredients = new List<UI.UIRecipeIngredient>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i);
                if (!t.TryGetComponent<UI.UIRecipeIngredient>(out var recipeIngredient)) continue;
                recipeIngredients.Add(recipeIngredient);
            }
            _uiRecipeIngredients = recipeIngredients.OrderByDescending(x => x.transform.GetComponent<RectTransform>().anchoredPosition.y).ThenBy(x => x.transform.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        }
        GlobalInventoryManager.RegisterToInventoryChange(-1, SetCraftButtonsVisibility);
        _itemManager = itemManagerObject.GetComponent<IItemManager>();
        _itemManager.SelectedItemChanged += OnSelectedItemChanged;
        MenuInputs.Ok += Craft;
        MenuInputs.Special += CraftMax;
    }

    void OnDisable()
    {
        GlobalInventoryManager.UnregisterToInventoryChange(-1, SetCraftButtonsVisibility);
        _itemManager.SelectedItemChanged -= OnSelectedItemChanged;
        MenuInputs.Ok -= Craft;
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
        foreach (var recipeIngredient in _uiRecipeIngredients)
        {
            recipeIngredient.Item = null;
        }
        _requiredIngredients = null;
        ChangeCraftButtonsVisibility(false, 0);
    }

    private void CheckForRecipe(Item item)
    {
        if (!DataHandling.TryConnectToDb(out var connection)) return;
        var ingredients = connection.Table<DB.Recipe>().AsEnumerable().Where(x => x.ItemId == item.id).OrderBy(x => x.Ingredient).ToList();
        if (!ingredients.Any())
        {
            ClearIngredients();
            return;
        }
        _requiredIngredients = ingredients.Select(x => new ItemBundle(DropController.GetDropInfo(x.Ingredient), x.Quantity)).ToList();
        for (int i = 0; i < _uiRecipeIngredients.Count; i++)
        {
            if (i < _requiredIngredients.Count)
            {
                _uiRecipeIngredients[i].Item = _requiredIngredients[i].Item;
                _uiRecipeIngredients[i].Quantity = _requiredIngredients[i].Quantity;
            }
            else
            {
                _uiRecipeIngredients[i].Item = null;
            }
        }
        SetCraftButtonsVisibility();
    }

    private bool CanRecipeCanBeMade(out int itemCount)
    {
        if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory))
        {
            itemCount = 0;
            return false;
        }
        var possible = true;
        itemCount = int.MaxValue;
        foreach (var ingredient in _requiredIngredients)
        {
            var playerQuantity = inventory.GetQuantity(ingredient.Item);
            var recipeIngredient = _uiRecipeIngredients.First(x => x.Item == ingredient.Item);
            recipeIngredient.PlayerInventory = playerQuantity;
            if (ingredient.Quantity > playerQuantity)
            {
                possible = false;
            }
            else
            {
                itemCount = Math.Min(itemCount, MathFunctions.DivFloorInt(playerQuantity, ingredient.Quantity));
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
        if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
        foreach (var ingredient in _requiredIngredients)
        {
            inventory.Remove(ingredient.Item, ingredient.Quantity);
        }
        inventory.Add(_item, 1);
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
