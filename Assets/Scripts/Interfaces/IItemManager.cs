using System;
using Classes;

namespace Interfaces
{
    public interface IItemManager
    {
        event Action<DB.Item, Constants.PanelTypes> SelectedItemChanged;
        DB.Item selectedItem { get; set; }
    }
}