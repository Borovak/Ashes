using System;
using System.Diagnostics;

[Serializable]
public class WorldData
{
    public string[] gatesId;
    public bool[] gatesStatus;

    public WorldData()
    {
        var count = GateController.gates.Count;
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