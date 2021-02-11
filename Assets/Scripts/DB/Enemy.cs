using SQLite4Unity3d;

namespace DB
{
    [Table("enemies")]
    public class Enemy
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("prefabname")]
        public string PrefabName { get; set; }
    }
}