using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;

public class TerrainDecor
{
    const string TAG_CHAMBERCONTAINER = "ChamberContainer";
    const string TAG_CHAMBER = "Chamber";
    const string TAG_DECORCONTAINER = "DecorContainer";
    const string NAME_DECORCONTAINER = "Decor";
    const int DECORRESOLUTION = 32;

    private class DecorAsset
    {
        internal Sprite Sprite;
        internal int W;
        internal int H;
        internal int size => W * H;
    }

    public static void Generate()
    {
        Utils.ClearLogConsole();
        var chambers = GameObject.FindGameObjectsWithTag(TAG_CHAMBER);
        foreach (var chamberGameObject in chambers)
        {
            var chamber = chamberGameObject.GetComponent<ChamberController>();
            var chamberContainer = GameObject.FindGameObjectsWithTag(TAG_CHAMBERCONTAINER).ToList().FirstOrDefault<GameObject>(x => x.name == chamber.chamberName);
            if (chamberContainer == null)
            {
                Debug.Log($"Chamber container {chamber.chamberName} not found, creating new");
                chamberContainer = new GameObject(chamber.chamberName);
                chamberContainer.tag = TAG_CHAMBERCONTAINER;
            }
            var decorContainer = GlobalFunctions.FindChildrenWithTag(chamberContainer, TAG_DECORCONTAINER, false).FirstOrDefault();
            if (decorContainer == null)
            {
                Debug.Log($"Decor container not found, creating new");
                decorContainer = new GameObject(NAME_DECORCONTAINER);
                decorContainer.transform.parent = chamberContainer.transform;
                decorContainer.tag = TAG_DECORCONTAINER;
            }
            else
            {
                if (decorContainer.name != NAME_DECORCONTAINER)
                {
                    decorContainer.name = NAME_DECORCONTAINER;
                }
                if (decorContainer.transform.childCount > 0)
                {
                    while (decorContainer.transform.childCount != 0)
                    {
                        GameObject.DestroyImmediate(decorContainer.transform.GetChild(0).gameObject);
                    }
                    Debug.Log($"Emptying decor container");
                }
            }
            if (string.IsNullOrEmpty(chamber.theme)) continue;
            var decorAssets = GetDecorAssets(chamber.theme);
            if (!GetMap(chamber, out var map, decorContainer.transform))
            {
                Debug.Log($"Cannot get map for {chamber.chamberName}");
                return;
            }
            Debug.Log($"Map found for {chamber.chamberName}: {map.GetLength(0)}x{map.GetLength(1)}");
            var autoDecorPrefab = Resources.Load<GameObject>("AutoDecor/AutoDecor");
            foreach (var asset in decorAssets.OrderByDescending(x => x.size))
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (IsRangeAvailable(map, x, y, asset.W, asset.H))
                        {
                            InstantiateAssetAtPosition(chamber, autoDecorPrefab, decorContainer.transform, asset, x, y);
                            SetRangeInArray(map, false, x, y, asset.W, asset.H);
                        }
                    }
                }
            }
        }
    }

    private static List<DecorAsset> GetDecorAssets(string theme)
    {
        var decorAssets = new List<DecorAsset>();
        var path = $"Assets/Sprites/AutoDecor/{theme.ToLower()}.png";
        var data = AssetDatabase.LoadAllAssetsAtPath(path);
        Debug.Log($"{data.Count()} assets found at {path}");
        foreach (var item in data)
        {
            var sprite = item as Sprite;
            if (sprite == null) continue;
            decorAssets.Add(new DecorAsset { Sprite = sprite, W = Convert.ToInt32(sprite.rect.width) / DECORRESOLUTION, H = Convert.ToInt32(sprite.rect.height) / DECORRESOLUTION });
        }
        return decorAssets;
    }

    private static bool GetMap(ChamberController chamberController, out bool[,] map, Transform decorContainer)
    {
        var chamberGameObject = GameObject.FindGameObjectsWithTag(TAG_CHAMBER).FirstOrDefault<GameObject>(x => x.name == $"Chamber_{chamberController.chamberName}");
        if (chamberGameObject == null)
        {
            map = new bool[0, 0];
            return false;
        }
        var y = chamberGameObject.GetComponentInChildren<Grid>().gameObject.transform.position.y - chamberController.size.y;
        decorContainer.localPosition = new Vector3(chamberController.position.x, y + 0.5f);
        map = new bool[chamberController.map.GetLength(0), chamberController.map.GetLength(1)];
        Array.Copy(chamberController.map, 0, map, 0, chamberController.map.Length);
        return map != null;
    }

    private static bool IsRangeAvailable(bool[,] map, int ox, int oy, int w, int h)
    {
        if (ox + w >= map.GetLength(0) || oy + h >= map.GetLength(1)) return false;
        for (int x = ox; x < ox + w; x++)
        {
            for (int y = oy; y < oy + h; y++)
            {
                if (!map[x, y]) return false;
            }
        }
        return true;
    }

    private static void SetRangeInArray(bool[,] map, bool value, int ox, int oy, int w, int h)
    {
        for (int x = ox; x < System.Math.Min(map.GetLength(0), ox + w); x++)
        {
            for (int y = oy; y < System.Math.Min(map.GetLength(1), oy + h); y++)
            {
                map[x, y] = value;
            }
        }
    }

    private static void InstantiateAssetAtPosition(ChamberController chamber, GameObject prefab, Transform decorContainer, DecorAsset asset, int x, int y)
    {
        var scale = 0.5f;
        var obj = GameObject.Instantiate<GameObject>(prefab);
        obj.name = asset.Sprite.name;
        obj.transform.parent = decorContainer;
        obj.transform.localPosition = new Vector3(x * scale, (y + asset.H - 1f) * scale, 0);
        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        var c = spriteRenderer.color;
        c.r -= UnityEngine.Random.Range(0f, chamber.colorShiftR);
        c.g -= UnityEngine.Random.Range(0f, chamber.colorShiftG);
        c.b -= UnityEngine.Random.Range(0f, chamber.colorShiftB);
        spriteRenderer.color = c;
        spriteRenderer.sprite = asset.Sprite;
    }
}