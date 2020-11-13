using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{

    public GameObject slotPrefab;
    public int slotCount;
    public int slotsPerRow;
    public float margin;
    public bool refreshNeeded = true;

    private List<GameObject> _slots;

    // Start is called before the first frame update
    void Start()
    {
        _slots = new List<GameObject>();
        for (int i = 0; i < slotCount; i++)
        {
            var slot = GameObject.Instantiate(slotPrefab);
            slot.transform.SetParent(transform);
            _slots.Add(slot);
            //Position
            var rectTransform = slot.GetComponent<RectTransform>();
            var position = rectTransform.anchoredPosition;
            position.x = (i % slotsPerRow) * (rectTransform.rect.size.x + margin) + margin;
            position.y = Convert.ToInt32(i / slotsPerRow) * -(rectTransform.rect.size.y + margin) - margin;
            rectTransform.anchoredPosition = position;
        }
    }

    void OnEnable(){
        refreshNeeded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!refreshNeeded) return;
        refreshNeeded = false;
        var playerInventory = GlobalFunctions.GetPlayerInventory();
        playerInventory.GetItemsAndCounts(out var items, out var counts);
        for (int i = 0; i < slotCount; i++)
        {
            var slot = _slots[i];
            var image = slot.GetComponentsInChildren<Image>().Where(x => x.gameObject.name == "ItemImage").ToList()[0];
            var text = slot.GetComponentsInChildren<Text>().Where(x => x.gameObject.name == "ItemCount").ToList()[0];
            if (i >= items.Count)
            {
                image.sprite = null;
                image.color = new Color(0f,0f,0f,0f);
                text.text = "";
                continue;
            }
            image.sprite = items[i].GetArt();
            image.color = new Color(1f,1f,1f,1f);
            text.text = counts[i].ToString();
        }
    }
}
