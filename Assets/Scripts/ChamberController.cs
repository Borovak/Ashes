using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberController : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera Camera;
    public float BackgroundLightIntensity;
    public Color BackgroundLightColor;
    public float TerrainLightIntensity;
    public Color TerrainLightColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        GameController.Instance.ChangeChamber(this);
    }
}
