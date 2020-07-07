using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPos;
    public float attackRange;
    public float attackDamage;
    public float attackRate;
    public LayerMask whatIsEnemies;
    public GameObject fireballPrefab;

    private float _attackCooldown;
    private Animator _animator;
    private AudioSource _audioSource;
    private PlayerInputs _inputs;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _inputs = GetComponent<PlayerInputs>();
        _inputs.Attack += Attack;
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
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
        Debug.Log("Attack");
        if (_attackCooldown > 0) return;
        _animator.SetTrigger("attack");
        MeleeAttack();
        _attackCooldown = 1f / attackRate;
    }

    private void MeleeAttack()
    {
        var enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (!enemiesToDamage[i].TryGetComponent<Enemy>(out var enemy)) continue;
            enemy.TakeDamage(attackDamage, (enemiesToDamage[i].transform.position - attackPos.position).normalized);
        }
    }

    void CastFireball()
    {
        var fireballObject = Instantiate(fireballPrefab, attackPos.position, Quaternion.identity);
        var fireball = fireballObject.GetComponent<DirectionalFireball>();
        fireball.speed = 20f;
        fireball.damage = 3;
        fireball.diameter = 0.3f;
        fireball.destination = attackPos.transform.position.x > transform.position.x ? attackPos.transform.position + Vector3.right * 1000f : attackPos.transform.position + Vector3.left * 1000f;
        fireball.emitFromPlayer = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
