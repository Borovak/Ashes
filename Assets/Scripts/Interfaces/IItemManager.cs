using System;
using Classes;

namespace Interfaces
{
    public interface IItemManager
    {
        event Action<Item, Constants.PanelTypes> SelectedItemChanged;
        Item selectedItem { get; set; }
    }
}