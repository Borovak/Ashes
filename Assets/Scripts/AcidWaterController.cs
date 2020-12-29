using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AcidWaterController : MonoBehaviour
{
    const string TextureName = "Texture2D_78F4DEAC";
    const int resolution = 128;

    public Color colorWhenWater = new Color(0.4f, 0.75f, 0.75f, 1f);
    public Color colorWhenAcid = new Color(0.3f, 0.9f, 0.3f, 1f);
    public bool isAcid;
    public Light2D light2D;

    private GameObject _reflectionObject;
    private SpriteRenderer _spriteRendererBack;
    private SpriteRenderer _spriteRendererReflection;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRendererBack = GetComponent<SpriteRenderer>();
        //adjust camera size
        var camera = GetComponentInChildren<Camera>();
        if (camera != null)
        {
            camera.orthographicSize = 0.5f * transform.localScale.y;
        }
        //Adjust reflection texture size
        _reflectionObject = gameObject.transform.Find("Reflection")?.gameObject;
        if (_reflectionObject != null)
        {
            var ratio = transform.localScale.x / transform.localScale.y;
            var w = Convert.ToInt32(ratio > 1f ? resolution * ratio : resolution);
            var h = Convert.ToInt32(ratio < 1f ? resolution / ratio : resolution);
            if (camera.targetTexture != null)
            {
                camera.targetTexture.Release();
            }
            camera.targetTexture = new RenderTexture(w, h, 24);
            _spriteRendererReflection = _reflectionObject.GetComponent<SpriteRenderer>();
            _spriteRendererReflection.material.SetTexture(TextureName, camera.targetTexture);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRendererBack.color = isAcid ? colorWhenAcid : colorWhenWater;
        if (_spriteRendererReflection != null)
        {
            _spriteRendererReflection.color = isAcid ? colorWhenAcid : colorWhenWater;
            light2D.color = isAcid ? colorWhenAcid : Color.white;
            light2D.intensity = isAcid ? 1f : 0.2f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.TryGetComponent<LifeController>(out var lifeController)) return;
        lifeController.RegisterAcidWater(this);
        _reflectionObject.SetActive(false);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.gameObject.TryGetComponent<LifeController>(out var lifeController)) return;
        lifeController.UnregisterAcidWater(this);
        _reflectionObject.SetActive(true);
    }
}
