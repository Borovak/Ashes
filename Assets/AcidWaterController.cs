using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidWaterController : MonoBehaviour
{
    public static Color colorWhenWater = new Color(0.4f, 0.75f, 0.75f, 1f);
    public static Color colorWhenAcid = new Color(0.4f, 0.75f, 0.4f, 1f);

    public bool isAcid;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.color = isAcid ? colorWhenAcid : colorWhenWater;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (!collider.gameObject.TryGetComponent<LifeController>(out var lifeController)) return;
        lifeController.RegisterAcidWater(this);
    }
    void OnTriggerExit2D(Collider2D collider){
        if (!collider.gameObject.TryGetComponent<LifeController>(out var lifeController)) return;
        lifeController.UnregisterAcidWater(this);
    }
}
