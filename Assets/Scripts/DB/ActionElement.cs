using Classes;
using Interfaces;
using SQLite4Unity3d;
using UnityEngine;

namespace DB
{
    [Table("actions")]
    public class ActionElement : IIconElement, IActionable
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

        public Sprite sprite { get; }
        public int id => Id;
        public string name => Name;
        public int quantity => -1;
        public Constants.IconElementTypes iconElementType => Constants.IconElementTypes.Action;
        public IAction Action { get; set; }
    }
}