using System;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Classes
{
    public abstract class ItemSlot : MonoBehaviour, ISlot
    {

        public event Action<int> SlotClicked;
        public Image backImage;
        public Image iconImage;
        public TextMeshProUGUI itemText;
        public TextMeshProUGUI countText;
        public int index;
        
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                if (_item == null)
                {
                    iconImage.sprite = null;
                    iconImage.color = colorWhenNull;
                    Count = -1;
                    if (itemText != null)
                    {
                        itemText.text = "";
                    }
                }
                else
                {
                    iconImage.sprite = _item.GetArt();
                    iconImage.color = Color.white;
                }
            }
        }
        
        public int Count
        {
            get => _count;
            set
            {
                _count = _showCount ? value : -1;
                UpdateCount(ref countText, _count);
            }
        }

        protected abstract void UpdateVisuals();
        protected abstract void UpdateCount(ref TextMeshProUGUI textControl, int count);
        protected readonly Color colorWhenSelected = new Color(0.4f, 0.6f, 0.4f, 0.73f);
        protected readonly Color colorWhenHovered = new Color(0.6f, 0.6f, 0.6f, 0.73f);
        protected readonly Color colorWhenNotSelected = new Color(0.283f, 0.283f, 0.283f, 0.73f);
        protected readonly Color colorWhenNull = new Color(0f, 0f, 0f, 0f);
        protected bool isSelected;
        protected bool isHovered;
        
        private Item _item;
        private int _count;
        private INavigablePanel _parentPanel;
        private bool _showCount;
        
        void OnEnable()
        {
            _parentPanel ??= transform.parent.GetComponent<INavigablePanel>();
            _showCount = _parentPanel.PanelType != Constants.PanelTypes.Craftables;
            _parentPanel.SelectedIndexChanged += OnSelectedIndexChanged;
            _parentPanel.FocusLost += OnFocusLost;
        }

        void OnDisable()
        {        
            _parentPanel.SelectedIndexChanged -= OnSelectedIndexChanged;
            _parentPanel.FocusLost -= OnFocusLost;
        }
        
        private void OnSelectedIndexChanged(int selectedIndex, Constants.PanelTypes panelType)
        {
            isSelected = index == selectedIndex;
            UpdateVisuals();
        }

        private void OnFocusLost()
        {
            isSelected = false;
            UpdateVisuals();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_item != null)
            {
                isHovered = true;
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
            if (_item != null)
            {
                isHovered = false;
            }
            UpdateVisuals();
        }
        
    }
}