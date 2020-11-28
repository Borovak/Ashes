using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public static event Action<Item> SelectedItemChanged;

    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item == null)
            {
                _image.sprite = null;
                _image.color = new Color(0f, 0f, 0f, 0f);
                Count = -1;
            }
            else
            {
                _image.sprite = _item.GetArt();
                _image.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            _text.text = _count >= 0 ? _count.ToString() : "";
        }
    }

    private Item _item;
    private int _count;
    private Image _back;
    private Image _image;
    private TextMeshProUGUI _text;
    private Color _colorWhenSelected = new Color(0.6f, 0.6f, 0.6f, 0.73f);
    private Color _colorWhenNotSelected = new Color(0.283f, 0.283f, 0.283f, 0.73f);

    // Start is called before the first frame update
    void Start()
    {
        _back = GetComponent<Image>();
        _image = GetComponentsInChildren<Image>().Where(x => x.gameObject.name == "ItemImage").FirstOrDefault();
        _text = GetComponentsInChildren<TextMeshProUGUI>().Where(x => x.gameObject.name == "ItemCount").FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
        {
            _back.color = _colorWhenSelected;
            SelectedItemChanged?.Invoke(_item);
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _back.color = _colorWhenNotSelected;
    }
}
