using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TerrainDecor
{
    const string TAG_CHAMBERCONTAINER = "ChamberContainer";
    const string TAG_CHAMBER = "Chamber";
    const string TAG_DECORCONTAINER = "DecorContainer";
    const string NAME_DECORCONTAINER = "Decor";

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
            if (!GetMap(chamber, out var map, decorContainer.transform))
            {
                Debug.Log($"Cannot get map for {chamber.chamberName}");
                return;
            }
            var autoDecorPrefab = Resources.Load<GameObject>("AutoDecor/AutoDecor");
            //Ground            
            var assetGroups = GetDecorAssets(chamber.theme, "Ground", 32);
            var workingMap = CopyMap(map);
            foreach (var assetGroup in assetGroups.OrderByDescending(x => x.Key))
            {
                // if (assetGroup.Value.Count > 1){
                //     Debug.Log(string.Join(", ", assetGroup.Value.Select(x => x.Sprite.name)));
                // }
                for (int x = 0; x < workingMap.GetLength(0); x++)
                {
                    for (int y = 0; y < workingMap.GetLength(1); y++)
                    {
                        if (IsRangeEmptyAbove(workingMap, x, y, assetGroup.Value[0].W))
                        {
                            var index = UnityEngine.Random.Range(0, assetGroup.Value.Count);
                            var asset = assetGroup.Value[index];
                            InstantiateAssetAtPosition(chamber, autoDecorPrefab, decorContainer.transform, asset, x, y, 1000);
                            SetRangeInArray(workingMap, -1, x, y, asset.W, asset.H);
                        }
                    }
                }
            }
            //Auto decor
            assetGroups = GetDecorAssets(chamber.theme, "AutoDecor", 32);
            //var workingMap = CopyMap(map);
            foreach (var assetGroup in assetGroups.OrderByDescending(x => x.Key))
            {
                // if (assetGroup.Value.Count > 1){
                //     Debug.Log(string.Join(", ", assetGroup.Value.Select(x => x.Sprite.name)));
                // }
                for (int x = 0; x < workingMap.GetLength(0); x++)
                {
                    for (int y = 0; y < workingMap.GetLength(1); y++)
                    {
                        if (IsRangeAvailable(workingMap, x, y, assetGroup.Value[0].W, assetGroup.Value[0].H))
                        {
                            var index = UnityEngine.Random.Range(0, assetGroup.Value.Count);
                            var asset = assetGroup.Value[index];
                            InstantiateAssetAtPosition(chamber, autoDecorPrefab, decorContainer.transform, asset, x, y, 0);
                            SetRangeInArray(workingMap, -1, x, y, asset.W, asset.H);
                        }
                    }
                }
            }
            //Hanging
            workingMap = CopyMap(map);
            var overhangs = GetSpritesAtPath(chamber.theme, "Overhang");
            if (overhangs.Any())
            {
                for (int y = 2; y < workingMap.GetLength(1) - 1; y++)
                {
                    for (int x = 1; x < workingMap.GetLength(0) - 1; x++)
                    {
                        if (workingMap[x, y] >= 1 && workingMap[x, y - 1] == 0 && workingMap[x, y + 1] >= 1 && workingMap[x - 1, y] >= 1 && workingMap[x + 1, y] >= 1)
                        {
                            if (UnityEngine.Random.Range(0f, 1f) > 0.35f) continue;
                            var index = UnityEngine.Random.Range(0, overhangs.Count);
                            InstantiateAssetAtPosition(chamber, autoDecorPrefab, decorContainer.transform, overhangs[index], 0, x, y, 2000, -0.05f);
                            x++;
                        }
                    }
                }
            }
        }
    }

    private static List<Sprite> GetSpritesAtPath(string theme, string type)
    {
        var path = $"Assets/Sprites/{type}/{theme.ToLower()}.png";
        var data = AssetDatabase.LoadAllAssetsAtPath(path);
        var sprites = new List<Sprite>();
        foreach (var item in data)
        {
            var sprite = item as Sprite;
            if (sprite == null) continue;
            sprites.Add(sprite);
        }
        return sprites;
    }

    private static Dictionary<int, List<DecorAsset>> GetDecorAssets(string theme, string type, int resolution)
    {
        var assetGroups = new Dictionary<int, List<DecorAsset>>();
        var sprites = GetSpritesAtPath(theme, type);
        foreach (var sprite in sprites)
        {
            var w = Convert.ToInt32(sprite.rect.width) / resolution;
            var h = Convert.ToInt32(sprite.rect.height) / resolution;
            var asset = new DecorAsset { Sprite = sprite, W = w, H = h };
            if (!assetGroups.ContainsKey(asset.size))
            {
                assetGroups.Add(asset.size, new List<DecorAsset>());
            }
            assetGroups[asset.size].Add(asset);
        }
        return assetGroups;
    }

    private static bool GetMap(ChamberController chamberController, out int[,] map, Transform decorContainer)
    {
        var chamberGameObject = GameObject.FindGameObjectsWithTag(TAG_CHAMBER).FirstOrDefault<GameObject>(x => x.name == chamberController.chamberName);
        if (chamberGameObject == null)
        {
            map = new int[0, 0];
            return false;
        }
        var y = chamberGameObject.GetComponentInChildren<Grid>().gameObject.transform.position.y - chamberController.size.y;
        decorContainer.localPosition = new Vector3(chamberController.position.x, y + 0.5f);
        map = CopyMap(chamberController.map);
        return map != null;
    }

    private static bool IsRangeAvailable(int[,] map, int ox, int oy, int w, int h)
    {
        if (ox + w >= map.GetLength(0) || oy + h >= map.GetLength(1)) return false;
        for (int x = ox; x < ox + w; x++)
        {
            for (int y = oy; y < oy + h; y++)
            {
                if (map[x, y] <= 0) return false;
            }
        }
        return true;
    }

    private static bool IsRangeEmptyAbove(int[,] map, int ox, int y, int w)
    {
        if (ox + w >= map.GetLength(0) || y >= map.GetLength(1) - 1) return false;
        for (int x = ox; x < ox + w; x++)
        {
            if (map[x, y] >= 1 && map[x, y + 1] == 0) continue;
            return false;
        }
        return true;
    }

    private static void SetRangeInArray(int[,] map, int value, int ox, int oy, int w, int h)
    {
        for (int x = ox; x < System.Math.Min(map.GetLength(0), ox + w); x++)
        {
            for (int y = oy; y < System.Math.Min(map.GetLength(1), oy + h); y++)
            {
                map[x, y] = value;
            }
        }
    }

    private static void InstantiateAssetAtPosition(ChamberController chamber, GameObject prefab, Transform decorContainer, DecorAsset asset, int x, int y, int sortingOrder)
    {
        InstantiateAssetAtPosition(chamber, prefab, decorContainer, asset.Sprite, asset.H, x, y, sortingOrder);
    }

    private static void InstantiateAssetAtPosition(ChamberController chamber, GameObject prefab, Transform decorContainer, Sprite sprite, int h, int x, int y, int sortingOrder, float yOffset = 0f)
    {
        var scale = 0.5f;
        var obj = GameObject.Instantiate<GameObject>(prefab);
        obj.name = sprite.name;
        obj.transform.parent = decorContainer;
        obj.transform.localPosition = new Vector3(x * scale, (y + h - 1f) * scale + yOffset, 0);
        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        var c = spriteRenderer.color;
        c.r -= UnityEngine.Random.Range(0f, chamber.colorShiftR);
        c.g -= UnityEngine.Random.Range(0f, chamber.colorShiftG);
        c.b -= UnityEngine.Random.Range(0f, chamber.colorShiftB);
        spriteRenderer.color = c;
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = UnityEngine.Random.Range(sortingOrder - 1000, sortingOrder);
    }

    private static int[,] CopyMap(int[,] map)
    {
        var newMap = new int[map.GetLength(0), map.GetLength(1)];
        Array.Copy(map, 0, newMap, 0, map.Length);
        return newMap;
    }
}