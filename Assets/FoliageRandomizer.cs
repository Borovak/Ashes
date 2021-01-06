using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FoliageRandomizer : MonoBehaviour
{
    public Sprite[] sprites; 
    public bool regenerate;
    public int groupSortingOrder;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!regenerate || sprites.Length == 0) return;
        regenerate = false;
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        var sortingOrderIndex = 0;
        foreach (var spriteRenderer in spriteRenderers)
        {
            var index = UnityEngine.Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[index];
            spriteRenderer.sortingOrder = groupSortingOrder + sortingOrderIndex;
            sortingOrderIndex++;
        }
    }
}
