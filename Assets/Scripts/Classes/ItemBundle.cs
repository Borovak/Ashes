    using Static;

    namespace Classes
    {
        public class ItemBundle
        {
            public Item Item;
            public int Quantity;

            public ItemBundle(int id, int quantity)
            {
                Item = DropController.GetDropInfo(id);
                Quantity = quantity;
            }

            public ItemBundle(Item item, int quantity)
            {
                Item = item;
                Quantity = quantity;
            }
        }
    }
