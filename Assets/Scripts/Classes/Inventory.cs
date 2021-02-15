using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Classes
{
    public class Inventory
    {
        public event Action InventoryChanged;
        public readonly int id;

        private readonly Dictionary<int, int> _items;

        internal Inventory(int newId)
        {
            id = newId;
            _items = new Dictionary<int, int>();
        }

        public void AddMoney(int quantity)
        {
            Add(Constants.MONEY_ID, quantity);
        }

        public void Add(ItemBundle itemBundle)
        {
            Add(itemBundle.Item.id, itemBundle.Quantity);
        }

        public void Add(Item item, int quantity)
        {
            Add(item.id, quantity);
        }

        public void Add(int id, int quantity)
        {
            if (!_items.ContainsKey(id))
            {
                _items.Add(id, quantity);
            }
            else
            {
                _items[id] = _items[id] + quantity;
            }
            InventoryChanged?.Invoke();
        }

        public int Remove(Item item, int quantity)
        {
            return Remove(item.id, quantity);
        }

        public int Remove(int id, int quantity)
        {
            int quantityBefore;
            int quantityAfter;
            if (!_items.ContainsKey(id))
            {
                quantityBefore = 0;
                quantityAfter = 0;
                _items.Add(id, 0);
            }
            else
            {
                quantityBefore = _items[id];
                _items[id] = Math.Max(_items[id] - quantity, 0);
                quantityAfter = _items[id];
            }
            InventoryChanged?.Invoke();
            return quantityBefore - quantityAfter;
        }
        
        public int GetMoneyQuantity()
        {
            return GetQuantity(Constants.MONEY_ID);
        }

        public int GetQuantity(int id)
        {
            if (_items == null) return 0;
            return _items.TryGetValue(id, out var count) ? count : 0;
        }

        public XElement GetInventoryXe()
        {
            var xeInventory = new XElement("Inventory");
            xeInventory.SetAttributeValue("id", id);
            foreach (var item in _items.Where(x => x.Value > 0))
            {
                var xeItem = new XElement("Item");
                xeItem.SetAttributeValue("id", item.Key);
                xeItem.SetAttributeValue("quantity", item.Value);
                xeInventory.Add(xeItem);
            }
            return xeInventory;
        }


        public static Inventory GetInventoryFromXe(XElement xeInventory)
        {
            var id = int.TryParse(xeInventory.Attribute("id")?.Value ?? "", out var idTemp) ? idTemp : int.MinValue;
            if (id == int.MinValue) return null;
            var inventory = new Inventory(id);
            foreach (var xeItem in xeInventory.Elements("Item"))
            {
                var itemId = int.TryParse(xeItem.Attribute("id")?.Value ?? "", out var itemIdTemp) ? itemIdTemp : -1;
                var itemQuantity = int.TryParse(xeItem.Attribute("quantity")?.Value ?? "", out var itemQuantityTemp) ? itemQuantityTemp : -1;
                if (itemId == -1 || itemQuantity == -1) continue;
                inventory.Add(itemId, itemQuantity);
            }
            return inventory;
        }

        public List<ItemBundle> GetItemBundles()
        {
            return _items.Where(x => x.Value > 0).Select(x => new ItemBundle(x.Key, x.Value)).ToList();
        }

        public int GetQuantity(Item item)
        {
            return GetQuantity(item.id);
        }
    }
}