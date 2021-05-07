    using Interfaces;
    using Static;
    using UnityEngine;

    namespace Classes
    {
        public class ItemBundle : IIconElement
        {
            public DB.Item Item;
            public int Quantity;

            public ItemBundle(int id, int quantity)
            {
                Item = DropController.GetDropInfo(id);
                Quantity = quantity;
            }

            public ItemBundle(DB.Item item, int quantity)
            {
                Item = item;
                Quantity = quantity;
            }

            public Sprite sprite => Item.GetArt();
            public int id => Item.Id;
            public string name => Item.Name;
            public int quantity => Quantity;
            public Constants.IconElementTypes iconElementType => Constants.IconElementTypes.Item;
        }
    }
