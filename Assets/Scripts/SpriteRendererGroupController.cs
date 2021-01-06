using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererGroupController : MonoBehaviour
{
    public int groupSortingOrder;
    // Start is called before the first frame update
    void Start()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder += groupSortingOrder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
