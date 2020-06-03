using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicMissileController : MonoBehaviour
{
    public float speed;
    public float radius;
    public float damage;
    public float lifetime;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsLayout;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0){
            GameObject.Destroy(gameObject);
            return;
        }
        var enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (!enemiesToDamage[i].TryGetComponent<Enemy>(out var enemy)) continue;
            enemy.TakeDamage(damage, (enemiesToDamage[i].transform.position - transform.position).normalized);
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
