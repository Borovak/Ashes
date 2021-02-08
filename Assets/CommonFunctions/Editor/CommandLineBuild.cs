using UnityEditor;

namespace CommonFunctions.Editor
{
    public static class CommandLineBuild
    {
        public static void BuildPlayer()
        {
            string[] scenes = { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/LevelDesignerLoader.unity" };
            BuildPipeline.BuildPlayer(scenes, "D:/AshesLatestBuild/Ashes.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        }    
    }
}
