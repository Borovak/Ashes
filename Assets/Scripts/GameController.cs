using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {
        TransitionIn,
        Paused,
        Running,
        ActionMenu,
        TransitionOut,
    }

    public static bool init;
    public static bool loadedAlone;
    public static ChamberController currentChamber = null;
    public GameObject playerPrefab;
    public GameStates gameState;

    private FadeInOutController _fadeInOutController;
    private CinemachineVirtualCamera _virtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        _fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
        _fadeInOutController.FadeOutCompleted += SpawnPlayer;
        if (!init)
        {
            init = true;
            LocationInformation.Init();
        }
        //Loading default save if none loaded
        if (SaveData.workingData == null)
        {
            var loadSuccessful = SaveSystem.Load(out _, out var errorMessage);
            if (!loadSuccessful)
            {
                Debug.Log(errorMessage);
                var saveSuccess = SaveSystem.Save(out var saveErrorMessage);
                Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
            }
        }
        DropController.Init();
    }

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        SaveData.workingData.Hp = SaveData.workingData.MaxHp;
        SaveData.workingData.Mp = SaveData.workingData.MaxMp; 
        var playerSpawnPosition = Vector3.zero;
        if (SaveData.workingData.SavePointGuid != string.Empty)
        {   
            foreach (var savePointGameObject in GameObject.FindGameObjectsWithTag("SavePoint"))
            {
                var savePointController = savePointGameObject.GetComponent<SavePointController>();
                if (savePointController.guid != SaveData.workingData.SavePointGuid) continue;
                playerSpawnPosition = savePointGameObject.transform.position;
                break;
            }
        }
        if (playerSpawnPosition == Vector3.zero)
        {
            playerSpawnPosition = new Vector3(163f, 78f, 0f);
        }
        var playerTransform = GameObject.Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity).transform;
        Debug.Log("Player spawned");
        _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.Follow = playerTransform;
        _virtualCamera.LookAt = playerTransform;
        _fadeInOutController.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != GameStates.Paused)
        {
            SaveData.workingData.GameTime += Time.deltaTime;
        }
    }

}
