using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Xml.Linq;
using System.Collections.Generic;

public static class SaveSystem
{
    public static int index = 0;
    public static event Action<int> SaveDeleted; 
    public static event Action GameSaved;

    private static string _filePath => Application.persistentDataPath + $"/ashes_{index}.sav";

    public static bool Save(out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            //SaveBinary(filePath);
            SaveXml(filePath);
            errorMessage = "";
            GameSaved?.Invoke();
            return true;
        }
        catch (System.Exception ex)
        {
            errorMessage = ex.ToString();
            return false;
        }
    }

    private static void SaveBinary(string filePath)
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(filePath, FileMode.OpenOrCreate);
        SaveData.workingData = new SaveData();
        formatter.Serialize(stream, SaveData.workingData);
        stream.Close();
    }

    private static void SaveXml(string filePath)
    {
        SaveData.workingData = new SaveData();
        var xeRoot = new XElement("Ashes");
        xeRoot.SetAttributeValue("gameTime", SaveData.workingData.GameTime);
        var xePlayer = new XElement("Player");
        xeRoot.Add(xePlayer);
        xePlayer.SetAttributeValue("maxHp", SaveData.workingData.MaxHp);
        xePlayer.SetAttributeValue("hp", SaveData.workingData.Hp);
        xePlayer.SetAttributeValue("maxMp", SaveData.workingData.MaxMp);
        xePlayer.SetAttributeValue("mp", SaveData.workingData.Mp);
        xePlayer.SetAttributeValue("hasDoubleJump", SaveData.workingData.HasDoubleJump);
        xePlayer.SetAttributeValue("savePointGuid", SaveData.workingData.SavePointGuid);
        xePlayer.SetAttributeValue("inventory", Inventory.GetInventoryString());
        var xeGates = new XElement("Gates");
        xeRoot.Add(xeGates);
        if (SaveData.workingData.gatesId != null)
        {
            for (int i = 0; i < SaveData.workingData.gatesId.Length; i++)
            {
                var xeGate = new XElement("Gate");
                xeGates.Add(xeGate);
                xeGate.SetAttributeValue("id", SaveData.workingData.gatesId[i]);
                xeGate.SetAttributeValue("status", SaveData.workingData.gatesStatus[i]);
            }
        }
        xeRoot.Save(filePath);
    }

    public static bool Load(out SaveData data, out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            if (!File.Exists(filePath))
            {
                errorMessage = "Game file not found";
                data = null;
                return false;
            }
            //LoadBinary(filePath, out data);
            if (!LoadXml(filePath, out data))
            {
                errorMessage = "Game file load failed (XML)";
                return false;
            }
            errorMessage = "";
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.ToString();
            data = null;
            return false;
        }
    }

    private static void LoadBinary(string filePath, out SaveData data)
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_filePath, FileMode.Open);
        data = SaveData.workingData = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        SaveData.workingData.Load();
    }

    private static bool LoadXml(string filePath, out SaveData data)
    {
        data = new SaveData();
        var xeRoot = XElement.Load(filePath);
        if (!float.TryParse(xeRoot.Attribute("gameTime").Value, out data.GameTime)) return false;
        var xePlayer = xeRoot.Element("Player");
        if (!int.TryParse(xePlayer.Attribute("maxHp").Value, out data.MaxHp)) return false;
        if (!int.TryParse(xePlayer.Attribute("hp").Value, out data.Hp)) return false;
        if (!float.TryParse(xePlayer.Attribute("maxMp").Value, out data.MaxMp)) return false;
        if (!float.TryParse(xePlayer.Attribute("mp").Value, out data.Mp)) return false;
        if (!bool.TryParse(xePlayer.Attribute("hasDoubleJump").Value, out data.HasDoubleJump)) return false;
        data.SavePointGuid = xePlayer.Attribute("savePointGuid").Value ?? string.Empty;
        var inventoryString = xePlayer.Attribute("inventory").Value ?? string.Empty;
        Inventory.SetInventoryFromString(inventoryString);
        var xeGates = xeRoot.Element("Gates");
        var gatesId = new List<string>();
        var gatesStatus = new List<bool>();
        foreach (var xeGate in xeGates.Elements("Gate"))
        {

            if (xePlayer.Attribute("id") == null) return false;
            gatesId.Add(xePlayer.Attribute("id").Value);
            if (!bool.TryParse(xePlayer.Attribute("status").Value, out var status)) return false;
            gatesStatus.Add(status);
            SaveData.workingData.gatesId = gatesId.ToArray();
            SaveData.workingData.gatesStatus = gatesStatus.ToArray();
        }
        SaveData.workingData = data;
        return true;
    }

    public static void Delete(int i)
    {
        index = i;
        System.IO.File.Delete(_filePath);
        index = 0;
        SaveDeleted?.Invoke(i);
    }

    public static void WipeFiles()
    {
        for (int i = 0; i < 3; i++)
        {
            index = i;
            System.IO.File.Delete(_filePath);
        }
        index = 0;
    }
}
