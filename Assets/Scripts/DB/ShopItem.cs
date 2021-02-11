using SQLite4Unity3d;

namespace DB
{
    [Table("shop_items")]
    public class ShopItem
    {
        [Column("shops_id")]
        public int ShopId { get; set; }

        [Column("items_id")]
        public int ItemId { get; set; }

        [Column("min")]
        public int Min { get; set; }

        [Column("max")]
        public int Max { get; set; }
    }
}