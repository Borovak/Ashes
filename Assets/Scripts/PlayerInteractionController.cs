using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public LayerMask whatIsInteraction;
    public LayerMask whatIsCampsite;
    public static PlayerInteractionController Instance;

    public bool interactionPossible;
    public Vector3 interactionPosition;
    public string interactionText;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var interactionColliders = Physics2D.OverlapCircleAll(transform.position, 1f, whatIsInteraction);
        if (interactionColliders.Length > 0)
        {
            interactionPossible = true;
            interactionPosition = interactionColliders[0].transform.position;
            interactionText = "";
            return;
        }
        var campsiteColliders = Physics2D.OverlapCircleAll(transform.position, 1f, whatIsCampsite);
        if (campsiteColliders.Length > 0)
        {
            interactionPossible = true;
            interactionPosition = campsiteColliders[0].transform.position;
            interactionText = "Camp";
            if (Input.GetButtonDown("Fire3")){
                PlayerPlatformerController.Instance.campsiteLocation = campsiteColliders[0].transform.position;
                SaveSystem.Save();
                Debug.Log($"Campsite changed to {PlayerPlatformerController.Instance.campsiteLocation}");
            }
            return;
        }
        interactionPossible = false;
    }
}
