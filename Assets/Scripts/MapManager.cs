using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject mapRoomPrefab;
    public GameObject mapPinPrefab;
    public float sizeMultiplierInit = 5f;
    public Vector2 centeringBiasInit;
    public float moveSpeed = 2f;
    public float zoomSpeed = 1.2f;
    public float zoomMinimum = 2f;
    public float zoomMaximum = 16f;
    public float sizeMultiplier;
    public Vector2 centeringBias;
    public float zoomFactor;

    private List<GameObject> _mapRooms;
    private Dictionary<GameObject, Transform> _pins;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        centeringBias += MenuInputs.movement * moveSpeed;
        foreach (var mapRoom in _mapRooms)
        {
            var mapRoomController = mapRoom.GetComponent<MapRoomController>();
            var rectTransform = mapRoom.GetComponent<RectTransform>();
            rectTransform.sizeDelta = mapRoomController.chamberController.chamberBounds.size * sizeMultiplier;
            var playerOffsetVector3 = GlobalFunctions.TryGetPlayer(out var player) ? player.transform.position : Vector3.zero;
            var playerOffset = new Vector2(playerOffsetVector3.x, playerOffsetVector3.y);
            var chamberPosition = (mapRoomController.chamberController.chamberBounds.position + mapRoomController.chamberController.chamberBounds.size / 2f) * sizeMultiplier;
            var centeringOffset = centeringBias * sizeMultiplier;
            rectTransform.anchoredPosition = chamberPosition - centeringOffset + playerOffset;
            foreach (var pin in _pins)
            {
                if (pin.Value == null) continue;
                var offsetVector3 = pin.Value.position;
                var offset = new Vector2(offsetVector3.x, offsetVector3.y);
                var pinRectTransform = pin.Key.GetComponent<RectTransform>();
                pinRectTransform.anchoredPosition = new Vector2(offset.x, offset.y) * sizeMultiplier - centeringOffset + playerOffset;
            }
        }
    }

    void OnEnable()
    {
        MenuInputs.MapZoomIn += ZoomIn;
        MenuInputs.MapZoomOut += ZoomOut;
        sizeMultiplier = sizeMultiplierInit;
        centeringBias = centeringBiasInit;
        if (_mapRooms == null)
        {
            _mapRooms = new List<GameObject>();
        }
        if (_pins == null)
        {
            _pins = new Dictionary<GameObject, Transform>();
        }
        // var mapRooms = GameObject.FindGameObjectsWithTag("MapRoom");
        // foreach (var mapRoom in mapRooms)
        // {
        //     var mapRoomController = mapRoom.GetComponent<MapRoomController>();
        //     mapRoomController.mustBeDeleted = true;
        // }
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        _pins.Clear();
        _mapRooms.Clear();
        foreach (var chamberGameObject in GameObject.FindGameObjectsWithTag("Chamber"))
        {
            var chamberController = chamberGameObject.GetComponent<ChamberController>();
            var mapRoom = GameObject.Instantiate(mapRoomPrefab, transform);
            _mapRooms.Add(mapRoom);
            mapRoom.name = chamberController.chamberName;
            var mapRoomController = mapRoom.GetComponent<MapRoomController>();
            mapRoomController.chamberController = chamberController;
        }
        //Save point pin
        var savePoints = GameObject.FindGameObjectsWithTag("SavePoint");
        foreach (var savePoint in savePoints)
        {
            var savePointPin = GameObject.Instantiate(mapPinPrefab, transform);
            var savePointImage = savePointPin.GetComponent<Image>();
            savePointImage.color = Color.red;
            _pins.Add(savePointPin, savePoint.transform);
        }
        //Player pin
        var playerPin = GameObject.Instantiate(mapPinPrefab, transform);
        var playerImage = playerPin.GetComponent<Image>();
        playerImage.color = Color.green;
        GlobalFunctions.TryGetPlayer(out var playerGameObject);
        _pins.Add(playerPin, playerGameObject.transform);
    }

    void OnDisable()
    {
        MenuInputs.MapZoomIn -= ZoomIn;
        MenuInputs.MapZoomOut -= ZoomOut;
    }

    private void ZoomIn()
    {
        Zoom(true);
    }

    private void ZoomOut()
    {
        Zoom(false);
    }

    private void Zoom(bool direction)
    {
        zoomFactor = direction ? zoomSpeed : 1f / zoomSpeed;
        sizeMultiplier *= zoomFactor;
        sizeMultiplier = System.Math.Max(zoomMinimum, sizeMultiplier);
        sizeMultiplier = System.Math.Min(zoomMaximum, sizeMultiplier);
    }
}
