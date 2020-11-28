using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InventorySlotSpawner : MonoBehaviour
{

    public GameObject inventorySlotPrefab;
    public bool place;
    public float biasX;
    public float biasY;
    public float margin;
    public float xCount;
    public float yCount;

    // Start is called before the first frame update
    void Start()
    {
        place = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!place) return;
        place = false;
        var secIndex = 10000;
        while (transform.childCount > 0 && secIndex > 0)
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            secIndex--;
        }
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                var inventorySlot = GameObject.Instantiate(inventorySlotPrefab, transform);
                var rectTransform = inventorySlot.GetComponent<RectTransform>();
                var ap = rectTransform.anchoredPosition;
                ap.x = biasX + (margin * x);
                ap.y = biasY - (margin * y);
                rectTransform.anchoredPosition = ap;
            }
        }
    }
}
