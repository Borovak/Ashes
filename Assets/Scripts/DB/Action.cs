using SQLite4Unity3d;

namespace DB
{
    [Table("actions")]
    public class Action
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("path")]
        public string Path { get; set; }

        public event System.Action Pressed;
        public event System.Action Released;

        public void Press()
        {
            Pressed?.Invoke();
        }
        public void Release()
        {
            Released?.Invoke();
        }
        
    }
}