using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectileController : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public int damage;
    public float diameter = 1f;
    public float angle;
    public bool canHitPlayer;
    public bool canHitEnemies;
    public Transform particleSystemTransform;
    public GameObject splashPrefab;
    public Transform target;

    private List<LayerMask> _layerMaskToConsider;

    // Start is called before the first frame update
    void Start()
    {
        _layerMaskToConsider = new List<LayerMask>();
        if (canHitPlayer)
        {
            _layerMaskToConsider.Add(LayerManagement.Player);
        }
        if (canHitEnemies)
        {
            _layerMaskToConsider.Add(LayerManagement.Enemies);
        }
        transform.localScale = new Vector3(diameter, diameter, 1f);
        UpdateAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {            
            direction = (target.position - transform.position).normalized;
            UpdateAngle();
        }
        transform.Translate(direction * speed * Time.deltaTime);
        foreach (var layer in _layerMaskToConsider)
        {
            var entitiesToDamage = Physics2D.OverlapCircleAll(transform.position, diameter / 2f, layer);
            for (int i = 0; i < entitiesToDamage.Length; i++)
            {
                if (!entitiesToDamage[i].TryGetComponent<LifeController>(out var entity)) continue;
                entity.TakeDamage(damage, gameObject.name);
                if (splashPrefab != null)
                {
                    var splash = GameObject.Instantiate<GameObject>(splashPrefab, transform.position, Quaternion.identity);
                    GameObject.Destroy(splash, 2f);
                }
                GameObject.Destroy(gameObject);
                return;
            }
        }
    }

    private void UpdateAngle(){        
        angle = GlobalFunctions.GetAngleFromPoints(Vector3.zero, direction);
        if (particleSystemTransform != null)
        {
            particleSystemTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}
