using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public UnityEvent EventOnClick;
    public int index;
    public bool IsBackButton;

    private MenuGroup _menuGroup;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _menuGroup = transform.parent.gameObject.GetComponent<MenuGroup>();
        _menuGroup.Register(this);
    }

    void OnEnable()
    {
        MenuInputs.OK += OnOk;
    }

    void OnDisable()
    {
        MenuInputs.OK -= OnOk;
    }

    // Update is called once per frame
    public void ChangeColor(Color color)
    {
        _image.color = color;
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
        if (_menuGroup.HoveredButton != this) return;
        EventOnClick?.Invoke();
    }
}
