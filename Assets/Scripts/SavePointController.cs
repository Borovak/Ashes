using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePointController : InteractionController
{
    public override string interactionText => "Save";

    public override Constants.InteractionTypes interactionType => Constants.InteractionTypes.SavePoint;

    public override void Interact()
    {
        var saveSuccess = SaveSystem.Save(guid, true, out var saveErrorMessage);
        Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
    }
}
