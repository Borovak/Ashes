using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class SelectableItem : MonoBehaviour, IPointerEnterHandler
    {
        public static event Action<SelectableItem> SelectedItemChanged;
        public int index;
        public GameObject[] selectionControls;
        public UnityEvent eventOnEnter;
        protected bool IsSelected => SelectedItem == this;

        protected abstract void OnEnableAfter();
        protected abstract void OnDisableAfter();
        private static List<SelectableItem> _selectableItems;
        private static SelectableItem _selectedItem;

        void Awake()
        {
            foreach (var selectionControl in selectionControls)
            {
                selectionControl.SetActive(false);
            }
        }
        
        void OnEnable()
        {
            _selectableItems ??= new List<SelectableItem>();
            _selectableItems.Add(this);
            SelectedItemChanged += OnSelectedItemChanged;
            OnEnableAfter();
            SelectMin();
        }

        void OnDisable()
        {
            _selectableItems.Remove(this);
            SelectedItemChanged -= OnSelectedItemChanged;
            OnDisableAfter();
            SelectMin();
        }

        protected static SelectableItem SelectedItem
        {
            get => _selectedItem;
            private set
            {
                _selectedItem = value;
                SelectedItemChanged?.Invoke(value);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            SelectedItem = this;
            eventOnEnter?.Invoke();
        }

        private void OnSelectedItemChanged(SelectableItem selectedItem)
        {
            foreach (var selectionControl in selectionControls)
            {
                selectionControl.SetActive(selectedItem == this);
            }
        }

        public void Select()
        {
            SelectedItem = this;
        }
        
        public static void SelectByIndex(int index)
        {
            SelectedItem = _selectableItems.FirstOrDefault(x => x.index == index);
        }
        
        public static void SelectByName(string name)
        {
            var item = _selectableItems.FirstOrDefault(x => x.name == name);
            if (item == null) return;
            SelectedItem = item;
        }
        
        public static void SelectMin()
        {
            if (!_selectableItems.Any())
            {
                SelectedItem = null;
            }
            else
            {
                var minIndex = _selectableItems.Min(x => x.index);
                SelectedItem = _selectableItems.FirstOrDefault(x => x.index == minIndex);
            }
        }

        private static void SelectMax()
        {
            var maxIndex = _selectableItems.Max(x => x.index);
            SelectedItem = _selectableItems.Find(x => x.index == maxIndex);
        }

        public static void SelectionUp()
        {
            if (_selectedItem == null)
            {
                SelectMin();
            } 
            else if (_selectedItem.index == _selectableItems.Min(x => x.index))
            {
                SelectMax();
            }
            else
            {
                SelectedItem = _selectableItems.Where(x => x.index < SelectedItem.index).OrderByDescending(x => x.index).First();
            }
        }

        public static void SelectionDown()
        {
            if (_selectedItem == null)
            {
                SelectMin();
            } 
            else if (_selectedItem.index == _selectableItems.Max(x => x.index))
            {
                SelectMin();
            }
            else
            {
                var item = _selectableItems.Where(x => x.index > SelectedItem.index).OrderBy(x => x.index).First();
                SelectedItem = item;
            }
        }
    }
}
