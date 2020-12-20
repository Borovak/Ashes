using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerAttack : MonoBehaviour
{
    private enum AttackStates
    {
        Idle,
        Charging,
        Attack
    }

    public SpriteRenderer attackSpriteRenderer;
    public float attackRange;
    public int attackDamage;
    public float attackRate;
    public LayerMask whatIsEnemies;
    public GameObject fireballPrefab;
    public float chargingDuration = 0.05f;
    public float attackDuration = 0.05f;
    public Light2D attackLight;

    private float _attackCooldown;
    private Animator _animator;
    private AudioSource _audioSource;
    private PlayerInputs _inputs;
    private ManaController _manaController;
    private LifeController _lifeController;
    private AttackStates _attackState;
    private float _attackStateTimeRemaining;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _inputs = GetComponent<PlayerInputs>();
        _manaController = GetComponent<ManaController>();
        _lifeController = GetComponent<LifeController>();
        _inputs.Attack += Attack;
        _inputs.AttackSpell += AttackSpell;
        _inputs.SelfSpell += SelfSpell;
        _inputs.GroundBreak += GroundBreak;
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
        attackSpriteRenderer.enabled = _attackState == AttackStates.Attack;
        attackLight.enabled = _attackState == AttackStates.Attack;
        if (_attackState != AttackStates.Idle)
        {
            _attackStateTimeRemaining -= Time.deltaTime;
            if (_attackStateTimeRemaining <= 0)
            {
                switch (_attackState)
                {
                    case AttackStates.Charging:
                        _attackState = AttackStates.Attack;
                        _attackStateTimeRemaining = attackDuration;
                        break;
                    case AttackStates.Attack:
                        _attackState = AttackStates.Idle;
                        break;
                }
            }
        }

        // if (_attackCooldown <= 0)
        // {
        //     if (Input.GetButtonDown("Fire1"))
        //     {
        //         _animator.SetBool("attack", true);
        //         MeleeAttack();
        //         _attackCooldown = 1f / attackRate;
        //     }
        //     // else if (Input.GetButtonDown("Fire2"))
        //     // {
        //     //     _animator.SetTrigger("attack");
        //     //     _attackCooldown = 1f / attackRate;
        //     // }
        // }
        // else
        // {
        //     _animator.SetBool("attack", false);
        //     _attackCooldown -= Time.deltaTime;
        // }
    }

    private void Attack()
    {
        if (_attackCooldown > 0) return;
        _animator.SetTrigger("attack");
        MeleeAttack();
        _attackCooldown = 1f / attackRate;
        _attackState = AttackStates.Charging;
        _attackStateTimeRemaining = chargingDuration;
    }

    private void AttackSpell()
    {
        if (_attackCooldown > 0) return;
        if (!_manaController.TryCastSpell(3f)) return;
        _animator.SetTrigger("fireball");
        _attackCooldown = 1f / attackRate;
    }

    private void SelfSpell()
    {
        if (_attackCooldown > 0) return;
        if (!_manaController.TryCastSpell(5f)) return;
        _animator.SetTrigger("heal");
        _lifeController.Heal(1);
        _attackCooldown = 1f / attackRate;
    }

    private void GroundBreak()
    {
        if (_attackCooldown > 0) return;
        if (!_manaController.TryCastSpell(5f)) return;
        _animator.SetTrigger("groundBreak");
        _attackCooldown = 1f / attackRate;
    }


    private void MeleeAttack()
    {
        var hits = Physics2D.CircleCastAll(attackSpriteRenderer.transform.position, attackRange, Vector2.zero, 0f, whatIsEnemies);
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];            
            if (!hit.collider.gameObject.TryGetComponent<EnemyLifeController>(out var enemy)) continue;
            enemy.TakeDamage(attackDamage, gameObject.name, hit.point);
        }
    }

    void CastFireball()
    {
        var fireballObject = Instantiate(fireballPrefab, attackSpriteRenderer.transform.position, Quaternion.identity);
        var fireball = fireballObject.GetComponent<DirectionalFireball>();
        fireball.speed = 20f;
        fireball.damage = 3;
        fireball.diameter = 0.3f;
        fireball.destination = attackSpriteRenderer.transform.transform.position.x > transform.position.x ? attackSpriteRenderer.transform.transform.position + Vector3.right * 1000f : attackSpriteRenderer.transform.transform.position + Vector3.left * 1000f;
        fireball.emitFromPlayer = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackSpriteRenderer.transform.position, attackRange);
    }
}
