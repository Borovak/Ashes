using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int MaxHp;
    public bool HasDoubleJump;
    public float[] CampsiteLocation;
    public int Hp;
    public string[] gatesId;
    public bool[] gatesStatus;
    public float GameTime;

    public SaveData()
    {
        //Player
        var lifeController = PlayerPlatformerController.Instance.GetComponent<LifeController>();
        MaxHp = lifeController.maxHp;
        Hp = lifeController.hp;
        HasDoubleJump = PlayerPlatformerController.Instance.hasDoubleJump;
        var campsiteLocation = PlayerPlatformerController.Instance.campsiteLocation;
        CampsiteLocation = new float[] { campsiteLocation.x, campsiteLocation.y, campsiteLocation.z };
        GameTime = PlayerPlatformerController.Instance.gameTime;
        //World
        var count = GateController.gates?.Count ?? 0;
        if (count > 0)
        {
            gatesId = new string[count];
            gatesStatus = new bool[count];
            var i = 0;
            foreach (var item in GateController.gates)
            {
                gatesId[i] = item.Key;
                gatesStatus[i] = item.Value;
                i++;
            }
        }
    }

    public void Load()
    {
        //World
        if (GateController.gates == null)
        {
            GateController.gates = new Dictionary<string, bool>();
            for (int i = 0; i < gatesId.Length; i++)
            {
                if (!GateController.gates.ContainsKey(gatesId[i]))
                {
                    GateController.gates.Add(gatesId[i], gatesStatus[i]);
                }
            }
        }
    }
}