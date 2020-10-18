using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LifeController : MonoBehaviour
{
    private class SpriteColor
    {
        internal SpriteRenderer spriteRenderer;
        internal Color defaultColor;
    }

    public const int AcidDamage = 1;
    public const float _damageFlashTimeTotal = 1f;
    public const float _damageFlashRate = 0.5f;
    public Color _damageColor = Color.red;

    public float recurrentDamageDelay = 1f;    
    public bool destroyOnDeath = true;
    public bool isInAcid => _acidWaters.Count(x => x.isAcid) > 0;
    protected abstract void AfterStart();
    protected abstract void AfterUpdate();
    protected abstract int GetMaxHp();
    protected abstract void SetMaxHp(int value);
    protected abstract int GetHp();
    protected abstract void SetHp(int value);
    protected abstract void OnDeath();
    public bool IsAlive => GetHp() > 0;
    public float HealthRatio => Convert.ToSingle(GetHp()) / Convert.ToSingle(GetMaxHp());
    public ParticleSystem blood;
    public Vector2 bloodOffset;

    protected bool _isPlayer;
    private InvinsibilityController _invinsibilityController;
    private bool _dead;
    private float _damageDelay;
    private List<AcidWaterController> _acidWaters = new List<AcidWaterController>();
    private float _damageFlashTimeRemaining;
    private List<SpriteColor> _spriteColors;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<InvinsibilityController>(out _invinsibilityController);
        AfterStart();
        _spriteColors = new List<SpriteColor>();
        GetSpriteRendererRecursively(ref _spriteColors, transform);
    }

    private void GetSpriteRendererRecursively(ref List<SpriteColor> spriteColors, Transform childTransform)
    {

        if (childTransform.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            _spriteColors.Add(new SpriteColor { spriteRenderer = spriteRenderer, defaultColor = spriteRenderer.color });
        }
        for (int i = 0; i < childTransform.childCount; i++)
        {
            GetSpriteRendererRecursively(ref spriteColors, childTransform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead) return;
        else if (!IsAlive)
        {
            _dead = true;  
            OnDeath();   
            Debug.Log($"{gameObject.name} dies");   
            if (TryGetComponent<Animator>(out var animator))
            {
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
        if (_damageFlashTimeRemaining > 0){
            var flashRatio = _damageFlashTimeRemaining % _damageFlashRate / _damageFlashRate;
            foreach (var spriteColor in _spriteColors)
            {
                var c1 = flashRatio > 0.5f ? _damageColor : spriteColor.defaultColor;
                var c2 = flashRatio > 0.5f ? spriteColor.defaultColor : _damageColor;
                var c = Color.Lerp(c1, c2, flashRatio);
                spriteColor.spriteRenderer.color = c;
            }
            _damageFlashTimeRemaining -= Time.deltaTime;
        }
        AfterUpdate();
    }

    public void TakeDamage(int damage, string attackerName)
    {
        if (_invinsibilityController != null && !_invinsibilityController.TryTakeDamage()) return;
        if (blood != null){
            Instantiate(blood, transform.position + new Vector3(bloodOffset.x, bloodOffset.y, 0f), Quaternion.identity, transform);
        }
        SetHp(GetHp() - damage);
        _damageFlashTimeRemaining = _damageFlashTimeTotal;
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
