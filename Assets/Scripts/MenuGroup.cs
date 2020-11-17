using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MenuGroup : MonoBehaviour
{
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

    private readonly Color _colorWhenSelected = new Color(1f, 1f, 1f, 0.1f);
    private readonly Color _colorWhenUnselected = new Color(1f, 1f, 1f, 0.02f);
    private List<MenuButton> _menuButtons;
    private MenuButton _hoveredButton;

    void Start()
    {
    }

    void OnEnable()
    {
        HoveredButton = MenuButtons?.FirstOrDefault();
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.Back += OnEventOnBack;
    }

    void OnDisable()
    {
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.Back -= OnEventOnBack;        
    }

    // Update is called once per frame
    public void Register(MenuButton menuButton)
    {
        if (_menuButtons == null)
        {
            _menuButtons = new List<MenuButton>();
        }
        _menuButtons.Add(menuButton);
    }

    private void UpdateVisuals()
    {
        if (_menuButtons == null)
        {
            _menuButtons = new List<MenuButton>();
        }
        foreach (var menuButton in _menuButtons)
        {
            menuButton.ChangeColor(menuButton == HoveredButton ? _colorWhenSelected : _colorWhenUnselected);
        }
    }

    private void OnSelectionChangeUp()
    {
        if (StandardSetup()) return;
        for (int i = 0; i < MenuButtons.Count; i++)
        {
            if (MenuButtons[i] != _hoveredButton) continue;
            HoveredButton = i == MenuButtons.Count - 1 ? MenuButtons.First() : MenuButtons[i + 1];
            return;
        }
    }

    private void OnSelectionChangeDown()
    {
        if (StandardSetup()) return;
        for (int i = 0; i < MenuButtons.Count; i++)
        {
            if (MenuButtons[i] != _hoveredButton) continue;
            HoveredButton = i == 0 ? MenuButtons.Last() : MenuButtons[i - 1];
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

    private void OnEventOnBack(){
        var backButton = _menuButtons?.FirstOrDefault(x => x.IsBackButton);
        backButton.EventOnClick?.Invoke();
    }
}
