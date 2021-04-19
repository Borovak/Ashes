using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongSlotController : ItemSlot
{
    
    protected override void UpdateCount(ref TextMeshProUGUI textControl, int count)
    {
        textControl.text = count >= 0 ? $"x{count.ToString()}" : "";
    }

    protected override void UpdateVisuals()
    {
        backImage.color = Item != null && isSelected ? colorWhenSelected : (isHovered ? colorWhenHovered : colorWhenNotSelected);
        itemText.text = Item != null ? Item.Name : "";
        iconImage.sprite = Item?.GetArt();
        iconImage.color = Item != null ? Color.white : colorWhenNull;
    }
}
