using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    
    public int index;

    private Image _image;
    private Text _text;
    private int _lastId;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.GetItemAtIndex(index, out var item, out var count)){
            _image.sprite = null;
            _text.text = "";
            return;
        }
        if (item.id == _lastId) return;
        _lastId = item.id;
        var sprite = Resources.Load<Sprite>($"Ingredients/{item.artFilePath}");
        _image.sprite = sprite;
        _text.text = count.ToString();
    }
}
