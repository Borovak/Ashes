using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraPlayerBinding : MonoBehaviour
{

    public bool isPlayerInsideChamber;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineConfiner _confiner;

    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confiner = GetComponent<CinemachineConfiner>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerInsideChamber = _virtualCamera.enabled = IsPlayerInsideChamber();
        if (_virtualCamera.Follow == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _virtualCamera.Follow = player.transform;
            }
        }
    }

    private bool IsPlayerInsideChamber(){
        if (PlayerPlatformerController.Instance == null) return false;
        var position = PlayerPlatformerController.Instance.transform.position;
        return _confiner.m_BoundingShape2D.OverlapPoint(new Vector2(position.x, position.y));
    }
}
