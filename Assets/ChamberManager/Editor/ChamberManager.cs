using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
        var buttons = new Dictionary<string, Action>
        {
            {"Wipe save files", SaveSystem.WipeFiles},
            {"Load level designer file", LevelDesigner.LoadSpecific},
            {"Load default level designer file", LevelDesigner.LoadDefault},
            {"Generate terrain decor", TerrainDecor.Generate},
        };
        foreach (var button in buttons)
        {
            if (GUILayout.Button(button.Key))
            {
                button.Value.Invoke();
            }
        }
    }

    
}
