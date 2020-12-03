using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileShooterController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Vector3 offset;
    public bool seeksPlayer;
    public float visionRange;
    public Vector3 forcedDirection;
    public float projectilesPerSecond;
    public float projectileSpeed;
    public int projectileDamage;
    public float projectileDiameter;
    public bool canHitPlayer;
    public bool canHitEnemies;
    public float distance;
    public string objectSeen;

    private Transform _playerTarget;
    private float _waitDelay;
    private Vector3 _shootsFrom => transform.position + offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (seeksPlayer)
        {
            if (_playerTarget == null){
                _playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
            }
            distance = Vector3.Distance(_playerTarget.position, _shootsFrom);
            var direction = (_playerTarget.position - _shootsFrom) * 2f;
            var hitPlayer = Physics2D.Raycast(_shootsFrom, direction, visionRange, LayerManagement.Player);
            var hitTilemap = Physics2D.Raycast(_shootsFrom, direction, visionRange, LayerManagement.Layout);
            if (hitPlayer.distance > 0 && hitPlayer.distance < visionRange && (hitTilemap.distance == 0 || hitTilemap.distance > hitPlayer.distance))
            {
                TryShoot((_playerTarget.position - _shootsFrom).normalized);
            }
        }
        else
        {
            TryShoot(forcedDirection);
        }
    }

    private void TryShoot(Vector3 direction)
    {
        if (_waitDelay > 0)
        {
            _waitDelay -= Time.deltaTime;
            return;
        }
        _waitDelay = 1f / projectilesPerSecond;
        var projectile = GameObject.Instantiate<GameObject>(projectilePrefab, _shootsFrom, Quaternion.identity);
        var projectileController = projectile.GetComponent<StandardProjectileController>();
        projectileController.direction = direction;
        projectileController.speed = projectileSpeed;
        projectileController.diameter = projectileDiameter;
        projectileController.canHitEnemies = canHitEnemies;
        projectileController.canHitPlayer = canHitPlayer;
        projectileController.damage = projectileDamage;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_shootsFrom, projectileDiameter / 2f);
    }
}
