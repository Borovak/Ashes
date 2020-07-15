using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffsetMovement : MonoBehaviour
{
    public bool x;
    public bool y;
    public Vector2 amount;

    private Material _material;


    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        var offset = _material.GetTextureOffset("_MainTex");
        var newOffset = offset + (amount * Time.deltaTime);
        _material.SetTextureOffset("_MainTex", newOffset);
    }
}
