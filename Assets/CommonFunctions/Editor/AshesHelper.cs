using System;
using System.Collections.Generic;
using Static;
using UnityEditor;
using UnityEngine;

namespace CommonFunctions.Editor
{
    public class AshesHelper : EditorWindow
    {

        [MenuItem("Tools/Ashes Helper")]
        public static void ShowWindow()
        {
            GetWindow(typeof(AshesHelper));
        }

        // Update is called once per frame
        private void OnGUI()
        {
            var buttons = new Dictionary<string, Action>
            {
                {"Wipe save files", SaveSystem.WipeFiles},
                {"Load level designer file", LevelDesigner.LoadSpecific},
                {"Load default level designer file", LevelDesigner.LoadDefault},
                {"Generate terrain/grass/overhangs", AutoDecor.Generate},
            };
            foreach (var button in buttons)
            {
                if (GUILayout.Button(button.Key))
                {
                    Utils.ClearLogConsole();
                    button.Value.Invoke();
                }
            }
        }

    
    }
}
