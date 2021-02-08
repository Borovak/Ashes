using System;
using Classes;
using Interfaces;
using Static;
using UnityEngine;

namespace Dialog
{
    public class DialogHoboTom : IDialogItemSimple
    {
        Sprite IDialogItem.npcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.HoboTom, out var sprite) ? sprite : null;

        string IDialogItem.npcName => "Hobo Tom";

        string IDialogItem.text => "Hey, don't mind me, but I heard there are weird things happening in the forest...";

        Action IDialogItemSimple.ActionOnOk => null;
    }
}
