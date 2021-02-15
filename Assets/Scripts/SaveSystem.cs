using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Player;
using Static;

public static class SaveSystem
{
    public static int index = 0;
    public static event Action<int> SaveDeleted; 
    public static event Action<bool> GameSaved;
    public static event Action GameLoaded;

    public static SaveData LastLoadedSave
    {
        get => _lastLoadedSave;
        set
        {
            _lastLoadedSave = value;
            GameLoaded?.Invoke();
        }
    }

    private static string _filePath => Application.persistentDataPath + $"/ashes_{index}.sav";
    private static SaveData _lastLoadedSave;

    public static bool Save(string savePointGuid, bool healOnSave, out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            LastLoadedSave = PrepareSaveData(savePointGuid, healOnSave);
            //SaveBinary(filePath);
            SaveXml(filePath, LastLoadedSave);
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

    public static bool EditorSave(string savePointGuid, out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            LastLoadedSave = new SaveData();
            LastLoadedSave.SavePointGuid = savePointGuid;
            //SaveBinary(filePath);
            SaveXml(filePath, LastLoadedSave);
            errorMessage = "";
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
            //SaveBinary(filePath);
            LastLoadedSave = new SaveData();
            SaveXml(filePath, LastLoadedSave);
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

    private static SaveData PrepareSaveData(string savePointGuid, bool healOnSave){
        var saveData = new SaveData();
        if (!GlobalFunctions.TryGetPlayerComponent<PlayerPlatformerController>(out var playerPlatformerController)) return saveData;
        if (!GlobalFunctions.TryGetPlayerComponent<PlayerLifeController>(out var playerLifeController)) return saveData;
        if (!GlobalFunctions.TryGetPlayerComponent<ManaController>(out var manaController)) return saveData;
        saveData.GameTime = GameController.gameTime;
        saveData.HasDoubleJump = playerPlatformerController.hasDoubleJump;
        saveData.Hp = healOnSave ? playerLifeController.GetMaxHp() : playerLifeController.GetHp();
        saveData.MaxMp = playerLifeController.GetMaxHp();
        saveData.Mp = healOnSave ? manaController.GetMaxMp() : manaController.GetMp();
        saveData.MaxMp = manaController.GetMaxMp();
        saveData.MpRegenPerSec = manaController.mpRegenPerSec;
        saveData.XeInventories = GlobalInventoryManager.GetXeFromInventories();
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
        xeRoot.Add(saveData.XeInventories);
        xePlayer.SetAttributeValue("maxHp", saveData.MaxHp);
        xePlayer.SetAttributeValue("hp", saveData.Hp);
        xePlayer.SetAttributeValue("maxMp", saveData.MaxMp);
        xePlayer.SetAttributeValue("mp", saveData.Mp);
        xePlayer.SetAttributeValue("mpRegenPerSec", saveData.MpRegenPerSec);
        xePlayer.SetAttributeValue("hasDoubleJump", saveData.HasDoubleJump);
        xePlayer.SetAttributeValue("savePointGuid", saveData.SavePointGuid);
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

    public static bool Load(out string errorMessage)
    {
        var filePath = _filePath;
        try
        {
            if (!File.Exists(filePath))
            {
                errorMessage = "Game file not found";
                LastLoadedSave = null;
                return false;
            }
            //LoadBinary(filePath, out data);
            if (!LoadXml(filePath, out var data))
            {
                errorMessage = "Game file load failed (XML)";
                LastLoadedSave = null;
                return false;
            }
            LastLoadedSave = data;
            errorMessage = "";
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.ToString();
            LastLoadedSave = null;
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
        XElement xeRoot;
        try
        {
            xeRoot = XElement.Load(filePath);
        }
        catch (Exception e)
        {
            Debug.Log($"Cannot load xml: {e}");
            return false;
        }
        if (!float.TryParse(xeRoot.Attribute("gameTime")?.Value ?? "_", out saveData.GameTime)) return false;
        //Player
        var xePlayer = xeRoot.Element("Player");
        if (xePlayer != null)
        {
            if (!int.TryParse(xePlayer.Attribute("maxHp")?.Value ?? "_", out saveData.MaxHp)) return false;
            if (!int.TryParse(xePlayer.Attribute("hp")?.Value ?? "_", out saveData.Hp)) return false;
            if (!float.TryParse(xePlayer.Attribute("maxMp")?.Value ?? "_", out saveData.MaxMp)) return false;
            if (!float.TryParse(xePlayer.Attribute("mp")?.Value ?? "_", out saveData.Mp)) return false;
            if (!float.TryParse(xePlayer.Attribute("mpRegenPerSec")?.Value ?? "_", out saveData.MpRegenPerSec)) return false;
            if (!bool.TryParse(xePlayer.Attribute("hasDoubleJump")?.Value ?? "_", out saveData.HasDoubleJump)) return false;
            saveData.SavePointGuid = xePlayer.Attribute("savePointGuid")?.Value ?? string.Empty;
        }

        //Inventories
        saveData.XeInventories = xeRoot.Element("Inventories");
        GlobalInventoryManager.SetInventoriesFromXe(saveData.XeInventories);
        //Gates
        var xeGates = xeRoot.Element("Gates");
        var gatesId = new List<string>();
        var gatesStatus = new List<bool>();
        if (xeGates != null)
        {
            foreach (var xeGate in xeGates.Elements("Gate"))
            {
                var gateId = xeGate.Attribute("id")?.Value;
                if (string.IsNullOrEmpty(gateId)) continue;
                if (!bool.TryParse(xeGate.Attribute("status")?.Value ?? "_", out var gateStatus)) continue;
                gatesId.Add(gateId);
                gatesStatus.Add(gateStatus);
                saveData.gatesId = gatesId.ToArray();
                saveData.gatesStatus = gatesStatus.ToArray();
            }
        }
        Debug.Log("Load successful");
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