using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogShopMarket : IDialogItem
{
    Sprite IDialogItem.npcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

    string IDialogItem.npcName => "Shopkeeper";

    string IDialogItem.text => "Welcome to my shop youngling";

    Action IDialogItem.ActionOnOk => GlobalFunctions.OpenShop;

    Dictionary<string, IDialogItem> IDialogItem.followUp => null;
}
