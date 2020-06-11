using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ChamberManager : EditorWindow
{
    List<GameObject> chamberPrefabs;
    int chamberId;

    [MenuItem("Tools/Chamber Manager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChamberManager));
    }

    // Update is called once per frame
    private void OnGUI()
    {
        if (chamberPrefabs == null)
        {
            chamberPrefabs = new List<GameObject>();
            foreach (var filePath in Directory.GetFiles("./Assets/Resources/Chambers", "*.prefab"))
            {
                var fileInfo = new FileInfo(filePath);
                var nameWithoutExt = fileInfo.Name.Split('.')[0];
                var chamber = Resources.Load<GameObject>($"Chambers/{nameWithoutExt}");
                chamberPrefabs.Add(chamber);
            }
        }
        GUILayout.Label("Chamber Switcher", EditorStyles.boldLabel);
        if (GUILayout.Button("None"))
        {
            ChangeChamber(null);
        }
        for (int i = 0; i < chamberPrefabs.Count; i++)
        {
            if (GUILayout.Button(chamberPrefabs[i].name))
            {
                var chamberController = chamberPrefabs[i].GetComponent<ChamberController>();
                ChangeChamber(chamberController);
                break;
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
