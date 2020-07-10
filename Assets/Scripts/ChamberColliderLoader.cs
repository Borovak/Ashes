using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberColliderLoader : MonoBehaviour
{
    public string chamberResourcePath;

    private static string _lastRoomCollided;

    // Start is called before the first frame update
    void Start()
    {
        _lastRoomCollided = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player" || _lastRoomCollided == name) return;
        _lastRoomCollided = name;
        Debug.Log($"Chamber entered: {name}");
        GameController.ChangeChamber(chamberResourcePath);
    }
}
