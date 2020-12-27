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
    public bool angleRestriction;
    public float minAngle = -180f;
    public float maxAngle = 180f;
    public Vector3 shootingDirection;
    public ParticleSystem shootingParticleSystem;
    public AudioClip shootingSound;

    private Transform _playerTarget;
    private Vector3 _shootsFrom => shootsFromTransform != null ? shootsFromTransform.position : transform.position;
    private bool _targetInSight;
    private Animator _animator;
    private bool _shouldShoot;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Core")?.TryGetComponent<Animator>(out _animator);
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
            //Moving core
            if (movingCoreTransform != null)
            {
                direction = (_playerTarget.position - movingCoreTransform.position).normalized;
                var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                var restrictedAngle = angleRestriction ? GlobalFunctions.Bound(targetAngle, minAngle, maxAngle) : targetAngle;
                _targetInSight = targetAngle == restrictedAngle;
                var rotateToTarget = Quaternion.AngleAxis(restrictedAngle, Vector3.forward);
                movingCoreTransform.localRotation = Quaternion.Slerp(movingCoreTransform.localRotation, rotateToTarget, Time.deltaTime * 5f);
            }
            else
            {
                _targetInSight = true;
            }
            //Check if shooting possible
            if (hitPlayer.distance > 0 && hitPlayer.distance < visionRange && (hitTilemap.distance == 0 || hitTilemap.distance > hitPlayer.distance) && _targetInSight)
            {
                shootingDirection = (_playerTarget.position - _shootsFrom).normalized;
                _shouldShoot = true;
            }
            else
            {
                _shouldShoot = false;
            }
        }
        else
        {
            shootingDirection = forcedDirection;
            _shouldShoot = true;
        }
        if (_animator != null)
        {
            _animator.SetBool("targetInSight", _targetInSight);
            _animator.SetFloat("chargeSpeed", projectilesPerSecond);
            _animator.SetBool("shoot", _shouldShoot);
        }
    }

    public void Shoot()
    {
        var projectile = GameObject.Instantiate<GameObject>(projectilePrefab, _shootsFrom, Quaternion.identity);
        var projectileController = projectile.GetComponent<StandardProjectileController>();
        projectileController.direction = shootingDirection;
        projectileController.speed = projectileSpeed;
        projectileController.diameter = projectileDiameter;
        projectileController.canHitEnemies = canHitEnemies;
        projectileController.canHitPlayer = canHitPlayer;
        projectileController.damage = projectileDamage;
        projectileController.target = _playerTarget;
        projectileController.isHoming = homingProjectiles;
        shootingParticleSystem.Play();
        GlobalFunctions.PlaySound(shootingSound, _shootsFrom);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_shootsFrom, projectileDiameter / 2f);
    }
}
