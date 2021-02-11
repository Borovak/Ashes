using SQLite4Unity3d;

namespace DB
{
    [Table("recipes")]
    public class Recipe
    {
        [Column("items_id")]
        public int ItemId { get; set; }

        [Column("ingredient")]
        public int Ingredient { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}