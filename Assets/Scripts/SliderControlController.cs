using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class SliderControlController : MonoBehaviour, IOptionItemControl
{

    public Slider slider;
    public event Action<string> ValueChanged;

    public float state
    {
        get => slider.value;
        set
        {
            if (!_init)
            {
                _init = true;
                slider.minValue = 0f;
                slider.maxValue = 100f;
            }
            slider.value = value;
            ValueChanged?.Invoke(GetValue());
        }
    }

    private float _positionIfOn;
    private bool _init;

    event Action<string> IOptionItemControl.ValueChanged
    {
        add
        {
            ValueChanged += value;
        }

        remove
        {
            ValueChanged -= value;
        }
    }

    public void OnSliderValueChanged()
    {
        state = slider.value;
    }

    public void SetValue(string value)
    {
        state = Convert.ToSingle(value);
    }

    public string GetValue()
    {
        return state.ToString();
    }
}
