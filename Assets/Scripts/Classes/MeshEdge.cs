using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Classes
{
    internal class MeshEdge
    {
        private static List<MeshEdge> _edges = new List<MeshEdge>();
        private static int _counter;

        internal int index;
        internal Vector2 a;
        internal Vector2 b;
        internal bool isVertical => a.x != b.x;
        internal bool isClean;

        internal static void Add(Vector2 a, Vector2 b)
        {
            //if (_edges.Any(e => e.a.Equals(a) && e.b.Equals(b))) return;
            var edge = new MeshEdge {index = _counter++, a = a, b = b};
            _edges.Add(edge);
            //Debug.Log($"MeshEdge.Add: {a.x}, {a.y}, {b.x}, {b.y}");
        }
        
        internal static void Clear()
        {
            _edges.Clear();
            _counter = 0;
            //Debug.Log($"MeshEdge.Clear");
        }
        
        internal static void Clean()
        {
            var beginningEdgeCount = _edges.Count;
            while (true)
            {
                var edge = _edges.FirstOrDefault(e => !e.isClean);
                if (edge == null) break;
                while (true)
                {
                    var nextEdge = _edges.FirstOrDefault(e => e.isVertical == edge.isVertical && edge.b.Equals(e.a));
                    if (nextEdge == null)
                    {
                        edge.isClean = true;
                        break;
                    }
                    edge.b = nextEdge.b;
                    _edges.Remove(nextEdge);
                }
            }
            //Debug.Log($"MeshEdge.Clean: {_edges.Count} vs {beginningEdgeCount}");
        }

        internal static List<List<Vector2>> GetCorners()
        {
            var pointGroups = new List<List<Vector2>>();
            var chain = new HashSet<MeshEdge>();
            while (_edges.Count != chain.Count)
            {
                var points = new List<Vector2>();
                var edge = _edges.First(e => !chain.Contains(e));
                points.Add(edge.a);
                chain.Add(edge);
                while (edge != null)
                {
                    var nextPoint = points.Last().Equals(edge.a) ? edge.b : edge.a;
                    //Debug.Log($"Chain: {edge.a.x}, {edge.a.y}, {edge.b.x}, {edge.b.y}; next: {nextPoint.x}, {nextPoint.y}");
                    edge = _edges.Where(e => !chain.Contains(e)).FirstOrDefault(e => e.a.Equals(nextPoint) || e.b.Equals(nextPoint));
                    if (edge != null)
                    {
                        chain.Add(edge);
                        points.Add(nextPoint);
                    }
                }
                pointGroups.Add(points);
            }
            //Debug.Log($"MeshEdge.GetCorners: {points.Count} => {string.Join(";", chain.Select(e => $"{e.a.x}, {e.a.y}, {e.b.x}, {e.b.y}"))}");
            return pointGroups;
        }
    }
}