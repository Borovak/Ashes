using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicMissileController : MonoBehaviour
{
    public float speed;
    public float radius;
    public int damage;
    public float lifetime;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsLayout;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        var playerTransform = PlayerPlatformerController.Instance.transform;
        direction = playerTransform.position.x > transform.position.x ? Vector2.left : Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0){
            GameObject.Destroy(gameObject);
            return;
        }
        var enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (!enemiesToDamage[i].TryGetComponent<EnemyLifeController>(out var enemy)) continue;
            enemy.TakeDamage(damage, gameObject.name);
            GameObject.Destroy(gameObject);
            return;
        }
        var layout = Physics2D.OverlapCircleAll(transform.position, radius, whatIsLayout);
        if (layout.Length > 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
