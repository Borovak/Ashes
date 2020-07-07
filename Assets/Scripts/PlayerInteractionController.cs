using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public LayerMask whatIsInteraction;
    public LayerMask whatIsCampsite;
    public static PlayerInteractionController Instance;
    public bool interactionIsCamp;
    public bool interactionPossible;
    public Vector3 interactionPosition;
    public string interactionText;

    private PlayerInputs _inputs;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _inputs = GetComponent<PlayerInputs>();
        _inputs.Interact += Interact;
    }

    // Update is called once per frame
    void Update()
    {
        var interactionColliders = Physics2D.OverlapCircleAll(transform.position, 1f, whatIsInteraction);
        if (interactionColliders.Length > 0)
        {
            interactionIsCamp = false;
            interactionPossible = true;
            interactionPosition = interactionColliders[0].transform.position;
            interactionText = "";
            return;
        }
        var campsiteColliders = Physics2D.OverlapCircleAll(transform.position, 1f, whatIsCampsite);
        if (campsiteColliders.Length > 0)
        {
            interactionIsCamp = true;
            interactionPossible = true;
            interactionPosition = campsiteColliders[0].transform.position;
            interactionText = "Camp";
            return;
        }
        interactionPossible = false;
    }

    public void Interact()
    {
        if (interactionIsCamp)
        {
            PlayerPlatformerController.Instance.campsiteLocation = interactionPosition;
            var saveFilePath = SaveSystem.Save();
            if (saveFilePath != "")
            {
                Debug.Log($"Game saved at {saveFilePath}");
            }
            else
            {
                Debug.Log($"Game save unsuccessful");
            }
            Debug.Log($"Campsite changed to {PlayerPlatformerController.Instance.campsiteLocation}");
        }
    }
}
