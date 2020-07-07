using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public event Action<int> HpChanged;
    public int maxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
        }
    }
    public int hp
    {
        get => _localHp;
        set
        {
            if (value > 0)
            {
                _dead = false;
            }
            _localHp = value;
            HpChanged?.Invoke(value);
        }
    }
    public bool destroyOnDeath = true;
    private bool _isPlayer;
    private InvinsibilityController _invinsibilityController;
    private bool _dead;

    private int _maxHp;
    private int _localHp;

    // Start is called before the first frame update
    void Start()
    {
        _isPlayer = gameObject.tag == "Player";
        if (_isPlayer)
        {
            hp = SaveSystem.latestSaveData?.Hp ?? 3;
            maxHp = SaveSystem.latestSaveData?.MaxHp ?? 3;
        }
        TryGetComponent<InvinsibilityController>(out _invinsibilityController);
    }

    // Update is called once per frame
    void Update()
    {
        if (_localHp != hp)
        {
            _localHp = hp;
        }
        if (hp <= 0 && !_dead)
        {
            _dead = true;
            if (destroyOnDeath)
            {
                GameObject.Destroy(gameObject);
                return;
            }
            else if (TryGetComponent<Animator>(out var animator))
            {
                animator.SetBool("death", true);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_invinsibilityController != null && !_invinsibilityController.TryTakeDamage()) return;
        hp -= damage;
    }
}
