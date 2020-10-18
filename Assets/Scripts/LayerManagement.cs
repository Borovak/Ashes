using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManagement : MonoBehaviour
{
    public static LayerMask Player;
    public static LayerMask Layout;

    public LayerMask playerLayer;
    public LayerMask layoutLayer;
    // Start is called before the first frame update
    void Start()
    {
        Player = playerLayer;
        Layout = layoutLayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
