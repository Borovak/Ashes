using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{

    private static string _playerFile = Application.persistentDataPath + "/player.sav";
    private static string _worldFile = Application.persistentDataPath + "/world.sav";

    public static void Save()
    {
        SavePlayer();
        SaveWorld();
    }

    // Start is called before the first frame update
    public static void SavePlayer()
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_playerFile, FileMode.OpenOrCreate);
        var data = new PlayerData(PlayerPlatformerController.Instance);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        try
        {
            if (!File.Exists(_playerFile))
            {
                Debug.LogError("Player save file not found");
                return null;
            }
            var formatter = new BinaryFormatter();
            var stream = new FileStream(_playerFile, FileMode.Open);
            var data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static void SaveWorld()
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_worldFile, FileMode.OpenOrCreate);
        var data = new WorldData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static WorldData LoadWorld()
    {
        if (!File.Exists(_worldFile))
        {
            Debug.LogError("World save file not found");
            return null;
        }
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_worldFile, FileMode.Open);
        var data = formatter.Deserialize(stream) as WorldData;
        stream.Close();
        return data;
    }

    public static void WipeFiles()
    {
        System.IO.File.Delete(_playerFile);
        System.IO.File.Delete(_worldFile);
    }
}
