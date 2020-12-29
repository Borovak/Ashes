using System;
using UnityEngine;

public interface IOptionItemControl
{
    event Action<string> ValueChanged;
    void SetValue(string value);
    string GetValue();
}