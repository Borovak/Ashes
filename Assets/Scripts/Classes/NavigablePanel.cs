using System;
using System.Collections.Generic;
using Interfaces;
using UI;
using UnityEngine;

namespace Classes
{
    public abstract class NavigablePanel : MonoBehaviour, INavigablePanel
    {    
        public event Action<int, Constants.PanelTypes> SelectedIndexChanged;
        public event Action<Item, Constants.PanelTypes> SelectedItemChanged;
        public event Action ExitUp;
        public event Action ExitDown;
        public event Action ExitLeft;
        public event Action ExitRight;
        public event Action FocusGained;
        public event Action FocusLost;
        public event Action<INavigablePanel> FocusRequested;
        protected abstract void OnEnableSpecific();
        protected abstract void OnDisableSpecific();
        protected abstract void OnSelectionChangeUp();
        protected abstract void OnSelectionChangeDown();
        protected abstract void OnSelectionChangeLeft();
        protected abstract void OnSelectionChangeRight();
        protected abstract void OnSelectedItemChanged(Item item);
        protected abstract List<ItemSlot> GetItemSlots();

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                SelectedIndexChanged?.Invoke(value, panelTypeSelection);
                if (itemSlots != null && _selectedIndex >= 0)
                {
                    SelectedItemChanged?.Invoke(itemSlots[_selectedIndex].Item, panelTypeSelection);
                }
            }
        }
        public bool CanExitUp { get => canSelectionExitUp; set => canSelectionExitUp = value; }
        public bool CanExitDown { get => canSelectionExitDown; set => canSelectionExitDown = value; }
        public bool CanExitLeft { get => canSelectionExitLeft; set => canSelectionExitLeft = value; }
        public bool CanExitRight { get => canSelectionExitRight; set => canSelectionExitRight = value; }
        public bool HasFocus 
        { 
            get => _hasFocus;
            set
            {
                if (value && !_hasFocus)
                {
                    FocusGained?.Invoke();
                    MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
                    MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
                    MenuInputs.SelectionChangeLeft += OnSelectionChangeLeft;
                    MenuInputs.SelectionChangeRight += OnSelectionChangeRight;
                    SelectedIndex = 0;
                } 
                else if (!value && _hasFocus)
                {
                    FocusLost?.Invoke();
                    MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
                    MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
                    MenuInputs.SelectionChangeLeft -= OnSelectionChangeLeft;
                    MenuInputs.SelectionChangeRight -= OnSelectionChangeRight;
                }
                _hasFocus = value;
            } 
        }
        public Constants.PanelTypes PanelType => panelTypeSelection;
        public Constants.PanelTypes panelTypeSelection;
        public bool canSelectionExitUp;
        public bool canSelectionExitDown;
        public bool canSelectionExitLeft;
        public bool canSelectionExitRight;
        public bool refreshNeeded = true;
        protected List<ItemSlot> itemSlots;
        private int _selectedIndex;
        private bool _hasFocus;

        void OnEnable()
        {
            if (itemSlots == null)
            {
                itemSlots = GetItemSlots();
                foreach (var itemSlot in itemSlots)
                {
                    itemSlot.SlotClicked += OnSlotClicked;
                }
            }
            GlobalInventoryManager.RegisterToInventoryChange(-1, OnRefreshNeeded);
            if (GlobalShopManager.currentShopId != -1)
            {
                GlobalInventoryManager.RegisterToInventoryChange(GlobalShopManager.currentShopId, OnRefreshNeeded);
            }
            OnEnableSpecific();
        }
        
        void OnDisable()
        {
            GlobalInventoryManager.UnregisterToInventoryChange(-1, OnRefreshNeeded);
            if (GlobalShopManager.currentShopId != -1)
            {
                GlobalInventoryManager.UnregisterToInventoryChange(GlobalShopManager.currentShopId, OnRefreshNeeded);
            }
            OnDisableSpecific();
        }

        private void OnSlotClicked(int index)
        {
            FocusRequested?.Invoke(this);
            SelectedIndex = index;
        }
        
        protected void TryExitUp()
        {
            if (!CanExitUp) return;
            HasFocus = false;
            ExitUp?.Invoke();
        }

        protected void TryExitDown()
        {
            if (!CanExitDown) return;
            HasFocus = false;
            ExitDown?.Invoke();
        }

        protected void TryExitLeft()
        {
            if (!CanExitLeft) return;
            HasFocus = false;
            ExitLeft?.Invoke();
        }

        protected void TryExitRight()
        {
            if (!CanExitRight) return;
            HasFocus = false;
            ExitRight?.Invoke();
        }

        private void OnRefreshNeeded()
        {
            refreshNeeded = true;
        }
        
        
    }
}