using System.Collections;
using System.Collections.Generic;
using Static;
using UnityEngine;

public class GroundedEnemyController : Enemy
{
    public float groundCheckLength = 1f;
    
    protected override float jumpTakeOffSpeed => 1f;

    public override bool canFly => false;

    private RaycastHit2D _leftLedgeHitCheck;
    private RaycastHit2D _rightLedgeHitCheck;


    protected override void CheckForLedges()
    {
        if (followPlayer) return;
        
        _rightLedgeHitCheck = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, groundCheckLength, LayerManagement.Layout);
        Debug.DrawRay(new Vector2(transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, Color.blue);
        if (_rightLedgeHitCheck.collider == null)
        {
            direction = -1;
            //Debug.Log($"{gameObject.name} sees right ledge");
        }

        _leftLedgeHitCheck = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, groundCheckLength, LayerManagement.Layout);
        Debug.DrawRay(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, Color.blue);

        if (_leftLedgeHitCheck.collider == null)
        {
            direction = 1;
            //Debug.Log($"{gameObject.name} sees left ledge");
        }
    }

    protected override void AfterStart()
    {
    }
}
