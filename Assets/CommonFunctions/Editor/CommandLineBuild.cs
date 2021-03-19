using UnityEditor;

namespace CommonFunctions.Editor
{
    public static class CommandLineBuild
    {
        public static void BuildPlayer()
        {
            string[] scenes = { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Game.unity" };
            BuildPipeline.BuildPlayer(scenes, "C:/Users/Public/Documents/AshesBuild/Ashes.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        }    
    }
}
