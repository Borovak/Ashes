using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : Enemy
{
    public override bool canFly => true;

    protected override float jumpTakeOffSpeed => 0;

    protected override void AfterStart()
    {
        gravityModifier = 0f;
    }

    protected override void CheckForLedges()
    {
        //Not for flying enemies
    }
}
