using System.Collections;
using System.Collections.Generic;
using Classes;
using Dialog;
using UnityEngine;

public class ShopInteractionController : InteractionController
{
    public override string interactionText => "Shop";

    public override Constants.InteractionTypes interactionType => Constants.InteractionTypes.Shop;

    public override void Interact()
    {
        DialogController.UpdateDialog(new DialogShopMarket());
    }
}
