using System;
using System.Collections.Generic;
using Classes;
using Interfaces;
using Static;
using UI;
using UnityEngine;

namespace Dialog
{
    public class DialogShopMarket : IDialogItemChoices
    {
        Sprite IDialogItem.NpcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

        string IDialogItem.NpcName => "Shopkeeper";

        string IDialogItem.Text => "Welcome to my shop youngling";

        Dictionary<string, Action> IDialogItemChoices.FollowUp => new Dictionary<string, Action>
        {
            {"Shop", () => MenuController.OpenShop(0)},
            {"Who are you?", () => DialogController.UpdateDialog(new DialogShopMarket0())}
        };
    }
}
