using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    public static int index = 0;
    public static SaveData latestSaveData;

    private static string _filePath => Application.persistentDataPath + $"/ashes_{index}.sav";

    public static void Save()
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_filePath, FileMode.OpenOrCreate);
        var data = new SaveData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load(bool loadGame)
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                if (loadGame)
                {
                    Debug.Log("Player save file not found");
                }
                latestSaveData = null;
                return null;
            }
            var formatter = new BinaryFormatter();
            var stream = new FileStream(_filePath, FileMode.Open);
            var data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            data.Load();
            latestSaveData = data;
            return data;
        }
        catch (Exception)
        {
            return null;
        }
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
