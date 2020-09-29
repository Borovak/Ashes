using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LifeController : MonoBehaviour
{
    public const int AcidDamage = 1;
    public float recurrentDamageDelay = 1f;
    
    public bool destroyOnDeath = true;
    public bool isInAcid => _acidWaters.Count(x => x.isAcid) > 0;
    protected abstract void AfterStart();
    protected abstract void AfterUpdate();
    protected abstract int GetMaxHp();
    protected abstract void SetMaxHp(int value);
    protected abstract int GetHp();
    protected abstract void SetHp(int value);
    public bool IsAlive => GetHp() > 0;
    public float HealthRatio => Convert.ToSingle(GetHp()) / Convert.ToSingle(GetMaxHp());
    public ParticleSystem blood;
    public Vector2 bloodOffset;

    protected bool _isPlayer;
    private InvinsibilityController _invinsibilityController;
    private bool _dead;
    private float _damageDelay;
    private List<AcidWaterController> _acidWaters = new List<AcidWaterController>();

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<InvinsibilityController>(out _invinsibilityController);
        AfterStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead) return;
        else if (!IsAlive)
        {
            _dead = true;        
            if (TryGetComponent<Animator>(out var animator))
            {
                Debug.Log($"{gameObject.name} dies");
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
                TakeDamage(AcidDamage, "Acid");
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
        AfterUpdate();
    }

    public void TakeDamage(int damage, string attackerName)
    {
        if (_invinsibilityController != null && !_invinsibilityController.TryTakeDamage()) return;
        if (blood != null){
            Instantiate(blood, transform.position + new Vector3(bloodOffset.x, bloodOffset.y, 0f), Quaternion.identity);
        }
        SetHp(GetHp() - damage);
        Debug.Log($"{attackerName} damages {gameObject.name} for {damage} damage, {GetHp()} hp remaining");
    }

    public void Heal(int value)
    {
        var newHp = System.Math.Min(GetHp() + value, GetMaxHp());
        SetHp(newHp);
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
