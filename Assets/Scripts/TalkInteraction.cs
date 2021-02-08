using System.Collections;
using System.Collections.Generic;
using Classes;
using Dialog;
using Interfaces;
using UnityEngine;

public class TalkInteraction : InteractionController
{
    public override string interactionText => "Talk";
    public Constants.Npc npc;

    public override Constants.InteractionTypes interactionType => Constants.InteractionTypes.Talk;

    public override void Interact()
    {
        IDialogItem dialogItem = null;
        switch (npc)
        {
            case Constants.Npc.HoboTom:
                dialogItem = new DialogHoboTom();
                break;
            default:
                return;
        }
        DialogController.UpdateDialog(dialogItem);
    }
}
