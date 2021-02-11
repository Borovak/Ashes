using SQLite4Unity3d;

namespace DB
{
    [Table("items")]
    public class Item
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("iscraftable")]
        public bool IsCraftable { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("description")]
        public string Description { get; set; }
        
        [Column("value")]
        public int Value { get; set; }
    }
}