using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLayoutImpact : MonoBehaviour
{
    public bool alsoOnWater;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        var hit = Physics2D.OverlapCircle(transform.position, transform.localScale.x / 2f, LayerManagement.Layout.value + (alsoOnWater ? LayerManagement.Water.value : 0));
        if (hit != null)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
