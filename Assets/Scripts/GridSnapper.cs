using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSnapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentPos = transform.position;
        transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
    }
}
