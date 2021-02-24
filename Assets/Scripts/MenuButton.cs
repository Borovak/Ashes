using System.Collections;
using System.Collections.Generic;
using Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private readonly Color _colorWhenHovered = new Color(1f, 1f, 1f, 0.1f);
    private readonly Color _colorWhenNotHovered = new Color(1f, 1f, 1f, 0.02f);
    private readonly Color _colorWhenActive = new Color(0f, 1f, 0f, 1f);
    private readonly Color _colorWhenInactive = new Color(1f, 1f, 1f, 1f);

    public UnityEvent EventOnClick;
    public int index;
    public bool IsBackButton;

    private MenuGroup _menuGroup;
    private Image _image;
    private TextMeshProUGUI[] _texts;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
            _texts = GetComponentsInChildren<TextMeshProUGUI>();
        }
        if (_menuGroup == null)
        {
            if (!transform.parent.gameObject.TryGetComponent<MenuGroup>(out _menuGroup)) return;
            _menuGroup.Register(this);
        }
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OnOk;
    }

    void OnDisable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OnOk;
        if (_menuGroup == null) return;
    }

    // Update is called once per frame
    public void ChangeState(bool isHovered, bool isActive)
    {
        _image.color = isHovered ? _colorWhenHovered : _colorWhenNotHovered;
        foreach (var text in _texts)
        {
            text.color = isActive ? _colorWhenActive : _colorWhenInactive;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _menuGroup.HoveredButton = this;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _menuGroup.HoveredButton = null;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        EventOnClick?.Invoke();
    }

    private void OnOk()
    {
        if (_menuGroup == null) return;
        if (_menuGroup.HoveredButton != this) return;
        if (_menuGroup.canBeActive)
        {
            _menuGroup.ActiveButton = this;
        }
        EventOnClick?.Invoke();
    }
}
