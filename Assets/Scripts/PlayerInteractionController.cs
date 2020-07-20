using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public LayerMask whatIsInteraction;
    public LayerMask whatIsSavePoint;
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
        var savePointColliders = Physics2D.OverlapCircleAll(transform.position, 1f, whatIsSavePoint);
        if (savePointColliders.Length > 0)
        {
            interactionIsCamp = true;
            interactionPossible = true;
            interactionPosition = savePointColliders[0].transform.position;
            interactionText = "Camp";
            return;
        }
        interactionPossible = false;
    }

    public void Interact()
    {
        if (interactionIsCamp)
        {
            var chamber = GameObject.FindGameObjectWithTag("Chamber").GetComponent<ChamberController>();
            SaveData.workingData.SavePointChamberId = chamber.chamberId;
            SaveData.workingData.SavePointId = -1;
            Debug.Log($"Save point changed to {SaveData.workingData.SavePointChamberId},{SaveData.workingData.SavePointId}");
            var saveSuccess = SaveSystem.Save(out var saveErrorMessage);
            Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
        }
    }
}
