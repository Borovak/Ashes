using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Xml.Linq;

public class AutoDecor
{
    const string TAG_CHAMBERCONTAINER = "ChamberContainer";
    const string TAG_CHAMBER = "Chamber";
    const string TAG_DECORCONTAINER = "DecorContainer";
    const string NAME_DECORCONTAINER = "Decor";

    private enum FilterValues
    {
        Empty,
        Placeholder,
        Occupied,
        DontCare
    }

    private class DecorAsset
    {
        internal Sprite Sprite;
        internal int W;
        internal int H;
        internal int Size => W * H;
        internal int FilterId;
        internal int Index;
        internal Filter Filter => _filters[FilterId];
    }

    private class Filter
    {
        internal int Id;
        internal int W;
        internal int H;
        internal FilterValues[,] Values;
        internal Vector2Int Offset;
        internal int EmptyCount;
        internal int OccupiedCount;
        internal int PlaceholderCount;
        internal int DontCareCount;
        internal int ContraintCount => EmptyCount + OccupiedCount;
        internal Filter(int id, int w, int h)
        {
            Id = id;
            W = w;
            H = h;
            Values = new FilterValues[w, h];
        }
    }

    private static Dictionary<int, Filter> _filters;

    public static void Generate()
    {
        //Clearing log
        Utils.ClearLogConsole();
        //Loading filters
        LoadingFilters();
        //Looping chambers
        var chambers = GameObject.FindGameObjectsWithTag(TAG_CHAMBER);
        foreach (var chamberGameObject in chambers)
        {
            var chamberController = chamberGameObject.GetComponent<ChamberController>();
            ManageDecorContainer(chamberController, out var decorContainer);
            //if (chamberController.chamberName != "Forest1") continue;
            //Identifying theme
            if (string.IsNullOrEmpty(chamberController.theme)) continue;
            //Obtaining map
            if (!GetMap(chamberController, out var map, decorContainer.transform))
            {
                Debug.Log($"Cannot get map for {chamberController.chamberName}");
                return;
            }
            var workingMap = CopyMap(map);
            //Getting base prefab
            var autoDecorPrefab = Resources.Load<GameObject>("AutoDecor/AutoDecor");
            //Loading assets
            var assetGroups = GetDecorAssets(chamberController.theme, "AutoDecor", 32);
            foreach (var assetGroupKvp in assetGroups.OrderByDescending(x => _filters[x.Key].OccupiedCount).ThenByDescending(x => _filters[x.Key].EmptyCount).ThenByDescending(x => _filters[x.Key].PlaceholderCount))
            {
                var assetGroup = assetGroupKvp.Value;
                var w = assetGroup.First().W;
                var h = assetGroup.First().H;
                var filter = assetGroup.First().Filter;
                //if (filter.Id != 0) continue;
                for (int y = 0; y < workingMap.GetLength(1); y++)
                {
                    for (int x = 0; x < workingMap.GetLength(0); x++)
                    {
                        if (!IsFilterMatching(workingMap, filter, x, y, w, h, out var filledCells)) continue;
                        var index = UnityEngine.Random.Range(0, assetGroup.Count);
                        var decorAsset = assetGroup[index];
                        InstantiateAssetAtPosition(chamberController, autoDecorPrefab, decorContainer.transform, decorAsset, x + filter.Offset.x, y + filter.Offset.y, 0);
                        Debug.Log($"{chamberController.chamberName} {filter.Id} {x} {filter.Offset.x} {y} {filter.Offset.y}");
                        foreach (var cell in filledCells)
                        {
                            workingMap[cell.x, cell.y] = -1;
                        }
                    }
                }
            }
            //Hanging
            //ApplyOverhang(map, chamberController, autoDecorPrefab, decorContainer);
        }
    }

