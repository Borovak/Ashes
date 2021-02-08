using Classes;
using TMPro;

public class InventorySlotController : ItemSlot
{
    protected override void UpdateVisuals()
    {
        if (isSelected && Item != null)
        {
            backImage.color = colorWhenSelected;
        }
        else
        {
            backImage.color = isHovered ? colorWhenHovered : colorWhenNotSelected;
        }
    }

    protected override void UpdateCount(ref TextMeshProUGUI textControl, int count)
    {
        textControl.text = count >= 0 ? count.ToString() : "";
    }
}
