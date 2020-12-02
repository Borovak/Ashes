using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLayoutImpact : MonoBehaviour
{
    public LayerMask whatIsLayout;
    public float checkDiameter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        var layout = Physics2D.OverlapCircleAll(transform.position, checkDiameter / 2f, whatIsLayout);
        if (layout.Length > 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
