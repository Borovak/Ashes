using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public string artFilePath;

    public GameObject _baseDrop;

    public GameObject Instantiate(Vector3 position)
    {
        if (_baseDrop == null)
        {
            _baseDrop = Resources.Load<GameObject>("baseDrop");
        }
        var currentDrop = GameObject.Instantiate(_baseDrop, position, Quaternion.identity);
        var itemController = currentDrop.GetComponent<ItemController>();
        itemController.id = id;
        var spriteRenderer = currentDrop.GetComponent<SpriteRenderer>();
        var sprite = Resources.Load<Sprite>($"Ingredients/{artFilePath}");
        spriteRenderer.sprite = sprite;
        var colliders2D = currentDrop.GetComponents<BoxCollider2D>();
        foreach (var collider2D in colliders2D)
        {
            collider2D.size = spriteRenderer.bounds.size + (collider2D.isTrigger ? new Vector3(0.05f, 0.05f, 0f) : Vector3.zero);
        }
        return currentDrop;
    }
}