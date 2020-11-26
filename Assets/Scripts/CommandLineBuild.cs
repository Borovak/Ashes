using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CommandLineBuild
{
    public static void BuildPlayer()
    {
        string[] scenes = { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/LevelDesignerLoader.unity" };
        BuildPipeline.BuildPlayer(scenes, @"C:\Users\Claude\Jenkins\workspace\Ashes\Build\Ashes.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }    
}
