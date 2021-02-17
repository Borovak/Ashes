using System.Collections.Generic;
using UnityEngine;

namespace Classes
{
    public class MeshPointAnalyzer
    {
        private readonly int[,] _map;
        
        public MeshPointAnalyzer(int[,] map)
        {
            _map = map;
        }
        

        // 1--3
        // |  |
        // 0--2
        public List<List<Vector2>> GetPointGroups()
        {
            MeshEdge.Clear();
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y] <= 0) continue;
                    GetNeighbors(x, y, out var left, out var right, out var top, out var bottom);
                    var pnt0 = new Vector2(x, y);
                    var pnt1 = new Vector2(x, y + 1);
                    var pnt2 = new Vector2(x + 1, y);
                    var pnt3 = new Vector2(x + 1, y + 1);
                    //Debug.Log($"GetPoints MeshEdge.Add: x: {x}, y: {y}, left: {left}, right: {right}, top: {top}, bottom: {bottom}");
                    if (!left) MeshEdge.Add(pnt0, pnt1);
                    if (!right) MeshEdge.Add(pnt2, pnt3);
                    if (!top) MeshEdge.Add(pnt1, pnt3);
                    if (!bottom) MeshEdge.Add(pnt0, pnt2);
                }
            }
            MeshEdge.Clean();
            var points = MeshEdge.GetCorners();
            return points;
        }

        private void GetNeighbors(int x, int y, out bool left, out bool right, out bool top, out bool bottom)
        {
            left = x > 0 && _map[x - 1, y] >= 1;
            right = x < _map.GetLength(0) - 1 && _map[x + 1, y] >= 1;
            bottom = y > 0 && _map[x, y - 1] >= 1;
            top = y < _map.GetLength(1) - 1 && _map[x, y + 1] >= 1;
        }
    }
}