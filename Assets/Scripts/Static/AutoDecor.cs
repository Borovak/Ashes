using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Classes;
using UnityEngine;

#if UNITY_EDITOR
namespace Static
{
    public static class AutoDecor
    {

        private enum FilterValues
        {
            Empty,
            Placeholder,
            Occupied,
            DontCare
        }

        private class TerrainAsset
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
            //Loading filters
            LoadingFilters();
            //Looping chambers
            var chambers = GameObject.FindGameObjectsWithTag(Constants.TAG_CHAMBER);
            foreach (var chamberGameObject in chambers)
            {
                var chamberController = chamberGameObject.GetComponent<ChamberController>();
                ManageContainer(chamberController, Constants.TAG_TERRAINCONTAINER, Constants.NAME_TERRAINCONTAINER, Vector3.zero, out var terrainContainer);
                //Identifying theme
                if (string.IsNullOrEmpty(chamberController.theme)) continue;
                //Obtaining map
                if (!GetMap(chamberController, out var map, terrainContainer.transform))
                {
                    Debug.Log($"Cannot get map for {chamberController.chamberName}");
                    return;
                }
                var workingMap = CopyMap(map);
                //Getting base prefab
                var autoDecorPrefab = Resources.Load<GameObject>("Terrain");
                //Loading assets
                var assetGroups = GetAssets(chamberController.theme, 32);
                var sortingOrder = 31000;
                foreach (var assetGroupKvp in assetGroups.OrderByDescending(x => _filters[x.Key].OccupiedCount).ThenByDescending(x => _filters[x.Key].EmptyCount).ThenByDescending(x => _filters[x.Key].PlaceholderCount + _filters[x.Key].DontCareCount))
                {
                    var assetGroup = assetGroupKvp.Value;
                    var w = assetGroup.First().W;
                    var h = assetGroup.First().H;
                    var filter = assetGroup.First().Filter;
                    for (int y = 0; y < workingMap.GetLength(1); y++)
                    {
                        for (int x = 0; x < workingMap.GetLength(0); x++)
                        {
                            if (!IsFilterMatching(workingMap, filter, x, y, w, h, out var filledCells)) continue;
                            var index = UnityEngine.Random.Range(0, assetGroup.Count);
                            var decorAsset = assetGroup[index];
                            InstantiateAssetAtPosition(chamberController, autoDecorPrefab, terrainContainer.transform, decorAsset, x + filter.Offset.x, y + filter.Offset.y, sortingOrder);
                            foreach (var cell in filledCells)
                            {
                                workingMap[cell.x, cell.y] = -1;
                            }
                        }
                    }
                    sortingOrder -= 1000;
                }
                //Others
                //ApplyOverhang(map, chamberController, terrainContainer.transform.localPosition);
                ApplyGrass(map, chamberController, terrainContainer.transform.localPosition);
            }
        }

        private static void ManageContainer(ChamberController chamberController, string tag, string name, Vector3 position, out GameObject container)
        {
            container = GlobalFunctions.FindChildrenWithTag(chamberController.gameObject, tag, false).FirstOrDefault();
            if (container == null)
            {
                Debug.Log($"Container for {tag} not found, creating new");
                container = new GameObject(name);
                container.transform.parent = chamberController.gameObject.transform;
                container.tag = tag;
            }
            else
            {
                if (container.name != name)
                {
                    container.name = name;
                }
                if (container.transform.childCount > 0)
                {
                    while (container.transform.childCount != 0)
                    {
                        GameObject.DestroyImmediate(container.transform.GetChild(0).gameObject);
                    }
                }
            }
            container.transform.localPosition = position;
        }

        private static Dictionary<int, List<TerrainAsset>> GetAssets(string theme, int resolution)
        {
            var assets = new Dictionary<int, List<TerrainAsset>>();
            var sprites = GlobalFunctions.GetSpritesAtPath(theme, Constants.AssetTypes.Terrain);
            foreach (var sprite in sprites)
            {
                var w = Convert.ToInt32(sprite.rect.width) / resolution;
                var h = Convert.ToInt32(sprite.rect.height) / resolution;
                var nameSplit = sprite.name.Split('_');
                if (!int.TryParse(nameSplit[2], out var filterId)) continue;
                if (!int.TryParse(nameSplit[3], out var index)) continue;
                var decorAsset = new TerrainAsset { Sprite = sprite, W = w, H = h, FilterId = filterId, Index = index };
                if (!assets.ContainsKey(filterId))
                {
                    assets.Add(filterId, new List<TerrainAsset>());
                }
                assets[filterId].Add(decorAsset);
            }
            return assets;
        }

