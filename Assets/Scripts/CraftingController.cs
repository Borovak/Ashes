using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Interfaces;
using UnityEngine;

public class CraftingController : MonoBehaviour, IItemManager
{
    public GameObject recipePanelObject;
    public GameObject ingredientPanelObject;
    public Item selectedItem { get; set; }
    public event Action<Item, Constants.PanelTypes> SelectedItemChanged;

    private INavigablePanel _recipePanel;
    private INavigablePanel _ingredientPanel;

    void OnEnable()
    {
        _recipePanel ??= recipePanelObject.GetComponent<INavigablePanel>(); 
        _ingredientPanel ??= ingredientPanelObject.GetComponent<INavigablePanel>();
        _recipePanel.SelectedIndexChanged += OnSelectedIndexChanged;
        _recipePanel.SelectedItemChanged += OnSelectedItemChanged;
        _recipePanel.ExitRight += OnRecipePanelExitRight;
        _recipePanel.FocusRequested += SelectPanel;
        _ingredientPanel.SelectedIndexChanged += OnSelectedIndexChanged;
        _ingredientPanel.SelectedItemChanged += OnSelectedItemChanged;
        _ingredientPanel.ExitLeft += OnIngredientPanelExitLeft;
        _ingredientPanel.FocusRequested += SelectPanel;
        _recipePanel.HasFocus = true;
    }

    void OnDisable()
    {
        _recipePanel.SelectedIndexChanged -= OnSelectedIndexChanged;
        _recipePanel.SelectedItemChanged -= OnSelectedItemChanged;
        _recipePanel.ExitRight -= OnRecipePanelExitRight;
        _recipePanel.FocusRequested -= SelectPanel;
        _ingredientPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
        _ingredientPanel.SelectedItemChanged -= OnSelectedItemChanged;
        _ingredientPanel.ExitLeft -= OnIngredientPanelExitLeft;
        _ingredientPanel.FocusRequested -= SelectPanel;
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

    private void OnRecipePanelExitRight()
    {
        SelectPanel(_ingredientPanel);
    }

    private void OnIngredientPanelExitLeft()
    {
        SelectPanel(_recipePanel);
    }

    private void SelectPanel(INavigablePanel panel)
    {
        _ingredientPanel.HasFocus = panel == _ingredientPanel;
        _recipePanel.HasFocus = panel == _recipePanel;
    }
}
