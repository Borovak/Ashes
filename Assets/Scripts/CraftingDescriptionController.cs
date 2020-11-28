using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingDescriptionController : MonoBehaviour
{
    // Start is called before the first frame update

    private Image _image;
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;

    void Start()
    {
        InventoryItemController.SelectedItemChanged += OnSelectedItemChanged;
        _image = GetComponentsInChildren<Image>().First(x => x.gameObject.name == "Image");
        _title = GetComponentsInChildren<TextMeshProUGUI>().First(x => x.gameObject.name == "Title");
        _description = GetComponentsInChildren<TextMeshProUGUI>().First(x => x.gameObject.name == "Description");
        _image.sprite = null;
        Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSelectedItemChanged(Item item)
    {
        if (item == null)
        {
            Clear();
        }
        else
        {
            _image.sprite = item.GetArt();
            var c = _image.color;
            c.a = 1f;
            _image.color = c;
            _title.text = item.name;
            _description.text = item.description;
        }
    }

    private void Clear()
    {
        var c = _image.color;
        c.a = 0f;
        _image.color = c;
        _title.text = "";
        _description.text = "";
    }
}
