using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControlController : MonoBehaviour, IOptionItemControl
{

    public ToggleControlButtonController toggleControlButtonController;
    public event Action<string> ValueChanged;

    public bool state
    {
        get => _state;
        set {
            if (_state != value)
            {
                _state = value;
                ValueChanged?.Invoke(GetValue());
            }
            var anchoredPosition = toggleControlButtonController.GetComponent<RectTransform>().anchoredPosition;
            anchoredPosition.x = value ? _positionIfOn : 0f;
            toggleControlButtonController.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }
    }


    private bool _state;
    private float _positionIfOn;

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

    // Start is called before the first frame update
    void Start()
    {
        _positionIfOn = transform.GetComponent<RectTransform>().sizeDelta.x; 
    }

    void OnEnable()
    {
        toggleControlButtonController.MouseClick += OnMouseClick;
    }

    void OnDisable()
    {
        toggleControlButtonController.MouseClick -= OnMouseClick;
    }

    void OnMouseClick()
    {
        state = !state;
    }

    public void SetValue(string value)
    {
        state = value == true.ToString();
    }

    public string GetValue()
    {
        return state ? true.ToString() : false.ToString();
    }
}
