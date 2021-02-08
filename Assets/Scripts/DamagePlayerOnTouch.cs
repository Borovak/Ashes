using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class DamagePlayerOnTouch : MonoBehaviour
{
    public int damage = 1; 

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.TryGetComponent<ShieldController>(out var shield))
        {
            shield.ShowContact(collision.GetContact(0).point);
        }
        if (collision.gameObject.TryGetComponent<PlayerLifeController>(out var player))
        {
            player.TakeDamage(damage, gameObject.name, collision.contacts[0].point);
        }
    }
}
