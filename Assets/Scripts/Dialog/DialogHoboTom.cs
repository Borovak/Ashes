using System;
using System.Collections.Generic;
using Classes;
using Interfaces;
using Static;
using UI;
using UnityEngine;

namespace Dialog
{
    public class DialogHoboTom : IDialogItemChoices
    {
        Sprite IDialogItem.NpcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.HoboTom, out var sprite) ? sprite : null;

        string IDialogItem.NpcName => "Hobo Tom";

        string IDialogItem.Text => "Hey, don't mind me, but I heard there are weird things happening in the forest...";

        Dictionary<string, Action> IDialogItemChoices.FollowUp => new Dictionary<string, Action>
        {
            {"Leave", MenuController.ReturnToGame }
        };
    }
}
