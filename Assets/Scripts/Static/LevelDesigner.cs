using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using System;
using UnityEngine.Tilemaps;
using System.Reflection;
using System.Linq;

public static class LevelDesigner
{
    #if UNITY_EDITOR
    public static void LoadSpecific()
    {
        var path = EditorUtility.OpenFilePanel("Select level designer file", "", "xml");
        if (string.IsNullOrEmpty(path)) return;
        if (!System.IO.File.Exists(path))
        {
            EditorUtility.DisplayDialog("Select level designer file", "The path is invalid", "OK");
            return;
        }
        Debug.Log(path);
        Load(path);
    }
    #endif  

    public static void LoadDefault()
    {
        Load("c:/users/borovak/source/repos/Ashes/Assets/Resources/layout.xml");
    }

    private static void Load(string path)
    {
        //Find or create folders
        var chambersFolder = FindCreateFolder("ChambersFolder", "Chambers", false);
        var savePointsFolder = FindCreateFolder("SavePointsFolder", "SavePoints", true);
        //Reading xml layout
        XElement xeRoot = null;
        try
        {
            xeRoot = XElement.Load(path);
        }
        catch (Exception)
        {
            throw (new Exception($"Cannot parse file: {path}"));
        }
        var xeRooms = xeRoot.Element("Rooms");
        if (xeRooms == null) throw (new Exception($"Invalid file (no 'Rooms' element)"));
        var scene = SceneManager.GetActiveScene();
        //Finding tile size
        var scale = 2f / Convert.ToSingle(xeRoot.Element("Ash").Attribute("height").Value);
        //Generating rooms
        var chamberPrefab = Resources.Load<GameObject>("Chamber");
        var tiles = GetTiles();
        var index = 0;
        foreach (var xeRoom in xeRoot.Element("Rooms").Elements("Room"))
        {
            //Loading attributes
            if (!LoadedAttribute<string>.Load(xeRoom.Attribute("guid"), out var chamberGuid)) return;
            if (!LoadedAttribute<string>.Load(xeRoom.Attribute("name"), out var chamberName)) return;
            if (!LoadedAttribute<string>.Load(xeRoom.Attribute("theme"), out var theme)) return;
            if (!LoadedAttribute<string>.Load(xeRoom.Attribute("zoneGuid"), out var zoneGuid)) return;
            if (!LoadedAttribute<string>.Load(xeRoom.Attribute("cells"), out var cellsConcat)) return;
            if (!LoadedAttribute<int>.Load(xeRoom.Attribute("x"), out var chamberX)) return;
            if (!LoadedAttribute<int>.Load(xeRoom.Attribute("y"), out var chamberY)) return;
            if (!LoadedAttribute<int>.Load(xeRoom.Attribute("w"), out var chamberWidth)) return;
            if (!LoadedAttribute<int>.Load(xeRoom.Attribute("h"), out var chamberHeight)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("colorShiftR"), out var colorShiftR)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("colorShiftG"), out var colorShiftG)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("colorShiftB"), out var colorShiftB)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("backgroundLightIntensity"), out var backgroundLightIntensity)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("backgroundLightColorR"), out var backgroundLightColorR)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("backgroundLightColorG"), out var backgroundLightColorG)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("backgroundLightColorB"), out var backgroundLightColorB)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("terrainLightIntensity"), out var terrainLightIntensity)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("terrainLightColorR"), out var terrainLightColorR)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("terrainLightColorG"), out var terrainLightColorG)) return;
            if (!LoadedAttribute<float>.Load(xeRoom.Attribute("terrainLightColorB"), out var terrainLightColorB)) return;
            //Creating game object
            if (!TryGetExistingChamber(chamberGuid, out var chamberGameObject))
            {
                chamberGameObject = GameObject.Instantiate<GameObject>(chamberPrefab, chambersFolder.transform);
                if (chamberGameObject == null) throw (new Exception($"Chamber resource not found"));
            }
            var gridGameObject = GlobalFunctions.FindChildrenWithTag(chamberGameObject, "Grid", false).FirstOrDefault();
            if (gridGameObject == null) throw (new Exception($"Grid not found"));
            var tilemap = gridGameObject.GetComponentInChildren<Tilemap>();
            if (tilemap == null) throw (new Exception($"Tilemap not found"));
            chamberGameObject.name = chamberName;
            var chamber = chamberGameObject.GetComponentInChildren<ChamberController>();
            chamber.chamberName = chamberName;
            chamber.theme = theme;
            chamber.colorShiftR = colorShiftR;
            chamber.colorShiftG = colorShiftG;
            chamber.colorShiftB = colorShiftB;
            chamber.BackgroundLightIntensity = backgroundLightIntensity;
            chamber.BackgroundLightColor = new Color(backgroundLightColorR, backgroundLightColorG, backgroundLightColorB);
            chamber.TerrainLightIntensity = terrainLightIntensity;
            chamber.TerrainLightColor = new Color(terrainLightColorR, terrainLightColorG, terrainLightColorB);
            chamber.colorShiftG = colorShiftG;
            chamber.colorShiftB = colorShiftB;
            var chamberPosition = new Vector2(Convert.ToSingle(chamberX) * scale, -Convert.ToSingle(chamberY) * scale);
            chamber.SetBasicSettings(chamberGuid, chamberPosition, chamberWidth, chamberHeight, scale);
            gridGameObject.transform.position = new Vector3(chamberPosition.x, chamberPosition.y + 50f, 0f);
            gridGameObject.transform.localScale = new Vector3(scale, -scale, 1f);
            var cells = new int[chamberWidth, chamberHeight];
            chamber.grassManager.ClearGrass();
            if (cellsConcat != string.Empty)
            {
                var cellsInfo = cellsConcat.Split(';');
                foreach (var cellInfo in cellsInfo)
                {
                    var cell = cellInfo.Split(',');
                    var x = Convert.ToInt32(cell[0]);
                    var y = Convert.ToInt32(cell[1]);
                    var b = Convert.ToInt32(cell[2]);
                    var g = Convert.ToInt32(cell[3]);
                    cells[x, y] = b;
                    if (g > 0)
                    {
                        chamber.grassManager.AddGrass(x, chamber.cellCount.y - y - 1, g);
                    }
                }
            }
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var b = cells[x, y];
                    var tile = tiles[b == 0 ? 0 : 1];
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    chamber.SetMapCell(x, chamber.cellCount.y - y - 1, b);
                }
            }
            var savePointResource = Resources.Load<GameObject>("SavePoint");
            foreach (var xeSavePoint in xeRoom.Elements("SavePoint"))
            {
                var savePointGameObject = GameObject.Instantiate<GameObject>(savePointResource, savePointsFolder.transform);
                var savePointUnscaledX = int.TryParse(xeSavePoint.Attribute("x")?.Value ?? "", out var x) ? Convert.ToSingle(x + 1) : 0f;
                var savePointUnscaledY = int.TryParse(xeSavePoint.Attribute("y")?.Value ?? "", out var y) ? Convert.ToSingle(y + 1) : 0f;
                var savePointX = savePointUnscaledX * scale;
                var savePointY = savePointUnscaledY * scale;
                var ay = (chamberPosition.y + 50f - savePointUnscaledY * scale);
                savePointGameObject.transform.position = new Vector3(chamberPosition.x + savePointX, ay, 0f);
                var savePointController = savePointGameObject.GetComponent<SavePointController>();
                savePointController.forcedGuid = xeSavePoint.Attribute("guid").Value;
                index++;
            }
            ManageShadowsContainer(chamber);
        }
        Debug.Log($"Chambers updated successfully");

    }

    private static void ManageShadowsContainer(ChamberController chamberController)
    {
        const string CONTAINER_NAME = "Shadows";
        //Removing old container
        var shadowsContainerTransform = chamberController.transform.Find(CONTAINER_NAME);
        if (shadowsContainerTransform != null)
        {
            GameObject.DestroyImmediate(shadowsContainerTransform.gameObject);
        }
        //Creating container
        var shadowsContainerPrefab = Resources.Load<GameObject>("ShadowsContainer");
        var shadowsContainer = GameObject.Instantiate(shadowsContainerPrefab, chamberController.transform);
        shadowsContainer.name = CONTAINER_NAME;
        //Positioning container
        var x = chamberController.position.x;
        var y = chamberController.gameObject.GetComponentInChildren<Grid>().gameObject.transform.position.y - chamberController.size.y + 0.5f;
        shadowsContainer.transform.localPosition = new Vector3(x, y);
        //Generating shadow casters
        var shadowCastersController = shadowsContainer.GetComponent<ShadowCastersController>();
        shadowCastersController.Generate();
    }

    private static bool TryGetExistingChamber(string guid, out GameObject chamberGameObject)
    {
        chamberGameObject = null;
        foreach (var chamberLoopItem in GameObject.FindGameObjectsWithTag("Chamber"))
        {
            if (!chamberLoopItem.TryGetComponent<ChamberController>(out var chamberControllerLoopItem)) continue;
            if (chamberControllerLoopItem.chamberGuid != guid) continue;
            chamberGameObject = chamberLoopItem;
            break;
        }
        return chamberGameObject != null;
    }

    private static GameObject FindCreateFolder(string tag, string name, bool deleteContent)
    {
        var folder = GameObject.FindGameObjectWithTag(tag);
        if (folder == null)
        {
            folder = new GameObject(name);
            folder.tag = tag;
        }
        else
        {
            //Deleting previous objects
            if (deleteContent)
            {
                while (folder.transform.childCount != 0)
                {
                    GameObject.DestroyImmediate(folder.transform.GetChild(0).gameObject);
                }
            }
        }
        return folder;
    }

    private static List<Tile> GetTiles()
    {
        var tiles = new List<Tile>();
        tiles.Add(null);
        tiles.Add(Resources.Load<Tile>("TileBlack"));
        return tiles;
    }

    private static class LoadedAttribute<T>
    {
        internal static bool Load(XAttribute xa, out T value)
        {
            if (xa == null)
            {
                value = default(T);
                throw (new Exception($"Missing attribute '{xa.Name}'"));
            }
            try
            {
                if (typeof(T) == typeof(int))
                {
                    value = (T)(object)Convert.ToInt32(xa.Value);
                    return true;
                }
                else if (typeof(T) == typeof(float))
                {
                    value = (T)(object)Convert.ToSingle(xa.Value);
                    return true;
                }
                else if (typeof(T) == typeof(string))
                {
                    value = (T)(object)xa.Value;
                    return true;
                }
            }
            catch (Exception)
            {
                throw (new Exception($"Invalid attribute '{xa.Name}'"));
            }
            throw (new Exception($"Invalid type for attribute '{xa.Name}'"));
        }
    }
}