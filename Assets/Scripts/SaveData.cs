using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string ZoneName;
    public int MaxHp;
    public int Hp;
    public float MaxMp;
    public float Mp;
    public bool HasDoubleJump;
    public float[] CampsiteLocation;
    public string[] gatesId;
    public bool[] gatesStatus;
    public float GameTime;

    public SaveData()
    {
        //Player
        var lifeController = PlayerPlatformerController.Instance.GetComponent<LifeController>();
        MaxHp = lifeController.maxHp;
        Hp = lifeController.hp;
        var manaController = PlayerPlatformerController.Instance.GetComponent<ManaController>();
        MaxMp = manaController.maxMp;
        Mp = manaController.maxMp;
        HasDoubleJump = PlayerPlatformerController.Instance.hasDoubleJump;
        var campsiteLocation = PlayerPlatformerController.Instance.campsiteLocation;
        CampsiteLocation = new float[] { campsiteLocation.x, campsiteLocation.y, campsiteLocation.z };
        GameTime = PlayerPlatformerController.Instance.gameTime;
        //World
        ZoneName = RegionAnnouncementController.LastRegionVisited;
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