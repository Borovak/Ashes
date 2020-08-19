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
            var path = EditorUtility.OpenFilePanel("Select level designer file", "", "xml");
            if (string.IsNullOrEmpty(path)) return;
            if (!System.IO.File.Exists(path))
            {
                EditorUtility.DisplayDialog("Select level designer file", "The path is invalid", "OK");
                return;
            }
            LoadTilemapFromLevelDesigner(path);  
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
    
    private void LoadTilemapFromLevelDesigner(string path){
        var chambersFolder = GameObject.FindGameObjectWithTag("ChambersFolder");
        if (chambersFolder == null){
            chambersFolder = new GameObject("Chambers");
            chambersFolder.tag = "ChambersFolder";
        }
        XElement xeRoot = null;
        try{
            xeRoot = XElement.Load(path);
        } catch (Exception){
            throw(new Exception("Cannot parse file"));
        }
        var xeRooms = xeRoot.Element("Rooms");
        if (xeRooms == null){            
            throw(new Exception($"Invalid file (no 'Rooms' element)"));
        }
        var scene = SceneManager.GetActiveScene();    
        //Deleting previous objects
        while(chambersFolder.transform.childCount != 0){
             DestroyImmediate(chambersFolder.transform.GetChild(0).gameObject);
         }
        //Finding tile size
        var scale = 2f / Convert.ToSingle(xeRoot.Element("Ash").Attribute("height").Value);
        //Generating rooms
        var chamberResource = Resources.Load<GameObject>("Chamber");
        var tiles = GetTiles();
        foreach (var xeRoom in xeRoot.Element("Rooms").Elements("Room")){
            var chamberGuid = xeRoom.Attribute("guid")?.Value ?? string.Empty;
            if (chamberGuid == string.Empty) 
            {            
                throw(new Exception($"Invalid file (no 'guid' attribute)"));
            }
            var chamberName = xeRoom.Attribute("name")?.Value ?? string.Empty;
            if (chamberName == string.Empty) 
            {            
                throw(new Exception($"Invalid file (no 'name' attribute)"));
            }
            var zoneGuid = xeRoom.Attribute("zoneGuid")?.Value ?? string.Empty;
            if (zoneGuid == string.Empty)
            {            
                throw(new Exception($"Invalid file (no 'zoneGuid' attribute)"));
            }
            var cellsConcat = xeRoom.Attribute("cells")?.Value ?? "!";
            if (cellsConcat == "!")
            {            
                throw(new Exception($"Invalid file (no 'cells' attribute)"));
            }
            if (!int.TryParse(xeRoom.Attribute("x")?.Value ?? string.Empty, out var chamberX))
            {            
                throw(new Exception($"Invalid file (no 'x' attribute)"));
            }
            if (!int.TryParse(xeRoom.Attribute("y")?.Value ?? string.Empty, out var chamberY))
            {            
                throw(new Exception($"Invalid file (no 'y' attribute)"));
            }
            if (!int.TryParse(xeRoom.Attribute("w")?.Value ?? string.Empty, out var chamberWidth))
            {            
                throw(new Exception($"Invalid file (no 'w' attribute)"));
            }
            if (!int.TryParse(xeRoom.Attribute("h")?.Value ?? string.Empty, out var chamberHeight))
            {            
                throw(new Exception($"Invalid file (no 'h' attribute)"));
            }
            var chamberGameObject = GameObject.Instantiate<GameObject>(chamberResource, chambersFolder.transform);
            if (chamberGameObject == null){            
                throw(new Exception($"Chamber resource not found"));
            }
            var gridGameObject = new List<GameObject>(GameObject.FindGameObjectsWithTag("Grid")).Find(g => g.transform.IsChildOf(chamberGameObject.transform));
            if (gridGameObject == null){            
                throw(new Exception($"Grid not found"));
            }
            var tilemap = gridGameObject.GetComponentInChildren<Tilemap>();
            if (tilemap == null){            
                throw(new Exception($"Tilemap not found"));
            }
            chamberGameObject.name = $"Chamber_{chamberName}";
            var chamber = chamberGameObject.GetComponentInChildren<ChamberController>();
            var position = new Vector2(Convert.ToSingle(chamberX) * scale, -Convert.ToSingle(chamberY) * scale);
            var size = new Vector2(Convert.ToSingle(chamberWidth) * scale, Convert.ToSingle(chamberHeight) * scale);
            chamber.SetBasicSettings(chamberGuid, position, size, scale);
            gridGameObject.transform.position = new Vector3(position.x, position.y + 50f, 0f);
            gridGameObject.transform.localScale = new Vector3(scale, -scale, 1f);
            var cells = new int[chamberWidth, chamberHeight];
            if (cellsConcat != string.Empty){         
            var cellsInfo = cellsConcat.Split(';');
                foreach (var cellInfo in cellsInfo){
                    var cell = cellInfo.Split(',');
                    var x = Convert.ToInt32(cell[0]);
                    var y = Convert.ToInt32(cell[1]);
                    var b = Convert.ToInt32(cell[2]);
                    cells[x,y] = b;
                }
            }    
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {                
                    var b = cells[x,y];
                    var tile = tiles[b == 0 ? 0 : 1];
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
            foreach (var xeSavePoint in xeRoom.Elements("SavePoint")){
                var savePointResource = Resources.Load<GameObject>("SavePoint");
                var savePointGameObject = GameObject.Instantiate<GameObject>(savePointResource, chamberGameObject.transform);
                var savePointUnscaledX = int.TryParse(xeSavePoint.Attribute("x")?.Value ?? "", out var x) ? Convert.ToSingle(x + 1) : 0f;
                var savePointUnscaledY = int.TryParse(xeSavePoint.Attribute("y")?.Value ?? "", out var y) ? Convert.ToSingle(y + 1) : 0f;
                var savePointX = savePointUnscaledX * scale;
                var savePointY = savePointUnscaledY * scale;
                savePointGameObject.transform.position = new Vector3(position.x + savePointX, position.y + (chamberHeight - savePointUnscaledY) * scale, 0f);
                var savePointController = savePointGameObject.GetComponent<SavePointController>();
                savePointController.guid = xeSavePoint.Attribute("guid").Value;
            }
        }
        Debug.Log($"Chambers updated successfully");
    }

    private List<Tile> GetTiles(){
        var tiles = new List<Tile>();
        tiles.Add(null);
        tiles.Add(Resources.Load<Tile>("TileBlack"));
        return tiles;
    }

}
