using SQLite4Unity3d;

namespace DB
{
        [Table("npcs")]
        public class Npc
        {
            [Column("id")]
            public int Id { get; set; }

            [Column("name")]
            public string Name { get; set; }

            [Column("path")]
            public string Path { get; set; }
        }
}