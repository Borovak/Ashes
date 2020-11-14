using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public UnityEvent EventOnClick;

    private MenuGroup _menuGroup;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _menuGroup = transform.parent.gameObject.GetComponent<MenuGroup>();
        _menuGroup.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
