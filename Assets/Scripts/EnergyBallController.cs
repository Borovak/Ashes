using System.Collections;
using System.Collections.Generic;
using Classes;
using Static;
using UnityEngine;

public class EnergyBallController : MonoBehaviour
{
    public Vector3 destination;
    public float speed;
    public float diameter;
    public float damage;
    public bool emitFromPlayer;
    public Constants.SpellElements spellElement;
    private Vector3 _direction;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;
    public Constants.SpellElements _spellElement;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(diameter, diameter, 1f);
        _direction = (destination - transform.position);
        _direction.Normalize();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();
        UpdateColors();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
        var hits = Physics2D.CircleCastAll(transform.position, diameter / 2f, Vector2.zero, 0f, emitFromPlayer ? LayerManagement.Enemies : LayerManagement.Player);
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (!hit.collider.gameObject.TryGetComponent<LifeController>(out var entity)) continue;
            entity.TakeDamage(damage, gameObject.name, hit.point);
            GameObject.Destroy(gameObject);
            return;
        }
        UpdateColors();
    }

    private void UpdateColors()
    {
        if (spellElement == _spellElement) return;
        _spellElement = spellElement;
        var color = SpellElementManager.GetColorFromSpellElement(spellElement);
        _spriteRenderer.color = color;
        var colorOverLifetime = _particleSystem.colorOverLifetime;
        var gradient = new Gradient();
        gradient.SetKeys(
            new [] {new GradientColorKey(color, 0f), new GradientColorKey(Color.white, 1f)},
            new [] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f)}
        );
        colorOverLifetime.color = gradient;
    }
}
