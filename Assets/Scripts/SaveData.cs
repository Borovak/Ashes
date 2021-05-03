using System;
using System.Collections.Generic;
using System.Xml.Linq;

[Serializable]
public class SaveData
{
    const float defaultHp = 30f;
    const float defaultMp = 30f;
    const float defaultMpRegenPerSec = 1f;

    public float MaxHp;
    public float Hp;
    public float MaxMp;
    public float Mp;
    public float MpRegenPerSec;
    public bool HasDoubleJump;
    public string SavePointGuid;
    public string[] gatesId;
    public bool[] gatesStatus;
    public float GameTime;
    public XElement XeInventories;

    public SaveData()
    {
        //Player
        MaxHp = defaultHp;
        Hp = defaultHp;
        MaxMp = defaultMp;
        Mp = defaultMp;
        MpRegenPerSec = defaultMpRegenPerSec;
        HasDoubleJump = false;
        SavePointGuid = string.Empty;
        XeInventories = new XElement("Inventories");
        GameTime = 0;
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