using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Static;
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
    private bool _zoomIn;
    private bool _zoomOut;

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

        if (_zoomIn)
        {
            Zoom(true);
        } else if (_zoomOut)
        {
            Zoom(false);
        }
    }

    void OnEnable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.LT].StateChanged += ZoomOutAffected;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.RT].StateChanged += ZoomInAffected;
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
        ControllerInputs.controllerButtons[Constants.ControllerButtons.LT].StateChanged -= ZoomOutAffected;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.RT].StateChanged -= ZoomInAffected;
    }

    private void ZoomInAffected(bool state)
    {
        _zoomIn = state;
    }

    private void ZoomOutAffected(bool state)
    {
        _zoomOut = state;
    }

    private void Zoom(bool direction)
    {
        var speed = zoomSpeed * Time.deltaTime + 1f;
        zoomFactor = direction ? speed : 1f / speed;
        sizeMultiplier *= zoomFactor;
        sizeMultiplier = Math.Max(zoomMinimum, sizeMultiplier);
        sizeMultiplier = Math.Min(zoomMaximum, sizeMultiplier);
    }
}
