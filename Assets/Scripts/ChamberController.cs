using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ChamberController : MonoBehaviour
{
    public static string[] ContainerNames => new[] { Constants.NAME_TERRAINCONTAINER, Constants.NAME_ENVIRONMENTCONTAINER, Constants.NAME_SHADOWSCONTAINER, Constants.NAME_NPCCONTAINER, Constants.NAME_GRASSCONTAINER, Constants.NAME_OVERHANGCONTAINER, Constants.NAME_BACKGROUNDCONTAINER };
    public static event Action<string, string> ZoneChanged;
    public string chamberGuid;
    public LocationInformation.Chamber chamber => LocationInformation.Chambers[chamberGuid];
    public string chamberName;
    public LocationInformation.Zone zone => LocationInformation.Zones[chamber.ZoneGuid];
    public string zoneGuid => chamber.ZoneGuid;
    public float BackgroundLightIntensity;
    public Color BackgroundLightColor;
    public float TerrainLightIntensity;
    public Color TerrainLightColor;
    public GameObject chamberContainer;
    public Vector2 position;
    public Vector2 size;
    public Rect chamberBounds;
    public Vector2Int cellCount;
    public float scale;
    [SerializeField] public int[] map1D;
    [SerializeField] public string theme;
    [SerializeField] public float colorShiftR;
    [SerializeField] public float colorShiftG;
    [SerializeField] public float colorShiftB;
    [SerializeField] public string grass;
    public AudioClip ambientAudioClip;
    public AudioClip musicAudioClip;
    public bool isPlayerHere;
    public GrassManager grassManager
    {
        get
        {
            if (_grassManager == null)
            {
                _grassManager = new GrassManager(this);
            }
            return _grassManager;
        }
    }

    private static LocationInformation.Zone _lastZoneEntered;
    private bool _isPlayerInsideChamber => GlobalFunctions.TryGetPlayer(out var playerGameObject) ? chamberBounds.Contains(playerGameObject.transform.position) : false;
    private Transform _enemyFolder;
    private VirtualCameraPlayerBinding _virtualCameraPlayerBinding;    
    private bool _previousIsPlayerInsideChamber;
    private List<GameObject> _containersToEnableDisable;
    private ChamberAudioManager _chamberAudioManager;
    private GrassManager _grassManager;

    // Start is called before the first frame update
    void Start()
    {
        //Creating audio manager
        _chamberAudioManager = new ChamberAudioManager(this, ambientAudioClip, musicAudioClip);
        //
        _virtualCameraPlayerBinding = GetComponentInChildren<VirtualCameraPlayerBinding>();
        _enemyFolder = new GameObject("Enemies").transform;
        _enemyFolder.parent = transform;
        var tilemapRenderer = GetComponentInChildren<TilemapRenderer>();
        tilemapRenderer.enabled = false;
        //Find containers to enable/disable        
        _containersToEnableDisable = new List<GameObject>();
        foreach (var containerName in ContainerNames)
        {
            var obj = transform.Find(containerName);
            if (obj == null) continue;
            _containersToEnableDisable.Add(obj.gameObject);
        }
        //Debug.Log($"{chamberName}: {_containersToEnableDisable.Count} containers");
        foreach (var container in _containersToEnableDisable)
        {
            container.SetActive(false);
        }
        GameController.PlayerSpawned += OnPlayerSpawned;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameState != GameController.GameStates.Running || !GlobalFunctions.TryGetPlayer(out var playerGameObject)) return;
        //Spawning/despawning enemies
        if (_previousIsPlayerInsideChamber && !_isPlayerInsideChamber)
        {
            DeleteEnemiesOnExit();
            foreach (var container in _containersToEnableDisable)
            {
                container?.SetActive(false);
            }
            _chamberAudioManager.Stop();
        }
        else if (!_previousIsPlayerInsideChamber && _isPlayerInsideChamber)
        {
            if (_lastZoneEntered == null || _lastZoneEntered.Guid != chamber.ZoneGuid)
            {
                _lastZoneEntered = chamber.Zone;
                ZoneChanged?.Invoke(chamber.ZoneName, chamber.Name);
            }
            CreateEnemiesOnEnter();
            foreach (var container in _containersToEnableDisable)
            {
                container?.SetActive(true);
            }
            Apply();
        }
        else return;
        _previousIsPlayerInsideChamber = _isPlayerInsideChamber;
    }

    private void CreateEnemiesOnEnter()
    {
        Debug.Log($"{chamberName} enemies created");
        foreach (var enemy in chamber.Enemies)
        {
            enemy.Instantiate(_enemyFolder, position, size, scale);
        }
        FadeInOutController.FadeIn();
    }

    private void DeleteEnemiesOnExit()
    {
        if (_enemyFolder == null) return;
        var enemiesToDelete = GlobalFunctions.FindChildrenWithTag(_enemyFolder.gameObject, "Enemy", false);
        foreach (var enemy in enemiesToDelete)
        {
            GameObject.Destroy(enemy);
        }
    }

    private void OnPlayerSpawned(GameObject playerGameObject)
    {
        if (_previousIsPlayerInsideChamber && _isPlayerInsideChamber)
        {
            DeleteEnemiesOnExit();
            CreateEnemiesOnEnter();
        }
    }

    void Apply()
    {
        _chamberAudioManager.Start();
        GlobalLightController.UpdateLights(BackgroundLightIntensity, BackgroundLightColor, TerrainLightIntensity, TerrainLightColor);
    }

    public void SetBasicSettings(string guid, Vector2 position, int width, int height, float scale)
    {
        chamberGuid = guid;
        this.position = position;
        this.size = new Vector2(Convert.ToSingle(width) * scale, Convert.ToSingle(height) * scale);
        this.cellCount = new Vector2Int(width, height);
        this.map1D = new int[width * height];
        this.scale = scale;
        SetCollider();
    }

    private void SetCollider()
    {
        var minX = position.x;
        var maxX = position.x + size.x;
        var biasY = 50f - size.y;
        var minY = position.y + biasY;
        var maxY = position.y + size.y + biasY;
        chamberBounds = new Rect(minX, minY, maxX - minX, maxY - minY);
        var points = new List<Vector2>
        {
            { new Vector2(minX, minY) },
            { new Vector2(minX + 1f, minY) },
            { new Vector2(maxX, minY) },
            { new Vector2(maxX, maxY) },
            { new Vector2(minX, maxY) }
        };
        var colliderGameObject = new List<GameObject>(GameObject.FindGameObjectsWithTag("ChamberCollider")).Find(g => g.transform.IsChildOf(transform));
        var collider = colliderGameObject.GetComponent<PolygonCollider2D>();
        collider.SetPath(0, points);
    }

    public int[,] GetMap()
    {
        var map = new int[cellCount.x, cellCount.y];
        for (int x = 0; x < cellCount.x; x++)
        {
            for (int y = 0; y < cellCount.y; y++)
            {
                map[x, y] = GetMapCell(x, y);
            }
        }
        return map;
    }

    public int GetMapCell(int x, int y)
    {
        var index = GetMapIndex(x, y);
        return map1D[index];
    }

    public void SetMapCell(int x, int y, int value)
    {
        var index = GetMapIndex(x, y);
        map1D[index] = value;
    }

    private int GetMapIndex(int x, int y)
    {
        return x * cellCount.y + y;
    }


}
