using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Classes;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionController : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemTitle;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemValue;
    public Image currencySymbol;
    public GameObject itemManagerObject;

    private DB.Item _item;
    private IItemManager _itemManager;

    void Start()
    {
        itemImage.sprite = null;
        Clear();
    }

    void OnEnable()
    {
        Clear();
        _itemManager = itemManagerObject.GetComponent<IItemManager>();
        _itemManager.SelectedItemChanged += OnSelectedItemChanged;
    }

    void OnDisable()
    {
        _itemManager.SelectedItemChanged -= OnSelectedItemChanged;
        _item = null;
    }

    private void OnSelectedItemChanged(DB.Item item, Constants.PanelTypes panelType)
    {
        _item = item;
        if (item == null)
        {
            Clear();
        }
        else
        {
            itemImage.sprite = item.GetArt();
            var c = itemImage.color;
            c.a = 1f;
            itemImage.color = c;
            itemTitle.text = item.Name;
            itemDescription.text = item.Description;
            itemValue.text = item.Value.ToString();
            currencySymbol.enabled = true;
        }
    }

    // private void OnSelectedIndexChanged(int selectedIndex)
    // {
    //     if (selectedIndex == -1)
    //     {
    //         OnSelectedItemChanged(null, Constants.PanelTypes.Inventory);
    //     }
    // }

    private void Clear()
    {
        _item = null;
        var c = itemImage.color;
        c.a = 0f;
        itemImage.color = c;
        itemTitle.text = "";
        itemDescription.text = "";
        itemValue.text = "";
        currencySymbol.enabled = false;
    }

}
