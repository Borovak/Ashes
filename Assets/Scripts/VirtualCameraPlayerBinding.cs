using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraPlayerBinding : MonoBehaviour
{

    private CinemachineVirtualCamera _virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_virtualCamera.Follow == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _virtualCamera.Follow = player.transform;
            }
        }
    }
}
