using System;

public interface IItemManager
{
    event Action<Item> SelectedItemChanged;
    Item selectedItem { get; set; }
}