    private static void ManageDecorContainer(ChamberController chamberController, out GameObject decorContainer)
    {
        var chamberContainer = GameObject.FindGameObjectsWithTag(TAG_CHAMBERCONTAINER).ToList().FirstOrDefault<GameObject>(x => x.name == chamberController.chamberName);
        if (chamberContainer == null)
        {
            Debug.Log($"Chamber container {chamberController.chamberName} not found, creating new");
            chamberContainer = new GameObject(chamberController.chamberName);
            chamberContainer.tag = TAG_CHAMBERCONTAINER;
        }
        decorContainer = GlobalFunctions.FindChildrenWithTag(chamberContainer, TAG_DECORCONTAINER, false).FirstOrDefault();
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
        var decorAssets = new Dictionary<int, List<DecorAsset>>();
        var sprites = GetSpritesAtPath(theme, type);
        foreach (var sprite in sprites)
        {
            var w = Convert.ToInt32(sprite.rect.width) / resolution;
            var h = Convert.ToInt32(sprite.rect.height) / resolution;
            var nameSplit = sprite.name.Split('_');
            if (!int.TryParse(nameSplit[1], out var filterId)) continue;
            if (!int.TryParse(nameSplit[2], out var index)) continue;
            var decorAsset = new DecorAsset { Sprite = sprite, W = w, H = h, FilterId = filterId, Index = index };
            if (!decorAssets.ContainsKey(filterId))
            {
                decorAssets.Add(filterId, new List<DecorAsset>());
            }
            decorAssets[filterId].Add(decorAsset);
        }
        return decorAssets;
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

    private static bool IsFilterMatching(int[,] map, Filter filter, int x, int y, int w, int h, out List<Vector2Int> filledCells)
    {
        filledCells = new List<Vector2Int>();
        if (x < 0) return false;
        if (x + filter.W >= map.GetLength(0)) return false;
        if (y < 0) return false;
        if (y + filter.H >= map.GetLength(1)) return false;
        for (int ox = 0; ox < filter.W; ox++)
        {
            for (int oy = 0; oy < filter.H; oy++)
            {
                var filterValue = filter.Values[ox, oy];
                var mapX = x + ox;
                var mapY = y + oy;
                var mapValue = map[mapX, mapY];
                switch (filterValue)
                {
                    case FilterValues.Empty:
                        if (mapValue != 0) return false;
                        break;
                    case FilterValues.Placeholder:
                        if (mapValue < 1) return false;
                        filledCells.Add(new Vector2Int(mapX, mapY));
                        break;
                    case FilterValues.Occupied:
                        if (mapValue == 0) return false;
                        break;
                }
            }
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

    private static void LoadingFilters()
    {
        _filters = new Dictionary<int, Filter>();
        var path = "c:/users/borovak/source/repos/Ashes/Assets/Resources/layout.xml";
        XElement xeRoot = null;
        try
        {
            xeRoot = XElement.Load(path);
        }
        catch (Exception)
        {
            throw (new Exception($"Cannot parse file: {path}"));
        }
        var xeFilters = xeRoot.Element("Filters");
        foreach (var xeFilter in xeFilters.Elements())
        {
            var id = int.Parse(xeFilter.Attribute("id").Value);
            var w = int.Parse(xeFilter.Attribute("w").Value);
            var h = int.Parse(xeFilter.Attribute("h").Value);
            var filter = new Filter(id, w, h);
            var valuesString = xeFilter.Attribute("values").Value;
            var valuesSplit = valuesString.Split(',');
            var offsetSet = false;
            for (int i = 0; i < valuesSplit.Length; i++)
            {
                var x = i % w;
                var y = Convert.ToInt32(Math.Floor(Convert.ToDouble(i) / Convert.ToDouble(w)));
                Enum.TryParse(valuesSplit[i], out filter.Values[x, y]);
                switch (filter.Values[x, y])
                {
                    case FilterValues.Empty:
                        filter.EmptyCount++;
                        break;
                    case FilterValues.Placeholder:
                        filter.PlaceholderCount++;
                        if (!offsetSet)
                        {
                            offsetSet = true;
                            filter.Offset.x = x;
                            filter.Offset.y = y;
                        }
                        break;
                    case FilterValues.Occupied:
                        filter.OccupiedCount++;
                        break;
                    case FilterValues.DontCare:
                        filter.DontCareCount++;
                        break;
                }
            }
            _filters.Add(id, filter);
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
        obj.transform.localPosition = new Vector3(x * scale, y * scale - scale + yOffset, 0);
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

    private static void ApplyOverhang(int[,] map, ChamberController chamberController, GameObject autoDecorPrefab, GameObject decorContainer)
    {
        var workingMap = CopyMap(map);
        var overhangs = GetSpritesAtPath(chamberController.theme, "Overhang");
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
                        InstantiateAssetAtPosition(chamberController, autoDecorPrefab, decorContainer.transform, overhangs[index], 0, x, y, 2000, -0.05f);
                        x++;
                    }
                }
            }
        }
    }
}