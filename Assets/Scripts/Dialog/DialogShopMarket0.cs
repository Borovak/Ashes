using System;
using System.Collections.Generic;
using Classes;
using Interfaces;
using Static;
using UnityEngine;

namespace Dialog
{
    public class DialogShopMarket0 : IDialogItemChoices
    {
        Sprite IDialogItem.NpcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

        string IDialogItem.NpcName => "Shopkeeper";

        string IDialogItem.Text => "I am, but a humble shopkeeper";

        public Dictionary<string, Action> FollowUp => new Dictionary<string, Action>
        {
            {"Return", () => DialogController.UpdateDialog(new DialogShopMarket())}
        };
    }
}
