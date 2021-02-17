#if UNITY_EDITOR
using UnityEngine;

namespace Classes
{
    public static class MeshPointAnalyzerTest
    {
        public static void SmallSquare()
        {
            var map = new int[2, 2];
            map[0, 0] = 1;
            map[0, 1] = 1;
            map[1, 0] = 1;
            map[1, 1] = 1;
            var meshPointAnalyzer = new MeshPointAnalyzer(map);
            var pointGroups = meshPointAnalyzer.GetPointGroups();
            Debug.Log($"SmallSquare: {pointGroups.Count == 1 && pointGroups[0].Count == 4}");
        }
        
        public static void Plus()
        {
            var map = new int[3, 3];
            map[0, 0] = 0;
            map[0, 1] = 1;
            map[0, 2] = 0;
            map[1, 0] = 1;
            map[1, 1] = 1;
            map[1, 2] = 1;
            map[2, 0] = 0;
            map[2, 1] = 1;
            map[2, 2] = 0;
            var meshPointAnalyzer = new MeshPointAnalyzer(map);
            var pointGroups = meshPointAnalyzer.GetPointGroups();
            Debug.Log($"Plus: {pointGroups.Count == 1 && pointGroups[0].Count == 12}");
        }
    }
}
#endif