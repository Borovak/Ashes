using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    public static int index = 0;
    public static SaveData latestSaveData;
    public static event Action<int> SaveDeleted;

    private static string _filePath => Application.persistentDataPath + $"/ashes_{index}.sav";

    public static string Save()
    {
        var filePath = _filePath;
        try
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(filePath, FileMode.OpenOrCreate);
            var data = new SaveData();
            formatter.Serialize(stream, data);
            stream.Close();
            return filePath;
        }
        catch (System.Exception)
        {
            return "";
        }
    }

    public static string Load()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                latestSaveData = null;
                return "Game file not found";
            }
            var formatter = new BinaryFormatter();
            var stream = new FileStream(_filePath, FileMode.Open);
            var data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            data.Load();
            latestSaveData = data;
            return "";
        }
        catch (Exception ex)
        {
            latestSaveData = null;
            return ex.ToString();
        }
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
