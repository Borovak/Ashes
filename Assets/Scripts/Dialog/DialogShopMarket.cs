using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogShopMarket : IDialogItemChoices
{
    Sprite IDialogItem.npcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

    string IDialogItem.npcName => "Shopkeeper";

    string IDialogItem.text => "Welcome to my shop youngling";

    Dictionary<string, Action> IDialogItemChoices.followUp => new Dictionary<string, Action>
    {
        {"Shop", () => DialogController.DoAction(GlobalFunctions.OpenShop)},
        {"Who are you?", () => DialogController.UpdateDialog(new DialogShopMarket0())}
    };
}
