using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManagement : MonoBehaviour
{
    public static LayerMask Player;
    public static LayerMask Layout;
    public static LayerMask Enemies;
    public static LayerMask Water;
    public static LayerMask Interaction;
    public static LayerMask SavePoint;

    public LayerMask playerLayer;
    public LayerMask layoutLayer;
    public LayerMask enemiesLayer;
    public LayerMask waterLayer;
    public LayerMask interactionLayer;
    public LayerMask savePointLayer;
    // Start is called before the first frame update
    void Start()
    {
        Player = playerLayer;
        Layout = layoutLayer;
        Enemies = enemiesLayer;
        Water = waterLayer;
        Interaction = interactionLayer;
        SavePoint = savePointLayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
