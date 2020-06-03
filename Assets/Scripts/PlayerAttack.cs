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
    public GameObject spellMagicMissile;

    private float _attackCooldown;
    private Animator _animator;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackCooldown <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _animator.SetTrigger("attack");
                Instantiate(spellMagicMissile, attackPos.position, Quaternion.identity);
                var enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (!enemiesToDamage[i].TryGetComponent<Enemy>(out var enemy)) continue;
                    enemy.TakeDamage(attackDamage, (enemiesToDamage[i].transform.position - attackPos.position).normalized);
                }
                _attackCooldown = 1f / attackRate;
            }
        }
        else
        {
            _animator.SetBool("attack", false);
            _attackCooldown -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
