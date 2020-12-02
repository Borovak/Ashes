using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooterController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Vector3 shootsFrom;
    public bool seeksPlayer;
    public float visionRange;
    public Vector3 forcedDirection;
    public float projectilesPerSecond;
    public float projectileSpeed;
    public int projectileDamage;
    public bool canHitPlayer;
    public bool canHitEnemies;

    private float _waitDelay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!seeksPlayer){
            TryShoot(forcedDirection);
        } else {
            var playerPosition = GlobalFunctions.GetPlayerPlatformerController().transform.position;
            var distance = Vector3.Distance(playerPosition, transform.position);
            if (distance < visionRange){
                TryShoot((playerPosition - transform.position).normalized);
            }
        }
    }

    private void TryShoot(Vector3 direction){
        if (_waitDelay > 0){
            _waitDelay -= Time.deltaTime;
            return;
        }
        _waitDelay = 1f / projectilesPerSecond;
        var projectile = GameObject.Instantiate<GameObject>(projectilePrefab, shootsFrom, Quaternion.identity);
        var projectileController = projectile.GetComponent<StandardProjectileController>();
        projectileController.direction = direction;
        projectileController.speed = projectileSpeed;
        projectileController.canHitEnemies = canHitEnemies;
        projectileController.canHitPlayer = canHitPlayer;
        projectileController.damage = projectileDamage;

    }
}
