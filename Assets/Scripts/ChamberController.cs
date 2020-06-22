using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class ChamberController : MonoBehaviour
{
    public const float unitSize = 50f;
    public int x;
    public int y;
    public int w = 1;
    public int h = 1;
    public int region;
    public Cinemachine.CinemachineVirtualCamera Camera;
    public float BackgroundLightIntensity;
    public Color BackgroundLightColor;
    public float TerrainLightIntensity;
    public Color TerrainLightColor;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            virtualCamera.Follow = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x != unitSize * x || transform.position.y != unitSize * y)
        {
            transform.position = new Vector3(unitSize * x, unitSize * y, 0f);
            var polygonCollider = GetComponent<PolygonCollider2D>();
            var points = new List<Vector2>{
                new Vector2(0f, 0f),
                new Vector2(25f, 0f),
                new Vector2(unitSize * w, 0f),
                new Vector2(unitSize * w, unitSize * h),
                new Vector2(0f, unitSize * h),
            };
            polygonCollider.points = points.ToArray();
        }
    }
}
