using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    const int defaultHp = 3;
    const int defaultMp = 30;

    public static SaveData workingData;
    public int MaxHp;
    public int Hp;
    public float MaxMp;
    public float Mp;
    public bool HasDoubleJump;
    public string SavePointGuid;
    public string[] gatesId;
    public bool[] gatesStatus;
    public float GameTime;

    public SaveData()
    {
        //Player
        MaxHp = workingData?.MaxHp ?? defaultHp;
        Hp = workingData?.Hp ?? defaultHp;
        MaxMp = workingData?.MaxMp ?? defaultMp;
        Mp = workingData?.Mp ?? defaultMp;
        HasDoubleJump = workingData?.HasDoubleJump ?? false;
        SavePointGuid = workingData?.SavePointGuid ?? string.Empty;
        GameTime = workingData?.GameTime ?? 0;
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
            if (gatesId == null) return;
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