        private static bool GetMap(ChamberController chamberController, out int[,] map, Transform decorContainer)
        {
            var chamberGameObject = GameObject.FindGameObjectsWithTag(Constants.TAG_CHAMBER).FirstOrDefault<GameObject>(x => x.name == chamberController.chamberName);
            if (chamberGameObject == null)
            {
                map = new int[0, 0];
                return false;
            }
            var y = chamberGameObject.GetComponentInChildren<Grid>().gameObject.transform.position.y - chamberController.size.y;
            decorContainer.localPosition = new Vector3(chamberController.position.x, y + 0.5f);
            map = CopyMap(chamberController.GetMap());
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

        private static void InstantiateAssetAtPosition(ChamberController chamber, GameObject prefab, Transform container, TerrainAsset asset, int x, int y, int sortingOrder)
        {
            InstantiateAssetAtPosition(chamber, prefab, container, asset.Sprite, asset.H, x, y, sortingOrder, Vector2.zero, 0f, false);
        }

        private static void InstantiateAssetAtPosition(ChamberController chamber, GameObject prefab, Transform container, Sprite sprite, int h, int x, int y, int sortingOrder, Vector2 offset, float angle, bool flipSprite)
        {
            var scale = 0.5f;
            var obj = GameObject.Instantiate<GameObject>(prefab);
            obj.name = sprite.name;
            obj.transform.parent = container;
            obj.transform.localPosition = new Vector3(x * scale + offset.x, y * scale - scale + offset.y, 0);
            var spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.transform.localEulerAngles = new Vector3(0f, 0f, angle);
            spriteRenderer.sprite = sprite;
            spriteRenderer.flipX = flipSprite;
            spriteRenderer.sortingOrder = UnityEngine.Random.Range(sortingOrder - 1000, sortingOrder);
        }

        private static int[,] CopyMap(int[,] map)
        {
            var newMap = new int[map.GetLength(0), map.GetLength(1)];
            Array.Copy(map, 0, newMap, 0, map.Length);
            return newMap;
        }

        private static void ApplyOverhang(int[,] map, ChamberController chamberController, Vector3 position)
        {
            var prefab = Resources.Load<GameObject>("Overhang");
            ManageContainer(chamberController, Constants.TAG_OVERHANGCONTAINER, Constants.NAME_OVERHANGCONTAINER, position, out var container);
            var workingMap = CopyMap(map);
            var sprites = GlobalFunctions.GetSpritesAtPath(chamberController.theme, Constants.AssetTypes.Overhang);
            if (!sprites.Any()) return;
            for (int y = 2; y < workingMap.GetLength(1) - 1; y++)
            {
                for (int x = 1; x < workingMap.GetLength(0) - 1; x++)
                {
                    if (workingMap[x, y] >= 1 && workingMap[x, y - 1] == 0 && workingMap[x, y + 1] >= 1 && workingMap[x - 1, y] >= 1 && workingMap[x + 1, y] >= 1)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) > 0.35f) continue;
                        var index = UnityEngine.Random.Range(0, sprites.Count);
                        var offsetX = UnityEngine.Random.Range(-0.5f, 0.5f);
                        var flip = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
                        InstantiateAssetAtPosition(chamberController, prefab, container.transform, sprites[index], 0, x, y, 2000, new Vector2(offsetX, -0.05f), 0f, flip);
                        x++;
                    }
                }
            }

        }

        private static void ApplyGrass(int[,] map, ChamberController chamberController, Vector3 position)
        {
            var prefab = Resources.Load<GameObject>("Grass");
            ManageContainer(chamberController, Constants.TAG_GRASSCONTAINER, Constants.NAME_GRASSCONTAINER, position, out var container);
            var workingMap = CopyMap(map);
            var sprites = new List<Sprite>();
            sprites.AddRange(GlobalFunctions.GetSpritesAtPath($"Assets/Sprites/Grass/{chamberController.theme}/{chamberController.chamberName}/grass_{chamberController.theme}_{chamberController.chamberName}.png"));
            sprites.AddRange(GlobalFunctions.GetSpritesAtPath($"Assets/Sprites/Grass/{chamberController.theme}/grass_{chamberController.theme}_generic.png"));
            if (!sprites.Any()) return;
            if (!chamberController.grassManager.IsThereAnyGrass()) return;
            for (int y = 0; y < workingMap.GetLength(1) - 3; y++)
            {
                for (int x = 1; x < workingMap.GetLength(0) - 1; x++)
                {
                    if (!chamberController.grassManager.IsThereGrassThere(x, y)) continue;
                    if (workingMap[x, y] >= 1 && workingMap[x, y + 1] == 0 && workingMap[x, y - 1] >= 1 && workingMap[x - 1, y] >= 1 && workingMap[x + 1, y] >= 1)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) > 0.35f) continue;
                        var index = UnityEngine.Random.Range(0, sprites.Count);
                        var offsetX = UnityEngine.Random.Range(-0.5f, 0.5f);
                        var angle = UnityEngine.Random.Range(-15f, 15f);
                        var flip = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
                        InstantiateAssetAtPosition(chamberController, prefab, container.transform, sprites[index], 0, x, y, 2000, new Vector2(offsetX, -0.05f), angle, flip);
                        x++;
                    }
                }
            }
        }
    }
}
#endif