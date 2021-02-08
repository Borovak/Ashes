using System;
using System.Collections.Generic;
using Classes;
using Interfaces;
using Static;
using UnityEngine;

namespace Dialog
{
    public class DialogShopMarket : IDialogItemChoices
    {
        Sprite IDialogItem.npcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

        string IDialogItem.npcName => "Shopkeeper";

        string IDialogItem.text => "Welcome to my shop youngling";

        Dictionary<string, Action> IDialogItemChoices.followUp => new Dictionary<string, Action>
        {
            {"Shop", () => DialogController.DoAction(ShopController.Open)},
            {"Who are you?", () => DialogController.UpdateDialog(new DialogShopMarket0())}
        };
    }
}
