using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapRoomController : MonoBehaviour
{
    public bool mustBeDeleted;
    public ChamberController chamberController;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = chamberController.chamberName;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustBeDeleted)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
