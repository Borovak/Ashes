using Classes;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButton : SelectableItem, IPointerClickHandler
{
    public UnityEvent eventOnClick;
    public TextMeshProUGUI textControl;
    public bool isBackButton;
    public bool isSelected;

    private readonly Color _normalColor = new Color(0.82f, 0.82f, 0.82f);
    private readonly Color _selectedColor = new Color(0.56f, 0.89f, 0.56f);

    protected override void OnEnableAfter()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OnOk;
        if (isBackButton)
        {
            ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed += OnBack;
        }
        textControl.color = isSelected ? _selectedColor : _normalColor;
    }

    protected override void OnDisableAfter()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OnOk;
        if (isBackButton)
        {
            ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed -= OnBack;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        eventOnClick?.Invoke();
    }

    private void OnBack()
    {
        if (!isBackButton) return;
        eventOnClick?.Invoke();
    }

    private void OnOk()
    {
        if (SelectedItem != this) return;
        eventOnClick?.Invoke();
    }
}
