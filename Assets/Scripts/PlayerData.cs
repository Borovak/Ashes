using System;
using System.Diagnostics;

[Serializable]
public class PlayerData
{
    public event Action<int> HpChanged;
    public int MaxHp;
    public int Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            HpChanged?.Invoke(value);
        }
    }
    public bool HasDoubleJump;

    private int _hp;

    public PlayerData(int maxHp, int hp, bool hasDoubleJump)
    {
        MaxHp = maxHp;
        Hp = hp;
        HasDoubleJump = hasDoubleJump;
    }
}