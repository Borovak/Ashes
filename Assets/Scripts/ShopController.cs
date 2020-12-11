using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : InteractionController
{
    public override string interactionText => "Shop";

    public override Constants.InteractionTypes interactionType => Constants.InteractionTypes.Shop;

    public override void Interact()
    {
        DialogController.UpdateDialog(new DialogShopMarket());
    }
}
