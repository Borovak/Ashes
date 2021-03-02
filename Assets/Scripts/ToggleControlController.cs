using System;
using Interfaces;
using TMPro;
using UnityEngine;

public class ToggleControlController : MonoBehaviour, IOptionItemControl
{

    public TextMeshProUGUI textControl;
    public GameObject changeLeft;
    public GameObject changeRight;
    public event Action<string> ValueChanged;

    private bool State
    {
        get => _state;
        set {
            if (_state != value)
            {
                _state = value;
                ValueChanged?.Invoke(GetValue());
            }
            changeLeft.SetActive(_state);
            changeRight.SetActive(!_state);
            textControl.text = _state ? "Yes" : "No";
        }
    }

    private bool _state;

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

    void OnEnable()
    {
        changeLeft.GetComponent<ClickableController>().Clicked += OnDLeftPressed;
        changeRight.GetComponent<ClickableController>().Clicked += OnDRightPressed;
    }

    void OnDisable()
    {
        changeLeft.GetComponent<ClickableController>().Clicked -= OnDLeftPressed;
        changeRight.GetComponent<ClickableController>().Clicked -= OnDRightPressed;
    }

    public void SetValue(string value)
    {
        State = value == true.ToString();
    }

    public string GetValue()
    {
        return State ? true.ToString() : false.ToString();
    }

    public void OnDLeftPressed()
    {
        State = false;
    }

    public void OnDRightPressed()
    {
        State = true;
    }
}
