using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class ChamberManager : EditorWindow
{
    private struct ChamberInfo{
        internal string name;
        internal string resourcePath;
    }
    Dictionary<int, List<ChamberInfo>> regions;

    [MenuItem("Tools/Chamber Manager")]
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
        if (SceneManager.GetActiveScene().name != "Game") return;
        if (regions == null)
        {
            var colliderFolder = GameObject.FindGameObjectWithTag("ChamberCollidersFolder").transform;
            DeleteOldColliders(colliderFolder);
            regions = new Dictionary<int, List<ChamberInfo>>();
            foreach (var filePath in Directory.GetFiles("./Assets/Resources/Chambers", "*.prefab"))
            {
                var fileInfo = new FileInfo(filePath);
                var nameWithoutExt = fileInfo.Name.Split('.')[0];
                var resourcePath = $"Chambers/{nameWithoutExt}";
                var chamber = Resources.Load<GameObject>(resourcePath);
                var chamberController = chamber.GetComponent<ChamberController>();
                Transform regionFolder;
                if (!regions.ContainsKey(chamberController.region))
                {
                    regions.Add(chamberController.region, new List<ChamberInfo>());
                    regionFolder = new GameObject(RegionAnnouncementController.GetText(chamberController.region)).transform;
                    regionFolder.transform.parent = colliderFolder;
                }
                else
                {
                    regionFolder = colliderFolder.Find(RegionAnnouncementController.GetText(chamberController.region));
                }
                regions[chamberController.region].Add(new ChamberInfo {name = chamber.name, resourcePath = resourcePath});
                CreateChamberCollider(ref regionFolder, chamberController, resourcePath);
            }
        }
        GUILayout.Label("Action", EditorStyles.boldLabel);
        if (GUILayout.Button("Refresh"))
        {
            regions = null;
            return;
        }
        if (GUILayout.Button("Clear Chambers"))
        {
            ChangeChamber(null);
        }
        foreach (var region in regions)
        {
            GUILayout.Label(RegionAnnouncementController.GetText(region.Key), EditorStyles.boldLabel);
            foreach (var chamber in region.Value)
            {
                if (GUILayout.Button(chamber.name))
                {
                    ChangeChamber(chamber.resourcePath);
                    break;
                }
            }
        }
    }

    private void CreateChamberCollider(ref Transform regionFolder, ChamberController chamber, string resourcePath)
    {
        var colliderPrefab = Resources.Load<GameObject>($"ChamberCollider");
        var colliderObject = GameObject.Instantiate(colliderPrefab, regionFolder);        
        colliderObject.transform.position = Vector3.zero;
        colliderObject.name = chamber.name;
        var collider = colliderObject.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(chamber.w * ChamberController.unitSize, chamber.h * ChamberController.unitSize);
        collider.offset = new Vector2(chamber.x * ChamberController.unitSize + chamber.w * ChamberController.unitSize / 2f, chamber.y * ChamberController.unitSize + chamber.h * ChamberController.unitSize / 2f);
        var chamberLoader = colliderObject.GetComponent<ChamberColliderLoader>();
        chamberLoader.chamberResourcePath = resourcePath;
    }

    private void DeleteOldColliders(Transform colliderFolder)
    {
        for (int i = colliderFolder.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(colliderFolder.GetChild(i).gameObject);
        }
    }

    private void ChangeChamber(string resourcePath)
    {
        var chambersFolder = GameObject.FindGameObjectWithTag("ChambersFolder").transform;
        for (int i = 0; i < chambersFolder.childCount; i++)
        {
            GameObject.DestroyImmediate(chambersFolder.GetChild(i).gameObject);
        }        
        GameController.ChangeChamber(resourcePath);
        UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
        UnityEditor.SceneView.RepaintAll();
    }
}
