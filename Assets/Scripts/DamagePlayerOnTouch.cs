using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Static;
using UnityEngine;

public class DamagePlayerOnTouch : MonoBehaviour
{
    private const float BaseDamage = 10f;
    public float damageBias = 0f;
    public bool manualDetection;
    public float manualDiameter = 1f;

    void FixedUpdate()
    {
        if (!manualDetection) return;
        var enemiesHit = new HashSet<GameObject>();
        var hits = Physics2D.CircleCastAll(transform.position, manualDiameter / 2f, Vector2.zero, 0f, LayerManagement.Player);
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (!hit.collider.gameObject.TryGetComponent<PlayerLifeController>(out var playerLifeController)) continue;
            enemiesHit.Add(hit.collider.gameObject);
            playerLifeController.TakeDamage(BaseDamage + damageBias, gameObject.name, hit.point);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (manualDetection) return;
        Debug.Log($"{gameObject.name} collision with {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent<ShieldController>(out var shield))
        {
            shield.ShowContact(collision.GetContact(0).point);
        }

        if (collision.gameObject.TryGetComponent<PlayerLifeController>(out var player))
        {
            player.TakeDamage(BaseDamage + damageBias, gameObject.name, collision.contacts[0].point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, manualDiameter / 2f);
    }
}