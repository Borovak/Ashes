using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnTouch : MonoBehaviour
{
    public int damage = 1; 

    void OnTriggerEnter2D(Collider2D collider){
        if (!collider.gameObject.TryGetComponent<PlayerLifeController>(out var player)) return;
        player.TakeDamage(damage, gameObject.name);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (!collision.gameObject.TryGetComponent<ShieldController>(out var shield)) return;
        shield.ShowContact(collision.GetContact(0).point);
    }
}
