using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using UnityEngine;
using UnityEngine.Events;

public class MenuGroup : MonoBehaviour
{
    public bool canBeActive;
    public bool isNavigable;
    public List<MenuButton> MenuButtons => _menuButtons?.OrderBy(x => x.index).ToList();
    public MenuButton HoveredButton
    {
        get => _hoveredButton;
        set
        {
            _hoveredButton = value;
            UpdateVisuals();
        }
    }
    public MenuButton ActiveButton
    {
        get => _activeButton;
        set
        {
            _activeButton = value;
            UpdateVisuals();
        }
    }

    private List<MenuButton> _menuButtons;
    private MenuButton _hoveredButton;
    private MenuButton _activeButton;

    void OnEnable()
    {
        if (!canBeActive)
        {
            _activeButton = null;
        }
        if (isNavigable)
        {
            HoveredButton = MenuButtons?.FirstOrDefault();
            MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
            MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed += OnEventOnBack;
        }
        UpdateVisuals();
    }

    void OnDisable()
    {
        if (isNavigable)
        {
            MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
            MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed -= OnEventOnBack;
        }
    }

    // Update is called once per frame
    public void Register(MenuButton menuButton)
    {
        if (_menuButtons == null)
        {
            _menuButtons = new List<MenuButton>();
        }
        _menuButtons.Add(menuButton);
        if (isNavigable)
        {
            HoveredButton = MenuButtons?.FirstOrDefault();
        }
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_menuButtons == null)
        {
            _menuButtons = new List<MenuButton>();
        }
        foreach (var menuButton in _menuButtons)
        {
            menuButton.ChangeState(menuButton == HoveredButton, menuButton == _activeButton);
        }
    }

    private void OnSelectionChangeUp()
    {
        if (!isNavigable || StandardSetup()) return;
        for (int i = 0; i < MenuButtons.Count; i++)
        {
            if (MenuButtons[i] != _hoveredButton) continue;
            HoveredButton = i == 0 ? MenuButtons.Last() : MenuButtons[i - 1];
            return;
        }
    }

    private void OnSelectionChangeDown()
    {
        if (!isNavigable || StandardSetup()) return;
        for (int i = 0; i < MenuButtons.Count; i++)
        {
            if (MenuButtons[i] != _hoveredButton) continue;
            HoveredButton = i == MenuButtons.Count - 1 ? MenuButtons.First() : MenuButtons[i + 1];
            return;
        }
    }

    private bool StandardSetup()
    {
        if (_menuButtons == null)
        {
            _menuButtons = new List<MenuButton>();
        }
        if (_menuButtons.Count == 0) return true;
        if (_hoveredButton == null || _menuButtons.Count == 1)
        {
            _hoveredButton = MenuButtons.First();
            return true;
        }
        return false;
    }

    private void OnEventOnBack()
    {
        if (!isNavigable) return;
        var backButton = _menuButtons?.FirstOrDefault(x => x.IsBackButton);
        if (backButton == null) return;
        backButton.EventOnClick?.Invoke();
    }

    public void SelectByName(string name)
    {
        var menuButton = MenuButtons.FirstOrDefault(x => x.name == name);
        HoveredButton = menuButton;
    }
}
