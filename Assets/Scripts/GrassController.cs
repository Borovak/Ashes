using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrassController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private Animator _animator;
    private float _totalCooldown = 0.5f;
    private float _cooldownRemaining;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _cooldownRemaining -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player") || _cooldownRemaining > 0f) return;
        _cooldownRemaining = _totalCooldown;
        _animator.SetTrigger("moving");
    }
}
