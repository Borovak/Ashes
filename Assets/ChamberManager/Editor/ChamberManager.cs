using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using System;
using UnityEngine.Tilemaps;

public class ChamberManager : EditorWindow
{

    [MenuItem("Tools/Ashes Helper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChamberManager));
    }

    // Update is called once per frame
    private void OnGUI()
    {
        if (GUILayout.Button("Wipe save files"))
        {
            SaveSystem.WipeFiles();    
        }
        if (GUILayout.Button("Load Tilemap From Level Designer"))
        {
            LoadTilemapFromLevelDesigner();  
        }
        //if (SceneManager.GetActiveScene().name != "Game") return;
        // if (regions == null)
        // {
        //     var colliderFolder = GameObject.FindGameObjectWithTag("ChamberCollidersFolder").transform;
        //     DeleteOldColliders(colliderFolder);
        //     regions = new Dictionary<int, List<ChamberInfo>>();
        //     foreach (var filePath in Directory.GetFiles("./Assets/Resources/Chambers", "*.prefab"))
        //     {
        //         var fileInfo = new FileInfo(filePath);
        //         var nameWithoutExt = fileInfo.Name.Split('.')[0];
        //         var resourcePath = $"Chambers/{nameWithoutExt}";
        //         var chamber = Resources.Load<GameObject>(resourcePath);
        //         var chamberController = chamber.GetComponent<ChamberController>();
        //         Transform regionFolder;
        //         if (!regions.ContainsKey(chamberController.region))
        //         {
        //             regions.Add(chamberController.region, new List<ChamberInfo>());
        //             regionFolder = new GameObject(RegionAnnouncementController.GetText(chamberController.region)).transform;
        //             regionFolder.transform.parent = colliderFolder;
        //         }
        //         else
        //         {
        //             regionFolder = colliderFolder.Find(RegionAnnouncementController.GetText(chamberController.region));
        //         }
        //         regions[chamberController.region].Add(new ChamberInfo {name = chamber.name, resourcePath = resourcePath});
        //         CreateChamberCollider(ref regionFolder, chamberController, resourcePath);
        //     }
        // }
        // GUILayout.Label("Action", EditorStyles.boldLabel);
        // if (GUILayout.Button("Refresh"))
        // {
        //     regions = null;
        //     return;
        // }
        // if (GUILayout.Button("Clear Chambers"))
        // {
        //     ChangeChamber(null);
        // }
        // foreach (var region in regions)
        // {
        //     GUILayout.Label(RegionAnnouncementController.GetText(region.Key), EditorStyles.boldLabel);
        //     foreach (var chamber in region.Value)
        //     {
        //         if (GUILayout.Button(chamber.name))
        //         {
        //             ChangeChamber(chamber.resourcePath);
        //             break;
        //         }
        //     }
        // }
    }

    // private void CreateChamberCollider(ref Transform regionFolder, ChamberController chamber, string resourcePath)
    // {
    //     var colliderPrefab = Resources.Load<GameObject>($"ChamberCollider");
    //     var colliderObject = GameObject.Instantiate(colliderPrefab, regionFolder);        
    //     colliderObject.transform.position = Vector3.zero;
    //     colliderObject.name = chamber.name;
    //     var collider = colliderObject.GetComponent<BoxCollider2D>();
    //     collider.size = new Vector2(chamber.w * ChamberController.unitSize, chamber.h * ChamberController.unitSize);
    //     collider.offset = new Vector2(chamber.x * ChamberController.unitSize + chamber.w * ChamberController.unitSize / 2f, chamber.y * ChamberController.unitSize + chamber.h * ChamberController.unitSize / 2f);
    //     var chamberLoader = colliderObject.GetComponent<ChamberColliderLoader>();
    //     chamberLoader.chamberResourcePath = resourcePath;
    // }

    // private void DeleteOldColliders(Transform colliderFolder)
    // {
    //     for (int i = colliderFolder.childCount - 1; i >= 0; i--)
    //     {
    //         GameObject.DestroyImmediate(colliderFolder.GetChild(i).gameObject);
    //     }
    // }
    
    private void LoadTilemapFromLevelDesigner(){
        var path = @"C:\Users\Borovak\Documents\1.mcm";
        if (!System.IO.File.Exists(path)){
            throw(new Exception($"Cannot find file '{path}'"));
        }
        XElement xeRoot = null;
        try{
            xeRoot = XElement.Load(path);
        } catch (Exception){
            throw(new Exception("Cannot parse file"));
        }
        var sceneName = SceneManager.GetActiveScene().name;
        var tilemapObject = GameObject.FindGameObjectWithTag("Tilemap");
        if (tilemapObject == null){            
            throw(new Exception($"Tilemap tag not set"));
        }
        var tilemap = tilemapObject.GetComponent<Tilemap>();
        var tiles = GetTiles();
        var xeRooms = xeRoot.Element("Rooms");
        if (xeRooms == null){            
            throw(new Exception($"Invalid file (no 'Rooms' element)"));
        }
        foreach (var xeRoom in xeRooms.Elements("Room")){
            var xaName = xeRoom.Attribute("name");
            if (xaName == null || xaName.Value != sceneName) continue;
            var xaCells = xeRoom.Attribute("cells");
            if (xaCells == null)
            {            
                throw(new Exception($"Invalid file (no 'cells' attribute)"));
            }
            var xaWidth = xeRoom.Attribute("w");
            if (xaWidth == null)
            {            
                throw(new Exception($"Invalid file (no 'w' attribute)"));
            }
            var xaHeight = xeRoom.Attribute("h");
            if (xaHeight == null)
            {            
                throw(new Exception($"Invalid file (no 'h' attribute)"));
            }
            var cellsInfo = xaCells.Value.Split(';');
            var cells = new int[Convert.ToInt32(xaWidth.Value), Convert.ToInt32(xaHeight.Value)];
            foreach (var cellInfo in cellsInfo){
                var cell = cellInfo.Split(',');
                var x = Convert.ToInt32(cell[0]);
                var y = Convert.ToInt32(cell[1]);
                var b = Convert.ToInt32(cell[2]);
                cells[x,y] = b;
            }
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {                
                    var b = cells[x,y];
                    var tile = tiles[b == 0 ? 0 : 1];
                    tilemap.SetTile(new Vector3Int(x, cells.GetLength(0) - 1 - y, 0), tile);
                }
            }
            Debug.Log($"Tilemap updated successfully");
            return;
        }
        throw(new Exception($"No room data found matching this scene"));
    }

    private List<Tile> GetTiles(){
        var tiles = new List<Tile>();
        tiles.Add(null);
        tiles.Add(Resources.Load<Tile>("TileBlack"));
        return tiles;
    }

}
