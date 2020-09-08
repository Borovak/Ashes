using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public const int AcidDamage = 1;
    public float recurrentDamageDelay = 1f;
    
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
            Debug.Log($"HP changed to {value}");
            if (value > 0)
            {
                _dead = false;
            }
            _localHp = value;
            if (_isPlayer){
                SaveData.workingData.Hp = _localHp;
            }
            HpChanged?.Invoke(value);
        }
    }
    public bool destroyOnDeath = true;
    public bool isInAcid => _acidWaters.Count(x => x.isAcid) > 0;

    private bool _isPlayer;
    private InvinsibilityController _invinsibilityController;
    private bool _dead;
    private int _maxHp = 3;
    private int _localHp = 3;
    private float _damageDelay;
    private List<AcidWaterController> _acidWaters = new List<AcidWaterController>();

    // Start is called before the first frame update
    void Start()
    {
        _isPlayer = gameObject.tag == "Player";
        if (_isPlayer)
        {
            hp = SaveData.workingData.Hp;
            maxHp = SaveData.workingData.MaxHp;
        }
        TryGetComponent<InvinsibilityController>(out _invinsibilityController);
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead) return;
        else if (hp <= 0)
        {
            _dead = true;        
            if (TryGetComponent<Animator>(out var animator))
            {
                Debug.Log("Death triggered");
                animator.SetTrigger("dying");
            } 
            else if (destroyOnDeath)
            {            
                GameObject.Destroy(gameObject);
            }
            return;
        }
        else if (isInAcid)
        {
            if (_damageDelay <= 0)
            {
                TakeDamage(AcidDamage);
                _damageDelay = recurrentDamageDelay;
            } 
            else 
            {
                _damageDelay -= Time.deltaTime;
            }
        } 
        else 
        {
            _damageDelay = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (_invinsibilityController != null && !_invinsibilityController.TryTakeDamage()) return;
        hp -= damage;
    }

    public void Heal(int value)
    {
        hp = System.Math.Min(hp + value, maxHp);
    }

    public void RegisterAcidWater(AcidWaterController acidWater)
    {
        if (!_acidWaters.Contains(acidWater)){
            _acidWaters.Add(acidWater);
        }
    }

    public void UnregisterAcidWater(AcidWaterController acidWater)
    {
        if (_acidWaters.Contains(acidWater)){
            _acidWaters.Remove(acidWater);
        }
    }
}
