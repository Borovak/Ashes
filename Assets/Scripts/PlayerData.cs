using System;
using System.Diagnostics;

[Serializable]
public class PlayerData
{
    public int MaxHp;
    public bool HasDoubleJump;
    public float[] CampsiteLocation;
    public int Hp;

    public PlayerData(PlayerPlatformerController player)
    {
        MaxHp = player.maxHp;
        Hp = player.hp;
        HasDoubleJump = player.hasDoubleJump;
        CampsiteLocation = new float[] { player.campsiteLocation.x, player.campsiteLocation.y, player.campsiteLocation.z };
    }
}