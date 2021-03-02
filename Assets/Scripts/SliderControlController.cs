using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Static;
using UnityEngine;
using UnityEngine.UI;

public class SliderControlController : MonoBehaviour, IOptionItemControl
{
    private const float Step = 10f;
    
    public Slider slider;
    public event Action<string> ValueChanged;
    public GameObject changeLeft;
    public GameObject changeRight;

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
            changeLeft.SetActive(state > slider.minValue);
            changeRight.SetActive(state < slider.maxValue);
            ValueChanged?.Invoke(GetValue());
        }
    }

    private float _positionIfOn;
    private bool _init;

    private void OnEnable()
    {
        changeLeft.GetComponent<ClickableController>().Clicked += OnDLeftPressed;
        changeRight.GetComponent<ClickableController>().Clicked += OnDRightPressed;
    }
    
    private void OnDisable()
    {
        changeLeft.GetComponent<ClickableController>().Clicked -= OnDLeftPressed;
        changeRight.GetComponent<ClickableController>().Clicked -= OnDRightPressed;
    }

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

    public void OnDLeftPressed()
    {
        state = GlobalFunctions.Bound(state - Step, slider.minValue, slider.maxValue);
    }

    public void OnDRightPressed()
    {
        state = GlobalFunctions.Bound(state + Step,  slider.minValue, slider.maxValue);
    }
}
