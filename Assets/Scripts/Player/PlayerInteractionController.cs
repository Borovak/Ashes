using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public LayerMask whatIsInteraction;
    public LayerMask whatIsSavePoint;
    public static PlayerInteractionController Instance;
    public bool interactionIsSavePoint;
    public bool interactionPossible;
    public string interactionGuid;
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
            interactionIsSavePoint = false;
            interactionPossible = true;
            interactionPosition = interactionColliders[0].transform.position;
            interactionText = "";
            return;
        }
        var savePointColliders = Physics2D.OverlapCircleAll(transform.position, 1f, whatIsSavePoint);
        if (savePointColliders.Length > 0)
        {
            interactionIsSavePoint = true;
            interactionPossible = true;
            interactionPosition = savePointColliders[0].transform.position;
            var savePoint = savePointColliders[0].gameObject.GetComponent<SavePointController>();
            interactionGuid = savePoint.guid;
            interactionText = "Save";
            return;
        }
        interactionPossible = false;
    }

    public void Interact()
    {
        if (interactionPossible && interactionIsSavePoint)
        {
            var chamber = GameObject.FindGameObjectWithTag("Chamber").GetComponent<ChamberController>();
            var saveSuccess = SaveSystem.Save(interactionGuid, true, out var saveErrorMessage);
            Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
        }
    }
}
