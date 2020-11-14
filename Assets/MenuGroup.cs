using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGroup : MonoBehaviour
{
    public List<MenuButton> MenuButtons;
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
    private MenuButton _hoveredButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called once per frame
    public void Register(MenuButton menuButton)
    {
        MenuButtons.Add(menuButton);
    }

    private void UpdateVisuals()
    {
        foreach (var menuButton in MenuButtons)
        {
            menuButton.ChangeColor(menuButton == HoveredButton ? _colorWhenSelected : _colorWhenUnselected);
        }
    }
}
