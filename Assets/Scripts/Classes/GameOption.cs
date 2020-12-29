using System;

public class GameOption
{

    public event Action<string> ValueChanged;
    public string id;
    public string name;
    public string value
    {
        get => _value;
        set{
            _value = value;
            ValueChanged?.Invoke(value);
            GameOptionsManager.Save(out _);
        }
    }

    private string _value;
}