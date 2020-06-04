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
    public int CampsiteId;

    private int _hp;

    public PlayerData(int maxHp, int hp, bool hasDoubleJump, int campsiteId)
    {
        MaxHp = maxHp;
        Hp = hp;
        HasDoubleJump = hasDoubleJump;
        CampsiteId = campsiteId;
    }
}