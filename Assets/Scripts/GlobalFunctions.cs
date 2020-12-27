using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public static class GlobalFunctions
{
    public static event Action OpenShopTriggered;

    public static DataTable ParseCsv(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        var fileData = textAsset.text;
        var lines = fileData.Split("\n"[0]);
        var dt = new DataTable();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var lineData = (line.Trim()).Split("|"[0]);
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
        if (includeParent && parent.tag == tag)
        {
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

    public static float Bound(float value, float min, float max)
    {
        if (value > max) return max;
        if (value < min) return min;
        return value;
    }

    public static bool TryGetNpcSprite(Constants.Npc npc, out Sprite sprite)
    {
        string resourceName;
        switch (npc)
        {
            case Constants.Npc.Shopkeeper:
                resourceName = "Shopkeeper";
                break;
            case Constants.Npc.HoboTom:
                resourceName = "HoboTom";
                break;
            default:
                sprite = null;
                return false;
        }
        sprite = Resources.Load<Sprite>($"Npc/{resourceName}");
        return sprite != null;
    }

    public static void OpenShop()
    {
        Debug.Log("Shop interface test");
        OpenShopTriggered?.Invoke();
    }

    public static float GetAngleFromPoints(Vector3 reference, Vector3 target)
    {
        return GetAngleFromPoints(new Vector2(reference.x, reference.y), new Vector2(target.x, target.y));
    }

    public static float GetAngleFromPoints(Vector2 reference, Vector2 target)
    {
        double dx = target.x - reference.x;
        // Minus to correct for coord re-mapping
        double dy = target.y - reference.y;
        double inRads = Math.Atan2(dy, dx);
        // We need to map to coord system when 0 degree is at 3 O'clock, 270 at 12 O'clock
        inRads = inRads < 0 ? Math.Abs(inRads) : 2 * Math.PI - inRads;
        return 360f - Convert.ToSingle((180 / Math.PI) * inRads);
    }

    public static void PlaySound(AudioClip audioClip, Vector3 position)
    {
        if (audioClip == null) return;
        LeanAudio.playClipAt(audioClip, position);
    }

    public static void PlayRandomSound(AudioClip[] audioClips, Vector3 position)
    {
        if (audioClips.Length == 0) return;
        var index = UnityEngine.Random.Range(0, audioClips.Length);
        LeanAudio.playClipAt(audioClips[index], position);
    }
}