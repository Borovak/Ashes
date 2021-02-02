using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongSlotController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public static event Action<Item> SelectedSlotChanged;
    public static event Action<int> SlotClicked;

    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            UpdateVisuals();
        }
    }

    public int Count
    {
        set => itemCount.text = $"x{value.ToString()}";
    }

    public Image backImage;
    public Image itemImage;
    public TextMeshProUGUI itemTitle;
    public TextMeshProUGUI itemCount;
    public Constants.PanelTypes panelType;
    public int index;
    public LongSlotPanel panel;

    private Item _item;
    private Color _colorWhenSelected = new Color(0.4f, 0.6f, 0.4f, 0.73f);
    private Color _colorWhenHovered = new Color(0.6f, 0.6f, 0.6f, 0.73f);
    private Color _colorWhenNotSelected = new Color(0.283f, 0.283f, 0.283f, 0.73f);
    private bool _isHovered;
    private bool _isSelected;

    // Start is called before the first frame update
    void Start()
    {
        Item = null;
        itemCount.gameObject.SetActive(panelType != Constants.PanelTypes.Craftables);
    }

    // Update is called once per frame
    void Update()
    {
        if (Item == null)
        {
            itemCount.text = "";
        }
    }

    void OnEnable()
    {
        LongSlotPanel.SelectedIndexChanged += OnSelectedIndexChanged;
    }

    void OnDisable()
    {
        LongSlotPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
        {
            _isHovered = true;
        }
        UpdateVisuals();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (_item == null) return;
        SlotClicked?.Invoke(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        UpdateVisuals();
    }

    private void OnSelectedIndexChanged(int selectedIndex)
    {        
        _isSelected = selectedIndex == index;
        if (_isSelected)
        {
            SelectedSlotChanged?.Invoke(_item);
        }
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        backImage.color = _item != null && panel.selectedIndex == index ? _colorWhenSelected : (_isHovered ? _colorWhenHovered : _colorWhenNotSelected);
        if (_item == null)
        {
            itemImage.sprite = null;
            itemImage.color = new Color(0f, 0f, 0f, 0f);
            itemTitle.text = "";
        }
        else
        {
            itemImage.sprite = _item.GetArt();
            itemImage.color = new Color(1f, 1f, 1f, 1f);
            itemTitle.text = _item.name;
        }
    }
}
