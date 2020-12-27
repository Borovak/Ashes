using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileShooterController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public bool aimsAtPlayer;
    public bool homingProjectiles;
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
    public Transform movingCoreTransform;
    public Transform shootsFromTransform;
    public float minAngle = -180f;
    public float maxAngle = 180f;

    private Transform _playerTarget;
    private float _waitDelay;
    private Vector3 _shootsFrom => shootsFromTransform != null ? shootsFromTransform.position : transform.position;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (aimsAtPlayer)
        {
            if (_playerTarget == null)
            {
                _playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
            }
            distance = Vector3.Distance(_playerTarget.position, _shootsFrom);
            var direction = _playerTarget.position - _shootsFrom;
            var hitPlayer = Physics2D.Raycast(_shootsFrom, direction, visionRange, LayerManagement.Player);
            var hitTilemap = Physics2D.Raycast(_shootsFrom, direction, visionRange, LayerManagement.Layout);
            if (hitPlayer.distance > 0 && hitPlayer.distance < visionRange && (hitTilemap.distance == 0 || hitTilemap.distance > hitPlayer.distance))
            {
                TryShoot((_playerTarget.position - _shootsFrom).normalized, _playerTarget);
            }
            if (movingCoreTransform != null)
            {
                direction = (_playerTarget.position - movingCoreTransform.position).normalized;
                var a = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                a = GlobalFunctions.Bound(a, minAngle, maxAngle);
                Debug.Log(a);
                var rotateToTarget = Quaternion.AngleAxis(a, Vector3.forward);
                movingCoreTransform.localRotation = Quaternion.Slerp(movingCoreTransform.localRotation, rotateToTarget, Time.deltaTime * 5f);
            }
        }
        else
        {
            TryShoot(forcedDirection);
        }
    }

    private void TryShoot(Vector3 direction, Transform target = null)
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
        projectileController.target = homingProjectiles ? target : null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_shootsFrom, projectileDiameter / 2f);
    }
}
