using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ChamberManager : EditorWindow
{
    Dictionary<int, List<GameObject>> regions;

    [MenuItem("Tools/Chamber Manager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChamberManager));
    }

    // Update is called once per frame
    private void OnGUI()
    {
        if (regions == null)
        {
            regions = new Dictionary<int, List<GameObject>>();
            foreach (var filePath in Directory.GetFiles("./Assets/Resources/Chambers", "*.prefab"))
            {
                var fileInfo = new FileInfo(filePath);
                var nameWithoutExt = fileInfo.Name.Split('.')[0];
                var chamber = Resources.Load<GameObject>($"Chambers/{nameWithoutExt}");
                var chamberController = chamber.GetComponent<ChamberController>();
                if (!regions.ContainsKey(chamberController.region))
                {
                    regions.Add(chamberController.region, new List<GameObject>());
                }
                regions[chamberController.region].Add(chamber);
            }
        }
        if (GUILayout.Button("None"))
        {
            ChangeChamber(null);
        }
        foreach (var zone in regions)
        {
            GUILayout.Label(RegionAnnouncementController.GetText(zone.Key), EditorStyles.boldLabel);
            foreach (var chamber in zone.Value)
            {
                if (GUILayout.Button(chamber.name))
                {
                    var chamberController = chamber.GetComponent<ChamberController>();
                    ChangeChamber(chamberController);
                    break;
                }
            }
        }
    }

    private void ChangeChamber(ChamberController chamberController)
    {
        GameController.currentChamber = chamberController;
        UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
        UnityEditor.SceneView.RepaintAll();
    }
}
