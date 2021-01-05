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
    public static event Action<bool> GameSaved;
    public static SaveData LastLoadedSave;

    private static string _filePath => Application.persistentDataPath + $"/ashes_{index}.sav";

    public static bool Save(string savePointGuid, bool healOnSave, out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            var saveData = PrepareSaveData(false, savePointGuid, healOnSave);
            //SaveBinary(filePath);
            SaveXml(filePath, saveData);
            errorMessage = "";
            GameSaved?.Invoke(healOnSave);
            return true;
        }
        catch (System.Exception ex)
        {
            errorMessage = ex.ToString();
            return false;
        }
    }

    public static bool SaveVirgin(out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            var saveData = PrepareSaveData(true, "", false);
            //SaveBinary(filePath);
            SaveXml(filePath, saveData);
            errorMessage = "";
            GameSaved?.Invoke(false);
            return true;
        }
        catch (System.Exception ex)
        {
            errorMessage = ex.ToString();
            return false;
        }
    }

    private static SaveData PrepareSaveData(bool isVirginSave, string savePointGuid, bool healOnSave){
        var saveData = new SaveData();
        if (isVirginSave) return saveData;
        var gameController = GlobalFunctions.GetGameController();
        var playerPlatformerController = GlobalFunctions.GetPlayerPlatformerController();
        var playerLifeController = GlobalFunctions.GetPlayerLifeController();
        var playerManaController = GlobalFunctions.GetPlayerManaController();
        var playerInventory = GlobalFunctions.GetPlayerInventory();
        saveData.GameTime = gameController.gameTime;
        saveData.HasDoubleJump = playerPlatformerController.hasDoubleJump;
        saveData.Hp = healOnSave ? playerLifeController.GetMaxHp() : playerLifeController.GetHp();
        saveData.MaxMp = playerLifeController.GetMaxHp();
        saveData.Mp = healOnSave ? playerManaController.GetMaxMp() : playerManaController.GetMp();
        saveData.MaxMp = playerManaController.GetMaxMp();
        saveData.MpRegenPerSec = playerManaController.mpRegenPerSec;
        saveData.Inventory = playerInventory.GetInventoryString();
        saveData.SavePointGuid = savePointGuid;
        return saveData;
    }

    // private static void SaveBinary(string filePath)
    // {
    //     var formatter = new BinaryFormatter();
    //     var stream = new FileStream(filePath, FileMode.OpenOrCreate);
    //     SaveData.workingData = new SaveData();
    //     formatter.Serialize(stream, SaveData.workingData);
    //     stream.Close();
    // }

    private static void SaveXml(string filePath, SaveData saveData)
    {
        var xeRoot = new XElement("Ashes");
        xeRoot.SetAttributeValue("gameTime", saveData.GameTime);
        var xePlayer = new XElement("Player");
        xeRoot.Add(xePlayer);
        xePlayer.SetAttributeValue("maxHp", saveData.MaxHp);
        xePlayer.SetAttributeValue("hp", saveData.Hp);
        xePlayer.SetAttributeValue("maxMp", saveData.MaxMp);
        xePlayer.SetAttributeValue("mp", saveData.Mp);
        xePlayer.SetAttributeValue("mpRegenPerSec", saveData.MpRegenPerSec);
        xePlayer.SetAttributeValue("hasDoubleJump", saveData.HasDoubleJump);
        xePlayer.SetAttributeValue("savePointGuid", saveData.SavePointGuid);
        xePlayer.SetAttributeValue("inventory", saveData.Inventory);
        var xeGates = new XElement("Gates");
        xeRoot.Add(xeGates);
        if (saveData.gatesId != null)
        {
            for (int i = 0; i < saveData.gatesId.Length; i++)
            {
                var xeGate = new XElement("Gate");
                xeGates.Add(xeGate);
                xeGate.SetAttributeValue("id", saveData.gatesId[i]);
                xeGate.SetAttributeValue("status", saveData.gatesStatus[i]);
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

    // private static void LoadBinary(string filePath, out SaveData data)
    // {
    //     var formatter = new BinaryFormatter();
    //     var stream = new FileStream(_filePath, FileMode.Open);
    //     data = SaveData.workingData = formatter.Deserialize(stream) as SaveData;
    //     stream.Close();
    //     SaveData.workingData.Load();
    // }

    private static bool LoadXml(string filePath, out SaveData saveData)
    {
        saveData = new SaveData();
        var xeRoot = XElement.Load(filePath);
        if (!float.TryParse(xeRoot.Attribute("gameTime").Value, out saveData.GameTime)) return false;
        var xePlayer = xeRoot.Element("Player");
        if (!int.TryParse(xePlayer.Attribute("maxHp").Value, out saveData.MaxHp)) return false;
        if (!int.TryParse(xePlayer.Attribute("hp").Value, out saveData.Hp)) return false;
        if (!float.TryParse(xePlayer.Attribute("maxMp").Value, out saveData.MaxMp)) return false;
        if (!float.TryParse(xePlayer.Attribute("mp").Value, out saveData.Mp)) return false;
        if (!float.TryParse(xePlayer.Attribute("mpRegenPerSec").Value, out saveData.MpRegenPerSec)) return false;
        if (!bool.TryParse(xePlayer.Attribute("hasDoubleJump").Value, out saveData.HasDoubleJump)) return false;
        saveData.SavePointGuid = xePlayer.Attribute("savePointGuid").Value ?? string.Empty;
        saveData.Inventory = xePlayer.Attribute("inventory").Value ?? string.Empty;
        var xeGates = xeRoot.Element("Gates");
        var gatesId = new List<string>();
        var gatesStatus = new List<bool>();
        foreach (var xeGate in xeGates.Elements("Gate"))
        {

            if (xePlayer.Attribute("id") == null) return false;
            gatesId.Add(xePlayer.Attribute("id").Value);
            if (!bool.TryParse(xePlayer.Attribute("status").Value, out var status)) return false;
            gatesStatus.Add(status);
            saveData.gatesId = gatesId.ToArray();
            saveData.gatesStatus = gatesStatus.ToArray();
        }
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