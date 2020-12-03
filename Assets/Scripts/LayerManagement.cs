using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManagement : MonoBehaviour
{
    public static LayerMask Player;
    public static LayerMask Layout;
    public static LayerMask Enemies;
    public static LayerMask Water;

    public LayerMask playerLayer;
    public LayerMask layoutLayer;
    public LayerMask enemiesLayer;
    public LayerMask waterLayer;
    // Start is called before the first frame update
    void Start()
    {
        Player = playerLayer;
        Layout = layoutLayer;
        Enemies = enemiesLayer;
        Water = waterLayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
