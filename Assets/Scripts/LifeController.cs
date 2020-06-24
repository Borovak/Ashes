using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public event Action<int> HpChanged;
    public int maxHp
    {
        get => _isPlayer ? SaveSystem.latestSaveData.MaxHp : localMaxHp;
        set
        {
            localMaxHp = value;
            if (_isPlayer)
            {
                SaveSystem.latestSaveData.MaxHp = localMaxHp;
            }
        }
    }
    public int hp
    {
        get => _isPlayer ? SaveSystem.latestSaveData.Hp : localHp;
        set
        {
            if (value > 0)
            {
                dead = false;
            }
            localHp = value;
            if (_isPlayer)
            {
                SaveSystem.latestSaveData.Hp = localHp;
            }
            HpChanged?.Invoke(value);
        }
    }

    public int localMaxHp;
    public int localHp;
    public bool destroyOnDeath = true;
    private bool _isPlayer;
    private InvinsibilityController _invinsibilityController;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        _isPlayer = gameObject.tag == "Player";
        TryGetComponent<InvinsibilityController>(out _invinsibilityController);
    }

    // Update is called once per frame
    void Update()
    {
        if (localHp != hp)
        {
            localHp = hp;
        }
        if (hp <= 0 && !dead)
        {
            dead = true;
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
