using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlotController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public static event Action<Item> SelectedRecipeChanged;

    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item == null)
            {
                var c = backImage.color;
                c.a = 0f;
                backImage.color = c;
                itemImage.sprite = null;
                itemImage.color = new Color(0f, 0f, 0f, 0f);
                itemTitle.text = "";
            }
            else
            {
                backImage.color = _colorWhenNotSelected;
                itemImage.sprite = _item.GetArt();
                itemImage.color = new Color(1f, 1f, 1f, 1f);
                itemTitle.text = _item.name;
            }
        }
    }

    public Image backImage;
    public Image itemImage;
    public TextMeshProUGUI itemTitle;

    private Item _item;
    private Color _colorWhenSelected = new Color(0.4f, 0.6f, 0.4f, 0.73f);
    private Color _colorWhenHovered = new Color(0.6f, 0.6f, 0.6f, 0.73f);
    private Color _colorWhenNotSelected = new Color(0.283f, 0.283f, 0.283f, 0.73f);
    private bool _isHovered;

    // Start is called before the first frame update
    void Start()
    {
        Item = null;
    }

    // Update is called once per frame
    void Update()
    {
        backImage.color = CraftingDescriptionController.SelectedItem == Item && Item != null ? _colorWhenSelected : (_isHovered ? _colorWhenHovered : _colorWhenNotSelected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
        {
            _isHovered = true;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (_item != null)
        {
            SelectedRecipeChanged?.Invoke(_item);
            CraftingDescriptionController.SelectedItem = Item;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_item != null)
        {
            _isHovered = false;
        }
    }
}
