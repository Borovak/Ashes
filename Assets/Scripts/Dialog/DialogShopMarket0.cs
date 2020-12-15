using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogShopMarket0 : IDialogItemSimple
{
    Sprite IDialogItem.npcSprite => GlobalFunctions.TryGetNpcSprite(Constants.Npc.Shopkeeper, out var sprite) ? sprite : null;

    string IDialogItem.npcName => "Shopkeeper";

    string IDialogItem.text => "I am, but a humble shopkeeper";

    Action IDialogItemSimple.ActionOnOk => () => DialogController.UpdateDialog(new DialogShopMarket());
}
