using SQLite4Unity3d;

namespace DB
{
    [Table("shops")]
    public class Shop
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}