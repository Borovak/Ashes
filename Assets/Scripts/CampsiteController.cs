using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampsiteController : MonoBehaviour
{
    public static Dictionary<int, CampsiteController> campsites;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        if (campsites == null){
            campsites = new Dictionary<int, CampsiteController>();
        }
        campsites.Add(id, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
