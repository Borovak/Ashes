using System;
using Classes;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interfaces
{
    public interface ISlot : IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public event Action<int> SlotClicked;
        public Item Item { get; set; }
        public int Count { get; set; }
    }
}