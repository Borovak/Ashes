using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlickeringColorController : MonoBehaviour
{
    private enum Channels
    {
        r,g,b
    }
    
    public Color colorA;
    public Color colorB;
    public float period = 0.5f;
    public Light2D light2D;

    private bool _isDisabled;
    private SpriteRenderer _spriteRenderer;
    
    void OnEnable()
    {
        _isDisabled = false;
        ChangeColor();
    }
    
    void OnDisable()
    {
        _isDisabled = true;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (light2D != null)
        {
            light2D.color = _spriteRenderer.color;
        }
    }

    private void ChangeColor()
    {
        if (_isDisabled) return;
        var color = new Color(GetRandomChannel(Channels.r), GetRandomChannel(Channels.g), GetRandomChannel(Channels.b));
        var lt = LeanTween.Framework.LeanTween.color(gameObject, color, period);
        lt.setOnComplete(ChangeColor);
    }

    private float GetRandomChannel(Channels channel)
    {
        float min;
        float max;
        switch (channel)
        {
            case Channels.r:
                min = Math.Min(colorA.r, colorB.r);
                max = Math.Max(colorA.r, colorB.r);
                break;
            case Channels.g:
                min = Math.Min(colorA.g, colorB.g);
                max = Math.Max(colorA.g, colorB.g);
                break;
            case Channels.b:
                min = Math.Min(colorA.b, colorB.b);
                max = Math.Max(colorA.b, colorB.b);
                break;
            default:
                return 1f;
        }
        var rng = UnityEngine.Random.Range(0f, 1f);
        var value = rng * (max - min) + min;
        //Debug.Log($"{channel.ToString()}: {value}, {rng}, {min}, {max}");
        return value;
    }
}
