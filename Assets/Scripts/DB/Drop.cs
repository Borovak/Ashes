using SQLite4Unity3d;

namespace DB
{
    [Table("drops")]
    public class Drop
    {
        [Column("enemies_id")]
        public int EnemyId { get; set; }

        [Column("items_id")]
        public int ItemId { get; set; }

        [Column("dropquantitymin")]
        public int DropQuantityMin { get; set; }

        [Column("dropquantitymax")]
        public int DropQuantityMax { get; set; }

        [Column("droprate")]
        public float DropRate { get; set; }
    }
}