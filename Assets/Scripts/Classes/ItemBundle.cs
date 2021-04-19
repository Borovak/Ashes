    using Static;

    namespace Classes
    {
        public class ItemBundle
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
        }
    }
