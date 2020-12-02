using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalFireball : MonoBehaviour
{
    public Vector3 destination;
    public float speed;
    public float diameter;
    public int damage;
    public bool emitFromPlayer;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(diameter, diameter, 1f);
        _direction = (destination - transform.position);
        _direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
        var entitiesToDamage = Physics2D.OverlapCircleAll(transform.position, diameter / 2f, emitFromPlayer ? LayerManagement.Enemies : LayerManagement.Player);
        for (int i = 0; i < entitiesToDamage.Length; i++)
        {
            if (!entitiesToDamage[i].TryGetComponent<LifeController>(out var entity)) continue;
            entity.TakeDamage(damage, gameObject.name);
            GameObject.Destroy(gameObject);
            return;
        }
    }
}
