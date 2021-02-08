using System;
using Classes;
using Interfaces;
using Static;
using UnityEngine;

namespace Dialog
{
    public class DialogShopMarket0 : IDialogItemSimple
    {
        Sprite IDialogItem.npcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

        string IDialogItem.npcName => "Shopkeeper";

        string IDialogItem.text => "I am, but a humble shopkeeper";

        Action IDialogItemSimple.ActionOnOk => () => DialogController.UpdateDialog(new DialogShopMarket());
    }
}
