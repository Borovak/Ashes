using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidWaterController : MonoBehaviour
{
    public static Color colorWhenWater = new Color(0.4f, 0.75f, 0.75f, 1f);
    public static Color colorWhenAcid = new Color(0.4f, 0.75f, 0.4f, 1f);

    public int damage = 1;    
    public float recurrentDamageDelay = 1f;
    public bool isAcid;
    private LifeController _playerLifeController; 

    private SpriteRenderer _spriteRenderer;
    private float _damageDelay;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.color = isAcid ? colorWhenAcid : colorWhenWater;
        if (isAcid && _playerLifeController != null)
        {
            if (_damageDelay <= 0)
            {
                _playerLifeController.TakeDamage(damage);
                _damageDelay = recurrentDamageDelay;
            } 
            else 
            {
                _damageDelay -= Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (!collider.gameObject.TryGetComponent<LifeController>(out var player)) return;
        _playerLifeController = player;
        _damageDelay = 0;  
    }
    void OnTriggerExit2D(Collider2D collider){
        if (!collider.gameObject.TryGetComponent<LifeController>(out _)) return;
        _playerLifeController = null;
    }
}
