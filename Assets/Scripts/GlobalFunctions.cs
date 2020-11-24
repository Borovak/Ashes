using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public static class GlobalFunctions
{
    public static DataTable ParseCsv(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        var fileData = textAsset.text;
        var lines = fileData.Split("\n"[0]);
        var dt = new DataTable();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var lineData = (line.Trim()).Split(","[0]);
            if (dt.Columns.Count == 0)
            {
                foreach (var cell in lineData)
                {
                    dt.Columns.Add();
                }
            }
            var dr = dt.Rows.Add();
            for (int i = 0; i < lineData.Length; i++)
            {
                dr[i] = lineData[i];
            }
        }
        return dt;
    }

    public static GameController GetGameController()
    {
        var gameObject = GameObject.FindGameObjectWithTag("GameController");
        return gameObject.GetComponent<GameController>();
    }

    public static PlayerPlatformerController GetPlayerPlatformerController()
    {
        var gameObject = GameObject.FindGameObjectWithTag("Player");
        return gameObject.GetComponent<PlayerPlatformerController>();
    }

    public static PlayerLifeController GetPlayerLifeController()
    {
        var gameObject = GameObject.FindGameObjectWithTag("Player");
        return gameObject.GetComponent<PlayerLifeController>();
    }

    public static ManaController GetPlayerManaController()
    {
        var gameObject = GameObject.FindGameObjectWithTag("Player");
        return gameObject.GetComponent<ManaController>();
    }

    public static PlayerInventory GetPlayerInventory()
    {
        var gameObject = GameObject.FindGameObjectWithTag("Player");
        return gameObject.GetComponent<PlayerInventory>();
    }

    public static List<GameObject> FindChildrenWithTag(GameObject parent, string tag, bool includeParent)
    {
        var gameObjects = new List<GameObject>();
        if (includeParent && parent.tag == tag){
            gameObjects.Add(parent);
        }
        foreach (Transform tr in parent.transform)
        {
            var gameObjectsFound = FindChildrenWithTag(tr.gameObject, tag, true);
            if (gameObjectsFound.Any())
            {
                gameObjects.AddRange(gameObjectsFound);
            }
        }
        return gameObjects;
    }

    public static float Bound(float value, float min, float max){
        if (value > max) return max;
        if (value < min) return min;
        return value;
    }
}