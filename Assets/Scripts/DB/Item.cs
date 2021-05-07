using Interfaces;
using ItemCode;
using SQLite4Unity3d;
using UnityEngine;

namespace DB
{
    [Table("items")]
    public class Item : IActionable
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("description")]
        public string Description { get; set; }
        
        [Column("value")]
        public int Value { get; set; }

        [Column("iscraftable")]
        public bool IsCraftable { get; set; }

        [Column("isdrinkable")]
        public bool IsDrinkable { get; set; }

        [Column("isspell")]
        public bool IsSpell { get; set; }

        public GameObject _baseDrop;

        public GameObject Instantiate(Vector3 position)
        {
            if (_baseDrop == null)
            {
                _baseDrop = Resources.Load<GameObject>("baseDrop");
            }
            var currentDrop = GameObject.Instantiate(_baseDrop, position, Quaternion.identity);
            var itemController = currentDrop.GetComponent<DroppedItemController>();
            itemController.id = Id;
            var spriteRenderer = currentDrop.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetArt();
            var colliders2D = currentDrop.GetComponents<BoxCollider2D>();
            foreach (var collider2D in colliders2D)
            {
                collider2D.size = spriteRenderer.bounds.size + (collider2D.isTrigger ? new Vector3(0.05f, 0.05f, 0f) : Vector3.zero);
            }
            return currentDrop;
        }

        public Sprite GetArt()
        {
            var path = $"Items/{Path}";
            var sprite = Resources.Load<Sprite>(path);
            if (sprite == null){
                Debug.Log($"Art not found: {path}");
            }
            return sprite;
        }

        public IAction Action { get; set; }
    }
}