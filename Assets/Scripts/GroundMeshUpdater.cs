using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using UnityEngine;

[ExecuteInEditMode]
public class GroundMeshUpdater : MonoBehaviour
{
    public bool requestUpdate = false;
    public GameObject terrainMeshPrefab;

    private Transform _meshFolder;
    private ChamberController _chamberController;
    

    // Update is called once per frame
    void Update()
    {
        if (!requestUpdate) return;
        requestUpdate = false;
        //MeshPointAnalyzerTest.SmallSquare();
        //MeshPointAnalyzerTest.Plus();
        UpdateMeshes();
    }

    private void UpdateMeshes()
    {
        if (terrainMeshPrefab == null) return;
        _chamberController ??= GetComponent<ChamberController>();
        _meshFolder ??= transform.Find("Meshes");
        if (_meshFolder == null)
        {
            _meshFolder = new GameObject("Meshes").transform;
            _meshFolder.parent = transform;
        }
        var map = _chamberController.GetMap();
        var meshPointAnalyzer = new MeshPointAnalyzer(map);
        var pointGroups = meshPointAnalyzer.GetPointGroups();
        foreach (Transform childTransform in _meshFolder)
        {
            DestroyImmediate(childTransform.gameObject);
        }
        //Front
        foreach (var pointGroup in pointGroups)
        {
            var meshObject = Instantiate(terrainMeshPrefab, _meshFolder);
            if (!meshObject.TryGetComponent<MeshFilter>(out var meshFilter)) continue;
            UpdateMesh(meshFilter, pointGroup);
            for (int i = 0; i < pointGroup.Count; i++)
            {
                var meshObjectDepth = Instantiate(terrainMeshPrefab, _meshFolder);
                if (!meshObjectDepth.TryGetComponent<MeshFilter>(out var meshFilterDepth)) continue;
                CreateDepthMesh(meshFilterDepth,pointGroup[i], pointGroup[i == pointGroup.Count - 1 ? 0 : i + 1]);
            }
        }
    }

    private void UpdateMesh(MeshFilter meshFilter, List<Vector2> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = points[i] * _chamberController.scale + new Vector2(_chamberController.scale,0f);
        }
        var triangulator = new Triangulator(points.ToArray());
        var indices = triangulator.Triangulate().ToList();
        // Create the Vector3 vertices
        var vertices = new List<Vector3>();
        for (int i = 0; i < points.Count; i++) {
            vertices.Add(new Vector3(points[i].x, points[i].y, -2.5f));
        }
        // Create the mesh
        //Debug.Log($"{vertices.Count} vertices, {indices.Count} indices");
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

    private void CreateDepthMesh(MeshFilter meshFilter, Vector2 pointA, Vector2 pointB)
    {
        var vertices = new[]
        {
            new Vector3(pointB.x, pointB.y, -2.5f),
            new Vector3(pointA.x, pointA.y, -2.5f),
            new Vector3(pointA.x, pointA.y, 2.5f),
            new Vector3(pointB.x, pointB.y, 2.5f)
        };
        var indices = new[] {0, 1, 2, 0, 2, 3};
        // Create the mesh
        //Debug.Log($"{vertices.Count} vertices, {indices.Count} indices");
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }
}
