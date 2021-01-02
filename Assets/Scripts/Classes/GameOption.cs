using System;
using System.Collections.Generic;

public class GameOption
{

    public event Action<string> ValueChanged;
    public string id;
    public string name;
    public Type type;
    public string value
    {
        get => _init ? _value : defaultValue;
        set{
            _value = value;
            _init = true;
            ValueChanged?.Invoke(value);
            GameOptionsManager.Save(out _);
        }
    }
    public string defaultValue => new Dictionary<Type, string> {
        {typeof(float), "0"}, 
        {typeof(bool), false.ToString()}
        }[type];


    private string _value;
    private bool _init;
}